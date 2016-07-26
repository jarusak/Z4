using System.Windows.Forms;

using Z3.View;
using Z3.Workspace;
using Z3.State;
using System;
using System.Collections.Generic;
using Z3.Model;
using Z3.Util;
using Z3.Forms;
using System.IO;
using System.Data.SqlServerCe;
using System.Text;
using System.Data.Common;
using Z3.View.Util;
using System.Diagnostics;

namespace Z3.Logic
{
    public class LogicLayer : ALogic
    {
        public LogicLayer() : base("LogicLayer")
        {
            StatusBarLogic sbl = new StatusBarLogic();
            Insert(sbl);  // i dont understand what this does
            Insert(new FileMenuLogic());
            DataPointsDisplayLogic points = new DataPointsDisplayLogic(sbl);
            Insert(points);
            Insert(new MeasurementTypeLogic());
            Insert(new DataSetLogic());
            Insert(new OptionsLogic());
            CountingLogic counting = new CountingLogic();
            Insert(counting);
            Insert(new CalibrationLogic(counting));
            Insert(new CountableLogic());
            Insert(new WhatYouCantCountLogic());
            Insert(new KeystrokeLogic(counting, points));
            Insert(new ExportLogic());
            Insert(new ImportLogic());
            Insert(new ShowProgressLogic());
        }
    }

    public abstract class ALogic {
        protected System.ComponentModel.Container components;
        protected Z3.View.IView view;
        protected ProgramState state;
        private bool _initialized = false;
        private bool _disposed = false;
        private ALogic _next;
        private ALogic _prev;
        private string _name;

        public ALogic(string name)
        {
            this.components = new System.ComponentModel.Container();
            _next = this;
            _prev = this;
            _name = name;
        }

        public ALogic Next()
        {
            return _next;
        }

        public void Insert(ALogic l)
        {
            l._next = _next;
            l._next._prev = l;
            _next = l;
            l._prev = this;
        }

        public ALogic Remove()
        {
            if (_next == this) return null;
            _prev._next = _next;
            _next._prev = _prev;
            return _next;
        }

        public ALogic Find(string Name)
        {
            if (_name.Equals(Name))
            {
                return this;
            }
            else
            {
                return _next.Find(Name, _name);
            }
        }

        public ALogic Find(string name, string caller)
        {
            if (_name.Equals(caller))
            {
                throw new IndexOutOfRangeException("Unable to find " + name);
            }

            if (_name.Equals(name))
            {
                return this;
            }
            else
            {
                return _next.Find(name, _name);
            }
        }

        public bool Disposed { get { return _disposed; } }

        public void Dispose()
        {
            if (_disposed) return;
            components.Dispose();
            _disposed = true;
            _next.Dispose();
        }

        public void Initialize(IView view, ProgramState state)
        {
            if (_disposed) throw new ObjectDisposedException("this");
            if (_initialized) return;
            _initialized = true;
            
            this.view = view;
            this.state = state;

            Initialize();

            _next.Initialize(view, state);
        }

        protected virtual void Initialize() { }
    }

    public class StatusBarLogic : ALogic {
        public StatusBarLogic() : base("StatusBarLogic") {}

        private Z3.State.StatusMessages.StatusMessageState _state;
        protected override void Initialize()
        {
            _state = new Z3.State.StatusMessages.InitialState();
            state.CurrentWorkspace.ReplacedPreview += new EventHandler<ObjectEventArgs<Watcher<IWorkspace>>>(CurrentWorkspace_Replaced);
            state.CurrentWorkspace.Cleared += new EventHandler<ObjectEventArgs<Watcher<IWorkspace>>>(CurrentWorkspace_Cleared);
            state.CurrentCountable.ReplacedPreview += new EventHandler<ObjectEventArgs<Watcher<ZCountable>>>(CurrentCountable_Replaced);
            state.CurrentCountable.Cleared += new EventHandler<ObjectEventArgs<Watcher<ZCountable>>>(CurrentCountable_Cleared);
            state.CurrentDataSet.ReplacedPreview += new EventHandler<ObjectEventArgs<Watcher<ZDataSet>>>(CurrentDataSet_Replaced);
            state.CurrentDataSet.Cleared += new EventHandler<ObjectEventArgs<Watcher<ZDataSet>>>(CurrentDataSet_Cleared);
            state.CurrentMeasurement.ReplacedPreview += new EventHandler<ObjectEventArgs<Watcher<ZMeasurement>>>(CurrentMeasurement_Replaced);
            state.CurrentMeasurement.Cleared += new EventHandler<ObjectEventArgs<Watcher<ZMeasurement>>>(CurrentMeasurement_Cleared);
            view.Ready.RecalibrateClicked += new EventHandler(Ready_RecalibrateClicked);
            view.Display.Measurer.Calibrated += new EventHandler(Measurer_Calibrated);
        }

        public event EventHandler MessageChanged;

        void CurrentDataSet_Replaced(object sender, ObjectEventArgs<Watcher<ZDataSet>> e)
        {
            if (state.CurrentDataSet.Loaded && state.CurrentDataSet.Value.Type.Measurable)
            {
                _state = _state.DataSetPicked();
            }
            else
            {
                _state = _state.DataSetCleared();
            }
            
            if (MessageChanged != null)
                MessageChanged(this, new EventArgs());
        }

        void CurrentDataSet_Cleared(object sender, ObjectEventArgs<Watcher<ZDataSet>> e)
        {
            _state = _state.DataSetCleared();
            if (MessageChanged != null)
                MessageChanged(this, new EventArgs());
        }

        void CurrentMeasurement_Replaced(object sender, ObjectEventArgs<Watcher<ZMeasurement>> e)
        {
            _state = _state.MeasurementPicked();
            if (MessageChanged != null)
                MessageChanged(this, new EventArgs());
        }

        void CurrentMeasurement_Cleared(object sender, ObjectEventArgs<Watcher<ZMeasurement>> e)
        {
            _state = _state.MeasurementCleared();
            if (MessageChanged != null)
                MessageChanged(this, new EventArgs());
        }

        void Ready_RecalibrateClicked(object sender, EventArgs e)
        {
            _state = _state.Decalibrated();
            if (MessageChanged != null)
                MessageChanged(this, new EventArgs());
        }

        void Measurer_Calibrated(object sender, EventArgs e)
        {
            _state = _state.Calibrated();
            if (MessageChanged != null)
                MessageChanged(this, new EventArgs());
        }

        void CurrentWorkspace_Cleared(object sender, ObjectEventArgs<Watcher<IWorkspace>> e)
        {
            _state = _state.FileClosed();
            if (MessageChanged != null)
                MessageChanged(this, new EventArgs());
        }

        void CurrentWorkspace_Replaced(object sender, ObjectEventArgs<Watcher<IWorkspace>> e)
        {
            _state = _state.FileLoaded();
            if (MessageChanged != null)
                MessageChanged(this, new EventArgs());
        }

        void CurrentCountable_Cleared(object sender, ObjectEventArgs<Watcher<ZCountable>> e)
        {
            _state = _state.CountableCleared();
            if (MessageChanged != null)
                MessageChanged(this, new EventArgs());
        }

        void CurrentCountable_Replaced(object sender, ObjectEventArgs<Watcher<ZCountable>> e)
        {
            if (state.CurrentCountable.Loaded && state.CurrentCountable.Value.Type.Measurable)
            {
                _state = _state.CountablePicked();
            }
            else
            {
                _state = _state.CountableCleared();
            }

            if (MessageChanged != null)
                MessageChanged(this, new EventArgs());
        }
        public string Message
        {
            get
            {
                return _state.Message;
            }
        }

    }

    /// <summary>
    /// This logic component responds to the commands on the File Menu.
    /// </summary>
    public class FileMenuLogic : ALogic
    {
        /// <summary>
        /// Create a new FileMenuHandler, tying it to the given view and state machine.
        /// </summary>
        /// <param name="view">A Z3 View</param>
        /// <param name="state">A Z3 State Machine</param>
        public FileMenuLogic() : base("FileMenuLogic")
        {
            InitializeComponent();
        }

        protected override void Initialize()
        {   
            view.Files.FileNewClicked += new System.EventHandler(Files_FileNewClicked);
            view.Files.FileOpenClicked += new System.EventHandler(Files_FileOpenClicked);
            view.Files.FileExitClicked += new System.EventHandler(Files_FileExitClicked);
            view.Global.OneWindowClicked += new System.EventHandler(Global_OneWindowClicked);
            state.CurrentWorkspace.Cleared += new EventHandler<ObjectEventArgs<Watcher<IWorkspace>>>(CurrentWorkspace_Cleared);
            state.CurrentWorkspace.Replaced += new EventHandler<ObjectEventArgs<Watcher<IWorkspace>>>(CurrentWorkspace_Replaced);

            view.ActiveFile.Enabled = false;
        }

