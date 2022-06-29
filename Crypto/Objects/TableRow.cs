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
        public float PhemexFunding { get; set; } = -100;
        public float PhemexPredicted { get; set; } = -100;
        public float BitfinexFunding { get; set; } = -100;
        public float BitfinexPredicted { get; set; } = -100;
        public float HuobiFunding { get; set; } = -100;
        public float HuobiPredicted { get; set; } = -100;
        public float BinanceFunding { get; set; } = -100;
        public float FtxPredicted { get; set; } = -100;
    }
}
