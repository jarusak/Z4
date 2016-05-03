/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.Model;

namespace Z3.View.Standard
{
    class VideoManager //: IVideoManager
    {
        //private ViewManager _brain;
        private Panel _panel;
        private ToolStripMenuItem _menu;
        private Form _form;
        private FormWindowState lastWindowState;
        private IContainer _Container;

        public Zoopomatic2.Controls.Measurer Measurer {
            get { return measures; }
        }

        public VideoManager(Panel p, ToolStripMenuItem m, Form f, IContainer cont)
        {
            _Container = cont;
            InitializeComponent();
            _form = f;
            //_brain = v;
            _panel = p;
            _menu = m;
            _menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miVideoDevice,
            this.miVideoFrameSize,
            this.miVideoFrameRate,
            this.miVideoSep0,
            this.miVideoInputSource,
            this.miVideoTVChannel,
            this.meVideoSep1,
            this.miVideoProperties});
            p.Controls.Add(vHolderPanel);
        }

        public void EnableVideo(bool p)
        {
            measures.clear();
            measures.Enabled = p;
            if (p)
                measures.startCalibration();
        }

        #region Video Menu 
 */
        /*private void menuVideoDeviceItem_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            _menu.DropDown.Close();
            _brain.WindowMgr.Cursor = Cursors.WaitCursor;

            Filter newFilter = (Filter)(e.ClickedItem.Tag);
            if (newFilter != vPanel.VideoDevice)
            {
                vPanel.VideoDevice = newFilter;

                if (vPanel.isReady())
                    videoResize();

                if (vPanel.isReady() && !vPanel.isRendering())
                    vPanel.startRendering();

                checkVideoDevice();
                populateVideoDependents();
                _brain.State.sourceVideo();
            }
            _brain.WindowMgr.Cursor = Cursors.Default;
        }*/

        /*private void miVideoFrameSize_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _menu.DropDown.Close();
            _brain.WindowMgr.Cursor = Cursors.WaitCursor;

            Size currentSize = vPanel.FrameSize;
            bool wasRendering = vPanel.isRendering();

            try
            {
                if (e.ClickedItem.Tag != null)
                {
                    _brain.WindowMgr.Cursor = Cursors.WaitCursor;
                    vPanel.FrameSize = (Size)(e.ClickedItem.Tag);
                    videoResize();
                    _brain.WindowMgr.Cursor = Cursors.Default;
                }
                else
                {
                    FrameSizeForm fsf = new FrameSizeForm();

                    while (!fsf.IsDisposed)
                    {
                        try
                        {
                            if (fsf.ShowDialog() == DialogResult.OK)
                            {
                                // User clicked OK.
                                _brain.WindowMgr.Cursor = Cursors.WaitCursor;
                                vPanel.FrameSize = new Size(fsf.FrameWidth, fsf.FrameHeight);
                                videoResize();
                                _brain.WindowMgr.Cursor = Cursors.Default;
                            }
                            fsf.Dispose();
                        }
                        catch (FormatException)
                        {
                            MessageBox.Show("Please enter a valid frame size.", "Warning");
                            //fsf.Show();
                        }
                        catch (Exception ex)
                        {
                            fsf.Dispose();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("The specified frame size is not supported.", "Error");
                _brain.WindowMgr.Cursor = Cursors.WaitCursor;
                vPanel.FrameSize = currentSize;
                _brain.WindowMgr.Cursor = Cursors.Default;
            }
            finally
            {
                if (wasRendering && !vPanel.isRendering())
                {
                    _brain.WindowMgr.Cursor = Cursors.WaitCursor;
                    vPanel.startRendering();
                }
                _brain.WindowMgr.Cursor = Cursors.Default;
            }

            _brain.WindowMgr.Cursor = Cursors.WaitCursor;
            populateVideoFrameSizes();
            _brain.WindowMgr.Cursor = Cursors.Default;
        }*/

        /*private void miVideoFrameRate_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _menu.DropDown.Close();
            _brain.WindowMgr.Cursor = Cursors.WaitCursor;

            double currentRate = vPanel.FrameRate;
            bool wasRendering = vPanel.isRendering();

            try
            {
                if (e.ClickedItem.Tag != null)
                {
                    _brain.WindowMgr.Cursor = Cursors.WaitCursor;
                    vPanel.FrameRate = (double)(e.ClickedItem.Tag);
                    _brain.WindowMgr.Cursor = Cursors.Default;
                }
                else
                {
                    FrameRateForm frf = new FrameRateForm();

                    while (!frf.IsDisposed)
                    {
                        try
                        {
                            if (frf.ShowDialog() == DialogResult.OK)
                            {
                                // User clicked OK.
                                _brain.WindowMgr.Cursor = Cursors.WaitCursor;
                                vPanel.FrameRate = frf.FrameRate;
                                _brain.WindowMgr.Cursor = Cursors.Default;
                            }
                            frf.Dispose();
                        }
                        catch (FormatException)
                        {
                            MessageBox.Show("Please enter a valid frame rate.", "Warning");
                            //frf.Show();
                        }
                        catch (Exception ex)
                        {
                            frf.Dispose();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("The specified frame rate is not supported.", "Error");
                _brain.WindowMgr.Cursor = Cursors.WaitCursor;
                vPanel.FrameRate = currentRate;
                _brain.WindowMgr.Cursor = Cursors.Default;

            }
            finally
            {
                if (wasRendering && !vPanel.isRendering())
                {
                    _brain.WindowMgr.Cursor = Cursors.WaitCursor;
                    vPanel.startRendering();
                }
                _brain.WindowMgr.Cursor = Cursors.Default;
            }

            _brain.WindowMgr.Cursor = Cursors.WaitCursor;
            populateVideoFrameRates();
            _brain.WindowMgr.Cursor = Cursors.Default;
        }*/

        /*private void miVideoInputSource_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _menu.DropDown.Close();
            _brain.WindowMgr.Cursor = Cursors.WaitCursor;

            Source newSource = (Source)(e.ClickedItem.Tag);
            if (newSource == vPanel.InputSource)
                return;
            vPanel.InputSource = newSource;
            checkVideoInputSource();
            _brain.State.decalibrateVideo();

            _brain.WindowMgr.Cursor = Cursors.Default;
        }*/

        /*private void miVideoTVChannel_Click(object sender, EventArgs e)
        {
            _menu.DropDown.Close();

            System.Windows.Forms.MessageBox.Show("This feature is not yet implemented.", "Feature Unavailable");

        }*/

        /*private void miVideoProperties_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            using (PropertyPage p = (PropertyPage)(e.ClickedItem.Tag))
            {
                _menu.DropDown.Close();
                p.Show(_panel);
            }
        }*/

