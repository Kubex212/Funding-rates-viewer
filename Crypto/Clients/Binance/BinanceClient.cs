using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Crypto.Objects;
using System.Configuration;
using Crypto.Utility;
using Newtonsoft.Json;

namespace Crypto.Clients.Binance
{
    public class BinanceClient : IClient
    {
        public string Name { get; } = "Binance";
        public static HttpClient Client { get; set; }
        private static string BaseUrl { get; } = "https://www.binance.com/";
        public static void InitializeClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<TableData>> GetTableDataAsync(List<string> globalSymbols)
        {
            if (globalSymbols == null || globalSymbols.Count == 0)
            {
                throw new ArgumentException("no symbols were provided");
            }
            var symbols = NameTranslator.GlobalToClientNames(globalSymbols, Name);
            var result = new List<TableData>();

            string url = BaseUrl + "fapi/v1/premiumIndex";
            try
            {
                using (HttpResponseMessage response = await Client.GetAsync(url))
                {
                    string data = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(data);
                    foreach (var item in obj)
                    {
                        if (item.lastFundingRate == null) item.lastFundingRate = 0f;
                        try
                        {
                            string globalName = NameTranslator.ClientToGlobalName((string)item.symbol, Name);
                            result.Add(new TableData(globalName, (float)item.lastFundingRate, Name, -100f));
                        }
                        catch (InvalidOperationException ex)
                        {
                            Logger.Log(ex.Message, Utility.Type.Warning);
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Logger.Log($"Błąd na {Name}: {ex.Message}", Utility.Type.Error);

                return result;
            }
            return result;
        }

    }
}
