namespace com.webkingsoft.JSONSource_120
{
    partial class WebPathSource
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
            this.uiWebURLCustom = new System.Windows.Forms.TextBox();
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
            this.uiCustomUrlGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.cookieGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiCustomUrlGroup
            // 
            this.uiCustomUrlGroup.Controls.Add(this.uiWebURLCustom);
            this.uiCustomUrlGroup.Location = new System.Drawing.Point(3, 3);
            this.uiCustomUrlGroup.Name = "uiCustomUrlGroup";
            this.uiCustomUrlGroup.Size = new System.Drawing.Size(339, 108);
            this.uiCustomUrlGroup.TabIndex = 15;
            this.uiCustomUrlGroup.TabStop = false;
            this.uiCustomUrlGroup.Text = "Pre-Defined URL";
            // 
            // uiWebURLCustom
            // 
            this.uiWebURLCustom.Location = new System.Drawing.Point(13, 20);
            this.uiWebURLCustom.Multiline = true;
            this.uiWebURLCustom.Name = "uiWebURLCustom";
            this.uiWebURLCustom.Size = new System.Drawing.Size(316, 73);
            this.uiWebURLCustom.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.delRadio);
            this.groupBox1.Controls.Add(this.getRadio);
            this.groupBox1.Controls.Add(this.putRadio);
            this.groupBox1.Controls.Add(this.postRadio);
            this.groupBox1.Location = new System.Drawing.Point(3, 117);
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
            // cookieGroup
            // 
            this.cookieGroup.Controls.Add(this.cookieVarTb);
            this.cookieGroup.Controls.Add(this.button3);
            this.cookieGroup.Controls.Add(this.button4);
            this.cookieGroup.Location = new System.Drawing.Point(173, 117);
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
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(9, 46);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(71, 31);
            this.button4.TabIndex = 15;
            this.button4.Text = "Browse...";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // httpparams
            // 
            this.httpparams.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.httpparams.Location = new System.Drawing.Point(3, 228);
            this.httpparams.Name = "httpparams";
            this.httpparams.Size = new System.Drawing.Size(339, 23);
            this.httpparams.TabIndex = 22;
            this.httpparams.Text = "HTTP Parameters...";
            this.httpparams.UseVisualStyleBackColor = true;
            this.httpparams.Click += new System.EventHandler(this.httpparams_Click);
            // 
            // WebPathSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.httpparams);
            this.Controls.Add(this.cookieGroup);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.uiCustomUrlGroup);
            this.Name = "WebPathSource";
            this.Size = new System.Drawing.Size(351, 258);
            this.uiCustomUrlGroup.ResumeLayout(false);
            this.uiCustomUrlGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.cookieGroup.ResumeLayout(false);
            this.cookieGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox uiCustomUrlGroup;
        private System.Windows.Forms.TextBox uiWebURLCustom;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton delRadio;
        private System.Windows.Forms.RadioButton getRadio;
        private System.Windows.Forms.RadioButton putRadio;
        private System.Windows.Forms.RadioButton postRadio;
        private System.Windows.Forms.GroupBox cookieGroup;
        private System.Windows.Forms.TextBox cookieVarTb;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button httpparams;
    }
}
