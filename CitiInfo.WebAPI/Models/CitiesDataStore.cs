namespace CitiInfo.WebAPI.Models
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto() { Id = 1, Name = "New York City", Description = "The one with that big park."},
                new CityDto() { Id = 2, Name = "Ho Chi Minh", Description = "The one with busy atmosphere"},
                new CityDto() { Id = 3, Name = "Paris", Description = "The one with that big tower."}
            };
        }
    }
}
