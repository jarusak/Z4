using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Z3.Forms {
    public partial class FrameSizeForm : Form {
        private int m_height;
        private int m_width;

        public FrameSizeForm() {
            InitializeComponent();
        }

        public int FrameHeight {
            get {
                return m_height;
            }
            set {
                m_height = value;

                txtHeight.Text = m_height.ToString();
            }
        }

        public int FrameWidth {
            get {
                return m_width;
            }
            set {
                m_width = value;

                txtWidth.Text = m_width.ToString();
            }
        }

        private void txtWidth_Validating(object sender, CancelEventArgs e) {
            try {
                m_width = Convert.ToInt32(txtWidth.Text);
            } catch (InvalidCastException) {
                txtWidth.Text = m_width.ToString();
            } catch (FormatException) {
                txtWidth.Text = m_width.ToString();
            }
        }

        private void txtHeight_Validating(object sender, CancelEventArgs e) {
            try {
                m_height = Convert.ToInt32(txtHeight.Text);
            } catch (InvalidCastException) {
                txtHeight.Text = m_height.ToString();
            } catch (FormatException) {
                txtHeight.Text = m_height.ToString();
            }
        }
    }
}