using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Z3.Model;
using Zoopomatic2.Controls;
using Z3.Util;

namespace Z3.View
{
    /// <summary>
    /// A User Interface for Z3.
    /// </summary>
    public interface IView
    {
        FileElements Files { get; }
        ActiveFileElements ActiveFile { get; }
        
        DataSetElements DataSets { get; }
        ActiveDataSetElements ActiveDataSet { get; }

        DataPointElements DataPoints { get; }
        ActiveDataPointElements ActiveDataPoint { get; }

        CountableElements Countables { get; }
        ActiveCountableElements ActiveCountable { get; }

        ReadyControlElements Ready { get; }
        GlobalControlElements Global { get; }
        CalibControlElements Calibration { get; }
        WindowElements Display { get; }

        ItemContainer<ZMeasurement> MeasurementCtl { get; }
        ItemContainer<ZCountable> CountableCtl { get; }
        ItemContainer<ZDataPoint> DataPointCtl { get; }

        ListView Progress { get; }
    }

    /// <summary>
    /// An facade for the View interface, which allows Elements to be exchanged.
    /// </summary>
    internal class ViewFacade : IView, WindowElements
    {
        public ViewFacade() {
            closedHandler = new EventHandler<FormClosedEventArgs>(_display_ViewClosed);
            closingHandler = new EventHandler<FormClosingEventArgs>(_display_ViewClosing);
        }

        #region Aspects
        private FileElements _files;
        public FileElements Files
        {
            get { return _files; }
            set { _files = value; }
        }

        private ActiveFileElements _afiles;
        public ActiveFileElements ActiveFile
        {
            get { return _afiles; }
            set { _afiles = value; }
        }

        private DataSetElements _sets;
        public DataSetElements DataSets
        {
            get { return _sets; }
            set { _sets = value; }
        }

        private ActiveDataSetElements _asets;
        public ActiveDataSetElements ActiveDataSet
        {
            get { return _asets; }
            set { _asets = value; }
        }

        private ActiveDataPointElements _apoints;
        public ActiveDataPointElements ActiveDataPoint
        {
            get { return _apoints; }
            set { _apoints = value; }
        }

        private DataPointElements _datapts;
        public DataPointElements DataPoints {
            get { return _datapts; }
            set { _datapts = value; }
        }

        private CountableElements _items;
        public CountableElements Countables
        {
            get { return _items; }
            set { _items = value; }
        }

        private ActiveCountableElements _aitems;
        public ActiveCountableElements ActiveCountable
        {
            get { return _aitems; }
            set { _aitems = value; }
        }

        private ReadyControlElements _ready;
        public ReadyControlElements Ready
        {
            get { return _ready; }
            set { _ready = value; }
        }

        private GlobalControlElements _menu;
        public GlobalControlElements Global
        {
            get { return _menu; }
            set { _menu = value; }
        }

        private CalibControlElements _primary;
        public CalibControlElements Calibration
        {
            get { return _primary; }
            set { _primary = value; }
        }

        private ItemContainer<ZCountable> _itemsctl;
        public ItemContainer<ZCountable> CountableCtl
        {
            get { return _itemsctl; }
            set { _itemsctl = value; }
        }

        private ItemContainer<ZDataPoint> _pointsctl;
        public ItemContainer<ZDataPoint> DataPointCtl
        {
            get { return _pointsctl; }
            set { _pointsctl = value; }
        }

        private ItemContainer<ZMeasurement> _mctl;
        public ItemContainer<ZMeasurement> MeasurementCtl
        {
            get { return _mctl; }
            set { _mctl = value; }
        }

        private WindowElements _display;
        public Measurer Measurer
        {
            get
            {
                return _display.Measurer;
            }
        }

