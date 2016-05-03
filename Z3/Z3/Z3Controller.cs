using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Z3.Model;

namespace Z3.Leger
{
    /// <summary>
    /// The Z3 Controller.  Negotiates between the View and the Model.
    /// Encapsulates the business logic of Z3: Counting, Measuring, 
    /// handling Countable and Container levels.
    /// </summary>
    public class Z3Controller
    {
        /// <summary>
        /// The state of the controller.
        /// </summary>
        private Z3ControllerState _state;

        /// <summary>
        /// The current View object
        /// </summary>
        private List<Z3View> _views;

        /// <summary>
        /// The filename of the currently loaded workspace.
        /// </summary>
        private string _fn;

        /// <summary>
        /// Whether a workspace is actually loaded.
        /// </summary>
        //private bool _valid;

        private Z3Countable _countable;

        /// <summary>
        /// Returns true if a valid Workspace file is open.
        /// </summary>
        public bool IsValid {
            get { return State.isFileLoaded(); }
        }

        /// <summary>
        /// Creates a Z3Controller for a new, blank workspace.
        /// </summary>
        public Z3Controller()
        {
            //_valid = false;
            _views = new List<Z3View>();
            _state = new Z3ControllerState.NoFileState(this);
        }

        /// <summary>
        /// Closes the controller.
        /// </summary>
        public void Close()
        {
            while (_views.Count > 0)
            {
                Z3View v = _views[0];
                v.Close();
                _views.Remove(v);
            }
            Z3Schema.getInstance().Close();
        }

        /// <summary>
        /// Opens a workspace file.
        /// </summary>
        /// <param name="filename">The workspace or schema file to open.</param>
        public void openWorkspace(String filename) {
            if (_fn != null && filename.ToUpper().Equals(_fn.ToUpper())) return;

            _fn = filename;
            Z3Schema.open(filename);

            Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "RecentFile", filename);
            _state.loadFile();
            resetView();
        }

        /// <summary>
        /// Creates a new workspace and returns a Z3Controller associated with it.
        /// </summary>
        /// <param name="schema">The schema file to base the workspace on.</param>
        /// <param name="filename">The workspace file to create.</param>
        public void newWorkspace(string schema, string filename) {
            if (File.Exists(filename)) File.Delete(filename);
            File.Copy(schema, filename);
            Z3Schema.open(filename);
            Z3Schema.getInstance().makeWorkspace();

            _fn = filename;

            _state.loadFile();
            resetView();
        }

        private Z3DataSet _dataset;

        public Z3DataSet DataSet
        {
            get { return _dataset; }
            set
            {
                _dataset = value;
                if (_dataset == null)
                    _state.deselectContainer();
                else
                    _state.selectContainer();
            }
        }

        public Z3Countable Countable
        {
            get { return _countable; }
            set {
                _countable = value;
                if (value == null)
                    _state.deselectCountable();
                else 
                    _state.selectCountable();
            }
        }

        public Z3ControllerState State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// Reset view elements after new/open file.
        /// </summary>
        private void resetView() {
            List<Z3Measurement> m = Z3Schema.getInstance().getMeasurementTypes();
            foreach (Z3View _view in _views) {
                // Populate measurement types
                _view.setMeasurementTypes(m);
            }
        }

        public void registerView(Z3View v) {
            _views.Add(v);
            if (IsValid)
                resetView();
            _state.enforce();
        }

        public void unregisterView(Z3View v) {
            _views.Remove(v);
            if (_views.Count == 0)
                Close();
        }

        internal void cs_enableDataControls(bool p)
        {
            foreach (Z3View v in _views)
                v.cs_enableDataControls(p);
        }

        internal void cs_hotkeyReady(bool p)
        {
            foreach (Z3View v in _views)
                v.cs_hotkeyReady(p);
        }

        internal void cs_ready(bool p)
        {
            foreach (Z3View v in _views)
                v.cs_ready(p);
        }
    }
}