        /// <summary>
        /// Enables file menu items when a file has been loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CurrentWorkspace_Replaced(object sender, ObjectEventArgs<Watcher<IWorkspace>> e)
        {
            view.ActiveFile.Enabled = true;
            view.ActiveFile.Name = e.Value.Value.ShortFileName;
        }

        /// <summary>
        /// Disables file menu items when a file is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CurrentWorkspace_Cleared(object sender, ObjectEventArgs<Watcher<IWorkspace>> e)
        {
            view.ActiveFile.Enabled = false;
        }

        /// <summary>
        /// Close the View when user requests to exit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Files_FileExitClicked(object sender, System.EventArgs e)
        {
            if (Disposed) throw new ObjectDisposedException("FileMenuHandler");
            view.Display.Close();       
        }

        // handles the OneWindowClicked
        void Global_OneWindowClicked(object sender, System.EventArgs e)
        {
            view.Display.Rearrange();
        }

        /// <summary>
        /// Open File workflow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Files_FileOpenClicked(object sender, System.EventArgs e)
        {
            if (Disposed) throw new ObjectDisposedException("FileMenuHandler");

            // Open Workspace file
            if (openWorkspace.ShowDialog() == DialogResult.Cancel) return;
            if (openWorkspace.FileName == null || openWorkspace.FileName.Equals("")) return;

            // Load into Workspace object
            IWorkspace w = Z3.Workspace.Factory.Load(openWorkspace.FileName);

            // Replace current Workspace with new one
            state.CurrentWorkspace.Value = w;
        }

        /// <summary>
        /// New File workflow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Files_FileNewClicked(object sender, System.EventArgs e)
        {
            if (Disposed) throw new ObjectDisposedException("FileMenuHandler");

            // Use current Schema?
            string schemaFile = null, workFile;
            if (state.CurrentWorkspace.Loaded)
            {
                DialogResult r = MessageBox.Show("Would you like to base " +
                    "the new workspace on the current Schema, " +
                    state.CurrentWorkspace.Value.Schema + "?",
                    "Keep current Schema?", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (r == DialogResult.Cancel) return;
                if (r == DialogResult.Yes) schemaFile = state.CurrentWorkspace.Value.Filename;
            }
            
            // Open Schema file
            if (schemaFile == null)
            {
                if (openSchema.ShowDialog() == DialogResult.Cancel) return;
                if (openSchema.FileName == null || openSchema.FileName.Equals("")) return;
                schemaFile = openSchema.FileName;
            }

            // Which Aspects of the Schema to Copy? (Report Types, Countables)
            bool taxon, rpt;
            using (ImportForm imform = new ImportForm()) {
                imform.TopMost = view.Display.TopMost;
                if (imform.ShowDialog() == DialogResult.Cancel) return;
                taxon = imform.ImportTaxonomy;
                rpt = imform.ImportReports;
            }
            
            // Save Workspace As
            if (newWorkspace.ShowDialog() == DialogResult.Cancel) return;
            if (newWorkspace.FileName == null || newWorkspace.FileName.Equals("")) return;
            workFile = newWorkspace.FileName;
            
            // Load new WorkSpace object and set as current workspace
            try
            {
                IWorkspace newWS = Z3.Workspace.Factory.Create(schemaFile, workFile, taxon, rpt);
               
                //    -> State cascades state changes to everyone
                state.CurrentWorkspace.Value = newWS;
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Failed to create Workspace");
            }
            
        }

        #region Components
        private System.Windows.Forms.OpenFileDialog openSchema;
        private System.Windows.Forms.SaveFileDialog newWorkspace;
        private System.Windows.Forms.OpenFileDialog openWorkspace;
        
        private void InitializeComponent()
        {
            this.openSchema = new System.Windows.Forms.OpenFileDialog();
            this.newWorkspace = new System.Windows.Forms.SaveFileDialog();
            this.openWorkspace = new System.Windows.Forms.OpenFileDialog();
            components.Add(openSchema);
            components.Add(newWorkspace);
            components.Add(openWorkspace);
            // 
            // openSchema
            // 
            this.openSchema.DefaultExt = "z3s";
            this.openSchema.FileName = "Z3Schema";
            this.openSchema.Filter = "Z3 Schemas|*.z3s|All files|*.*";
            this.openSchema.Title = "Select Counting Schema";
            // 
            // newWorkspace
            // 
            this.newWorkspace.DefaultExt = "z3w";
            this.newWorkspace.FileName = "Z3Workspace";
            this.newWorkspace.Filter = "Z3 Workspaces|*.z3w|All files|*.*";
            this.newWorkspace.Title = "Create new Workspace";
            // 
            // openWorkspace
            // 
            this.openWorkspace.DefaultExt = "z3w";
            this.openWorkspace.FileName = "Z3Workspace";
            this.openWorkspace.Filter = "Z3 Workspaces|*.z3w|All files|*.*";
            this.openWorkspace.Title = "Open Workspace";
        }

        #endregion
    }

    /// <summary>
    /// This logic component handles the display of Data Points on the UI.
    /// </summary>
    public class DataPointsDisplayLogic : ALogic
    {
        private StatusBarLogic _status;
        public DataPointsDisplayLogic(StatusBarLogic bar) : base("DataPointDisplayLogic")
        {
            _status = bar;
            _status.MessageChanged += new EventHandler(_status_MessageChanged);
        }

        void _status_MessageChanged(object sender, EventArgs e)
        {
            view.DataPoints.StatusBarLabel = _label + _status.Message;
        }

        protected override void Initialize()
        {
            // Tie to DataPointsElements
            state.CurrentIndividual.Cleared += new EventHandler<ObjectEventArgs<Z3.State.Watcher<ZIndividual>>>(CurrentIndividual_Cleared);
            state.CurrentIndividual.Replaced += new EventHandler<ObjectEventArgs<Z3.State.Watcher<ZIndividual>>>(CurrentIndividual_Replaced);

            state.CurrentDataSet.Cleared += new EventHandler<ObjectEventArgs<Z3.State.Watcher<ZDataSet>>>(CurrentDataSet_Cleared);
            state.CurrentDataSet.Replaced += new EventHandler<ObjectEventArgs<Z3.State.Watcher<ZDataSet>>>(CurrentDataSet_Replaced);
            
            state.CurrentDataPoint.Cleared += new EventHandler<ObjectEventArgs<Z3.State.Watcher<ZDataPoint>>>(CurrentDataPoint_Cleared);
            state.CurrentDataPoint.Replaced += new EventHandler<ObjectEventArgs<Z3.State.Watcher<ZDataPoint>>>(CurrentDataPoint_Replaced);
            state.CurrentCountable.Cleared += new EventHandler<ObjectEventArgs<Watcher<ZCountable>>>(CurrentCountable_Cleared);
            view.DataPointCtl.Selected += new EventHandler(DataPointCtl_Selected);

            view.DataPoints.Enabled = false;
            view.ActiveDataPoint.Enabled = false;

            view.ActiveDataPoint.PointEditClicked += new EventHandler(ActiveDataPoint_PointEditClicked);
            view.ActiveDataPoint.PointDeleteClicked += new EventHandler(ActiveDataPoint_PointDeleteClicked);
            view.DataPoints.PointDeleteClicked += new EventHandler(ActiveDataPoint_PointDeleteClicked);
            view.DataPoints.PointEditClicked += new EventHandler(ActiveDataPoint_PointEditClicked);

            myAdd = new EventHandler<ObjectEventArgs<ZDataPoint>>(_indiv_PointAdded);
            myEdit = new EventHandler<ObjectEventArgs<ZDataPoint>>(_indiv_PointModified);
            myDelete = new EventHandler<ObjectEventArgs<ZDataPoint>>(_indiv_PointDeleted);
        }

        void CurrentCountable_Cleared(object sender, ObjectEventArgs<Watcher<ZCountable>> e)
        {
            state.CurrentIndividual.Clear();
        }

        void DataPointCtl_Selected(object sender, EventArgs e)
        {
            if (view.DataPointCtl.SelectedItem == null) state.CurrentDataPoint.Clear();
            else state.CurrentDataPoint.Value = view.DataPointCtl.SelectedItem;
        }

