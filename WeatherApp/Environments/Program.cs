namespace Environments
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            
            var app = builder.Build();

            if(app.Environment.IsDevelopment()){ //app.Environment.IsEnvironment("Beta")
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();            

            app.Run();
        }
    }
}