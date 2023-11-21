using Microsoft.AspNetCore.Mvc;
using Models;

namespace WeatherApplication.ViewComponents
{
    public class CityViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CityWeather city)
        {
            ViewBag.CityCssClass = GetCssByFahrenheit(city.TemperatureFahrenheit);

            return View(city); //invokes view of the view component at Views/Shared/Components/Grid/Sample.cshtml
        }

        string GetCssByFahrenheit(int? TemperatureFahrenheit)
        {
            return TemperatureFahrenheit switch
            {
                (< 44) => "blue-back",
                (>= 44) and (< 75) => "green-back",
                (>= 75) => "orange-back"
            };
        }
    }
}