        void ActiveDataPoint_PointDeleteClicked(object sender, EventArgs e)
        {
            state.CurrentIndividual.Value = state.CurrentDataPoint.Value.Individual;
            state.CurrentDataPoint.Value.Delete();
            if (state.CurrentIndividual.Value.DataPoints.GetAll().Count == 0)
            {
                state.CurrentIndividual.Clear();
            }
        }

        public void EditDataPoint()
        {
            if (state.CurrentDataPoint.Loaded)
            {
                ActiveDataPoint_PointEditClicked(null, null);
            }
        }

        private bool _shown = false;
        void ActiveDataPoint_PointEditClicked(object sender, EventArgs e)
        {
            if (_shown) return;
            _shown = true;
            state.CurrentIndividual.Value = state.CurrentDataPoint.Value.Individual;

            using (DataPointEditForm f = new DataPointEditForm())
            {
                f.TopMost = view.Display.TopMost;
                f.Type = state.CurrentDataPoint.Value.MeasurementType.Name;
                f.Value = state.CurrentDataPoint.Value.Value;

                if (f.ShowDialog() == DialogResult.OK)
                {
                    state.CurrentDataPoint.Value.Value = f.Value;
                }
            }
            _shown = false;
        }

        private EventHandler<ObjectEventArgs<ZDataPoint>> myAdd;
        private EventHandler<ObjectEventArgs<ZDataPoint>> myEdit;
        private EventHandler<ObjectEventArgs<ZDataPoint>> myDelete;
        
        void CurrentDataPoint_Replaced(object sender, ObjectEventArgs<Z3.State.Watcher<ZDataPoint>> e)
        {
            // Show the new selection
            view.DataPointCtl.SelectedItem = e.Value.Value;
            view.ActiveDataPoint.Enabled = true;

            state.CurrentIndividual.Value = e.Value.Value.Individual;
        }

        void CurrentDataPoint_Cleared(object sender, ObjectEventArgs<Z3.State.Watcher<ZDataPoint>> e)
        {
            // Clear the selection
            view.DataPointCtl.SelectedItem = null;
            view.ActiveDataPoint.Enabled = false;
        }

        void CurrentDataSet_Replaced(object sender, ObjectEventArgs<Z3.State.Watcher<ZDataSet>> e)
        {
            // Refresh the data points view
            view.DataPointCtl.Clear();
            foreach (ZIndividual i in e.Value.Value.Individuals)
            {
                
                foreach (ZDataPoint p in i.DataPoints.GetAll())
                {
                    view.DataPointCtl.Add(p);
                }
            }
            view.DataPoints.Enabled = true;
        }

        void CurrentDataSet_Cleared(object sender, ObjectEventArgs<Z3.State.Watcher<ZDataSet>> e)
        {
            // Clear the data points view
            view.DataPointCtl.Clear();
            view.DataPoints.Enabled = false;
        }

        private ZIndividual _curidv = null;
        private string _label;
        void CurrentIndividual_Replaced(object sender, ObjectEventArgs<Z3.State.Watcher<ZIndividual>> e)
        {
            // Change status bar text
            ZIndividual _indiv = e.Value.Value;
            if (_indiv == null)
            {
                _label = "*New Individual - ";
            }
            else
            {
                //_label = _indiv.ID + " " + _indiv.Countable.Name + " - <Esc> to close this individual - ";
                _indiv.PointAdded += myAdd;
                _indiv.PointModified += myEdit;
                _indiv.PointDeleted += myDelete;
                _curidv = _indiv;
            }
            _status_MessageChanged(this, new EventArgs());
        }

        void _indiv_PointDeleted(object sender, ObjectEventArgs<ZDataPoint> e)
        {
            view.DataPointCtl.Delete(e.Value);
        }

        void _indiv_PointModified(object sender, ObjectEventArgs<ZDataPoint> e)
        {
            view.DataPointCtl.Update(e.Value);
        }

        void _indiv_PointAdded(object sender, ObjectEventArgs<ZDataPoint> e)
        {
            view.DataPointCtl.Add(e.Value);
        }

        void CurrentIndividual_Cleared(object sender, ObjectEventArgs<Z3.State.Watcher<ZIndividual>> e)
        {
            // Clear status bar text
            if (_curidv != null)
            {
                _curidv.PointAdded -= myAdd;
                _curidv.PointModified -= myEdit;
                _curidv.PointDeleted -= myDelete;
            }

            _label = "*New Individual - ";
            _status_MessageChanged(null, null);
        }
    }

    public class CountingLogic : ALogic
    {
        public CountingLogic() : base("CountingLogic") { }
        private bool _counting;
        public bool Counting
        {
            get
            {
                return _counting;
            }
            set
            {
                _counting = value;

                if (!value)
                {
                    view.Display.Measurer.clear();
                }

                view.Display.TopMost = value;
                view.Display.Measurer.Enabled = value;
                view.Ready.Counting = value;
            }
        }

        protected override void Initialize()
        {
            view.Ready.StartStopCountingClicked += new EventHandler(Ready_StartStopCountingClicked);
            view.Display.Measurer.Measure += new EventHandler<Zoopomatic2.Controls.Measurer.MeasureEventArgs>(Measurer_Measure);
            view.Display.Measurer.Count += new EventHandler(Measurer_Count);
        }

        void Measurer_Count(object sender, EventArgs e)
        {
            try
            {
                Count();
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show(ex.Message, "Count Failed");
            }
        }

        void Measurer_Measure(object sender, Zoopomatic2.Controls.Measurer.MeasureEventArgs e)
        {
            try
            {
                // caculate the estamated weight
                Dictionary<ZField, ZFieldValue> countableFields = view.CountableCtl.ContextMenuSubject.GetValues();
                int a = 0;
                int b = 0;
                double weight = 0;

                foreach (ZFieldValue v in countableFields.Values)
                {
                    if (v.Field.Name.Equals("A"))
                    {
                        a = Int32.Parse(v.ReadableValue.ToString());
                    }
                    else if (v.Field.Name.Equals("B"))
                    {
                        b = Int32.Parse(v.ReadableValue.ToString());
                    }
                }

                weight = a + b + e.convertedLength;
                
                Measure(e.convertedLength, weight);
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show(ex.Message, "Measurement Failed");
            }
        }

        void Ready_StartStopCountingClicked(object sender, EventArgs e)
        {
            Counting = !Counting;
        }

        public void Count()
        {
            CheckState();

            Count(state.CurrentIndividual.Value, state.CurrentMeasurement.Value);
        }

        public void Measure(double value, double weight)
        {
            CheckState();

            Measure(state.CurrentIndividual.Value, state.CurrentMeasurement.Value, value, weight);
        }

        public void CheckState()
        {
            CheckState(true, true, true);
        }

        public void CheckState(bool checkMType, bool checkDS, bool checkSpecies)
        {
            if (!_counting)
            {
                throw new OperationCanceledException("Z3 is not ready for counting yet.  You must hit Start Counting to begin.");
            }

            if (checkMType && !state.CurrentMeasurement.Loaded)
            {
                throw new OperationCanceledException("You must choose a Measurement Type before you can record any data points.");
            }

            if ((checkDS || !state.CurrentIndividual.Loaded) && !state.CurrentDataSet.Loaded)
            {
                throw new OperationCanceledException("You must select a valid data set to record data points into.");
            }

            if (checkSpecies && !state.CurrentCountable.Loaded)
            {
                throw new OperationCanceledException("You must indicate what type of thing you are counting (something Countable like a Species).");
            }

            if (!state.CurrentIndividual.Loaded)
            {
                state.CurrentIndividual.Value = state.CurrentDataSet.Value.AddIndividual(state.CurrentCountable.Value, state.CurrentDataSet.Value);
            }
        }

        public void Count(ZMeasurement mtype)
        {
            CheckState(false, true, true);

            Count(state.CurrentIndividual.Value, mtype);
        }

        public void Count(ZIndividual indiv, ZMeasurement mtype)
        {
            if (!mtype.Counted)
            {
                if (mtype.CountTypeID > 0)
                {
                    mtype = mtype.CountType;
                }
                else
                {
                    throw new OperationCanceledException("You must measure a distance in order to record a " + mtype.Name + ".\n\nReason: This Schema specifies that it does not make sense to Count this.");
                }
            }

            if (!mtype.Increment && indiv.DataPoints.Get(mtype, false) != null)
            {
                //This type has already been measured on this individual
                // and it is not set to Increment the existing count
                // So we need to create a new individual.
                indiv = indiv.DataSet.AddIndividual(state.CurrentCountable.Value, state.CurrentDataSet.Value);
                state.CurrentIndividual.Value = indiv;
            }

            if (mtype.AutoCount)
            {
                ZDataPoint auto = indiv.DataPoints.Get(mtype.AutoType, true);
                auto.Value = 1;
            }
            
            ZDataPoint point = indiv.DataPoints.Get(mtype, true);
            point.Value = point.Value + 1;
            state.CurrentDataPoint.Value = point;
        }

