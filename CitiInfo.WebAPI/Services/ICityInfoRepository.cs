using CitiInfo.WebAPI.Entities;

namespace CitiInfo.WebAPI.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
        Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestDetailsForCityAsync(int cityId, int pointOfInterestId);
    }
}
