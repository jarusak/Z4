using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.Model;

namespace Z3SchemaEditor
{
    public partial class EntityTypeBox : Form
    {
        public EntityTypeBox()
        {
            InitializeComponent();
        }

        public ZLevel Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                Debug.WriteLine("Value: " + _value);
                refreshFields();
            }
        }

        private ZLevel _value;

        private void refreshFields()
        {
            nameBox.Text = _value.Name;
            measurableBox.Checked = _value.Measurable;

            fieldsList.Items.Clear();
            foreach (ZField f in _value.Fields)
            {
                ListViewItem i = fieldsList.Items.Add(f.Name);
                i.Tag = f;
            }
        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {
            _value.Name = nameBox.Text;
        }

        private void measurableBox_CheckedChanged(object sender, EventArgs e)
        {
            _value.Measurable = measurableBox.Checked;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (fieldsList.SelectedItems.Count > 0)
            {
                using (EntityFieldBox f = new EntityFieldBox())
                {
                    f.Value = ((ZField)fieldsList.SelectedItems[0].Tag);
                    f.ShowDialog();
                    Value.SaveField(f.Value);
                }

                refreshFields();
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            nameBox_TextChanged(null, null);
            measurableBox_CheckedChanged(null, null);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            Value.AddField("NewField");
            refreshFields();
        }

        private void delButton_Click(object sender, EventArgs e)
        {
            if (fieldsList.SelectedItems.Count > 0)
            {
                Value.DeleteField((ZField)fieldsList.SelectedItems[0].Tag);

                refreshFields();
            }
        }
    }
}