using CitiInfo.WebAPI.DbContexts;
using CitiInfo.WebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CitiInfo.WebAPI.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _cityInfoContext;

        public CityInfoRepository(CityInfoContext cityInfoContext)
        {
            _cityInfoContext = cityInfoContext ?? throw new ArgumentNullException(nameof(cityInfoContext));
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);

            if (city != null)
            {
                city.PointsOfInterest.Add(pointOfInterest);
            }
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _cityInfoContext.Cities.AnyAsync(c => c.Id == cityId);
        }

        public void DeletePointOfInterestsForCity(PointOfInterest pointOfInterest)
        {
            _cityInfoContext.PointsOfInterest.Remove(pointOfInterest);
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _cityInfoContext.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<(IEnumerable<City>, PaginationMetaData)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
        {

            // collection to start from
            var collection = _cityInfoContext.Cities as IQueryable<City>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                // filter by exact city name
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(c => c.Name.Contains(searchQuery)
                || (c.Description != null && c.Description.Contains(searchQuery)));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetaData = new PaginationMetaData(totalItemCount, pageSize, pageNumber);

            var collectionCitiesReturn = await collection
                .OrderBy(c => c.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionCitiesReturn, paginationMetaData);
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return await _cityInfoContext.Cities
                    .Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }

            return await _cityInfoContext.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<PointOfInterest?> GetPointOfInterestDetailsForCityAsync(int cityId, int pointOfInterestId)
        {
            return await _cityInfoContext.PointsOfInterest
                .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityId)
        {
            return await _cityInfoContext.PointsOfInterest.Where(p => p.CityId == cityId).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _cityInfoContext.SaveChangesAsync() >= 0);
        }
    }
}
