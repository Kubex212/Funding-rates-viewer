using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Objects
{
    public class TableRow
    {
        public string Symbol { get; set; }
        public float BitfinexFunding { get; set; } = -100;
        public float BitfinexPredicted { get; set; } = -100;
        public float PhemexFunding { get; set; } = Consts.Unknown;
        public float PhemexUsdtFunding { get; set; } = Consts.Unknown;
        public float HuobiFunding { get; set; } = -100;
        public float HuobiPredicted { get; set; } = -100;
        public float BinanceFunding { get; set; } = Consts.Unknown;
        public float BinanceBUSDFunding { get; set; } = Consts.Unknown;
        public float OkxFunding { get; set; } = -100;
        public float OkxPredicted { get; set; } = -100;
        public float OkxUsdFunding { get; set; } = -100;
        public float OkxUsdPredicted { get; set; } = -100;
        public float ByBitLinearFunding { get; set; } = Consts.Unknown;
        public float ByBitInverseFunding { get; set; } = Consts.Unknown;
        public float ByBitPerpFunding { get; set; } = Consts.Unknown;
    }
}
