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
        public string MarketName { get; }
        public float FundingRate { get; }
        public float PredictedFunding { get; }
        public TableData(string symbol, float fundingRate, string name, float futureFunding)
        {
            Symbol = symbol;
            FundingRate = fundingRate;
            MarketName = name;
            PredictedFunding = futureFunding;
        }
    }

    public class LabeledTableData : TableData
    {
        public string CoinName { get; }
        public LabeledTableData(string coinName, string symbol, float fundingRate, string name, float futureFunding) : base(symbol, fundingRate, name, futureFunding)
        {
            CoinName = coinName;
        }
    }
}
