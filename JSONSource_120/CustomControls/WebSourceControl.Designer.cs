namespace com.webkingsoft.JSONSource_120.CustomControls
{
    partial class WebSourceControl
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
            this.uiCustomUrlGroup = new System.Windows.Forms.GroupBox();
            this.addButton = new System.Windows.Forms.Button();
            this.browseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.directInputR = new System.Windows.Forms.RadioButton();
            this.variableR = new System.Windows.Forms.RadioButton();
            this.uiTestWebURL = new System.Windows.Forms.Button();
            this.httpparams = new System.Windows.Forms.Button();
            this.cookieGroup = new System.Windows.Forms.GroupBox();
            this.cookieVarTb = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.delRadio = new System.Windows.Forms.RadioButton();
            this.getRadio = new System.Windows.Forms.RadioButton();
            this.putRadio = new System.Windows.Forms.RadioButton();
            this.postRadio = new System.Windows.Forms.RadioButton();
            this.uiWebURL = new System.Windows.Forms.TextBox();
            this.uiCustomUrlGroup.SuspendLayout();
            this.cookieGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiCustomUrlGroup
            // 
            this.uiCustomUrlGroup.Controls.Add(this.addButton);
            this.uiCustomUrlGroup.Controls.Add(this.browseButton);
            this.uiCustomUrlGroup.Controls.Add(this.label1);
            this.uiCustomUrlGroup.Controls.Add(this.directInputR);
            this.uiCustomUrlGroup.Controls.Add(this.variableR);
            this.uiCustomUrlGroup.Controls.Add(this.uiTestWebURL);
            this.uiCustomUrlGroup.Controls.Add(this.httpparams);
            this.uiCustomUrlGroup.Controls.Add(this.cookieGroup);
            this.uiCustomUrlGroup.Controls.Add(this.groupBox1);
            this.uiCustomUrlGroup.Controls.Add(this.uiWebURL);
            this.uiCustomUrlGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiCustomUrlGroup.Location = new System.Drawing.Point(0, 0);
            this.uiCustomUrlGroup.Name = "uiCustomUrlGroup";
            this.uiCustomUrlGroup.Size = new System.Drawing.Size(520, 187);
            this.uiCustomUrlGroup.TabIndex = 15;
            this.uiCustomUrlGroup.TabStop = false;
            this.uiCustomUrlGroup.Text = "Pre-Defined URL";
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(436, 37);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(74, 25);
            this.addButton.TabIndex = 28;
            this.addButton.Text = "Add new...";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Visible = false;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(359, 37);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(71, 25);
            this.browseButton.TabIndex = 27;
            this.browseButton.Text = "Browse...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Visible = false;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Mode:";
            // 
            // directInputR
            // 
            this.directInputR.AutoSize = true;
            this.directInputR.Checked = true;
            this.directInputR.Location = new System.Drawing.Point(59, 14);
            this.directInputR.Name = "directInputR";
            this.directInputR.Size = new System.Drawing.Size(80, 17);
            this.directInputR.TabIndex = 25;
            this.directInputR.TabStop = true;
            this.directInputR.Text = "Direct Input";
            this.directInputR.UseVisualStyleBackColor = true;
            this.directInputR.CheckedChanged += new System.EventHandler(this.directInputR_CheckedChanged);
            // 
            // variableR
            // 
            this.variableR.AutoSize = true;
            this.variableR.Location = new System.Drawing.Point(145, 14);
            this.variableR.Name = "variableR";
            this.variableR.Size = new System.Drawing.Size(63, 17);
            this.variableR.TabIndex = 24;
            this.variableR.Text = "Variable";
            this.variableR.UseVisualStyleBackColor = true;
            this.variableR.CheckedChanged += new System.EventHandler(this.variableR_CheckedChanged);
            // 
            // uiTestWebURL
            // 
            this.uiTestWebURL.Location = new System.Drawing.Point(358, 133);
            this.uiTestWebURL.Name = "uiTestWebURL";
            this.uiTestWebURL.Size = new System.Drawing.Size(152, 40);
            this.uiTestWebURL.TabIndex = 23;
            this.uiTestWebURL.Text = "Test...";
            this.uiTestWebURL.UseVisualStyleBackColor = true;
            // 
            // httpparams
            // 
            this.httpparams.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.httpparams.Location = new System.Drawing.Point(358, 68);
            this.httpparams.Name = "httpparams";
            this.httpparams.Size = new System.Drawing.Size(152, 64);
            this.httpparams.TabIndex = 22;
            this.httpparams.Text = "HTTP Parameters...";
            this.httpparams.UseVisualStyleBackColor = true;
            this.httpparams.Click += new System.EventHandler(this.httpparams_Click);
            // 
            // cookieGroup
            // 
            this.cookieGroup.Controls.Add(this.cookieVarTb);
            this.cookieGroup.Controls.Add(this.button3);
            this.cookieGroup.Controls.Add(this.button4);
            this.cookieGroup.Location = new System.Drawing.Point(183, 68);
            this.cookieGroup.Name = "cookieGroup";
            this.cookieGroup.Size = new System.Drawing.Size(169, 108);
            this.cookieGroup.TabIndex = 21;
            this.cookieGroup.TabStop = false;
            this.cookieGroup.Text = "COOKIE Variable";
            // 
            // cookieVarTb
            // 
            this.cookieVarTb.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
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
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.delRadio);
            this.groupBox1.Controls.Add(this.getRadio);
            this.groupBox1.Controls.Add(this.putRadio);
            this.groupBox1.Controls.Add(this.postRadio);
            this.groupBox1.Location = new System.Drawing.Point(13, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(164, 108);
            this.groupBox1.TabIndex = 20;
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
            // uiWebURL
            // 
            this.uiWebURL.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.uiWebURL.Location = new System.Drawing.Point(13, 37);
            this.uiWebURL.Multiline = true;
            this.uiWebURL.Name = "uiWebURL";
            this.uiWebURL.Size = new System.Drawing.Size(339, 25);
            this.uiWebURL.TabIndex = 1;
            // 
            // WebSourceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uiCustomUrlGroup);
            this.Name = "WebSourceControl";
            this.Size = new System.Drawing.Size(520, 187);
            this.uiCustomUrlGroup.ResumeLayout(false);
            this.uiCustomUrlGroup.PerformLayout();
            this.cookieGroup.ResumeLayout(false);
            this.cookieGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox uiCustomUrlGroup;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox cookieGroup;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button httpparams;
        private System.Windows.Forms.Button uiTestWebURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton directInputR;
        private System.Windows.Forms.RadioButton variableR;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button browseButton;
        public System.Windows.Forms.TextBox uiWebURL;
        public System.Windows.Forms.RadioButton delRadio;
        public System.Windows.Forms.RadioButton getRadio;
        public System.Windows.Forms.RadioButton putRadio;
        public System.Windows.Forms.RadioButton postRadio;
        public System.Windows.Forms.TextBox cookieVarTb;
    }
}
