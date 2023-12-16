using CitiInfo.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CitiInfo.WebAPI.Controllers
{
    [Route("api/city/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterestCity(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{pointOfInterestId}", Name = "GetDetailsOfPointInterest")]
        public ActionResult<PointOfInterestDto> GetDetailsOfPointInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

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

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            // 
            var maxNewPointofInterestId = CitiesDataStore.Current.Cities
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
    }
}
