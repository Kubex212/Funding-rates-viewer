﻿using System;
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
using Newtonsoft.Json;

namespace Crypto.Clients
{
    public class BinanceBClient : BaseClient
    {
        public override string Name { get; } = "BinanceBUSD";
        public BinanceBClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public override async Task<List<TableData>> GetTableDataAsync(List<string> globalSymbols)
        {
            var result = new List<TableData>();

            string url = "https://www.binance.com/fapi/v1/premiumIndex";
            try
            {
                using (HttpResponseMessage response = await Client.GetAsync(url))
                {
                    string data = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(data);
                    foreach (var item in obj)
                    {
                        var globalNameRes = NameTranslator.ClientToGlobalName((string)item.symbol, Name);
                        if(globalNameRes.Success)
                        {
                            result.Add(new TableData(globalNameRes.Name, (float)item.lastFundingRate, Name, -100f));
                        }
                        else
                        {
                            var globalName = ToGlobalName((string)item.symbol);
                            if (globalName == null)
                            {
                                Logger.Log(globalNameRes.Reason, Utility.Type.Message);
                                continue;
                            }
                            result.Add(new TableData(globalName, (float)item.lastFundingRate, Name, -100f));
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

            string url = "https://www.binance.com/fapi/v1/ticker/bookTicker";
            using (HttpResponseMessage response = await Client.GetAsync(url))
            {
                string data = await response.Content.ReadAsStringAsync();
                dynamic obj = JsonConvert.DeserializeObject(data)!;
                foreach (var item in obj)
                {
                    result.Add(((string)item.symbol).Replace("USDT", "").Replace("USD", ""));
                }
            }

            return result;
        }

        protected override string ToGlobalName(string marketName)
        {
            if (!marketName.EndsWith("BUSD"))
            {
                return null;
            }
            return marketName.Replace("BUSD", "");
        }

        public async override Task<PriceResult> GetPrice(string globalName)
        {
            var clientName = ToClientName(globalName);
            string url = $"https://www.binance.com/fapi/v1/ticker/price";
            try
            {
                using (HttpResponseMessage response = await Client.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();
                    var data = await response.Content.ReadAsStringAsync();

                    var objList = JsonConvert.DeserializeObject<List<dynamic>>(data)!;
                    if (objList != null)
                    {
                        Func<dynamic, bool> condition = item => item.symbol == clientName;
                        var selectedElement = objList.FirstOrDefault(condition);

                        if (selectedElement != null)
                        {
                            var price = (decimal)selectedElement.price;
                            return new PriceResult() { Price = price };
                        }
                        else
                        {
                            return new PriceResult() { Message = "No suitable element found." };
                        }
                    }
                    else
                    {
                        return new PriceResult() { Message = "Invalid data format" };
                    }
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
            return globalName + "BUSD";
        }
    }
}
