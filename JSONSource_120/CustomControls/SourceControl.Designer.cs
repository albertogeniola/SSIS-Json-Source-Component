namespace com.webkingsoft.JSONSource_120.CustomControls
{
    partial class SourceControl
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
            this.label5 = new System.Windows.Forms.Label();
            this.uiSourceType = new System.Windows.Forms.ComboBox();
            this.main = new System.Windows.Forms.Panel();
            this.top.SuspendLayout();
            this.SuspendLayout();
            // 
            // top
            // 
            this.top.Controls.Add(this.label5);
            this.top.Controls.Add(this.uiSourceType);
            this.top.Dock = System.Windows.Forms.DockStyle.Top;
            this.top.Location = new System.Drawing.Point(0, 0);
            this.top.Name = "top";
            this.top.Size = new System.Drawing.Size(689, 65);
            this.top.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Source Type:";
            // 
            // uiSourceType
            // 
            this.uiSourceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uiSourceType.FormattingEnabled = true;
            this.uiSourceType.Location = new System.Drawing.Point(17, 28);
            this.uiSourceType.Name = "uiSourceType";
            this.uiSourceType.Size = new System.Drawing.Size(221, 21);
            this.uiSourceType.TabIndex = 7;
            this.uiSourceType.SelectedIndexChanged += new System.EventHandler(this.uiSourceType_SelectedIndexChanged);
            // 
            // main
            // 
            this.main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main.Location = new System.Drawing.Point(0, 65);
            this.main.Name = "main";
            this.main.Size = new System.Drawing.Size(689, 298);
            this.main.TabIndex = 1;
            // 
            // SourceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.main);
            this.Controls.Add(this.top);
            this.Name = "SourceControl";
            this.Size = new System.Drawing.Size(689, 363);
            this.top.ResumeLayout(false);
            this.top.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel top;
        private System.Windows.Forms.Panel main;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.ComboBox uiSourceType;

    }
}
