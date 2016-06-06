using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Z3.View;
using System.IO;
using Z3.Model;
using System.Diagnostics;

namespace Z3.View.Impl
{
    public class MenuElementsImpl : FileElements, ActiveFileElements, 
        DataSetElements, ActiveDataSetElements, ActiveDataPointElements, 
        CountableElements, ActiveCountableElements, ReadyControlElements, 
        GlobalControlElements, CalibControlElements
    {
        private Form _form;

        public MenuElementsImpl() {
            InitializeComponent();
        }
        
        public void Bind(Form f, ContainerControl cont)
        {
            _form = f;
            cont.Controls.Add(menu);
            _form.MainMenuStrip = menu;
        }
        
        /*internal void EnableData(bool p)
        {
            menuDataCountableDelete.Enabled = p;
            menuDataCountableEdit.Enabled = p;
            menuDataCountableManage.Enabled = p;
            menuDataCountableNew.Enabled = p;
            menuDataPointDelete.Enabled = p;
            menuDataPointEdit.Enabled = p;
            menuDataSetNew.Enabled = p;
            menuDataSetProperties.Enabled = p;
            menuDataSetSelect.Enabled = p;
            menuFileImport.Enabled = p;
            menuFileExport.Enabled = p;
            menuFileProperties.Enabled = p;
        }*/

        //internal void EnableVideoCalib(bool p)
        //{
        //    menuToolsRecalibrate.Enabled = p;
        //    menuToolsZoom.Enabled = p;
       // }

        //internal void EnableControls(bool p)
        //{
        //    menuToolsCounting.Enabled = p;
        //    menuToolsUndo.Enabled = p;
        //}

        //public ToolStripMenuItem VideoMenu
        //{
        //    get { return menuVideo; }
        //}

        #region Menu Bar
        #region File

        //NEW
        private void menuFileNew_Click(object sender, EventArgs e)
        {
            if (FileNewClicked != null) 
                FileNewClicked(sender, e);
            //String schemaFile, wfFile;
           /* DialogResult r;
            if (_brain.Controller == null || !_brain.Controller.IsValid)
                r = DialogResult.No;
            else
                r = MessageBox.Show("Would you like to base the new workspace on the current Schema, " + ZSchema.getInstance().getName() + "?", "Keep current schema?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (r == DialogResult.Cancel)
                return;
            if (r == DialogResult.Yes)
                schemaFile = ZSchema.getInstance().getFileName();
            else
            */
            //if (openSchema.ShowDialog() == DialogResult.Cancel)
            //    return;
            //else
            //    schemaFile = openSchema.FileName;

            //if (schemaFile == null || schemaFile.Equals(""))
            //    return;

            //if (newWorkspace.ShowDialog() == DialogResult.Cancel)
            //    return;
            //else
            //    wfFile = newWorkspace.FileName;
            //if (wfFile == null || wfFile.Equals(""))
            //    return;

            //_brain.Controller.newWorkspace(schemaFile, wfFile);
        }

        //OPEN
        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            if (FileOpenClicked != null)
                FileOpenClicked(sender, e);
            /*
            if (openWorkspace.ShowDialog() == DialogResult.Cancel)
                return;
            if (openWorkspace.FileName == null || openWorkspace.FileName.Equals(""))
                return;
            _brain.Controller.openWorkspace(openWorkspace.FileName);
        */}

        //IMPORT
        private void menuFileImport_Click(object sender, EventArgs e)
        {
            if (FileImportClicked != null)
                FileImportClicked(sender, e);
        }

        //EXPORT
        private void menuFileExport_Click(object sender, EventArgs e)
        {
            if (FileExportClicked != null)
                FileExportClicked(sender, e);
        }

        //PROPERTIES
        private void menuFileProperties_Click(object sender, EventArgs e)
        {
            if (FilePropertiesClicked != null)
                FilePropertiesClicked(sender, e);
        }

        //EXIT
        private void menuFileExit_Click(object sender, EventArgs e)
        {
            if (FileExitClicked != null)
                FileExitClicked(sender, e);
            //_brain.Controller.Close();
        }

