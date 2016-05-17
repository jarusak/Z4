using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Z3.Model;
using Z3.View;

namespace Z3.View.Impl
{
    class DataSetElementsImpl : DataSetElements, ActiveDataSetElements, IDisposable
    {
        private Panel _panel;
        private System.ComponentModel.IContainer _components;
        private bool _disposed = false;

        public DataSetElementsImpl()
        {
            _components = new System.ComponentModel.Container();
            InitializeComponent();
        }

        public void Bind(Panel p)
        {
            _panel = p;
            _panel.Controls.Add(dataSetPanel);
        }

        #region Controls
        private void InitializeComponent()
        {
            this.dataSetPanel = new System.Windows.Forms.Panel();
            this.dataSetLabel = new System.Windows.Forms.Label();
            this.dataSetLabel_label = new System.Windows.Forms.Label();
            this.switchDataSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataSetMenu = new System.Windows.Forms.ContextMenuStrip(_components);
            this.dataSetMenu.SuspendLayout();
            this.dataSetPanel.SuspendLayout();
            // 
            // switchDataSetToolStripMenuItem
            // 
            this.switchDataSetToolStripMenuItem.Name = "switchDataSetToolStripMenuItem";
            this.switchDataSetToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.switchDataSetToolStripMenuItem.Text = "Switch Data Set...";
            this.switchDataSetToolStripMenuItem.Click += new EventHandler(switchDataSetToolStripMenuItem_Click);
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size(161, 6);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.newToolStripMenuItem.Text = "New Data Set...";
            this.newToolStripMenuItem.Click += new EventHandler(newToolStripMenuItem_Click);
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new System.Drawing.Size(161, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new EventHandler(deleteToolStripMenuItem_Click);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new EventHandler(propertiesToolStripMenuItem_Click);
            // 
            // dataSetMenu
            // 
            this.dataSetMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.switchDataSetToolStripMenuItem ,
            this.toolStripMenuItem13,
            this.newToolStripMenuItem,
            this.toolStripMenuItem14,
            this.deleteToolStripMenuItem,
            this.propertiesToolStripMenuItem});
            this.dataSetMenu.Name = "dataSetMenu";
            this.dataSetMenu.Size = new System.Drawing.Size(165, 104);
            // 
            // dataSetPanel
            // 
            this.dataSetPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataSetPanel.ContextMenuStrip = this.dataSetMenu;
            this.dataSetPanel.Controls.Add(this.dataSetLabel);
            this.dataSetPanel.Controls.Add(this.dataSetLabel_label);
            this.dataSetPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataSetPanel.Location = new System.Drawing.Point(0, 0);
            this.dataSetPanel.Name = "dataSetPanel";
            this.dataSetPanel.Size = new System.Drawing.Size(192, 60);
            this.dataSetPanel.TabIndex = 0;
            this.dataSetPanel.DoubleClick += new EventHandler(this.switchDataSetToolStripMenuItem_Click);
            // 
            // dataSetLabel
            // 
            this.dataSetLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataSetLabel.Location = new System.Drawing.Point(-2, 20);
            this.dataSetLabel.Name = "dataSetLabel";
            this.dataSetLabel.Size = new System.Drawing.Size(192, 36);
            this.dataSetLabel.TabIndex = 1;
            this.dataSetLabel.Text = "Set 41B  12/21/01";
            this.dataSetLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.dataSetLabel.DoubleClick += new EventHandler(this.switchDataSetToolStripMenuItem_Click);
            // 
            // dataSetLabel_label
            // 
            this.dataSetLabel_label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataSetLabel_label.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataSetLabel_label.Location = new System.Drawing.Point(-2, 2);
            this.dataSetLabel_label.Name = "dataSetLabel_label";
            this.dataSetLabel_label.Size = new System.Drawing.Size(192, 18);
            this.dataSetLabel_label.TabIndex = 0;
            this.dataSetLabel_label.Text = "Current Data Set";
            this.dataSetLabel_label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.dataSetLabel_label.DoubleClick += new EventHandler(this.switchDataSetToolStripMenuItem_Click);

            this.dataSetMenu.ResumeLayout(false);
            this.dataSetPanel.ResumeLayout(false);
            
        }

        void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DataSetPropertiesClicked != null)
                DataSetPropertiesClicked(sender, e);
        }

        void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DataSetDeleteClicked != null)
                DataSetDeleteClicked(sender, e);
        }

        void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DataSetNewClicked != null)
                DataSetNewClicked(sender, e);
        }

        private System.Windows.Forms.Panel dataSetPanel;
        private System.Windows.Forms.Label dataSetLabel_label;
        private System.Windows.Forms.Label dataSetLabel;
        private System.Windows.Forms.ContextMenuStrip dataSetMenu;
        private System.Windows.Forms.ToolStripMenuItem switchDataSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem13;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem14;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;

        #endregion

        void switchDataSetToolStripMenuItem_Click(object sender, EventArgs e) {
            if (DataSetOpenClicked != null)
            {
                DataSetOpenClicked(sender, e);
            }
        }

        #region DataSetElements Members

        bool DataSetElements.Enabled
        {
            set
            {
                if (_disposed) throw new ObjectDisposedException("Z3DataSetElementsImpl");

                if (!value)
                {
                    dataSetLabel.Text = "< No Workspace Loaded >";
                }

                this.newToolStripMenuItem.Enabled = value;
                this.switchDataSetToolStripMenuItem.Enabled = value;
            }
        }

        public event EventHandler DataSetNewClicked;
        public event EventHandler DataSetOpenClicked;

        #endregion

        #region ActiveDataSetElements Members
        
        string ActiveDataSetElements.Name
        {
            set
            {
                if (_disposed) throw new ObjectDisposedException("Z3DataSetElementsImpl");

                dataSetLabel.Text = value;
            }
        }
        
        bool ActiveDataSetElements.Enabled
        {
            set
            {
                if (_disposed) throw new ObjectDisposedException("Z3DataSetElementsImpl");

                this.deleteToolStripMenuItem.Enabled = value;
                this.propertiesToolStripMenuItem.Enabled = value;
            }
        }

        public event EventHandler DataSetPropertiesClicked;
        public event EventHandler DataSetDeleteClicked;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException("Z3DataSetElementsImpl");
            _disposed = true;

            _components.Dispose();
        }

        #endregion
    }
}
