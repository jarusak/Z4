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
    public partial class EntityFieldBox : Form
    {
        public EntityFieldBox()
        {
            InitializeComponent();
        }

        private ZField _value;

        public ZField Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                nameBox.Text = _value.Name;
                defaultBox.Text = _value.Default;
                
                int idx = _value.Type.IndexOf('(');
                if (idx < 0)
                {
                    typeBox.Text = _value.Type;
                }
                else
                {
                    typeBox.Text = _value.Type.Substring(0, idx);
                    typeLenBox.Text = _value.Type.Substring(idx + 1, (_value.Type.Length - idx - 2));
                }
                labelBox.Text = _value.Label;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            _value.Name = nameBox.Text;
            _value.Label = labelBox.Text;
            _value.Default = defaultBox.Text;
            _value.Type = typeBox.Text;
            if (sizable)
            {
                if (typeLenBox.Text.Length > 0)
                {
                    _value.Type = _value.Type + "(" + typeLenBox.Text + ")";
                }
            }
        }

        private void typeBox_TextChanged(object sender, EventArgs e)
        {

        }

        private bool sizable;

        private void typeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            sizable = (typeBox.Text.Equals("nvarchar"));
            typeLenBox.Enabled = sizable;
        }
    }
}