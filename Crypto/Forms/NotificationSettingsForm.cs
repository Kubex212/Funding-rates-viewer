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
    public partial class NotificationSettingsForm : Form
    {
        private List<GroupBox> _groupBoxes;
        private int _groupBoxHeight = 120;
        public NotificationSettings Settings { get; set; }
        public NotificationSettingsForm(NotificationSettings settings)
        {
            _groupBoxes = new List<GroupBox>();
            Settings = settings;

            InitializeComponent();

            numericUpDown1.Value = (decimal)settings.DefaultStandardDifference * 100;
            numericUpDown2.Value = (decimal)settings.DefaultSoundDifference * 100;

            ignoredTb.Text = string.Join(";", settings.IgnoredSymbolNames);
            mutedTb.Text = string.Join(";", settings.MutedSymbolNames);

            Icon = Properties.Resources.btc;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Settings.DefaultStandardDifference = (double)numericUpDown1.Value / 100.0;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            Settings.DefaultSoundDifference = (double)numericUpDown2.Value / 100.0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var newGroup = new NotificationGroup();
            var groupBox = CreateNotificationGroup(Settings.Groups.Count + 1, newGroup);
            flowLayoutPanel.Controls.Add(groupBox);
            _groupBoxes.Add(groupBox);
            Settings.Groups.Add(newGroup);
            AdjustFormSize();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void noSoundTb_TextChanged(object sender, EventArgs e)
        {
            Settings.MutedSymbolNames = mutedTb.Text.Split(";").ToList();
        }

        private void ignoredTb_TextChanged(object sender, EventArgs e)
        {
            Settings.IgnoredSymbolNames = ignoredTb.Text.Split(";").ToList();
        }

        private GroupBox CreateNotificationGroup(int index, NotificationGroup group)
        {
            var newGroupBox = new GroupBox();
            newGroupBox.Text = $"Grupa {index}";
            newGroupBox.Size = new Size(320, _groupBoxHeight);

            var symbolsLabel = new Label();
            symbolsLabel.Text = "Symbole";
            symbolsLabel.Location = new Point(5, 25);

            var symbolsTextBox = new TextBox();
            symbolsTextBox.Text = string.Join(";", group.Symbols);
            symbolsTextBox.Location = new Point(155, 20);
            symbolsTextBox.TextChanged += dynamicTextBox_TextChanged!;
            symbolsTextBox.Size = new Size(160, 23);

            var diffLabel = new Label();
            diffLabel.Text = "Różnica bez dźwięku";
            diffLabel.Location = new Point(5, 60);
            diffLabel.Size = new Size(150, 25);

            var diffUpDown = new NumericUpDown();
            diffUpDown.Value = (decimal)group.StandardDifference * 100;
            diffUpDown.Minimum = 0;
            diffUpDown.Maximum = 10;
            diffUpDown.Increment = 0.005m;
            diffUpDown.Location = new Point(155, 55);
            diffUpDown.DecimalPlaces = 3;
            diffUpDown.ValueChanged += dynamicNumericUpDown_ValueChanged!;

            var soundDiffLabel = new Label();
            soundDiffLabel.Text = "Różnica z dźwiękiem";
            soundDiffLabel.Location = new Point(5, 95);
            soundDiffLabel.Size = new Size(150, 25);

            var soundDiffUpDown = new NumericUpDown();
            soundDiffUpDown.Value = (decimal)group.SoundDifference * 100;
            soundDiffUpDown.Minimum = 0;
            soundDiffUpDown.Maximum = 20;
            soundDiffUpDown.Increment = 0.005m;
            soundDiffUpDown.Location = new Point(155, 90);
            soundDiffUpDown.DecimalPlaces = 3;
            soundDiffUpDown.ValueChanged += dynamicNumericUpDown_ValueChanged!;

            var removeButton = new Button();
            //removeButton.Text = "×";
            removeButton.Font = new Font(removeButton.Font.FontFamily, 12, FontStyle.Regular);
            removeButton.Size = new Size(25, 25);
            removeButton.Location = new Point(290, 89);
            removeButton.Click += dynamicRemoveGroupButton_Click!;
            var resizedImage = Properties.Resources.CancelIcon.GetThumbnailImage(removeButton.Width, removeButton.Height, null, IntPtr.Zero);
            removeButton.Image = resizedImage;

            newGroupBox.Controls.Add(symbolsLabel);
            newGroupBox.Controls.Add(symbolsTextBox);
            newGroupBox.Controls.Add(diffLabel);
            newGroupBox.Controls.Add(diffUpDown);
            newGroupBox.Controls.Add(soundDiffLabel);
            newGroupBox.Controls.Add(soundDiffUpDown);
            newGroupBox.Controls.Add(removeButton);

            return newGroupBox;
        }

        private void dynamicTextBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            var groupIndex = GetIndex(sender);
            var symbols = textBox.Text.Split(";").ToList();
            Settings.Groups[groupIndex].Symbols = symbols;
        }

        private void dynamicNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            var numericUpDown = (NumericUpDown)sender;
            var groupIndex = GetIndex(sender);
            var forSound = numericUpDown.Maximum == 20;
            if (forSound)
            {
                Settings.Groups[groupIndex].SoundDifference = (double)numericUpDown.Value / 100;
            }
            else
            {
                Settings.Groups[groupIndex].StandardDifference = (double)numericUpDown.Value / 100;
            }
        }

        private void dynamicRemoveGroupButton_Click(object sender, EventArgs e)
        {
            var groupIndex = GetIndex(sender);
            var groupBox = _groupBoxes[groupIndex];
            flowLayoutPanel.Controls.Remove(groupBox);
            _groupBoxes.RemoveAt(groupIndex);
            Settings.Groups.RemoveAt(groupIndex);
            AdjustFormSize();
            AdjustGroupBoxTexts();
        }

        private int GetIndex(object obj)
        {
            var control = (Control)obj;
            return _groupBoxes.IndexOf(_groupBoxes.Single(g => g.Controls.Contains(control)));
        }

        private void AdjustFormSize()
        {
            var groupCount = _groupBoxes.Count;
            var height = settingsGroupBox.Margin.Top + settingsGroupBox.Size.Height + settingsGroupBox.Margin.Bottom
                + (3 + _groupBoxHeight + 3) * groupCount
                + button1.Margin.Top + button1.Size.Height + button1.Margin.Bottom + 70;
            Size = new Size(Size.Width, height);
        }

        private void NotificationSettingsForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < Settings.Groups.Count; i++)
            {
                var group = Settings.Groups[i];
                var groupBox = CreateNotificationGroup(i + 1, group);
                _groupBoxes.Add(groupBox);
                flowLayoutPanel.Controls.Add(groupBox);
            }
            AdjustFormSize();
        }

        private void AdjustGroupBoxTexts()
        {
            for (int i = 0; i < _groupBoxes.Count; i++)
            {
                var group = _groupBoxes[i];
                group.Text = $"Grupa {i + 1}";
            }
        }
    }
}
