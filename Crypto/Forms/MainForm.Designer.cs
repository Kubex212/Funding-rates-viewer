namespace Crypto
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 178);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(204, 27);
            this.button1.TabIndex = 0;
            this.button1.Text = "Giełdy";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numericUpDown3);
            this.groupBox2.Controls.Add(this.numericUpDown2);
            this.groupBox2.Controls.Add(this.numericUpDown1);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(204, 157);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Opcje tabelki";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 30);
            this.label3.TabIndex = 5;
            this.label3.Text = "Maksimum na\r\nczerwono [%]\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 30);
            this.label2.TabIndex = 4;
            this.label2.Text = "Minimum na\r\nzielono [%]";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Czas odświeżania [s]";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.DecimalPlaces = 3;
            this.numericUpDown3.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numericUpDown3.Location = new System.Drawing.Point(129, 110);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            -2147418112});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(69, 23);
            this.numericUpDown3.TabIndex = 2;
            this.numericUpDown3.ThousandsSeparator = true;
            this.numericUpDown3.Value = new decimal(new int[] {
            5,
            0,
            0,
            -2147287040});
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DecimalPlaces = 3;
            this.numericUpDown2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numericUpDown2.Location = new System.Drawing.Point(129, 74);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            65536});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            -2147287040});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(69, 23);
            this.numericUpDown2.TabIndex = 1;
            this.numericUpDown2.ThousandsSeparator = true;
            this.numericUpDown2.Value = new decimal(new int[] {
            15,
            0,
            0,
            196608});
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(129, 36);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(69, 23);
            this.numericUpDown1.TabIndex = 0;
            this.numericUpDown1.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 216);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Crypto";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Button button1;
        private GroupBox groupBox2;
        private Label label3;
        private Label label2;
        private Label label1;
        private NumericUpDown numericUpDown3;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown1;
    }
}