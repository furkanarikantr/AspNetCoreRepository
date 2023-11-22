using Configuration.AppSettingModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Configuration.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IConfiguration _configuration;
        private readonly WeatherApiOptions _weatherApiOptions;

        public HomeController(IOptions<WeatherApiOptions> weatherApiOptions/*,IConfiguration configuration*/)
        {
            //_configuration = configuration;
            _weatherApiOptions = weatherApiOptions.Value;
        }

        [Route("/")]
        public IActionResult Index()
        {
            //--------------------
            //ViewBag.MyKey = _configuration["MyKey"];
            //ViewBag.MyAPIKey = _configuration.GetValue("MyAPIKey","The default key!");
            //--------------------

            //--------------------
            //ViewBag.ClientId = _configuration["weatherapi:ClientId"];
            //ViewBag.ClientSecret = _configuration.GetValue("weatherapi:ClientSecret", "the default key");
            //--------------------

            //--------------------
            /*
            IConfigurationSection weatherapiSection = _configuration.GetSection("weatherapi");
            ViewBag.ClientId = weatherapiSection["ClientId"];
            ViewBag.ClientSecret = weatherapiSection["ClientSecret"];
            */
            //--------------------

            //--------------------
            //Options Pattern - 1
            //WeatherApiOptions weatherApiOptions = _configuration.GetSection("weatherapi").Get<WeatherApiOptions>();

            //Options Pattern - 2 => Bind
            //WeatherApiOptions weatherApiOptions = new WeatherApiOptions();
            //_configuration.GetSection("weatherapi").Bind(weatherApiOptions);

            //ViewBag.ClientId = weatherApiOptions.ClientId;
            //ViewBag.ClientSecret = weatherApiOptions.ClientSecret;
            //--------------------


            ViewBag.ClientId = _weatherApiOptions.ClientId;
            ViewBag.ClientSecret = _weatherApiOptions.ClientSecret;

            return View();
        }
    }
}
