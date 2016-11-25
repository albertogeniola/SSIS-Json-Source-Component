namespace com.webkingsoft.JSONSource_Common
{
    partial class SourceView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SourceView));
            this.uiWebURL = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.delRadio = new System.Windows.Forms.RadioButton();
            this.getRadio = new System.Windows.Forms.RadioButton();
            this.putRadio = new System.Windows.Forms.RadioButton();
            this.postRadio = new System.Windows.Forms.RadioButton();
            this.cookieGroup = new System.Windows.Forms.GroupBox();
            this.cookieVarTb = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.httpparams = new System.Windows.Forms.Button();
            this.variableR = new System.Windows.Forms.RadioButton();
            this.directInputR = new System.Windows.Forms.RadioButton();
            this.browseButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.uiCustomUrlGroup = new System.Windows.Forms.GroupBox();
            this.header_button = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.inputLaneR = new System.Windows.Forms.RadioButton();
            this.inputLaneCb = new System.Windows.Forms.ComboBox();
            this.urlInputModeGb = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.cookieGroup.SuspendLayout();
            this.uiCustomUrlGroup.SuspendLayout();
            this.urlInputModeGb.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiWebURL
            // 
            this.uiWebURL.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiWebURL.Location = new System.Drawing.Point(19, 147);
            this.uiWebURL.Multiline = true;
            this.uiWebURL.Name = "uiWebURL";
            this.uiWebURL.Size = new System.Drawing.Size(417, 78);
            this.uiWebURL.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.delRadio);
            this.groupBox1.Controls.Add(this.getRadio);
            this.groupBox1.Controls.Add(this.putRadio);
            this.groupBox1.Controls.Add(this.postRadio);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(19, 267);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(213, 108);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Method";
            // 
            // delRadio
            // 
            this.delRadio.AutoSize = true;
            this.delRadio.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delRadio.Location = new System.Drawing.Point(6, 88);
            this.delRadio.Name = "delRadio";
            this.delRadio.Size = new System.Drawing.Size(61, 17);
            this.delRadio.TabIndex = 17;
            this.delRadio.TabStop = true;
            this.delRadio.Text = "DELETE";
            this.delRadio.UseVisualStyleBackColor = true;
            // 
            // getRadio
            // 
            this.getRadio.AutoSize = true;
            this.getRadio.Checked = true;
            this.getRadio.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.getRadio.Location = new System.Drawing.Point(6, 19);
            this.getRadio.Name = "getRadio";
            this.getRadio.Size = new System.Drawing.Size(44, 17);
            this.getRadio.TabIndex = 14;
            this.getRadio.TabStop = true;
            this.getRadio.Text = "GET";
            this.getRadio.UseVisualStyleBackColor = true;
            // 
            // putRadio
            // 
            this.putRadio.AutoSize = true;
            this.putRadio.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.putRadio.Location = new System.Drawing.Point(6, 65);
            this.putRadio.Name = "putRadio";
            this.putRadio.Size = new System.Drawing.Size(44, 17);
            this.putRadio.TabIndex = 16;
            this.putRadio.TabStop = true;
            this.putRadio.Text = "PUT";
            this.putRadio.UseVisualStyleBackColor = true;
            // 
            // postRadio
            // 
            this.postRadio.AutoSize = true;
            this.postRadio.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.postRadio.Location = new System.Drawing.Point(6, 42);
            this.postRadio.Name = "postRadio";
            this.postRadio.Size = new System.Drawing.Size(51, 17);
            this.postRadio.TabIndex = 15;
            this.postRadio.TabStop = true;
            this.postRadio.Text = "POST";
            this.postRadio.UseVisualStyleBackColor = true;
            // 
            // cookieGroup
            // 
            this.cookieGroup.Controls.Add(this.cookieVarTb);
            this.cookieGroup.Controls.Add(this.button3);
            this.cookieGroup.Controls.Add(this.button4);
            this.cookieGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cookieGroup.Location = new System.Drawing.Point(238, 267);
            this.cookieGroup.Name = "cookieGroup";
            this.cookieGroup.Size = new System.Drawing.Size(169, 108);
            this.cookieGroup.TabIndex = 21;
            this.cookieGroup.TabStop = false;
            this.cookieGroup.Text = "COOKIE Variable";
            // 
            // cookieVarTb
            // 
            this.cookieVarTb.Location = new System.Drawing.Point(7, 18);
            this.cookieVarTb.Name = "cookieVarTb";
            this.cookieVarTb.ReadOnly = true;
            this.cookieVarTb.Size = new System.Drawing.Size(152, 21);
            this.cookieVarTb.TabIndex = 17;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.button4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(9, 46);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(71, 31);
            this.button4.TabIndex = 15;
            this.button4.Text = "Browse...";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // httpparams
            // 
            this.httpparams.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.httpparams.Location = new System.Drawing.Point(442, 147);
            this.httpparams.Name = "httpparams";
            this.httpparams.Size = new System.Drawing.Size(152, 60);
            this.httpparams.TabIndex = 22;
            this.httpparams.Text = "HTTP Parameters...";
            this.httpparams.UseVisualStyleBackColor = true;
            this.httpparams.Click += new System.EventHandler(this.httpparams_Click);
            // 
            // variableR
            // 
            this.variableR.AutoSize = true;
            this.variableR.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variableR.Location = new System.Drawing.Point(207, 15);
            this.variableR.Name = "variableR";
            this.variableR.Size = new System.Drawing.Size(90, 17);
            this.variableR.TabIndex = 24;
            this.variableR.Text = "From variable";
            this.variableR.UseVisualStyleBackColor = true;
            this.variableR.CheckedChanged += new System.EventHandler(this.direct_variable_check_change);
            // 
            // directInputR
            // 
            this.directInputR.AutoSize = true;
            this.directInputR.Checked = true;
            this.directInputR.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.directInputR.Location = new System.Drawing.Point(6, 15);
            this.directInputR.Name = "directInputR";
            this.directInputR.Size = new System.Drawing.Size(90, 17);
            this.directInputR.TabIndex = 25;
            this.directInputR.TabStop = true;
            this.directInputR.Tag = "";
            this.directInputR.Text = "Custom Value";
            this.directInputR.UseVisualStyleBackColor = true;
            this.directInputR.CheckedChanged += new System.EventHandler(this.direct_variable_check_change);
            // 
            // browseButton
            // 
            this.browseButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseButton.Location = new System.Drawing.Point(19, 230);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(213, 34);
            this.browseButton.TabIndex = 27;
            this.browseButton.Text = "Browse...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // addButton
            // 
            this.addButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addButton.Location = new System.Drawing.Point(238, 230);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(198, 34);
            this.addButton.TabIndex = 28;
            this.addButton.Text = "Add new...";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Visible = false;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // uiCustomUrlGroup
            // 
            this.uiCustomUrlGroup.Controls.Add(this.urlInputModeGb);
            this.uiCustomUrlGroup.Controls.Add(this.inputLaneCb);
            this.uiCustomUrlGroup.Controls.Add(this.header_button);
            this.uiCustomUrlGroup.Controls.Add(this.richTextBox1);
            this.uiCustomUrlGroup.Controls.Add(this.addButton);
            this.uiCustomUrlGroup.Controls.Add(this.browseButton);
            this.uiCustomUrlGroup.Controls.Add(this.httpparams);
            this.uiCustomUrlGroup.Controls.Add(this.cookieGroup);
            this.uiCustomUrlGroup.Controls.Add(this.groupBox1);
            this.uiCustomUrlGroup.Controls.Add(this.uiWebURL);
            this.uiCustomUrlGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiCustomUrlGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiCustomUrlGroup.Location = new System.Drawing.Point(0, 0);
            this.uiCustomUrlGroup.Name = "uiCustomUrlGroup";
            this.uiCustomUrlGroup.Size = new System.Drawing.Size(600, 400);
            this.uiCustomUrlGroup.TabIndex = 15;
            this.uiCustomUrlGroup.TabStop = false;
            this.uiCustomUrlGroup.Text = "Data source configuration";
            // 
            // header_button
            // 
            this.header_button.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.header_button.Location = new System.Drawing.Point(442, 213);
            this.header_button.Name = "header_button";
            this.header_button.Size = new System.Drawing.Size(152, 51);
            this.header_button.TabIndex = 30;
            this.header_button.Text = "HTTP Headers...";
            this.header_button.UseVisualStyleBackColor = true;
            this.header_button.Click += new System.EventHandler(this.button1_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(19, 20);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(562, 85);
            this.richTextBox1.TabIndex = 29;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "JSON File|*.json|Txt files|*.txt|All files|*.*";
            this.openFileDialog1.ShowReadOnly = true;
            // 
            // inputLaneR
            // 
            this.inputLaneR.AutoSize = true;
            this.inputLaneR.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputLaneR.Location = new System.Drawing.Point(102, 15);
            this.inputLaneR.Name = "inputLaneR";
            this.inputLaneR.Size = new System.Drawing.Size(99, 17);
            this.inputLaneR.TabIndex = 31;
            this.inputLaneR.Text = "From input lane";
            this.inputLaneR.UseVisualStyleBackColor = true;
            // 
            // inputLaneCb
            // 
            this.inputLaneCb.Enabled = false;
            this.inputLaneCb.FormattingEnabled = true;
            this.inputLaneCb.Location = new System.Drawing.Point(19, 147);
            this.inputLaneCb.Name = "inputLaneCb";
            this.inputLaneCb.Size = new System.Drawing.Size(417, 21);
            this.inputLaneCb.TabIndex = 32;
            this.inputLaneCb.Visible = false;
            // 
            // urlInputModeGb
            // 
            this.urlInputModeGb.Controls.Add(this.directInputR);
            this.urlInputModeGb.Controls.Add(this.inputLaneR);
            this.urlInputModeGb.Controls.Add(this.variableR);
            this.urlInputModeGb.Location = new System.Drawing.Point(19, 103);
            this.urlInputModeGb.Name = "urlInputModeGb";
            this.urlInputModeGb.Size = new System.Drawing.Size(417, 38);
            this.urlInputModeGb.TabIndex = 33;
            this.urlInputModeGb.TabStop = false;
            this.urlInputModeGb.Text = "URL Input:";
            // 
            // SourceView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uiCustomUrlGroup);
            this.Name = "SourceView";
            this.Size = new System.Drawing.Size(600, 400);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.cookieGroup.ResumeLayout(false);
            this.cookieGroup.PerformLayout();
            this.uiCustomUrlGroup.ResumeLayout(false);
            this.uiCustomUrlGroup.PerformLayout();
            this.urlInputModeGb.ResumeLayout(false);
            this.urlInputModeGb.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TextBox uiWebURL;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton delRadio;
        public System.Windows.Forms.RadioButton getRadio;
        public System.Windows.Forms.RadioButton putRadio;
        public System.Windows.Forms.RadioButton postRadio;
        private System.Windows.Forms.GroupBox cookieGroup;
        public System.Windows.Forms.TextBox cookieVarTb;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button httpparams;
        private System.Windows.Forms.RadioButton variableR;
        private System.Windows.Forms.RadioButton directInputR;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.GroupBox uiCustomUrlGroup;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button header_button;
        private System.Windows.Forms.RadioButton inputLaneR;
        private System.Windows.Forms.ComboBox inputLaneCb;
        private System.Windows.Forms.GroupBox urlInputModeGb;
    }
}
