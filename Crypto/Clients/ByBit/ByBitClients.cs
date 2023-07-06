using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Crypto.Objects.Models.Phemex;
using Crypto.Objects;
using System.Configuration;
using Crypto.Utility;
using System.Globalization;

namespace Crypto.Clients.Phemex
{
    public abstract class BaseByBitClient
    {
        private static HttpClient? Client { get; set; }
        private static string BaseUrl { get; } = "https://api.bybit.com";

        public static void InitializeClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected async Task<List<TableData>> GetTableDataAsyncInternal(List<string> globalSymbols, string path, string name, IClient c)
        {
            if (globalSymbols == null || globalSymbols.Count == 0)
            {
                throw new ArgumentException("no symbols were provided");
            }
            var result = c.HandleUnknowns(globalSymbols);
            var noUnknowns = c.RemoveUnknowns(globalSymbols);
            var symbols = NameTranslator.GlobalToClientNames(noUnknowns, name);

            var options = new ParallelOptions { MaxDegreeOfParallelism = 100 };
            await Parallel.ForEachAsync(symbols, options, async (s, token) =>
            {
                try
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + path + "&" + "symbol=" + s))
                    {

                        var response = await Client!.SendAsync(request);

                        response.EnsureSuccessStatusCode();

                        string json = await response.Content.ReadAsStringAsync();

                        var resultObj = JObject.Parse(json);
                        var symbol = Convert.ToString(resultObj["result"]!["list"]![0]!["symbol"]!);
                        var fundingRate = float.Parse(Convert.ToString(resultObj["result"]!["list"]![0]!["fundingRate"]!)!, CultureInfo.InvariantCulture);

                        var data = new TableData(NameTranslator.ClientToGlobalName(symbol!, name), fundingRate, name, -100);

                        result.Add(data);
                    }
                }
                catch (HttpRequestException ex)
                {
                    Logger.Log($"Symbol {s} nie działa ({name}): {ex.Message}");

                    var data = new TableData(NameTranslator.ClientToGlobalName(s, name), -100, name, -100);
                    result.Add(data);

                    return;
                }
                catch (ArgumentException ex)
                {
                    Logger.Log($"Problem z symbolem {s}({name}): {ex.Message}");

                    var data = new TableData(NameTranslator.ClientToGlobalName(s, name), -100, name, -100);
                    result.Add(data);

                    return;
                }
            });

            return result;
        }

    }
    public class ByBitLinearClient : BaseByBitClient, IClient
    {
        public string Name => "ByBitLinear";

        public async Task<List<TableData>> GetTableDataAsync(List<string> globalSymbols)
        {
            return await GetTableDataAsyncInternal(globalSymbols, "/v5/market/tickers?category=linear", Name, this);
        }

        
    }

    public class ByBitInverseClient : BaseByBitClient, IClient
    {
        public string Name => "ByBitInverse";

        public async Task<List<TableData>> GetTableDataAsync(List<string> globalSymbols)
        {
            return await GetTableDataAsyncInternal(globalSymbols, "/v5/market/tickers?category=inverse", Name, this);
        }

    }

    public class ByBitPerpClient : BaseByBitClient, IClient
    {
        public string Name => "ByBitPerp";

        public async Task<List<TableData>> GetTableDataAsync(List<string> globalSymbols)
        {
            return await GetTableDataAsyncInternal(globalSymbols, "/v5/market/tickers?category=linear", Name, this);
        }


    }
}
