using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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

                if (_value.Type.Equals("combobox"))
                {
                    foreach (String param in _value.AllComboVals)
                    {
                        Debug.WriteLine("This is param: " + param);
                        listBox1.Items.Add(param);
                    }
                }

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
            _value.Name = nameBox.Text.Replace(" ","");
            _value.Label = labelBox.Text.Replace(" ", "");
            _value.Default = defaultBox.Text;
            _value.Type = typeBox.Text;
            _value.AddedComboVals = new List<String>(listBox2.Items.Cast<String>());
            
            if(_value.AllComboVals == null)
            {
                _value.AllComboVals = new List<String>(listBox2.Items.Cast<String>());
            } else
            {
                _value.AllComboVals.AddRange(new List<String>(listBox2.Items.Cast<String>()));
            }
                
            listBox2.Items.Clear();

            if (sizable)
            {
                if (typeLenBox.Text.Length > 0)
                {
                    _value.Type = _value.Type + "(" + typeLenBox.Text + ")";
                }
            }
        }

        private bool sizable;
        private bool showPanel;
       
        private void typeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            sizable = (typeBox.Text.Equals("nvarchar"));
            typeLenBox.Enabled = sizable;
            showPanel = (typeBox.Text.Equals("combobox"));
            panel1.Enabled = showPanel;
            listBox1.Enabled = showPanel;
            listBox1.BackColor = System.Drawing.SystemColors.Control;
            listBox2.Enabled = showPanel;
            listBox2.BackColor = System.Drawing.SystemColors.Control;

            if (showPanel)
            {
                defaultBox.Enabled = false;
                listBox1.BackColor = System.Drawing.SystemColors.Window;
                listBox2.BackColor = System.Drawing.SystemColors.Window;
            } 
        }
        private void AddButton_Click(object sender, EventArgs e)
        {
            // only adds value if the value has not already been added
            if (_value.AllComboVals != null && _value.AllComboVals.Contains(textBox1.Text))
            {
                MessageBox.Show("Value has already been added\n\n");
            }
            else if (listBox2.Items.Contains(textBox1.Text))
            {
                MessageBox.Show("Value has already been added\n\n");
            }
            else
            {
                listBox2.Items.Add(textBox1.Text);
            }
            
            // clears the text in the text box
            textBox1.Clear();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                // Delete from database
                _value.Level.DeleteComboBoxValue(_value.Name, listBox1.SelectedItem.ToString());

                // Delete from view
                _value.AllComboVals.Remove(listBox1.SelectedItem.ToString());
                listBox1.Items.Remove(listBox1.SelectedItem);
                listBox1.Refresh();
            }
            if (listBox2.SelectedItem != null)
            {
                // Delete from view
                listBox2.Items.Remove(listBox2.SelectedItem);
                listBox2.Refresh();
            }
        }
    }
}