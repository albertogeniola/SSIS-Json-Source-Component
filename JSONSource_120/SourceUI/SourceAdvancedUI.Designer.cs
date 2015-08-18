namespace com.webkingsoft.JSONSource_120.SourceUI
{
    partial class SourceAdvancedUI
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
            this.topPanel = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.menulist = new System.Windows.Forms.ListBox();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.YellowGreen;
            this.topPanel.Controls.Add(this.richTextBox1);
            this.topPanel.Controls.Add(this.pictureBox1);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(683, 104);
            this.topPanel.TabIndex = 0;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.YellowGreen;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.richTextBox1.Location = new System.Drawing.Point(120, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(563, 104);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "You may set up the JSON Source component by editing settings in this window. \nDoc" +
    "umentation at http://www.codeplex.com/jsonsource\nAuthor: Alberto Geniola, albert" +
    "ogeniola@gmail.com";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.YellowGreen;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = global::com.webkingsoft.JSONSource_120.Properties.Resources._256x256_96x96;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(120, 104);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.cancel);
            this.bottomPanel.Controls.Add(this.ok);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 513);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(683, 42);
            this.bottomPanel.TabIndex = 1;
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(483, 3);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(92, 36);
            this.cancel.TabIndex = 10;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(581, 3);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(92, 36);
            this.ok.TabIndex = 9;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // menulist
            // 
            this.menulist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.menulist.Dock = System.Windows.Forms.DockStyle.Left;
            this.menulist.FormattingEnabled = true;
            this.menulist.Items.AddRange(new object[] {
            "Settings",
            "Columns",
            "Advanced"});
            this.menulist.Location = new System.Drawing.Point(0, 104);
            this.menulist.Name = "menulist";
            this.menulist.Size = new System.Drawing.Size(120, 409);
            this.menulist.TabIndex = 2;
            this.menulist.SelectedIndexChanged += new System.EventHandler(this.menulist_SelectedIndexChanged);
            // 
            // mainPanel
            // 
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(120, 104);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(563, 409);
            this.mainPanel.TabIndex = 3;
            // 
            // SourceAdvancedUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 555);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.menulist);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.topPanel);
            this.Name = "SourceAdvancedUI";
            this.Text = "SourceAdvancedUI";
            this.topPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.ListBox menulist;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}