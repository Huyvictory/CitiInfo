using AutoMapper;
using CitiInfo.WebAPI.Models;
using CitiInfo.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CitiInfo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
            string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }
            var (cityEntties, paginationMetaData) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetaData));

            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntties));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCityById(int id, bool includePointsOfInterset = false)
        {
            var cityDetails = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterset);

            if (cityDetails == null)
            {
                return NotFound();
            }

            if (includePointsOfInterset)
            {
                return Ok(_mapper.Map<CityDto>(cityDetails));
            }

            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(cityDetails));
        }
    }
}