        #endregion
        #region Data
        private void menuDataSetNew_Click(object sender, EventArgs e)
        {
            if (DataSetNewClicked != null)
                DataSetNewClicked(sender, e);
        }

        private void menuDataSetSelect_Click(object sender, EventArgs e)
        {
            if (DataSetOpenClicked != null) 
                DataSetOpenClicked(sender, e);
//            if (_brain.ContainerForm != null && !_brain.ContainerForm.IsDisposed)
//            {
//                _brain.ContainerForm = new BrowserForm("Container", _brain.DataSetsMgr.Components);
//                _brain.ContainerMgr = _brain.ContainerForm.Manager;
//                _brain.ContainerMgr.EntitySelected += new EventHandler(_brain.DataSetsMgr.Manager_EntitySelected);
//
//                _brain.ContainerForm.TopLevel = true;
//                _brain.ContainerForm.Show();
//                _brain.ContainerForm.Focus();
//                _brain.ContainerForm.Select();
//            } else if (_brain.ContainerForm != null) {
//                _brain.ContainerForm.Focus();
//                _brain.ContainerForm.Select();
//            }
        }

        private void menuDataSetProperties_Click(object sender, EventArgs e)
        {
            if (DataSetPropertiesClicked != null) 
                DataSetPropertiesClicked(sender, e);
        }

        private void menuDataPointEdit_Click(object sender, EventArgs e)
        {
            if (PointEditClicked != null)
                PointEditClicked(sender, e);
        }

        private void menuDataPointDelete_Click(object sender, EventArgs e)
        {
            if (PointDeleteClicked != null)
                PointDeleteClicked(sender, e);
            //_brain.DataPointsMgr.RemoveSelected();
        }

        private void menuDataCountableNew_Click(object sender, EventArgs e)
        {
            if (CountableNewClicked != null)
                CountableNewClicked(sender, e);
            //_brain.newCountable();
        }

        private void menuDataCountableEdit_Click(object sender, EventArgs e)
        {
            if (CountableEditClicked != null)
                CountableEditClicked(sender, e);
            //_brain.editCountable();
        }

        private void menuDataCountableDelete_Click(object sender, EventArgs e)
        {
            if (CountableDeleteClicked != null)
                CountableDeleteClicked(sender, e);
            //_brain.deleteCountable();
        }

        #endregion
        #region Tools
        private void menuToolsCounting_Click(object sender, EventArgs e)
        {
            if (StartStopCountingClicked != null)
                StartStopCountingClicked(sender, e);
        }

        private void menuToolsUndo_Click(object sender, EventArgs e)
        {
            if (UndoClicked != null)
                UndoClicked(sender, e);
        }

        private void menuToolsRecalibrate_Click(object sender, EventArgs e)
        {
            if (RecalibrateClicked != null)
                RecalibrateClicked(sender, e);
        }

        private void menuVideoRecalibrate_Click(object sender, EventArgs e)
        {
            if (RecalibrateClicked != null)
                RecalibrateClicked(sender, e);
        }

        private void menuToolsZoom_Click(object sender, EventArgs e)
        {
            if (ZoomClicked != null)
                ZoomClicked(sender, e);
        }

        private void menuToolsSchema_Click(object sender, EventArgs e)
        {
            if (SchemaEditorClicked != null)
                SchemaEditorClicked(sender, e);
        }

        private void menuToolsOptions_Click(object sender, EventArgs e)
        {
            if (OptionsClicked != null)
                OptionsClicked(sender, e);
        }

        #endregion

        #region Help

