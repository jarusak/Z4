namespace Z3.Forms {
    partial class NewEntityForm {
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
            this.typeLabel = new System.Windows.Forms.Label();
            this.parentLabel = new System.Windows.Forms.Label();
            this.typeBox = new System.Windows.Forms.ComboBox();
            this.parentBox = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(12, 15);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(34, 13);
            this.typeLabel.TabIndex = 0;
            this.typeLabel.Text = "Type:";
            // 
            // parentLabel
            // 
            this.parentLabel.AutoSize = true;
            this.parentLabel.Location = new System.Drawing.Point(12, 44);
            this.parentLabel.Name = "parentLabel";
            this.parentLabel.Size = new System.Drawing.Size(41, 13);
            this.parentLabel.TabIndex = 1;
            this.parentLabel.Text = "Parent:";
            // 
            // typeBox
            // 
            this.typeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.typeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeBox.FormattingEnabled = true;
            this.typeBox.Location = new System.Drawing.Point(59, 12);
            this.typeBox.Name = "typeBox";
            this.typeBox.Size = new System.Drawing.Size(251, 21);
            this.typeBox.TabIndex = 2;
            this.typeBox.SelectedIndexChanged += new System.EventHandler(this.typeBox_SelectedIndexChanged);
            // 
            // parentBox
            // 
            this.parentBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.parentBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parentBox.FormattingEnabled = true;
            this.parentBox.Location = new System.Drawing.Point(59, 41);
            this.parentBox.Name = "parentBox";
            this.parentBox.Size = new System.Drawing.Size(251, 21);
            this.parentBox.TabIndex = 3;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(235, 76);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(154, 76);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // NewEntityForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(322, 111);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.parentBox);
            this.Controls.Add(this.typeBox);
            this.Controls.Add(this.parentLabel);
            this.Controls.Add(this.typeLabel);
            this.MaximumSize = new System.Drawing.Size(999999999, 145);
            this.MinimumSize = new System.Drawing.Size(233, 145);
            this.Name = "NewEntityForm";
            this.Text = "New";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.Label parentLabel;
        private System.Windows.Forms.ComboBox typeBox;
        private System.Windows.Forms.ComboBox parentBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}