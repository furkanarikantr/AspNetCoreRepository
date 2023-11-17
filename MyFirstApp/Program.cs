/*namespace 
    C# da bir isim alaný tanýmlanýr. Bir projede içindeki sýnýflarýn ve diðer kullanýlan türlerin gruplanmasýný saðlar. 
    Proje adýmýz : MyFirstApp
*/
using Microsoft.Extensions.Primitives;

namespace MyFirstApp
{
    /*public Class Program
         Program adýmda bir C# class'ý tanýmlanýr. Asp.Net Core uygulamamýzý ayaða kaldýran ve çalýþtýran giriþ noktasýdýr.
    */
    public class Program
    {
        /*public static void Main(string[] args)
            C# programýnýnýn baþlangýç noktasýný belirler. Main metodu uygulama baþlandýðýnda otomatik olarak çaðýrýlýr.
            Bu metoddaki "args" ifadesi, uygulamanýn komut satýr argümanlarýný içerir. Bu argümanlar uygulamanýn çalýþma mantýðýný belirlemek
                için kullanýlýr.
        */
        public static void Main(string[] args)
        {
            /*var builder = WebApplication.CreateBuilder(args)
                Bu satýr "WebApplication" sýnýfýný kullanarak bir uygulama oluþturur. Bu sýnýf Asp.Net Core uygulamasýndan gelir, Asp.Net Core'un
                    bir parçasýdýr. Bu sýnýf ayný zamanda Asp.Net Core uygulamalarýnýn baþlatýlmasýný ve yapýlandýrýlmasýný saðlayan bir Api'dir.
                "CreateBuilder" metodu ise, uygulamanýn yapýlandýrmasýný baþlatýr ve kullanýcý tarafýndan saðlanan komut satýrý argümanlarý
                    kullanýr.
            */
            var builder = WebApplication.CreateBuilder(args);

            /*var app = builder.Build()
                Üstteki kod satýrý veya satýrlarý ile yapýlandýrdýðýmýz uygulamayý tamamlar. 
                ".Build()" metodu, uygulamanýn üstteki yapýlandýrmalarýný tamamlar ve uygulamayý baþlatmaya hazýr hale getirir.
            */
            var app = builder.Build();

            /*app.MapGet("/", () => "Hello World!")
                Bu kod, kök URL olan "/" yolunu, HttpGet isteði yapýldýðýnda, "Hello World!" yanýtýný döndürür.
                ".MapGet(...) metodu, belirtilen URL yollarýna, Get isteklerini iþlemek için kullanýlýr.

                Detayýna inersek, app.Map iþlevleri, web uygulamamýza yapýlan HTTP isteklerini iþlemek ve yönlendirmek için kullanabileceðimiz
                    bir dizi iþlevlerdir. 

                app.MapGet/app.MapPost/app.MapPut/app.MapDelete/app.MapMethods/app.MapWhen/app.MapFallBack en yaygýnlarýdýr.
                    app.MapGet => HTTP Get isteði için URL yolu belirler ve bir yanýt döndürür.
                    app.MapPost => HTTP Post isteði için URL yolu belirler ve bir yanýt döndürür.
                    app.MapPut => HTTP Get isteði için URL yolu belirler ve bir yanýt döndürür.
                    app.MapDelete => HTTP Get isteði için URL yolu belirler ve bir yanýt döndürür.
                    app.MapMethods => HTTP Get isteði için URL yolu belirler ve bir yanýt döndürür.
                    app.MapWhen => Belirli bir koþula dayalý olarak HTTP isteðini iþler, koþul saðlanýrsa iþlem yapýlýr.
                    app.MapMethods => Bir eþleþme yapýlmadýðý zaman bu yol çalýþýr.
            */
            //app.MapGet("/", () => "Hello World!");

            app.Run(async (HttpContext context) =>
            {
                //context.Response.Headers["MyKey"] = "my value";

                //string path = context.Request.Path;
                //string method = context.Request.Method;
                //context.Response.Headers["Content-Type"] = "text/html";
                //await context.Response.WriteAsync($"<p>{path}</p>");
                //await context.Response.WriteAsync($"<p>{method}</p>");

                //if (context.Request.Method == "GET")
                //{
                //if (context.Request.Query.ContainsKey("id"))
                //{
                //string id = context.Request.Query["id"];
                //await context.Response.WriteAsync($"<p>{id}</p>");
                //}
                //}

                //await context.Response.WriteAsync("Hello");
                //await context.Response.WriteAsync("World!");

                System.IO.StreamReader reader = new System.IO.StreamReader(context.Request.Body);
                string body = await reader.ReadToEndAsync();
                Dictionary<string, StringValues> queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);
                if (queryDictionary.ContainsKey("firstName"))
                {
                    string firstName = queryDictionary["firstName"][0];
                    await context.Response.WriteAsync(firstName);
                }
            });

            /*app.Run()
                Yapýlandýrýlmasý tamamlanmýþ ve hazýr hale gelmiþ uygulamamýzý baþlatmamýzý saðlar. 
            */
            app.Run();
        }
    }
}
/* GENEL ÖZET
    Oluþturduðumuz web uygulamasý çalýþtýrýlmaya baþlandýðýnda Program.Cs dosyasýna gelir.
    Burada WebApplication.CreateBuildger(args) komutu ile örnek ve baþlangýç web uygulamasýnýn örneðini alýr.
    Daha sonra biz eklemek istediðimiz yapýlandýrmalarýný tanýmlarýz.
    Akabinde uygulamamýz ayaða kalmak için hazýr hale getirilir.
    Uygulama hazýr hale getirildek sonra Middleware'lere baþvurulur. Middleware ara katman demektir.
    Middleware iþlevlerini belirttikten sonra app.Run() diyerek hazýr haldeki uygulamamýzý baþlatýrýz.
*/




