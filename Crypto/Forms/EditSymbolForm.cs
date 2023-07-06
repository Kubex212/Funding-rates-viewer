using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crypto.Objects;
using Crypto.Utility;

namespace Crypto.Forms
{
    public partial class EditSymbolForm : Form
    {
        Symbol _symbol;
        List<LabeledTableData> _specials;

        public EditSymbolForm(Symbol symbol, List<LabeledTableData> specials)
        {
            InitializeComponent();

            _symbol = symbol;
            _specials = specials;

            InitTextBoxes();
            InitLabels();

            ToolTip toolTip1 = new ToolTip();
            ToolTip toolTip2 = new ToolTip();
            toolTip1.SetToolTip(button1 , "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button2 , "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button3 , "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button4 , "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button5 , "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button6 , "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button7 , "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button8 , "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button9 , "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button10, "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button11, "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button12, "Oznacz jako nieznane, czyli \"?\"");
            toolTip2.SetToolTip(button1a , "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button2a , "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button3a , "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button4a , "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button5a , "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button6a , "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button7a , "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button8a , "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button9a , "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button10a, "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button11a, "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button12a, "Wypełnij wartością domniemaną");

            Text = "Edytuj " + symbol.Name;
        }

        void InitTextBoxes()
        {
            nameTextBox.Text = _symbol.Name;
            textBox1.Text = _symbol.Bitfinex;
            textBox2.Text = _symbol.Phemex;
            textBox3.Text = _symbol.PhemexUsdt;
            textBox4.Text = _symbol.Huobi;
            textBox5.Text = _symbol.Binance;
            //textBox6.Text = _symbol.Ftx;
            textBox7.Text = _symbol.Okx;
            textBox8.Text = _symbol.OkxUsd;
            textBox9.Text = _symbol.ByBitLinear;
            textBox10.Text = _symbol.ByBitInverse;
            textBox11.Text = _symbol.ByBitPerp;
            textBox12.Text = "na przyszłość";
        }

        void InitLabels()
        {
            if(_symbol.Bitfinex == "?")
            {
                label1.ForeColor = Color.DarkOrange;
            }
            else if (_specials.Any(d => d.Symbol == _symbol.Bitfinex && d.MarketName == "Bitfinex")) label1.ForeColor = Color.DarkRed;

            if (_symbol.Phemex == "?")
            {
                label2.ForeColor = Color.DarkOrange;
            }
            else if (_specials.Any(d => d.Symbol == _symbol.Phemex && d.MarketName == "Phemex")) label2.ForeColor = Color.DarkRed;

            if (_symbol.PhemexUsdt == "?")
            {
                label3.ForeColor = Color.DarkOrange;
            }
            else if (_specials.Any(d => d.Symbol == _symbol.PhemexUsdt && d.MarketName == "PhemexUsdt")) label5.ForeColor = Color.DarkRed;

            if (_symbol.Huobi == "?")
            {
                label4.ForeColor = Color.DarkOrange;
            }
            else if (_specials.Any(d => d.Symbol == _symbol.Huobi && d.MarketName == "Huobi")) label4.ForeColor = Color.DarkRed;

            if (_symbol.Binance == "?")
            {
                label5.ForeColor = Color.DarkOrange;
            }
            else if (_specials.Any(d => d.Symbol == _symbol.Binance && d.MarketName == "Binance")) label5.ForeColor = Color.DarkRed;

            //if (_symbol.Ftx == "?")
            //{
            //    label6.ForeColor = Color.Brown;
            //}
            //else if (_specials.Any(d => d.Symbol == _symbol.Ftx)) label6.ForeColor = Color.DarkRed;

            if (_symbol.Okx == "?")
            {
                label7.ForeColor = Color.DarkOrange;
            }
            else if (_specials.Any(d => d.Symbol == _symbol.Okx && d.MarketName == "Okx")) label7.ForeColor = Color.DarkRed;

            if (_symbol.OkxUsd == "?")
            {
                label8.ForeColor = Color.DarkOrange;
            }
            else if (_specials.Any(d => d.Symbol == _symbol.OkxUsd && d.MarketName == "OkxUsd")) label8.ForeColor = Color.DarkRed;

            if (_symbol.ByBitLinear == "?")
            {
                label9.ForeColor = Color.DarkOrange;
            }
            else if (_specials.Any(d => d.Symbol == _symbol.ByBitLinear && d.MarketName == "ByBitLinear")) label9.ForeColor = Color.DarkRed;

            if (_symbol.ByBitInverse == "?")
            {
                label10.ForeColor = Color.DarkOrange;
            }
            else if (_specials.Any(d => d.Symbol == _symbol.ByBitInverse && d.MarketName == "ByBitInverse")) label10.ForeColor = Color.DarkRed;

            if (_symbol.ByBitPerp == "?")
            {
                label11.ForeColor = Color.DarkOrange;
            }
            else if (_specials.Any(d => d.Symbol == _symbol.ByBitPerp && d.MarketName == "ByBitPerp")) label11.ForeColor = Color.DarkRed;

            //if (_symbol.OkxUsd == "?")
            //{
            //    label12.ForeColor = Color.Brown;
            //}
            //else if (_specials.Any(d => d.Symbol == _symbol.OkxUsd)) label8.ForeColor = Color.DarkRed;
        }
        private void okButton_Click(object sender, EventArgs e)
        {
            _symbol.Name = nameTextBox.Text;
            _symbol.Bitfinex = textBox1.Text;
            _symbol.Phemex = textBox2.Text;
            _symbol.PhemexUsdt = textBox3.Text;
            _symbol.Huobi = textBox4.Text;
            _symbol.Binance = textBox5.Text;
            //_symbol.Ftx = textBox6.Text;
            _symbol.Okx = textBox7.Text;
            _symbol.OkxUsd = textBox8.Text;
            _symbol.ByBitLinear = textBox9.Text;
            _symbol.ByBitInverse = textBox10.Text;
            _symbol.ByBitPerp = textBox11.Text;

            Close();
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "?";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "?";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox3.Text = "?";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox4.Text = "?";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox5.Text = "?";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox6.Text = "?";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox7.Text = "?";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox8.Text = "?";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox9.Text = "?";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox10.Text = "?";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox11.Text = "?";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox12.Text = "?";
        }

        private void button1a_Click(object sender, EventArgs e)
        {
            textBox1.Text = $"t{nameTextBox.Text}F0:USTF0";
        }

        private void button2a_Click(object sender, EventArgs e)
        {
            textBox2.Text = $"{nameTextBox.Text}USD";
        }

        private void button3a_Click(object sender, EventArgs e)
        {
            textBox3.Text = $"{nameTextBox.Text}USDT";
        }

        private void button4a_Click(object sender, EventArgs e)
        {
            textBox4.Text = $"{nameTextBox.Text}-USDT";
        }

        private void button5a_Click(object sender, EventArgs e)
        {
            textBox4.Text = $"{nameTextBox.Text}USDT";
        }

        private void button6a_Click(object sender, EventArgs e)
        {
            textBox6.Text = $"{nameTextBox.Text}BUSDT";
        }

        private void button7a_Click(object sender, EventArgs e)
        {
            textBox7.Text = $"{nameTextBox.Text}-USD-SWAP";
        }

        private void button8a_Click(object sender, EventArgs e)
        {
            textBox8.Text = $"{nameTextBox.Text}-USDT-SWAP";
        }

        private void button9a_Click(object sender, EventArgs e)
        {
            textBox9.Text = $"{nameTextBox.Text}USD";
        }

        private void button10a_Click(object sender, EventArgs e)
        {
            textBox10.Text = $"{nameTextBox.Text}USDT";
        }

        private void button11a_Click(object sender, EventArgs e)
        {
            textBox11.Text = $"{nameTextBox.Text}PERP";
        }

        private void button12a_Click(object sender, EventArgs e)
        {
            textBox12.Text = $"na przyszłość...";
        }


    }
}
