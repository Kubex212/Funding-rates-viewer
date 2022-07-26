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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableForm));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.konsolaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pokażCałośćToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jakoMessageBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jakoRichTextBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabelkaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.odświeżToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wartościToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimalnaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maksymalnaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.symboleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modyfikujToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 24);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(1160, 453);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.konsolaToolStripMenuItem,
            this.tabelkaToolStripMenuItem,
            this.symboleToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1160, 24);
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
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(433, 160);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(289, 145);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // symboleToolStripMenuItem
            // 
            this.symboleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modyfikujToolStripMenuItem});
            this.symboleToolStripMenuItem.Name = "symboleToolStripMenuItem";
            this.symboleToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.symboleToolStripMenuItem.Text = "Symbole";
            // 
            // modyfikujToolStripMenuItem
            // 
            this.modyfikujToolStripMenuItem.Name = "modyfikujToolStripMenuItem";
            this.modyfikujToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.modyfikujToolStripMenuItem.Text = "Modyfikuj";
            this.modyfikujToolStripMenuItem.Click += new System.EventHandler(this.modyfikujToolStripMenuItem_Click);
            // 
            // TableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 477);
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
    }
}