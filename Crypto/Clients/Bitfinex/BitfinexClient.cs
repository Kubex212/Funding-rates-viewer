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

namespace Crypto.Clients.Bitfinex
{
    public class BitfinexClient : IClient
    {
        public string Name { get; } = "Bitfinex";
        public static HttpClient Client { get; private set; } 

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
            IClient c = this;
            var result = c.HandleUnknowns(globalSymbols);
            var noUnknowns = c.RemoveUnknowns(globalSymbols);
            var symbols = NameTranslator.GlobalToClientNames(noUnknowns, Name);
            string baseUrl = "https://api-pub.bitfinex.com/v2/status/deriv";
            var symbolsArray = symbols.ToArray<string>();
            string url = baseUrl + "?keys=" + String.Join(",", symbolsArray);

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
            foreach(var d in derivatives)
            {
                result.Add(new TableData(NameTranslator.ClientToGlobalName(d.Symbol, Name), d.CurrentFunding, Name, d.NextFundingAccrued));
            }
            return result;
        }

        private List<string> TranslateSymbols(List<string> symbols)
        {
            List<string> result = new List<string>();
            foreach(var s in symbols)
            {
                result.Add(s switch
                {
                    "BTCUSD" => "tBTCF0:USTF0",
                    "ETHUSD" => "tETHF0:USTF0",
                    "XRPUSD" => "tXRPF0:USTF0",
                    _ => throw new ArgumentException($"Giełda {Name} nie zna symbolu {s}")
                });
            }
            return result;
        }

    }
}
