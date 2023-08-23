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
using System.Globalization;
using Crypto.Objects.Models;
using Newtonsoft.Json;

namespace Crypto.Clients
{
    public class PhemexV2Client : BaseClient
    {
        public override string Name { get; } = "PhemexUsdt";
        private static string Id { get; } = ConfigurationManager.AppSettings["phemexId"]!;
        private static string SecretKey { get; } = ConfigurationManager.AppSettings["phemexSecretKey"]!;
        public PhemexV2Client()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Add("x-phemex-access-token", Id);
        }

        public override async Task<List<TableData>> GetTableDataAsync(List<string> globalSymbols)
        {
            var result = new List<TableData>();

            string url = "https://api.phemex.com/md/v2/ticker/24hr/all";
            try
            {
                using (HttpResponseMessage response = await Client.GetAsync(url))
                {
                    var data = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(data)!;
                    foreach (JObject item in obj.result)
                    {
                        var globalNameRes = NameTranslator.ClientToGlobalName((string)item["symbol"]!, Name);
                        if (globalNameRes.Success)
                        {
                            result.Add(new TableData(globalNameRes.Name, (float)item["fundingRateRr"]! /*/ 1000f*/, Name, -100f));
                        }
                        else
                        {
                            var globalName = ToGlobalName((string)item["symbol"]!);
                            if (globalName == null)
                            {
                                Logger.Log(globalNameRes.Reason, Utility.Type.Message);
                                continue;
                            }
                            result.Add(new TableData(globalName, (float)item["fundingRateRr"]!, Name, -100f));
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

        public async Task<List<string>> GetSymbolNames()
        {
            var result = new List<string>();

            string url = "https://api.phemex.com/md/v3/ticker/24hr/all";
            using (HttpResponseMessage response = await Client.GetAsync(url))
            {
                string data = await response.Content.ReadAsStringAsync();
                dynamic obj = JsonConvert.DeserializeObject(data)!;
                foreach (var item in obj.result)
                {
                    result.Add(((string)item.symbol).Replace("USDT", ""));
                }
            }

            return result;
        }
        protected override string ToGlobalName(string marketName)
        {
            if (!marketName.EndsWith("USDT"))
            {
                return null;
            }
            return marketName.Replace("USDT", "");
        }

        public async override Task<PriceResult> GetPrice(string globalName)
        {

            var clientName = ToClientName(globalName);
            string url = $"https://api.phemex.com/md/v3/ticker/24hr?symbol={clientName}";
            try
            {
                using (HttpResponseMessage response = await Client.GetAsync(url))
                {
                    var data = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(data)!;
                    var price = (decimal)obj.result.lastRp;
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
