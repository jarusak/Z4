using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.Model;

namespace Z3.Forms {
    public partial class EditForm : Form {
        private HierarchicalEntity _e;
        private Dictionary<ZField, ZFieldValue> values;
        private ZFieldValue _hotkey;
        private DataGridViewComboBoxCell cmb = new DataGridViewComboBoxCell();

        public char Hotkey {
            get {
                if (_hotkey != null)
                {
                    return Convert.ToChar(_hotkey.DatabaseValue);
                }
                else
                {
                    return (char)0;
                }
            }
            set {
                if (_hotkey != null)
                {
                    _hotkey.Value = Convert.ToInt32(value);
                }
            }
        }

        public ZCountable Countable { get { return (ZCountable)_e; } }

        public EditForm() {
            InitializeComponent();
        }

        public EditForm(HierarchicalEntity e)
            : this() {
            _e = e;
            values = e.GetValues();
            int columnNum = 0;
            
            foreach (ZFieldValue v in values.Values) {
                object[] row = { v, v.Field.Name, v.ReadableValue };
                Debug.WriteLine(v.ReadableValue);
                if (v.Field.Type.Equals("combobox"))
                {
                    cmb.DataSource = e.GetComboVals(v.Field.Name); ;
                    cmb.Value = v.ReadableValue;           
                    properties.Rows.Add(row);
                    properties[2,columnNum] = cmb;
                } else
                {
                    properties.Rows.Add(row);
                }
               
                if (v.Field.Name.ToLower().Equals("hotkey"))
                    _hotkey = v;
                columnNum++;
            }
        }

        private void btnOK_Click(object sender, EventArgs e) {
            foreach (DataGridViewRow r in properties.Rows) {
                if (r.Cells[0].Value != null) {
                    ZFieldValue v = ((ZFieldValue)r.Cells[0].Value);
                    v.Value = r.Cells[2].Value;
                }
            }

            Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public void Save()
        {
            try
            {
                _e.SetValues(values);
            }
            catch (System.Data.SqlServerCe.SqlCeException ex)
            {
                MessageBox.Show("Could not update the record.\n\n" + ex.Message);
                return;
            }
        }
    }
}