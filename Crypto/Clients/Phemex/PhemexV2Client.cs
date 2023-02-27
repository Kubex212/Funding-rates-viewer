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
    public class PhemexV2Client : IClient
    {
        public string Name { get; } = "PhemexUsdt";
        public static HttpClient Client { get; set; }
        private static string Id { get; } = ConfigurationManager.AppSettings["phemexId"];
        private static string SecretKey { get; } = ConfigurationManager.AppSettings["phemexSecretKey"];
        private static string BaseUrl { get; } = "https://api.phemex.com";
        public static void InitializeClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Add("x-phemex-access-token", Id);
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
            var res = new List<FundingModelV2>();

            string path = "/md/v2/ticker/24hr";

            var options = new ParallelOptions { MaxDegreeOfParallelism = 100 };
            await Parallel.ForEachAsync(symbols, options, async (s, token) =>
            {
                try
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + path + "?" + "symbol=" + s))
                    {
                        var exp = GetExpiry();
                        request.Headers.Add("x-phemex-request-expiry", exp.ToString());
                        request.Headers.Add("x-phemex-request-signature", GetSignature(path + "symbol=" + s + exp.ToString()));

                        var response = await Client.SendAsync(request);

                        response.EnsureSuccessStatusCode();

                        string json = await response.Content.ReadAsStringAsync();

                        var resultObj = JObject.Parse(json);
                        var symbol = Convert.ToString(resultObj["result"]["symbol"]);
                        var frString = Convert.ToString(resultObj["result"]["fundingRateRr"]);
                        var fundingRate = float.Parse(frString, CultureInfo.InvariantCulture);
                        var predFrString = Convert.ToString(resultObj["result"]["predFundingRateRr"]);
                        var predictedRate = float.Parse(predFrString, CultureInfo.InvariantCulture);
                        var model = new FundingModelV2(NameTranslator.ClientToGlobalName(symbol, Name), fundingRate, predictedRate);

                        res.Add(model);
                    }
                }
                catch (HttpRequestException ex)
                {
                    Logger.Log($"Symbol {s} nie działa ({Name}): {ex.Message}");

                    var model = new FundingModelV2(NameTranslator.ClientToGlobalName(s, Name), -99, 0);
                    res.Add(model);

                    return;
                }
                catch (ArgumentException ex)
                {
                    Logger.Log($"Problem z symbolem {s}({Name}): {ex.Message}");

                    var model = new FundingModelV2(NameTranslator.ClientToGlobalName(s, Name), -99, 0);
                    res.Add(model);

                    return;
                }
            });
            
            result.AddRange(FundingModelToTableData(res));
            return result;
        }

        private string GetSignature(string data)
        {
            HMACSHA256 hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey));
            var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(data));
            string value = Convert.ToBase64String(signature);
            return value; 
        }

        private long GetExpiry(int offset = 60)
        {
            return (int)((DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds + offset);
        }

        private List<TableData> FundingModelToTableData(List<FundingModelV2> fundingModels)
        {
            List<TableData> result = new List<TableData>();
            foreach (var model in fundingModels)
            {
                if (model == null) continue;
                if (model.FundingRate == -12345) result.Add(new TableData(model.Symbol, -100f, Name, -100f));
                result.Add(new TableData(model.Symbol, model.FundingRate, Name, model.PredFundingRate));
            }
            return result;
        }

        private List<string> TranslateSymbols(List<string> symbols)
        {
            List<string> result = new List<string>();
            foreach (var s in symbols)
            {
                result.Add(s);
            }
            return result;
        }
    }
}
