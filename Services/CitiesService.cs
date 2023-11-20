using ServiceContracts;

namespace Services
{
    public class CitiesService : ICitiesService
    {
        private List<string> _cities;

        public CitiesService()
        {
            _cities = new List<string>()
            {
                "London",
                "Paris",
                "Barcelona",
                "Madrid",
                "Munich",
                "Berlin",
                "Amsterdam",
                "Vienna",
                "Manchester",
                "Stockholm"
            };
        }

        public List<string> GetCities()
        {
            return _cities;
        }
    }
}