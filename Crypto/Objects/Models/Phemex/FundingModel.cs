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
        public long FundingRate { get; set; }
        public long PredFundingRate { get; set; }
        public FundingModel(string symbol, long fundingRate, long predFundingRate)
        {
            Symbol = symbol;
            FundingRate = fundingRate;
            PredFundingRate = predFundingRate;
        }
    }

    public class FundingModelV2
    {
        public string Symbol { get; set; }
        public float FundingRate { get; set; }
        public float PredFundingRate { get; set; }
        public FundingModelV2(string symbol, float fundingRate, float predFundingRate)
        {
            Symbol = symbol;
            FundingRate = fundingRate;
            PredFundingRate = predFundingRate;
        }
    }
}
