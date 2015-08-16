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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.sourceTabPage = new System.Windows.Forms.TabPage();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.httpparams = new System.Windows.Forms.Button();
            this.cookieGroup = new System.Windows.Forms.GroupBox();
            this.cookieVarTb = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.uiTestWebURL = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.delRadio = new System.Windows.Forms.RadioButton();
            this.getRadio = new System.Windows.Forms.RadioButton();
            this.putRadio = new System.Windows.Forms.RadioButton();
            this.postRadio = new System.Windows.Forms.RadioButton();
            this.uiVariableFilePathGroup = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.uiBrowseFilePathVariable = new System.Windows.Forms.Button();
            this.uiFilePathVariable = new System.Windows.Forms.TextBox();
            this.uiVariableUrlGroup = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.uiSelectURLVariable = new System.Windows.Forms.Button();
            this.uiURLVariable = new System.Windows.Forms.TextBox();
            this.uiCustomUrlGroup = new System.Windows.Forms.GroupBox();
            this.uiFilePathGroup = new System.Windows.Forms.GroupBox();
            this.uiBrowseFilePath = new System.Windows.Forms.Button();
            this.uiFilePathCustom = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.uiSourceType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.uiPathToArray = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tmpBrowse = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.uiIOGrid)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.sourceTabPage.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.cookieGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.ok.Location = new System.Drawing.Point(670, 366);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(92, 36);
            this.ok.TabIndex = 7;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(572, 366);
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
            this.uiWebURLCustom.Location = new System.Drawing.Point(13, 20);
            this.uiWebURLCustom.Multiline = true;
            this.uiWebURLCustom.Name = "uiWebURLCustom";
            this.uiWebURLCustom.Size = new System.Drawing.Size(238, 73);
            this.uiWebURLCustom.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.sourceTabPage);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(750, 348);
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
            this.sourceTabPage.Size = new System.Drawing.Size(742, 322);
            this.sourceTabPage.TabIndex = 0;
            this.sourceTabPage.Text = "Source";
            this.sourceTabPage.UseVisualStyleBackColor = true;
            this.sourceTabPage.Click += new System.EventHandler(this.sourceTabPage_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.httpparams);
            this.MainPanel.Controls.Add(this.cookieGroup);
            this.MainPanel.Controls.Add(this.uiTestWebURL);
            this.MainPanel.Controls.Add(this.groupBox1);
            this.MainPanel.Controls.Add(this.uiVariableFilePathGroup);
            this.MainPanel.Controls.Add(this.uiVariableUrlGroup);
            this.MainPanel.Controls.Add(this.uiCustomUrlGroup);
            this.MainPanel.Controls.Add(this.uiFilePathGroup);
            this.MainPanel.Location = new System.Drawing.Point(9, 80);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(733, 237);
            this.MainPanel.TabIndex = 7;
            // 
            // httpparams
            // 
            this.httpparams.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.httpparams.Location = new System.Drawing.Point(18, 206);
            this.httpparams.Name = "httpparams";
            this.httpparams.Size = new System.Drawing.Size(263, 23);
            this.httpparams.TabIndex = 21;
            this.httpparams.Text = "HTTP Parameters...";
            this.httpparams.UseVisualStyleBackColor = true;
            this.httpparams.Click += new System.EventHandler(this.button5_Click);
            // 
            // cookieGroup
            // 
            this.cookieGroup.Controls.Add(this.cookieVarTb);
            this.cookieGroup.Controls.Add(this.button3);
            this.cookieGroup.Controls.Add(this.button4);
            this.cookieGroup.Location = new System.Drawing.Point(563, 114);
            this.cookieGroup.Name = "cookieGroup";
            this.cookieGroup.Size = new System.Drawing.Size(164, 86);
            this.cookieGroup.TabIndex = 20;
            this.cookieGroup.TabStop = false;
            this.cookieGroup.Text = "COOKIE Variable";
            // 
            // cookieVarTb
            // 
            this.cookieVarTb.Location = new System.Drawing.Point(7, 18);
            this.cookieVarTb.Name = "cookieVarTb";
            this.cookieVarTb.ReadOnly = true;
            this.cookieVarTb.Size = new System.Drawing.Size(152, 20);
            this.cookieVarTb.TabIndex = 17;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(85, 46);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(74, 31);
            this.button3.TabIndex = 16;
            this.button3.Text = "Add new...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(9, 46);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(71, 31);
            this.button4.TabIndex = 15;
            this.button4.Text = "Browse...";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // uiTestWebURL
            // 
            this.uiTestWebURL.Location = new System.Drawing.Point(563, 206);
            this.uiTestWebURL.Name = "uiTestWebURL";
            this.uiTestWebURL.Size = new System.Drawing.Size(164, 23);
            this.uiTestWebURL.TabIndex = 13;
            this.uiTestWebURL.Text = "Test...";
            this.uiTestWebURL.UseVisualStyleBackColor = true;
            this.uiTestWebURL.Click += new System.EventHandler(this.uiTestWebURL_Click_1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.delRadio);
            this.groupBox1.Controls.Add(this.getRadio);
            this.groupBox1.Controls.Add(this.putRadio);
            this.groupBox1.Controls.Add(this.postRadio);
            this.groupBox1.Location = new System.Drawing.Point(563, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(164, 108);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Method";
            // 
            // delRadio
            // 
            this.delRadio.AutoSize = true;
            this.delRadio.Location = new System.Drawing.Point(6, 88);
            this.delRadio.Name = "delRadio";
            this.delRadio.Size = new System.Drawing.Size(67, 17);
            this.delRadio.TabIndex = 17;
            this.delRadio.TabStop = true;
            this.delRadio.Text = "DELETE";
            this.delRadio.UseVisualStyleBackColor = true;
            // 
            // getRadio
            // 
            this.getRadio.AutoSize = true;
            this.getRadio.Checked = true;
            this.getRadio.Location = new System.Drawing.Point(6, 19);
            this.getRadio.Name = "getRadio";
            this.getRadio.Size = new System.Drawing.Size(47, 17);
            this.getRadio.TabIndex = 14;
            this.getRadio.TabStop = true;
            this.getRadio.Text = "GET";
            this.getRadio.UseVisualStyleBackColor = true;
            // 
            // putRadio
            // 
            this.putRadio.AutoSize = true;
            this.putRadio.Location = new System.Drawing.Point(6, 65);
            this.putRadio.Name = "putRadio";
            this.putRadio.Size = new System.Drawing.Size(47, 17);
            this.putRadio.TabIndex = 16;
            this.putRadio.TabStop = true;
            this.putRadio.Text = "PUT";
            this.putRadio.UseVisualStyleBackColor = true;
            // 
            // postRadio
            // 
            this.postRadio.AutoSize = true;
            this.postRadio.Location = new System.Drawing.Point(6, 42);
            this.postRadio.Name = "postRadio";
            this.postRadio.Size = new System.Drawing.Size(54, 17);
            this.postRadio.TabIndex = 15;
            this.postRadio.TabStop = true;
            this.postRadio.Text = "POST";
            this.postRadio.UseVisualStyleBackColor = true;
            // 
            // uiVariableFilePathGroup
            // 
            this.uiVariableFilePathGroup.Controls.Add(this.button2);
            this.uiVariableFilePathGroup.Controls.Add(this.uiBrowseFilePathVariable);
            this.uiVariableFilePathGroup.Controls.Add(this.uiFilePathVariable);
            this.uiVariableFilePathGroup.Location = new System.Drawing.Point(287, 3);
            this.uiVariableFilePathGroup.Name = "uiVariableFilePathGroup";
            this.uiVariableFilePathGroup.Size = new System.Drawing.Size(270, 108);
            this.uiVariableFilePathGroup.TabIndex = 16;
            this.uiVariableFilePathGroup.TabStop = false;
            this.uiVariableFilePathGroup.Text = "Path from varible";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(180, 46);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(74, 31);
            this.button2.TabIndex = 16;
            this.button2.Text = "Add new...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // uiBrowseFilePathVariable
            // 
            this.uiBrowseFilePathVariable.Location = new System.Drawing.Point(103, 46);
            this.uiBrowseFilePathVariable.Name = "uiBrowseFilePathVariable";
            this.uiBrowseFilePathVariable.Size = new System.Drawing.Size(71, 31);
            this.uiBrowseFilePathVariable.TabIndex = 15;
            this.uiBrowseFilePathVariable.Text = "Browse...";
            this.uiBrowseFilePathVariable.UseVisualStyleBackColor = true;
            this.uiBrowseFilePathVariable.Click += new System.EventHandler(this.uiBrowseFilePathVariable_Click);
            // 
            // uiFilePathVariable
            // 
            this.uiFilePathVariable.Location = new System.Drawing.Point(9, 20);
            this.uiFilePathVariable.Name = "uiFilePathVariable";
            this.uiFilePathVariable.ReadOnly = true;
            this.uiFilePathVariable.Size = new System.Drawing.Size(245, 20);
            this.uiFilePathVariable.TabIndex = 14;
            // 
            // uiVariableUrlGroup
            // 
            this.uiVariableUrlGroup.Controls.Add(this.button1);
            this.uiVariableUrlGroup.Controls.Add(this.uiSelectURLVariable);
            this.uiVariableUrlGroup.Controls.Add(this.uiURLVariable);
            this.uiVariableUrlGroup.Location = new System.Drawing.Point(18, 114);
            this.uiVariableUrlGroup.Name = "uiVariableUrlGroup";
            this.uiVariableUrlGroup.Size = new System.Drawing.Size(263, 86);
            this.uiVariableUrlGroup.TabIndex = 15;
            this.uiVariableUrlGroup.TabStop = false;
            this.uiVariableUrlGroup.Text = "URL From variable";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(177, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(74, 31);
            this.button1.TabIndex = 14;
            this.button1.Text = "Add new...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // uiSelectURLVariable
            // 
            this.uiSelectURLVariable.Location = new System.Drawing.Point(100, 45);
            this.uiSelectURLVariable.Name = "uiSelectURLVariable";
            this.uiSelectURLVariable.Size = new System.Drawing.Size(71, 31);
            this.uiSelectURLVariable.TabIndex = 13;
            this.uiSelectURLVariable.Text = "Browse...";
            this.uiSelectURLVariable.UseVisualStyleBackColor = true;
            this.uiSelectURLVariable.Click += new System.EventHandler(this.uiSelectURLVariable_Click);
            // 
            // uiURLVariable
            // 
            this.uiURLVariable.Location = new System.Drawing.Point(6, 19);
            this.uiURLVariable.Name = "uiURLVariable";
            this.uiURLVariable.ReadOnly = true;
            this.uiURLVariable.Size = new System.Drawing.Size(245, 20);
            this.uiURLVariable.TabIndex = 0;
            // 
            // uiCustomUrlGroup
            // 
            this.uiCustomUrlGroup.Controls.Add(this.uiWebURLCustom);
            this.uiCustomUrlGroup.Location = new System.Drawing.Point(18, 3);
            this.uiCustomUrlGroup.Name = "uiCustomUrlGroup";
            this.uiCustomUrlGroup.Size = new System.Drawing.Size(263, 108);
            this.uiCustomUrlGroup.TabIndex = 14;
            this.uiCustomUrlGroup.TabStop = false;
            this.uiCustomUrlGroup.Text = "Pre-Defined URL";
            // 
            // uiFilePathGroup
            // 
            this.uiFilePathGroup.Controls.Add(this.uiBrowseFilePath);
            this.uiFilePathGroup.Controls.Add(this.uiFilePathCustom);
            this.uiFilePathGroup.Location = new System.Drawing.Point(287, 114);
            this.uiFilePathGroup.Name = "uiFilePathGroup";
            this.uiFilePathGroup.Size = new System.Drawing.Size(270, 86);
            this.uiFilePathGroup.TabIndex = 13;
            this.uiFilePathGroup.TabStop = false;
            this.uiFilePathGroup.Text = "Pre-Defined File Path";
            // 
            // uiBrowseFilePath
            // 
            this.uiBrowseFilePath.Location = new System.Drawing.Point(180, 45);
            this.uiBrowseFilePath.Name = "uiBrowseFilePath";
            this.uiBrowseFilePath.Size = new System.Drawing.Size(71, 31);
            this.uiBrowseFilePath.TabIndex = 12;
            this.uiBrowseFilePath.Text = "Browse...";
            this.uiBrowseFilePath.UseVisualStyleBackColor = true;
            this.uiBrowseFilePath.Click += new System.EventHandler(this.FilePathFromVariable_Click);
            // 
            // uiFilePathCustom
            // 
            this.uiFilePathCustom.Location = new System.Drawing.Point(9, 19);
            this.uiFilePathCustom.Name = "uiFilePathCustom";
            this.uiFilePathCustom.ReadOnly = true;
            this.uiFilePathCustom.Size = new System.Drawing.Size(242, 20);
            this.uiFilePathCustom.TabIndex = 11;
            this.uiFilePathCustom.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(238, 56);
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
            this.uiSourceType.Location = new System.Drawing.Point(315, 53);
            this.uiSourceType.Name = "uiSourceType";
            this.uiSourceType.Size = new System.Drawing.Size(221, 21);
            this.uiSourceType.TabIndex = 5;
            this.uiSourceType.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(725, 43);
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
            this.tabPage3.Size = new System.Drawing.Size(742, 322);
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
            this.tabPage2.Controls.Add(this.tmpBrowse);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.uiTempDir);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(742, 322);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Advanced";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tmpBrowse
            // 
            this.tmpBrowse.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tmpBrowse.BackgroundImage")));
            this.tmpBrowse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tmpBrowse.Location = new System.Drawing.Point(348, 25);
            this.tmpBrowse.Name = "tmpBrowse";
            this.tmpBrowse.Size = new System.Drawing.Size(26, 24);
            this.tmpBrowse.TabIndex = 6;
            this.tmpBrowse.UseVisualStyleBackColor = true;
            this.tmpBrowse.Click += new System.EventHandler(this.tmpBrowse_Click);
            // 
            // JsonSourceUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 408);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "JsonSourceUI";
            this.Text = "JsonSourceUI";
            ((System.ComponentModel.ISupportInitialize)(this.uiIOGrid)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.sourceTabPage.ResumeLayout(false);
            this.sourceTabPage.PerformLayout();
            this.MainPanel.ResumeLayout(false);
            this.cookieGroup.ResumeLayout(false);
            this.cookieGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.GroupBox uiVariableFilePathGroup;
        private System.Windows.Forms.GroupBox uiVariableUrlGroup;
        private System.Windows.Forms.GroupBox uiCustomUrlGroup;
        private System.Windows.Forms.GroupBox uiFilePathGroup;
        private System.Windows.Forms.Button uiBrowseFilePathVariable;
        private System.Windows.Forms.TextBox uiFilePathVariable;
        private System.Windows.Forms.Button uiSelectURLVariable;
        private System.Windows.Forms.TextBox uiURLVariable;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn JSONFieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn JSONMaxLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutColName;
        private System.Windows.Forms.DataGridViewComboBoxColumn OutColumnType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox uiPathToArray;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton delRadio;
        private System.Windows.Forms.RadioButton getRadio;
        private System.Windows.Forms.RadioButton putRadio;
        private System.Windows.Forms.RadioButton postRadio;
        private System.Windows.Forms.GroupBox cookieGroup;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button httpparams;
        private System.Windows.Forms.Button uiTestWebURL;
        private System.Windows.Forms.TextBox cookieVarTb;
        private System.Windows.Forms.Button tmpBrowse;
    }
}