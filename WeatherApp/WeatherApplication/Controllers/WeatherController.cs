using Microsoft.AspNetCore.Mvc;
using WeatherApplication.Models;

namespace WeatherApplication.Controllers
{
    public class WeatherController : Controller
    {
        List<CityWeather> citiesList = new List<CityWeather>()
            {
                new CityWeather
                {
                    CityUniqueCode = "LDN", CityName = "London", DateAndTime = Convert.ToDateTime("2030-01-01 8:00"),  TemperatureFahrenheit = 33
                },
                new CityWeather
                {
                    CityUniqueCode = "NYC", CityName = "NewYork", DateAndTime = Convert.ToDateTime("2030-01-01 3:00"),  TemperatureFahrenheit = 60
                },
                new CityWeather
                {
                    CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = Convert.ToDateTime("2030-01-01 9:00"),  TemperatureFahrenheit = 82
                }
            };

        [Route("/")]
        public IActionResult Index()
        {
            return View(citiesList);
        }

        [Route("/city-detail/{cityCode}")]
        public IActionResult GetCityByCode(string? cityCode)
        {
            if (cityCode == null)
            {
                return View("Index");
            }

            CityWeather? selectedCity = citiesList.Where(temp => temp.CityUniqueCode == cityCode).FirstOrDefault();

            return View(selectedCity);
        }
    }
}
