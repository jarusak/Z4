namespace Z3.Forms {
    partial class OptionsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.dlgColorPicker = new System.Windows.Forms.ColorDialog();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.grpCounting = new System.Windows.Forms.GroupBox();
            this.pnlDot = new System.Windows.Forms.Panel();
            this.txtDotSize = new System.Windows.Forms.TextBox();
            this.lblSize = new System.Windows.Forms.Label();
            this.btnOld = new System.Windows.Forms.Button();
            this.btnActive = new System.Windows.Forms.Button();
            this.lblCompleted = new System.Windows.Forms.Label();
            this.grpDisplay = new System.Windows.Forms.GroupBox();
            this.lblPlaces = new System.Windows.Forms.Label();
            this.txtPlaces = new System.Windows.Forms.TextBox();
            this.lblRounding = new System.Windows.Forms.Label();
            this.grpMemory = new System.Windows.Forms.GroupBox();
            this.lblNotice = new System.Windows.Forms.Label();
            this.optInvert = new System.Windows.Forms.RadioButton();
            this.optNormal = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpCounting.SuspendLayout();
            this.grpDisplay.SuspendLayout();
            this.grpMemory.SuspendLayout();
            this.SuspendLayout();
            // 
            // dlgColorPicker
            // 
            this.dlgColorPicker.AnyColor = true;
            // 
            // lblCurrent
            // 
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.Location = new System.Drawing.Point(6, 22);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(75, 13);
            this.lblCurrent.TabIndex = 0;
            this.lblCurrent.Text = "Current Count:";
            // 
            // grpCounting
            // 
            this.grpCounting.Controls.Add(this.pnlDot);
            this.grpCounting.Controls.Add(this.txtDotSize);
            this.grpCounting.Controls.Add(this.lblSize);
            this.grpCounting.Controls.Add(this.btnOld);
            this.grpCounting.Controls.Add(this.btnActive);
            this.grpCounting.Controls.Add(this.lblCompleted);
            this.grpCounting.Controls.Add(this.lblCurrent);
            this.grpCounting.Location = new System.Drawing.Point(12, 12);
            this.grpCounting.Name = "grpCounting";
            this.grpCounting.Size = new System.Drawing.Size(134, 106);
            this.grpCounting.TabIndex = 1;
            this.grpCounting.TabStop = false;
            this.grpCounting.Text = "Counting Dots";
            // 
            // pnlDot
            // 
            this.pnlDot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDot.Location = new System.Drawing.Point(103, 75);
            this.pnlDot.Name = "pnlDot";
            this.pnlDot.Size = new System.Drawing.Size(19, 20);
            this.pnlDot.TabIndex = 5;
            // 
            // txtDotSize
            // 
            this.txtDotSize.Location = new System.Drawing.Point(62, 75);
            this.txtDotSize.Name = "txtDotSize";
            this.txtDotSize.Size = new System.Drawing.Size(33, 20);
            this.txtDotSize.TabIndex = 2;
            this.txtDotSize.TextChanged += new System.EventHandler(this.txtDotSize_TextChanged);
            this.txtDotSize.Validating += new System.ComponentModel.CancelEventHandler(this.txtDotSize_Validating);
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(6, 78);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(50, 13);
            this.lblSize.TabIndex = 4;
            this.lblSize.Text = "Dot Size:";
            // 
            // btnOld
            // 
            this.btnOld.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnOld.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOld.Location = new System.Drawing.Point(103, 47);
            this.btnOld.Name = "btnOld";
            this.btnOld.Size = new System.Drawing.Size(19, 19);
            this.btnOld.TabIndex = 1;
            this.btnOld.UseVisualStyleBackColor = false;
            this.btnOld.Click += new System.EventHandler(this.btnOld_Click);
            // 
            // btnActive
            // 
            this.btnActive.BackColor = System.Drawing.Color.Red;
            this.btnActive.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnActive.Location = new System.Drawing.Point(103, 19);
            this.btnActive.Name = "btnActive";
            this.btnActive.Size = new System.Drawing.Size(19, 19);
            this.btnActive.TabIndex = 0;
            this.btnActive.UseVisualStyleBackColor = false;
            this.btnActive.Click += new System.EventHandler(this.btnActive_Click);
            // 
            // lblCompleted
            // 
            this.lblCompleted.AutoSize = true;
            this.lblCompleted.Location = new System.Drawing.Point(6, 50);
            this.lblCompleted.Name = "lblCompleted";
            this.lblCompleted.Size = new System.Drawing.Size(91, 13);
            this.lblCompleted.TabIndex = 2;
            this.lblCompleted.Text = "Completed Count:";
            // 
            // grpDisplay
            // 
            this.grpDisplay.Controls.Add(this.lblPlaces);
            this.grpDisplay.Controls.Add(this.txtPlaces);
            this.grpDisplay.Controls.Add(this.lblRounding);
            this.grpDisplay.Enabled = false;
            this.grpDisplay.Location = new System.Drawing.Point(12, 124);
            this.grpDisplay.Name = "grpDisplay";
            this.grpDisplay.Size = new System.Drawing.Size(340, 51);
            this.grpDisplay.TabIndex = 2;
            this.grpDisplay.TabStop = false;
            this.grpDisplay.Text = "Data Display";
            // 
            // lblPlaces
            // 
            this.lblPlaces.AutoSize = true;
            this.lblPlaces.Location = new System.Drawing.Point(128, 22);
            this.lblPlaces.Name = "lblPlaces";
            this.lblPlaces.Size = new System.Drawing.Size(77, 13);
            this.lblPlaces.TabIndex = 2;
            this.lblPlaces.Text = "decimal places";
            // 
            // txtPlaces
            // 
            this.txtPlaces.Location = new System.Drawing.Point(62, 19);
            this.txtPlaces.Name = "txtPlaces";
            this.txtPlaces.Size = new System.Drawing.Size(60, 20);
            this.txtPlaces.TabIndex = 5;
            this.txtPlaces.Validating += new System.ComponentModel.CancelEventHandler(this.txtPlaces_Validating);
            // 
            // lblRounding
            // 
            this.lblRounding.AutoSize = true;
            this.lblRounding.Location = new System.Drawing.Point(6, 22);
            this.lblRounding.Name = "lblRounding";
            this.lblRounding.Size = new System.Drawing.Size(54, 13);
            this.lblRounding.TabIndex = 0;
            this.lblRounding.Text = "Round to:";
            // 
            // grpMemory
            // 
            this.grpMemory.Controls.Add(this.lblNotice);
            this.grpMemory.Controls.Add(this.optInvert);
            this.grpMemory.Controls.Add(this.optNormal);
            this.grpMemory.Location = new System.Drawing.Point(152, 12);
            this.grpMemory.Name = "grpMemory";
            this.grpMemory.Size = new System.Drawing.Size(200, 106);
            this.grpMemory.TabIndex = 3;
            this.grpMemory.TabStop = false;
            this.grpMemory.Text = "Mouse Control";
            // 
            // lblNotice
            // 
            this.lblNotice.AutoSize = true;
            this.lblNotice.Location = new System.Drawing.Point(6, 87);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.Size = new System.Drawing.Size(189, 13);
            this.lblNotice.TabIndex = 5;
            this.lblNotice.Text = "(Note: Ctrl-click always inverts buttons)";
            // 
            // optInvert
            // 
            this.optInvert.Location = new System.Drawing.Point(6, 50);
            this.optInvert.Name = "optInvert";
            this.optInvert.Size = new System.Drawing.Size(188, 30);
            this.optInvert.TabIndex = 4;
            this.optInvert.TabStop = true;
            this.optInvert.Text = "Primary: Right\r\nMulti-point: Left";
            this.optInvert.UseVisualStyleBackColor = true;
            // 
            // optNormal
            // 
            this.optNormal.Location = new System.Drawing.Point(6, 18);
            this.optNormal.Name = "optNormal";
            this.optNormal.Size = new System.Drawing.Size(188, 30);
            this.optNormal.TabIndex = 3;
            this.optNormal.TabStop = true;
            this.optNormal.Text = "Primary: Left\r\nMulti-point: Right";
            this.optNormal.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(277, 181);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(196, 181);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(364, 216);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grpMemory);
            this.Controls.Add(this.grpDisplay);
            this.Controls.Add(this.grpCounting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.Shown += new System.EventHandler(this.OptionsForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsForm_FormClosing);
            this.grpCounting.ResumeLayout(false);
            this.grpCounting.PerformLayout();
            this.grpDisplay.ResumeLayout(false);
            this.grpDisplay.PerformLayout();
            this.grpMemory.ResumeLayout(false);
            this.grpMemory.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColorDialog dlgColorPicker;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.GroupBox grpCounting;
        private System.Windows.Forms.Button btnOld;
        private System.Windows.Forms.Button btnActive;
        private System.Windows.Forms.Label lblCompleted;
        private System.Windows.Forms.TextBox txtDotSize;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.GroupBox grpDisplay;
        private System.Windows.Forms.GroupBox grpMemory;
        private System.Windows.Forms.RadioButton optNormal;
        private System.Windows.Forms.RadioButton optInvert;
        private System.Windows.Forms.Label lblPlaces;
        private System.Windows.Forms.TextBox txtPlaces;
        private System.Windows.Forms.Label lblRounding;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlDot;
        private System.Windows.Forms.Label lblNotice;
    }
}