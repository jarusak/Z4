using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Z3;
using System.Diagnostics;

//TO-DO Add calibration support inside this class

namespace Zoopomatic2.Controls {
    public class Measurer : TransparentPanel {
        #region Fields
        
        //private int dsize;
        //private Color oldColor;
        private Brush oldBrush;
        private Pen oldPen;
        //private Color newColor;
        private Brush newBrush;
        private Pen newPen;
        private List<MPoint> points;
        private Boolean meta = false;
        private Boolean closed = true;
        private int myWidth = -1, myHeight = -1;

        // conversion factor f:
        // f * pixels = microns
        // f = microns / pixels
        // Note:  pixels at original resolution.
        private double conversionFactor;
        private int calibratedZoom;
        private Size calibratedSize;

        private Boolean calib = false;
        #endregion

        #region Properties
        
        /*public int DotSize {
            get {
                return Options.DotSize;
            }
            set {
                Options.DotSize = value;
            }
        }*/

        public void notifyColorChange()
        {
            newBrush = new SolidBrush(Options.ActiveColor);
            newPen = new Pen(newBrush);
            oldBrush = new SolidBrush(Options.HistoryColor);
            oldPen = new Pen(oldBrush);
        }

        public bool Ready {
            get {
                return closed;
            }
        }

        public bool Calibrating {
            get {
                return calib;
            }
            set {
                calib = value;
            }
        }

        //public bool KeepHistory {
        //    get {
        //        return historize;
        //    }
        //    set {
        //        historize = value;
        //    }
        //}

        #endregion

        public event EventHandler Calibrated;

        public void ExternalClick(MouseEventArgs e)
        {
            this.OnMouseDown(e);
            this.OnMouseUp(e);
        }
        public void clear() {
            points.Clear();
            InvalidateEx();
        }
        
        public void undo() {
            if (points.Count > 0)
                points.RemoveAt(points.Count - 1);

        }

        // this is never used
        //public void startCalibration()
        //{
        //    clear();
        //    calib = true;
        //}

        public bool CanCalibrate()
        {
            return calib && (points.Count == 2);
        }

        public void calibrate(double numberOfMicrons, int zoomFactor) {

            // Thank Pythagoras:  x*x + y*y = z*z
            double xx = Math.Pow(Math.Abs((points[0].x - points[1].x) * this.Size.Width), 2.0);
            double yy = Math.Pow(Math.Abs((points[0].y - points[1].y) * this.Size.Height), 2.0);

            conversionFactor = numberOfMicrons / Math.Sqrt(xx + yy);
            calibratedZoom = zoomFactor;
            calibratedSize = new Size(this.Size.Width, this.Size.Height);

            clear();
            calib = false;
            if (Calibrated != null)
                Calibrated(this, new EventArgs());
        }

        public void zoom(int newZoomFactor) {
            conversionFactor = calibratedZoom * conversionFactor / newZoomFactor;
            calibratedZoom = newZoomFactor;
        }

        public int getCalibratedZoom()
        {
            return calibratedZoom;
        }

        private class MPoint {
            public float x, y;

            public MPoint(float x, float y) {
                this.x = x;
                this.y = y;
            }
        }
        #region Events
        
        public event EventHandler<MeasureEventArgs> Measure;
        public event EventHandler Count;
        public class MeasureEventArgs : EventArgs {
            public MeasureEventArgs(double len, double conv) {
                length = len;
                convertedLength = conv;
            }
            public double convertedLength;
            public double length;
        }
        #endregion       
        #region Constructor
        public Measurer() {
            points = new List<MPoint>();
            Options.Measurer = this;
            notifyColorChange();
        }
        #endregion
        #region Method Overrides
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            Pen p;
            Brush b;
            if (closed) p = oldPen; else p = newPen;
            if (closed) b = oldBrush; else b = newBrush;

            for (int i = 0; i < points.Count; i++) {
                e.Graphics.FillEllipse(b, (points[i].x * Width) - (Options.DotSize / 2), (points[i].y * Height) - (Options.DotSize / 2), Options.DotSize, Options.DotSize);
                if (i > 0)
                    e.Graphics.DrawLine(p, points[i - 1].x * Width, points[i - 1].y * Height, points[i].x * Width, points[i].y * Height);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            if (!Enabled) return;

            base.OnMouseDown(e);

            myWidth = Width;
            myHeight = Height;
            
            meta = (e.Button == MouseButtons.Right);
            // checks in the options if the mouse has been inverted
            if (Options.InvertMouseButtons)
                meta = !meta;
            if (Control.ModifierKeys == Keys.Control)
                meta = !meta;
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            if (!Enabled) return;

            base.OnMouseUp(e);

            //Debug.WriteLine("Calib: " + calib);
            //Debug.WriteLine("meta: " + meta);
            //Debug.WriteLine("closed: " + closed);
            // if we are in calibration mode
            if (calib)
            {
                if (points.Count > 1)
                    points.RemoveAt(0);

                points.Add(new MPoint((float)e.X / (float)Width, (float)e.Y / (float)Height));
                InvalidateEx(); // will set the overlay window to update
                Debug.WriteLine("In Calib");
            }
            // closed = when left button has been clicked and we are not in calibrate mode
            else if (closed)
            {
                points.Clear();
                points.Add(new MPoint((float)e.X / (float)Width, (float)e.Y / (float)Height));
                Debug.WriteLine("In closed");
                InvalidateEx();
            }
            else
            {
                points.Add(new MPoint((float)e.X / (float)Width, (float)e.Y / (float)Height));
                // Why isn't this InvalidateEx()?  Dan?
                Invalidate();
                Debug.WriteLine("In else");
            }

            closed = !meta && !calib;
            //Debug.WriteLine(calibratedSize.Width);
            //Debug.WriteLine(calibratedSize.Width);
            if (closed) {
                double totalPixels = 0;

                for (int i = 1; i < points.Count; i++)
                {
                    // Thank Pythagoras:  x*x + y*y = z*z
                    double xx = Math.Pow(Math.Abs((points[i - 1].x - points[i].x) 
                                                    * calibratedSize.Width), 2.0);
                    double yy = Math.Pow(Math.Abs((points[i - 1].y - points[i].y)
                                                    * calibratedSize.Height), 2.0);
                    totalPixels += Math.Sqrt(xx + yy);
                }
                // where i left off
                if (points.Count > 1) {
                    if (Measure != null)
                        Measure(this, new MeasureEventArgs(totalPixels, 
                            convertToMicrons(totalPixels)));
                } else {
                    if (Count != null)
                        Count(this, new EventArgs());
                }
            }
        }
        #endregion

        private double convertToMicrons(double pixelLength) {
            return pixelLength * conversionFactor;
        }
    }
}
