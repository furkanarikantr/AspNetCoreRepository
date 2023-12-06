using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;

namespace CrudExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<ICountriesService, CountriesService>();
            builder.Services.AddSingleton<IPersonService, PersonService>();
            builder.Services.AddDbContext<PersonsDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
            });
              
            var app = builder.Build();

            if (builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
    /*
        xUnit, .Net için ücretsiz ve açýk kaynaklý birim test aracýdýr.
    xUnit test odaklý bir geliþtirme (TDD) yaklaþýmýný destekler. Asp.Net Core uygulamalrýný tesk etmek için kullanýlabilir. Uygulamamýzý 
    test etmek ve test sonuçlarýný izlemek kodlarýmýzýn güvenliðini arttýrýr ve hatasýz bir þekilde sürdürülmesine yardýmcý olur.

        Test sýnýflarý "[Fact]" attribute'una sahiptir. "[Fact]" ile iþaretlenenlerin birim test olduðu anlaþýlýr. Her birim testin üç adýmý
    vardýr.
        - Arrange   (Hazýrlamak)
            Deðiþkenler ve girdiler ayarlanýr.
        - Act       (Eyleme Geçmek)
            Test edilmek istenen metot çaðýrýlýr.
        - Asset     (Karþýlaþtýrma)
            Beklenen ve test edilen iþlemin sonucu karþýlaþtýrýlýr. 
    */
}