        private void menuHelpContents_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://lter.limnology.wisc.edu/software/Z3:Help");
        }

        private void menuHelpSoftwareWiki_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://lter.limnology.wisc.edu/software/");
        }

        private void menuHelpAbout_Click(object sender, EventArgs e)
        {
            using (Z3.Forms.AboutBox b = new Z3.Forms.AboutBox())
            {
                b.TopMost = _form.TopMost;
                b.ShowDialog();
            }
        }
        #endregion
        #endregion

        #region Controls
        private void InitializeComponent()
        {
            this.menu = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSep0 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileImport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileExport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileExport2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuData = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDataSetNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDataSetSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDataSetDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDataSetProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDataSep0 = new System.Windows.Forms.ToolStripSeparator();
            this.menuDataPointEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDataPointDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDataSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuDataCountableNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDataCountableEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDataCountableDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.menuToolsCounting = new System.Windows.Forms.ToolStripMenuItem();
            this.menuToolsUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuToolsSep0 = new System.Windows.Forms.ToolStripSeparator();
            this.menuToolsRecalibrate = new System.Windows.Forms.ToolStripMenuItem();
            this.menuToolsZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.menuToolsSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuToolsSchema = new System.Windows.Forms.ToolStripMenuItem();
            this.menuToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVideo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpContents = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpSep0 = new System.Windows.Forms.ToolStripSeparator();
            this.menuHelpSoftwareWiki = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menu.SuspendLayout();
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuData,
            this.menuTools,
            this.menuVideo,
            this.menuHelp});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(560, 24);
            this.menu.TabIndex = 1;
            this.menu.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileNew,
            this.menuFileOpen,
            this.menuFileSep0,
            this.menuFileImport,
            this.menuFileExport,
            this.menuFileExport2,
            //this.menuFileSep1,
            this.menuFileProperties,
            this.menuFileSep2,
            this.menuFileExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "File";
            // 
            // menuFileNew
            // 
            this.menuFileNew.Name = "menuFileNew";
            this.menuFileNew.Size = new System.Drawing.Size(173, 22);
            this.menuFileNew.Text = "&New Workspace...";
            this.menuFileNew.Click += new EventHandler(menuFileNew_Click);
            // 
            // menuFileOpen
            // 
            this.menuFileOpen.Name = "menuFileOpen";
            this.menuFileOpen.Size = new System.Drawing.Size(173, 22);
            this.menuFileOpen.Text = "&Open Workspace...";
            this.menuFileOpen.Click += new EventHandler(menuFileOpen_Click);
            // 
            // menuFileSep0
            // 
            this.menuFileSep0.Name = "menuFileSep0";
            this.menuFileSep0.Size = new System.Drawing.Size(170, 6);
            // 
            // menuFileImport
            // 
            this.menuFileImport.Name = "menuFileImport";
            this.menuFileImport.Size = new System.Drawing.Size(173, 22);
            this.menuFileImport.Text = "&Import...";
            this.menuFileImport.Click += new EventHandler(menuFileImport_Click);
            this.menuFileImport.Visible = true;
            // 
            // menuFileExport
            // 
            this.menuFileExport.Name = "menuFileExport";
            this.menuFileExport.Size = new System.Drawing.Size(173, 22);
            this.menuFileExport.Text = "&Export Measurements...";
            this.menuFileExport.Click += new EventHandler(menuFileExport_Click);
            // 
            // menuFileExport2
            // 
            this.menuFileExport2.Name = "menuFileExport2";
            this.menuFileExport2.Size = new System.Drawing.Size(173, 22);
            this.menuFileExport2.Text = "&Query and Export...";
            this.menuFileExport2.Click += new EventHandler(menuFileExport2_Click);
            // 
            // menuFileSep1
            // 
            this.menuFileSep1.Name = "menuFileSep1";
            this.menuFileSep1.Size = new System.Drawing.Size(170, 6);
            // 
            // menuFileProperties
            // 
            this.menuFileProperties.Name = "menuFileProperties";
            this.menuFileProperties.Size = new System.Drawing.Size(173, 22);
            this.menuFileProperties.Text = "&Properties...";
            this.menuFileProperties.Click += new EventHandler(menuFileProperties_Click);
            this.menuFileProperties.Visible = false;
            // 
            // menuFileSep2
            // 
            this.menuFileSep2.Name = "menuFileSep2";
            this.menuFileSep2.Size = new System.Drawing.Size(170, 6);
            // 
            // menuFileExit
            // 
            this.menuFileExit.Name = "menuFileExit";
            this.menuFileExit.Size = new System.Drawing.Size(173, 22);
            this.menuFileExit.Text = "E&xit";
            this.menuFileExit.Click += new EventHandler(menuFileExit_Click);
            // 
            // menuData
            // 
            this.menuData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuDataSetNew,
            this.menuDataSetSelect,
            this.menuDataSetProperties,
            this.menuDataSetDelete,
            this.menuDataSep0,
            this.menuDataPointEdit,
            this.menuDataPointDelete,
            this.menuDataSep1,
            this.menuDataCountableNew,
            this.menuDataCountableEdit,
            this.menuDataCountableDelete});
            this.menuData.Name = "menuData";
            this.menuData.Size = new System.Drawing.Size(43, 20);
            this.menuData.Text = "Data";
            // 
            // menuDataSetNew
            // 
            this.menuDataSetNew.Name = "menuDataSetNew";
            this.menuDataSetNew.Size = new System.Drawing.Size(189, 22);
            this.menuDataSetNew.Text = "New Data Set...";
            this.menuDataSetNew.Click += new EventHandler(menuDataSetSelect_Click);
            // 
            // menuDataSetSelect
            // 
            this.menuDataSetSelect.Name = "menuDataSetSelect";
            this.menuDataSetSelect.Size = new System.Drawing.Size(189, 22);
            this.menuDataSetSelect.Text = "Select Data Set...";
            this.menuDataSetSelect.Click += new EventHandler(menuDataSetSelect_Click);
            // 
            // menuDataSetProperties
            // 
            this.menuDataSetProperties.Name = "menuDataSetProperties";
            this.menuDataSetProperties.Size = new System.Drawing.Size(189, 22);
            this.menuDataSetProperties.Text = "Data Set Properties...";
            this.menuDataSetProperties.Click += new EventHandler(menuDataSetProperties_Click);
            // 
            // menuDataSetDelete
            // 
            this.menuDataSetDelete.Name = "menuDataSetDelete";
            this.menuDataSetDelete.Size = new System.Drawing.Size(189, 22);
            this.menuDataSetDelete.Text = "Delete Data Set";
            this.menuDataSetDelete.Click += new EventHandler(menuDataSetDelete_Click);
            this.menuDataSetDelete.Visible = false;
            // 
            // menuDataSep0
            // 
            this.menuDataSep0.Name = "menuDataSep0";
            this.menuDataSep0.Size = new System.Drawing.Size(186, 6);
            // 
            // menuDataPointEdit
            // 
            this.menuDataPointEdit.Name = "menuDataPointEdit";
            this.menuDataPointEdit.Size = new System.Drawing.Size(189, 22);
            this.menuDataPointEdit.Text = "Edit Data Point...";
            this.menuDataPointEdit.Click += new EventHandler(menuDataPointEdit_Click);
            // 
            // menuDataPointDelete
            // 
            this.menuDataPointDelete.Name = "menuDataPointDelete";
            this.menuDataPointDelete.Size = new System.Drawing.Size(189, 22);
            this.menuDataPointDelete.Text = "Delete Data Point";
            this.menuDataPointDelete.Click += new EventHandler(menuDataPointDelete_Click);
            // 
            // menuDataSep1
            // 
            this.menuDataSep1.Name = "menuDataSep1";
            this.menuDataSep1.Size = new System.Drawing.Size(186, 6);
            // 
            // menuDataCountableNew
            // 
            this.menuDataCountableNew.Name = "menuDataCountableNew";
            this.menuDataCountableNew.Size = new System.Drawing.Size(189, 22);
            this.menuDataCountableNew.Text = "New Countable...";
            this.menuDataCountableNew.Click += new EventHandler(menuDataCountableNew_Click);
            // 
            // menuDataCountableEdit
            // 
            this.menuDataCountableEdit.Name = "menuDataCountableEdit";
            this.menuDataCountableEdit.Size = new System.Drawing.Size(189, 22);
            this.menuDataCountableEdit.Text = "Edit Countable...";
            this.menuDataCountableEdit.Click += new EventHandler(menuDataCountableEdit_Click);
            // 
            // menuDataCountableDelete
            // 
            this.menuDataCountableDelete.Name = "menuDataCountableDelete";
            this.menuDataCountableDelete.Size = new System.Drawing.Size(189, 22);
            this.menuDataCountableDelete.Text = "Delete Countable...";
            this.menuDataCountableDelete.Click += new EventHandler(menuDataCountableDelete_Click);
            // 
            // menuTools
            // 
            this.menuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolsCounting,
            this.menuToolsUndo,
            this.menuToolsSep0,
            this.menuToolsRecalibrate,
            this.menuToolsZoom,
            this.menuToolsSep1,
            this.menuToolsSchema,
            this.menuToolsOptions});
            this.menuTools.Name = "menuTools";
            this.menuTools.Size = new System.Drawing.Size(48, 20);
            this.menuTools.Text = "Tools";
            // 
            // menuToolsCounting
            // 
            this.menuToolsCounting.Name = "menuToolsCounting";
            this.menuToolsCounting.Size = new System.Drawing.Size(163, 22);
            this.menuToolsCounting.Text = "Counting";
            this.menuToolsCounting.Click += new EventHandler(menuToolsCounting_Click);
            // 
            // menuToolsUndo
            // 
            this.menuToolsUndo.Name = "menuToolsUndo";
            this.menuToolsUndo.Size = new System.Drawing.Size(163, 22);
            this.menuToolsUndo.Text = "Undo Last Count";
            this.menuToolsUndo.Click += new EventHandler(menuToolsUndo_Click);
            this.menuToolsUndo.Visible = false;
            // 
            // menuToolsSep0
            // 
            this.menuToolsSep0.Name = "menuToolsSep0";
            this.menuToolsSep0.Size = new System.Drawing.Size(160, 6);
            // 
            // menuToolsRecalibrate
            // 
            this.menuToolsRecalibrate.Name = "menuToolsRecalibrate";
            this.menuToolsRecalibrate.Size = new System.Drawing.Size(163, 22);
            this.menuToolsRecalibrate.Text = "Recalibrate";
            this.menuToolsRecalibrate.Click += new EventHandler(menuToolsRecalibrate_Click);
            // 
            // menuToolsZoom
            // 
            this.menuToolsZoom.Name = "menuToolsZoom";
            this.menuToolsZoom.Size = new System.Drawing.Size(163, 22);
            this.menuToolsZoom.Text = "Zoom...";
            this.menuToolsZoom.Click += new EventHandler(menuToolsZoom_Click);
            this.menuToolsZoom.Visible = false;
            // 
            // menuToolsSep1
            // 
            this.menuToolsSep1.Name = "menuToolsSep1";
            this.menuToolsSep1.Size = new System.Drawing.Size(160, 6);
            // 
            // menuToolsSchema
            // 
            this.menuToolsSchema.Name = "menuToolsSchema";
            this.menuToolsSchema.Size = new System.Drawing.Size(163, 22);
            this.menuToolsSchema.Text = "Schema Editor";
            this.menuToolsSchema.Click += new EventHandler(menuToolsSchema_Click);
            this.menuToolsSchema.Visible = false;
            // 
            // menuToolsOptions
            // 
            this.menuToolsOptions.Name = "menuToolsOptions";
            this.menuToolsOptions.Size = new System.Drawing.Size(163, 22);
            this.menuToolsOptions.Text = "Options...";
            this.menuToolsOptions.Click += new EventHandler(menuToolsOptions_Click);
            // 
            // menuVideo
            // 
            this.menuVideo.Name = "menuVideo";
            this.menuVideo.Size = new System.Drawing.Size(49, 20);
            this.menuVideo.Text = "View";
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpContents,
            this.menuHelpSep0,
            this.menuHelpSoftwareWiki,
            this.menuHelpSep1,
            this.menuHelpAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 20);
            this.menuHelp.Text = "Help";
            // 
            // menuHelpContents
            // 
            this.menuHelpContents.Name = "menuHelpContents";
            this.menuHelpContents.Size = new System.Drawing.Size(200, 22);
            this.menuHelpContents.Text = "Contents";
            this.menuHelpContents.Click +=new EventHandler(menuHelpContents_Click);
            // 
            // menuHelpSep0
            // 
            this.menuHelpSep0.Name = "menuHelpSep0";
            this.menuHelpSep0.Size = new System.Drawing.Size(197, 6);
            // 
            // menuHelpSoftwareWiki
            // 
            this.menuHelpSoftwareWiki.Name = "menuHelpSoftwareWiki";
            this.menuHelpSoftwareWiki.Size = new System.Drawing.Size(200, 22);
            this.menuHelpSoftwareWiki.Text = "NTL LTER Software Website";
            this.menuHelpSoftwareWiki.Click += new EventHandler(menuHelpSoftwareWiki_Click);
            // 
            // menuHelpSep1
            // 
            this.menuHelpSep1.Name = "menuHelpSep1";
            this.menuHelpSep1.Size = new System.Drawing.Size(197, 6);
            // 
            // menuHelpAbout
            // 
            this.menuHelpAbout.Name = "menuHelpAbout";
            this.menuHelpAbout.Size = new System.Drawing.Size(200, 22);
            this.menuHelpAbout.Text = "About Z3...";
            this.menuHelpAbout.Click += new EventHandler(menuHelpAbout_Click);

            //this.menu.Resize += new EventHandler(sendToTray);

            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
        }

        void menuFileExport2_Click(object sender, EventArgs e)
        {
            if (FileQueryClicked != null)
                FileQueryClicked(sender, e);
        }

        void menuDataSetDelete_Click(object sender, EventArgs e)
        {
            if (DataSetDeleteClicked != null)
                DataSetDeleteClicked(sender, e);
        }

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuFileNew;
        private System.Windows.Forms.ToolStripMenuItem menuFileOpen;
        private System.Windows.Forms.ToolStripSeparator menuFileSep0;
        private System.Windows.Forms.ToolStripMenuItem menuFileImport;
        private System.Windows.Forms.ToolStripMenuItem menuFileExport;
        private System.Windows.Forms.ToolStripMenuItem menuFileExport2;
        private System.Windows.Forms.ToolStripSeparator menuFileSep1;
        private System.Windows.Forms.ToolStripMenuItem menuFileProperties;
        private System.Windows.Forms.ToolStripSeparator menuFileSep2;
        private System.Windows.Forms.ToolStripMenuItem menuFileExit;
        private System.Windows.Forms.ToolStripMenuItem menuData;
        private System.Windows.Forms.ToolStripMenuItem menuDataSetProperties;
        private System.Windows.Forms.ToolStripMenuItem menuDataSetNew;
        private System.Windows.Forms.ToolStripMenuItem menuDataSetSelect;
        private System.Windows.Forms.ToolStripMenuItem menuDataSetDelete;
        private System.Windows.Forms.ToolStripSeparator menuDataSep0;
        private System.Windows.Forms.ToolStripMenuItem menuDataPointEdit;
        private System.Windows.Forms.ToolStripMenuItem menuDataPointDelete;
        private System.Windows.Forms.ToolStripMenuItem menuTools;
        private System.Windows.Forms.ToolStripMenuItem menuToolsCounting;
        private System.Windows.Forms.ToolStripMenuItem menuToolsUndo;
        private System.Windows.Forms.ToolStripMenuItem menuToolsRecalibrate;
        private System.Windows.Forms.ToolStripMenuItem menuToolsZoom;
        private System.Windows.Forms.ToolStripSeparator menuToolsSep0;
        private System.Windows.Forms.ToolStripSeparator menuToolsSep1;
        private System.Windows.Forms.ToolStripMenuItem menuToolsOptions;
        private System.Windows.Forms.ToolStripMenuItem menuVideo;
        private System.Windows.Forms.ToolStripMenuItem menuToolsSchema;
        private System.Windows.Forms.ToolStripSeparator menuDataSep1;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem menuDataCountableNew;
        private System.Windows.Forms.ToolStripMenuItem menuDataCountableEdit;
        private System.Windows.Forms.ToolStripMenuItem menuDataCountableDelete;
        private System.Windows.Forms.ToolStripMenuItem menuHelpContents;
        private System.Windows.Forms.ToolStripSeparator menuHelpSep0;
        private System.Windows.Forms.ToolStripMenuItem menuHelpSoftwareWiki;
        private System.Windows.Forms.ToolStripSeparator menuHelpSep1;
        

        #endregion



        //internal void EnableCounting(bool _counting) {
        //    menuToolsCounting.Checked = _counting;
        //}

        //internal void ShowFileName()
        //{
        //    _form.Text = "Z3 - [" + ZSchema.getInstance().ShortFileName + "]";
        //}

        #region FileElements Members

        public event EventHandler FileNewClicked;
        public event EventHandler FileOpenClicked;
        public event EventHandler FileExitClicked;

        #endregion

        #region ActiveFileElements Members

        string ActiveFileElements.Name
        {
            set
            {
                _form.Text = "Z3 - [" + value + "]";
            }
        }

        bool ActiveFileElements.Enabled
        {
            set
            {
                if (value == false)
                {
                    _form.Text = "Z3 - <No Workspace>";
                }

                menuFileExport.Enabled = value;
                menuFileImport.Enabled = value;
                menuFileProperties.Enabled = value;
            }
        }

        public event EventHandler FileImportClicked;
        public event EventHandler FileExportClicked;
        public event EventHandler FileQueryClicked;
        public event EventHandler FilePropertiesClicked;

        #endregion

        #region DataSetElements Members

        bool DataSetElements.Enabled
        {
            set
            {
                menuDataSetSelect.Enabled = value;
                menuDataSetNew.Enabled = value;
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
                //Do nothing; do not display name on menu bar
            }
        }

        bool ActiveDataSetElements.Enabled
        {
            set
            {
                menuDataSetProperties.Enabled = value;
                menuDataSetDelete.Enabled = value;
            }
        }

        public event EventHandler DataSetPropertiesClicked;
        public event EventHandler DataSetDeleteClicked;
        #endregion

        #region ActiveDataPointElements Members
        
        bool ActiveDataPointElements.Enabled
        {
            set
            {
                menuDataPointDelete.Enabled = value;
                menuDataPointEdit.Enabled = value;
            }
        }
        
        public event EventHandler PointEditClicked;
        public event EventHandler PointDeleteClicked;
        
        #endregion

        #region CountableElements Members

        bool CountableElements.Enabled
        {
            set
            {
                menuDataCountableNew.Enabled = value;
            }
        }

        public event EventHandler CountableNewClicked;
        #endregion
        
        #region ActiveCountableElements Members

        bool ActiveCountableElements.Enabled
        {
            set
            {
                menuDataCountableEdit.Enabled = value;
                menuDataCountableDelete.Enabled = value;
            }
        }

        public event EventHandler CountableEditClicked;
        public event EventHandler CountableDeleteClicked;
        event EventHandler ActiveCountableElements.CountableAssignHotkeyClicked { add { } remove { } }
        event EventHandler ActiveCountableElements.CountableClearHotkeyClicked { add { } remove { } }

        #endregion

        #region ReadyControlElements Members
        
        bool ReadyControlElements.Enabled
        {
            set
            {
                menuToolsCounting.Enabled = value;
                menuToolsCounting.Text = ("Start Counting");
            }
        }

        bool ReadyControlElements.Counting
        {
            set
            {
                menuToolsCounting.Text = (value ? "Stop" : "Start") + " Counting";
            }
        }

        public event EventHandler StartStopCountingClicked;
        public event EventHandler UndoClicked;
        public event EventHandler RecalibrateClicked;
        public event EventHandler ZoomClicked;

        #endregion

        #region GlobalControlElements Members

        public event EventHandler SchemaEditorClicked;
        public event EventHandler OptionsClicked;
        
        #endregion


        #region CalibControlElements Members

        bool CalibControlElements.Enabled
        {
            set {
                menuToolsRecalibrate.Enabled = !value;
            }
        }

        event EventHandler<CalibrationEventArgs> CalibControlElements.CalibratePerformed
        {
            add { }
            remove { }
        }

        #endregion
    }
}
