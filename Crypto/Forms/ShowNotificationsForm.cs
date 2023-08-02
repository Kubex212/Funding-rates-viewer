using Crypto.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crypto.Forms
{
    public partial class ShowNotificationsForm : Form
    {
        private static int _symbolCount = 15;
        private IEnumerable<string> _symbols;
        public NotificationSettings Settings { get; set; }
        public List<Notification> Notifications { get; set; }
        public int? IgnoreTime { get => minutesUpDown.Enabled ? (int)minutesUpDown.Value : null; }
        public List<string> Symbols { get; set; }
        public string? GoToSymbol { get => !_symbols.Contains(goToBox.Text) ? null : goToBox.Text; }

        public ShowNotificationsForm(NotificationSettings notificationsSettings, List<Notification> notifications)
        {
            InitializeComponent();

            Settings = notificationsSettings;
            Notifications = notifications;
            _symbols = notifications.Select(n => n.Name).Distinct();

            var messageSb = new StringBuilder();
            foreach (var notification in notifications.OrderBy(n => -n.Difference).Take(_symbolCount))
            {
                messageSb.AppendLine($"{notification.Name}: różnica {notification.Difference * 100:0.000} na giełdach" +
                    $" {notification.Prop1} i {notification.Prop2} ({(notification.Predicted ? "predicted" : "funding")})");
            }
            if (notifications.Count > _symbolCount)
            {
                messageSb.AppendLine($"i {notifications.Count - 10} innych...");
            }

            if(!notifications.Any())
            {
                messageSb.AppendLine("Brak powiadomień");
                richTextBox.Text = messageSb.ToString();
            }
            else
            {
                richTextBox.Text = messageSb.ToString();
                for (int i = 0; i < richTextBox.Lines.Length; i++)
                {
                    var line = richTextBox.Lines[i];

                    // Find the index of the first space character (word delimiter)
                    var index = line.IndexOf(':');

                    if (index >= 0 && notifications[i].Sound)
                    {
                        // Set the first word in bold
                        richTextBox.Select(richTextBox.GetFirstCharIndexFromLine(i), index);
                        richTextBox.SelectionFont = new Font(richTextBox.Font, FontStyle.Bold);
                    }
                }
                richTextBox.DeselectAll();
            }
            goToBox.Items.AddRange(_symbols.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(radioButton3.Checked)
            {
                Close();
            }
            DialogResult = DialogResult.OK;
            Symbols = Notifications.Select(n => n.Name).Take(_symbolCount).Distinct().ToList();
            if(radioButton1.Checked)
            {
                Settings.IgnoredSymbolNames.AddRange(Symbols);
            }
            else if(radioButton2.Checked)
            {
                Settings.MutedSymbolNames.AddRange(Symbols);
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            minutesUpDown.Enabled = radioButton1.Checked;
            label1.Enabled = radioButton1.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            minutesUpDown.Enabled = radioButton2.Checked;
            label1.Enabled = radioButton2.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            minutesUpDown.Enabled = !radioButton3.Checked;
            label1.Enabled = !radioButton3.Checked;
        }
    }
}
