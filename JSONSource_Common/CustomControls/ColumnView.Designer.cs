namespace com.webkingsoft.JSONSource_Common
{
    partial class ColumnView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.top = new System.Windows.Forms.Panel();
            this.uiRootType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.uiPathToArray = new System.Windows.Forms.TextBox();
            this.uiIOGrid = new System.Windows.Forms.DataGridView();
            this.JSONFieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JSONMaxLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutColumnType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.inputsCb = new System.Windows.Forms.CheckedListBox();
            this.top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiIOGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // top
            // 
            this.top.Controls.Add(this.label1);
            this.top.Controls.Add(this.uiRootType);
            this.top.Controls.Add(this.label2);
            this.top.Controls.Add(this.uiPathToArray);
            this.top.Dock = System.Windows.Forms.DockStyle.Top;
            this.top.Location = new System.Drawing.Point(0, 0);
            this.top.Name = "top";
            this.top.Size = new System.Drawing.Size(600, 64);
            this.top.TabIndex = 0;
            // 
            // uiRootType
            // 
            this.uiRootType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uiRootType.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiRootType.FormattingEnabled = true;
            this.uiRootType.Location = new System.Drawing.Point(10, 22);
            this.uiRootType.Name = "uiRootType";
            this.uiRootType.Size = new System.Drawing.Size(142, 22);
            this.uiRootType.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(158, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(266, 14);
            this.label2.TabIndex = 11;
            this.label2.Text = "Path to Json Array/Object from response:";
            // 
            // uiPathToArray
            // 
            this.uiPathToArray.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiPathToArray.Location = new System.Drawing.Point(158, 22);
            this.uiPathToArray.Name = "uiPathToArray";
            this.uiPathToArray.Size = new System.Drawing.Size(428, 22);
            this.uiPathToArray.TabIndex = 10;
            // 
            // uiIOGrid
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F);
            this.uiIOGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.uiIOGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiIOGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.uiIOGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.uiIOGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.JSONFieldName,
            this.JSONMaxLen,
            this.OutColName,
            this.OutColumnType});
            this.uiIOGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiIOGrid.Location = new System.Drawing.Point(0, 0);
            this.uiIOGrid.Name = "uiIOGrid";
            this.uiIOGrid.Size = new System.Drawing.Size(597, 216);
            this.uiIOGrid.TabIndex = 7;
            this.uiIOGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.uiIOGrid_CellContentClick);
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
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 14);
            this.label1.TabIndex = 13;
            this.label1.Text = "Json response type:";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 64);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 336);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 64);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.uiIOGrid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.inputsCb);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Size = new System.Drawing.Size(597, 336);
            this.splitContainer1.SplitterDistance = 216;
            this.splitContainer1.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(580, 38);
            this.label3.TabIndex = 0;
            this.label3.Text = "It follows the list of attached columns to the component input. Each selected col" +
    "umn will be copied to the outputs of this component, so that any JSON result is " +
    "associated with its inputs.";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // inputsCb
            // 
            this.inputsCb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputsCb.FormattingEnabled = true;
            this.inputsCb.Location = new System.Drawing.Point(3, 41);
            this.inputsCb.Name = "inputsCb";
            this.inputsCb.ScrollAlwaysVisible = true;
            this.inputsCb.Size = new System.Drawing.Size(584, 64);
            this.inputsCb.TabIndex = 1;
            // 
            // ColumnView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.top);
            this.Name = "ColumnView";
            this.Size = new System.Drawing.Size(600, 400);
            this.top.ResumeLayout(false);
            this.top.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiIOGrid)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel top;
        private System.Windows.Forms.DataGridViewTextBoxColumn JSONFieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn JSONMaxLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutColName;
        private System.Windows.Forms.DataGridViewComboBoxColumn OutColumnType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView uiIOGrid;
        private System.Windows.Forms.TextBox uiPathToArray;
        private System.Windows.Forms.ComboBox uiRootType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckedListBox inputsCb;
        private System.Windows.Forms.Label label3;
    }
}
