using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Z3SchemaEditor
{
    public partial class PropertyForm : Form
    {
        public PropertyForm()
        {
            InitializeComponent();
        }

        public string PropertyName
        {
            get
            {
                return nameBox.Text;
            }
            set
            {
                nameBox.Text = value;
            }
        }

        public string PropertyValue
        {
            get
            {
                return valueBox.Text;
            }
            set
            {
                valueBox.Text = value;
            }
        }

        public bool Rename
        {
            get
            {
                return nameBox.Enabled;
            }
            set
            {
                nameBox.Enabled = value;
            }
        }
    }
}