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
                        var globalNameRes = NameTranslator.ClientToGlobalName((string)item["symbol"]!, Name);
                        if (globalNameRes.Success)
                        {
                            result.Add(new TableData(globalNameRes.Name, (float)item["fundingRate"]!, Name, -100f));
                        }
                        else
                        {
                            Logger.Log(globalNameRes.Reason, Utility.Type.Message);
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
    public class ByBitLinearClient : BaseByBitClient
    {
        public override string Name => "ByBitLinear";
        protected override string Path { get; } = "/v5/market/tickers?category=linear";
    }

    public class ByBitInverseClient : BaseByBitClient
    {
        public override string Name => "ByBitInverse";
        protected override string Path { get; } = "/v5/market/tickers?category=inverse";
    }

    public class ByBitPerpClient : BaseByBitClient
    {
        public override string Name => "ByBitPerp";
        protected override string Path { get; } = "/v5/market/tickers?category=linear";
    }
}
