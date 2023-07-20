namespace Crypto.Forms
{
    partial class TableForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableForm));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tableContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ignorujXWPowiadomieniachToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usuńXZIgnorowanychToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.konsolaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pokażCałośćToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jakoMessageBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jakoRichTextBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabelkaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.odświeżToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.odświeżajToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wartościToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimalnaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maksymalnaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.symboleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modyfikujToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dodajSymbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pobierzSymboleZBinanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ustawieniaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kolorPowyżejToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kolorPoniżejToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kolorBłęduToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kolorNieobsługiwanegoSymboluToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.powiadomieniaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ustawieniaToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pokażToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableContextMenu.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.tableContextMenu;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 24);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(1492, 453);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            // 
            // tableContextMenu
            // 
            this.tableContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ignorujXWPowiadomieniachToolStripMenuItem,
            this.usuńXZIgnorowanychToolStripMenuItem});
            this.tableContextMenu.Name = "tableContextMenu";
            this.tableContextMenu.Size = new System.Drawing.Size(232, 70);
            // 
            // ignorujXWPowiadomieniachToolStripMenuItem
            // 
            this.ignorujXWPowiadomieniachToolStripMenuItem.Name = "ignorujXWPowiadomieniachToolStripMenuItem";
            this.ignorujXWPowiadomieniachToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.ignorujXWPowiadomieniachToolStripMenuItem.Text = "Ignoruj x w powiadomieniach";
            this.ignorujXWPowiadomieniachToolStripMenuItem.Click += new System.EventHandler(this.ignorujXWPowiadomieniachToolStripMenuItem_Click);
            // 
            // usuńXZIgnorowanychToolStripMenuItem
            // 
            this.usuńXZIgnorowanychToolStripMenuItem.Name = "usuńXZIgnorowanychToolStripMenuItem";
            this.usuńXZIgnorowanychToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.usuńXZIgnorowanychToolStripMenuItem.Text = "Usuń x z ignorowanych";
            this.usuńXZIgnorowanychToolStripMenuItem.Click += new System.EventHandler(this.usuńXZIgnorowanychToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.konsolaToolStripMenuItem,
            this.tabelkaToolStripMenuItem,
            this.symboleToolStripMenuItem,
            this.ustawieniaToolStripMenuItem,
            this.powiadomieniaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1492, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // konsolaToolStripMenuItem
            // 
            this.konsolaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pokażCałośćToolStripMenuItem});
            this.konsolaToolStripMenuItem.Name = "konsolaToolStripMenuItem";
            this.konsolaToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.konsolaToolStripMenuItem.Text = "Konsola";
            // 
            // pokażCałośćToolStripMenuItem
            // 
            this.pokażCałośćToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jakoMessageBoxToolStripMenuItem,
            this.jakoRichTextBoxToolStripMenuItem});
            this.pokażCałośćToolStripMenuItem.Name = "pokażCałośćToolStripMenuItem";
            this.pokażCałośćToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.pokażCałośćToolStripMenuItem.Text = "Pokaż całość";
            // 
            // jakoMessageBoxToolStripMenuItem
            // 
            this.jakoMessageBoxToolStripMenuItem.Name = "jakoMessageBoxToolStripMenuItem";
            this.jakoMessageBoxToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.jakoMessageBoxToolStripMenuItem.Text = "Jako MessageBox";
            this.jakoMessageBoxToolStripMenuItem.Click += new System.EventHandler(this.jakoMessageBoxToolStripMenuItem_Click);
            // 
            // jakoRichTextBoxToolStripMenuItem
            // 
            this.jakoRichTextBoxToolStripMenuItem.Name = "jakoRichTextBoxToolStripMenuItem";
            this.jakoRichTextBoxToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.jakoRichTextBoxToolStripMenuItem.Text = "Jako RichTextBox";
            this.jakoRichTextBoxToolStripMenuItem.Click += new System.EventHandler(this.jakoRichTextBoxToolStripMenuItem_Click);
            // 
            // tabelkaToolStripMenuItem
            // 
            this.tabelkaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.odświeżToolStripMenuItem,
            this.odświeżajToolStripMenuItem,
            this.wartościToolStripMenuItem});
            this.tabelkaToolStripMenuItem.Name = "tabelkaToolStripMenuItem";
            this.tabelkaToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.tabelkaToolStripMenuItem.Text = "Tabelka";
            // 
            // odświeżToolStripMenuItem
            // 
            this.odświeżToolStripMenuItem.Name = "odświeżToolStripMenuItem";
            this.odświeżToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.odświeżToolStripMenuItem.Text = "Odśwież";
            this.odświeżToolStripMenuItem.Click += new System.EventHandler(this.odświeżToolStripMenuItem_Click);
            // 
            // odświeżajToolStripMenuItem
            // 
            this.odświeżajToolStripMenuItem.Checked = true;
            this.odświeżajToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.odświeżajToolStripMenuItem.Name = "odświeżajToolStripMenuItem";
            this.odświeżajToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.odświeżajToolStripMenuItem.Text = "Odświeżaj";
            this.odświeżajToolStripMenuItem.Click += new System.EventHandler(this.odświeżajToolStripMenuItem_Click);
            // 
            // wartościToolStripMenuItem
            // 
            this.wartościToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.minimalnaToolStripMenuItem,
            this.maksymalnaToolStripMenuItem});
            this.wartościToolStripMenuItem.Name = "wartościToolStripMenuItem";
            this.wartościToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.wartościToolStripMenuItem.Text = "Wartości...";
            // 
            // minimalnaToolStripMenuItem
            // 
            this.minimalnaToolStripMenuItem.Name = "minimalnaToolStripMenuItem";
            this.minimalnaToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.minimalnaToolStripMenuItem.Text = "Minimalna";
            this.minimalnaToolStripMenuItem.Click += new System.EventHandler(this.minimalnaToolStripMenuItem_Click);
            // 
            // maksymalnaToolStripMenuItem
            // 
            this.maksymalnaToolStripMenuItem.Name = "maksymalnaToolStripMenuItem";
            this.maksymalnaToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.maksymalnaToolStripMenuItem.Text = "Maksymalna";
            this.maksymalnaToolStripMenuItem.Click += new System.EventHandler(this.maksymalnaToolStripMenuItem_Click);
            // 
            // symboleToolStripMenuItem
            // 
            this.symboleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modyfikujToolStripMenuItem,
            this.dodajSymbolToolStripMenuItem,
            this.pobierzSymboleZBinanceToolStripMenuItem});
            this.symboleToolStripMenuItem.Name = "symboleToolStripMenuItem";
            this.symboleToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.symboleToolStripMenuItem.Text = "Symbole";
            // 
            // modyfikujToolStripMenuItem
            // 
            this.modyfikujToolStripMenuItem.Name = "modyfikujToolStripMenuItem";
            this.modyfikujToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.modyfikujToolStripMenuItem.Text = "Modyfikuj";
            this.modyfikujToolStripMenuItem.Click += new System.EventHandler(this.modyfikujToolStripMenuItem_Click);
            // 
            // dodajSymbolToolStripMenuItem
            // 
            this.dodajSymbolToolStripMenuItem.Name = "dodajSymbolToolStripMenuItem";
            this.dodajSymbolToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.dodajSymbolToolStripMenuItem.Text = "Dodaj symbol";
            this.dodajSymbolToolStripMenuItem.Click += new System.EventHandler(this.dodajSymbolToolStripMenuItem_Click);
            // 
            // pobierzSymboleZBinanceToolStripMenuItem
            // 
            this.pobierzSymboleZBinanceToolStripMenuItem.Name = "pobierzSymboleZBinanceToolStripMenuItem";
            this.pobierzSymboleZBinanceToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.pobierzSymboleZBinanceToolStripMenuItem.Text = "Aktualizuj symbole";
            this.pobierzSymboleZBinanceToolStripMenuItem.Click += new System.EventHandler(this.pobierzSymboleZBinanceToolStripMenuItem_Click);
            // 
            // ustawieniaToolStripMenuItem
            // 
            this.ustawieniaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kolorPowyżejToolStripMenuItem,
            this.kolorPoniżejToolStripMenuItem,
            this.kolorBłęduToolStripMenuItem,
            this.kolorNieobsługiwanegoSymboluToolStripMenuItem});
            this.ustawieniaToolStripMenuItem.Name = "ustawieniaToolStripMenuItem";
            this.ustawieniaToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            this.ustawieniaToolStripMenuItem.Text = "Ustawienia";
            // 
            // kolorPowyżejToolStripMenuItem
            // 
            this.kolorPowyżejToolStripMenuItem.Name = "kolorPowyżejToolStripMenuItem";
            this.kolorPowyżejToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
            this.kolorPowyżejToolStripMenuItem.Text = "Kolor powyżej";
            this.kolorPowyżejToolStripMenuItem.Click += new System.EventHandler(this.kolorPowyżejToolStripMenuItem_Click);
            // 
            // kolorPoniżejToolStripMenuItem
            // 
            this.kolorPoniżejToolStripMenuItem.Name = "kolorPoniżejToolStripMenuItem";
            this.kolorPoniżejToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
            this.kolorPoniżejToolStripMenuItem.Text = "Kolor poniżej";
            this.kolorPoniżejToolStripMenuItem.Click += new System.EventHandler(this.kolorPoniżejToolStripMenuItem_Click);
            // 
            // kolorBłęduToolStripMenuItem
            // 
            this.kolorBłęduToolStripMenuItem.Name = "kolorBłęduToolStripMenuItem";
            this.kolorBłęduToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
            this.kolorBłęduToolStripMenuItem.Text = "Kolor błędu";
            this.kolorBłęduToolStripMenuItem.Click += new System.EventHandler(this.kolorBłęduToolStripMenuItem_Click);
            // 
            // kolorNieobsługiwanegoSymboluToolStripMenuItem
            // 
            this.kolorNieobsługiwanegoSymboluToolStripMenuItem.Name = "kolorNieobsługiwanegoSymboluToolStripMenuItem";
            this.kolorNieobsługiwanegoSymboluToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
            this.kolorNieobsługiwanegoSymboluToolStripMenuItem.Text = "Kolor nieobsługiwanego symbolu";
            this.kolorNieobsługiwanegoSymboluToolStripMenuItem.Click += new System.EventHandler(this.kolorNieobsługiwanegoSymboluToolStripMenuItem_Click);
            // 
            // powiadomieniaToolStripMenuItem
            // 
            this.powiadomieniaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ustawieniaToolStripMenuItem1,
            this.pokażToolStripMenuItem});
            this.powiadomieniaToolStripMenuItem.Name = "powiadomieniaToolStripMenuItem";
            this.powiadomieniaToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.powiadomieniaToolStripMenuItem.Text = "Powiadomienia";
            this.powiadomieniaToolStripMenuItem.MouseEnter += new System.EventHandler(this.powiadomieniaToolStripMenuItem_MouseEnter);
            // 
            // ustawieniaToolStripMenuItem1
            // 
            this.ustawieniaToolStripMenuItem1.Name = "ustawieniaToolStripMenuItem1";
            this.ustawieniaToolStripMenuItem1.Size = new System.Drawing.Size(131, 22);
            this.ustawieniaToolStripMenuItem1.Text = "Ustawienia";
            this.ustawieniaToolStripMenuItem1.Click += new System.EventHandler(this.ustawieniaToolStripMenuItem1_Click);
            // 
            // pokażToolStripMenuItem
            // 
            this.pokażToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pokażToolStripMenuItem.Image")));
            this.pokażToolStripMenuItem.Name = "pokażToolStripMenuItem";
            this.pokażToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.pokażToolStripMenuItem.Text = "Pokaż";
            this.pokażToolStripMenuItem.Click += new System.EventHandler(this.pokażToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1492, 453);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 100000;
            this.refreshTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1492, 477);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TableForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tabelka";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TableForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableContextMenu.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView dataGridView1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem konsolaToolStripMenuItem;
        private ToolStripMenuItem pokażCałośćToolStripMenuItem;
        private ToolStripMenuItem tabelkaToolStripMenuItem;
        private ToolStripMenuItem odświeżToolStripMenuItem;
        private ToolStripMenuItem jakoMessageBoxToolStripMenuItem;
        private ToolStripMenuItem jakoRichTextBoxToolStripMenuItem;
        private PictureBox pictureBox1;
        private ToolStripMenuItem wartościToolStripMenuItem;
        private ToolStripMenuItem minimalnaToolStripMenuItem;
        private ToolStripMenuItem maksymalnaToolStripMenuItem;
        private ToolStripMenuItem symboleToolStripMenuItem;
        private ToolStripMenuItem modyfikujToolStripMenuItem;
        private ToolStripMenuItem ustawieniaToolStripMenuItem;
        private ToolStripMenuItem kolorPowyżejToolStripMenuItem;
        private ToolStripMenuItem kolorPoniżejToolStripMenuItem;
        private ToolStripMenuItem kolorBłęduToolStripMenuItem;
        private ToolStripMenuItem kolorNieobsługiwanegoSymboluToolStripMenuItem;
        private ToolStripMenuItem dodajSymbolToolStripMenuItem;
        private ToolStripMenuItem pobierzSymboleZBinanceToolStripMenuItem;
        private ToolStripMenuItem powiadomieniaToolStripMenuItem;
        private ToolStripMenuItem ustawieniaToolStripMenuItem1;
        private ToolStripMenuItem pokażToolStripMenuItem;
        private System.Windows.Forms.Timer refreshTimer;
        private ToolStripMenuItem odświeżajToolStripMenuItem;
        private ContextMenuStrip tableContextMenu;
        private ToolStripMenuItem ignorujXWPowiadomieniachToolStripMenuItem;
        private ToolStripMenuItem usuńXZIgnorowanychToolStripMenuItem;
    }
}