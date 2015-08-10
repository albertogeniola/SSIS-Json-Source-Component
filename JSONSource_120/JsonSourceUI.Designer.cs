namespace com.webkingsoft.JSONSource_120
{
    partial class JsonSourceUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JsonSourceUI));
            this.uiTempDir = new System.Windows.Forms.TextBox();
            this.uiIOGrid = new System.Windows.Forms.DataGridView();
            this.JSONFieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JSONMaxLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutColumnType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.uiWebURLCustom = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.sourceTabPage = new System.Windows.Forms.TabPage();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.uiVariableFilePathGroup = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.uiBrowseFilePathVariable = new System.Windows.Forms.Button();
            this.uiFilePathVariable = new System.Windows.Forms.TextBox();
            this.uiVariableUrlGroup = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.uiSelectURLVariable = new System.Windows.Forms.Button();
            this.uiVarLabel = new System.Windows.Forms.Label();
            this.uiURLVariable = new System.Windows.Forms.TextBox();
            this.uiCustomUrlGroup = new System.Windows.Forms.GroupBox();
            this.postRadio = new System.Windows.Forms.RadioButton();
            this.getRadio = new System.Windows.Forms.RadioButton();
            this.uiTestWebURL = new System.Windows.Forms.Button();
            this.uiFilePathGroup = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.uiBrowseFilePath = new System.Windows.Forms.Button();
            this.uiFilePathCustom = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.uiSourceType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.uiPathToArray = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.putRadio = new System.Windows.Forms.RadioButton();
            this.delRadio = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.uiIOGrid)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.sourceTabPage.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.uiVariableFilePathGroup.SuspendLayout();
            this.uiVariableUrlGroup.SuspendLayout();
            this.uiCustomUrlGroup.SuspendLayout();
            this.uiFilePathGroup.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiTempDir
            // 
            this.uiTempDir.Location = new System.Drawing.Point(9, 28);
            this.uiTempDir.Name = "uiTempDir";
            this.uiTempDir.Size = new System.Drawing.Size(333, 20);
            this.uiTempDir.TabIndex = 5;
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
            this.uiIOGrid.Location = new System.Drawing.Point(6, 42);
            this.uiIOGrid.Name = "uiIOGrid";
            this.uiIOGrid.Size = new System.Drawing.Size(730, 363);
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
            // uiWebURLCustom
            // 
            this.uiWebURLCustom.Location = new System.Drawing.Point(13, 42);
            this.uiWebURLCustom.Multiline = true;
            this.uiWebURLCustom.Name = "uiWebURLCustom";
            this.uiWebURLCustom.Size = new System.Drawing.Size(280, 82);
            this.uiWebURLCustom.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Web URL:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.sourceTabPage);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(750, 437);
            this.tabControl1.TabIndex = 9;
            // 
            // sourceTabPage
            // 
            this.sourceTabPage.Controls.Add(this.MainPanel);
            this.sourceTabPage.Controls.Add(this.label5);
            this.sourceTabPage.Controls.Add(this.uiSourceType);
            this.sourceTabPage.Controls.Add(this.label4);
            this.sourceTabPage.Location = new System.Drawing.Point(4, 22);
            this.sourceTabPage.Name = "sourceTabPage";
            this.sourceTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.sourceTabPage.Size = new System.Drawing.Size(742, 411);
            this.sourceTabPage.TabIndex = 0;
            this.sourceTabPage.Text = "Source";
            this.sourceTabPage.UseVisualStyleBackColor = true;
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.uiVariableFilePathGroup);
            this.MainPanel.Controls.Add(this.uiVariableUrlGroup);
            this.MainPanel.Controls.Add(this.uiCustomUrlGroup);
            this.MainPanel.Controls.Add(this.uiFilePathGroup);
            this.MainPanel.Location = new System.Drawing.Point(9, 91);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(599, 314);
            this.MainPanel.TabIndex = 7;
            // 
            // uiVariableFilePathGroup
            // 
            this.uiVariableFilePathGroup.Controls.Add(this.button2);
            this.uiVariableFilePathGroup.Controls.Add(this.uiBrowseFilePathVariable);
            this.uiVariableFilePathGroup.Controls.Add(this.uiFilePathVariable);
            this.uiVariableFilePathGroup.Location = new System.Drawing.Point(338, 117);
            this.uiVariableFilePathGroup.Name = "uiVariableFilePathGroup";
            this.uiVariableFilePathGroup.Size = new System.Drawing.Size(258, 84);
            this.uiVariableFilePathGroup.TabIndex = 16;
            this.uiVariableFilePathGroup.TabStop = false;
            this.uiVariableFilePathGroup.Text = "Path from varible";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(177, 46);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(74, 31);
            this.button2.TabIndex = 16;
            this.button2.Text = "Add new...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // uiBrowseFilePathVariable
            // 
            this.uiBrowseFilePathVariable.Location = new System.Drawing.Point(100, 46);
            this.uiBrowseFilePathVariable.Name = "uiBrowseFilePathVariable";
            this.uiBrowseFilePathVariable.Size = new System.Drawing.Size(71, 31);
            this.uiBrowseFilePathVariable.TabIndex = 15;
            this.uiBrowseFilePathVariable.Text = "Browse...";
            this.uiBrowseFilePathVariable.UseVisualStyleBackColor = true;
            this.uiBrowseFilePathVariable.Click += new System.EventHandler(this.uiBrowseFilePathVariable_Click);
            // 
            // uiFilePathVariable
            // 
            this.uiFilePathVariable.Location = new System.Drawing.Point(6, 20);
            this.uiFilePathVariable.Name = "uiFilePathVariable";
            this.uiFilePathVariable.Size = new System.Drawing.Size(245, 20);
            this.uiFilePathVariable.TabIndex = 14;
            // 
            // uiVariableUrlGroup
            // 
            this.uiVariableUrlGroup.Controls.Add(this.button1);
            this.uiVariableUrlGroup.Controls.Add(this.uiSelectURLVariable);
            this.uiVariableUrlGroup.Controls.Add(this.uiVarLabel);
            this.uiVariableUrlGroup.Controls.Add(this.uiURLVariable);
            this.uiVariableUrlGroup.Location = new System.Drawing.Point(338, 3);
            this.uiVariableUrlGroup.Name = "uiVariableUrlGroup";
            this.uiVariableUrlGroup.Size = new System.Drawing.Size(258, 108);
            this.uiVariableUrlGroup.TabIndex = 15;
            this.uiVariableUrlGroup.TabStop = false;
            this.uiVariableUrlGroup.Text = "URL From variable";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(177, 68);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(74, 31);
            this.button1.TabIndex = 14;
            this.button1.Text = "Add new...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // uiSelectURLVariable
            // 
            this.uiSelectURLVariable.Location = new System.Drawing.Point(100, 68);
            this.uiSelectURLVariable.Name = "uiSelectURLVariable";
            this.uiSelectURLVariable.Size = new System.Drawing.Size(71, 31);
            this.uiSelectURLVariable.TabIndex = 13;
            this.uiSelectURLVariable.Text = "Browse...";
            this.uiSelectURLVariable.UseVisualStyleBackColor = true;
            this.uiSelectURLVariable.Click += new System.EventHandler(this.uiSelectURLVariable_Click);
            // 
            // uiVarLabel
            // 
            this.uiVarLabel.AutoSize = true;
            this.uiVarLabel.Location = new System.Drawing.Point(6, 26);
            this.uiVarLabel.Name = "uiVarLabel";
            this.uiVarLabel.Size = new System.Drawing.Size(48, 13);
            this.uiVarLabel.TabIndex = 1;
            this.uiVarLabel.Text = "Variable:";
            // 
            // uiURLVariable
            // 
            this.uiURLVariable.Location = new System.Drawing.Point(6, 42);
            this.uiURLVariable.Name = "uiURLVariable";
            this.uiURLVariable.Size = new System.Drawing.Size(245, 20);
            this.uiURLVariable.TabIndex = 0;
            // 
            // uiCustomUrlGroup
            // 
            this.uiCustomUrlGroup.Controls.Add(this.delRadio);
            this.uiCustomUrlGroup.Controls.Add(this.putRadio);
            this.uiCustomUrlGroup.Controls.Add(this.postRadio);
            this.uiCustomUrlGroup.Controls.Add(this.getRadio);
            this.uiCustomUrlGroup.Controls.Add(this.uiTestWebURL);
            this.uiCustomUrlGroup.Controls.Add(this.uiWebURLCustom);
            this.uiCustomUrlGroup.Controls.Add(this.label1);
            this.uiCustomUrlGroup.Location = new System.Drawing.Point(18, 3);
            this.uiCustomUrlGroup.Name = "uiCustomUrlGroup";
            this.uiCustomUrlGroup.Size = new System.Drawing.Size(314, 188);
            this.uiCustomUrlGroup.TabIndex = 14;
            this.uiCustomUrlGroup.TabStop = false;
            this.uiCustomUrlGroup.Text = "Pre-Defined URL";
            // 
            // postRadio
            // 
            this.postRadio.AutoSize = true;
            this.postRadio.Location = new System.Drawing.Point(74, 130);
            this.postRadio.Name = "postRadio";
            this.postRadio.Size = new System.Drawing.Size(54, 17);
            this.postRadio.TabIndex = 15;
            this.postRadio.TabStop = true;
            this.postRadio.Text = "POST";
            this.postRadio.UseVisualStyleBackColor = true;
            // 
            // getRadio
            // 
            this.getRadio.AutoSize = true;
            this.getRadio.Checked = true;
            this.getRadio.Location = new System.Drawing.Point(21, 131);
            this.getRadio.Name = "getRadio";
            this.getRadio.Size = new System.Drawing.Size(47, 17);
            this.getRadio.TabIndex = 14;
            this.getRadio.TabStop = true;
            this.getRadio.Text = "GET";
            this.getRadio.UseVisualStyleBackColor = true;
            // 
            // uiTestWebURL
            // 
            this.uiTestWebURL.Location = new System.Drawing.Point(222, 159);
            this.uiTestWebURL.Name = "uiTestWebURL";
            this.uiTestWebURL.Size = new System.Drawing.Size(71, 20);
            this.uiTestWebURL.TabIndex = 13;
            this.uiTestWebURL.Text = "Test...";
            this.uiTestWebURL.UseVisualStyleBackColor = true;
            this.uiTestWebURL.Click += new System.EventHandler(this.uiTestWebURL_Click);
            // 
            // uiFilePathGroup
            // 
            this.uiFilePathGroup.Controls.Add(this.label6);
            this.uiFilePathGroup.Controls.Add(this.uiBrowseFilePath);
            this.uiFilePathGroup.Controls.Add(this.uiFilePathCustom);
            this.uiFilePathGroup.Location = new System.Drawing.Point(18, 197);
            this.uiFilePathGroup.Name = "uiFilePathGroup";
            this.uiFilePathGroup.Size = new System.Drawing.Size(314, 100);
            this.uiFilePathGroup.TabIndex = 13;
            this.uiFilePathGroup.TabStop = false;
            this.uiFilePathGroup.Text = "Pre-Defined File Path";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "File path:";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // uiBrowseFilePath
            // 
            this.uiBrowseFilePath.Location = new System.Drawing.Point(222, 67);
            this.uiBrowseFilePath.Name = "uiBrowseFilePath";
            this.uiBrowseFilePath.Size = new System.Drawing.Size(71, 20);
            this.uiBrowseFilePath.TabIndex = 12;
            this.uiBrowseFilePath.Text = "Browse...";
            this.uiBrowseFilePath.UseVisualStyleBackColor = true;
            this.uiBrowseFilePath.Click += new System.EventHandler(this.FilePathFromVariable_Click);
            // 
            // uiFilePathCustom
            // 
            this.uiFilePathCustom.Location = new System.Drawing.Point(21, 41);
            this.uiFilePathCustom.Name = "uiFilePathCustom";
            this.uiFilePathCustom.Size = new System.Drawing.Size(272, 20);
            this.uiFilePathCustom.TabIndex = 11;
            this.uiFilePathCustom.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(147, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Source Type:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // uiSourceType
            // 
            this.uiSourceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uiSourceType.FormattingEnabled = true;
            this.uiSourceType.Location = new System.Drawing.Point(224, 64);
            this.uiSourceType.Name = "uiSourceType";
            this.uiSourceType.Size = new System.Drawing.Size(221, 21);
            this.uiSourceType.TabIndex = 5;
            this.uiSourceType.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(602, 43);
            this.label4.TabIndex = 4;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // tabPage3
            // 
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Array Root Path:";
            // 
            // uiPathToArray
            // 
            this.uiPathToArray.Location = new System.Drawing.Point(97, 12);
            this.uiPathToArray.Name = "uiPathToArray";
            this.uiPathToArray.Size = new System.Drawing.Size(639, 20);
            this.uiPathToArray.TabIndex = 8;
            // 
            // tabPage2
            // 
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
            // putRadio
            // 
            this.putRadio.AutoSize = true;
            this.putRadio.Location = new System.Drawing.Point(137, 131);
            this.putRadio.Name = "putRadio";
            this.putRadio.Size = new System.Drawing.Size(47, 17);
            this.putRadio.TabIndex = 16;
            this.putRadio.TabStop = true;
            this.putRadio.Text = "PUT";
            this.putRadio.UseVisualStyleBackColor = true;
            // 
            // delRadio
            // 
            this.delRadio.AutoSize = true;
            this.delRadio.Location = new System.Drawing.Point(190, 130);
            this.delRadio.Name = "delRadio";
            this.delRadio.Size = new System.Drawing.Size(67, 17);
            this.delRadio.TabIndex = 17;
            this.delRadio.TabStop = true;
            this.delRadio.Text = "DELETE";
            this.delRadio.UseVisualStyleBackColor = true;
            // 
            // JsonSourceUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 500);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "JsonSourceUI";
            this.Text = "JsonSourceUI";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.uiIOGrid)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.sourceTabPage.ResumeLayout(false);
            this.sourceTabPage.PerformLayout();
            this.MainPanel.ResumeLayout(false);
            this.uiVariableFilePathGroup.ResumeLayout(false);
            this.uiVariableFilePathGroup.PerformLayout();
            this.uiVariableUrlGroup.ResumeLayout(false);
            this.uiVariableUrlGroup.PerformLayout();
            this.uiCustomUrlGroup.ResumeLayout(false);
            this.uiCustomUrlGroup.PerformLayout();
            this.uiFilePathGroup.ResumeLayout(false);
            this.uiFilePathGroup.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox uiTempDir;
        private System.Windows.Forms.DataGridView uiIOGrid;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox uiWebURLCustom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage sourceTabPage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox uiSourceType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button uiBrowseFilePath;
        private System.Windows.Forms.TextBox uiFilePathCustom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox uiVariableFilePathGroup;
        private System.Windows.Forms.GroupBox uiVariableUrlGroup;
        private System.Windows.Forms.GroupBox uiCustomUrlGroup;
        private System.Windows.Forms.GroupBox uiFilePathGroup;
        private System.Windows.Forms.Button uiTestWebURL;
        private System.Windows.Forms.Button uiBrowseFilePathVariable;
        private System.Windows.Forms.TextBox uiFilePathVariable;
        private System.Windows.Forms.Button uiSelectURLVariable;
        private System.Windows.Forms.Label uiVarLabel;
        private System.Windows.Forms.TextBox uiURLVariable;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn JSONFieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn JSONMaxLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutColName;
        private System.Windows.Forms.DataGridViewComboBoxColumn OutColumnType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox uiPathToArray;
        private System.Windows.Forms.RadioButton postRadio;
        private System.Windows.Forms.RadioButton getRadio;
        private System.Windows.Forms.RadioButton delRadio;
        private System.Windows.Forms.RadioButton putRadio;
    }
}