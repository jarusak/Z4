namespace Z3.View.Floating
{
    partial class Z3FloatingVideoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Z3FloatingVideoForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this._meas = new Zoopomatic2.Controls.Measurer();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this._meas);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(231, 127);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 0;
            // 
            // _meas
            // 
            this._meas.AutoSize = true;
            this._meas.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._meas.Calibrating = false;
            this._meas.Dock = System.Windows.Forms.DockStyle.Fill;
            this._meas.Enabled = false;
            this._meas.Location = new System.Drawing.Point(0, 0);
            this._meas.Name = "_meas";
            this._meas.Size = new System.Drawing.Size(231, 127);
            this._meas.TabIndex = 1;
            // 
            // Z3FloatingVideoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(231, 127);
            this.ControlBox = true;
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Z3FloatingVideoForm";
            this.Text = "Video Overlay";
            this.TransparencyKey = this.BackColor;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Z3FloatingVideoForm_FormClosing);
            this.Shown += new System.EventHandler(this.Z3FloatingVideoForm_Shown);
            this.ResizeBegin += new System.EventHandler(this.Z3FloatingVideoForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.Z3FloatingVideoForm_ResizeEnd);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private Zoopomatic2.Controls.Measurer _meas;
    }
}