        public void Measure(ZIndividual indiv, ZMeasurement mtype, double value, double weight)
        {
            if (!mtype.Measured)
            {
                throw new OperationCanceledException("You cannot measure a " + mtype.Name + ".\n\nReason: This Schema specifies that it does not make sense to Measure this.");
            }

            if (!mtype.Increment && indiv.DataPoints.Get(mtype, false) != null
                || (mtype.AutoCount && indiv.DataPoints.Get(mtype.AutoType, false) != null))
            {
                //This type has already been measured on this individual
                // and it is not set to Increment the existing count
                // So we need to create a new individual.
                //Increment for Counts means, increment by 1
                //Increment for Measurements means you can have more than 1 per indiv
                indiv = indiv.DataSet.AddIndividual(indiv.Countable, state.CurrentDataSet.Value);
                state.CurrentIndividual.Value = indiv;
            }

            if (mtype.AutoCount)
            {
                ZDataPoint auto = indiv.DataPoints.Get(mtype.AutoType, true);
                auto.Value = 1;
            }

            ZDataPoint point = indiv.DataPoints.Add(mtype, value, weight);
            state.CurrentDataPoint.Value = point;
        }


        /*public void Count(ZMeasurement mtype, ZCountable zSpec) {
            
            if (zSpec == null)
            {
                _brain.ShowError("Error: No Countable Selected?");
                return;
            }
            else if (_currentDataSet == null)
            {
                _brain.ShowError("Error: No Data Set selected");
                return;
            }

            if (!zSpec.Type.Measurable)
            {
                _brain.ShowError("You cannot count a " + zSpec.Type.Name + ".");
                return;
            }


            if (mtype.AutoCount)
            {
                if (Individual == null ||
                    (!mtype.Increment && Individual.DataPoints.Get(mtype, false) != null))
                {
                    Individual = ZIndividual.insert(_currentDataSet, zSpec, "");
                    _indivmap.Add(Individual.ID, Individual);
                }
                ZDataPoint apoint = Individual.DataPoints.Get(mtype.AutoType, true);
                apoint.Value = 1;
                _display.AddOrUpdate(Individual, apoint);
            }

            ZDataPoint point;
            // Get the value for the current individual, and increment it by one
            if (Individual == null ||
                (!mtype.Increment && Individual.DataPoints.Get(mtype, false) != null))
            {
                Individual = ZIndividual.insert(_currentDataSet, zSpec, "");
                _indivmap.Add(Individual.ID, Individual);
                point = Individual.DataPoints.Add(mtype, 1);
            } else {
                point = Individual.DataPoints.Get(mtype, true);
                point.Value = point.Value + 1;
            }
            _display.AddOrUpdate(Individual, point);

        }*/

        /*public void Measure(ZMeasurement mtype, ZCountable zSpec, double value) {
            // make sure it's a measurable type
            if (mtype == null)
            {
                _brain.ShowError("ILLEGAL PROGRAM STATE: You have not loaded a file. Please load a file and select a ZDataSet to record your measurements");
                return;
            }
            else if (zSpec == null)
            {
                _brain.ShowError("Error: No Species selected");
                return;
            }
            else if (_currentDataSet == null)
            {
                _brain.ShowError("Error: No Data Set selected");
                return;
            }

            if (!zSpec.Type.Measurable)
            {
                _brain.ShowError("You cannot measure data points at the " + zSpec.Type.Name + " level.");
                return;
            }
            
            if (mtype.Measured)
            {
                if (mtype.AutoCount)
                    Count(mtype.AutoType, zSpec);

                // Does this mtype require creation of a new individual, or is indiv null?
                if (Individual == null ||  //no indiv selected
                    (!mtype.Increment && Individual.DataPoints.Get(mtype, false) != null))
                    //we can only have one && there already is one
                {
                    Individual = ZIndividual.insert(_currentDataSet, zSpec, "");
                    _indivmap.Add(Individual.ID, Individual);
                }

                ZDataPoint point = Individual.DataPoints.Add(mtype, value);
                _display.Add(Individual, point);
            }
            else
            {
                //Error: Cannot measure this type
                _brain.ShowError("You cannot measure a " + mtype.Name + ".");
                return;
            }
        }*/

        //private void measHelper(ZMeasurement mtype, ZContainer zContainer, ZCountable zSpec, double value) {
        // Individual functionality should go here
        //if (zSpec == null) {
        //    _brain.ShowError("ILLEGAL PROGRAM STATE: You have not selected anything to count/measure!");
        //} else if (zContainer == null) {
        //    _brain.ShowError("ILLEGAL PROGRAM STATE: You have not selected a data set to record your measurements in!");
        // Must stop mouse events since the overlay window is 'top' 
        //if (_brain.WindowMgr.isFloating())
        //{
        //    Z3FloatingVideoForm x = (Z3FloatingVideoForm)_brain.WindowMgr.Windows.FindAll(isFloatingForm)[0];
        //    if (x != null)
        //        x.stopHook();
        //}
        //_brain.ContainerForm.Show();
        //_brain.ContainerForm.Focus();
        //_brain.ContainerForm.Select();
        //} else {
        //    ZDataPoint dp = ZDataPoint.insert(mtype.ID, zContainer.TypeID, zContainer.ID, zSpec.TypeID, zSpec.ID, value);
        //    ListViewItem i = new ListViewItem(new string[] { dp.ID.ToString(), mtype.Name + "=" + dp.Value, zSpec.Name});
        //    dataPoints.Items.Add(i);
        //    i.EnsureVisible();
        //}
        //}

    }

    public class KeystrokeLogic : ALogic
    {
        public KeystrokeLogic(CountingLogic c, DataPointsDisplayLogic dl) : base("KeystrokeLogic")
        {
            _counting = c;
            _points = dl;
        }

        private DataPointsDisplayLogic _points;
        private CountingLogic _counting;
        private Dictionary<Keys, HotkeyCommand> _hotkeys;
        private HotkeyContext _hkcontext;

        protected override void Initialize()
        {
            _hotkeys = new Dictionary<Keys, HotkeyCommand>();
            _hkcontext = new HotkeyContext(view, this, state);

            view.Display.Hook.KeyUp += new KeyEventHandler(Hook_KeyUp);
            view.Display.Hook.Start();

            state.CurrentWorkspace.Replaced += new EventHandler<ObjectEventArgs<Watcher<IWorkspace>>>(CurrentWorkspace_Replaced);
            view.ActiveCountable.CountableAssignHotkeyClicked += new EventHandler(ActiveCountable_CountableAssignHotkeyClicked);
            view.ActiveCountable.CountableClearHotkeyClicked += new EventHandler(ActiveCountable_CountableClearHotkeyClicked);
        }

        private void ActiveCountable_CountableClearHotkeyClicked(object sender, EventArgs e)
        {
            ZCountable zc = getPicked();
            removeHotkey(zc.Hotkey);
            zc.Hotkey = Keys.None;
        }

        private ZCountable getPicked()
        {
            ZCountable zc = view.CountableCtl.ContextMenuSubject;
            if (zc == null) zc = state.CurrentCountable.Value;
            return zc;
        }

        private void ActiveCountable_CountableAssignHotkeyClicked(object sender, EventArgs e)
        {
            using (HotkeyRecordingForm hrf = new HotkeyRecordingForm())
            {
                ZCountable zc = getPicked();

                hrf.TopMost = view.Display.TopMost;
                hrf.Key = zc.Hotkey;
                if (hrf.ShowDialog() == DialogResult.OK)
                {
                    if (zc.Hotkey != hrf.Key)
                    {
                        removeHotkey(zc.Hotkey);
                        zc.Hotkey = hrf.Key;
                        assignHotkey(new HotkeyCommand(CountableHotkeyCommand, _hkcontext, zc));
                    }
                }
            }
        }

        private static void CountableHotkeyCommand(HotkeyContext context, Hotkeyable zc)
        {
            
            CountingLogic counting = (CountingLogic)context.Logic.Find("CountingLogic");
            if (counting.Counting)
            {
                context.State.CurrentCountable.Value = (ZCountable)zc;
                counting.Count();
            }
        }

