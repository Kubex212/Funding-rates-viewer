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

namespace Crypto.Clients
{
    public class BinanceClient : BaseClient
    {
        public override string Name { get; } = "Binance";
        public static HttpClient Client { get; set; }
        public BinanceClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public override async Task<List<TableData>> GetTableDataAsync(List<string> globalSymbols)
        {
            var result = new List<TableData>();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            string url = "https://www.binance.com/fapi/v1/premiumIndex";
            try
            {
                using (HttpResponseMessage response = await Client.GetAsync(url))
                {
                    var data = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(data)!;
                    foreach (var item in obj)
                    {
                        var globalNameRes = NameTranslator.ClientToGlobalName((string)item.symbol, Name);
                        if (globalNameRes.Success)
                        {
                            result.Add(new TableData(globalNameRes.Name, (float)item.lastFundingRate, Name, -100f));
                        }
                        else
                        {
                            var globalName = ToGlobalName((string)item.symbol);
                            if (globalName == null)
                            {
                                Logger.Log(globalNameRes.Reason, Utility.Type.Message);
                                continue;
                            }
                            result.Add(new TableData(globalName, (float)item.lastFundingRate, Name, -100f));
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Logger.Log($"Błąd na {Name}: {ex.Message}", Utility.Type.Error);
                return result;
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return result;
        }

        public static async Task<List<string>> GetSymbolNames()
        {
            var result = new List<string>();

            string url = "https://www.binance.com/fapi/v1/ticker/bookTicker";
            using (HttpResponseMessage response = await Client.GetAsync(url))
            {
                string data = await response.Content.ReadAsStringAsync();
                dynamic obj = JsonConvert.DeserializeObject(data)!;
                foreach (var item in obj)
                {
                    result.Add(((string)item.symbol).Replace("USDT", "").Replace("USD", ""));
                }
            }

            return result;
        }

        protected override string? ToGlobalName(string marketName)
        {
            if (!marketName.EndsWith("USDT"))
            {
                return null;
            }
            return marketName.Replace("USDT", "");
        }

        public override async Task<PriceResult> GetPrice(string globalName)
        {
            var clientName = ToClientName(globalName);
            string url = $"https://www.binance.com/fapi/v1/premiumIndex?symbol={clientName}";
            try
            {
                using (HttpResponseMessage response = await Client.GetAsync(url))
                {
                    var data = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(data)!;
                    var price = (decimal)obj.indexPrice;
                    return new PriceResult() { Price = price };
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Błąd na {Name}: {ex.Message}", Utility.Type.Error);
                return new PriceResult() { Message = "Nie udało się pobrać ceny." };
            }
        }

        protected override string? ToClientName(string globalName)
        {
            return globalName + "USDT";
        }
    }
}
