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

namespace Crypto.Clients
{
    public class FtxClient : BaseClient
    {
        public override string Name { get; } = "Ftx";

        private static string BaseUrl { get; } = "https://ftx.com/api";
        public FtxClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public override async Task<List<TableData>> GetTableDataAsync(List<string> globalSymbols)
        {
            if (globalSymbols == null || globalSymbols.Count == 0)
            {
                throw new ArgumentException("no symbols were provided");
            }
            BaseClient c = this;
            var result = c.HandleUnknowns(globalSymbols);
            var noUnknowns = c.RemoveUnknowns(globalSymbols);
            var symbols = NameTranslator.GlobalToClientNames(noUnknowns, Name);

            var options = new ParallelOptions { MaxDegreeOfParallelism = 100 };
            await Parallel.ForEachAsync(symbols, options, async (s, token) =>
            {
                string path = $"/futures/{s}/stats";

                try
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + path))
                    {
                        var response = await Client.SendAsync(request);

                        response.EnsureSuccessStatusCode();

                        string json = await response.Content.ReadAsStringAsync();

                        var resultObj = JObject.Parse(json);
                        var getNameRes = NameTranslator.ClientToGlobalName(s, Name);
                        var predictedRate = float.Parse(Convert.ToString(resultObj["result"]!["nextFundingRate"])!);
                        var fundingRate = -100f;
                        var data = new TableData(getNameRes.Name, fundingRate, Name, predictedRate);

                        result.Add(data);
                    }
                }
                catch (HttpRequestException ex)
                {
                    Logger.Log($"Symbol {s} nie działa ({Name}): {ex.Message}");

                    var getNameRes = NameTranslator.ClientToGlobalName(s, Name);
                    var data = new TableData(getNameRes.Name, -100f, Name, -100f);
                    result.Add(data);

                    return;
                }
                catch (ArgumentException ex)
                {
                    Logger.Log($"Problem z symbolem {s}({Name}): {ex.Message}");

                    var symbol = NameTranslator.ClientToGlobalName(s, Name);
                    var data = new TableData(symbol.Name, -100f, Name, -100f);
                    result.Add(data);

                    return;
                }
                catch (FormatException ex)
                {
                    Logger.Log($"Problem z symbolem {s}({Name}): {ex.Message}");

                    var symbol = NameTranslator.ClientToGlobalName(s, Name);
                    var data = new TableData(symbol.Name, -100f, Name, -100f);
                    result.Add(data);

                    return;
                }
            });

            return result;
        }
    }
}
