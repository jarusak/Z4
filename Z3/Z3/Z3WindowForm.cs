using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.Leger;
using Z3.Model;

namespace Z3.View.Standard { /*
    public partial class Z3WindowForm : Form //: UIControllerForm 
    {
        #region Data Members
        //private FormWindowState lastWindowState;
        //private ViewManager _brain;
        //private BrowserForm _browser;
        #endregion

        #region Constructors
        private IContainer _components = new System.ComponentModel.Container();
        public Z3WindowForm(ZController z) {
            InitializeComponent();
            //InitializeComponent2();
            //_brain = new ViewManager(z);
            //_brain.WindowMgr = new WindowManager(_brain);
            //_brain.WindowMgr.Windows.Add(this);
            //_brain.CtlMgr = new ControlsManager(_brain, infoSplit.Panel2);
            //_brain.MenuMgr = new MenuManager(_brain, this, this.toolStripContainer1.TopToolStripPanel);
            //_brain.MenuMgr.VideoMenu.DropDownItems.Add(overlaymode);
            //_brain.VideoMgr = new VideoManager(_brain, measurerSplit.Panel2, _brain.MenuMgr.VideoMenu, this, _components);
            //_brain.DataPointsMgr = new DataManager(_brain, dataSplit.Panel2);
            //_brain.ContainerForm = _browser = new BrowserForm("Container", _components);
            //_brain.ContainerMgr = _browser.Manager;
            //_brain.DataSetsMgr = new DataSetManager(_brain, infoSplit.Panel1, _components);
            //_brain.CountableMgr = new EntityBrowser(_brain.CtlMgr.SpeciesTree, "Countable", _components);
            //_brain.State = new ZViewState.NoVideoState(_brain);
            //_brain.FileNameChanged += new EventHandler<ViewManager.StringEventArgs>(_brain_FileNameChanged);
            //_brain.Controller.registerView(_brain);
            //_brain.State.enforce();
            //if (ZSchema.getInstance() != null)
            //    this.Text = "Z3 - [" + ZSchema.getInstance().ShortFileName + "]";
        }

        //void _brain_FileNameChanged(object sender, ViewManager.StringEventArgs e) {
            //this.Text = "Z3 [" + e.value + "]";
            //_brain.ContainerMgr.Delete();
            //_brain.ContainerForm.Dispose();
            //_brain.CtlMgr = new ControlsManager(_brain, infoSplit.Panel2);
            //_brain.CountableMgr = new EntityBrowser(_brain.CtlMgr.SpeciesTree, "Countable", components);

        //}
        #endregion
        #region Form Events

        #region private controls
        //private ToolStripMenuItem overlaymode;
        //private void InitializeComponent2() {
        //    overlaymode = new ToolStripMenuItem();
        //    this.overlaymode.Name = "menuToolsOverlay";
        //    this.overlaymode.Size = new System.Drawing.Size(173, 22);
        //    this.overlaymode.Text = "Switch to Overlay Mode";
        //    this.overlaymode.Click += new EventHandler(overlaymode_Click);
        //}

        //void overlaymode_Click(object sender, EventArgs e) {
        //    changeToMode(VideoMode.OVERLAY, _brain.Controller);
        //}

        #endregion

        //private void Z3WindowForm_Resize(object sender, EventArgs e) {
        //    if (_brain != null && _brain.VideoMgr != null)
        //    if (this.WindowState != FormWindowState.Minimized) {
        //        _brain.VideoMgr.Resize();
        //        lastWindowState = this.WindowState;
        //    }
        //}

        //private void Z3WindowForm_ResizeEnd(object sender, EventArgs e) {
        //    if (this.WindowState != FormWindowState.Minimized) {
        //        _brain.VideoMgr.Resize();
        //        lastWindowState = this.WindowState;
        //    }
        //}

        //private void Z3WindowForm_FormClosing(object sender, FormClosingEventArgs e) {
        //    _brain.WindowClosing();
        //}
        
        //private void Z3WindowForm_Load(object sender, EventArgs e) {
        //    _brain.FormLoading();
        //}
        #endregion

        //private void Z3WindowForm_FormClosed(object sender, FormClosedEventArgs e) {
        //}
    }*/
}