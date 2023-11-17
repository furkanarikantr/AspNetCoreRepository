using Microsoft.AspNetCore.Http;
using System.Diagnostics.Metrics;

namespace RoutingExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            //Veriler
            Dictionary<int, string> countries = new Dictionary<int, string>()
            {
                { 1, "United States" },
                { 2, "Canada" },
                { 3, "United Kingdom" },
                { 4, "India" },
                { 5, "Japan" }
            };

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //Açýlýþ Sayfasý
                endpoints.Map("/", async (context) =>
                {
                    await context.Response.WriteAsync("Acilis Sayfasi!");
                });

                //Ülkelerin Listelenmesi
                endpoints.MapGet("countries", async (context) =>
                {
                    foreach (var country in countries)
                    {
                        await context.Response.WriteAsync($"Ulke Id : {country.Key} - Ulke Adý : {country.Value}\n");
                    }
                });

                //Id'ye Göre Ülke Getirilmesi
                endpoints.MapGet("/countries/{countryID:int:range(1,100)}", async (context) =>
                {
                    int? countryId = Convert.ToInt32(context.Request.RouteValues["countryId"]);
                    bool hasCountry = false;
                    foreach (var country in countries)
                    {
                        if (country.Key == countryId)
                        {
                            await context.Response.WriteAsync($"{country.Value}");
                            hasCountry = true;
                        }
                    }

                    if (hasCountry == false)
                    {
                        await context.Response.WriteAsync("Girilen Id'ye Gore Ulke Yok!");
                    }
                });

                endpoints.MapGet("/countries/{countryID:min(101)}", async context =>
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Ulke Id'si 1 ile 100 arasinda olmali!");
                });
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Url Dogru Degil!");
            });

            app.Run();
        }
    }
}