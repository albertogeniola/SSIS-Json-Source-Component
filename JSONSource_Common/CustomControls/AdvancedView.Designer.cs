namespace com.webkingsoft.JSONSource_Common
{
    partial class AdvancedView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedView));
            this.label3 = new System.Windows.Forms.Label();
            this.uiTempDir = new System.Windows.Forms.TextBox();
            this.tmpBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(210, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Local temp dir (if different from system one):";
            // 
            // uiTempDir
            // 
            this.uiTempDir.Location = new System.Drawing.Point(18, 26);
            this.uiTempDir.Name = "uiTempDir";
            this.uiTempDir.Size = new System.Drawing.Size(333, 20);
            this.uiTempDir.TabIndex = 8;
            // 
            // tmpBrowse
            // 
            this.tmpBrowse.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tmpBrowse.BackgroundImage")));
            this.tmpBrowse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tmpBrowse.Location = new System.Drawing.Point(357, 23);
            this.tmpBrowse.Name = "tmpBrowse";
            this.tmpBrowse.Size = new System.Drawing.Size(26, 24);
            this.tmpBrowse.TabIndex = 9;
            this.tmpBrowse.UseVisualStyleBackColor = true;
            this.tmpBrowse.Click += new System.EventHandler(this.tmpBrowse_Click);
            // 
            // AdvancedView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tmpBrowse);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.uiTempDir);
            this.Name = "AdvancedView";
            this.Size = new System.Drawing.Size(398, 60);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button tmpBrowse;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox uiTempDir;
    }
}
