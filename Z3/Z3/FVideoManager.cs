using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.Model;
using Z3.Util;

namespace Z3.View.Floating {
    class FVideoManager //: IVideoManager 
    {
        //private ViewManager _brain;
        private Panel _panel;
        private ToolStripMenuItem _menu;
        private Form _form;
        private IContainer _Container;
        private MouseHook _hook;

        public Zoopomatic2.Controls.Measurer Measurer
        {
            get { return measures; }
        }
     
        public FVideoManager( Panel p, ToolStripMenuItem m, Form f, IContainer cont, MouseHook hook) {
            _Container = cont;
            InitializeComponent();
            _form = f;
            //_brain = v;
            _panel = p;
            _menu = m;
            _menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miVideoDevice,
            this.miVideoOverlay});
            p.Controls.Add(vHolderPanel);
            _hook = hook;
        }
        
        public void EnableVideo(bool p) {
            measures.clear();
            measures.Enabled = p;
            if (p)
            {
                measures.startCalibration();
                _hook.Start(true, false);
            }
            else
            {
                _hook.Stop();
            }
        }
        
        #region Controls
        private void InitializeComponent() {
            this.miVideoDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.miVideoOverlay = new System.Windows.Forms.ToolStripMenuItem();
            this.vHolderPanel = new Panel();
            this.measures = new Zoopomatic2.Controls.Measurer();
            // 
            // miVideoDevice
            // 
            this.miVideoDevice.Name = "miVideoDevice";
            this.miVideoDevice.Size = new System.Drawing.Size(152, 22);
            this.miVideoDevice.Text = "Switch to Device Mode";
            this.miVideoDevice.Click += new EventHandler(miVideoDevice_Click);
            // 
            // miVideoOverlay
            // 
            this.miVideoOverlay.Name = "miVideoOverlay";
            this.miVideoOverlay.Size = new System.Drawing.Size(152, 22);
            this.miVideoOverlay.Text = "Show Overlay Window";
            this.miVideoOverlay.Checked = true;
            this.miVideoOverlay.Click += new EventHandler(miVideoOverlay_Click);
            // 
            // vHolderPanel
            // 
            this.vHolderPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.vHolderPanel.Controls.Add(this.measures);
            this.vHolderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vHolderPanel.Location = new System.Drawing.Point(0, 0);
            this.vHolderPanel.Name = "vHolderPanel";
            this.vHolderPanel.Size = new System.Drawing.Size(364, 298);
            this.vHolderPanel.TabIndex = 0;
            // 
            // measures
            // 
            //this.measures.ActiveColor = System.Drawing.Color.Red;
            this.measures.Calibrating = false;
            this.measures.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.measures.DotSize = 5;
            //this.measures.HistoryColor = System.Drawing.Color.Blue;
            //this.measures.KeepHistory = false;
            this.measures.Location = new System.Drawing.Point(0, 0);
            this.measures.Name = "measures";
            this.measures.Size = new System.Drawing.Size(364, 298);
            this.measures.TabIndex = 2;
            this.measures.Count += new EventHandler(measures_Count);
            this.measures.Measure += new EventHandler<Zoopomatic2.Controls.Measurer.MeasureEventArgs>(measures_Measure);
            
            this.vHolderPanel.ResumeLayout(false);
        }

        void measures_Measure(object sender, Zoopomatic2.Controls.Measurer.MeasureEventArgs e)
        {
            throw new NotImplementedException("todo");

        }

        void measures_Count(object sender, EventArgs e)
        {
            throw new NotImplementedException("todo");
        }

        void miVideoOverlay_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException("todo");
        }

        void miVideoDevice_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException("todo");
        }

        //void miVideoOverlay_Click(object sender, EventArgs e)
        //{
        //    miVideoOverlay.Checked = !miVideoOverlay.Checked;
        //    _brain.WindowMgr.ShowOverlay(miVideoOverlay.Checked);
        //}

        //void miVideoDevice_Click(object sender, EventArgs e) {
        //    VideoModeChangeRequested(this, new VideoModeEventArgs(VideoMode.FEED, _brain.Controller));
        //}

        //void measures_Measure(object sender, Zoopomatic2.Controls.Measurer.MeasureEventArgs e) {
        //    // Record: MType, Container, Countable
        //    _brain.DataPointsMgr.Measure(_brain.CtlMgr.MeasurementType, (ZCountable)_brain.CountableMgr.Countable, e.convertedLength);
        //}

        //void measures_Count(object sender, EventArgs e) {
        //    // Record: MType, Container, Countable
        //    _brain.DataPointsMgr.Count(_brain.CtlMgr.MeasurementType, (ZCountable)_brain.CountableMgr.Countable);
        //}

        private System.Windows.Forms.ToolStripMenuItem miVideoDevice;
        private System.Windows.Forms.ToolStripMenuItem miVideoOverlay;
        private System.Windows.Forms.Panel vHolderPanel;
        private Zoopomatic2.Controls.Measurer measures;

        #endregion

        /*public void Resize() {
            videoResize();
        }*/

        /*private void videoResize() {
            _brain.State.decalibrateVideo();
        }*/

        /*public void Loading() {
            videoResize();
            _brain.State.sourceVideo();
        }*/

        //public void EnableVideoCalib(bool p) {
        //    measures.clear();
        //    measures.Enabled = p && _brain.Ready;
        //    if (p)
        //    {
        //        _hook.Start(true, true);
        //    }
        //    else
        //    {
        //        _hook.Stop();
        //    }
        //}

        /*public void EnableCounting(bool _counting) {
            if (measures.Calibrating) return;

            if (_counting)
            {
                measures.Calibrating = false;
            }

            measures.clear();
            measures.Enabled = _counting;
        }*/

        //public event EventHandler<VideoModeEventArgs> VideoModeChangeRequested;
    }
}
