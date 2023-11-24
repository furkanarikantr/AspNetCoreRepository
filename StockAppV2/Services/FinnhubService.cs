using Microsoft.Extensions.Configuration;
using ServiceContracts;
using System.Net.Http;
using System.Text.Json;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        //https://finnhub.io/api/v1/stock/profile2?symbol={symbol}&token={token}
        public Dictionary<string, object>? GetCompanyProfile(string stockSymbol)
        {
            //Http client'ini oluştur.
            HttpClient httpClient = _httpClientFactory.CreateClient();

            //Http request'ini oluştur.
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };

            //Oluşturduğun request'i gönder.
            HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

            //Gönderdiğin request'e karşılık gelen response'un body'sini tut.
            string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

            //Response body'den gelen json verilerini Dictionary'e çevir
            Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

            if (responseDictionary == null)
                throw new InvalidOperationException("No response from server");

            if (responseDictionary.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

            //Servis fonksiyonunu çağıran yere dictionary'i döndür.
            return responseDictionary;
        }

        //https://finnhub.io/api/v1/quote?symbol={symbol}&token={token}
        public Dictionary<string, object>? GetStockPriceQuote(string stockSymbol)
        {
            //Http client'ini oluştur.
            HttpClient httpClient = _httpClientFactory.CreateClient();

            //Http request'ini oluştur.
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };

            //Oluşturduğun request'i gönder.
            HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

            //Gönderdiğin request'e karşılık gelen response'un body'sini tut.
            string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

            //Response body'den gelen json verilerini Dictionary'e çevir
            Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

            if (responseDictionary == null)
                throw new InvalidOperationException("No response from server");

            if (responseDictionary.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

            //Servis fonksiyonunu çağıran yere dictionary'i döndür.
            return responseDictionary;
        }
    }
}