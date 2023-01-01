using System.ComponentModel;
using Crypto.Objects;
using Crypto.Utility;

namespace Crypto.Forms
{
    public partial class ViewSymbolsForm : Form
    {
        bool _madeChanges = false;
        List<TableData> _data = new List<TableData>();
        List<TableData> _specials = new List<TableData>();
        public ViewSymbolsForm(List<TableData> data)
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
            editButtonColumn.Name = "Modyfikuj";
            editButtonColumn.HeaderText = "Modyfikuj";
            int columnIndex = 8;
            if (dataGridView1.Columns["Modyfikuj"] == null)
            {
                dataGridView1.Columns.Insert(columnIndex, editButtonColumn);
            }
            dataGridView1.Columns["Modyfikuj"].Width = 50;
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
                    Symbol? symbol2 = (Symbol?)symbol.Clone();

                    var form = new EditSymbolForm(symbol, _specials);
                    form.ShowDialog();

                    if(!symbol.Equals(symbol2)) _madeChanges = true;

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

        private List<TableData> GetSpecials()
        {
            _specials = new List<TableData>();
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
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    if (col.Name == "Name") continue;
                    var cell = dataGridView1[col.Index, row.Index];
                    var cellText = (string)cell.Value;
                    //if (cellText == "GOLD-USDT") MessageBox.Show("");
                    TableData? data = null;
                    try 
                    {
                        if ((data = specials.Find(d => d.Symbol == cellText)) != null)
                        {
                            DataGridViewCellStyle style = new DataGridViewCellStyle();
                            if (data.FundingRate == -99f || data.PredictedFunding == -99f) style.BackColor = Color.DarkOrange;
                            else style.BackColor = Color.DarkRed;
                            cell.Style = style;
                        }
                    }
                    catch(ArgumentException ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                }
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
