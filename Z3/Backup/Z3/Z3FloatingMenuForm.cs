using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.Model;

namespace Z3.View.Floating {
    internal partial class Z3FloatingMenuForm : Form {
        public Z3FloatingMenuForm() {
            InitializeComponent();
            if (components == null)
                components = new System.ComponentModel.Container();

            this.Load += new EventHandler(Z3FloatingMenuForm_Load);
        }

        void Z3FloatingMenuForm_Load(object sender, EventArgs e)
        {
            new Z3.View.Util.WindowSizeMemory(this);
        }
    }
}