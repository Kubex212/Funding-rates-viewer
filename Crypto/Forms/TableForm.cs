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
using Crypto.Clients.Okx;
using WebScraper.Services;
using System.Reflection;
using Crypto.Properties;

namespace Crypto.Forms
{
    public partial class TableForm : Form
    {
        private List<Symbol> Symbols { get => SymbolProvider.GetSymbols(); }
        private List<string> Names { get => SymbolProvider.GetSymbolNames(); }

        private List<TableRow> _rows = new List<TableRow>();
        private List<TableData> _data = new List<TableData>();
        private NotificationSettings _notificationSettings = new NotificationSettings(Settings.Default.notifications_difference);
        

        List<IClient> _clients;

        string[] _displayedColumnNames = { "Symbol", "Bitfinex funding", "Bitfinex predicted", "Phemex funding", "Phemex predicted", "Phemex USDT funding", "Phemex USDT predicted", "Huobi funding", "Huobi predicted", "Binance funding", "Binance BUSD funding", "OKX coin funding", "OKX coin predicted", "OKX USD funding", "OKX USD predicted", "ByBit USDT funding", "ByBit Inverse funding", "ByBit Perp funding" };
        string[] _columnNames = { "Symbol", "BitfinexFunding", "BitfinexPredicted", "PhemexFunding", "PhemexPredicted", "PhemexUsdtFunding", "PhemexUsdtPredicted", "HuobiFunding", "HuobiPredicted", "BinanceFunding", "BinanceBUSDFunding", "OkxFunding", "OkxPredicted", "OkxUsdFunding", "OkxUsdPredicted", "ByBitLinearFunding", "ByBitInverseFunding", "ByBitPerpFunding" };

        int _workingCells;
        (int ind, bool ascending) _sort = (-1, false);
        double _redMax;
        double _greenMin;

        public TableForm(int refresh, double redMax, double greenMin)
        {
            InitializeComponent();

            _redMax = redMax;
            _greenMin = greenMin;

            InitTable(Names);
            notificationCheckTimer.Enabled = true;

        }

        private void DataToRows(List<TableData> data)
        {
            _rows = new List<TableRow>();
            foreach (var n in Names)
            {
                _rows.Add(new TableRow() { Symbol = n });
            }
            foreach(var d in data)
            {
                var row = _rows.First(r => r.Symbol == d.Symbol);
                switch(d.MarketName)
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
                    case "PhemexUsdt":
                        {
                            row.PhemexUsdtFunding = d.FundingRate;
                            row.PhemexUsdtPredicted = d.PredictedFunding;
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
                    case "BinanceBUSD":
                        {
                            row.BinanceBUSDFunding = d.FundingRate;
                            break;
                        }
                    case "Okx":
                        {
                            row.OkxFunding = d.FundingRate;
                            row.OkxPredicted = d.PredictedFunding;
                            break;
                        }
                    case "OkxUsd":
                        {
                            row.OkxUsdFunding = d.FundingRate;
                            row.OkxUsdPredicted = d.PredictedFunding;
                            break;
                        }
                    case "ByBitLinear":
                        {
                            row.ByBitLinearFunding = d.FundingRate;
                            break;
                        }
                    case "ByBitInverse":
                        {
                            row.ByBitInverseFunding = d.FundingRate;
                            break;
                        }
                    case "ByBitPerp":
                        {
                            row.ByBitPerpFunding = d.FundingRate;
                            break;
                        }
                    default:
                        throw new InvalidOperationException($"symbol of name {d.MarketName} was not found");
                };
            }
        }

