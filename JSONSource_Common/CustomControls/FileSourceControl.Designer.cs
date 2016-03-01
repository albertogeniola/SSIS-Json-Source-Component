namespace com.webkingsoft.JSONSource_Common
{
    partial class FileSourceControl
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
            this.uiBrowseFilePathVariable = new System.Windows.Forms.Button();
            this.addVarByutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.directInputR = new System.Windows.Forms.RadioButton();
            this.variableR = new System.Windows.Forms.RadioButton();
            this.jsonFilePath = new System.Windows.Forms.TextBox();
            this.uiBrowseFilePath = new System.Windows.Forms.Button();
            this.uiCustomUrlGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiCustomUrlGroup
            // 
            this.uiCustomUrlGroup.Controls.Add(this.uiBrowseFilePathVariable);
            this.uiCustomUrlGroup.Controls.Add(this.addVarByutton);
            this.uiCustomUrlGroup.Controls.Add(this.label1);
            this.uiCustomUrlGroup.Controls.Add(this.directInputR);
            this.uiCustomUrlGroup.Controls.Add(this.variableR);
            this.uiCustomUrlGroup.Controls.Add(this.uiBrowseFilePath);
            this.uiCustomUrlGroup.Controls.Add(this.jsonFilePath);
            this.uiCustomUrlGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiCustomUrlGroup.Location = new System.Drawing.Point(0, 0);
            this.uiCustomUrlGroup.Name = "uiCustomUrlGroup";
            this.uiCustomUrlGroup.Size = new System.Drawing.Size(409, 130);
            this.uiCustomUrlGroup.TabIndex = 15;
            this.uiCustomUrlGroup.TabStop = false;
            this.uiCustomUrlGroup.Text = "JSON file path";
            // 
            // uiBrowseFilePathVariable
            // 
            this.uiBrowseFilePathVariable.Location = new System.Drawing.Point(19, 77);
            this.uiBrowseFilePathVariable.Name = "uiBrowseFilePathVariable";
            this.uiBrowseFilePathVariable.Size = new System.Drawing.Size(94, 39);
            this.uiBrowseFilePathVariable.TabIndex = 31;
            this.uiBrowseFilePathVariable.Text = "Browse...";
            this.uiBrowseFilePathVariable.UseVisualStyleBackColor = true;
            this.uiBrowseFilePathVariable.Visible = false;
            this.uiBrowseFilePathVariable.Click += new System.EventHandler(this.uiBrowseFilePathVariable_Click);
            // 
            // addVarByutton
            // 
            this.addVarByutton.Location = new System.Drawing.Point(119, 77);
            this.addVarByutton.Name = "addVarByutton";
            this.addVarByutton.Size = new System.Drawing.Size(86, 39);
            this.addVarByutton.TabIndex = 30;
            this.addVarByutton.Text = "Add new...";
            this.addVarByutton.UseVisualStyleBackColor = true;
            this.addVarByutton.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Mode:";
            // 
            // directInputR
            // 
            this.directInputR.AutoSize = true;
            this.directInputR.Checked = true;
            this.directInputR.Location = new System.Drawing.Point(59, 28);
            this.directInputR.Name = "directInputR";
            this.directInputR.Size = new System.Drawing.Size(80, 17);
            this.directInputR.TabIndex = 28;
            this.directInputR.TabStop = true;
            this.directInputR.Text = "Direct Input";
            this.directInputR.UseVisualStyleBackColor = true;
            this.directInputR.CheckedChanged += new System.EventHandler(this.directInputR_CheckedChanged);
            // 
            // variableR
            // 
            this.variableR.AutoSize = true;
            this.variableR.Location = new System.Drawing.Point(145, 28);
            this.variableR.Name = "variableR";
            this.variableR.Size = new System.Drawing.Size(63, 17);
            this.variableR.TabIndex = 27;
            this.variableR.Text = "Variable";
            this.variableR.UseVisualStyleBackColor = true;
            this.variableR.CheckedChanged += new System.EventHandler(this.variableR_CheckedChanged);
            // 
            // jsonFilePath
            // 
            this.jsonFilePath.Location = new System.Drawing.Point(19, 46);
            this.jsonFilePath.Multiline = true;
            this.jsonFilePath.Name = "jsonFilePath";
            this.jsonFilePath.Size = new System.Drawing.Size(371, 25);
            this.jsonFilePath.TabIndex = 1;
            // 
            // uiBrowseFilePath
            // 
            this.uiBrowseFilePath.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiBrowseFilePath.Location = new System.Drawing.Point(19, 77);
            this.uiBrowseFilePath.Name = "uiBrowseFilePath";
            this.uiBrowseFilePath.Size = new System.Drawing.Size(94, 39);
            this.uiBrowseFilePath.TabIndex = 13;
            this.uiBrowseFilePath.Text = "Browse...";
            this.uiBrowseFilePath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.uiBrowseFilePath.UseVisualStyleBackColor = true;
            this.uiBrowseFilePath.Click += new System.EventHandler(this.uiBrowseFilePath_Click);
            // 
            // FileSourceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uiCustomUrlGroup);
            this.Name = "FileSourceControl";
            this.Size = new System.Drawing.Size(409, 130);
            this.uiCustomUrlGroup.ResumeLayout(false);
            this.uiCustomUrlGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox uiCustomUrlGroup;
        private System.Windows.Forms.Button uiBrowseFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton directInputR;
        private System.Windows.Forms.RadioButton variableR;
        private System.Windows.Forms.Button addVarByutton;
        private System.Windows.Forms.Button uiBrowseFilePathVariable;
        public System.Windows.Forms.TextBox jsonFilePath;
    }
}
