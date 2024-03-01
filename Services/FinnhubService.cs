using Microsoft.Extensions.Configuration;
using ServiceContracts;
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

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var httpRequestMessage = new HttpRequestMessage() {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get,
                };

                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();

                StreamReader streamReader = new StreamReader(stream);

                string response = await streamReader.ReadToEndAsync();
                var responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (response == null)
                {
                    throw new InvalidOperationException("No response from Finnhub server");
                }

                if (responseDictionary.ContainsKey("error"))
                {
                    throw new InvalidOperationException($"Error {responseDictionary["error"]}");
                }

                return responseDictionary;
            }
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get,
                };

                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();

                StreamReader streamReader = new StreamReader(stream);

                string response = await streamReader.ReadToEndAsync();
                var responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (response == null)
                {
                    throw new InvalidOperationException("No response from Finnhub server");
                }

                if (responseDictionary.ContainsKey("error"))
                {
                    throw new InvalidOperationException($"Error {responseDictionary["error"]}");
                }

                return responseDictionary;
            }
        }
    }
}