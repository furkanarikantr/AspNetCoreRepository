using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceContracts;
using Services;

namespace DependencyInjection.Controllers
{
    public class HomeController : Controller
    {
        //private readonly CitiesService _citiesService;
        private readonly ICitiesService _citiesService1;
        private readonly ICitiesService _citiesService2;
        private readonly ICitiesService _citiesService3;

        //private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILifetimeScope _lifeTimeScope;

        public HomeController(ICitiesService citiesService1, ICitiesService citiesService2, ICitiesService citiesService3, ILifetimeScope lifeTimeScope/*IServiceScopeFactory serviceScopeFactory*/)
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

            _citiesService1 = citiesService1; //Object from IoC container.
            _citiesService2 = citiesService2;
            _citiesService3 = citiesService3;
            _lifeTimeScope = lifeTimeScope;

            /*
                Transient => Aynı servisin birden çok kullanımı durumu, aynı servisten kullanılan kadar üretir.
                Scoped => Aynı servisi birden çok kullansak bile, aynı controller içinde olduğumuz için tek servis üretir ve o controller içinde
                    üretilen tek servis kullanılır.
                Singleton => Aynı servisi birden çok kullansak bile, uygulama çalışır çalışmaz o servisten bir tane üretilir ve uygulama çalışma
                    ömrü boyunca o servis kullanılr.
                

                ** Hangisi ne zaman kullanılmalı ? Tam olarak anlaşılmadı, bakılacak!
                
                ** Üretilen bütün servisler IoC Container'da saklanır ve ihtiyaç duyulduğunda oradan alınır.
            */
        }

        [Route("/")]
        public IActionResult Index()
        {
            List<string> cities = _citiesService1.GetCities();

            ViewBag.InstanceId_CitiesService_1 = _citiesService1.ServiceInstanceId;
            ViewBag.InstanceId_CitiesService_2 = _citiesService2.ServiceInstanceId;
            ViewBag.InstanceId_CitiesService_3 = _citiesService3.ServiceInstanceId;

            //using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            using (ILifetimeScope lifeTimeScope = _lifeTimeScope.BeginLifetimeScope())
            {
                //ICitiesService citiesService = serviceScope.ServiceProvider.GetRequiredService<ICitiesService>();
                ICitiesService citiesService = lifeTimeScope.Resolve<ICitiesService>();
                ViewBag.InstanceId_CitiesService_InScope = citiesService.ServiceInstanceId;
            }//Using sonunda servisteki Dispose otomatik olarak çağırılır.

            return View(cities);
        }
    }
}