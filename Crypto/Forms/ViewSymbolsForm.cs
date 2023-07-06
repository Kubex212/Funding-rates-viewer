using System.ComponentModel;
using Crypto.Objects;
using Crypto.Utility;

namespace Crypto.Forms
{
    public partial class ViewSymbolsForm : Form
    {
        bool _madeChanges = false;
        List<LabeledTableData> _data = new List<LabeledTableData>();
        List<LabeledTableData> _specials = new List<LabeledTableData>();
        public ViewSymbolsForm(List<LabeledTableData> data)
        {
            InitializeComponent();
            Width = 1000;
            _data = data;
            _specials = GetSpecials();

            var bindingList = new BindingList<Symbol>(SymbolProvider.GetSymbols());
            var source = new BindingSource(bindingList, null);
            dataGridView1.DataSource = source;

            DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
            dataGridView1.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["Name"].MinimumWidth = 50;
            editButtonColumn.Name = "Modyfikuj";
            editButtonColumn.HeaderText = "Modyfikuj";
            editButtonColumn.MinimumWidth = 80;
            int columnIndex = 12;
            if (dataGridView1.Columns["Modyfikuj"] == null)
            {
                dataGridView1.Columns.Insert(columnIndex, editButtonColumn);
            }
        }

        private void ViewSymbolsForm_Load(object sender, EventArgs e)
        {
            MarkSpecials();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Modyfikuj"].Index)
            {
                var symbol = SymbolProvider.GetSymbols().Find(s => s.Name == (string)dataGridView1[1, e.RowIndex].Value);

                if(symbol != null)
                {
                    var symbol2 = (Symbol)symbol.Clone();

                    var form = new EditSymbolForm(symbol, _specials);
                    form.ShowDialog();

                    if(!symbol.Equals(symbol2))
                    {
                        _madeChanges = true;
                    }

                    var bindingList = new BindingList<Symbol>(SymbolProvider.GetSymbols());
                    var source = new BindingSource(bindingList, null);
                    dataGridView1.DataSource = source;
                    MarkSpecials();
                }
            }
        }

        private void ViewSymbolsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(_madeChanges)
            {
                if (SymbolProvider.SaveToFile())
                {
                    MessageBox.Show("Pomyślnie zapisano zmiany!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Wystąpił błąd podczas zapisywania do pliku! Jeśli to widzisz, to daj mi znać.", "Poważny błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private List<LabeledTableData> GetSpecials()
        {
            _specials = new List<LabeledTableData>();
            foreach(var d in _data)
            {
                //if (d.Symbol == "GOLD-USDT") MessageBox.Show($"{d.FundingRate}");
                if ((d.FundingRate == -100f && d.PredictedFunding == -100f) || (d.FundingRate == -99f && d.PredictedFunding == -99f))
                {
                    _specials.Add(d);
                }
            }
            return _specials;
        }

        private void MarkSpecials()
        {
            var specials = GetSpecials();

            var reds = specials.Where(d => d.PredictedFunding == -100f);
            var browns = specials.Where(d => d.PredictedFunding == -99f);
            foreach(var data in specials)
            {
                var row = -1;
                foreach(DataGridViewRow r in dataGridView1.Rows)
                {
                    if (r.DataBoundItem is Symbol symbol && symbol.Name == data.CoinName)
                    {
                        row = r.Index;
                        break;
                    }
                }
                if(row == -1)
                    throw new Exception("nie znalazło symbolu...");

                var col = dataGridView1.Columns[data.MarketName].Index;
                var cell = dataGridView1[col, row];
                var style = new DataGridViewCellStyle();

                if (data.FundingRate == -99f || data.PredictedFunding == -99f) 
                    style.BackColor = Color.DarkOrange;
                else
                    style.BackColor = Color.DarkRed;
                cell.Style = style;
            }

            dataGridView1.Update();
            dataGridView1.Refresh();
        }

        private void ViewSymbolsForm_Paint(object sender, PaintEventArgs e)
        {
            MarkSpecials();
        }
    }
}
