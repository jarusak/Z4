/*
using System;
using System.Collections.Generic;
using System.Text;
using DirectX.Capture;
using System.Drawing;

namespace Zoopomatic2.Controls {
    public class VideoPanel : System.Windows.Forms.Panel {
        private Capture myCapture;
        private Filters myFilters;
        private Filter myDevice;
        private bool previewing;

        #region Constructors

        public VideoPanel() {
            myCapture = null;
            try {
                myFilters = new Filters();
            } catch (NotSupportedException) {
                myFilters = null;
            }
            myDevice = null;
            previewing = false;

            // Debugging only:
            //VideoDevice = VideoDevices[1];
        }

        #endregion

        public bool isReady() {
            return myCapture != null;
        }

        public FilterCollection VideoDevices {
            get {
                if (myFilters == null)
                    return null;

                return myFilters.VideoInputDevices;
            }
        }

        public Filter VideoDevice {
            get {
                return myDevice;
            }
            set {
                myDevice = value;

                if (value != null) {
                    if (isRendering()) {
                        stopRendering();
                        
                        if (myCapture != null)
                            myCapture.Dispose();

                        myCapture = new Capture(myDevice, null);
                        
                        startRendering();
                    } else {
                        if (myCapture != null)
                            myCapture.Dispose();
                        
                        myCapture = new Capture(myDevice, null);
                    }
                } else {
                    if (myCapture != null)
                        myCapture.Dispose();

                    myCapture = null;
                }
            }
        }

        public bool isRendering()
        {
            return previewing;
        }

        public void startRendering()
        {
            if (!isReady())
            {
                throw new InvalidOperationException();
            }
            myCapture.PreviewWindow = this;
            previewing = true;
        }

        public void stopRendering()
        {
            if (!isReady())
            {
                throw new InvalidOperationException();
            }

            myCapture.PreviewWindow = null;
            previewing = false;
        }

        public Size FrameSize {
            get {
                if (!isReady())
                    return this.Size;

                return myCapture.FrameSize;
            }
            set {
                if (isReady())
                {
                    if (isRendering())
                    {
                        stopRendering();
                        myCapture.FrameSize = value;
                        startRendering();
                    }
                    else
                    {
                        myCapture.FrameSize = value;
                    }
                }
            }
        }

        public double FrameRate {
            get {
                if (!isReady())
                    return 0;

                return myCapture.FrameRate;
            }
            set {
                if (isReady())
                {
                    if (isRendering())
                    {
                        stopRendering();
                        myCapture.FrameRate = value;
                        startRendering();
                    }
                    else
                    {
                        myCapture.FrameRate = value;
                    }
                }
            }
        }

        public PropertyPageCollection PropertyPages {
            get {
                if (!isReady())
                    return null;

                return myCapture.PropertyPages;
            }
        }

        public SourceCollection InputSources {
            get {
                if (!isReady())
                    return null;

                return myCapture.VideoSources;
            }
        }

        public Source InputSource {
            get {
                if (!isReady())
                {
                    return null;
                }

                return myCapture.VideoSource;
            }
            set {
                if (isReady())
                {
                    myCapture.VideoSource = value;
                }
            }
        }

        public List<Size> getFrameSizes()
        {
            List<Size> ls = new List<Size>();

            if (!isReady())
                return ls;

            ls.Add(myCapture.VideoCaps.MinFrameSize);

            Size maxSize = myCapture.VideoCaps.MaxFrameSize;

            if (!ls[0].Equals(maxSize))
                ls.Add(maxSize);

            return ls;
        }

        public List<double> getFrameRates()
        {
            List<double> ls = new List<double>();

            if (!isReady())
                return ls;

            ls.Add(myCapture.VideoCaps.MinFrameRate);

            double maxRate = myCapture.VideoCaps.MaxFrameRate;

            if (!ls[0].Equals(maxRate))
                ls.Add(maxRate);

            return ls;
        }

        public bool hasChannels() {
            if (!isReady())
                return false;
            
            // Returns true if the device is a TV device.
            return myCapture.Tuner != null;
        }

        public int Channel {
            get {
                if (!hasChannels())
                    return 0;

                return myCapture.Tuner.Channel;
            }
            set {
                if (hasChannels())
                    myCapture.Tuner.Channel = value;
            }
        }

        public void doResize(Size s) {
            if (isReady()) {
                Size fs = FrameSize;

                double arCur = (double)fs.Width / (double)fs.Height;

                Size t = new Size();

                t.Width = (int)Math.Min(s.Width, s.Height * arCur);
                t.Height = (int)Math.Min(s.Height, s.Width / arCur);

                this.Size = t;
            } else {
                this.Size = s;
            }
        }

        public void DisposeCapture()
        {
            if (myCapture != null)
                myCapture.Dispose();
        }
    }
}
*/