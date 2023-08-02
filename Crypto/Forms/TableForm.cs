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
using Crypto.Clients;
using WebScraper.Services;
using System.Reflection;
using Crypto.Properties;
using System.Runtime.InteropServices;
using System.Media;

namespace Crypto.Forms
{
    public partial class TableForm : Form
    {
        private List<Symbol> Symbols { get => SymbolProvider.GetSymbols(); }
        private List<string> Names { get => SymbolProvider.GetSymbolNames(); }

        private List<TableRow> _rows = new List<TableRow>();
        private List<TableData> _data = new List<TableData>();
        private NotificationSettings _notificationSettings = 
            new NotificationSettings(Settings.Default.NotificationsDifference, Settings.Default.NotificationSoundDifference)
            { Groups = Settings.Default.GroupsJson == "" 
                ? new() 
                :  Newtonsoft.Json.JsonConvert.DeserializeObject<List<NotificationGroup>>(Settings.Default.GroupsJson)! };

        private Dictionary<string, BaseClient> _clientsByNames = new Dictionary<string, BaseClient>();
        

        List<BaseClient> _clients;

        string[] _displayedColumnNames = { "Symbol", "Bitfinex funding", "Bitfinex predicted", "Phemex funding", "Phemex USDT funding", "Huobi funding", "Huobi predicted", "Binance funding", "Binance BUSD funding", "OKX coin funding", "OKX coin predicted", "OKX USD funding", "OKX USD predicted", "ByBit USDT funding", "ByBit Inverse funding", "ByBit Perp funding", "Różnica" };
        string[] _columnNames = { "Symbol", "BitfinexFunding", "BitfinexPredicted", "PhemexFunding", "PhemexUsdtFunding", "HuobiFunding", "HuobiPredicted", "BinanceFunding", "BinanceBUSDFunding", "OkxFunding", "OkxPredicted", "OkxUsdFunding", "OkxUsdPredicted", "ByBitLinearFunding", "ByBitInverseFunding", "ByBitPerpFunding", "MaxDiff" };

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
            _lastRefresh = DateTime.Now;
            refreshTimer.Interval = Settings.Default.RefreshTime * 1000;
            refreshTimer.Enabled = true;
            refreshCountdown.Enabled = true;

        }

