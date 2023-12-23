using CitiInfo.WebAPI.Entities;

namespace CitiInfo.WebAPI.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<(IEnumerable<City>, PaginationMetaData)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
        Task<bool> CityExistsAsync(int cityId);
        Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestDetailsForCityAsync(int cityId, int pointOfInterestId);
        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
        void DeletePointOfInterestsForCity(PointOfInterest pointOfInterest);
        Task<bool> SaveChangesAsync();
    }
}
