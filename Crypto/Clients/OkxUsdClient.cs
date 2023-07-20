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
    public class OkxUsdClient : BaseClient
    {
        public override string Name { get; } = "OkxUsd";

        private static string BaseUrl { get; } = "https://okx.com";
        public OkxUsdClient()
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
                string path = $"/api/v5/public/funding-rate?instId={s}";

                try
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + path))
                    {
                        var response = await Client.SendAsync(request);

                        response.EnsureSuccessStatusCode();

                        string json = await response.Content.ReadAsStringAsync();

                        var resultObj = JObject.Parse(json);
                        var getNameRes = NameTranslator.ClientToGlobalName(s, Name);
                        var msg = Convert.ToString(resultObj["msg"]);
                        if (msg != "")
                        {
                            Logger.Log($"Okx nie obsługuje symbolu {s}", Utility.Type.Message);
                            return;
                        }
                        var f = Convert.ToString(resultObj["data"][0]["fundingRate"]);
                        var p = Convert.ToString(resultObj["data"][0]["nextFundingRate"]);
                        f = f.Replace('.', ',');
                        p = p.Replace('.', ',');
                        var fundingRate = float.Parse(f);
                        var predictedRate = float.Parse(p);
                        var data = new TableData(getNameRes.Name, fundingRate, Name, predictedRate);

                        result.Add(data);
                    }
                }
                catch (HttpRequestException ex)
                {
                    Logger.Log($"Symbol {s} nie działa ({Name}): {ex.Message}", Utility.Type.Message);

                    var getNameRes = NameTranslator.ClientToGlobalName(s, Name);
                    var data = new TableData(getNameRes.Name, -100f, Name, -100f);
                    result.Add(data);

                    return;
                }
                catch (ArgumentException ex)
                {
                    Logger.Log($"Problem z symbolem {s}({Name}): {ex.Message}");

                    var getNameRes = NameTranslator.ClientToGlobalName(s, Name);
                    var data = new TableData(getNameRes.Name, -100f, Name, -100f);
                    result.Add(data);

                    return;
                }
                catch (FormatException ex)
                {
                    Logger.Log($"Problem z symbolem {s} ({Name}) - format exception: {ex.Message}", Utility.Type.Error);

                    var getNameRes = NameTranslator.ClientToGlobalName(s, Name);
                    var data = new TableData(getNameRes.Name, -100f, Name, -100f);
                    result.Add(data);

                    return;
                }
            });

            return result;
        }
    }
}
