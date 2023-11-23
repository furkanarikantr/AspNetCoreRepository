using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SocialMediaApplication.AppSettingModels;

namespace SocialMediaApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly SocialMediaLinksOptions _socialMediaLinksOptions;

        public HomeController(IOptions<SocialMediaLinksOptions> socialMediaLinksOptions)
        {
            _socialMediaLinksOptions = socialMediaLinksOptions.Value;
        }

        [Route("/")]
        public IActionResult Index()
        {
            ViewBag.SocialMediaLinks = _socialMediaLinksOptions;
            return View();
        }
    }
}
