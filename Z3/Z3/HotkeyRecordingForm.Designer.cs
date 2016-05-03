namespace Z3
{
    partial class HotkeyRecordingForm
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
            this.ctrlBox = new System.Windows.Forms.CheckBox();
            this.altBox = new System.Windows.Forms.CheckBox();
            this.shiftBox = new System.Windows.Forms.CheckBox();
            this.keyBox = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.recordButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.valueBox = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ctrlBox
            // 
            this.ctrlBox.AutoSize = true;
            this.ctrlBox.Location = new System.Drawing.Point(12, 41);
            this.ctrlBox.Name = "ctrlBox";
            this.ctrlBox.Size = new System.Drawing.Size(41, 17);
            this.ctrlBox.TabIndex = 1;
            this.ctrlBox.Text = "Ctrl";
            this.ctrlBox.UseVisualStyleBackColor = true;
            // 
            // altBox
            // 
            this.altBox.AutoSize = true;
            this.altBox.Location = new System.Drawing.Point(59, 41);
            this.altBox.Name = "altBox";
            this.altBox.Size = new System.Drawing.Size(38, 17);
            this.altBox.TabIndex = 2;
            this.altBox.Text = "Alt";
            this.altBox.UseVisualStyleBackColor = true;
            // 
            // shiftBox
            // 
            this.shiftBox.AutoSize = true;
            this.shiftBox.Location = new System.Drawing.Point(103, 41);
            this.shiftBox.Name = "shiftBox";
            this.shiftBox.Size = new System.Drawing.Size(47, 17);
            this.shiftBox.TabIndex = 3;
            this.shiftBox.Text = "Shift";
            this.shiftBox.UseVisualStyleBackColor = true;
            // 
            // keyBox
            // 
            this.keyBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.keyBox.Location = new System.Drawing.Point(12, 61);
            this.keyBox.Name = "keyBox";
            this.keyBox.Size = new System.Drawing.Size(228, 17);
            this.keyBox.TabIndex = 4;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(84, 106);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(165, 106);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // recordButton
            // 
            this.recordButton.Location = new System.Drawing.Point(12, 12);
            this.recordButton.Name = "recordButton";
            this.recordButton.Size = new System.Drawing.Size(75, 23);
            this.recordButton.TabIndex = 7;
            this.recordButton.Text = "Record";
            this.recordButton.UseVisualStyleBackColor = true;
            this.recordButton.Click += new System.EventHandler(this.recordButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(93, 12);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 8;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // valueBox
            // 
            this.valueBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.valueBox.Location = new System.Drawing.Point(156, 41);
            this.valueBox.Name = "valueBox";
            this.valueBox.Size = new System.Drawing.Size(84, 17);
            this.valueBox.TabIndex = 9;
            // 
            // HotkeyRecordingForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(252, 141);
            this.Controls.Add(this.valueBox);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.recordButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.keyBox);
            this.Controls.Add(this.shiftBox);
            this.Controls.Add(this.altBox);
            this.Controls.Add(this.ctrlBox);
            this.KeyPreview = true;
            this.Name = "HotkeyRecordingForm";
            this.Text = "HotkeyRecordingForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyRecordingForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox ctrlBox;
        private System.Windows.Forms.CheckBox altBox;
        private System.Windows.Forms.CheckBox shiftBox;
        private System.Windows.Forms.Label keyBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button recordButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Label valueBox;
    }
}