using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Crypto.Objects.Models;
using Crypto.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using Crypto.Objects;
using Crypto.Utility;

namespace Crypto.Clients
{
    public class HuobiClient : BaseClient
    {
        public override string Name { get; } = "Huobi";

        public HuobiClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public override async Task<List<TableData>> GetTableDataAsync(List<string> symbols)
        {
            var result = new List<TableData>();
            var url = "https://api.hbdm.com/linear-swap-api/v1/swap_batch_funding_rate";

            try
            {
                using (HttpResponseMessage response = await Client.GetAsync(url))
                {
                    string data = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(data);
                    var array = obj.data;
                    foreach (var item in array)
                    {
                        var globalNameRes = NameTranslator.ClientToGlobalName((string)item.contract_code, Name);
                        if (globalNameRes.Success)
                        {
                            result.Add(new TableData(globalNameRes.Name, (float)item.funding_rate, Name, Consts.Unknown));
                        }
                        else
                        {
                            var globalName = ToGlobalName((string)item.contract_code);
                            if (globalName == null)
                            {
                                Logger.Log(globalNameRes.Reason, Utility.Type.Message);
                                continue;
                            }
                            result.Add(new TableData(globalName, (float)item.funding_rate, Name, Consts.Unknown));
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Logger.Log($"Problem z zapytaniem na giełdzie Huobi. {ex.Message}", Utility.Type.Error);
            }
            return result;
        }

        private List<string> TranslateSymbols(List<string> symbols)
        {
            return symbols;
        }
        protected override string ToGlobalName(string marketName)
        {
            if (!marketName.EndsWith("-USDT"))
            {
                return null;
            }
            return marketName.Replace("-USDT", "");
        }

        public async override Task<PriceResult> GetPrice(string globalName)
        {
            var clientName = ToClientName(globalName);
            string url = $"https://api.hbdm.com/linear-swap-ex/market/trade?contract_code={clientName}";
            try
            {
                using (HttpResponseMessage response = await Client.GetAsync(url))
                {
                    var data = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(data)!;
                    var price = (decimal)obj.tick.data[0].price;
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
            return globalName + "-USDT";
        }
    }
}
