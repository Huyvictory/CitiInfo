using Asp.Versioning;
using AutoMapper;
using CitiInfo.WebAPI.Models;
using CitiInfo.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CitiInfo.WebAPI.Controllers
{
    [Route("api/city/{cityId}/pointsofinterest")]
    [Authorize(Policy = "MustBeFromHoChiMinh")]
    [ApiVersion("2.0")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _localMailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(
            ILogger<PointsOfInterestController> logger,
            IMailService localMailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _localMailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterestCity(int cityId)
        {
            var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;

            if (!(await _cityInfoRepository.CityNameMatchesCityId(cityName, cityId)))
            {
                return Forbid();
            }

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found");
                return NotFound();
            }

            var pointsOfInterestCity = await _cityInfoRepository.GetPointOfInterestsForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestCity));
        }

        [HttpGet("{pointOfInterestId}", Name = "GetDetailsOfPointInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetDetailsOfPointInterest(int cityId, int pointOfInterestId)
        {
            var city = await _cityInfoRepository.CityExistsAsync(cityId);

            if (!city)
            {
                return NotFound();
            }

            var pointInterest = await _cityInfoRepository.GetPointOfInterestDetailsForCityAsync(cityId, pointOfInterestId);

            if (pointInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointInterest));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
            int cityId,
            PointofInterestForCreatingDto pointofInterestForCreatingDto)
        {

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var newPointofInterest = _mapper.Map<Entities.PointOfInterest>(pointofInterestForCreatingDto);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, newPointofInterest);

            await _cityInfoRepository.SaveChangesAsync();

            var newPointOfInterestDTO = _mapper.Map<PointOfInterestDto>(newPointofInterest);

            return CreatedAtRoute("GetDetailsOfPointInterest", new
            {
                cityId = cityId,
                pointofInterestId = newPointOfInterestDTO.Id
            }, newPointOfInterestDTO);
        }

        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(
            int cityId,
            int pointOfInterestId,
            PointOfInterestForUpdateDto pointOfInterestForUpdateDto)
        {

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointInterest = await _cityInfoRepository
                .GetPointOfInterestDetailsForCityAsync(cityId, pointOfInterestId);

            if (pointInterest == null)
            {
                return NotFound();
            }

            _mapper.Map(pointOfInterestForUpdateDto, pointInterest);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PartaillyUpdatePointOfInterest(
            int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointInterest = await _cityInfoRepository.GetPointOfInterestDetailsForCityAsync(cityId, pointOfInterestId);

            if (pointInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointInterest);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointInterest);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointInterest = await _cityInfoRepository.GetPointOfInterestDetailsForCityAsync(cityId, pointOfInterestId);

            if (pointInterest == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterestsForCity(pointInterest);
            await _cityInfoRepository.SaveChangesAsync();

            _localMailService.Send("Point of interest deleted.", $"Point of interest {pointInterest.Name} with id " +
                $"{pointOfInterestId} has been deleted successfully");

            return NoContent();
        }
    }
}
