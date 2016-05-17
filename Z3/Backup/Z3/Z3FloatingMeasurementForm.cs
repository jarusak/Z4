using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Z3.View.Floating {
    public partial class Z3FloatingMeasurementForm : Form {
        public Z3FloatingMeasurementForm() {
            InitializeComponent();

            this.Load += new EventHandler(Z3FloatingMeasurementForm_Load);
            
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