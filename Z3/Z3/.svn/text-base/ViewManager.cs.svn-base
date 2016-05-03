using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using Z3.View.Floating;
using Z3.Leger;
using Z3.Model;

namespace Z3.View
{
    //public class ViewManager //: ZView
    //{
        //private ZController _z;
        
        //public ZController Controller
        //{
        //    get { return _z; }
        //    set {
        //        if (_z != value) {
        //            if (_z != null)
        //                _z.unregisterView(this);
        //            _z = value;
        //            if (_z != null)
        //                _z.registerView(this);
        //        }
        //    }
        //}
        
        /*private bool _cs_ready;
        private bool _cs_hotkey_ready;
        private bool _vs_ready;

        private bool hidden;
        public bool Inactive { get { return hidden; } }

        private Dictionary<char, ZCountable> _hk_cache = new Dictionary<char,ZCountable>();
        public Dictionary<char, ZCountable> Hotkeys { 
            get {
                if (_hk_cache.Count == 0) 
                    _hk_cache = getAllHotkeys();
                return _hk_cache; 
            } 
        }

        public bool Ready {
            get { return (_cs_ready || _cs_hotkey_ready) && _vs_ready; }
        }

        private BrowserForm _ContainerForm;

        public BrowserForm ContainerForm {
            get { return _ContainerForm; }
            set { _ContainerForm = value; }
        }


        private EntityBrowser _ContainerMgr;

        public EntityBrowser ContainerMgr {
            get { return _ContainerMgr; }
            set { _ContainerMgr = value; }
        }

        private EntityBrowser _CountableMgr;

        internal EntityBrowser CountableMgr
        {
            get { return _CountableMgr; }
            set {
                _CountableMgr = value;
                _CountableMgr.EntitySelected += new EventHandler(_CountableMgr_EntitySelected);
            }
        }
        


        public Dictionary<char, ZCountable> getAllHotkeys()
        {
            Dictionary<char, ZCountable> hk = new Dictionary<char,ZCountable>();
            //ZCountable zc = (ZCountable)new ZEntity("Countable");
            ZCountable zc = ZCountable.FactoryInstance;
            List<ZEntity> lz = zc.all();
            //TreeWrapper t = _CountableMgr.Items.
            foreach (ZEntity ze in lz) //_ctlMgr.SpeciesTree.Nodes)
            {
                if( ze.GetType().Name != "ZCountable" )
                    continue;
                ZCountable z = (ZCountable)ze;
                char key = Char.ToUpper(z.Hotkey);
                //ZCountable z = (ZCountable)n.Tag;
                if(z.Hotkey != '\0' && !hk.ContainsKey(key))
                    hk.Add(key, z);
            }
            return hk;
        }

        //void _CountableMgr_EntitySelected(object sender, EventArgs e) {
        //    Controller.State.isCountableSelected();
        //    this._ctlMgr.SpeciesTree.HideSelection = false;
        //}
        private IVideoManager _videoMgr;

        internal IVideoManager VideoMgr
        {
            get { return _videoMgr; }
            set { _videoMgr = value; }
        }*/
        //private DataSetManager _dataSetsMgr;

        //internal DataSetManager DataSetsMgr
        //{
        //    get { return _dataSetsMgr; }
        //    set { _dataSetsMgr = value; }
        //}
        
        //private MenuManager _menuMgr;

        //internal MenuManager MenuMgr
        //{
        //    get { return _menuMgr; }
        //    set { _menuMgr = value; }
        //}
        //private ControlsManager _ctlMgr;
        //
        //internal ControlsManager CtlMgr
        //{
        //    get { return _ctlMgr; }
        //    set { _ctlMgr = value; }
        //}
 //       private ZViewState _state;

 //       public ZViewState State
 //       {
//            get { return _state; }
//            set { _state = value; }
//        }
//        private WindowManager _windowMgr;

//        internal WindowManager WindowMgr
//        {
//            get { return _windowMgr; }
//            set { _windowMgr = value; }
//        }
//        private DataManager _dataPointsMgr;
//
//        internal DataManager DataPointsMgr
//        {
//            get { return _dataPointsMgr; }
//            set { _dataPointsMgr = value; }
//        }

//        public void cacheHotkeys() {
//            ZCountable zc = ZCountable.FactoryInstance;

        //}

