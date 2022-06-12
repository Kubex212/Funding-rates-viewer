using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Objects.Models.Bitfinex
{
    public class DerivativeStatus
    {
        public string Symbol { get; set; }
        public long Mts  { get; set; }
        public float DerivPrice { get; set; }
        public float SpotPrice { get; set; }
        public float InsuranceFundBalance { get; set; }
        public long NextFundingEvtTimestampMs { get; set; }
        public float NextFundingAccrued { get; set; }
        public long NextFundingStep { get; set; }
        public float CurrentFunding { get; set; }
        public float MarkPrice { get; set; }
        public float OpenInterest { get; set; }
        public float ClampMin { get; set; }
        public float ClampMax { get; set; }
    }
}
