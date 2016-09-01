using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Z3.Model;
using Z3.View.Impl;
using Zoopomatic2.Controls;
using Z3.Util;
using Z3.View.Util;
using System.Diagnostics;
using System.Drawing;

namespace Z3.View.Floating
{

    /// <summary>
    /// Contains static methods used to construct a Z3 Floating-View User Interface.
    /// </summary>
    public class FloatingViewFactory
    {
        /// <summary>
        /// Builds and returns a new Floating User Interface.
        /// </summary>
        /// <returns>A new Floating User Interface.</returns>
        public static IView Build()
        {
            ViewFacade fac = new ViewFacade();
            FloatingViewControl ctl = new FloatingViewControl();
            ctl.PopulateViewFacade(fac);
            fac.Display = ctl;
            return fac;
        }
    }

    /// <summary>
    /// Handles the Window elements in a Floating View.
    /// </summary>
    internal class FloatingViewControl : WindowElements
    {
        private Z3FloatingControlsForm _controlForm;
        private Z3FloatingMeasurementForm _dataPointsForm;
        private Z3FloatingMenuForm _menuForm;
        private Z3FloatingVideoForm _videoForm;
        private DataViewForm _progress;

        private MenuElementsImpl _menu;
        private PrimaryElementImpl _primary;
        private DataPointElements _datapts;
        private DataSetElementsImpl _datasets;

        private SpeciesTreeWrapper _species;

        private bool resize = true;

        public MouseHook Hook
        {
            get
            {
                return _videoForm.Hook;
            }
        }

        /// <summary>
        /// Constructs a Floating View Control, creating new Menu and Primary (counting) controls.
        /// </summary>
        public FloatingViewControl()
        {
            initialize(
                new MenuElementsImpl(),
                new PrimaryElementImpl(),
                new DataPointElements(),
                new DataSetElementsImpl()
                );
        }

        public FloatingViewControl(
            MenuElementsImpl menu,
            PrimaryElementImpl counting,
            DataPointElements dps,
            DataSetElementsImpl dss
            )
        {
            initialize(menu, counting, dps, dss);
        }

        private Measurer _meas;

        private void initialize(
            MenuElementsImpl menu,
            PrimaryElementImpl counting,
            DataPointElements datapts,
            DataSetElementsImpl datasets)
        {
            // Set up view elements
            _menu = menu;
            _primary = counting;
            _datapts = datapts;
            _datasets = datasets;

            // Set up Windows Forms
            _controlForm = new Z3FloatingControlsForm();
            _dataPointsForm = new Z3FloatingMeasurementForm();
            _dataPointsForm.ControlBox = false;
            _menuForm = new Z3FloatingMenuForm();
            _videoForm = new Z3FloatingVideoForm();
            _progress = new DataViewForm();
             _meas = _videoForm.Measurer;
            _videoForm.Workspace.Controls.Add(_meas);
            _species = new SpeciesTreeWrapper(_primary.CountableTree);

            _controlForm.FormClosing += new FormClosingEventHandler(_FormClosing);
            _dataPointsForm.FormClosing += new FormClosingEventHandler(_FormClosing);
            _menuForm.FormClosing += new FormClosingEventHandler(_menuForm_FormClosing);
            _videoForm.FormClosing += new FormClosingEventHandler(_FormClosing);
            _progress.FormClosing += new FormClosingEventHandler(_FormClosing);

            _controlForm.FormClosed += new FormClosedEventHandler(_FormClosed);
            _dataPointsForm.FormClosed += new FormClosedEventHandler(_FormClosed);
            _menuForm.FormClosed += new FormClosedEventHandler(_menuForm_FormClosed);
            _videoForm.FormClosed += new FormClosedEventHandler(_FormClosed);
            _progress.FormClosed += new FormClosedEventHandler(_FormClosed);

            _controlForm.ResizeEnd += new EventHandler(_controlForm_ResizeEnd);
            _dataPointsForm.ResizeEnd += new EventHandler(_dataPointsForm_ResizeEnd);
            _menuForm.ResizeEnd += new EventHandler(_menuForm_ResizeEnd);
            _videoForm.ResizeEnd += new EventHandler(_videoForm_ResizeEnd);
            _progress.ResizeEnd += new EventHandler(_progress_ResizeEnd);

            _progress.Text = "Measurement Progress";

            // Bind Elements onto Forms
            // Bind will add UI  elements form
            _menu.Bind(_menuForm, _menuForm);
            _primary.Bind(_controlForm.BottomHalf);
            _datapts.Bind(_dataPointsForm.Workspace);
            _datasets.Bind(_controlForm.TopHalf);
        }

