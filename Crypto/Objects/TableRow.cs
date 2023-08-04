using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Objects
{
    public class TableRow
    {
        public string Symbol { get; set; }
        public float BitfinexFunding { get; set; } = Consts.Unknown;
        public float BitfinexPredicted { get; set; } = Consts.Unknown;
        public float PhemexFunding { get; set; } = Consts.Unknown;
        public float PhemexUsdtFunding { get; set; } = Consts.Unknown;
        public float HuobiFunding { get; set; } = Consts.Unknown;
        public float HuobiPredicted { get; set; } = Consts.Unknown;
        public float BinanceFunding { get; set; } = Consts.Unknown;
        public float BinanceBUSDFunding { get; set; } = Consts.Unknown;
        public float OkxFunding { get; set; } = -100;
        public float OkxPredicted { get; set; } = -100;
        public float OkxUsdFunding { get; set; } = -100;
        public float OkxUsdPredicted { get; set; } = -100;
        public float ByBitLinearFunding { get; set; } = Consts.Unknown;
        public float ByBitInverseFunding { get; set; } = Consts.Unknown;
        public float ByBitPerpFunding { get; set; } = Consts.Unknown;
        public float MaxDiff
        {
            get
            {
                var type = this.GetType();

                // Get all properties of type float using reflection
                var fundingProps = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                     .Where(p => p.PropertyType == typeof(float) 
                                                     && p.Name != "MaxDiff"
                                                     && p.Name.Contains("Funding"))
                                                     .ToArray();


                // Initialize min and max values
                var minValue = float.MaxValue;
                var maxValue = float.MinValue;

                // Iterate through the float properties and find the min and max values
                foreach (var property in fundingProps)
                {
                    var value = (float)property.GetValue(this);
                    if(value == Consts.Unknown || value == Consts.Error)
                    {
                        continue;
                    }
                    minValue = Math.Min(minValue, value);
                    maxValue = Math.Max(maxValue, value);
                }
                
                var fundingDiff = maxValue - minValue;

                var predictedProps = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                     .Where(p => p.PropertyType == typeof(float)
                                                     && p.Name != "MaxDiff"
                                                     && p.Name.Contains("Predicted"))
                                                     .ToArray();

                minValue = float.MaxValue;
                maxValue = float.MinValue;

                // Iterate through the float properties and find the min and max values
                foreach (var property in fundingProps)
                {
                    var value = (float)property.GetValue(this);
                    if (value == Consts.Unknown || value == Consts.Error)
                    {
                        continue;
                    }
                    minValue = Math.Min(minValue, value);
                    maxValue = Math.Max(maxValue, value);
                }

                var predictedDiff = maxValue - minValue;

                return Math.Max(fundingDiff, predictedDiff);
            }
        }
    }
}
