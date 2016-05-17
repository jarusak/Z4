using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Z3.View.Floating {
    public partial class Z3FloatingControlsForm : Form {
        public Z3FloatingControlsForm() {
            InitializeComponent();

            this.Load += new EventHandler(Z3FloatingControlsForm_Load);
        }

        void Z3FloatingControlsForm_Load(object sender, EventArgs e)
        {
            new Z3.View.Util.WindowSizeMemory(this);
        }

        public Panel TopHalf {
            get { return splitContainer1.Panel1; }
        }

        public Panel BottomHalf {
            get { return splitContainer1.Panel2; }
        }
    }
}