        private static void MTypeHotkeyCommand(HotkeyContext context, Hotkeyable zm)
        {
            CountingLogic counting = (CountingLogic)context.Logic.Find("CountingLogic");
            if (counting.Counting)
            {
                //context.State.CurrentMeasurement.Value = (ZMeasurement)zm;
                try
                {
                    ((CountingLogic)context.Logic.Find("CountingLogic")).Count((ZMeasurement)zm);
                }
                catch (OperationCanceledException ex)
                {
                    MessageBox.Show("Unable to count this!  " + ex.Message, "Hotkey failed");
                }
            }
        }

        private void CurrentWorkspace_Replaced(object sender, ObjectEventArgs<Watcher<IWorkspace>> e)
        {
            _hotkeys.Clear();

            foreach (ZCountable c in state.CurrentWorkspace.Value.CountableStore.All)
            {
                assignHotkey(c);
            }

            foreach (ZMeasurement m in state.CurrentWorkspace.Value.MeasurementTypes)
            {
                assignHotkey(m);
            }
        }

        private void assignHotkey(ZCountable c)
        {
            assignHotkey(new HotkeyCommand(CountableHotkeyCommand, _hkcontext, c));
        }

        private void assignHotkey(ZMeasurement m)
        {
            assignHotkey(new HotkeyCommand(MTypeHotkeyCommand, _hkcontext, m));
        }

        private void assignHotkey(HotkeyCommand c)
        {
            if (!Keyboard.IsMeaningful(c.Hotkey)) return;

            try
            {
                if (_hotkeys.ContainsKey(c.Hotkey))
                {
                    HotkeyCommand old = _hotkeys[c.Hotkey];
                    MessageBox.Show("Hotkey [" + Keyboard.GetReadableKey(c.Hotkey) + "] was already assigned to [" +
                            old.Name + "].  It will now be assigned to " + c.Name + " instead.");
                    removeHotkey(old.Hotkey);
                    old.Hotkey = Keys.None;
                }

                _hotkeys.Add(c.Hotkey, c);
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void removeHotkey(Keys value)
        {
            if (!Keyboard.IsMeaningful(value)) return;
            _hotkeys.Remove(value);
        }

        void Hook_KeyUp(object sender, KeyEventArgs e)
        {
            if (_hotkeys == null) return;
            if (_counting.Counting)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    e.Handled = true;
                    state.CurrentIndividual.Clear();
                }
                else if (e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod)
                {
                    e.Handled = true;
                    _points.EditDataPoint();
                }
                else if (_hotkeys.ContainsKey(e.KeyData))
                {
                    e.Handled = true;
                    _hotkeys[e.KeyData].Perform();

                    //if (_counting.Counting)
                    //{
                    //    try
                    //    {
                    //        _counting.Count();
                    //    }
                    //    catch (OperationCanceledException ex)
                    //    {
                    //        MessageBox.Show(ex.Message, "Count Failed");
                    //    }
                    //}
                }
            }
        }
    }

    public class MeasurementTypeLogic : ALogic
    {
        public MeasurementTypeLogic() : base("MeasurementTypeLogic") { }
        protected override void Initialize()
        {
            state.CurrentWorkspace.Replaced += new EventHandler<ObjectEventArgs<Watcher<IWorkspace>>>(CurrentWorkspace_Replaced);
            state.CurrentMeasurement.Cleared += new EventHandler<ObjectEventArgs<Watcher<ZMeasurement>>>(CurrentMeasurement_Cleared);
            state.CurrentMeasurement.Replaced += new EventHandler<ObjectEventArgs<Watcher<ZMeasurement>>>(CurrentMeasurement_Replaced);
            view.MeasurementCtl.Selected += new EventHandler(MeasurementSelector_MTypeSelected);
        }

        void CurrentMeasurement_Replaced(object sender, ObjectEventArgs<Watcher<ZMeasurement>> e)
        {
            // Change the drop-down box to reflect the new selection
            view.MeasurementCtl.SelectedItem = e.Value.Value;
        }

        void CurrentMeasurement_Cleared(object sender, ObjectEventArgs<Watcher<ZMeasurement>> e)
        {
            // Clear the drop-down box
            view.MeasurementCtl.SelectedItem = null;
        }

        void MeasurementSelector_MTypeSelected(object sender, EventArgs e)
        {
            // Change the Current Measurement Type
            state.CurrentMeasurement.Value = view.MeasurementCtl.SelectedItem;
        }

        void CurrentWorkspace_Replaced(object sender, ObjectEventArgs<Watcher<IWorkspace>> e)
        {
            view.MeasurementCtl.Clear();
            ZMeasurement def = null;
            // Load Measurement Types
            foreach (ZMeasurement m in state.CurrentWorkspace.Value.MeasurementTypes)
            {
                if (m.Displayed)
                {
                    view.MeasurementCtl.Add(m);
                }

                if (m.Default)
                {
                    def = m;
                }
            }

            if (def != null)
            {
                state.CurrentMeasurement.Value = def;
            }
        }
    }

    public class WhatYouCantCountLogic : ALogic
    {
        public WhatYouCantCountLogic() : base("WhatYouCantCountLogic") { }
        protected override void Initialize()
        {
            state.CurrentDataSet.Replaced += new EventHandler<ObjectEventArgs<Watcher<ZDataSet>>>(CurrentDataSet_Replaced);
        }

        void CurrentDataSet_Replaced(object sender, ObjectEventArgs<Watcher<ZDataSet>> e)
        {
            if (e.Value.Value != null)
            {
                if (!e.Value.Value.Type.Measurable)
                {
                    MessageBox.Show("This schema does not allow you to record data at the " + e.Value.Value.Type.Name + " level.");
                    e.Value.Clear();
                }
            }
        }
    }

    public class DataSetLogic : ALogic
    {
        public DataSetLogic() : base("DataSetLogic") { }
        protected override void Initialize()
        {
            view.DataSets.DataSetNewClicked += new EventHandler(DataSets_DataSetNewClicked);
            view.DataSets.DataSetOpenClicked += new EventHandler(DataSets_DataSetOpenClicked);
            view.ActiveDataSet.DataSetPropertiesClicked += new EventHandler(ActiveDataSet_DataSetPropertiesClicked);
            view.ActiveDataSet.DataSetDeleteClicked += new EventHandler(ActiveDataSet_DataSetDeleteClicked);
            state.CurrentWorkspace.Cleared += new EventHandler<ObjectEventArgs<Watcher<IWorkspace>>>(CurrentWorkspace_Cleared);
            state.CurrentWorkspace.Replaced += new EventHandler<ObjectEventArgs<Watcher<IWorkspace>>>(CurrentWorkspace_Replaced);
            state.CurrentDataSet.Cleared += new EventHandler<ObjectEventArgs<Watcher<ZDataSet>>>(CurrentDataSet_Cleared);
            state.CurrentDataSet.Modified += new EventHandler<ObjectEventArgs<Watcher<ZDataSet>>>(CurrentDataSet_Modified);
            state.CurrentDataSet.Replaced += new EventHandler<ObjectEventArgs<Watcher<ZDataSet>>>(CurrentDataSet_Replaced);

            view.ActiveDataSet.Enabled = false;
            view.DataSets.Enabled = false;
        }

        void ActiveDataSet_DataSetDeleteClicked(object sender, EventArgs e)
        {
            try
            {
                state.CurrentDataSet.Value.Delete();
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show(ex.Message, "Cannot delete this Data Set");
            }
        }

        void ActiveDataSet_DataSetPropertiesClicked(object sender, EventArgs e)
        {
            using (EditForm ef = new EditForm(state.CurrentDataSet.Value))
            {
                ef.TopMost = view.Display.TopMost;
                ef.ShowDialog();
            }
        }

