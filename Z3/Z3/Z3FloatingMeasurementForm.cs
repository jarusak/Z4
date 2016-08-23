using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Z3.View.Floating {
    public partial class Z3FloatingMeasurementForm : Form {

        private bool _oneWindow = false;

        public Z3FloatingMeasurementForm() {
            InitializeComponent();

            this.Load += new EventHandler(Z3FloatingMeasurementForm_Load);
            
        }

        public bool OneWindow
        {
            get { return _oneWindow; }
            set { _oneWindow = value; }
        }

        protected override void WndProc(ref Message message)
        {
            if (_oneWindow)
            {
                const int WM_SYSCOMMAND = 0x0112;
                const int SC_MOVE = 0xF010;

                switch (message.Msg)
                {
                    case WM_SYSCOMMAND:
                        int command = message.WParam.ToInt32() & 0xfff0;
                        if (command == SC_MOVE)
                            return;
                        break;
                }
            }
            base.WndProc(ref message);
        }

        void Z3FloatingMeasurementForm_Load(object sender, EventArgs e)
        {
            new Z3.View.Util.WindowSizeMemory(this);
        }

        public Panel Workspace {
            get { return panel1; }
        }
    }
}