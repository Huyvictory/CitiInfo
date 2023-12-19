namespace CitiInfo.WebAPI.Models
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto() {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with that big park.",
                    PointsOfInterest = new List<PointOfInterestDto>() {
                        new PointOfInterestDto() {Id = 1, Name = "Central Park", Description = "description"},
                        new PointOfInterestDto() {Id = 2, Name = "Central Park", Description = "description"},
                } },
                new CityDto() {
                    Id = 2,
                    Name = "Ho Chi Minh",
                    Description = "The one with busy atmosphere",
                    PointsOfInterest = new List<PointOfInterestDto>() {
                        new PointOfInterestDto() {Id = 3, Name = "Central Park", Description = "description"},
                        new PointOfInterestDto() {Id = 4, Name = "Central Park", Description = "description"},
                } },
                new CityDto() {
                    Id = 3,
                    Name = "Paris",
                    Description = "The one with that big tower.",
                    PointsOfInterest = new List<PointOfInterestDto>() {
                        new PointOfInterestDto() {Id = 5, Name = "Central Park", Description = "description"},
                        new PointOfInterestDto() {Id = 6, Name = "Central Park", Description = "description"},
                }}
            };
        }
    }
}
