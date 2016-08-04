using System;

namespace Z3
{
    partial class DataViewForm
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.dataMenu = new System.Windows.Forms.ContextMenuStrip();
            this.stopperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorBox = new System.Windows.Forms.TextBox();
            this.dataMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(241, 119);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // errorBox
            // 
            this.errorBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorBox.Location = new System.Drawing.Point(0, 0);
            this.errorBox.Multiline = true;
            this.errorBox.Name = "errorBox";
            this.errorBox.Size = new System.Drawing.Size(241, 119);
            this.errorBox.TabIndex = 1;
            this.errorBox.Visible = false;
            // 
            // datatMenu
            // 
            this.dataMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stopperToolStripMenuItem});
            this.dataMenu.Name = "dataMenu";
            this.dataMenu.Size = new System.Drawing.Size(165, 22);
            // 
            // stopperToolStripMenuItem
            // 
            this.stopperToolStripMenuItem.Name = "stopperToolStripMenuItem";
            this.stopperToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.stopperToolStripMenuItem.Text = "Insert Stopper";
            this.stopperToolStripMenuItem.Click += new EventHandler(InsertStopper_Click);
            this.listView1.ContextMenuStrip = this.dataMenu;
            this.dataMenu.ResumeLayout(false);
            // 
            // DataViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(241, 119);
            this.Controls.Add(this.errorBox);
            this.Controls.Add(this.listView1);
            this.Name = "DataViewForm";
            this.Text = "DataViewForm";
            this.Load += new System.EventHandler(this.DataViewForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox errorBox;
        private System.Windows.Forms.ContextMenuStrip dataMenu;
        private System.Windows.Forms.ToolStripMenuItem stopperToolStripMenuItem;

        private void InsertStopper_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                if (StopperClicked != null)
                {
                    StopperClicked(sender, e);
                }
            }
        }

        public event EventHandler StopperClicked;
    }
}