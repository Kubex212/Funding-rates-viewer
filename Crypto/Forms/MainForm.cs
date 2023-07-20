using Crypto.Clients;
using Crypto.Clients;
using Crypto.Clients;
using Crypto.Forms;
using Crypto.Objects.Models;
using Crypto.Clients;
using Crypto.Objects;
using Crypto.Utility;
using System.Configuration;

namespace Crypto
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            numericUpDown1.Value = (decimal)Properties.Settings.Default.RefreshTime;
            numericUpDown2.Value = (decimal)Properties.Settings.Default.MinGreen;
            numericUpDown3.Value = (decimal)Properties.Settings.Default.MaxRed;

            if (Properties.Settings.Default.ColorHigh == Color.White)
                Properties.Settings.Default.ColorHigh = Color.Green;

            if (Properties.Settings.Default.ColorLow == Color.White)
                Properties.Settings.Default.ColorLow = Color.Red;

            if (Properties.Settings.Default.Color_error == Color.White)
                Properties.Settings.Default.Color_error = Color.Black;

            if (Properties.Settings.Default.ColorEmpty == Color.White)
                Properties.Settings.Default.ColorEmpty = Color.DarkMagenta;
            Properties.Settings.Default.Save();

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var tableForm = new TableForm((int)numericUpDown1.Value, (double)numericUpDown3.Value, (double)numericUpDown2.Value);
            tableForm.Show();
            Properties.Settings.Default.RefreshTime = (int)numericUpDown1.Value;
            Properties.Settings.Default.MinGreen = (double)numericUpDown2.Value;
            Properties.Settings.Default.MaxRed = (double)numericUpDown3.Value;
            Properties.Settings.Default.Save();
            Hide();
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