using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.Model;
using Z3.Util;

namespace Z3.View.Floating
{
    public partial class Z3FloatingVideoForm : Form
    {
        private MouseHook hook;
        //private Zoopomatic2.Controls.Measurer _meas;
        private int _w;
        private int _h;
        
        public Z3FloatingVideoForm()
        {
            InitializeComponent();

            hook = new MouseHook(true, true);
            //hook.OnMouseActivity += new MouseEventHandler(hook_OnMouseActivity);
            //hook.Start(true, true);

            this.Load += new EventHandler(Z3FloatingVideoForm_Load);
        }

        void Z3FloatingVideoForm_Load(object sender, EventArgs e)
        {
            new Z3.View.Util.WindowSizeMemory(this);
        }

        public Zoopomatic2.Controls.Measurer Measurer
        {
            get { return _meas; }
        }

        void Z3FloatingVideoForm_ResizeEnd(object sender, System.EventArgs e)
        {
            hook.Resume();
        }

        void Z3FloatingVideoForm_ResizeBegin(object sender, System.EventArgs e)
        {
            hook.Suspend();
        }

        public MouseHook Hook
        {
            get { return hook; }
        }
        
        //private void hook_KeyPress(object sender, KeyPressEventArgs k) {
        //    //Let's only respond to Alphabet characters
        //    if (!Char.IsLetter(k.KeyChar))
        //    {
        //        if (k.KeyChar == '`')
        //        {
        //            _brain.toggleVisibility();
        //        }
                //else if(!_brain.Inactive)
                //{
                //    switch (k.KeyChar)
                //    {
                //        case '!':
                //            _brain.FocusWindow("Z3FloatingMenuForm");
                //            k.Handled = true;
                //            break;
                /*        case '@':
                            _brain.FocusWindow("Z3FloatingControlsForm");
                            k.Handled = true;
                            break;
                        case '#':
                            _brain.FocusWindow("Z3FloatingMeasurementForm");
                            k.Handled = true;
                            break;
                        case '$':
                            _brain.FocusWindow("Z3FloatingVideoForm");
                            k.Handled = true;
                            break;
                        case (char)27:
                            _brain.DataPointsMgr.Individual = null;
                            k.Handled = true;
                            break;
                    }
                }*/
        //        return;
        //    }
        //
        //    if (_brain.Inactive) return;
        //
        //    Dictionary<char, ZCountable> dic = _brain.CountableMgr.Hotkeys;
        //
        //    char key = Char.ToUpper(k.KeyChar);
        //    if (dic.ContainsKey(key)) {
        //        ZCountable x;
        //        //shouldn't need the 2nd half of the if statement, but somehow received
        //        //an invalid cast exception: Unable to cast type  
        //        //'Z3.Model.ZContainer' to type 'Z3.Model.ZCountable'
        //        if (dic.TryGetValue(key, out x) && x.GetType().Name.Equals("ZCountable"))
        //        {
        //            _brain.CountableMgr.Select(x);
        //        }
        //    }
        //}

        void hook_OnMouseActivity(object sender, MouseEventArgs e)
        {
            if (_meas.Enabled)
            // checks if mouse click colliders with the video overlay
            if (e.Clicks == 1) if (e.X > (this.Left + _w + _meas.Left + 3) && e.X < (this.Left + _w + _meas.Left + _meas.Width + 3)
                    && e.Y > (this.Top + _h + _meas.Top + 6) && e.Y < (this.Top + _h + _meas.Top + _meas.Height + 6))
            {
                MouseEventArgs a = new MouseEventArgs(e.Button, e.Clicks, e.X - this.Left - _w - _meas.Left - 3, e.Y - this.Top - _h - _meas.Top - 6, e.Delta);
                _meas.ExternalClick(a);
            }
        }

        public Panel Workspace
        {
            get { return panel1; }
        }

        //private void _Resize(object sender, EventArgs e)
        //{
        //    if (_brain != null && _brain.VideoMgr != null)
        //        if (this.WindowState != FormWindowState.Minimized)
        //        {
        //            _brain.VideoMgr.Resize();
        //            lastWindowState = this.WindowState;
        //        }
        //}

        //private void _ResizeEnd(object sender, EventArgs e)
        //{
        //    if (this.WindowState != FormWindowState.Minimized)
        //    {
        //        _brain.VideoMgr.Resize();
        //        lastWindowState = this.WindowState;
        //    }
        //}

        private void Z3FloatingVideoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            hook.Stop();
        }

        private void Z3FloatingVideoForm_Shown(object sender, EventArgs e)
        {
            // gets the width of the border of the video overlay
            _w = (this.Width - this.ClientSize.Width) / 2;
            // get the height of borders, title area or any non-client area
            _h = this.Height - this.ClientSize.Height - 2 * _w;
        }
    }
}