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
using WebScraper.Services;
using System.Reflection;
using Crypto.Properties;
using System.Runtime.InteropServices;
using System.Media;

namespace Crypto.Forms
{
    public partial class TableForm : Form
    {
        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _sort = (e.ColumnIndex, _sort.ascending ? false : true);
            RefreshTable();
        }

        private async void odświeżToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // it also resets elapsed time of the timer
            refreshTimer.Stop();
            await UpdateTable();
            refreshTimer.Start();
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
            colorDialog.Color = Properties.Settings.Default.ColorHigh;

            if (colorDialog.ShowDialog() == DialogResult.OK)
                Properties.Settings.Default.ColorHigh = colorDialog.Color;
            Properties.Settings.Default.Save();
        }

        private void kolorPoniżejToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = true;
            colorDialog.ShowHelp = true;
            colorDialog.Color = Properties.Settings.Default.ColorLow;

            if (colorDialog.ShowDialog() == DialogResult.OK)
                Properties.Settings.Default.ColorLow = colorDialog.Color;
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
            colorDialog.Color = Properties.Settings.Default.ColorEmpty;

            if (colorDialog.ShowDialog() == DialogResult.OK)
                Properties.Settings.Default.ColorEmpty = colorDialog.Color;
            Properties.Settings.Default.Save();
        }

        private void dodajSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var window = new AddSymbolForm();
            window.ShowDialog();
        }

        private async void pobierzSymboleZBinanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var binanceNames = await BinanceClient.GetSymbolNames();
            var newBinanceNames = binanceNames.Where(n => !Names.Contains(n)).ToList();
            var namesUnknownForBinance = Names.Where(n => !binanceNames.Contains(n)).ToList();

            foreach (var name in namesUnknownForBinance)
            {
                Symbols.Single(s => s.Name == name).Binance = "?";
            }


            var phemexUsdNames = Utility.SymbolProvider.ReadFile("phemexUSD.txt", "USD");
            var newPhemexUsdNames = phemexUsdNames.Where(n => !Names.Contains(n)).ToList();
            var namesUnknownForPhemexUsd = Names.Where(n => !phemexUsdNames.Contains(n)).ToList();

            foreach (var name in namesUnknownForPhemexUsd)
            {
                //_symbols.Single(s => s.Name == name).Phemex = "?";
            }

            var phemexUsdtNames = SymbolProvider.ReadFile("phemexUSDT.txt", "USDT");
            var newPhemexUsdtNames = phemexUsdNames.Where(n => !Names.Contains(n)).ToList();
            var namesUnknownForPhemexUsdt = Names.Where(n => !phemexUsdNames.Contains(n)).ToList();

            foreach (var name in namesUnknownForPhemexUsd)
            {
                // _symbols.Single(s => s.Name == name).PhemexUsdt = "?";
            }

            var newNames = new List<string>(newBinanceNames);
            newNames.AddRange(newPhemexUsdNames);
            newNames.AddRange(newPhemexUsdtNames);
            newNames = newNames.Distinct().ToList();

            SymbolProvider.AddSymbols(newNames.ToArray());

            var bSymbolsCount = SymbolProvider.FixBinanceBSymbols(true);

            MessageBox.Show($"Binance zwróciła {binanceNames.Count} symboli (w tym {bSymbolsCount} b-symboli), z czego {newBinanceNames.Count}" +
                $" było wcześniej nieznanych.\n" +
                $"Phemex zwróciła {phemexUsdNames.Count} symboli, z czego {newPhemexUsdNames.Count}" +
                $" było wcześniej nieznanych.\n\n" +
                $"Łącznie dodano {newNames.Count - bSymbolsCount} symboli.",
                "Wynik aktualizacji",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ustawieniaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (var form = new NotificationForm(_notificationSettings))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _notificationSettings = form.Settings;
                    Settings.Default.NotificationsDifference = form.Settings.StandardDifference;
                    Settings.Default.NotificationSoundDifference = form.Settings.SoundDifference;
                    Settings.Default.Save();
                }
            }
        }

        private void pokażToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var notifications = Notify();

            var messageSb = new StringBuilder();
            foreach (var notification in notifications.OrderBy(n => -n.Difference).Take(10))
            {
                messageSb.AppendLine($"{notification.Name}: różnica {notification.Difference * 100:0.000} na giełdach" +
                    $" {notification.Prop1} i {notification.Prop2} ({(notification.Predicted ? "predicted" : "funding")})");
            }
            if (notifications.Count > 10)
            {
                messageSb.AppendLine($"i {notifications.Count - 10} innych...");
            }

            if (notifications.Any())
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
            refreshTimer.Stop();
            _ = UpdateTable();
            refreshTimer.Start();
        }

        private void powiadomieniaToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            var t = (menuStrip1.Items["powiadomieniaToolStripMenuItem"] as ToolStripMenuItem)!;
            t.Font = new Font(t.Font, FontStyle.Regular);
        }

        private void odświeżajToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = (sender as ToolStripMenuItem)!;
            item.Checked = !item.Checked;
            if(item.Checked)
            {
                refreshTimer.Start();
            }
            else
            {
                refreshTimer.Stop();
            }
        }

        private void ignorujXWPowiadomieniachToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: make this in a more elegant way?
            var item = (ToolStripMenuItem)sender;
            var symbolName = item.Text.Split()[1];
            _notificationSettings.IgnoredSymbolNames.Add(symbolName);
        }

        private void usuńXZIgnorowanychToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: make this in a more elegant way?
            var item = (ToolStripMenuItem)sender;
            var symbolName = item.Text.Split()[1];
            _notificationSettings.IgnoredSymbolNames.Remove(symbolName);
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DataGridViewCell c = (sender as DataGridView)![e.ColumnIndex, e.RowIndex];
                var symbol = _rows[e.RowIndex].Symbol;

                tableContextMenu.Items[0].Text = $"Ignoruj {symbol} w powiadomieniach";
                tableContextMenu.Items[1].Text = $"Usuń {symbol} z ignorowanych";

                var isIgnored = _notificationSettings.IgnoredSymbolNames.Contains(symbol);
                if (isIgnored)
                {
                    tableContextMenu.Items[0].Enabled = false;
                    tableContextMenu.Items[1].Enabled = true;
                }
                else
                {
                    tableContextMenu.Items[0].Enabled = true;
                    tableContextMenu.Items[1].Enabled = false;
                }
            }
        }
    }

    public static class ExtensionMethods
    {
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        private const uint FLASHW_ALL = 3;

        private const uint FLASHW_TIMERNOFG = 12;

        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            public uint cbSize;
            public IntPtr hwnd;
            public uint dwFlags;
            public uint uCount;
            public uint dwTimeout;
        }

        public static bool FlashNotification(this Form form)
        {
            IntPtr hWnd = form.Handle;
            FLASHWINFO fInfo = new FLASHWINFO();

            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = hWnd;
            fInfo.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
            fInfo.uCount = uint.MaxValue;
            fInfo.dwTimeout = 0;

            return FlashWindowEx(ref fInfo);
        }
    }
}