        private List<LabeledTableData> RowsToData(List<TableRow> rows)
        {
            var result = new List<LabeledTableData>();
            foreach(var r in rows)
            {
                string bitfinexName, phemexName, huobiName, binanceName, binanceBName, okxName, okxUsdName, byBitLinearName, byBitInverseName, byBitPerpName;
                try
                {
                    bitfinexName = NameTranslator.GlobalToClientName(r.Symbol, "Bitfinex");
                    phemexName = NameTranslator.GlobalToClientName(r.Symbol, "Phemex");
                    huobiName = NameTranslator.GlobalToClientName(r.Symbol, "Huobi");
                    binanceName = NameTranslator.GlobalToClientName(r.Symbol, "Binance");
                    binanceBName = NameTranslator.GlobalToClientName(r.Symbol, "BinanceBUSD");
                    okxName = NameTranslator.GlobalToClientName(r.Symbol, "Okx");
                    okxUsdName = NameTranslator.GlobalToClientName(r.Symbol, "OkxUsd");
                    byBitLinearName = NameTranslator.GlobalToClientName(r.Symbol, "ByBitLinear");
                    byBitInverseName = NameTranslator.GlobalToClientName(r.Symbol, "ByBitInverse");
                    byBitPerpName = NameTranslator.GlobalToClientName(r.Symbol, "ByBitPerp");
                }
                catch(ArgumentException)
                {
                    continue;
                }
                result.Add(new LabeledTableData(r.Symbol, bitfinexName, r.BitfinexFunding, "Bitfinex", r.BitfinexPredicted));
                result.Add(new LabeledTableData(r.Symbol, phemexName, r.PhemexFunding, "Phemex", r.PhemexPredicted));
                result.Add(new LabeledTableData(r.Symbol, huobiName, r.HuobiFunding, "Huobi", r.HuobiPredicted));
                result.Add(new LabeledTableData(r.Symbol, binanceName, r.BinanceFunding, "Binance", binanceName == "?" ? -99 : -100));
                result.Add(new LabeledTableData(r.Symbol, binanceBName, r.BinanceBUSDFunding, "BinanceBUSD", -100));
                result.Add(new LabeledTableData(r.Symbol, okxName, r.OkxFunding, "Okx", r.OkxPredicted));
                result.Add(new LabeledTableData(r.Symbol, okxUsdName, r.OkxUsdFunding, "OkxUsd", r.OkxUsdPredicted));
                result.Add(new LabeledTableData(r.Symbol, byBitLinearName, r.ByBitLinearFunding, "ByBitLinear", r.ByBitLinearFunding));
                result.Add(new LabeledTableData(r.Symbol, byBitInverseName, r.ByBitInverseFunding, "ByBitInverse", r.ByBitInverseFunding));
                result.Add(new LabeledTableData(r.Symbol, byBitPerpName, r.ByBitPerpFunding, "ByBitPerp", r.ByBitPerpFunding));
            }
            DeleteBadRows();
            return result;
        }

        private void DeleteBadRows()
        {
            _rows.RemoveAll(r => r.BinanceFunding < -90f &&
                r.BinanceBUSDFunding < -90f &&
                r.BitfinexFunding < -90f &&
                r.HuobiFunding < 90f &&
                r.PhemexFunding < 90f &&
                r.OkxFunding < 90f &&
                r.OkxUsdFunding < 90f);
        }

