using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crypto.Clients.Phemex;
using Crypto.Objects;

namespace Crypto
{
    public partial class FundingTableForm : Form
    {
        List<string> _symbols;
        List<TableData> _data;
        List<(Color, Color)> _colors;
        int[] _clicks;
        SortMode _sort;
        double _redMax;
        double _greenMin;

        public FundingTableForm(List<string> symbols, int refresh, double redMax, double greenMin)
        {
            InitializeComponent();
            _symbols = symbols;
            _clicks = new int[4];
            _redMax = redMax;
            _greenMin = greenMin;
            timer1.Interval = refresh * 1000;
            PhemexClient.InitializeClient();

            InitTable(symbols);
        }

        async void InitTable(List<string> symbols)
        {
            PhemexClient.InitializeClient();
            PhemexClient client = new PhemexClient();
            _data = await client.GetTableDataAsync(symbols);
            var bindingList = new BindingList<TableData>(_data);
            var source = new BindingSource(bindingList, null);
            dataGridView1.DataSource = source;

            dataGridView1.Columns["Name"].Visible = false;
            dataGridView1.Columns["Symbol"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["FundingRate"].DefaultCellStyle.Format = "0.0000%";
            dataGridView1.Columns["PredictedFunding"].DefaultCellStyle.Format = "0.0000%";
            SetColors();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            PhemexClient client = new PhemexClient();
            _data = await client.GetTableDataAsync(_symbols);
            UpdateTable();
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _clicks[e.ColumnIndex]++;
            if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "FundingRate")
            {
                if (_clicks[e.ColumnIndex]%2 == 0) _sort = SortMode.FundingDescending;
                else _sort = SortMode.FundingAscending;
            }
            else if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "PredictedFunding")
            {
                if (_clicks[e.ColumnIndex] % 2 == 0) _sort = SortMode.PredictedDescending;
                else _sort = SortMode.PredictedAscending;
            }
            UpdateTable();
        }

        private void UpdateTable()
        {
            Sort();
            var bindingList = new BindingList<TableData>(_data);
            var source = new BindingSource(bindingList, null);
            dataGridView1.DataSource = source;

            SetColors();
            dataGridView1.Update();
            dataGridView1.Refresh();
        }

        private void Sort()
        {
            if(_sort == SortMode.FundingDescending)
            {
                _data = _data.OrderBy(d => -d.FundingRate).ToList();
            }
            else if(_sort == SortMode.FundingAscending)
            {
                _data = _data.OrderBy(d => d.FundingRate).ToList();
            }
            else if (_sort == SortMode.PredictedAscending)
            {
                _data = _data.OrderBy(d => d.PredictedFunding).ToList();
            }
            else if (_sort == SortMode.PredictedDescending)
            {
                _data = _data.OrderBy(d => -d.PredictedFunding).ToList();
            }
        }

        private void SetColors()
        {
            var fundingColumns = dataGridView1.Columns["FundingRate"];
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var c = row.Cells["FundingRate"];
                var val = c.Value;
                DataGridViewCellStyle style = new DataGridViewCellStyle();
                if ((float)val * 100 >= _greenMin) style.BackColor = Color.Green;
                else if ((float)val * 100 <= _redMax) style.BackColor = Color.Red;
                else style.BackColor = Color.White;
                c.Style = style;

                c = row.Cells["PredictedFunding"];
                val = c.Value;
                style = new DataGridViewCellStyle();
                if ((float)val * 100 >= _greenMin) style.BackColor = Color.Green;
                else if ((float)val * 100 <= _redMax) style.BackColor = Color.Red;
                else style.BackColor = Color.White;
                c.Style = style;
            }
        }

        private void FundingTableForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }

    public enum SortMode
    {
        FundingAscending,
        FundingDescending,
        PredictedAscending,
        PredictedDescending,
    }
}
