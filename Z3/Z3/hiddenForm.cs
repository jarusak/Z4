using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Z3
{
    public partial class hiddenForm : Form
    {
        public hiddenForm()
        {
            InitializeComponent();

            this.Resize += new EventHandler(hiddenForm_Resize);
        }

        // moves Form1 when Form2 is moved
        protected override void WndProc(ref Message message)
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

            base.WndProc(ref message);
        }

        // Resizes Video Overlay Form when hiddenForm is resized 
        private void hiddenForm_Resize(object sender, EventArgs e)
        {
            Form childForm = (Form)sender;

            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top;

            Owner.SetBounds(childForm.Location.X, childForm.Location.Y - titleHeight, childForm.Width, childForm.Height);
        }
    }
}
