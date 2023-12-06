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
        xUnit, .Net i�in �cretsiz ve a��k kaynakl� birim test arac�d�r.
    xUnit test odakl� bir geli�tirme (TDD) yakla��m�n� destekler. Asp.Net Core uygulamalr�n� tesk etmek i�in kullan�labilir. Uygulamam�z� 
    test etmek ve test sonu�lar�n� izlemek kodlar�m�z�n g�venli�ini artt�r�r ve hatas�z bir �ekilde s�rd�r�lmesine yard�mc� olur.

        Test s�n�flar� "[Fact]" attribute'una sahiptir. "[Fact]" ile i�aretlenenlerin birim test oldu�u anla��l�r. Her birim testin �� ad�m�
    vard�r.
        - Arrange   (Haz�rlamak)
            De�i�kenler ve girdiler ayarlan�r.
        - Act       (Eyleme Ge�mek)
            Test edilmek istenen metot �a��r�l�r.
        - Asset     (Kar��la�t�rma)
            Beklenen ve test edilen i�lemin sonucu kar��la�t�r�l�r. 
    */
}
