using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace Zoopomatic2.Controls {
    public class TransparentPanel : Panel {
        public TransparentPanel() {
            // TO-DO: Add constructor logic here
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020;
                return cp;
            }
        }

        protected void InvalidateEx() {
            if (Parent == null)
                return;

            Rectangle rc = new Rectangle(this.Location, this.Size);
            Parent.Invalidate(rc, true);
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
            // Do not allow background to be painted
        }

        //protected override void OnPaint(PaintEventArgs e) {
        //    e.Graphics.DrawString("Test", Parent.Font, Brushes.Aquamarine, new PointF(0, 0));
            // paint the shapes here
        //}
    }
}
