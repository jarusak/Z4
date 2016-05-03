using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Z3.View.Floating;

namespace Z3.View
{
    class WindowManager
    {
        private List<Form> _windows = new List<Form>();

        public List<Form> Windows
        {
            get { return _windows; }
            set { _windows = value; }
        }
        //private ViewManager _brain;

        //public WindowManager(ViewManager view)
        //{
        //    _brain = view;
        //}

        public Cursor Cursor
        {
            set
            {
                foreach (Form w in _windows)
                    w.Cursor = value;
            }
        }

        //public bool isFloating()
        //{
        //    return (_brain.VideoMgr.GetType().Name.Equals("FVideoManager"));
        //}

        public Z3FloatingVideoForm getFloatingVideoForm()
        {
            foreach( Form x in _windows) 
                if (x.Name.Equals("Z3FloatingVideoForm"))
                    return (Z3FloatingVideoForm)x;
            return null;
        }

        public void ShowOverlay(bool show)
        {
            Z3FloatingVideoForm v = getFloatingVideoForm();
            if (v != null) v.Visible = show;
        }

        internal void Close()
        {
            foreach (Form w in _windows)
            {
                w.Close();
            }
        }
    }
}
