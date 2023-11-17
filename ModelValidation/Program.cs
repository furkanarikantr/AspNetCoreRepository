using ModelValidations.CustomModelBinders;

namespace ModelValidations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new PersonBinderProvider());
            });
            //XML veri format�n� etkinle�tirmek i�in kullan�l�r.
            builder.Services.AddControllers().AddXmlDataContractSerializerFormatters();

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
}