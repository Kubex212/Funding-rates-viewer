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

namespace Crypto.Forms
{
    public partial class NotificationForm : Form
    {
        public NotificationSettings Settings { get; set; }
        public NotificationForm(NotificationSettings settings)
        {
            Settings = settings;

            InitializeComponent();

            radioButton1.Checked = settings.Type == NotificationType.All;
            radioButton2.Checked = settings.Type == NotificationType.Specified;
            radioButton3.Checked = settings.Type == NotificationType.AllExcept;

            numericUpDown1.Value = (decimal)settings.Difference;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                allTB.Enabled = false;
                exceptTB.Enabled = false;

                Settings.Type = NotificationType.All;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                allTB.Enabled = true;
                exceptTB.Enabled = false;

                Settings.Type = NotificationType.Specified;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                allTB.Enabled = false;
                exceptTB.Enabled = true;

                Settings.Type = NotificationType.AllExcept;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Settings.Difference = (double)numericUpDown1.Value / 100.0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
