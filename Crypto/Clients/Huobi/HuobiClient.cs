using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Crypto.Objects.Models.Bitfinex;
using RestSharp;
using Crypto.Converters.Bitfinex;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using Crypto.Objects;
using Crypto.Utility;

namespace Crypto.Clients.Huobi
{
    public class HuobiClient : IClient
    {
        private string _baseUrl = "https://api.hbdm.com";
        public string Name { get; } = "Huobi";
        public static HttpClient Client { get; private set; }

        public static void InitializeClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<TableData>> GetTableDataAsync(List<string> symbols)
        {
            try
            {
                string name = NameTranslator.ClientToGlobalName("BTC-USDT", Name);
            }
            catch(InvalidOperationException)
            {
                return new List<TableData>();
            }

            var result = new List<TableData>();
            string path = "/linear-swap-api/v1/swap_batch_funding_rate";
            string url = _baseUrl + path;

            try
            {
                using (HttpResponseMessage response = await Client.GetAsync(url))
                {
                    string data = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(data);
                    var array = obj.data;
                    foreach (var item in array)
                    {
                        if (item.funding_rate == null) item.funding_rate = 0f;
                        if (item.estimated_rate == null) item.estimated_rate = 0f;
                        try
                        {
                            string globalName = NameTranslator.ClientToGlobalName((string)item.contract_code, Name);
                            result.Add(new TableData(globalName, (float)item.funding_rate, Name, (float)item.estimated_rate));
                        }
                        catch(InvalidOperationException ex)
                        {
                            Logger.Log(ex.Message, Utility.Type.Warning);
                        }
                    }
                }
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                Logger.Log($"Problem z zapytaniem na giełdzie Huobi. {ex.Message}", Utility.Type.Error);
            }
            return result;
        }

        private List<string> TranslateSymbols(List<string> symbols)
        {
            return symbols;
        }

    }
}
