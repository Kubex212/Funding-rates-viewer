using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Objects
{
    /// <summary>
    /// Class that contains symbol names on different markets
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// Global name used througout the program and on the UI
        /// </summary>
        public string? Name { get; set; }
        public string? Bitfinex { get; set; }
        public string? Huobi { get; set; }
        public string? Phemex { get; set; }
        public string? Binance { get; set; }
        public string? Ftx { get; set; }
    }
}
