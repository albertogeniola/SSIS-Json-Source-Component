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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedView));
            this.label3 = new System.Windows.Forms.Label();
            this.uiTempDir = new System.Windows.Forms.TextBox();
            this.tmpBrowse = new System.Windows.Forms.Button();
            this.cbParseJsonDate = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.waitTimeLabel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.retryLabel = new System.Windows.Forms.TextBox();
            this.retryR = new System.Windows.Forms.RadioButton();
            this.skipR = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.stopImmediatelyR = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.httpErrorHandlingGv = new System.Windows.Forms.DataGridView();
            this.status_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handling_policy = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.retry_attempts = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SleepTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.httpErrorHandlingGv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 333);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Custom temporary directory:";
            // 
            // uiTempDir
            // 
            this.uiTempDir.Location = new System.Drawing.Point(9, 350);
            this.uiTempDir.Name = "uiTempDir";
            this.uiTempDir.Size = new System.Drawing.Size(333, 20);
            this.uiTempDir.TabIndex = 8;
            // 
            // tmpBrowse
            // 
            this.tmpBrowse.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tmpBrowse.BackgroundImage")));
            this.tmpBrowse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tmpBrowse.Location = new System.Drawing.Point(345, 346);
            this.tmpBrowse.Name = "tmpBrowse";
            this.tmpBrowse.Size = new System.Drawing.Size(26, 24);
            this.tmpBrowse.TabIndex = 9;
            this.tmpBrowse.UseVisualStyleBackColor = true;
            this.tmpBrowse.Click += new System.EventHandler(this.tmpBrowse_Click);
            // 
            // cbParseJsonDate
            // 
            this.cbParseJsonDate.Location = new System.Drawing.Point(6, 19);
            this.cbParseJsonDate.Name = "cbParseJsonDate";
            this.cbParseJsonDate.Size = new System.Drawing.Size(473, 51);
            this.cbParseJsonDate.TabIndex = 10;
            this.cbParseJsonDate.Text = "Parse Json dates: json.net library has some problem with timezone conversion. Che" +
    "ck this in order to perfom manual dates conversion.";
            this.cbParseJsonDate.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbParseJsonDate);
            this.groupBox1.Location = new System.Drawing.Point(9, 254);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(513, 76);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Json Parsing";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.waitTimeLabel);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.retryLabel);
            this.groupBox2.Controls.Add(this.retryR);
            this.groupBox2.Controls.Add(this.skipR);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.stopImmediatelyR);
            this.groupBox2.Location = new System.Drawing.Point(9, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(513, 118);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Network error handling";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(327, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "seconds.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(262, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Wait time among failures:";
            // 
            // waitTimeLabel
            // 
            this.waitTimeLabel.Location = new System.Drawing.Point(265, 88);
            this.waitTimeLabel.Name = "waitTimeLabel";
            this.waitTimeLabel.Size = new System.Drawing.Size(56, 20);
            this.waitTimeLabel.TabIndex = 6;
            this.waitTimeLabel.Validating += new System.ComponentModel.CancelEventHandler(this.validate_positive_integer);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(102, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "times (0 indicates forever)";
            // 
            // retryLabel
            // 
            this.retryLabel.Location = new System.Drawing.Point(76, 90);
            this.retryLabel.Name = "retryLabel";
            this.retryLabel.Size = new System.Drawing.Size(24, 20);
            this.retryLabel.TabIndex = 4;
            this.retryLabel.Validating += new System.ComponentModel.CancelEventHandler(this.validate_positive_integer);
            // 
            // retryR
            // 
            this.retryR.AutoSize = true;
            this.retryR.Location = new System.Drawing.Point(12, 91);
            this.retryR.Name = "retryR";
            this.retryR.Size = new System.Drawing.Size(68, 17);
            this.retryR.TabIndex = 3;
            this.retryR.Text = "Retry for:";
            this.retryR.UseVisualStyleBackColor = true;
            this.retryR.CheckedChanged += new System.EventHandler(this.network_error_policy_CheckedChanged);
            // 
            // skipR
            // 
            this.skipR.AutoSize = true;
            this.skipR.Location = new System.Drawing.Point(12, 68);
            this.skipR.Name = "skipR";
            this.skipR.Size = new System.Drawing.Size(143, 17);
            this.skipR.TabIndex = 2;
            this.skipR.Text = "Move to the next request";
            this.skipR.UseVisualStyleBackColor = true;
            this.skipR.CheckedChanged += new System.EventHandler(this.network_error_policy_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose how to react in case of network errors.";
            // 
            // stopImmediatelyR
            // 
            this.stopImmediatelyR.AutoSize = true;
            this.stopImmediatelyR.Location = new System.Drawing.Point(12, 45);
            this.stopImmediatelyR.Name = "stopImmediatelyR";
            this.stopImmediatelyR.Size = new System.Drawing.Size(103, 17);
            this.stopImmediatelyR.TabIndex = 0;
            this.stopImmediatelyR.Text = "Immediately stop";
            this.stopImmediatelyR.UseVisualStyleBackColor = true;
            this.stopImmediatelyR.CheckedChanged += new System.EventHandler(this.network_error_policy_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.httpErrorHandlingGv);
            this.groupBox3.Location = new System.Drawing.Point(6, 127);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(516, 121);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Http Status Code Handling";
            // 
            // httpErrorHandlingGv
            // 
            this.httpErrorHandlingGv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.httpErrorHandlingGv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.httpErrorHandlingGv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.status_code,
            this.handling_policy,
            this.retry_attempts,
            this.SleepTime});
            this.httpErrorHandlingGv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.httpErrorHandlingGv.Location = new System.Drawing.Point(3, 16);
            this.httpErrorHandlingGv.Name = "httpErrorHandlingGv";
            this.httpErrorHandlingGv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.httpErrorHandlingGv.Size = new System.Drawing.Size(510, 102);
            this.httpErrorHandlingGv.TabIndex = 0;
            this.httpErrorHandlingGv.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.httpErrorHandlingGv_CellValidated);
            this.httpErrorHandlingGv.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.cell_validation);
            this.httpErrorHandlingGv.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.httpErrorHandlingGv_RowValidated);
            this.httpErrorHandlingGv.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.httpErrorHandlingGv_RowValidating);
            // 
            // status_code
            // 
            this.status_code.HeaderText = "HTTP Status Code";
            this.status_code.Name = "status_code";
            // 
            // handling_policy
            // 
            this.handling_policy.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.handling_policy.HeaderText = "Handling Policy";
            this.handling_policy.Name = "handling_policy";
            // 
            // retry_attempts
            // 
            this.retry_attempts.HeaderText = "Retry Attempts";
            this.retry_attempts.Name = "retry_attempts";
            // 
            // SleepTime
            // 
            this.SleepTime.HeaderText = "Wait Time in Seconds";
            this.SleepTime.Name = "SleepTime";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // AdvancedView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tmpBrowse);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.uiTempDir);
            this.Name = "AdvancedView";
            this.Size = new System.Drawing.Size(541, 389);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.httpErrorHandlingGv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button tmpBrowse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox uiTempDir;
        private System.Windows.Forms.CheckBox cbParseJsonDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox retryLabel;
        private System.Windows.Forms.RadioButton retryR;
        private System.Windows.Forms.RadioButton skipR;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton stopImmediatelyR;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView httpErrorHandlingGv;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox waitTimeLabel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn status_code;
        private System.Windows.Forms.DataGridViewComboBoxColumn handling_policy;
        private System.Windows.Forms.DataGridViewTextBoxColumn retry_attempts;
        private System.Windows.Forms.DataGridViewTextBoxColumn SleepTime;
    }
}
