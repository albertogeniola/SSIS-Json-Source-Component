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
            this.top = new System.Windows.Forms.Panel();
            this.uiRootType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.uiPathToArray = new System.Windows.Forms.TextBox();
            this.uiIOGrid = new System.Windows.Forms.DataGridView();
            this.JSONFieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JSONMaxLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutColumnType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiIOGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // top
            // 
            this.top.Controls.Add(this.uiRootType);
            this.top.Controls.Add(this.label2);
            this.top.Controls.Add(this.uiPathToArray);
            this.top.Dock = System.Windows.Forms.DockStyle.Top;
            this.top.Location = new System.Drawing.Point(0, 0);
            this.top.Name = "top";
            this.top.Size = new System.Drawing.Size(700, 42);
            this.top.TabIndex = 0;
            // 
            // uiRootType
            // 
            this.uiRootType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uiRootType.FormattingEnabled = true;
            this.uiRootType.Location = new System.Drawing.Point(497, 9);
            this.uiRootType.Name = "uiRootType";
            this.uiRootType.Size = new System.Drawing.Size(158, 21);
            this.uiRootType.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Json Root Path:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // uiPathToArray
            // 
            this.uiPathToArray.Location = new System.Drawing.Point(105, 9);
            this.uiPathToArray.Name = "uiPathToArray";
            this.uiPathToArray.Size = new System.Drawing.Size(386, 20);
            this.uiPathToArray.TabIndex = 10;
            this.uiPathToArray.TextChanged += new System.EventHandler(this.uiPathToArray_TextChanged);
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
            this.uiIOGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiIOGrid.Location = new System.Drawing.Point(0, 42);
            this.uiIOGrid.Name = "uiIOGrid";
            this.uiIOGrid.Size = new System.Drawing.Size(700, 412);
            this.uiIOGrid.TabIndex = 7;
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
            // ColumnView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uiIOGrid);
            this.Controls.Add(this.top);
            this.Name = "ColumnView";
            this.Size = new System.Drawing.Size(700, 454);
            this.top.ResumeLayout(false);
            this.top.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiIOGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel top;
        private System.Windows.Forms.DataGridViewTextBoxColumn JSONFieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn JSONMaxLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutColName;
        private System.Windows.Forms.DataGridViewComboBoxColumn OutColumnType;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.DataGridView uiIOGrid;
        public System.Windows.Forms.TextBox uiPathToArray;
        public System.Windows.Forms.ComboBox uiRootType;
    }
}