        private ListView _progress;
        public ListView Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = value;
            }
        }

        public bool TopMost
        {
            get
            {
                return _display.TopMost;
            }
            set
            {
                _display.TopMost = value;
            }
        }

        private EventHandler<FormClosedEventArgs> closedHandler;
        private EventHandler<FormClosingEventArgs> closingHandler;
        public WindowElements Display
        {
            get { return this; }
            set {
                if (_display != null)
                {
                    _display.ViewClosed -= closedHandler;
                    _display.ViewClosing -= closingHandler;
                }
                _display = value;
                _display.ViewClosed += closedHandler;
                _display.ViewClosing += closingHandler;
            }
        }

        void _display_ViewClosing(object sender, FormClosingEventArgs e)
        {
            if (ViewClosing != null) ViewClosing(sender, e);
        }

        void _display_ViewClosed(object sender, FormClosedEventArgs e)
        {
            if (ViewClosed != null) ViewClosed(sender, e);
        }

        #endregion
        
        #region WindowElements Members

        public MouseHook Hook
        {
            get
            {
                return _display.Hook;
            }
        }
        void WindowElements.Show()
        {
            _display.Show();
        }

        void WindowElements.Hide()
        {
            _display.Hide();
        }

        void WindowElements.Close()
        {
            _display.Close();
        }

        void WindowElements.Dispose()
        {
            _display.Dispose();
        }

        FormWindowState WindowElements.WindowState
        {
            get
            {
                return _display.WindowState;
            }
            set
            {
                _display.WindowState = value;
            }
        }

        public event EventHandler<FormClosingEventArgs> ViewClosing;
        public event EventHandler<FormClosedEventArgs> ViewClosed;

        #endregion
    }

    #region Element Interfaces
    public interface FileElements
    {
        event EventHandler FileNewClicked;
        event EventHandler FileOpenClicked;
        event EventHandler FileExitClicked;
    }

    public interface ActiveFileElements
    {
        string Name { set; }
        bool Enabled { set; }
        event EventHandler FileImportClicked;
        event EventHandler FileImportFromDBClicked;
        event EventHandler FileExportClicked;
        event EventHandler FileQueryClicked;
        event EventHandler FilePropertiesClicked;
    }

    public interface DataSetElements
    {
        bool Enabled { set; }
        event EventHandler DataSetNewClicked;
        event EventHandler DataSetOpenClicked;
    }

    public interface ActiveDataSetElements
    {
        string Name { set; }
        bool Enabled { set; }
        event EventHandler DataSetPropertiesClicked;
        event EventHandler DataSetDeleteClicked;
    }

    public interface ActiveDataPointElements
    {
        bool Enabled { set; }
        event EventHandler PointEditClicked;
        event EventHandler PointDeleteClicked;
    }

    public interface CountableElements
    {
        bool Enabled { set; }
        event EventHandler CountableNewClicked;
    }

    public interface ActiveCountableElements
    {
        bool Enabled { set; }
        event EventHandler CountableEditClicked;
        event EventHandler CountableDeleteClicked;
        event EventHandler CountableAssignHotkeyClicked;
        event EventHandler CountableClearHotkeyClicked;
    }
    
    public interface ReadyControlElements
    {
        bool Enabled { set; }
        bool Counting { set; }
        event EventHandler StartStopCountingClicked;
        event EventHandler UndoClicked;
        event EventHandler RecalibrateClicked;
    }

    public interface GlobalControlElements
    {
        event EventHandler SchemaEditorClicked;
        event EventHandler OptionsClicked;
    }

    public interface CalibControlElements
    {
        bool Enabled { set; }
        event EventHandler<CalibrationEventArgs> CalibratePerformed;
    }

    public class CalibrationEventArgs : EventArgs
    {
        private double value;
        private int zoom;
        public CalibrationEventArgs(double value, int zoom)
        {
            this.value = value;
            this.zoom = zoom;
        }
        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }
        public int Zoom
        {
            get { return zoom; }
            set { this.zoom = value; }
        }
    }

    public interface ItemContainer<T>
    {
        void Add(T item);
        void Update(T item);
        void Delete(T item);
        void Clear();
        event EventHandler Selected;
        T SelectedItem { get; set; }
        T ContextMenuSubject { get; }
    }

    public interface ItemManipulationElements
    {
        event EventHandler ItemNewClicked;
        event EventHandler ItemEditClicked;
        event EventHandler ItemDeleteClicked;
    }

    public interface DataPointSelectionElements
    {
        event EventHandler Selected;
    }

    #endregion

    public interface WindowElements
    {
        void Show();
        void Hide();
        void Close();
        void Dispose();

        FormWindowState WindowState { get; set; }
        Measurer Measurer { get; }
        bool TopMost { get; set; }
        MouseHook Hook { get; }

        event EventHandler<FormClosingEventArgs> ViewClosing;
        event EventHandler<FormClosedEventArgs> ViewClosed;
    }

}

