using ServiceContracts;
using Services;

namespace DependencyInjection
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.Add(new ServiceDescriptor(
                typeof(ICitiesService),
                typeof(CitiesService),
                ServiceLifetime.Transient
            ));

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
}