        //public ViewManager(ZController c)
        //{
        //    _z = c;
        //    _state = new ZViewState.NoVideoState(this);
        //    hidden = false;
        //}

        //public void newCountable()
        //{
         //   CountableMgr.Create();
        //}

        //public void editCountable()
        //{
        //    CountableMgr.Edit();
        //}

        //public void deleteCountable()
        //{
         //   if (CountableMgr == null || CountableMgr.SelectedItem == null) return;
         //   CountableMgr.Delete();
         //   if (CountableMgr.SelectedItem.GetType().Name.Equals("ZCountable"))
         //   {
         //       ZCountable z = (ZCountable)CountableMgr.SelectedItem;
         //       char key = Char.ToUpper(z.Hotkey);
        //        _hk_cache.Remove(key);
        //    }
        //}

        #region Z3View Members

        //void ZView.setMeasurementTypes(List<Z3.Model.ZMeasurement> types)
        //{
        //    CtlMgr.SetMeasurementTypes(types);
        //}

        //void ZView.setState(ZViewState z)
        //{
        //    State = z;
        //}

        //void ZView.vs_enableVideoControls(bool p)
        //{
        //    CtlMgr.EnableVideo(p);
        //    VideoMgr.EnableVideo(p);
        //}

        //void ZView.vs_enableVideoCalibControls(bool p)
        //{
        //    MenuMgr.EnableVideoCalib(p);
        //    CtlMgr.EnableVideoCalib(p);
        //    VideoMgr.EnableVideoCalib(p);
        //    CtlMgr.EnableControls(Ready);
        //    MenuMgr.EnableControls(Ready);
        //}
        /*
        void ZView.vs_ready(bool p)
        {
            _vs_ready = p;
            CtlMgr.EnableControls(Ready);
            MenuMgr.EnableControls(Ready);
        }

        void ZView.cs_enableDataControls(bool p)
        {
            MenuMgr.EnableData(p);
            CtlMgr.EnableData(p);
            DataPointsMgr.EnableData(p);
            DataSetsMgr.EnableData(p);
        }

        void ZView.cs_hotkeyReady(bool p)
        {
            _cs_hotkey_ready = p;
            CtlMgr.EnableControls(Ready);
            MenuMgr.EnableControls(Ready);
        }

        void ZView.cs_ready(bool p)
        {
            _cs_ready = p;
            CtlMgr.EnableControls(Ready);
            MenuMgr.EnableControls(Ready);
        }

        void ZView.Close()
        {
            WindowMgr.Close();
        }*/

        #endregion

        //public void WindowClosing() {
        //    VideoMgr.Dispose();
        //}

        //public void FormLoading() {
        //    VideoMgr.Loading();
        //    State.enforce();
        //}

        //public static bool isFloatingForm(Form x)
        //{
            //Z3FloatingVideoForm x = (Z3FloatingVideoForm)y;
        //    return x.Name.Equals("Z3FloatingVideoForm");
        //}

        //internal void ShowError(string p) {
            //this.VideoMgr.EnableCounting(false);
            //this.MenuMgr.EnableCounting(false);
            //this.CtlMgr.EnableControls(false);
        //    this.CtlMgr.Counting = false;

        //    System.Windows.Forms.MessageBox.Show(this.WindowMgr.Windows[0], p);
        //}

        //void ZView.setFileName(string fn) {
        //    FileNameChanged(this, new StringEventArgs(fn));
        //}

        //public event EventHandler<StringEventArgs> FileNameChanged;
        //public class StringEventArgs : EventArgs {
        //    public StringEventArgs(string s) {
        //        value = s;
        //    }
        //    public string value;
        //}

        //public void ShowFileName()
        //{
        //    MenuMgr.ShowFileName();
        //}

        //public void FocusWindow(String win)
        //{
        //    foreach( Form f in WindowMgr.Windows )
        //        if (f.Name.Equals(win))
        //        {
        //            f.Activate();
        //            f.Select();
        //            f.Focus();
        //        }
        //}

        /*public void toggleVisibility()
        {
            foreach( Form f in WindowMgr.Windows ) 
                _toggleVisibility(f);
            hidden = !hidden;
        }*/

        /*private void _toggleVisibility(Form f)
        {
            if (hidden)
                f.WindowState = FormWindowState.Normal;
            else
                f.WindowState = FormWindowState.Minimized;
        }*/
    //}
}
