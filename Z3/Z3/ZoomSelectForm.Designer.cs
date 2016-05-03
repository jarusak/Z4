namespace Z3.View.Video
{
    partial class ZoomSelectForm
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
            this.cboZoom = new System.Windows.Forms.ComboBox();
            this.lblNewZoom = new System.Windows.Forms.Label();
            this.lblCurrentZoom = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblZoomUnits2 = new System.Windows.Forms.Label();
            this.lblZoomUnits1 = new System.Windows.Forms.Label();
            this.lblCurrentZoomValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboZoom
            // 
            this.cboZoom.FormattingEnabled = true;
            this.cboZoom.Items.AddRange(new object[] {
            "5",
            "10",
            "20",
            "40",
            "80",
            "100"});
            this.cboZoom.Location = new System.Drawing.Point(165, 45);
            this.cboZoom.Name = "cboZoom";
            this.cboZoom.Size = new System.Drawing.Size(75, 21);
            this.cboZoom.TabIndex = 0;
            this.cboZoom.Validating += new System.ComponentModel.CancelEventHandler(this.cboZoom_Validating);
            // 
            // lblNewZoom
            // 
            this.lblNewZoom.AutoSize = true;
            this.lblNewZoom.Location = new System.Drawing.Point(12, 48);
            this.lblNewZoom.Name = "lblNewZoom";
            this.lblNewZoom.Size = new System.Drawing.Size(138, 13);
            this.lblNewZoom.TabIndex = 1;
            this.lblNewZoom.Text = "Please enter the new zoom:";
            // 
            // lblCurrentZoom
            // 
            this.lblCurrentZoom.AutoSize = true;
            this.lblCurrentZoom.Location = new System.Drawing.Point(12, 13);
            this.lblCurrentZoom.Name = "lblCurrentZoom";
            this.lblCurrentZoom.Size = new System.Drawing.Size(72, 13);
            this.lblCurrentZoom.TabIndex = 2;
            this.lblCurrentZoom.Text = "Current zoom:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(183, 91);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(102, 91);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblZoomUnits2
            // 
            this.lblZoomUnits2.AutoSize = true;
            this.lblZoomUnits2.Location = new System.Drawing.Point(246, 48);
            this.lblZoomUnits2.Name = "lblZoomUnits2";
            this.lblZoomUnits2.Size = new System.Drawing.Size(12, 13);
            this.lblZoomUnits2.TabIndex = 5;
            this.lblZoomUnits2.Text = "x";
            // 
            // lblZoomUnits1
            // 
            this.lblZoomUnits1.AutoSize = true;
            this.lblZoomUnits1.Location = new System.Drawing.Point(246, 13);
            this.lblZoomUnits1.Name = "lblZoomUnits1";
            this.lblZoomUnits1.Size = new System.Drawing.Size(12, 13);
            this.lblZoomUnits1.TabIndex = 7;
            this.lblZoomUnits1.Text = "x";
            // 
            // lblCurrentZoomValue
            // 
            this.lblCurrentZoomValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentZoomValue.AutoSize = true;
            this.lblCurrentZoomValue.Location = new System.Drawing.Point(205, 13);
            this.lblCurrentZoomValue.Name = "lblCurrentZoomValue";
            this.lblCurrentZoomValue.Size = new System.Drawing.Size(35, 13);
            this.lblCurrentZoomValue.TabIndex = 8;
            this.lblCurrentZoomValue.Text = "label2";
            this.lblCurrentZoomValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ZoomSelectForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(270, 126);
            this.Controls.Add(this.lblCurrentZoomValue);
            this.Controls.Add(this.lblZoomUnits1);
            this.Controls.Add(this.lblZoomUnits2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblCurrentZoom);
            this.Controls.Add(this.lblNewZoom);
            this.Controls.Add(this.cboZoom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ZoomSelectForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Zoom";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboZoom;
        private System.Windows.Forms.Label lblNewZoom;
        private System.Windows.Forms.Label lblCurrentZoom;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblZoomUnits2;
        private System.Windows.Forms.Label lblZoomUnits1;
        private System.Windows.Forms.Label lblCurrentZoomValue;

    }
}