using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Z3.Forms {
    public partial class OptionsForm : Form {
        private Graphics g;

        private Boolean m_invert = false;
        private Color m_activecolor = Color.Black;
        private Color m_pastcolor = Color.Blue;
        private int m_dotsize = 5;
        private int m_roundingplaces = 5;

        public OptionsForm() {
            InitializeComponent();
            g = pnlDot.CreateGraphics();
        }

        public Boolean InvertButtons {
            get {
                m_invert = optInvert.Checked;

                return m_invert;
            }
            set {
                m_invert = value;

                optNormal.Checked = !value;
                optInvert.Checked = value;
            }
        }

        public Color ActiveColor {
            get {
                return m_activecolor;
            }
            set {
                m_activecolor = value;

                btnActive.BackColor = value;
            }
        }

        public Color PastColor {
            get {
                return btnOld.BackColor;
            }
            set {
                btnOld.BackColor = value;
            }
        }

        public int DotSize {
            get {
                return m_dotsize;
            }
            set {
                m_dotsize = value;

                txtDotSize.Text = value.ToString();
            }
        }

        public int RoundingPlaces {
            get {
                return m_roundingplaces;
            }
            set {
                m_roundingplaces = value;

                txtPlaces.Text = value.ToString();
            }
        }

        private void txtDotSize_Validating(object sender, CancelEventArgs e) {
            try {
                m_dotsize = Convert.ToInt32(txtDotSize.Text);
            } catch (FormatException) {
                txtDotSize.Text = m_dotsize.ToString();
            }

            g.Clear(Control.DefaultBackColor);
            g.FillEllipse(Brushes.Black, (g.VisibleClipBounds.Width - m_dotsize) / 2, (g.VisibleClipBounds.Height - m_dotsize) / 2, m_dotsize, m_dotsize);
        }

        private void txtPlaces_Validating(object sender, CancelEventArgs e) {
            try {
                m_roundingplaces = Convert.ToInt32(txtPlaces.Text);
            } catch (FormatException) {
                txtPlaces.Text = m_roundingplaces.ToString();
            }
        }

        private void btnActive_Click(object sender, EventArgs e) {
            dlgColorPicker.Color = btnActive.BackColor;
            if (dlgColorPicker.ShowDialog() == DialogResult.OK) {
                btnActive.BackColor = dlgColorPicker.Color;
                m_activecolor = dlgColorPicker.Color;
            }
        }

        private void btnOld_Click(object sender, EventArgs e) {
            dlgColorPicker.Color = btnOld.BackColor;
            if (dlgColorPicker.ShowDialog() == DialogResult.OK) {
                btnOld.BackColor = dlgColorPicker.Color;
                m_pastcolor = dlgColorPicker.Color;
            }
        }

        private void OptionsForm_FormClosing(object sender, FormClosingEventArgs e) {
            g.Dispose();
        }

        private void OptionsForm_Shown(object sender, EventArgs e) {
            txtDotSize_TextChanged(sender, e);
        }

        private void txtDotSize_TextChanged(object sender, EventArgs e) {
            try {
                m_dotsize = Convert.ToInt32(txtDotSize.Text);
                g.Clear(Control.DefaultBackColor);
                g.FillEllipse(Brushes.Black, (g.VisibleClipBounds.Width - m_dotsize) / 2, (g.VisibleClipBounds.Height - m_dotsize) / 2, m_dotsize, m_dotsize);
            } catch (FormatException) { }
        }
    }
}