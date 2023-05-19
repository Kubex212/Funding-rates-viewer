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
        List<TableData> _specials;

        public EditSymbolForm(Symbol symbol, List<TableData> specials)
        {
            InitializeComponent();

            _symbol = symbol;
            _specials = specials;

            InitTextBoxes();
            InitLabels();

            ToolTip toolTip1 = new ToolTip();
            ToolTip toolTip2 = new ToolTip();
            toolTip1.SetToolTip(button1, "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button2, "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button3, "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button4, "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button5, "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button6, "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button7, "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button1, "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button2, "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button3, "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button4, "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button5, "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button6, "Oznacz jako nieznane, czyli \"?\"");
            toolTip1.SetToolTip(button7, "Oznacz jako nieznane, czyli \"?\"");
            toolTip2.SetToolTip(button8, "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button9, "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button10, "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button11, "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button12, "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button13, "Wypełnij wartością domniemaną");
            toolTip2.SetToolTip(button14, "Wypełnij wartością domniemaną");

            Text = "Edytuj " + symbol.Name;
        }

        void InitTextBoxes()
        {
            textBox1.Text = _symbol.Name;
            textBox2.Text = _symbol.Bitfinex;
            textBox3.Text = _symbol.Phemex;
            textBox4.Text = _symbol.Huobi;
            textBox5.Text = _symbol.Binance;
            textBox6.Text = _symbol.Ftx;
            textBox11.Text = _symbol.Okx;
            textBox87.Text = _symbol.OkxUsd;
        }

        void InitLabels()
        {
            if(_symbol.Bitfinex == "?")
            {
                label3.ForeColor = Color.Brown;
            }
            else if (_specials.Find(d => d.Symbol == _symbol.Bitfinex) != null) label3.ForeColor = Color.DarkRed;

            if (_symbol.Phemex == "?")
            {
                label5.ForeColor = Color.Brown;
            }
            else if (_specials.Find(d => d.Symbol == _symbol.Phemex) != null) label5.ForeColor = Color.DarkRed;

            if (_symbol.Huobi == "?")
            {
                label7.ForeColor = Color.Brown;
            }
            else if (_specials.Find(d => d.Symbol == _symbol.Huobi) != null) label7.ForeColor = Color.DarkRed;

            if (_symbol.Binance == "?")
            {
                label9.ForeColor = Color.Brown;
            }
            else if (_specials.Find(d => d.Symbol == _symbol.Binance) != null) label9.ForeColor = Color.DarkRed;

            if (_symbol.Ftx == "?")
            {
                label11.ForeColor = Color.Brown;
            }
            else if (_specials.Find(d => d.Symbol == _symbol.Ftx) != null) label11.ForeColor = Color.DarkRed;

            if (_symbol.Okx == "?")
            {
                label2.ForeColor = Color.Brown;
            }
            else if (_specials.Find(d => d.Symbol == _symbol.Okx) != null) label2.ForeColor = Color.DarkRed;

            if (_symbol.OkxUsd == "?")
            {
                label4.ForeColor = Color.Brown;
            }
            else if (_specials.Find(d => d.Symbol == _symbol.OkxUsd) != null) label4.ForeColor = Color.DarkRed;
        }
        private void okButton_Click(object sender, EventArgs e)
        {
            _symbol.Name = textBox1.Text;
            _symbol.Bitfinex = textBox2.Text;
            _symbol.Phemex = textBox3.Text;
            _symbol.Huobi = textBox4.Text;
            _symbol.Binance = textBox5.Text;
            _symbol.Ftx = textBox6.Text;
            _symbol.Okx = textBox11.Text;
            _symbol.OkxUsd = textBox87.Text;
            Close();
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "?";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = "?";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox4.Text = "?";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox5.Text = "?";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox6.Text = "?";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox11.Text = "?";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox87.Text = "?";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox2.Text = $"t{textBox1.Text}F0:USTF0";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox3.Text = $"{textBox1.Text}USD";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox4.Text = $"{textBox1.Text}-USDT";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox5.Text = $"{textBox1.Text}USDT";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox6.Text = $"{textBox1.Text}-PERP";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBox11.Text = $"{textBox1.Text}-USD-SWAP";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            textBox87.Text = $"{textBox1.Text}-USDT-SWAP";
        }
    }
}