/*
        #endregion
        #region Video
        private void populateVideoDevices()
        {
            // Populates the "Video Device" submenu.
            FilterCollection fc = vPanel.VideoDevices;
            if (fc != null && fc.Count > 0)
            {
                miVideoDevice.DropDownItems.Clear();
                foreach (Filter f in vPanel.VideoDevices)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(f.Name);
                    item.Tag = f;
                    miVideoDevice.DropDownItems.Add(item);
                }
            }

            checkVideoDevice();
        }

        private void checkVideoDevice()
        {
            Filter f = vPanel.VideoDevice;

            foreach (ToolStripMenuItem item in miVideoDevice.DropDownItems)
            {
                item.Checked = (f != null) && (((Filter)item.Tag).Equals(f));
            }
        }

        private void videoResize()
        {
            vPanel.doResize(vHolderPanel.Size);

            Control currentSource = vPanel;
            measures.Size = currentSource.Size;

            int x = (vHolderPanel.Size.Width - currentSource.Size.Width) / 2;
            int y = (vHolderPanel.Size.Height - currentSource.Size.Height) / 2;

            int i = x + currentSource.Size.Width - hackPanel.Size.Width;
            int j = y + currentSource.Size.Height - hackPanel.Size.Height;

            measures.Location =
                currentSource.Location = new Point(x, y);
            hackPanel.Location = new Point(i, j);
        }

        private void populateVideoDependents()
        {
            populateVideoFrameRates();
            populateVideoFrameSizes();
            populateVideoInputSources();
            populateVideoTvChannels();
            populateVideoProperties();
        }

        private void populateVideoFrameRates()
        {
            // Populates the "Frame Rate" submenu.
            miVideoFrameRate.DropDownItems.Clear();

            //ToolStripMenuItem
            List<double> defaultRates = new List<double>();
            defaultRates.Add(15);
            defaultRates.Add(23.976);
            defaultRates.Add(25);
            defaultRates.Add(29.97);
            defaultRates.Add(30);

            List<double> givenRates = vPanel.getFrameRates();

            foreach (double r in defaultRates)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = Math.Round(r, 3).ToString() + " fps";
                item.Tag = r;

                // Inserts the given size if appropriate
                if (givenRates.Count > 0)
                {
                    if (givenRates[0] < r)
                    {
                        ToolStripMenuItem givenItem = new ToolStripMenuItem();
                        givenItem.Text = Math.Round(givenRates[0], 3).ToString() + " fps";
                        givenItem.Tag = givenRates[0];
                        miVideoFrameRate.DropDownItems.Add(givenItem);
                        givenRates.RemoveAt(0);
                    }
                    else if (Math.Abs(givenRates[0] - r) <= 0.001)
                    {
                        givenRates.RemoveAt(0);
                    }
                }
                miVideoFrameRate.DropDownItems.Add(item);
            }

            foreach (double r in givenRates)
            {
                ToolStripMenuItem givenItem = new ToolStripMenuItem();
                givenItem.Text = Math.Round(r, 3).ToString() + " fps";
                givenItem.Tag = r;
                miVideoFrameRate.DropDownItems.Add(givenItem);
            }

            double currentRate = vPanel.FrameRate;
            bool checkedOne = false;
            foreach (ToolStripMenuItem item in miVideoFrameRate.DropDownItems)
            {
                item.Checked = Math.Abs(((double)item.Tag) - currentRate) <= 0.001;
                checkedOne = checkedOne || item.Checked;
            }

            ToolStripMenuItem customItem = new ToolStripMenuItem();
            customItem.Text = "Custom...";

            if (!checkedOne)
            {
                customItem.Text += " (" + Math.Round(currentRate, 3).ToString() + " fps) ...";
                customItem.Checked = true;
            }

            miVideoFrameRate.DropDownItems.Add(customItem);

            miVideoFrameRate.Enabled = vPanel.isRendering();
        }

        private void checkVideoInputSource()
        {
            Source s = vPanel.InputSource;

            foreach (ToolStripMenuItem item in miVideoInputSource.DropDownItems)
            {
                item.Checked = (s != null) && (((Source)item.Tag).Equals(s));
            }
        }

        private void populateVideoInputSources()
        {
            // Populates the "Input Source" submenu.
            if (vPanel.isReady())
            {
                SourceCollection sources = vPanel.InputSources;

                if (sources.Count > 0)
                {
                    miVideoInputSource.Enabled = true;
                    // Populates submenu
                    miVideoInputSource.DropDownItems.Clear();
                    foreach (Source s in sources)
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(s.Name);
                        item.Tag = s;
                        miVideoInputSource.DropDownItems.Add(item);
                    }
                }
                else
                {
                    miVideoInputSource.Enabled = false;
                }
            }
            else
            {
                miVideoInputSource.Enabled = false;
            }

            checkVideoInputSource();
        }

        private void populateVideoTvChannels()
        {
            // Populates the "TV Channel" submenu.
            miVideoTVChannel.Enabled = vPanel.hasChannels();
        }

        private void populateVideoProperties()
        {
            // Populates the "Properties" submenu if necessary.
            if (vPanel.isReady())
            {
                PropertyPageCollection pages = vPanel.PropertyPages;

                if (pages.Count > 1)
                {
                    miVideoProperties.Text = "Properties";
                    miVideoProperties.Enabled = true;
                    // Creates a drop down menu.

                    miVideoProperties.DropDownItems.Clear();
                    foreach (PropertyPage p in pages)
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(p.Name);
                        item.Tag = p;
                        miVideoProperties.DropDownItems.Add(item);
                    }
                }
                else if (pages.Count == 1)
                {
                    miVideoProperties.Text = "Properties";
                    miVideoProperties.Enabled = true;
                    // Does not create a drop down menu.
                }
                else
                {
                    miVideoProperties.Text = "Properties";
                    miVideoProperties.Enabled = false;
                }
            }
            else
            {
                // Disables menu item.
                miVideoProperties.Enabled = false;
            }
        }
        private void populateVideoFrameSizes()
        {
            // Populates the "Frame Size" submenu.
            miVideoFrameSize.DropDownItems.Clear();

            //ToolStripMenuItem
            List<Size> defaultSizes = new List<Size>();
            defaultSizes.Add(new Size(160, 120));
            defaultSizes.Add(new Size(320, 240));
            defaultSizes.Add(new Size(640, 480));
            defaultSizes.Add(new Size(1024, 768));

            List<Size> givenSizes = vPanel.getFrameSizes();

            foreach (Size s in defaultSizes)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = s.Width.ToString() + "x" + s.Height.ToString();
                item.Tag = s;

                // Inserts the given size if appropriate
                if (givenSizes.Count > 0 && givenSizes[0].Width <= s.Width)
                {
                    ToolStripMenuItem givenItem = new ToolStripMenuItem();
                    givenItem.Text = givenSizes[0].Width.ToString() + "x" +
                        givenSizes[0].Height.ToString();
                    givenItem.Tag = givenSizes[0];
                    if (givenSizes[0].Width < s.Width ||
                        (givenSizes[0].Width == s.Width &&
                         givenSizes[0].Height < s.Height))
                    {
                        miVideoFrameSize.DropDownItems.Add(givenItem);
                        miVideoFrameSize.DropDownItems.Add(item);
                    }
                    else if (givenSizes[0].Width == s.Width &&
                         givenSizes[0].Height > s.Height)
                    {
                        miVideoFrameSize.DropDownItems.Add(item);
                        miVideoFrameSize.DropDownItems.Add(givenItem);
                    }
                    else
                    {
                        miVideoFrameSize.DropDownItems.Add(item);
                        givenItem.Dispose();
                    }
                    givenSizes.RemoveAt(0);
                }
                else
                {
                    miVideoFrameSize.DropDownItems.Add(item);
                }
            }

            foreach (Size s in givenSizes)
            {
                ToolStripMenuItem givenItem = new ToolStripMenuItem();
                givenItem.Text = s.Width.ToString() + "x" + s.Height.ToString();
                givenItem.Tag = s;
                miVideoFrameSize.DropDownItems.Add(givenItem);
            }

            Size currentSize = vPanel.FrameSize;
            bool checkedOne = false;
            foreach (ToolStripMenuItem item in miVideoFrameSize.DropDownItems)
            {
                item.Checked = ((Size)item.Tag).Equals(currentSize);
                checkedOne = checkedOne || item.Checked;
            }

            ToolStripMenuItem customItem = new ToolStripMenuItem();
            customItem.Text = "Custom...";

            if (!checkedOne)
            {
                customItem.Text += " (" + currentSize.Width.ToString() + "x" +
                    currentSize.Height.ToString() + ")";
                customItem.Checked = true;
            }

            miVideoFrameSize.DropDownItems.Add(customItem);

            miVideoFrameSize.Enabled = vPanel.isRendering();
        }
                #endregion

        #region Controls
        private void InitializeComponent()
        {
            this.miVideoDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.miVideoFrameSize = new System.Windows.Forms.ToolStripMenuItem();
            this.miVideoFrameRate = new System.Windows.Forms.ToolStripMenuItem();
            this.miVideoSep0 = new System.Windows.Forms.ToolStripSeparator();
            this.miVideoInputSource = new System.Windows.Forms.ToolStripMenuItem();
            this.miVideoTVChannel = new System.Windows.Forms.ToolStripMenuItem();
            this.meVideoSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.miVideoProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.vHolderPanel = new System.Windows.Forms.Panel();
            this.hackPanel = new System.Windows.Forms.Panel();
            this.measures = new Zoopomatic2.Controls.Measurer();
            this.vPanel = new Zoopomatic2.Controls.VideoPanel();
            this.vHolderPanel.SuspendLayout();
            // 
            // miVideoDevice
            // 
            this.miVideoDevice.Name = "miVideoDevice";
            this.miVideoDevice.Size = new System.Drawing.Size(152, 22);
            this.miVideoDevice.Text = "Video Device";
            this.miVideoDevice.DropDownItemClicked += new ToolStripItemClickedEventHandler(miVideoDevice_DropDownItemClicked);
            // 
            // miVideoFrameSize
            // 
            this.miVideoFrameSize.Name = "miVideoFrameSize";
            this.miVideoFrameSize.Size = new System.Drawing.Size(152, 22);
            this.miVideoFrameSize.Text = "Frame Size";
            this.miVideoFrameSize.DropDownItemClicked += new ToolStripItemClickedEventHandler(miVideoFrameSize_DropDownItemClicked);
            // 
            // miVideoFrameRate
            // 
            this.miVideoFrameRate.Name = "miVideoFrameRate";
            this.miVideoFrameRate.Size = new System.Drawing.Size(152, 22);
            this.miVideoFrameRate.Text = "Frame Rate";
            this.miVideoFrameRate.DropDownItemClicked += new ToolStripItemClickedEventHandler(miVideoFrameRate_DropDownItemClicked);
            // 
            // miVideoSep0
            // 
            this.miVideoSep0.Name = "miVideoSep0";
            this.miVideoSep0.Size = new System.Drawing.Size(149, 6);
            // 
            // miVideoInputSource
            // 
            this.miVideoInputSource.Name = "miVideoInputSource";
            this.miVideoInputSource.Size = new System.Drawing.Size(152, 22);
            this.miVideoInputSource.Text = "Input Source";
            this.miVideoInputSource.DropDownItemClicked += new ToolStripItemClickedEventHandler(miVideoInputSource_DropDownItemClicked);
            // 
            // miVideoTVChannel
            // 
            this.miVideoTVChannel.Name = "miVideoTVChannel";
            this.miVideoTVChannel.Size = new System.Drawing.Size(152, 22);
            this.miVideoTVChannel.Text = "TV Channel...";
            this.miVideoTVChannel.Visible = false;
            this.miVideoTVChannel.Click += new EventHandler(miVideoTVChannel_Click);
            // 
            // meVideoSep1
            // 
            this.meVideoSep1.Name = "meVideoSep1";
            this.meVideoSep1.Size = new System.Drawing.Size(149, 6);
            // 
            // miVideoProperties
            // 
            this.miVideoProperties.Name = "miVideoProperties";
            this.miVideoProperties.Size = new System.Drawing.Size(152, 22);
            this.miVideoProperties.Text = "Properties...";
            this.miVideoProperties.DropDownItemClicked += new ToolStripItemClickedEventHandler(miVideoProperties_DropDownItemClicked);
            // 
            // vHolderPanel
            // 
            this.vHolderPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.vHolderPanel.Controls.Add(this.measures);
            this.vHolderPanel.Controls.Add(this.hackPanel);
            this.vHolderPanel.Controls.Add(this.vPanel);
            this.vHolderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vHolderPanel.Location = new System.Drawing.Point(0, 0);
            this.vHolderPanel.Name = "vHolderPanel";
            this.vHolderPanel.Size = new System.Drawing.Size(364, 298);
            this.vHolderPanel.TabIndex = 0;
            this.vHolderPanel.Resize += new System.EventHandler(this.Object_Resize);
            // 
            // hackPanel
            // 
            this.hackPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.hackPanel.Location = new System.Drawing.Point(179, 146);
            this.hackPanel.Margin = new System.Windows.Forms.Padding(0);
            this.hackPanel.Name = "hackPanel";
            this.hackPanel.Size = new System.Drawing.Size(2, 2);
            this.hackPanel.TabIndex = 3;
            // 
            // measures
            // 
            //this.measures.ActiveColor = System.Drawing.Color.Red;
            this.measures.Calibrating = false;
            //this.measures.DotSize = 5;
            //this.measures.HistoryColor = System.Drawing.Color.Blue;
            //this.measures.KeepHistory = false;
            this.measures.Location = new System.Drawing.Point(73, 119);
            this.measures.Name = "measures";
            this.measures.Size = new System.Drawing.Size(200, 100);
            this.measures.TabIndex = 2;
            this.measures.Count += new EventHandler(measures_Count);
            this.measures.Measure += new EventHandler<Zoopomatic2.Controls.Measurer.MeasureEventArgs>(measures_Measure);
            //this.measures.Count += new System.EventHandler(this.measures_Count);
            //this.measures.Measure += new System.EventHandler<Zoopomatic2.Controls.Measurer.MeasureEventArgs>(this.measures_Measure);
            // 
            // tmrRedraw
            // 
            this.tmrRedraw = new System.Windows.Forms.Timer(_Container);
            this.tmrRedraw.Enabled = true;
            this.tmrRedraw.Interval = 2;
            this.tmrRedraw.Tick += new System.EventHandler(this.tmrRedraw_Tick);
            // 
            // vPanel
            // 
            this.vPanel.Channel = 0;
            this.vPanel.FrameRate = 0;
            this.vPanel.FrameSize = new System.Drawing.Size(200, 100);
            this.vPanel.InputSource = null;
            this.vPanel.Location = new System.Drawing.Point(0, 0);
            this.vPanel.Name = "vPanel";
            this.vPanel.Size = new System.Drawing.Size(200, 100);
            this.vPanel.TabIndex = 0;
            this.vPanel.VideoDevice = null;
            this.vHolderPanel.ResumeLayout(false);
        }

        void miVideoProperties_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            throw new NotImplementedException("todo");
        }

        void miVideoInputSource_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            throw new NotImplementedException("todo");
        }

        void miVideoFrameRate_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            throw new NotImplementedException("todo");
        }

        void miVideoFrameSize_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            throw new NotImplementedException("todo");
        }

        void miVideoTVChannel_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException("todo");
        }

        void miVideoDevice_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            throw new NotImplementedException("todo");
        }

        void measures_Measure(object sender, Zoopomatic2.Controls.Measurer.MeasureEventArgs e) {
            // Record: MType, Container, Countable
            //_brain.DataPointsMgr.Measure(_brain.CtlMgr.MeasurementType, (ZCountable)_brain.CountableMgr.Countable, e.convertedLength);
        }

        void measures_Count(object sender, EventArgs e) {
            // Record: MType, Container, Countable
            //_brain.DataPointsMgr.Count(_brain.CtlMgr.MeasurementType, (ZCountable)_brain.CountableMgr.Countable);
        }

        private void tmrRedraw_Tick(object sender, EventArgs e) {
            vPanel.Invalidate();
            measures.Invalidate();
        }


        private System.Windows.Forms.Timer tmrRedraw;
        private System.Windows.Forms.ToolStripMenuItem miVideoDevice;
        private System.Windows.Forms.ToolStripMenuItem miVideoFrameSize;
        private System.Windows.Forms.ToolStripMenuItem miVideoFrameRate;
        private System.Windows.Forms.ToolStripSeparator miVideoSep0;
        private System.Windows.Forms.ToolStripMenuItem miVideoInputSource;
        private System.Windows.Forms.ToolStripMenuItem miVideoTVChannel;
        private System.Windows.Forms.ToolStripSeparator meVideoSep1;
        private System.Windows.Forms.ToolStripMenuItem miVideoProperties;
        private System.Windows.Forms.Panel vHolderPanel;
        private Zoopomatic2.Controls.Measurer measures;
        private Zoopomatic2.Controls.VideoPanel vPanel;
        private System.Windows.Forms.Panel hackPanel;
        
        #endregion

        private void Object_Resize(object sender, EventArgs e)
        {
            if (_form.WindowState != FormWindowState.Minimized)
            {
                videoResize();
                lastWindowState = _form.WindowState;
            }
        }

        public void Resize() {
            videoResize();
        }

        public void Dispose() {
            vPanel.DisposeCapture();
            vPanel.Dispose();
        }

        //public void Loading() {
        //    populateVideoDevices();
        //    videoResize();
        //    populateVideoDependents();
        //    if (vPanel.isReady()) {
        //        _brain.State.sourceVideo();
        //        vPanel.startRendering();
        //    }
        //}

        //public void Calibrate(double len, int zoom) {
        //    if (measures.CanCalibrate()) {
        //        measures.calibrate(len, zoom);
        //        _brain.State.calibrateVideo();
        //    } else {
        //        MessageBox.Show("Please measure the distance in the preview window.", "Warning");
        //    }
        //}

        //public void EnableVideoCalib(bool p) {
        //    measures.clear();
        //    measures.Enabled = p && _brain.Ready;
        //}

        //public void EnableCounting(bool _counting) {
        //    if (measures.Calibrating) return;
        //
        //    if (_counting) {
        //        measures.Calibrating = false;
        //    }
        //    measures.clear();
        //    measures.Enabled = _counting;
        //}
    }
}
*/