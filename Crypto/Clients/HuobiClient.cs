using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Crypto.Objects.Models;
using RestSharp;
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
        private string _baseUrl = "https://api.hbdm.com";
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
            string path = "/linear-swap-api/v1/swap_batch_funding_rate";
            string url = _baseUrl + path;

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
                            result.Add(new TableData(globalNameRes.Name, (float)item.funding_rate, Name, (float)item.estimated_rate));
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
                Logger.Log($"Problem z zapytaniem na giełdzie Huobi. {ex.Message}", Utility.Type.Error);
            }
            return result;
        }

        private List<string> TranslateSymbols(List<string> symbols)
        {
            return symbols;
        }

    }
}
