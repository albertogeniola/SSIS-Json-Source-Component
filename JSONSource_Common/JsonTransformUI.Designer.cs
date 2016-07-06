namespace com.webkingsoft.JSONSource_Common
{
    partial class JsonTransformUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JsonTransformUI));
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.uiTempDir = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.inutColumnCb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.uiPathToArray = new System.Windows.Forms.TextBox();
            this.uiIOGrid = new System.Windows.Forms.DataGridView();
            this.JSONFieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JSONMaxLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutColumnType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbParseJsonDate = new System.Windows.Forms.CheckBox();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiIOGrid)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(670, 455);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(92, 36);
            this.ok.TabIndex = 7;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(572, 455);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(92, 36);
            this.cancel.TabIndex = 8;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.uiTempDir);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(742, 411);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Advanced";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(210, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Local temp dir (if different from system one):";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // uiTempDir
            // 
            this.uiTempDir.Location = new System.Drawing.Point(9, 28);
            this.uiTempDir.Name = "uiTempDir";
            this.uiTempDir.Size = new System.Drawing.Size(333, 20);
            this.uiTempDir.TabIndex = 5;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.inutColumnCb);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.uiPathToArray);
            this.tabPage3.Controls.Add(this.uiIOGrid);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(742, 411);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Input - Output";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // inutColumnCb
            // 
            this.inutColumnCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inutColumnCb.FormattingEnabled = true;
            this.inutColumnCb.Location = new System.Drawing.Point(97, 14);
            this.inutColumnCb.Name = "inutColumnCb";
            this.inutColumnCb.Size = new System.Drawing.Size(639, 21);
            this.inutColumnCb.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Input Column:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Array Root Path:";
            // 
            // uiPathToArray
            // 
            this.uiPathToArray.Location = new System.Drawing.Point(97, 39);
            this.uiPathToArray.Name = "uiPathToArray";
            this.uiPathToArray.Size = new System.Drawing.Size(639, 20);
            this.uiPathToArray.TabIndex = 8;
            // 
            // uiIOGrid
            // 
            this.uiIOGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.uiIOGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.uiIOGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.JSONFieldName,
            this.JSONMaxLen,
            this.OutColName,
            this.OutColumnType});
            this.uiIOGrid.Location = new System.Drawing.Point(6, 65);
            this.uiIOGrid.Name = "uiIOGrid";
            this.uiIOGrid.Size = new System.Drawing.Size(730, 340);
            this.uiIOGrid.TabIndex = 6;
            this.uiIOGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.uiIOGrid_CellEndEdit);
            // 
            // JSONFieldName
            // 
            this.JSONFieldName.HeaderText = "JSON Field Name";
            this.JSONFieldName.Name = "JSONFieldName";
            // 
            // JSONMaxLen
            // 
            this.JSONMaxLen.HeaderText = "Max Length";
            this.JSONMaxLen.Name = "JSONMaxLen";
            // 
            // OutColName
            // 
            this.OutColName.HeaderText = "OutputColumnName";
            this.OutColName.Name = "OutColName";
            // 
            // OutColumnType
            // 
            this.OutColumnType.HeaderText = "Column Type";
            this.OutColumnType.Name = "OutColumnType";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(750, 437);
            this.tabControl1.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbParseJsonDate);
            this.groupBox1.Location = new System.Drawing.Point(9, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(365, 76);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Json Parsing";
            // 
            // cbParseJsonDate
            // 
            this.cbParseJsonDate.Location = new System.Drawing.Point(6, 19);
            this.cbParseJsonDate.Name = "cbParseJsonDate";
            this.cbParseJsonDate.Size = new System.Drawing.Size(353, 47);
            this.cbParseJsonDate.TabIndex = 10;
            this.cbParseJsonDate.Text = "Parse Json dates: json.net library has some problem with timezone conversion. Che" +
    "ck this in order to perfom manual dates conversion.";
            this.cbParseJsonDate.UseVisualStyleBackColor = true;
            // 
            // JsonTransformUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 500);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "JsonTransformUI";
            this.Text = "JsonSourceUI";
            this.TopMost = true;
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiIOGrid)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox uiTempDir;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ComboBox inutColumnCb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox uiPathToArray;
        private System.Windows.Forms.DataGridView uiIOGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn JSONFieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn JSONMaxLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutColName;
        private System.Windows.Forms.DataGridViewComboBoxColumn OutColumnType;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbParseJsonDate;
    }
}