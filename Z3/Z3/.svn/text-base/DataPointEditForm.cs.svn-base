using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Z3.Forms
{
    public partial class DataPointEditForm : Form
    {
        public DataPointEditForm()
        {
            InitializeComponent();
        }

        public string Type
        {
            set
            {
                typeBox.Text = value;
            }
        }

        private double _value;
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                valueBox.Text = value.ToString();
                _value = value;
            }
        }

        private void valueBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                _value = Convert.ToDouble(valueBox.Text);
            }
            catch (InvalidCastException)
            {
                valueBox.Text = _value.ToString();
                e.Cancel = true;
            }
        }


    }
}