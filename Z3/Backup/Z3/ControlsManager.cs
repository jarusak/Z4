using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Z3.Model;
using Z3.View.Floating;

namespace Z3.View.Impl
{
    public class PrimaryElementImpl : ReadyControlElements, CalibControlElements
    {
        private Panel _panel;

        public PrimaryElementImpl() {
            InitializeComponent();
            _mtypes = new IME_ZMeasurements_Impl(this);
        }
        
        public void Bind(Panel p)
        {
            _panel = p;
            p.Controls.Add(calibrationPanel);
            p.Controls.Add(countingPanel);
        }

        //internal void EnableVideoCalib(bool p)
        //{
        //    calibrationPanel.Visible = !p;
        //    countingPanel.Visible = p;
        //}

        //public TreeView SpeciesTree {
        //    get { return CountableTree; }
        //}

        //internal void EnableData(bool p)
        //{
        //    countingPanel.Enabled = p;
        //}

        //internal void SetMeasurementTypes(List<Z3.Model.ZMeasurement> types) {
        //    measurementCombo.Items.Clear();
        //    foreach (ZMeasurement m in types) {
        //        measurementCombo.Items.Add(m);
        //    }
        //    if (measurementCombo.Items.Count > 0)
        //        measurementCombo.SelectedIndex = 0;
        //}

        //internal void EnableControls(bool p)
        //{
        //    btnCounting.Enabled = p;
        //    btnUndo.Enabled = p;
        //    if (!p) Counting = false;
        //}

        //internal void EnableVideo(bool p)
        //{
        //    calibrationPanel.Enabled = p;
        //}

        private void btnCalibrate_Click(object sender, EventArgs e) {
            try {
                //_brain.VideoMgr.Calibrate(
                CalibratePerformed(sender, new CalibrationEventArgs(
                    double.Parse(txtCalibrationLength.Text),
                    int.Parse(txtCalibrationZoom.Text)
                ));
                //);
                
            } catch (FormatException) {
                MessageBox.Show("Please enter a valid distance and a valid zoom.", "Warning");
            }
        }

        #region Controls

