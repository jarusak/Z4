using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Z3.View.Util
{
    // saves the size and location of the windows
    class WindowSizeMemory
    {
        private Form _f;

        public WindowSizeMemory(Form f)
        {
            _f = f;

            _f.Shown += new EventHandler(_f_Shown);
            _f.ResizeEnd += new EventHandler(_f_ResizeEnd);
            _f.Move += new EventHandler(_f_Move);
            _f.Disposed += new EventHandler(_f_Disposed);
        }

        void _f_Shown(object sender, EventArgs e)
        {
            if (_f.WindowState == FormWindowState.Normal)
            {
                int x, y, w, h;
                x = Options.GetNumeric(_f.Name + "X");
                y = Options.GetNumeric(_f.Name + "Y");
                w = Options.GetNumeric(_f.Name + "W");
                h = Options.GetNumeric(_f.Name + "H");

                if (x > -1)
                {
                    _f.Left = x;
                }
                if (y > -1)
                {
                    _f.Top = y;
                }
                if (w > -1)
                {
                    _f.Width = w;
                }
                if (h > -1)
                {
                    _f.Height = h;
                }
            }
        }

        void _f_Disposed(object sender, EventArgs e)
        {
            _f = null;
        }

        void _f_Move(object sender, EventArgs e)
        {
            if (_f.WindowState == FormWindowState.Normal)
            {
                Options.SetNumeric(_f.Name + "X", _f.Left);
                Options.SetNumeric(_f.Name + "Y", _f.Top);
            }
        }

        void _f_ResizeEnd(object sender, EventArgs e)
        {
            if (_f.WindowState == FormWindowState.Normal)
            {
                Options.SetNumeric(_f.Name + "W", _f.Width);
                Options.SetNumeric(_f.Name + "H", _f.Height);
            }
        }
    }
}
