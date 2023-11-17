using Controllers.Controllers;

namespace Controllers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Services Collection'�n�na AddControllers diyerek Controller'lar�n� buraya ekler ve ihtiyac� oldu�unda kullan�r.
            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseStaticFiles();
            app.UseRouting();

            app.MapControllers();

            app.Run();
        }
    }
}