        void DataSets_DataSetNewClicked(object sender, EventArgs e)
        {
            ZDataSet parent;
            ZLevel level;

            using (NewEntityForm nf = new NewEntityForm(state.CurrentWorkspace.Value, "Container"))
            {
                nf.TopMost = view.Display.TopMost;

                if (nf.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                parent = (ZDataSet)nf.ParentItem;
                level = nf.Type;
            }

            ZDataSet newDS = state.CurrentWorkspace.Value.DataSetStore.Create(parent);
            using (EditForm ef = new EditForm(newDS))
            {
                ef.TopMost = view.Display.TopMost;
                if (ef.ShowDialog() == DialogResult.Cancel)
                {
                    newDS.Delete();
                }
                else
                {
                    state.CurrentDataSet.Value = newDS;
                }
            }
        }

        private TreeWrapper<ZDataSet> _tree;
        void DataSets_DataSetOpenClicked(object sender, EventArgs e)
        {
            // Select Data Set
            using (BrowserForm bf = new BrowserForm())
            {
                bf.Text = "Select Data Set";
                bf.TopMost = view.Display.TopMost;
                using (_tree = new TreeWrapper<ZDataSet>(bf.Tree, "Container", "Data Set"))
                {
                    foreach (ZDataSet ds in state.CurrentWorkspace.Value.DataSetStore.All)
                    {
                        _tree.Add(ds);
                    }
                    _tree.ItemNewClicked += new EventHandler(_tree_ItemNewClicked);
                    _tree.ItemEditClicked += new EventHandler(tree_ItemEditClicked);
                    _tree.ItemDeleteClicked += new EventHandler(tree_ItemDeleteClicked);
                    EventHandler<ObjectEventArgs<ZDataSet>> myAdd = new EventHandler<ObjectEventArgs<ZDataSet>>(DataSetStore_ItemAdded);
                    EventHandler<ObjectEventArgs<ZDataSet>> myEdit = new EventHandler<ObjectEventArgs<ZDataSet>>(DataSetStore_ItemEdited);
                    EventHandler<ObjectEventArgs<ZDataSet>> myDelete = new EventHandler<ObjectEventArgs<ZDataSet>>(DataSetStore_ItemDeleted);
                    state.CurrentWorkspace.Value.DataSetStore.ItemAdded += myAdd;
                    state.CurrentWorkspace.Value.DataSetStore.ItemEdited += myEdit;
                    state.CurrentWorkspace.Value.DataSetStore.ItemDeleted += myDelete;
                    
                    if (bf.ShowDialog() == DialogResult.OK)
                    {
                        state.CurrentDataSet.Value = (ZDataSet)(_tree.SelectedItem);
                    }
                    Debug.WriteLine("Currenet Data Set" + state.CurrentDataSet.Value);
                    state.CurrentWorkspace.Value.DataSetStore.ItemAdded -= myAdd;
                    state.CurrentWorkspace.Value.DataSetStore.ItemEdited -= myEdit;
                    state.CurrentWorkspace.Value.DataSetStore.ItemDeleted -= myDelete;
                }
            }
        }

        void _tree_ItemNewClicked(object sender, EventArgs e)
        {
            ZDataSet parent;
            ZLevel level;

            using (NewEntityForm nf = new NewEntityForm(state.CurrentWorkspace.Value, "Container"))
            {
                nf.TopMost = view.Display.TopMost;
                if (_tree.ContextMenuSubject != null
                        && !_tree.ContextMenuSubject.Type.Final)
                {
                    nf.TypeID = _tree.ContextMenuSubject.Type.ChildID;
                    nf.ParentItemID = _tree.ContextMenuSubject.ID;
                }

                if (nf.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                parent = (ZDataSet)nf.ParentItem;
                level = nf.Type;
            }

            ZDataSet newDS = state.CurrentWorkspace.Value.DataSetStore.Create(parent);
            using (EditForm ef = new EditForm(newDS))
            {
                ef.TopMost = view.Display.TopMost;
                if (ef.ShowDialog() == DialogResult.Cancel)
                {
                    newDS.Delete();
                }
            }
        }

        void CurrentDataSet_Replaced(object sender, ObjectEventArgs<Watcher<ZDataSet>> e)
        {
            view.ActiveDataSet.Enabled = true;
            view.ActiveDataSet.Name = e.Value.Value.Name;
        }

        void CurrentDataSet_Modified(object sender, ObjectEventArgs<Watcher<ZDataSet>> e)
        {
            view.ActiveDataSet.Name = e.Value.Value.Name;
        }

        void CurrentDataSet_Cleared(object sender, ObjectEventArgs<Watcher<ZDataSet>> e)
        {
            view.ActiveDataSet.Enabled = false;
        }

        void CurrentWorkspace_Replaced(object sender, ObjectEventArgs<Watcher<IWorkspace>> e)
        {
            view.DataSets.Enabled = true;
            if (!state.CurrentDataSet.Loaded) view.ActiveDataSet.Name = "< No Data Set >";
        }

        void CurrentWorkspace_Cleared(object sender, ObjectEventArgs<Watcher<IWorkspace>> e)
        {
            view.DataSets.Enabled = false;
        }

        void DataSetStore_ItemDeleted(object sender, ObjectEventArgs<ZDataSet> e)
        {
            _tree.Delete(e.Value);
        }

        void DataSetStore_ItemEdited(object sender, ObjectEventArgs<ZDataSet> e)
        {
            _tree.Update(e.Value);
        }

        void DataSetStore_ItemAdded(object sender, ObjectEventArgs<ZDataSet> e)
        {
            _tree.Add(e.Value);
        }

        void tree_ItemDeleteClicked(object sender, EventArgs e)
        {
            try
            {
                _tree.ContextMenuSubject.Delete();
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show("Unable to delete item.  " + ex.Message);
            }
        }

        void tree_ItemEditClicked(object sender, EventArgs e)
        {
            using (EditForm ef = new EditForm(_tree.ContextMenuSubject))
            {
                ef.TopMost = view.Display.TopMost;
                ef.ShowDialog();
            }
        }
    }

    public class CountableLogic : ALogic
    {
        public CountableLogic() : base("CountableLogic") { }
        protected override void Initialize()
        {
            view.ActiveCountable.Enabled = false;
            view.Countables.Enabled = false;
            
            view.Countables.CountableNewClicked += new EventHandler(Countables_CountableNewClicked);
            
            state.CurrentCountable.Cleared += new EventHandler<ObjectEventArgs<Watcher<ZCountable>>>(CurrentCountable_Cleared);
            state.CurrentCountable.Replaced += new EventHandler<ObjectEventArgs<Watcher<ZCountable>>>(CurrentCountable_Replaced);
            
            state.CurrentWorkspace.Cleared += new EventHandler<ObjectEventArgs<Watcher<IWorkspace>>>(CurrentWorkspace_Cleared);
            state.CurrentWorkspace.Replaced += new EventHandler<ObjectEventArgs<Watcher<IWorkspace>>>(CurrentWorkspace_Replaced);

            view.ActiveCountable.CountableEditClicked += new EventHandler(ActiveCountable_CountableEditClicked);
            view.ActiveCountable.CountableDeleteClicked += new EventHandler(ActiveCountable_CountableDeleteClicked);

            view.CountableCtl.Selected += new EventHandler(CountableCtl_Selected);

            //if (bf.ShowDialog() == DialogResult.OK)
            //{
            //    state.CurrentDataSet.Value = (ZDataSet)(_tree.SelectedItem);
            //}

            //state.CurrentWorkspace.Value.DataSetStore.ItemAdded -= myAdd;
            //state.CurrentWorkspace.Value.DataSetStore.ItemEdited -= myEdit;
            //state.CurrentWorkspace.Value.DataSetStore.ItemDeleted -= myDelete;
        }

        void CountableCtl_Selected(object sender, EventArgs e)
        {
            state.CurrentCountable.Value = view.CountableCtl.SelectedItem;
        }

        void CountableStore_ItemDeleted(object sender, ObjectEventArgs<ZCountable> e)
        {
            view.CountableCtl.Delete(e.Value);
        }

        void CountableStore_ItemEdited(object sender, ObjectEventArgs<ZCountable> e)
        {
            view.CountableCtl.Update(e.Value);
        }

        void CountableStore_ItemAdded(object sender, ObjectEventArgs<ZCountable> e)
        {
            view.CountableCtl.Add(e.Value);
        }

        private void ActiveCountable_CountableDeleteClicked(object sender, EventArgs e)
        {
            view.CountableCtl.ContextMenuSubject.Delete();
        }

        private void ActiveCountable_CountableEditClicked(object sender, EventArgs e)
        {
            using (EditForm ef = new EditForm(view.CountableCtl.ContextMenuSubject))
            {
                ef.TopMost = view.Display.TopMost;
                ef.ShowDialog();
            }
        }

        void CurrentCountable_Replaced(object sender, ObjectEventArgs<Watcher<ZCountable>> e)
        {
            view.ActiveCountable.Enabled = true;
                view.CountableCtl.SelectedItem = e.Value.Value;
        }

        void CurrentCountable_Cleared(object sender, ObjectEventArgs<Watcher<ZCountable>> e)
        {
            view.ActiveCountable.Enabled = false;
            view.CountableCtl.SelectedItem = null;
        }

        void Countables_CountableNewClicked(object sender, EventArgs e)
        {
            ZCountable parent;
            ZLevel level;

            using (NewEntityForm nf = new NewEntityForm(state.CurrentWorkspace.Value, "Countable"))
            {
                nf.TopMost = view.Display.TopMost;
                if (view.CountableCtl.ContextMenuSubject != null
                        && !view.CountableCtl.ContextMenuSubject.Type.Final)
                {
                    nf.TypeID = view.CountableCtl.ContextMenuSubject.Type.ChildID;
                    nf.ParentItemID = view.CountableCtl.ContextMenuSubject.ID;
                }

                if (nf.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                parent = (ZCountable)nf.ParentItem;
                level = nf.Type;
            }

            ZCountable newDS = state.CurrentWorkspace.Value.CountableStore.Create(parent);
            using (EditForm ef = new EditForm(newDS))
            {
                ef.TopMost = view.Display.TopMost;
                if (ef.ShowDialog() == DialogResult.Cancel)
                {
                    newDS.Delete();
                }
            }
        }

        void CurrentWorkspace_Replaced(object sender, ObjectEventArgs<Watcher<IWorkspace>> e)
        {
            view.Countables.Enabled = true;
            view.CountableCtl.Clear();
            foreach (ZCountable c in state.CurrentWorkspace.Value.CountableStore.All)
            {
                view.CountableCtl.Add(c);
            }
            state.CurrentWorkspace.Value.CountableStore.ItemAdded += new EventHandler<ObjectEventArgs<ZCountable>>(CountableStore_ItemAdded);
            state.CurrentWorkspace.Value.CountableStore.ItemEdited += new EventHandler<ObjectEventArgs<ZCountable>>(CountableStore_ItemEdited);
            state.CurrentWorkspace.Value.CountableStore.ItemDeleted += new EventHandler<ObjectEventArgs<ZCountable>>(CountableStore_ItemDeleted);
        }

        void CurrentWorkspace_Cleared(object sender, ObjectEventArgs<Watcher<IWorkspace>> e)
        {
            view.Countables.Enabled = false;
            view.CountableCtl.Clear();
        }

        void tree_ItemDeleteClicked(object sender, EventArgs e)
        {
            view.CountableCtl.ContextMenuSubject.Delete();
        }

        void tree_ItemEditClicked(object sender, EventArgs e)
        {
            using (EditForm ef = new EditForm(view.CountableCtl.ContextMenuSubject))
            {
                ef.TopMost = view.Display.TopMost;
                ef.ShowDialog();
            }
        }
    }

    public class OptionsLogic : ALogic
    {
        public OptionsLogic() : base("OptionsLogic") { }
        protected override void Initialize()
        {
            view.Global.OptionsClicked += new EventHandler(Global_OptionsClicked);
        }

        void Global_OptionsClicked(object sender, EventArgs e)
        {
            using (OptionsForm of = new OptionsForm())
            {
                of.TopMost = view.Display.TopMost;
                of.ActiveColor = Options.ActiveColor;
                of.PastColor = Options.HistoryColor;
                of.DotSize = Options.DotSize;
                of.RoundingPlaces = Options.RoundingPlaces;
                of.InvertButtons = Options.InvertMouseButtons;

                if (of.ShowDialog() == DialogResult.OK)
                {
                    Options.ActiveColor = of.ActiveColor;
                    Options.HistoryColor = of.PastColor;
                    Options.DotSize = of.DotSize;
                    Options.RoundingPlaces = of.RoundingPlaces;
                    Options.InvertMouseButtons = of.InvertButtons;
                }
            }
        }
    }

    public class CalibrationLogic : ALogic
    {
        private CountingLogic _counting;
        public CalibrationLogic(CountingLogic counting) : base("CalibrationLogic")
        {
            _counting = counting;
        }

        protected override void Initialize()
        {
            view.Calibration.Enabled = true;
            view.Display.TopMost = true;
            view.Ready.Enabled = false;
            view.Display.Measurer.Enabled = true;
            view.Display.Measurer.Calibrating = true;

            view.Calibration.CalibratePerformed += new EventHandler<CalibrationEventArgs>(Calibration_CalibratePerformed);
            view.Ready.RecalibrateClicked += new EventHandler(Ready_RecalibrateClicked);
        }

        void Ready_RecalibrateClicked(object sender, EventArgs e)
        {
            _counting.Counting = false;
            view.Calibration.Enabled = true;
            view.Display.TopMost = true;
            view.Ready.Enabled = false;
            view.Display.Measurer.Enabled = true;
            view.Display.Measurer.Calibrating = true;
        }

        void Calibration_CalibratePerformed(object sender, CalibrationEventArgs e)
        {
            // If (measurer < 2 pts) error
            if (!view.Display.Measurer.CanCalibrate())
            {
                MessageBox.Show("Please measure the distance in the preview window.", "Warning");
                return;
            }

            //Calibrate the Measurer Control
            view.Display.Measurer.calibrate(e.Value, e.Zoom);

            //Update the View
            view.Calibration.Enabled = false;
            view.Display.TopMost = false;
            view.Ready.Enabled = true;
            view.Display.Measurer.Enabled = false;
            view.Display.Measurer.Calibrating = false;
        }
    }

    public class ExportLogic : ALogic
    {
        public ExportLogic() : base("ExportLogic") {
            InitializeComponent();
        }

        protected override void Initialize()
        {
            view.ActiveFile.FileExportClicked += new EventHandler(ActiveFile_FileExportClicked);
            view.ActiveFile.FileQueryClicked += new EventHandler(ActiveFile_FileQueryClicked);
        }

        void ActiveFile_FileQueryClicked(object sender, EventArgs e)
        {
            using (ExportForm ef = new ExportForm(state.CurrentWorkspace.Value))
            {
                ef.TopMost = view.Display.TopMost;
                ef.ShowDialog();
            }
        }

        void ActiveFile_FileExportClicked(object sender, EventArgs e)
        {
            if (saveCSV.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (File.Exists(saveCSV.FileName)) File.Delete(saveCSV.FileName);
                    using (StreamWriter sw = new StreamWriter(saveCSV.FileName))
                    {
                        DefaultReportBuilder report = new DefaultReportBuilder(state.CurrentWorkspace.Value);
                        sw.WriteLine(report.GetHeader());
                        foreach (String s in report.BuildReportQueries())
                        {
                            using (DbCommand cmd = ((WorkspaceInternals)state.CurrentWorkspace.Value).CreateCommand())
                            {
                                cmd.CommandText = s;
                                FileExporter.export(cmd.ExecuteReader(), sw);
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    using (ErrorForm ef = new ErrorForm())
                    {
                        ef.TopMost = true;
                        ef.Message = ex.Message + "\n\nException occurred in " + ex.Source + "\n\nStack Trace: " + ex.StackTrace;
                        ef.ShowDialog();
                    }
                }
            }
        }

        private System.Windows.Forms.SaveFileDialog saveCSV;

        private void InitializeComponent()
        {
            this.saveCSV = new System.Windows.Forms.SaveFileDialog();
            components.Add(saveCSV);
            // 
            // saveCSV
            // 
            this.saveCSV.DefaultExt = "csv";
            this.saveCSV.FileName = "z3data";
            this.saveCSV.Filter = "CSV Files|*.csv|All files|*.*";
            this.saveCSV.Title = "Export Measurement Data";
        }
    }

    public class ImportLogic : ALogic
    {
        public ImportLogic()
            : base("ImportLogic")
        {
            InitializeComponent();
        }

        protected override void Initialize()
        {
            view.ActiveFile.FileImportClicked += new EventHandler(ActiveFile_FileImportClicked);
        }

        void ActiveFile_FileImportClicked(object sender, EventArgs e)
        {
            if (openCSV.ShowDialog() == DialogResult.Cancel)
                return;
            if (openCSV.FileName == null || openCSV.FileName.Equals(""))
                return;
            try
            {
                SpeciesImporter importer = new SpeciesImporter((WorkspaceInternals)state.CurrentWorkspace.Value);
                importer.import(openCSV.FileName);
            }
            catch (InvalidDataException ex)
            {
                MessageBox.Show("An error occurred while importing the data.  Some of the data may have been partially imported.\n\nError Details:\n" + ex.Message, "Import Error");
            }
        }

        private System.Windows.Forms.OpenFileDialog openCSV;

        private void InitializeComponent()
        {
            this.openCSV = new System.Windows.Forms.OpenFileDialog();
            components.Add(openCSV);
            // 
            // openCSV
            // 
            this.openCSV.DefaultExt = "csv";
            this.openCSV.FileName = "species";
            this.openCSV.Filter = "CSV Files|*.csv|All files|*.*";
            this.openCSV.Title = "Import Taxonomy Data";
        }
    }

    public class ShowProgressLogic : ALogic
    {
        private int NumOfSamples = 0;

        public ShowProgressLogic()
            : base("ShowProgressLogic")
        {
            
        }

        public const string COUNT_QUERY =
@"SELECT COUNT(*) as count1, i.CountableLevel, i.Countable
FROM            Z3Individuals AS i INNER JOIN
                         Z3Measurements AS m ON m.Individual = i.internalid INNER JOIN
                         Z3MeasurementTypes AS t ON m.Measurement = t.internalid
WHERE        (t.internalid = ?) AND (i.containerlevel = ?) AND (i.container = ?)
GROUP BY i.CountableLevel, i.Countable
ORDER BY count1 desc";

        public const string SUM_QUERY =
@"SELECT        SUM(m.Value) as sum1, i.CountableLevel, i.Countable
FROM            Z3Individuals AS i INNER JOIN
                         Z3Measurements AS m ON m.Individual = i.internalid INNER JOIN
                         Z3MeasurementTypes AS t ON m.Measurement = t.internalid
WHERE        (t.internalid = ?) AND (i.containerlevel = ?) AND (i.container = ?)
GROUP BY i.CountableLevel, i.Countable
ORDER BY sum1 DESC";

        protected override void Initialize()
        {
            updateView();
            
            state.CurrentDataSet.Cleared += new EventHandler<ObjectEventArgs<Watcher<ZDataSet>>>(CurrentDataSet_Cleared);
            state.CurrentDataSet.Replaced += new EventHandler<ObjectEventArgs<Watcher<ZDataSet>>>(CurrentDataSet_Replaced);
            state.CurrentIndividual.Modified += new EventHandler<ObjectEventArgs<Watcher<ZIndividual>>>(CurrentIndividual_Modified);
        }

        void CurrentIndividual_Modified(object sender, ObjectEventArgs<Watcher<ZIndividual>> e)
        {
            updateView();
        }

        void CurrentDataSet_Replaced(object sender, ObjectEventArgs<Watcher<ZDataSet>> e)
        {
            updateView();
        }

        void CurrentDataSet_Cleared(object sender, ObjectEventArgs<Watcher<ZDataSet>> e)
        {
            view.Progress.Clear();
        }

        private void updateView()
        {
            if (!state.CurrentDataSet.Loaded) return;

            try
            {
                view.Progress.Clear();
                List<ZMeasurement> mtypes = state.CurrentWorkspace.Value.MeasurementTypes;
                List<ZDataSet> datasets = makeHeaders(mtypes);
                
                List<int[]> rows = new List<int[]>();
                Dictionary<String, List<int[]>> sampleData = new Dictionary<string, List<int[]>>();

                foreach (ZMeasurement m in mtypes)
                {
                    if (!m.Counted || m.Increment)
                    {
                        rows.AddRange(getData(COUNT_QUERY, m, state.CurrentDataSet.Value));
                    }
                    else
                    {
                        rows.AddRange(getData(SUM_QUERY, m, state.CurrentDataSet.Value));
                        
                        foreach (ZDataSet dataset in datasets) {
                            List<int[]> result = getData(SUM_QUERY, m, dataset);
                            sampleData.Add(dataset.Name,result);

                            foreach (int[] row in result)
                            {
                                for (int k = 0; k < row.Length; k++)
                                {
                                    Debug.Write(row[k] + " ");
                                }
                                Debug.WriteLine("");
                                
                            }                         
                        }
                    }
                }

                List<int[]> data = mergeData(rows, mtypes);

                foreach (int[] row in data)
                {         
                    ListViewItem current = view.Progress.Items.Add(state.CurrentWorkspace.Value.CountableStore.ByID(row[0], row[1]).Name);
                    Debug.WriteLine(row[0] + " " + row[1]);

                    foreach (KeyValuePair<string, List<int[]>> sample in sampleData)
                    {
                        Boolean hasValue = false;
                        for(int k = 0; k < sample.Value.Count; k++)
                        {
                            if(sample.Value[k][0] == row[0] && sample.Value[k][1] == row[1])
                            {
                                hasValue = true;
                                current.SubItems.Add(sample.Value[k][3].ToString());
                            }
                        }
                        if (!hasValue)
                        {
                            current.SubItems.Add("0");
                        }
                    }

                    for (int j = 2; j < row.Length; j++)
                    {
                        current.SubItems.Add(row[j].ToString());
                    }
                }

                //_form.hideError();
                
            }
            catch (SqlCeException)
            {
                //_form.showError(ex.Message);
            }
        }

        private List<int[]> mergeData(List<int[]> rows, List<ZMeasurement> cols)
        {
            List<int[]> data = new List<int[]>();
            Dictionary<int, int> index = new Dictionary<int, int>();
            
            int i=2;
            foreach (ZMeasurement m in cols) // count, length, eggcount
            {
                index.Add(m.ID, i);
                i++;
            }
            
            int rowsize = 2 + cols.Count;

            foreach (int[] row in rows) 
            {
                bool found = false;
                foreach (int[] datarow in data)
                {
                    if (datarow[1] == row[1] && datarow[0] == row[0])
                    {
                        found = true;
                        datarow[index[row[2]]] = row[3];
                    }
                }
                if (!found)
                {
                    int[] entry = new int[rowsize];
                    entry[0] = row[0]; // 2
                    entry[1] = row[1]; // 1
                    entry[index[row[2]]] = row[3]; // [2] = 6
                    data.Add(entry);
                }
            }

            return data;
        }

        private List<int[]> getData(string query, ZMeasurement mtype, ZDataSet dataset)
        {
            using (SqlCeCommand cmd = ((WorkspaceInternals)state.CurrentWorkspace.Value).CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@mtypeid_1", mtype.ID);
                cmd.Parameters.AddWithValue("@dslvlid_1", dataset/*state.CurrentDataSet.Value*/.TypeID);
                cmd.Parameters.AddWithValue("@dsid_1", dataset/*state.CurrentDataSet.Value*/.ID);

                List<int[]> retval = new List<int[]>();
                
                using (SqlCeDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        int[] datapt = new int[4];
                        datapt[0] = Convert.ToInt32(r[1]);
                        datapt[1] = Convert.ToInt32(r[2]);
                        datapt[2] = mtype.ID;
                        datapt[3] = Convert.ToInt32(r[0]);
                        retval.Add(datapt);
                    }
                }

                return retval;
            }
        }

        private int[] getSampleData()
        {
            using (SqlCeCommand cmd = ((WorkspaceInternals)state.CurrentWorkspace.Value).CreateCommand())
            {
                cmd.CommandText = "select * from Z3Individuals";

                int[] sampleCounts = new int[NumOfSamples];

                using (SqlCeDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                       // if(r['Container'])
                    }
                }

                return sampleCounts;
            }
        }

        private List<ZDataSet> makeHeaders(List<ZMeasurement> mtypes)
        {
            view.Progress.Columns.Clear();
            view.Progress.Columns.Add("Classification", 90);
            List<ZDataSet> datasets = new List<ZDataSet>();
            List<String> dsList = new List<String>();

            try { 
                foreach (ZDataSet ds in state.CurrentWorkspace.Value.DataSetStore.ChildrenOf(((ZDataSet)state.CurrentDataSet.Value.Parent)))
                {
                    datasets.Add(ds);
                    dsList.Add(ds.ToString());
                }
                dsList.Reverse();
                foreach (String sample in dsList)
                {
                    view.Progress.Columns.Add(sample, 90);
                }
            }  
            catch (InvalidOperationException)
            {
                foreach (ZDataSet ds in state.CurrentWorkspace.Value.DataSetStore.AllFromLevel(state.CurrentDataSet.Value.Type))
                {
                    datasets.Add(ds);
                    dsList.Add(ds.ToString());
                }
                dsList.Reverse();
                foreach (String sample in dsList)
                {
                    view.Progress.Columns.Add(sample, 90);
                }
            }

            foreach (ZMeasurement m in mtypes)
            {
                view.Progress.Columns.Add(m.Name, 60);
            }

            datasets.Reverse();
            return datasets;    
        }
    }
}
