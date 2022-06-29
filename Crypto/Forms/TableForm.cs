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
using Crypto.Clients;
using Crypto.Clients.Bitfinex;
using Crypto.Clients.Phemex;
using Crypto.Clients.Huobi;
using Crypto.Clients.Binance;
using Crypto.Clients.Ftx;

namespace Crypto.Forms
{
    public partial class TableForm : Form
    {
        private List<Symbol> _symbols;
        private List<string> _names;
        private List<TableRow> _rows = new List<TableRow>();
        List<IClient> _clients;

        string[] _columnNames = { "Symbol", "PhemexFunding", "PhemexPredicted", "BitfinexFunding", "BitfinexPredicted", "HuobiFunding", "HuobiPredicted", "BinanceFunding", "FtxPredicted" };
        int[] _clicks;
        (int ind, bool ascending) _sort;
        double _redMax;
        double _greenMin;
        public TableForm(int refresh, double redMax, double greenMin)
        {
            InitializeComponent();

            _clicks = new int[4];
            _redMax = redMax;
            _greenMin = greenMin;

            _symbols = SymbolProvider.GetSymbols();
            _names = SymbolProvider.GetSymbolNames();

            InitTable(_names);

        }

        private void DataToRows(List<TableData> data)
        {
            _rows = new List<TableRow>();
            foreach (var n in _names)
            {
                _rows.Add(new TableRow() { Symbol = n });
            }
            foreach(var d in data)
            {
                var row = _rows.First(r => r.Symbol == d.Symbol);
                switch(d.Name)
                {
                    case "Bitfinex":
                        {
                            row.BitfinexFunding = d.FundingRate;
                            row.BitfinexPredicted = d.PredictedFunding;
                            break;
                        }
                    case "Phemex":
                        {
                            row.PhemexFunding = d.FundingRate;
                            row.PhemexPredicted = d.PredictedFunding;
                            break;
                        }
                    case "Huobi":
                        {
                            row.HuobiFunding = d.FundingRate;
                            row.HuobiPredicted = d.PredictedFunding;
                            break;
                        }
                    case "Binance":
                        {
                            row.BinanceFunding = d.FundingRate;
                            break;
                        }
                    case "Ftx":
                        {
                            row.FtxPredicted = d.PredictedFunding;
                            break;
                        }
                    default:
                        throw new InvalidOperationException();
                };
            }
            Text = "Tabelka" + $" (załadowane symbole: {_symbols.Count})";
        }

        async void InitTable(List<string> symbols)
        {
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Show();
            pictureBox1.Update();
            List<TableData> data = new List<TableData>();
            MakeClients();
            foreach (var c in _clients)
            {
                var dat = await c.GetTableDataAsync(symbols);
                data.AddRange(dat);
            }
            DataToRows(data);

            var bindingList = new BindingList<TableRow>(_rows);
            var source = new BindingSource(bindingList, null);
            dataGridView1.DataSource = source;

            pictureBox1.Hide();

            dataGridView1.Columns["Symbol"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["BitfinexFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["BitfinexPredicted"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["PhemexFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["PhemexPredicted"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["HuobiFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["HuobiPredicted"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["BinanceFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["FtxPredicted"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Update();
            dataGridView1.Refresh();
            SetColors();
        }

        private void UpdateTable()
        {
            Sort();
            var bindingList = new BindingList<TableRow>(_rows);
            var source = new BindingSource(bindingList, null);
            dataGridView1.DataSource = source;

            SetColors();
            dataGridView1.Update();
            dataGridView1.Refresh();
        }

        private void SetColors()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach(var colName in _columnNames)
                {
                    if (colName == "Symbol") continue;
                    var c = row.Cells[colName];
                    var val = c.Value;
                    if(val == null) continue;
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    if ((float)val == -100f) style.BackColor = Color.Black;
                    else if ((float)val * 100 >= _greenMin) style.BackColor = Color.Green;
                    else if ((float)val * 100 <= _redMax) style.BackColor = Color.Red;
                    else style.BackColor = Color.White;
                    c.Style = style;
                }
            }
        }

        private void Sort()
        {
            if(_sort.ind == 0)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.Symbol).ToList();
                else _rows = _rows.OrderByDescending(r => r.Symbol).ToList();
            }
            else if (_sort.ind == 1)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.PhemexFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.PhemexFunding).ToList();
            }
            else if (_sort.ind == 2)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.PhemexPredicted).ToList();
                else _rows = _rows.OrderByDescending(r => r.PhemexPredicted).ToList();
            }
            else if (_sort.ind == 3)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.BitfinexFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.BitfinexFunding).ToList();
            }
            else if (_sort.ind == 4)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.BitfinexPredicted).ToList();
                else _rows = _rows.OrderByDescending(r => r.BitfinexPredicted).ToList();
            }
            else if (_sort.ind == 5)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.HuobiFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.HuobiFunding).ToList();
            }
            else if (_sort.ind == 6)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.HuobiPredicted).ToList();
                else _rows = _rows.OrderByDescending(r => r.HuobiPredicted).ToList();
            }
            else if (_sort.ind == 7)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.BinanceFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.BinanceFunding).ToList();
            }
            else if (_sort.ind == 8)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.FtxPredicted).ToList();
                else _rows = _rows.OrderByDescending(r => r.FtxPredicted).ToList();
            }
        }

        private void MakeClients()
        {
            _clients = new List<IClient>();
            BitfinexClient.InitializeClient();
            PhemexClient.InitializeClient();
            HuobiClient.InitializeClient();
            BinanceClient.InitializeClient();
            FtxClient.InitializeClient();
            _clients.Add(new BitfinexClient());
            _clients.Add(new PhemexClient());
            _clients.Add(new HuobiClient());
            _clients.Add(new BinanceClient());
            _clients.Add(new FtxClient());
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _sort = (e.ColumnIndex, _sort.ascending ? false : true);
            UpdateTable();
        }

        private async void odświeżToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<TableData> data = new List<TableData>();
            foreach (var c in _clients)
            {
                data.AddRange(await c.GetTableDataAsync(_names));
            }
            DataToRows(data);
            UpdateTable();
        }

        private void jakoMessageBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger.ViewAsMessageBox();
        }

        private void jakoRichTextBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var loggerWindow = new ViewLogForm();
            loggerWindow.Show();
        }

        private void TableForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void minimalnaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float min = float.PositiveInfinity;
            int x = 0, resX = 0, resY = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int y = 0;
                foreach (var colName in _columnNames)
                {
                    if (colName == "Symbol")
                    {
                        y++;
                        continue;
                    }
                    var c = row.Cells[colName];
                    var val = c.Value;
                    if (val == null || (float)val == -100f)
                    {
                        y++;
                        continue;
                    }
                    if ((float)val < min)
                    {
                        min = (float)val;
                        resX = x;
                        resY = y;
                    }
                    y++;
                }
                x++;
            }
            dataGridView1.CurrentCell = dataGridView1[resY, resX];
        }

        private void maksymalnaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float max = float.NegativeInfinity;
            int x = 0, column = 0, row = 0;
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                int y = 0;
                foreach (var colName in _columnNames)
                {
                    if (colName == "Symbol")
                    {
                        y++;
                        continue;
                    }
                    var c = r.Cells[colName];
                    var val = c.Value;
                    if (val == null || (float)val == -100f)
                    {
                        y++;
                        continue;
                    }
                    if ((float)val > max)
                    {
                        max = (float)val;
                        column = x;
                        row = y;
                    }
                    y++;
                }
                x++;
            }
            dataGridView1.CurrentCell = dataGridView1[row, column];
        }
    }
}
