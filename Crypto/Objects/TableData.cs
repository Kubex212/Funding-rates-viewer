using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Objects
{
    public class TableData
    {
        public string Symbol { get; }
        public string Name { get; }
        public float FundingRate { get; }
        public float PredictedFunding { get; }
        public TableData(string symbol, float fundingRate, string name, float futureFunding)
        {
            Symbol = symbol;
            FundingRate = fundingRate;
            Name = name;
            PredictedFunding = futureFunding;
        }
    }
}
