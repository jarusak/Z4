using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.Model;

namespace Z3SchemaEditor
{
    public partial class MTypeForm : Form
    {
        public MTypeForm()
        {
            InitializeComponent();
        }

        private void countBox_CheckedChanged(object sender, EventArgs e)
        {
            countTypeBox.Enabled = !countBox.Checked;
        }

        private void autoBox_CheckedChanged(object sender, EventArgs e)
        {
            autoTypeBox.Enabled = autoBox.Checked;
        }

        private ZMeasurement _value;
        public ZMeasurement Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                nameBox.Text = value.Name;
                measureBox.Checked = value.Measured;
                autoBox.Checked = value.AutoCount;
                autoTypeBox.Text = value.AutoTypeID.ToString();
                countBox.Checked = value.Counted;
                countTypeBox.Text = value.CountTypeID.ToString();
                isDefaultBox.Checked = value.Default;
                displayBox.Checked = value.Displayed;
                hotkeyBox.Text = ((int)value.Hotkey).ToString();
                idBox.Text = value.ID.ToString();
                incrementBox.Checked = value.Increment;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Value.Name = nameBox.Text;
            Value.Measured = measureBox.Checked;
            Value.AutoCount = autoBox.Checked;
            Value.AutoTypeID = getInteger(autoTypeBox.Text);
            Value.Counted = countBox.Checked;
            Value.CountTypeID = getInteger(countTypeBox.Text);
            Value.Default = isDefaultBox.Checked;
            Value.Displayed = displayBox.Checked;
            Value.Hotkey = (Keys)getInteger(hotkeyBox.Text);
            Value.Increment = incrementBox.Checked;
        }

        private int getInteger(string value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch (ArgumentException)
            {
                return 0;
            }
        }
    }
}