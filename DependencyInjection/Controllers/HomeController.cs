using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;

namespace DependencyInjection.Controllers
{
    public class HomeController : Controller
    {
        //private readonly CitiesService _citiesService;
        private readonly ICitiesService _citiesService;

        public HomeController(ICitiesService citiesService)
        {
            /*
            _citiesService = new CitiesService();
            
                13. satırdaki ifade sıkıntılı bir ifadedir.
                
                İlk olarak bu ifade yeni bir sınıf oluşturmaktadır. Bu yüzden controller çalışmadan önce bu sınıf oluşturulmalıdır. 
                    Eğer bu sınıf oluşturulamaz ise, controller'da oluşturulamaz, bu durum controller'ı oluşturulacak sınıfa bağımlı yapar.
                
                İkinci olarak ileride bu servis sınıfını değilde başka bir servis sınıfını kullanmak istersek, bu sınıfları değiştirmemiz 
                    ya da silmemiz gerekecek. Bu durum ise bize sürdürülebilirlik açısından sıkıntı çıkaracak.

                Bu sorunların çözümü, Dependency Inversion Principle (DIP)
            */

            _citiesService = citiesService;
        }

        [Route("/")]
        public IActionResult Index()
        {
            List<string> cities = _citiesService.GetCities();
            return View(cities);
        }
    }
}
