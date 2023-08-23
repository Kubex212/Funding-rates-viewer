using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Crypto.Objects.Models;
using Crypto.Objects;
using System.Configuration;
using Crypto.Utility;
using System.Globalization;
using Newtonsoft.Json;

namespace Crypto.Clients
{
    public abstract class BaseByBitClient : BaseClient
    {
        private static string BaseUrl { get; } = "https://api.bybit.com";
        protected abstract string Path { get; }
        protected abstract string PricePath { get; }

        public BaseByBitClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public override async Task<List<TableData>> GetTableDataAsync(List<string> symbols)
        {
            var result = new List<TableData>();

            string url = BaseUrl + Path;
            try
            {
                using (HttpResponseMessage response = await Client.GetAsync(url))
                {
                    var data = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(data)!;
                    foreach (JObject item in obj.result.list)
                    {
                        var marketName = (string)item["symbol"]!;
                        var globalNameRes = NameTranslator.ClientToGlobalName(marketName, Name);
                        if (globalNameRes.Success)
                        {
                            result.Add(new TableData(globalNameRes.Name, (float)item["fundingRate"]!, Name, -100f));
                        }
                        else
                        {
                            var globalName = ToGlobalName(marketName);
                            if (globalName == null)
                            {
                                Logger.Log(globalNameRes.Reason, Utility.Type.Message);
                                continue;
                            }
                            result.Add(new TableData(globalName, (float)item["fundingRate"]!, Name, -100f));
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

        public async override Task<PriceResult> GetPrice(string globalName)
        {
            var clientName = ToClientName(globalName);
            string url = BaseUrl + PricePath + $"&symbol={clientName}";
            try
            {
                using (HttpResponseMessage response = await Client.GetAsync(url))
                {
                    var data = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(data)!;
                    obj = obj.result.list[0];
                    var price = (decimal)obj.price;
                    return new PriceResult() { Price = price };
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Błąd na {Name}: {ex.Message}", Utility.Type.Error);
                return new PriceResult() { Message = "Nie udało się pobrać ceny." };
            }
        }
    }
    public class ByBitLinearClient : BaseByBitClient
    {
        public override string Name => "ByBitLinear";
        protected override string Path { get; } = "/v5/market/tickers?category=linear";
        protected override string PricePath { get; } = "/v5/market/recent-trade?category=linear";
        protected override string? ToClientName(string globalName) => globalName + "USDT";
        protected override string ToGlobalName(string marketName)
        {
            if(!marketName.EndsWith("USDT"))
            {
                return null;
            }
            return marketName.Replace("USDT", "");
        }
    }

    public class ByBitInverseClient : BaseByBitClient
    {
        public override string Name => "ByBitInverse";
        protected override string Path { get; } = "/v5/market/tickers?category=inverse";
        protected override string PricePath { get; } = "/v5/market/recent-trade?category=inverse";
        protected override string? ToClientName(string globalName) => globalName + "USD";
        protected override string ToGlobalName(string marketName)
        {
            if (!marketName.EndsWith("USD"))
            {
                return null;
            }
            return marketName.Replace("USD", "");
        }
    }

    public class ByBitPerpClient : BaseByBitClient
    {
        public override string Name => "ByBitPerp";
        protected override string Path { get; } = "/v5/market/tickers?category=linear";
        protected override string PricePath { get; } = "/v5/market/recent-trade?category=linear";
        protected override string? ToClientName(string globalName) => globalName + "PERP";
        protected override string ToGlobalName(string marketName)
        {
            if (!marketName.EndsWith("PERP"))
            {
                return null;
            }
            return marketName.Replace("PERP", "");
        }
    }
}