namespace Z3.View.Util
{

    #region Splitter Facades
    
    internal class CalibSplitter : CalibControlElements
    {
        private List<CalibControlElements> _list;
        public CalibSplitter(List<CalibControlElements> list)
        {
            _list = list;

            foreach (CalibControlElements e in _list)
            {
                e.CalibratePerformed += new EventHandler<CalibrationEventArgs>(e_CalibratePerformed);
            }
        }

        void e_CalibratePerformed(object sender, CalibrationEventArgs e)
        {
            if (CalibratePerformed != null)
                CalibratePerformed(sender, e);
        }

        public event EventHandler<CalibrationEventArgs> CalibratePerformed;

        public bool Enabled
        {
            set
            {
                foreach (CalibControlElements e in _list)
                {
                    e.Enabled = value;
                }
            }
        }
    }
    internal class ACSplitter : ActiveCountableElements
    {
        private List<ActiveCountableElements> _list;
        public ACSplitter(List<ActiveCountableElements> list)
        {
            _list = list;
            foreach (ActiveCountableElements ac in _list)
            {
                ac.CountableDeleteClicked += new EventHandler(ac_CountableDeleteClicked);
                ac.CountableEditClicked += new EventHandler(ac_CountableEditClicked);
                ac.CountableAssignHotkeyClicked += new EventHandler(ac_CountableAssignHotkeyClicked);
                ac.CountableClearHotkeyClicked += new EventHandler(ac_CountableClearHotkeyClicked);
            }
        }

        void ac_CountableClearHotkeyClicked(object sender, EventArgs e)
        {
            if (CountableClearHotkeyClicked != null)
                CountableClearHotkeyClicked(sender, e);
        }

        void ac_CountableAssignHotkeyClicked(object sender, EventArgs e)
        {
            if (CountableAssignHotkeyClicked != null)
                CountableAssignHotkeyClicked(sender, e);
        }

        void ac_CountableEditClicked(object sender, EventArgs e)
        {
            if (CountableEditClicked != null)
                CountableEditClicked(sender, e);
        }

        void ac_CountableDeleteClicked(object sender, EventArgs e)
        {
            if (CountableDeleteClicked != null)
                CountableDeleteClicked(sender, e);
        }

        #region ActiveCountableElements Members

        public bool Enabled
        {
            set
            {
                foreach (ActiveCountableElements ac in _list)
                {
                    ac.Enabled = value;
                }
            }
        }

        public event EventHandler CountableEditClicked;
        public event EventHandler CountableAssignHotkeyClicked;
        public event EventHandler CountableClearHotkeyClicked;

        public event EventHandler CountableDeleteClicked;

        #endregion
    }
    internal class CountableSplitter : CountableElements
    {
        private List<CountableElements> _list;
        public CountableSplitter(List<CountableElements> list)
        {
            _list = list;
            foreach (CountableElements e in _list)
            {
                e.CountableNewClicked += new EventHandler(e_CountableNewClicked);
            }
        }

        void e_CountableNewClicked(object sender, EventArgs e)
        {
            if (CountableNewClicked != null)
                CountableNewClicked(sender, e);
        }

        #region CountableElements Members
        public bool Enabled
        {
            set
            {
                foreach (CountableElements c in _list)
                {
                    c.Enabled = value;
                }
            }
        }

        public event EventHandler CountableNewClicked;

