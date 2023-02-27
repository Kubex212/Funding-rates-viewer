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
    public class Symbol : ICloneable, IEquatable<Symbol>
    {
        /// <summary>
        /// Global name used throughout the program and on the UI
        /// </summary>
        public string? Name { get; set; }
        public string? Bitfinex { get; set; }
        public string? Phemex { get; set; }
        public string? PhemexUsdt { get; set; }
        public string? Huobi { get; set; }
        public string? Binance { get; set; }
        public string? Ftx { get; set; }
        public string? Okx { get; set; }
        public string? OkxUsd { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public bool Equals(Symbol? other)
        {
            return (Name == other?.Name && Bitfinex == other?.Bitfinex && Phemex == other?.Phemex && Binance == other?.Binance && Ftx == other?.Ftx && Okx == other?.Okx && OkxUsd == other?.OkxUsd && Huobi == other?.Huobi);
        }
    }
}
