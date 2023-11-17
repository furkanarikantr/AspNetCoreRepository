/*namespace 
    C# da bir isim alan� tan�mlan�r. Bir projede i�indeki s�n�flar�n ve di�er kullan�lan t�rlerin gruplanmas�n� sa�lar. 
    Proje ad�m�z : MyFirstApp
*/
using Microsoft.Extensions.Primitives;

namespace MyFirstApp
{
    /*public Class Program
         Program ad�mda bir C# class'� tan�mlan�r. Asp.Net Core uygulamam�z� aya�a kald�ran ve �al��t�ran giri� noktas�d�r.
    */
    public class Program
    {
        /*public static void Main(string[] args)
            C# program�n�n�n ba�lang�� noktas�n� belirler. Main metodu uygulama ba�land���nda otomatik olarak �a��r�l�r.
            Bu metoddaki "args" ifadesi, uygulaman�n komut sat�r arg�manlar�n� i�erir. Bu arg�manlar uygulaman�n �al��ma mant���n� belirlemek
                i�in kullan�l�r.
        */
        public static void Main(string[] args)
        {
            /*var builder = WebApplication.CreateBuilder(args)
                Bu sat�r "WebApplication" s�n�f�n� kullanarak bir uygulama olu�turur. Bu s�n�f Asp.Net Core uygulamas�ndan gelir, Asp.Net Core'un
                    bir par�as�d�r. Bu s�n�f ayn� zamanda Asp.Net Core uygulamalar�n�n ba�lat�lmas�n� ve yap�land�r�lmas�n� sa�layan bir Api'dir.
                "CreateBuilder" metodu ise, uygulaman�n yap�land�rmas�n� ba�lat�r ve kullan�c� taraf�ndan sa�lanan komut sat�r� arg�manlar�
                    kullan�r.
            */
            var builder = WebApplication.CreateBuilder(args);

            /*var app = builder.Build()
                �stteki kod sat�r� veya sat�rlar� ile yap�land�rd���m�z uygulamay� tamamlar. 
                ".Build()" metodu, uygulaman�n �stteki yap�land�rmalar�n� tamamlar ve uygulamay� ba�latmaya haz�r hale getirir.
            */
            var app = builder.Build();

            /*app.MapGet("/", () => "Hello World!")
                Bu kod, k�k URL olan "/" yolunu, HttpGet iste�i yap�ld���nda, "Hello World!" yan�t�n� d�nd�r�r.
                ".MapGet(...) metodu, belirtilen URL yollar�na, Get isteklerini i�lemek i�in kullan�l�r.

                Detay�na inersek, app.Map i�levleri, web uygulamam�za yap�lan HTTP isteklerini i�lemek ve y�nlendirmek i�in kullanabilece�imiz
                    bir dizi i�levlerdir. 

                app.MapGet/app.MapPost/app.MapPut/app.MapDelete/app.MapMethods/app.MapWhen/app.MapFallBack en yayg�nlar�d�r.
                    app.MapGet => HTTP Get iste�i i�in URL yolu belirler ve bir yan�t d�nd�r�r.
                    app.MapPost => HTTP Post iste�i i�in URL yolu belirler ve bir yan�t d�nd�r�r.
                    app.MapPut => HTTP Get iste�i i�in URL yolu belirler ve bir yan�t d�nd�r�r.
                    app.MapDelete => HTTP Get iste�i i�in URL yolu belirler ve bir yan�t d�nd�r�r.
                    app.MapMethods => HTTP Get iste�i i�in URL yolu belirler ve bir yan�t d�nd�r�r.
                    app.MapWhen => Belirli bir ko�ula dayal� olarak HTTP iste�ini i�ler, ko�ul sa�lan�rsa i�lem yap�l�r.
                    app.MapMethods => Bir e�le�me yap�lmad��� zaman bu yol �al���r.
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
                Yap�land�r�lmas� tamamlanm�� ve haz�r hale gelmi� uygulamam�z� ba�latmam�z� sa�lar. 
            */
            app.Run();
        }
    }
}
/* GENEL �ZET
    Olu�turdu�umuz web uygulamas� �al��t�r�lmaya ba�land���nda Program.Cs dosyas�na gelir.
    Burada WebApplication.CreateBuildger(args) komutu ile �rnek ve ba�lang�� web uygulamas�n�n �rne�ini al�r.
    Daha sonra biz eklemek istedi�imiz yap�land�rmalar�n� tan�mlar�z.
    Akabinde uygulamam�z aya�a kalmak i�in haz�r hale getirilir.
    Uygulama haz�r hale getirildek sonra Middleware'lere ba�vurulur. Middleware ara katman demektir.
    Middleware i�levlerini belirttikten sonra app.Run() diyerek haz�r haldeki uygulamam�z� ba�lat�r�z.
*/




