using ServiceContracts;

namespace Services
{
    public class CitiesService : ICitiesService, IDisposable
    {
        private List<string> _cities;

        private Guid _serviceInstanceId;

        public CitiesService()
        {
            _serviceInstanceId = Guid.NewGuid();

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

            //Database bağlantısı oluşturma işlemleri
        }

        public Guid ServiceInstanceId
        {
            get
            {
                return _serviceInstanceId;
            }
        }

        public void Dispose()
        {
            //Veritabanı bağlantısı kapatma işlemleri
        }

        public List<string> GetCities()
        {
            return _cities;
        }
    }
}