        private void InitializeComponent()
        {
            this.countingPanel = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnCounting = new System.Windows.Forms.Button();
            this.measurementCombo = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CountableTree = new System.Windows.Forms.TreeView();
            this.calibrationPanel = new System.Windows.Forms.Panel();
            this.txtCalibrationZoom = new System.Windows.Forms.TextBox();
            this.txtCalibrationLength = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCalibrate = new System.Windows.Forms.Button();
            
            this.countingPanel.Panel1.SuspendLayout();
            this.countingPanel.Panel2.SuspendLayout();
            this.countingPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.calibrationPanel.SuspendLayout();
            
            // 
            // countingPanel
            // 
            this.countingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.countingPanel.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.countingPanel.IsSplitterFixed = true;
            this.countingPanel.Location = new System.Drawing.Point(0, 0);
            this.countingPanel.Name = "countingPanel";
            this.countingPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // countingPanel.Panel1
            // 
            this.countingPanel.Panel1.Controls.Add(this.panel2);
            // 
            // countingPanel.Panel2
            // 
            this.countingPanel.Panel2.Controls.Add(this.CountableTree);
            this.countingPanel.Size = new System.Drawing.Size(192, 234);
            this.countingPanel.SplitterDistance = 95;
            this.countingPanel.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnUndo);
            this.panel2.Controls.Add(this.btnCounting);
            this.panel2.Controls.Add(this.measurementCombo);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(192, 95);
            this.panel2.TabIndex = 0;
            // 
            // measurementCombo
            // 
            this.measurementCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.measurementCombo.FormattingEnabled = true;
            this.measurementCombo.Location = new System.Drawing.Point(3, 24);
            this.measurementCombo.Name = "measurementCombo";
            this.measurementCombo.Size = new System.Drawing.Size(182, 21);
            this.measurementCombo.TabIndex = 2;
            this.measurementCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(-2, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "Measurement Type";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // CountableTree
            // 
            this.CountableTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CountableTree.Location = new System.Drawing.Point(0, 0);
            this.CountableTree.Name = "CountableTree";
            this.CountableTree.Size = new System.Drawing.Size(192, 135);
            this.CountableTree.TabIndex = 0;
            this.CountableTree.HideSelection = false;
            // 
            // calibrationPanel
            // 
            this.calibrationPanel.Controls.Add(this.btnCalibrate);
            this.calibrationPanel.Controls.Add(this.txtCalibrationZoom);
            this.calibrationPanel.Controls.Add(this.txtCalibrationLength);
            this.calibrationPanel.Controls.Add(this.label6);
            this.calibrationPanel.Controls.Add(this.label5);
            this.calibrationPanel.Controls.Add(this.label4);
            this.calibrationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calibrationPanel.Location = new System.Drawing.Point(0, 0);
            this.calibrationPanel.Name = "calibrationPanel";
            this.calibrationPanel.Size = new System.Drawing.Size(192, 234);
            this.calibrationPanel.TabIndex = 5;
            this.calibrationPanel.Visible = false;
            // 
            // txtCalibrationZoom
            // 
            this.txtCalibrationZoom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCalibrationZoom.Location = new System.Drawing.Point(62, 70);
            this.txtCalibrationZoom.Name = "txtCalibrationZoom";
            this.txtCalibrationZoom.Size = new System.Drawing.Size(123, 21);
            this.txtCalibrationZoom.TabIndex = 4;
            this.txtCalibrationZoom.Text = "1";
            this.txtCalibrationZoom.Visible = false;
            // 
            // txtCalibrationLength
            // 
            this.txtCalibrationLength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCalibrationLength.Location = new System.Drawing.Point(62, 43);
            this.txtCalibrationLength.Name = "txtCalibrationLength";
            this.txtCalibrationLength.Size = new System.Drawing.Size(123, 21);
            this.txtCalibrationLength.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Zoom:";
            this.label6.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Length:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Calibration";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnCalibrate
            // 
            this.btnCalibrate.Location = new System.Drawing.Point(62, 97);
            this.btnCalibrate.Name = "btnCalibrate";
            this.btnCalibrate.Size = new System.Drawing.Size(75, 23);
            this.btnCalibrate.TabIndex = 5;
            this.btnCalibrate.Text = "Calibrate";
            this.btnCalibrate.UseVisualStyleBackColor = true;
            this.btnCalibrate.Click += new EventHandler(btnCalibrate_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUndo.Location = new System.Drawing.Point(98, 55);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(85, 23);
            this.btnUndo.TabIndex = 4;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Visible = false;
            this.btnUndo.Click += new EventHandler(btnUndo_Click);
            // 
            // btnCounting
            // 
            this.btnCounting.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCounting.Location = new System.Drawing.Point(6, 55);
            this.btnCounting.Name = "btnCounting";
            this.btnCounting.Size = new System.Drawing.Size(85, 23);
            this.btnCounting.TabIndex = 3;
            this.btnCounting.Text = "Start Counting";
            this.btnCounting.UseVisualStyleBackColor = true;
            this.btnCounting.Click += new EventHandler(btnCounting_Click);

            
            this.countingPanel.Panel1.ResumeLayout(false);
            this.countingPanel.Panel2.ResumeLayout(false);
            this.countingPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.calibrationPanel.ResumeLayout(false);
            this.calibrationPanel.PerformLayout();
            
        }

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox measurementCombo;
        private System.Windows.Forms.Button btnCounting;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.TreeView CountableTree;
        private System.Windows.Forms.SplitContainer countingPanel;
        private System.Windows.Forms.Panel calibrationPanel;
        private System.Windows.Forms.TextBox txtCalibrationZoom;
        private System.Windows.Forms.TextBox txtCalibrationLength;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCalibrate;
        #endregion

        void btnUndo_Click(object sender, EventArgs e)
        {
            if (UndoClicked != null) UndoClicked(sender, e);
        }

        //public static bool isFloatingForm(Form x)
        //{
        //    //Z3FloatingVideoForm x = (Z3FloatingVideoForm)y;
        //    return x.Name.Equals("Z3FloatingVideoForm");
        //}

        void btnCounting_Click(object sender, EventArgs e)
        {
            if (StartStopCountingClicked != null) StartStopCountingClicked(sender, e);
            //Counting = !Counting;
            //if (this.SpeciesTree.Nodes.Count == 0)
            //    _brain.CountableMgr = new EntityBrowser(_brain.CtlMgr.SpeciesTree, "Countable", _brain.DataSetsMgr.Components);
        }

        //private bool _counting;

        //public bool Counting
        //{
        //    get { return _counting; }
        //    set
        //    {
        //        _counting = value;
        //        btnCounting.Text = (_counting ? "Stop Counting" : "Start Counting");
        //        _brain.VideoMgr.EnableCounting(_counting);
        //        _brain.MenuMgr.EnableCounting(_counting);
        //
        //    }
        //}

        #region Boring Interface Impl. Classes
        #region ReadyControlElements Members

        bool ReadyControlElements.Enabled
        {
            set
            {
                btnCounting.Enabled = value;
                btnUndo.Enabled = value;
            }
        }

        bool ReadyControlElements.Counting
        {
            set
            {
                btnCounting.Text = (value ? "Stop" : "Start") + " Counting";
            }
        }

        public event EventHandler StartStopCountingClicked;
        public event EventHandler UndoClicked;
        event EventHandler ReadyControlElements.RecalibrateClicked
        {
            add { }
            remove { }
        }
        
        #endregion

        #region CalibControlElements Members

        bool CalibControlElements.Enabled
        {
            set
            {
                calibrationPanel.Visible = value;
                btnCalibrate.Enabled = value;
                txtCalibrationLength.Enabled = value;
                txtCalibrationZoom.Enabled = value;

                if (value)
                {
                    txtCalibrationLength.Focus();
                }
            }
        }

        public event EventHandler<CalibrationEventArgs> CalibratePerformed;

        #endregion

        private IME_ZMeasurements_Impl _mtypes;
        public ItemContainer<ZMeasurement> MTypes { get { return _mtypes; } }

        #region Internal Interface Impl. Classes
        /// <summary>
        /// This class translates the ItemManipulationElements&lt;ZMeasurement&gt; interface
        /// into calls to PrimaryElementImpl stuff.
        /// </summary>
        private class IME_ZMeasurements_Impl : ItemContainer<ZMeasurement>
        {
            private PrimaryElementImpl main;
            public IME_ZMeasurements_Impl(PrimaryElementImpl main)
            {
                this.main = main;
                main.measurementCombo.SelectedIndexChanged += new EventHandler(measurementCombo_SelectedIndexChanged);
            }

            public ZMeasurement ContextMenuSubject
            {
                get
                {
                    return SelectedItem;
                }
            }

            void measurementCombo_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (_notrigger) return;

                if (Selected != null)
                    Selected(sender, e);
            }

            #region ItemManipulationElements<ZMeasurement> Members

            public event EventHandler Selected;

            void ItemContainer<ZMeasurement>.Add(ZMeasurement item) {
                main.measurementCombo.Items.Add(item);
            }

            void ItemContainer<ZMeasurement>.Update(ZMeasurement item) {
                throw new NotSupportedException("Updating ZMeasurements is not supported!");
            }

            void ItemContainer<ZMeasurement>.Delete(ZMeasurement item) {
                throw new NotSupportedException("Deleting ZMeasurements is not supported!");
            }

            void ItemContainer<ZMeasurement>.Clear() {
                main.measurementCombo.Items.Clear();
            }

            private bool _notrigger = false;
            public ZMeasurement SelectedItem
            {
                get
                {
                    if (main.measurementCombo.SelectedItem == null) return null;
                    return (ZMeasurement)(main.measurementCombo.SelectedItem);
                }
                set
                {
                    _notrigger = true;
                    main.measurementCombo.SelectedItem = value;
                    _notrigger = false;
                }
            }

            #endregion

        }
        #endregion
        #endregion
    }
}
