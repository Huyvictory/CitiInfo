﻿using AutoMapper;
using CitiInfo.WebAPI.Models;
using CitiInfo.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitiInfo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
        {
            var cityEntties = await _cityInfoRepository.GetCitiesAsync();

            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntties));
        }
        //[HttpGet("{id}")]
        //public ActionResult<CityDto> GetCityById(int id)
        //{
        //    //var cityToReturn = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);
        //    //if (cityToReturn == null) { return NotFound(); }
        //    //return Ok(cityToReturn);
        //}
    }
}
