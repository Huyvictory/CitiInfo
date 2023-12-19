using CitiInfo.WebAPI.Models;
using CitiInfo.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CitiInfo.WebAPI.Controllers
{
    [Route("api/city/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _localMailService;
        private readonly CitiesDataStore _citiesDataStore;

        public PointsOfInterestController(
            ILogger<PointsOfInterestController> logger,
            IMailService localMailService,
            CitiesDataStore citiesDataStore
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _localMailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterestCity(int cityId)
        {
            try
            {

                var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

                if (city == null)
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when retrieving points of interest.");
                    return NotFound();
                }

                return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "A problem happended while handling your request.");
            }

        }

        [HttpGet("{pointOfInterestId}", Name = "GetDetailsOfPointInterest")]
        public ActionResult<PointOfInterestDto> GetDetailsOfPointInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointInterest == null)
            {
                return NotFound();
            }

            return Ok(pointInterest);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(
            int cityId,
            [FromBody] PointofInterestForCreatingDto pointofInterestForCreatingDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            // 
            var maxNewPointofInterestId = _citiesDataStore.Cities
                .SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

            var newPointofInterest = new PointOfInterestDto()
            {
                Id = ++maxNewPointofInterestId,
                Name = pointofInterestForCreatingDto.Name,
                Description = pointofInterestForCreatingDto.Description,
            };

            city.PointsOfInterest.Add(newPointofInterest);

            return CreatedAtRoute("GetDetailsOfPointInterest", new
            {
                cityId = cityId,
                pointOfInterestId = newPointofInterest.Id
            }, newPointofInterest);
        }

        [HttpPut("{pointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(
            int cityId,
            int pointOfInterestId,
            PointOfInterestForUpdateDto pointOfInterestForUpdateDto)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointInterest == null)
            {
                return NotFound();
            }

            pointInterest.Name = pointOfInterestForUpdateDto.Name;
            pointInterest.Description = pointOfInterestForUpdateDto.Description;

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public ActionResult PartaillyUpdatePointOfInterest(
            int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
            {
                Name = pointInterest.Name,
                Description = pointInterest.Description
            };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            pointInterest.Name = pointOfInterestToPatch.Name;
            pointInterest.Description = pointOfInterestToPatch?.Description;

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointInterest == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointInterest);
            _localMailService.Send("Point of interest deleted.", $"Point of interest {pointInterest.Name} with id " +
                $"{pointOfInterestId} has been deleted successfully");
            return NoContent();
        }
    }
}
