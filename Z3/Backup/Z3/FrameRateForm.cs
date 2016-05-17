using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Z3.Forms {
    public partial class FrameRateForm : Form {
        private double m_framerate = 0;

        public FrameRateForm() {
            InitializeComponent();
        }

        public double FrameRate {
            get {
                return m_framerate;
            }
            set {
                m_framerate = value;

                txtRate.Text = value.ToString();
            }
        }

        private void txtRate_Validating(object sender, CancelEventArgs e) {
            try {
                m_framerate = Convert.ToDouble(txtRate.Text);
            } catch (InvalidCastException) {
                txtRate.Text = m_framerate.ToString();
            }
        }
    }
}