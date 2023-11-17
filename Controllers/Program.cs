using Controllers.Controllers;

namespace Controllers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Services Collection'ýnýna AddControllers diyerek Controller'larýný buraya ekler ve ihtiyacý olduðunda kullanýr.
            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseStaticFiles();
            app.UseRouting();

            app.MapControllers();

            app.Run();
        }
    }
}