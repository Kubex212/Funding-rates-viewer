using System.ComponentModel;
using Crypto.Objects;
using Crypto.Utility;

namespace Crypto.Forms
{
    public partial class EditSymbolsForm : Form
    {
        public EditSymbolsForm()
        {
            InitializeComponent();

            var bindingList = new BindingList<Symbol>(SymbolProvider.GetSymbols());
            var source = new BindingSource(bindingList, null);
            dataGridView1.DataSource = source;

            DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
            editButtonColumn.Name = "Modyfikuj";
            editButtonColumn.HeaderText = "Modyfikuj";
            int columnIndex = 8;
            if (dataGridView1.Columns["Modyfikuj"] == null)
            {
                dataGridView1.Columns.Insert(columnIndex, editButtonColumn);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Modyfikuj"].Index)
            {
                MessageBox.Show("test");
            }
        }
    }
}
