using Routing.CustomConstraints;

namespace Routing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRouting(options =>
            {
                options.ConstraintMap.Add("months", typeof(MonthsCustomConstraints));
            });

            var app = builder.Build();

            //Route'larý kullanýma açtýðýmýz yer.
            app.UseRouting();

            //GetEndpoint() - Endpoint bilgilerini aldýðýmýz yer.
            //app.Use(async (context, next) =>
            //{
            //Microsoft.AspNetCore.Http.Endpoint? endPoint = context.GetEndpoint();
            //if (endPoint != null)
            //{
            //await context.Response.WriteAsync($"EndPoint : {endPoint.DisplayName}\n");
            //}
            //await next(context);
            //});

            //Endpoint'leri oluþturduðumuz yer.
            app.UseEndpoints(endpoints =>
            {
                //Endpoint'leri belirttiðimiz yer.
                //endpoints.MapGet("map1",async (context) => {
                //	await context.Response.WriteAsync("In Map 1");
                //});
                //endpoints.MapPost("map2", async (context) => {
                //	await context.Response.WriteAsync("In Map 2");
                //});

                //Route Parametresi
                endpoints.Map("files/{filename}.{extension}", async (context) =>
                {
                    string? fileName = Convert.ToString(context.Request.RouteValues["filename"]);
                    string? extension = Convert.ToString(context.Request.RouteValues["extension"]);
                    await context.Response.WriteAsync($"In files - {fileName} - {extension}");
                });
                //Route Parametresi
                //endpoints.Map("employee/profile/{employeename}", async (context) =>
                //{
                //  string? empyloyeeName = Convert.ToString(context.Request.RouteValues["employeename"]);
                //  await context.Response.WriteAsync($"Employee Name = {empyloyeeName}");
                //});

                //Default Route Parametresi
                endpoints.Map("employee/profile/{employeename=isimsiz}", async (context) =>
                {
                    string? empyloyeeName = Convert.ToString(context.Request.RouteValues["employeename"]);
                    await context.Response.WriteAsync($"Employee Name = {empyloyeeName}");
                });

                //Optimal Route Parametresi
                endpoints.Map("products/details/{id:int?}", async (context) =>
                {
                    if (context.Request.RouteValues.ContainsKey("id") == true)
                    {
                        int? id = Convert.ToInt32(context.Request.RouteValues["id"]);
                        await context.Response.WriteAsync($"Producta Details = {id}");
                    }
                    else
                    {
                        await context.Response.WriteAsync("Producta Details = id is not supplied");
                    }
                });

                //route constraints - date time
                endpoints.Map("daily-digest-report/{reportdate:datetime}", async (context) =>
                {
                    DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
                    await context.Response.WriteAsync($"Report Date : {reportDate.ToShortDateString()}");
                    ///daily-digest-report/2023-11-29
                });

                //salees-report/2030/apr
                //endpoints.Map("sales-report/{year:int:min(1900)}/{month:regex(^(apr|jul|oct|jan)$)}", async (context) =>
                endpoints.Map("sales-report/{year:int:min(1900)}/{month:months}", async (context) =>
                {
                    int year = Convert.ToInt32(context.Request.RouteValues["year"]);
                    string? month = Convert.ToString(context.Request.RouteValues["month"]);

                    if (month == "apr" || month == "jul" || month == "oct" || month == "jan")
                    {
                        await context.Response.WriteAsync($"Sales Report - {year} - {month}");
                    }
                });
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync($"Request received at {context.Request.Path}");
            });

            app.Run();
        }
    }
}