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
            //XML veri formatýný etkinleþtirmek için kullanýlýr.
            builder.Services.AddControllers().AddXmlDataContractSerializerFormatters();

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
}