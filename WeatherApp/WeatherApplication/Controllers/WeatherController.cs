using Microsoft.AspNetCore.Mvc;
using Models;
using ServiceContracts;

namespace WeatherApplication.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [Route("/")]
        public IActionResult Index()
        {
            List<CityWeather> citiesList = _weatherService.GetWeatherDetails();
            return View(citiesList);
        }

        [Route("/city-detail/{cityCode}")]
        public IActionResult GetCityByCode(string? cityCode)
        {
            if (cityCode == null)
            {
                return View("Index");
            }

            List<CityWeather> citiesList = _weatherService.GetWeatherDetails();
            CityWeather? selectedCity = citiesList.Where(temp => temp.CityUniqueCode == cityCode).FirstOrDefault();

            return View(selectedCity);
        }
    }
}
