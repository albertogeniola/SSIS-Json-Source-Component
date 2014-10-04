namespace JSONSource
{
    partial class VariableChooser
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
            this.label1 = new System.Windows.Forms.Label();
            this.varList = new System.Windows.Forms.ComboBox();
            this.SelectButton = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the input variable:";
            // 
            // varList
            // 
            this.varList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.varList.FormattingEnabled = true;
            this.varList.Location = new System.Drawing.Point(16, 47);
            this.varList.Name = "varList";
            this.varList.Size = new System.Drawing.Size(264, 21);
            this.varList.TabIndex = 1;
            // 
            // SelectButton
            // 
            this.SelectButton.Location = new System.Drawing.Point(189, 101);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(90, 34);
            this.SelectButton.TabIndex = 2;
            this.SelectButton.Text = "Select";
            this.SelectButton.UseVisualStyleBackColor = true;
            this.SelectButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(93, 101);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(90, 34);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // VariableChooser
            // 
            this.AcceptButton = this.SelectButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 147);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.SelectButton);
            this.Controls.Add(this.varList);
            this.Controls.Add(this.label1);
            this.Name = "VariableChooser";
            this.Text = "VariableChooser";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.VariableChooser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox varList;
        private System.Windows.Forms.Button SelectButton;
        private System.Windows.Forms.Button CancelBtn;
    }
}