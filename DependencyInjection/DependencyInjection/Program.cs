using Autofac;
using Autofac.Extensions.DependencyInjection;
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

            /*
            builder.Services.Add(new ServiceDescriptor(
                typeof(ICitiesService),
                typeof(CitiesService),
                //ServiceLifetime.Transient
                //ServiceLifetime.Singleton
                ServiceLifetime.Scoped
            ));
            */

            //builder.Services.AddTransient<ICitiesService, CitiesService>();
            //builder.Services.AddSingleton<ICitiesService, CitiesService>();
            //builder.Services.AddScoped<ICitiesService, CitiesService>();

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                //containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().InstancePerDependency();    //AddTransient
                //containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().SingleInstance();           //AddSingleton
                containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().InstancePerLifetimeScope(); //AddScope
            });

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
}