        private void DataToRows(List<TableData> data)
        {
            _rows = new List<TableRow>();
            //foreach (var n in Names)
            //{
            //    _rows.Add(new TableRow() { Symbol = n });
            //}
            foreach(var d in data)
            {
                var row = _rows.FirstOrDefault(r => r.Symbol == d.Symbol);
                if(row == null)
                {
                    row = new TableRow() { Symbol = d.Symbol };
                    _rows.Add(row);
                }
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
                            break;
                        }
                    case "PhemexUsdt":
                        {
                            row.PhemexUsdtFunding = d.FundingRate;
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
            _rows = _rows.OrderBy(r => Names.IndexOf(r.Symbol) < 0 ? 100_000 : Names.IndexOf(r.Symbol)).ToList();
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
            //foreach (var c in _clients)
            //{
            //    var watch = System.Diagnostics.Stopwatch.StartNew();
            //    _data.AddRange(await c.GetTableDataAsync(SymbolProvider.GetSymbolNames()));
            //    watch.Stop();
            //    var elapsedMs = watch.ElapsedMilliseconds;
            //}
            var tasks = _clients.Select(obj => obj.GetTableDataAsync(SymbolProvider.GetSymbolNames())).ToList();

            await Task.WhenAll(tasks);

            var results = tasks.SelectMany(task => task.Result).ToList();
            _data.AddRange(results);
            DataToRows(_data);

            var bindingList = new BindingList<TableRow>(_rows);
            var source = new BindingSource(bindingList, null);
            dataGridView1.DataSource = source;

            pictureBox1.Hide();

            // TODO: fix that 
            dataGridView1.Columns["Symbol"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["BitfinexFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["BitfinexPredicted"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["PhemexFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["PhemexUsdtFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["HuobiFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["HuobiPredicted"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["BinanceFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["BinanceBUSDFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["OkxFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["OkxPredicted"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["OkxUsdFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["OkxUsdPredicted"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["ByBitLinearFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["ByBitInverseFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["ByBitPerpFunding"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["MaxDiff"].DefaultCellStyle.Format = "0.0000%";

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
                        style.BackColor = Properties.Settings.Default.ColorEmpty;
                        style.ForeColor = Properties.Settings.Default.ColorEmpty;
                    }
                    else if ((float)val * 100 >= _greenMin && colName != _columnNames.Last())
                    {
                        style.BackColor = Properties.Settings.Default.ColorHigh;
                        _workingCells++;
                    }
                    else if ((float)val >= _notificationSettings.DefaultStandardDifference && colName == _columnNames.Last())
                    {
                        style.BackColor = Properties.Settings.Default.ColorHigh;
                        _workingCells++;
                    }
                    else if ((float)val * 100 <= _redMax && colName != _columnNames.Last())
                    {
                        style.BackColor = Properties.Settings.Default.ColorLow;
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
            int i = 0;
            if(_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.Symbol).ToList();
                else _rows = _rows.OrderByDescending(r => r.Symbol).ToList();
            }
            else if (_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.BitfinexFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.BitfinexFunding).ToList();
                firstRow = _rows.Count(r => r.BitfinexFunding < -90f);
            }
            else if (_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.BitfinexPredicted).ToList();
                else _rows = _rows.OrderByDescending(r => r.BitfinexPredicted).ToList();
                firstRow = _rows.Count(r => r.BitfinexPredicted < -90f);
            }
            else if (_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.PhemexFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.PhemexFunding).ToList();
                firstRow = _rows.Count(r => r.PhemexFunding < -90f);
            }
            else if (_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.PhemexUsdtFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.PhemexUsdtFunding).ToList();
                firstRow = _rows.Count(r => r.PhemexUsdtFunding < -90f);
            }
            else if (_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.HuobiFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.HuobiFunding).ToList();
                firstRow = _rows.Count(r => r.HuobiFunding < -90f);
            }
            else if (_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.HuobiPredicted).ToList();
                else _rows = _rows.OrderByDescending(r => r.HuobiPredicted).ToList();
                firstRow = _rows.Count(r => r.HuobiPredicted < -90f);
            }
            else if (_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.BinanceFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.BinanceFunding).ToList();
                firstRow = _rows.Count(r => r.BinanceFunding < -90f);
            }
            else if (_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.BinanceBUSDFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.BinanceBUSDFunding).ToList();
                firstRow = _rows.Count(r => r.BinanceBUSDFunding < -90f);
            }
            else if (_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.OkxFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.OkxFunding).ToList();
                firstRow = _rows.Count(r => r.OkxFunding < -90f);
            }
            else if (_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.OkxPredicted).ToList();
                else _rows = _rows.OrderByDescending(r => r.OkxPredicted).ToList();
                firstRow = _rows.Count(r => r.OkxPredicted < -90f);
            }
            else if (_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.OkxUsdFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.OkxUsdFunding).ToList();
                firstRow = _rows.Count(r => r.OkxUsdFunding < -90f);
            }
            else if (_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.OkxUsdPredicted).ToList();
                else _rows = _rows.OrderByDescending(r => r.OkxUsdPredicted).ToList();
                firstRow = _rows.Count(r => r.OkxUsdPredicted < -90f);
            }
            else if (_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.ByBitLinearFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.ByBitLinearFunding).ToList();
                firstRow = _rows.Count(r => r.ByBitLinearFunding < -90f);
            }
            else if (_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.ByBitInverseFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.ByBitInverseFunding).ToList();
                firstRow = _rows.Count(r => r.ByBitInverseFunding < -90f);
            }
            else if (_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.ByBitPerpFunding).ToList();
                else _rows = _rows.OrderByDescending(r => r.ByBitPerpFunding).ToList();
                firstRow = _rows.Count(r => r.ByBitPerpFunding < -90f);
            }
            else if (_sort.ind == i++)
            {
                if (_sort.ascending) _rows = _rows.OrderBy(r => r.MaxDiff).ToList();
                else _rows = _rows.OrderByDescending(r => r.MaxDiff).ToList();
                firstRow = _rows.Count(r => r.MaxDiff < -90f);
            }
            return _sort.ascending ? firstRow : 0;
        }

        private void MakeClients()
        {
            _clients = new List<BaseClient>();
            var bitfinexClient = new BitfinexClient();
            var phemexClient = new PhemexClient();
            var phemexV2Client = new PhemexV2Client();
            var huobiClient = new HuobiClient();
            var binanceClient = new BinanceClient();
            var binanceBClient = new BinanceBClient();
            var okxClient = new OkxClient();
            var okxUsdClient = new OkxUsdClient();
            var byBitLinearClient = new ByBitLinearClient();
            var byBitInverseClient = new ByBitInverseClient();
            var byBitPerpClient = new ByBitPerpClient();
            _clients.Add(bitfinexClient);
            _clients.Add(phemexClient);
            _clients.Add(phemexV2Client);
            _clients.Add(huobiClient);
            _clients.Add(binanceClient);
            _clients.Add(binanceBClient);
            //_clients.Add(new FtxClient());
            _clients.Add(okxClient);
            _clients.Add(okxUsdClient);
            _clients.Add(byBitLinearClient);
            _clients.Add(byBitInverseClient);
            _clients.Add(byBitPerpClient);
            _clientsByNames = new Dictionary<string, BaseClient>()
            {
                ["BitfinexFunding"] = bitfinexClient,
                ["BitfinexPredicted"] = bitfinexClient,
                ["PhemexFunding"] = phemexClient,
                ["PhemexUsdtFunding"] = phemexV2Client,
                ["HuobiFunding"] = huobiClient,
                ["HuobiPredicted"] = huobiClient,
                ["BinanceFunding"] = binanceClient,
                ["BinanceBUSDFunding"] = binanceBClient,
                ["OkxFunding"] = okxClient,
                ["OkxPredicted"] = okxClient,
                ["OkxUsdFunding"] = okxUsdClient,
                ["OkxUsdPredicted"] = okxUsdClient,
                ["ByBitLinearFunding"] = byBitLinearClient,
                ["ByBitInverseFunding"] = byBitInverseClient,
                ["ByBitPerpFunding"] = byBitPerpClient
            };
        }

        private async Task UpdateData()
        {
            _data = new List<TableData>();
            var tasks = _clients.Select(obj => obj.GetTableDataAsync(SymbolProvider.GetSymbolNames())).ToList();
            await Task.WhenAll(tasks);
            _data.AddRange(tasks.SelectMany(task => task.Result));

            DataToRows(_data);
        }

        private List<Notification> Notify()
        {
            var res = new List<Notification>();

            var groups = _data.GroupBy(d => d.Symbol);
            groups = groups.Where(g => !_notificationSettings.IgnoredSymbolNames.Contains(g.Key));
            var symbolsUsedInNotificationGroups = _notificationSettings.Groups.SelectMany(g => g.Symbols).ToList();
            groups = groups.Where(g => !symbolsUsedInNotificationGroups.Contains(g.Key));

            foreach (var group in groups)
            {
                // TODO: there's room for optimisation and cleaner code here
                if(group.Count(d => d.FundingRate > -90f) >= 2)
                {
                    var lowestFunding = group.Where(d => d.FundingRate > -90f).Aggregate((a, b) => a.FundingRate < b.FundingRate ? a : b);
                    var highestFunding = group.Where(d => d.FundingRate > -90f).Aggregate((a, b) => a.FundingRate > b.FundingRate ? a : b);

                    var cond1 = highestFunding.FundingRate != -100f && lowestFunding.FundingRate != -100f &&
                                highestFunding.FundingRate != -99f && lowestFunding.FundingRate != -99f;
                    var diff1 = highestFunding.FundingRate - lowestFunding.FundingRate;
                    var cond2 = diff1 > _notificationSettings.DefaultStandardDifference;

                    var playSound = diff1 > _notificationSettings.DefaultSoundDifference && !_notificationSettings.MutedSymbolNames.Contains(highestFunding.Symbol);
                    if (cond1 && cond2)
                    {
                        res.Add(new Notification(highestFunding.Symbol, highestFunding.MarketName, lowestFunding.MarketName, false, diff1, playSound));
                    }
                }

                if(group.Count(d => d.PredictedFunding > -90f) >= 2)
                {
                    var lowestPredicted = group.Where(d => d.PredictedFunding > -90f).Aggregate((a, b) => a.PredictedFunding < b.PredictedFunding ? a : b);
                    var highestPredicted = group.Where(d => d.PredictedFunding > -90f).Aggregate((a, b) => a.PredictedFunding > b.PredictedFunding ? a : b);

                    var cond3 = highestPredicted.PredictedFunding != -100f && lowestPredicted.PredictedFunding != -100f &&
                                highestPredicted.PredictedFunding != -99f && lowestPredicted.PredictedFunding != -99f;
                    var diff2 = highestPredicted.PredictedFunding - lowestPredicted.PredictedFunding;
                    var cond4 = diff2 > _notificationSettings.DefaultStandardDifference;

                    var playSound = diff2 > _notificationSettings.DefaultSoundDifference && !_notificationSettings.MutedSymbolNames.Contains(highestPredicted.Symbol);
                    if (cond3 && cond4)
                    {
                        res.Add(new Notification(highestPredicted.Symbol, highestPredicted.MarketName, lowestPredicted.MarketName, true, diff2, playSound));
                    }
                }
            }

            var other = _data.GroupBy(d => d.Symbol);
            other = other.Where(g => symbolsUsedInNotificationGroups.Contains(g.Key));
            other = other.Where(g => !_notificationSettings.IgnoredSymbolNames.Contains(g.Key));

            // iterate over settings groups
            foreach (var group in other)
            {
                foreach(var nGroup in _notificationSettings.Groups)
                {
                    var diff = nGroup.StandardDifference;
                    var soundDiff = nGroup.SoundDifference;

                    if (group.Count(d => d.FundingRate > -90f) >= 2)
                    {
                        var lowestFunding = group.Where(d => d.FundingRate > -90f).Aggregate((a, b) => a.FundingRate < b.FundingRate ? a : b);
                        var highestFunding = group.Where(d => d.FundingRate > -90f).Aggregate((a, b) => a.FundingRate > b.FundingRate ? a : b);

                        var cond1 = highestFunding.FundingRate != -100f && lowestFunding.FundingRate != -100f &&
                                    highestFunding.FundingRate != -99f && lowestFunding.FundingRate != -99f;
                        var diff1 = highestFunding.FundingRate - lowestFunding.FundingRate;
                        var cond2 = diff1 > diff;

                        var playSound = diff1 > soundDiff && !_notificationSettings.MutedSymbolNames.Contains(highestFunding.Symbol);
                        if (cond1 && cond2)
                        {
                            res.Add(new Notification(highestFunding.Symbol, highestFunding.MarketName, lowestFunding.MarketName, false, diff1, playSound));
                        }
                    }

                    if (group.Count(d => d.PredictedFunding > -90f) >= 2)
                    {
                        var lowestPredicted = group.Where(d => d.PredictedFunding > -90f).Aggregate((a, b) => a.PredictedFunding < b.PredictedFunding ? a : b);
                        var highestPredicted = group.Where(d => d.PredictedFunding > -90f).Aggregate((a, b) => a.PredictedFunding > b.PredictedFunding ? a : b);

                        var cond3 = highestPredicted.PredictedFunding != -100f && lowestPredicted.PredictedFunding != -100f &&
                                    highestPredicted.PredictedFunding != -99f && lowestPredicted.PredictedFunding != -99f;
                        var diff2 = highestPredicted.PredictedFunding - lowestPredicted.PredictedFunding;
                        var cond4 = diff2 > diff;

                        var playSound = diff2 > soundDiff && !_notificationSettings.MutedSymbolNames.Contains(highestPredicted.Symbol);
                        if (cond3 && cond4)
                        {
                            res.Add(new Notification(highestPredicted.Symbol, highestPredicted.MarketName, lowestPredicted.MarketName, true, diff2, playSound));
                        }
                    }
                }
            }

            return res;
        }

        public void CheckNotifications()
        {
            var notifications = Notify();
            var noSoundCount = notifications.Where(n => !n.Sound).Count();
            var soundCount = notifications.Count - noSoundCount;

            var t = (menuStrip1.Items["powiadomieniaToolStripMenuItem"] as ToolStripMenuItem)!;
            t.Text = "Powiadomienia " + (notifications.Any() ? $"({notifications.Count})" : "");
            t.DropDown.Items[1].Image = notifications.Any() ? new Bitmap(Resources.bell_full) : new Bitmap(Resources.bell_empty);

            if (notifications.Any())
            {
                t.Font = new Font(t.Font, FontStyle.Bold);
            }
            else
            {
                t.Font = new Font(t.Font, FontStyle.Regular);
            }

            if (soundCount > 0)
            {
                var player = new SoundPlayer(@"notification.wav");
                player.Play();
                this.FlashNotification();
            }
        }

        public async Task UpdateTable()
        {
            Text = "Odświeżanie...";
            await UpdateData();
            RefreshTable();
            CheckNotifications();
            _lastRefresh = DateTime.Now;
        }

        public void GoToRow(string symbol)
        {
            var rowInd = _rows.FindIndex(x => x.Symbol == symbol);
            dataGridView1.CurrentCell = dataGridView1[0, rowInd];
        }

    }

}