        async void InitTable(List<string> symbols)
        {
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Show();
            pictureBox1.Update();
            _data = new List<TableData>();
            MakeClients();
            foreach (var c in _clients)
            {
                var dat = await c.GetTableDataAsync(symbols);
                _data.AddRange(dat);
            }
            DataToRows(_data);

            var bindingList = new BindingList<TableRow>(_rows);
            var source = new BindingSource(bindingList, null);
            dataGridView1.DataSource = source;

            pictureBox1.Hide();

            dataGridView1.Columns["Symbol"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["BitfinexFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["BitfinexPredicted"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["PhemexFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["PhemexPredicted"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["PhemexUsdtFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["PhemexUsdtPredicted"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["HuobiFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["HuobiPredicted"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["BinanceFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["OkxFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["OkxPredicted"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["OkxUsdFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["OkxUsdPredicted"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["ByBitLinearFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["ByBitInverseFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["ByBitPerpFunding"].DefaultCellStyle.Format = "0.0000%";

            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkGray;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.EnableHeadersVisualStyles = false;

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Width = 80;
                dataGridView1.Columns[i].HeaderText = _displayedColumnNames[i];
            }

            dataGridView1.Update();
            dataGridView1.Refresh();
            SetColors();
        }

        private void RefreshTable()
        {
            var firstRow = Sort();
            var bindingList = new BindingList<TableRow>(_rows);
            var source = new BindingSource(bindingList, null);
            dataGridView1.DataSource = source;

            SetColors();
            dataGridView1.Update();
            dataGridView1.Refresh();
            dataGridView1.FirstDisplayedScrollingRowIndex = firstRow;
            dataGridView1.CurrentCell = dataGridView1.Rows[firstRow].Cells[0];
        }

        private void SetColors()
        {
            _workingCells = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach(var colName in _columnNames)
                {
                    if (colName == "Symbol") continue;
                    var c = row.Cells[colName];
                    var val = c.Value;
                    if(val == null) continue;
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    if ((float)val == -100f)
                    {
                        style.BackColor = Properties.Settings.Default.Color_error;
                        style.ForeColor = Properties.Settings.Default.Color_error;
                    }
                    else if ((float)val == -99f)
                    {
                        style.BackColor = Properties.Settings.Default.Color_empty;
                        style.ForeColor = Properties.Settings.Default.Color_empty;
                    }
                    else if ((float)val * 100 >= _greenMin)
                    {
                        style.BackColor = Properties.Settings.Default.Color_high;
                        _workingCells++;
                    }
                    else if ((float)val * 100 <= _redMax)
                    {
                        style.BackColor = Properties.Settings.Default.Color_low;
                        _workingCells++;
                    }
                    else
                    {
                        style.BackColor = Color.White;
                        _workingCells++;
                    }
                    c.Style = style;
                }
            }
            Text = "Tabelka" + $" (załadowane symbole: {Symbols.Count}, działające komórki: {_workingCells})";
        }

        private int Sort()
        {
            int firstRow = 0;
            if(_sort.ind == 0)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.Symbol).ToList();
                else _rows = _rows.OrderByDescending(r => r.Symbol).ToList();
            }
            else if (_sort.ind == 1)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.BitfinexFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.BitfinexFunding).ToList();
                firstRow = _rows.Count(r => r.BitfinexFunding < -90f);
            }
            else if (_sort.ind == 2)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.BitfinexPredicted).ToList();
                else _rows = _rows.OrderByDescending(r => r.BitfinexPredicted).ToList();
                firstRow = _rows.Count(r => r.BitfinexPredicted < -90f);
            }
            else if (_sort.ind == 3)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.PhemexFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.PhemexFunding).ToList();
                firstRow = _rows.Count(r => r.PhemexFunding < -90f);
            }
            else if (_sort.ind == 4)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.PhemexPredicted).ToList();
                else _rows = _rows.OrderByDescending(r => r.PhemexPredicted).ToList();
                firstRow = _rows.Count(r => r.PhemexPredicted < -90f);
            }
            else if (_sort.ind == 5)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.PhemexUsdtFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.PhemexUsdtFunding).ToList();
                firstRow = _rows.Count(r => r.PhemexUsdtFunding < -90f);
            }
            else if (_sort.ind == 6)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.PhemexUsdtPredicted).ToList();
                else _rows = _rows.OrderByDescending(r => r.PhemexUsdtPredicted).ToList();
                firstRow = _rows.Count(r => r.PhemexUsdtPredicted < -90f);
            }
            else if (_sort.ind == 7)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.HuobiFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.HuobiFunding).ToList();
                firstRow = _rows.Count(r => r.HuobiFunding < -90f);
            }
            else if (_sort.ind == 8)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.HuobiPredicted).ToList();
                else _rows = _rows.OrderByDescending(r => r.HuobiPredicted).ToList();
                firstRow = _rows.Count(r => r.HuobiPredicted < -90f);
            }
            else if (_sort.ind == 9)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.BinanceFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.BinanceFunding).ToList();
                firstRow = _rows.Count(r => r.BinanceFunding < -90f);
            }
            else if (_sort.ind == 10)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.BinanceBUSDFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.BinanceBUSDFunding).ToList();
                firstRow = _rows.Count(r => r.BinanceBUSDFunding < -90f);
            }
            else if (_sort.ind == 11)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.OkxFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.OkxFunding).ToList();
                firstRow = _rows.Count(r => r.OkxFunding < -90f);
            }
            else if (_sort.ind == 12)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.OkxPredicted).ToList();
                else _rows = _rows.OrderByDescending(r => r.OkxPredicted).ToList();
                firstRow = _rows.Count(r => r.OkxPredicted < -90f);
            }
            else if (_sort.ind == 13)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.OkxUsdFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.OkxUsdFunding).ToList();
                firstRow = _rows.Count(r => r.OkxUsdFunding < -90f);
            }
            else if (_sort.ind == 14)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.OkxUsdPredicted).ToList();
                else _rows = _rows.OrderByDescending(r => r.OkxUsdPredicted).ToList();
                firstRow = _rows.Count(r => r.OkxUsdPredicted < -90f);
            }
            else if (_sort.ind == 15)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.ByBitLinearFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.ByBitLinearFunding).ToList();
                firstRow = _rows.Count(r => r.ByBitLinearFunding < -90f);
            }
            else if (_sort.ind == 16)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.ByBitInverseFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.ByBitInverseFunding).ToList();
                firstRow = _rows.Count(r => r.ByBitInverseFunding < -90f);
            }
            else if (_sort.ind == 17)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.ByBitPerpFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.ByBitPerpFunding).ToList();
                firstRow = _rows.Count(r => r.ByBitPerpFunding < -90f);
            }
            return _sort.ascending ? firstRow : 0;
        }

        private void MakeClients()
        {
            _clients = new List<IClient>();
            BitfinexClient.InitializeClient();
            PhemexClient.InitializeClient();
            PhemexV2Client.InitializeClient();
            HuobiClient.InitializeClient();
            BinanceClient.InitializeClient();
            BinanceBClient.InitializeClient();
            //FtxClient.InitializeClient();
            OkxClient.InitializeClient();
            OkxUsdClient.InitializeClient();
            BaseByBitClient.InitializeClient();
            _clients.Add(new BitfinexClient());
            _clients.Add(new PhemexClient());
            _clients.Add(new PhemexV2Client());
            _clients.Add(new HuobiClient());
            _clients.Add(new BinanceClient());
            _clients.Add(new BinanceBClient());
            //_clients.Add(new FtxClient());
            _clients.Add(new OkxClient());
            _clients.Add(new OkxUsdClient());
            _clients.Add(new ByBitLinearClient());
            _clients.Add(new ByBitInverseClient());
            _clients.Add(new ByBitPerpClient());
        }

        private async Task UpdateData()
        {
            _data = new List<TableData>();
            foreach (var c in _clients)
            {
                _data.AddRange(await c.GetTableDataAsync(SymbolProvider.GetSymbolNames()));
            }
            DataToRows(_data);
            var notifications = Notify();
        }

        private List<(string Name, string Prop1, string Prop2, bool Predicted, float Difference)> Notify()
        {
            var res = new List<(string Name, string Prop1, string Prop2, bool Predicted, float Difference)>();

            var groups = _data.GroupBy(d => d.Symbol);
            foreach (var group in groups)
            {
                if(group.Count(d => d.FundingRate > -90f) >= 2)
                {
                    var lowestFunding = group.Where(d => d.FundingRate > -90f).Aggregate((a, b) => a.FundingRate < b.FundingRate ? a : b);
                    var highestFunding = group.Where(d => d.FundingRate > -90f).Aggregate((a, b) => a.FundingRate > b.FundingRate ? a : b);

                    var cond1 = highestFunding.FundingRate != -100f && lowestFunding.FundingRate != -100f &&
                                highestFunding.FundingRate != -99f && lowestFunding.FundingRate != -99f;
                    var diff1 = highestFunding.FundingRate - lowestFunding.FundingRate;
                    var cond2 = diff1 > _notificationSettings.Difference;

                    if (cond1 && cond2)
                    {
                        res.Add((highestFunding.Symbol, highestFunding.MarketName, lowestFunding.MarketName, false, diff1));
                    }
                }

                if(group.Count(d => d.PredictedFunding > -90f) >= 2)
                {
                    var lowestPredicted = group.Where(d => d.PredictedFunding > -90f).Aggregate((a, b) => a.PredictedFunding < b.PredictedFunding ? a : b);
                    var highestPredicted = group.Where(d => d.PredictedFunding > -90f).Aggregate((a, b) => a.PredictedFunding > b.PredictedFunding ? a : b);

                    var cond3 = highestPredicted.PredictedFunding != -100f && lowestPredicted.PredictedFunding != -100f &&
                                highestPredicted.PredictedFunding != -99f && lowestPredicted.PredictedFunding != -99f;
                    var diff2 = highestPredicted.PredictedFunding - lowestPredicted.PredictedFunding;
                    var cond4 = diff2 > _notificationSettings.Difference;


                    if (cond3 && cond4)
                    {
                        res.Add((highestPredicted.Symbol, highestPredicted.MarketName, lowestPredicted.MarketName, true, diff2));
                    }
                } 
            }

            return res;
        }

        private List<(string Name, string Prop1, string Prop2)> FindPairs()
        {
            var res = new List<(string Name, string Prop1, string Prop2)>();

            var tableRow = new TableRow();
            var props = tableRow.GetType().GetProperties().Where(p => p.PropertyType == typeof(float));

            foreach (var row in _rows)
            {
                var dictionary = props.ToDictionary(p => p.Name, p => p.GetValue(row, null));
                var propNames = props.Select(p => p.Name).ToArray();
                var values = props.Select(p => (float)p.GetValue(row, null)!).ToArray();

                for(int i = 0; i < values.Length; i++)
                {
                    for(int j = i + 1; j < values.Length; j++)
                    {
                        if (Math.Abs(values[i] - values[j]) < 0.1f)
                        {
                            res.Add((row.Symbol, propNames[i], propNames[j]));
                        }
                    }
                }
            }

            return res;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _sort = (e.ColumnIndex, _sort.ascending ? false : true);
            RefreshTable();
        }

        private async void odświeżToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await UpdateData();
            RefreshTable();
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
                    if (val == null || (float)val <= -99f)
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
                    if (val == null || (float)val <= -99f)
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

        private void modyfikujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var data = RowsToData(_rows).Where(d => d.Symbol != "Ftx").ToList();
            var window = new ViewSymbolsForm(data);
            window.ShowDialog();
        }

        private void kolorPowyżejToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = true;
            colorDialog.ShowHelp = true;
            colorDialog.Color = Properties.Settings.Default.Color_high;

            if (colorDialog.ShowDialog() == DialogResult.OK)
                Properties.Settings.Default.Color_high = colorDialog.Color;
            Properties.Settings.Default.Save();
        }

        private void kolorPoniżejToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = true;
            colorDialog.ShowHelp = true;
            colorDialog.Color = Properties.Settings.Default.Color_low;

            if (colorDialog.ShowDialog() == DialogResult.OK)
                Properties.Settings.Default.Color_low = colorDialog.Color;
            Properties.Settings.Default.Save();
        }

        private void kolorBłęduToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = true;
            colorDialog.ShowHelp = true;
            colorDialog.Color = Properties.Settings.Default.Color_error;

            if (colorDialog.ShowDialog() == DialogResult.OK)
                Properties.Settings.Default.Color_error = colorDialog.Color;
            Properties.Settings.Default.Save();
        }

        private void kolorNieobsługiwanegoSymboluToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = false;
            colorDialog.ShowHelp = true;
            colorDialog.Color = Properties.Settings.Default.Color_empty;

            if (colorDialog.ShowDialog() == DialogResult.OK)
                Properties.Settings.Default.Color_empty = colorDialog.Color;
            Properties.Settings.Default.Save();
        }

        private void dodajSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var window = new AddSymbolForm();
            window.ShowDialog();
        }

        private async void pobierzSymboleZBinanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var worker = new SeleniumWorker();
            var phemexNames = worker.GetPhemexNames();
            worker.QuitDriver();
            //var binanceNames = await BinanceClient.GetSymbolNames();
            //var newBinanceNames = binanceNames.Where(n => !Names.Contains(n)).ToList();
            //var namesUnknownForBinance = Names.Where(n => !binanceNames.Contains(n)).ToList();

            //foreach(var name in namesUnknownForBinance)
            //{
            //    // _symbols.Single(s => s.Name == name).Binance = "?";
            //}


            //var phemexUsdNames = Utility.SymbolProvider.ReadFile("phemexUSD.txt", "USD");
            //var newPhemexUsdNames = phemexUsdNames.Where(n => !Names.Contains(n)).ToList();
            //var namesUnknownForPhemexUsd = Names.Where(n => !phemexUsdNames.Contains(n)).ToList();

            //foreach (var name in namesUnknownForPhemexUsd)
            //{
            //    //_symbols.Single(s => s.Name == name).Phemex = "?";
            //}

            //var phemexUsdtNames = SymbolProvider.ReadFile("phemexUSDT.txt", "USDT");
            //var newPhemexUsdtNames = phemexUsdNames.Where(n => !Names.Contains(n)).ToList();
            //var namesUnknownForPhemexUsdt = Names.Where(n => !phemexUsdNames.Contains(n)).ToList();

            //foreach (var name in namesUnknownForPhemexUsd)
            //{
            //    // _symbols.Single(s => s.Name == name).PhemexUsdt = "?";
            //}

            //var newNames = new List<string>(newBinanceNames);
            //newNames.AddRange(newPhemexUsdNames);
            //newNames.AddRange(newPhemexUsdtNames);
            //newNames = newNames.Distinct().ToList();

            //SymbolProvider.AddSymbols(newNames.ToArray());

            //var bSymbolsCount = SymbolProvider.FixBinanceBSymbols(true);

            //MessageBox.Show($"Binance zwróciła {binanceNames.Count} symboli (w tym {bSymbolsCount} b-symboli), z czego {newBinanceNames.Count}" +
            //    $" było wcześniej nieznanych.\n" +
            //    $"Phemex zwróciła {phemexUsdNames.Count} symboli, z czego {newPhemexUsdNames.Count}" +
            //    $" było wcześniej nieznanych.\n\n" +
            //    $"Łącznie dodano {newNames.Count - bSymbolsCount} symboli.",
            //    "Wynik aktualizacji",
            //    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ustawieniaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (var form = new NotificationForm(_notificationSettings))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _notificationSettings = form.Settings;
                    Settings.Default.notifications_difference = form.Settings.Difference;
                    Settings.Default.Save();
                }
            }
        }

        private void pokażToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var notifications = Notify();

            var messageSb = new StringBuilder();
            foreach(var notification in notifications.OrderBy(n => -n.Difference).Take(10))
            {
                messageSb.AppendLine($"{notification.Name}: różnica {notification.Difference*100:0.000} na giełdach" +
                    $" {notification.Prop1} i {notification.Prop2} ({(notification.Predicted ? "predicted" : "funding")})");
            }
            if(notifications.Count > 10)
            {
                messageSb.AppendLine($"i {notifications.Count - 10} innych...");
            }

            if(notifications.Any())
            {
                MessageBox.Show(messageSb.ToString(), "Powiadomienia", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Brak powiadomień", "Powiadomienia", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            var t = menuStrip1.Items["powiadomieniaToolStripMenuItem"] as ToolStripMenuItem;
            t.Text = "Powiadomienia";
            t.DropDown.Items[1].Image = new Bitmap(Resources.bell_empty);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var notifications = Notify();

            var t = menuStrip1.Items["powiadomieniaToolStripMenuItem"] as ToolStripMenuItem;
            t.Text = "Powiadomienia " + (notifications.Any() ? $"({notifications.Count})" : "");
            t.DropDown.Items[1].Image = notifications.Any() ? new Bitmap(Resources.bell_full) : new Bitmap(Resources.bell_empty);

            if(notifications.Any())
            {
                t.Font = new Font(t.Font, FontStyle.Bold);
            }
            else
            {
                t.Font = new Font(t.Font, FontStyle.Regular);
            }
        }

        private void powiadomieniaToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            var t = menuStrip1.Items["powiadomieniaToolStripMenuItem"] as ToolStripMenuItem;
            t.Font = new Font(t.Font, FontStyle.Regular);
        }
    }
}
