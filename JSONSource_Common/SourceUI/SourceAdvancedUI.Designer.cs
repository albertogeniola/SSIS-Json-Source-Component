namespace com.webkingsoft.JSONSource_Common
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SourceAdvancedUI));
            this.topPanel = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.testButton = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.menulist = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelComponentVersion = new System.Windows.Forms.Label();
            this.labelMetadataVersion = new System.Windows.Forms.Label();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.White;
            this.topPanel.Controls.Add(this.richTextBox1);
            this.topPanel.Controls.Add(this.pictureBox1);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(792, 104);
            this.topPanel.TabIndex = 0;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBox1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.richTextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.richTextBox1.Location = new System.Drawing.Point(134, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Size = new System.Drawing.Size(648, 86);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "You may set up the JSON Source component by editing settings in this window. \nDoc" +
    "umentation at http://www.codeplex.com/jsonsource\nAuthor: Alberto Geniola.";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 104);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.labelMetadataVersion);
            this.bottomPanel.Controls.Add(this.labelComponentVersion);
            this.bottomPanel.Controls.Add(this.label2);
            this.bottomPanel.Controls.Add(this.label1);
            this.bottomPanel.Controls.Add(this.testButton);
            this.bottomPanel.Controls.Add(this.cancel);
            this.bottomPanel.Controls.Add(this.ok);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 531);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(792, 42);
            this.bottomPanel.TabIndex = 1;
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(197, 3);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(75, 36);
            this.testButton.TabIndex = 11;
            this.testButton.Text = "Test...";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(599, 3);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(92, 36);
            this.cancel.TabIndex = 10;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(697, 3);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(92, 36);
            this.ok.TabIndex = 9;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.Location = new System.Drawing.Point(128, 104);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(664, 400);
            this.mainPanel.TabIndex = 3;
            // 
            // menulist
            // 
            this.menulist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.menulist.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menulist.FormattingEnabled = true;
            this.menulist.ItemHeight = 19;
            this.menulist.Items.AddRange(new object[] {
            "Settings",
            "Columns",
            "Advanced"});
            this.menulist.Location = new System.Drawing.Point(12, 8);
            this.menulist.Name = "menulist";
            this.menulist.Size = new System.Drawing.Size(108, 382);
            this.menulist.TabIndex = 2;
            this.menulist.SelectedIndexChanged += new System.EventHandler(this.menulist_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.menulist);
            this.panel1.Location = new System.Drawing.Point(0, 104);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(128, 409);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Metadata version:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Component version:";
            // 
            // labelComponentVersion
            // 
            this.labelComponentVersion.AutoSize = true;
            this.labelComponentVersion.Location = new System.Drawing.Point(111, 20);
            this.labelComponentVersion.Name = "labelComponentVersion";
            this.labelComponentVersion.Size = new System.Drawing.Size(27, 13);
            this.labelComponentVersion.TabIndex = 14;
            this.labelComponentVersion.Text = "N/A";
            // 
            // labelMetadataVersion
            // 
            this.labelMetadataVersion.AutoSize = true;
            this.labelMetadataVersion.Location = new System.Drawing.Point(111, 3);
            this.labelMetadataVersion.Name = "labelMetadataVersion";
            this.labelMetadataVersion.Size = new System.Drawing.Size(27, 13);
            this.labelMetadataVersion.TabIndex = 15;
            this.labelMetadataVersion.Text = "N/A";
            // 
            // SourceAdvancedUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.topPanel);
            this.Name = "SourceAdvancedUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SourceAdvancedUI";
            this.topPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.ListBox menulist;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.Label labelMetadataVersion;
        private System.Windows.Forms.Label labelComponentVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}