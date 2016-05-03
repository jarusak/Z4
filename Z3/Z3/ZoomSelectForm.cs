using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Z3.View.Video {
    public partial class ZoomSelectForm : Form {
        private Boolean errorCondition;
        public ZoomSelectForm() {
            InitializeComponent();
        }

        public int CurrentZoomFactor {
            get {
                return Convert.ToInt32(lblCurrentZoomValue.Text);
            }
            set {
                lblCurrentZoomValue.Text = value.ToString();
            }
        }

        public int NewZoomFactor {
            get {
                return Convert.ToInt32(cboZoom.Text);
            }
            set {
                cboZoom.Text = value.ToString();
            }
        }

        private void cboZoom_Validating(object sender, CancelEventArgs e) {
            try {
                Convert.ToInt32(cboZoom.Text);
                if (Convert.ToInt32(cboZoom.Text) < 1) {
                    errorCondition = true;
                    cboZoom.Text = lblCurrentZoomValue.Text;
                } else {
                    errorCondition = false;
                }
            } catch (FormatException) {
                cboZoom.Text = lblCurrentZoomValue.Text;
                errorCondition = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e) {
            if (errorCondition)
                MessageBox.Show("The zoom factor must be an integer.  Unable to change zoom.");
        }
    }
}