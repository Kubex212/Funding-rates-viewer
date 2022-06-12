using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Objects.Models.Phemex
{
    public class FundingModel
    {
        public string Symbol { get; set; }
        public int FundingRate { get; set; }
        public int PredFundingRate { get; set; }
        public FundingModel(string symbol, int fundingRate, int predFundingRate)
        {
            Symbol = symbol;
            FundingRate = fundingRate;
            PredFundingRate = predFundingRate;
        }
    }
}