        #region Window Events
        void _progress_ResizeEnd(object sender, EventArgs e)
        {
            if(resize)
                ((WindowElements)this).WindowState = _progress.WindowState;
        }

        void _videoForm_ResizeEnd(object sender, EventArgs e)
        {
            if (resize)
                ((WindowElements)this).WindowState = _videoForm.WindowState;
        }

        void _menuForm_ResizeEnd(object sender, EventArgs e)
        {
            if (resize)
                ((WindowElements)this).WindowState = _menuForm.WindowState;
        }

        void _dataPointsForm_ResizeEnd(object sender, EventArgs e)
        {
            if (resize)
                ((WindowElements)this).WindowState = _dataPointsForm.WindowState;
        }

        void _controlForm_ResizeEnd(object sender, EventArgs e)
        {
            if (resize)
                ((WindowElements)this).WindowState = _controlForm.WindowState;
        }

        void _menuForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((WindowElements)this).Close();
            if (ViewClosed != null) ViewClosed(sender, e);
        }

        void _FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_intentionalClose) return;
            ((WindowElements)this).Close();
        }

        void _menuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_intentionalClose) return;

            if (ViewClosing != null) ViewClosing(sender, e);
        }

        void _FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_intentionalClose) return;

            if (e.CloseReason == CloseReason.UserClosing) e.Cancel = true;
            if (!e.Cancel) ViewClosing(sender, e);
        }
        #endregion

        #region Window Handling

        void WindowElements.Show()
        {
            _menuForm.Show();
            _controlForm.Show();
            _dataPointsForm.Show();
            _videoForm.Show();
            _progress.Show();
        }

        void WindowElements.Hide()
        {
            _menuForm.Hide();
            _controlForm.Hide();
            _dataPointsForm.Hide();
            _videoForm.Hide();
            _progress.Hide();
        }

        bool _intentionalState = false;

        FormWindowState WindowElements.WindowState
        {
            get
            {
                return _menuForm.WindowState;
            }
            set
            {
                if (_intentionalState) return;
                _intentionalState = true;
                _menuForm.WindowState = value;
                _controlForm.WindowState = value;
                _dataPointsForm.WindowState = value;
                _videoForm.WindowState = value;
                _progress.WindowState = value;
                _intentionalState = false;
            }
        }

        private bool _intentionalClose = false;

        void WindowElements.Close()
        {
            if (_intentionalClose) return;
            _intentionalClose = true;
            //_controlForm.Close();
            //_dataPointsForm.Close();
            //_videoForm.Close();
            _menuForm.Close();
            //_progress.Close();
            _intentionalClose = false;
        }

        void WindowElements.Dispose()
        {
            if (_intentionalClose) return;
            _intentionalClose = true;
            _controlForm.Dispose();
            _dataPointsForm.Dispose();
            _videoForm.Dispose();
            _menuForm.Dispose();
            _progress.Dispose();
            _intentionalClose = false;
        }

        Point menuLocation;
        Point videoLocation;
        Point controlLocation;
        Point progressLocation;
        Point dataPointLocation;

        Size menuSize;
        Size videoSize;
        Size controlSize;
        Size progressSize;
        Size dataPointsSize;

        void WindowElements.Rearrange()
        {
            MainWindow mainForm = new MainWindow(_menu, _videoForm, _controlForm, _progress, _dataPointsForm, ((WindowElements)this)); 

            if (_menu.isOneWindow)
            {
                ((WindowElements)this).Hide();

                menuLocation = _menuForm.Location;
                videoLocation = _videoForm.Location;
                controlLocation = _controlForm.Location;
                progressLocation = _progress.Location;
                dataPointLocation = _dataPointsForm.Location;

                menuSize = _menuForm.Size;
                videoSize = _videoForm.Size;
                controlSize = _controlForm.Size;
                progressSize = _progress.Size;
                dataPointsSize = _dataPointsForm.Size;

                mainForm.Show();
            }
            else
            {
                _menu.Bind(_menuForm, _menuForm);
                _menuForm.Show();
               
                _controlForm.MdiParent = null;
                _progress.MdiParent = null;
                _dataPointsForm.MdiParent = null;

                _menuForm.Location = menuLocation;
                _videoForm.Location = videoLocation;
                _controlForm.Location = controlLocation;
                _progress.Location = progressLocation;
                _dataPointsForm.Location = dataPointLocation;

                _menuForm.Size = menuSize;
                _videoForm.Size = videoSize;
                _controlForm.Size = controlSize;
                _progress.Size = progressSize;
                _dataPointsForm.Size = dataPointsSize;

                _controlForm.OneWindow = false;
                _progress.OneWindow = false;
                _dataPointsForm.OneWindow = false;
                mainForm.IsMdiContainer = false;
                
                mainForm.CloseIt();
            }
         }

        public event EventHandler<FormClosingEventArgs> ViewClosing;
        public event EventHandler<FormClosedEventArgs> ViewClosed;
        #endregion

        /// <summary>
        /// Ties actual UI components to the Elements they represent in a View.
        /// </summary>
        /// <param name="fac"></param>
        internal void PopulateViewFacade(ViewFacade fac)
        {

            // Assign elements to aspects of the View
            List<ActiveCountableElements> aclist = new List<ActiveCountableElements>();
            {
                aclist.Add(_menu);
                aclist.Add(_species);
            }
            List<ActiveDataSetElements> adselist = new List<ActiveDataSetElements>();
            {
                adselist.Add(_menu);
                adselist.Add(_datasets);
            }
            List<CountableElements> ctllist = new List<CountableElements>();
            {
                ctllist.Add(_menu);
                ctllist.Add(_species);
            }
            List<DataSetElements> dselist = new List<DataSetElements>();
            {
                dselist.Add(_menu);
                dselist.Add(_datasets);
            }
            List<ReadyControlElements> rcelist = new List<ReadyControlElements>();
            {
                rcelist.Add(_menu);
                rcelist.Add(_primary);
            }
            List<CalibControlElements> calibs = new List<CalibControlElements>();
            {
                calibs.Add(_menu);
                calibs.Add(_primary);
            }

            fac.ActiveCountable = new ACSplitter(aclist);
            fac.ActiveDataPoint = _menu;
            fac.ActiveDataSet = new ADSESplitter(adselist);
            fac.ActiveFile = _menu;
            fac.Calibration = new CalibSplitter(calibs);
            fac.CountableCtl = _species;
            fac.Countables = new CountableSplitter(ctllist);
            fac.DataPointCtl = _datapts.DataPoints;
            fac.DataPoints = _datapts;
            fac.DataSets = new DSESplitter(dselist);
            //fac.Display
            fac.Files = _menu;
            fac.Global = _menu;
            fac.Stopper = _menu;
            fac.MeasurementCtl = _primary.MTypes;
            fac.Ready = new RCESplitter(rcelist);
            fac.Progress = _progress.Data;
            fac.ControlForm = _controlForm;
        }

        #region WindowElements Members


        Measurer WindowElements.Measurer
        {
            get { return _meas; }
        }

        bool WindowElements.TopMost
        {
            get
            {
                return _videoForm.TopMost;
            }
            set
            {
                _videoForm.TopMost = value;
                _menuForm.TopMost = value;
                _controlForm.TopMost = value;
                _dataPointsForm.TopMost = value;
                _progress.TopMost = value;
            }
        }

        #endregion
    }
}
