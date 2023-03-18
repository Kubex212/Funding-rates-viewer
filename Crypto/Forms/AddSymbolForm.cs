using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crypto.Forms
{
    public partial class AddSymbolForm : Form
    {
        public AddSymbolForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var value = textBox1.Text;

            textBoxBitfinex.Text = $"t{value}F0:USTF0";
            textBoxPhemex.Text = $"{value}USD";
            textBoxPhemexUsdt.Text = $"{value}USDT";
            textBoxHuobi.Text = $"{value}-USDT";
            textBoxBinance.Text = $"{value}USDT";
            textBoxOkxCoin.Text = $"{value}-USD-SWAP";
            textBoxOkxUsd.Text = $"{value}-USDT-SWAP";
            textBoxByBitInverse.Text = $"{value}USDT";
            textBoxByBitLinear.Text = $"{value}USD";
            textBoxByBitPerp.Text = $"{value}PERP";
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            var symbol = new Objects.Symbol()
            {
                Name = textBox1.Text,
                Bitfinex = textBoxBitfinex.Text,
                Phemex = "to delete",
                PhemexUsdt = "to delete",
                Huobi = textBoxHuobi.Text,
                Binance = textBoxBinance.Text,
                Ftx = "to delete",
                Okx = textBoxOkxCoin.Text,
                OkxUsd = textBoxOkxUsd.Text,
                ByBitLinear = textBoxByBitLinear.Text,
                ByBitInverse = textBoxByBitInverse.Text,
                ByBitPerp = textBoxByBitPerp.Text,
            };

            Utility.SymbolProvider.AddSymbols(symbol);
            Close();
        }

    }
}



