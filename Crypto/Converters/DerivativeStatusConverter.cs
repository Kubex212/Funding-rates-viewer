using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Crypto.Objects.Models;

namespace Crypto.Converters
{
    public class DerivativeStatusConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<DerivativeStatus>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);

            var results = new List<DerivativeStatus>();

            foreach (var item in array)
            {
                var derivativeStatus = JsonConvert.DeserializeObject<DerivativeStatus>(item.ToString(), new DerivativeStatusResultConverter());
                results.Add(derivativeStatus);
            }

            return results;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class DerivativeStatusResultConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DerivativeStatus);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var array = JArray.Load(reader);

            return JArrayToDerivativeStatus(array);
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private DerivativeStatus JArrayToDerivativeStatus(JArray array)
        {
            string symbol = (string)array[0];
            string symbolStripped = symbol.Substring(0);

            try
            {
                return new DerivativeStatus
                {
                    Symbol = symbolStripped,
                    Mts = (long)array[1],
                    DerivPrice = (float)array[3],
                    SpotPrice = (float)array[4],
                    InsuranceFundBalance = (float)array[6],
                    NextFundingEvtTimestampMs = (long)array[8],
                    NextFundingAccrued = (float)array[9],
                    NextFundingStep = (long)array[10],
                    CurrentFunding = (float)array[12],
                    MarkPrice = (float)array[15],
                    OpenInterest = (float)array[18],
                    ClampMin = (float)array[22],
                    ClampMax = (float)array[23]
                };
            }
            catch (ArgumentException ex)
            {
                Utility.Logger.Log($"Bitfinex: błąd z {symbolStripped}. {ex.Message}", Utility.Type.Error);
                return new DerivativeStatus
                {
                    Symbol = symbolStripped,
                    NextFundingAccrued = (float)array[9],
                    CurrentFunding = (float)array[12],
                };
            }
        }
    }
}