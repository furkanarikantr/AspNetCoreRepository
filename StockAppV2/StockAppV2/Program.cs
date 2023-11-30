using ServiceContracts;
using Services;

namespace StockAppV2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IFinnhubService, FinnhubService>();
            builder.Services.AddSingleton<IStocksService, StocksService>();
            builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
            builder.Services.AddHttpClient();
            
            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
}