using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Crypto.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using Crypto.Objects;
using Crypto.Utility;
using Crypto.Objects.Models;

namespace Crypto.Clients
{
    public class BitfinexClient : BaseClient
    {
        public override string Name { get; } = "Bitfinex";

        public BitfinexClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public override async Task<List<TableData>> GetTableDataAsync(List<string> globalSymbols)
        {
            var result = new List<TableData>();
            string baseUrl = "https://api-pub.bitfinex.com/v2/status/deriv";
            string url = baseUrl + "?keys=ALL";

            try
            {
                using (HttpResponseMessage response = await Client.GetAsync(url))
                {
                    string derivativeStatuses = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<List<DerivativeStatus>>(derivativeStatuses, new DerivativeStatusConverter());

                    result.AddRange(DerivativeStatusToTableData(res));
                    return result;
                }
            }
            catch (HttpRequestException ex)
            {
                Logger.Log($"Błąd na {Name}: {ex.Message}", Utility.Type.Error);

                return result;
            }
        }

        private List<TableData> DerivativeStatusToTableData(List<DerivativeStatus> derivatives)
        {
            List<TableData> result = new List<TableData>();
            foreach (var d in derivatives)
            {
                var res = NameTranslator.ClientToGlobalName(d.Symbol, Name);
                if (res.Success)
                {
                    result.Add(new TableData(res.Name, d.CurrentFunding, Name, d.NextFundingAccrued));
                }
                else
                {
                    var globalName = ToGlobalName(d.Symbol);
                    if (globalName == null)
                    {
                        Logger.Log(res.Reason, Utility.Type.Message);
                        continue;
                    }
                    result.Add(new TableData(globalName, d.CurrentFunding, Name, d.NextFundingAccrued));
                }
            }
            return result;
        }

        protected override string? ToGlobalName(string marketName)
        {
            var firstF = marketName.IndexOf("F");
            var baseName = marketName.Substring(1, firstF - 1);
            if (marketName.EndsWith("USTF0"))
            {
                return baseName;
            }
            else if (marketName.EndsWith("BTCF0"))
            {
                return baseName + "-BTC";
            }
            else if (marketName.EndsWith("EUTF0"))
            {
                return baseName + "-EUT";
            }
            else
            {
                return null;
            }
            // tALGF0:USTF0
        }

        public async override Task<PriceResult> GetPrice(string globalName)
        {
            string baseUrl = "https://api-pub.bitfinex.com/v2/status/deriv";
            var clientName = ToClientName(globalName);
            string url = baseUrl + "?keys=ALL";

            try
            {
                using (HttpResponseMessage response = await Client.GetAsync(url))
                {
                    string derivativeStatuses = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<List<DerivativeStatus>>(derivativeStatuses, new DerivativeStatusConverter())!;

                    var price = res[0].DerivPrice;
                    return new PriceResult() { Price = (decimal)price };
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
            if (!globalName.Contains("-"))
            {
                return $"t{globalName}F0:USTF0";
            }
            else if (globalName.Contains("-EUT"))
            {
                return $"t{globalName.Replace("-EUT", "")}F0:EUTF0";
            }
            else if (globalName.Contains("-BTC"))
            {
                return $"t{globalName.Replace("-BTC", "")}F0:BTCF0";
            }
            else throw new Exception("nieoczekiwany globalny symbol");
        }
    }
}
