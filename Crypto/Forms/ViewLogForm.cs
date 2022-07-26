using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crypto.Utility;

namespace Crypto.Forms
{
    public partial class ViewLogForm : Form
    {
        public ViewLogForm()
        {
            InitializeComponent();

            int length = 0;
            foreach(var log in Logger.Logs)
            {
                richTextBox1.AppendText(log.msg);
                richTextBox1.Select(length, log.msg.Length);
                length += log.msg.Length;
                if (log.type == Utility.Type.Warning) richTextBox1.SelectionColor = Color.DarkOrange;
                else if (log.type == Utility.Type.Error) richTextBox1.SelectionColor = Color.Red;
            }
            richTextBox1.Select(0,0);
        }
    }
}