        #endregion
    }
    internal class ADSESplitter : ActiveDataSetElements
    {
        private List<ActiveDataSetElements> _list;

        public ADSESplitter(List<ActiveDataSetElements> list)
        {
            _list = list;
            foreach (ActiveDataSetElements e in _list)
            {
                e.DataSetDeleteClicked += new EventHandler(e_DataSetDeleteClicked);
                e.DataSetPropertiesClicked += new EventHandler(e_DataSetPropertiesClicked);
            }
        }

        void e_DataSetDeleteClicked(object sender, EventArgs e)
        {
            if (DataSetDeleteClicked != null)
                DataSetDeleteClicked(sender, e);
        }

        void e_DataSetPropertiesClicked(object sender, EventArgs e)
        {
            if (DataSetPropertiesClicked != null)
                DataSetPropertiesClicked(sender, e);
        }

        public string Name
        {
            set
            {
                foreach (ActiveDataSetElements a in _list)
                {
                    a.Name = value;
                }
            }
        }

        public bool Enabled
        {
            set
            {
                foreach (ActiveDataSetElements a in _list)
                {
                    a.Enabled = value;
                }
            }
        }

        public event EventHandler DataSetPropertiesClicked;
        public event EventHandler DataSetDeleteClicked;
    }

    internal class DSESplitter : DataSetElements
    {
        private List<DataSetElements> _list;

        public DSESplitter(List<DataSetElements> list)
        {
            _list = list;
            foreach (DataSetElements e in _list)
            {
                e.DataSetNewClicked += new EventHandler(e_DataSetNewClicked);
                e.DataSetOpenClicked += new EventHandler(e_DataSetOpenClicked);
            }
        }

        void e_DataSetOpenClicked(object sender, EventArgs e)
        {
            DataSetOpenClicked(sender, e);
        }

        void e_DataSetNewClicked(object sender, EventArgs e)
        {
            DataSetNewClicked(sender, e);
        }

        #region DataSetElements Members

        public bool Enabled
        {
            set { foreach (DataSetElements e in _list) { e.Enabled = value; } }
        }

        public event EventHandler DataSetNewClicked;

        public event EventHandler DataSetOpenClicked;

        #endregion
    }
    //internal class ADSESplitter : ActiveDataSetElements { }
    //internal class ADPESplitter : ActiveDataPointElements { }
    //internal class CESplitter : CountableElements { }
    //internal class ACESplitter : ActiveCountableElements { }
    internal class RCESplitter : ReadyControlElements
    {
        private List<ReadyControlElements> _list;
        public RCESplitter(List<ReadyControlElements> list)
        {
            _list = list;
            foreach (ReadyControlElements r in list)
            {
                r.RecalibrateClicked += new EventHandler(r_RecalibrateClicked);
                r.StartStopCountingClicked += new EventHandler(r_StartStopCountingClicked);
                r.UndoClicked += new EventHandler(r_UndoClicked);
            }
        }

        void r_ZoomClicked(object sender, EventArgs e)
        {
            if (ZoomClicked != null)
                ZoomClicked(sender, e);
        }

        void r_UndoClicked(object sender, EventArgs e)
        {
            if (UndoClicked != null)
                UndoClicked(sender, e);
        }

        void r_StartStopCountingClicked(object sender, EventArgs e)
        {
            if (StartStopCountingClicked != null)
                StartStopCountingClicked(sender, e);
        }

        void r_RecalibrateClicked(object sender, EventArgs e)
        {
            if (RecalibrateClicked != null)
                RecalibrateClicked(sender, e);
        }

        #region ReadyControlElements Members

        public bool Enabled
        {
            set { foreach (ReadyControlElements e in _list) e.Enabled = value; }
        }

        public bool Counting
        {
            set { foreach (ReadyControlElements e in _list) e.Counting = value; }
        }

        public event EventHandler StartStopCountingClicked;

        public event EventHandler UndoClicked;

        public event EventHandler RecalibrateClicked;

        public event EventHandler ZoomClicked;

        #endregion
    }
    #endregion
}