using Crypto.Clients.Bitfinex;
using Crypto.Clients.Phemex;
using Crypto.Clients.Huobi;
using Crypto.Forms;
using Crypto.Objects.Models.Phemex;
using Crypto.Clients;
using Crypto.Objects;
using Crypto.Utility;

namespace Crypto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var tableForm = new TableForm((int)numericUpDown1.Value, (double)numericUpDown3.Value, (double)numericUpDown2.Value);
            tableForm.Show();
            Hide();
        }

        private List<string> FetchSymbols()
        {
            List<string> symbols = new List<string>();
            //            foreach(var cb in groupBox1.Controls.OfType<CheckBox>())
            //{
            //                if (cb.Checked)
            //                {
            //                    symbols.Add("t"+cb.Text);
            //                }
            //            }
            symbols.Add("BTC");
            //symbols.Add("XRP");
            symbols.Add("ETH");
            return symbols;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> symbols = new List<string>();
            try
            {
                symbols = File.ReadAllLines("symbole.txt").ToList();
            }
            catch(System.IO.FileNotFoundException)
            {
                Logger.Log("Nie znaleziono pliku symbole.txt!", Utility.Type.Error);
                MessageBox.Show("Nie znaleziono pliku symbole.txt!", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //var symbols = new List<string>() { "BTCUSD", "LINKUSD", "XTZUSD", "LTCUSD", "GOLDUSD", "ADAUSD", "BCHUSD", "COMPUSD", "ALGOUSD", "YFIUSD", "DOTUSD", "UNIUSD", "BATUSD", "CHZUSD",
            //    "MANAUSD", "ENJUSD", "SUSHIUSD", "SNXUSD", "GRTUSD", "MKRUSD", "TRXUSD", "EOSUSD", "ONTUSD", "NEOUSD", "ZECUSD", "FILUSD", "KSMUSD", "XMRUSD", "QTUMUSD", "XLMUSD", "ATOMUSD", "LUNAUSD", "SOLUSD",
            //    "AGLDUSD", "INJUSD", "ILVUSD", "DAOUSD", "KLAYUSD", "ASTRUSD", "MINAUSD", "GLMRUSD", "SRMUSD", "BITUSD", "FTTUSD", "MOVRUSD", "YGGUSD", "CVXUSD", "KNCUSD", "SKLUSD", "ONEUSD", "ONTUSD", "OMGUSD",
            //    "APEUSD", "GOLDUSD", "YFIUSD", "EGLDUSD", "KSMUSD", "MKRUSD", "NEOUSD", "IMXUSD", "DODOUSD", "ICPUSD", "COMPUSD", "QTUMUSD", "XLMUSD", "ZILUSD", "THETAUSD", "GMTUSD", "AVAXUSD", "DYDXUSD", "CRVUSD",
            //    "AXSUSD", "FTMUSD", "SNXUSD", "EOSUSD", "GALUSD", "BNBUSD", "LRCUSD", "AAVEUSD", "ETCUSD", "ENJUSD", "CVCUSD", "SANDUSD", "NEARUSD", "ALGOUSD", "BATUSD", "API3USD", "WAVESUSD", "WOOUSD", "ZENUSD",
            //    "WOOUSD", "ARUSD", "CTKUSD", "BALUSD", "SXPUSD", "TRXUSD", "BCHUSD", "MTLUSD", "BNXUSD", "RENUSD", "DOGEUSD", "COTIUSD", "LITUSD", "LTCUSD", "TOMOUSD", "CELOUSD", "ZRXUSD", "OCEANUSD",
            //    "FLOWUSD", "STORJUSD", "GTCUSD", "MASKUSD", "SFPUSD", "AUDIOUSD"};
            var fundingTableForm = new FundingTableForm(symbols, (int)numericUpDown1.Value, (double)numericUpDown3.Value, (double)numericUpDown2.Value);
            fundingTableForm.Show();
            Hide();
        }
    }
}