using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.View;

namespace Z3
{
    public partial class DataViewForm : Form, StopperElement
    {
        private bool _oneWindow = false;

        public DataViewForm()
        {
            InitializeComponent();
        }

        public ListView Data
        {
            get
            {
                return this.listView1;
            }
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

        public void showError(string message)
        {
            this.errorBox.Visible = true;
            this.errorBox.Text = message;
        }

        public void hideError()
        {
            this.errorBox.Visible = false;
        }

        private void DataViewForm_Load(object sender, EventArgs e)
        {
            new Z3.View.Util.WindowSizeMemory(this);
        }
    }
}