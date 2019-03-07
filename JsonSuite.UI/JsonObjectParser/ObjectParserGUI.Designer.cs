namespace com.webkingsoft.JsonSuite.UI
{
    partial class ObjectParserGUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectParserGUI));
            this.label1 = new System.Windows.Forms.Label();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.mappingGrid = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.inputColumn = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.donateControl1 = new com.webkingsoft.JsonSuite.UI.DonateControl();
            this.OutpucColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AttributeExpression = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JsonDataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Precision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Scale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mappingGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(642, 66);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(697, 456);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 5;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(616, 456);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 6;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // mappingGrid
            // 
            this.mappingGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mappingGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.mappingGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mappingGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OutpucColumnName,
            this.AttributeExpression,
            this.JsonDataType,
            this.Precision,
            this.Scale});
            this.mappingGrid.Location = new System.Drawing.Point(12, 161);
            this.mappingGrid.Name = "mappingGrid";
            this.mappingGrid.Size = new System.Drawing.Size(760, 289);
            this.mappingGrid.TabIndex = 9;
            this.mappingGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.mappingGrid_CellContentClick);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Calibri Light", 10F, System.Drawing.FontStyle.Italic);
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(389, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(211, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "Note: only textual input columns are shown.";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(450, 27);
            this.label2.TabIndex = 2;
            this.label2.Text = "1. Select the input column containing the JSON object string:";
            // 
            // inputColumn
            // 
            this.inputColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputColumn.FormattingEnabled = true;
            this.inputColumn.Location = new System.Drawing.Point(16, 107);
            this.inputColumn.Name = "inputColumn";
            this.inputColumn.Size = new System.Drawing.Size(367, 21);
            this.inputColumn.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(450, 27);
            this.label4.TabIndex = 10;
            this.label4.Text = "2. Configure JSON object attribute parsing:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::com.webkingsoft.JsonSuite.UI.Properties.Resources.object_parser2;
            this.pictureBox1.Location = new System.Drawing.Point(646, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(126, 119);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // donateControl1
            // 
            this.donateControl1.AutoSize = true;
            this.donateControl1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.donateControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.donateControl1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.donateControl1.Location = new System.Drawing.Point(0, 485);
            this.donateControl1.Name = "donateControl1";
            this.donateControl1.Size = new System.Drawing.Size(784, 76);
            this.donateControl1.TabIndex = 7;
            // 
            // OutpucColumnName
            // 
            this.OutpucColumnName.HeaderText = "Output Column Name";
            this.OutpucColumnName.Name = "OutpucColumnName";
            // 
            // AttributeExpression
            // 
            this.AttributeExpression.HeaderText = "Json Object Attribute";
            this.AttributeExpression.Name = "AttributeExpression";
            // 
            // JsonDataType
            // 
            this.JsonDataType.HeaderText = "Expected Json Data type";
            this.JsonDataType.Name = "JsonDataType";
            this.JsonDataType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.JsonDataType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Precision
            // 
            this.Precision.HeaderText = "Data Precision";
            this.Precision.Name = "Precision";
            // 
            // Scale
            // 
            this.Scale.HeaderText = "Data Scale";
            this.Scale.Name = "Scale";
            // 
            // ObjectParserGUI
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.inputColumn);
            this.Controls.Add(this.mappingGrid);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.donateControl1);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ObjectParserGUI";
            this.Text = "Object Parser Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mappingGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private DonateControl donateControl1;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox inputColumn;
        private System.Windows.Forms.DataGridView mappingGrid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutpucColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn AttributeExpression;
        private System.Windows.Forms.DataGridViewComboBoxColumn JsonDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Precision;
        private System.Windows.Forms.DataGridViewTextBoxColumn Scale;
    }
}