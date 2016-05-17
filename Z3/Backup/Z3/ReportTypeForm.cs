using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.Workspace;

namespace Z3.Forms
{
    public partial class ReportTypeForm : Form
    {
        public ReportTypeForm(IWorkspace ws)
        {
            InitializeComponent();
            _ws = ws;
        }

        private IWorkspace _ws;
        private bool _new;
        private ReportType _value;

        internal ReportType Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public ReportType ShowNewDialog()
        {
            _new = true;

            if (ShowDialog() == DialogResult.OK)
                return Value;
            else
                return null;
        }

        public DialogResult ShowEditDialog(ReportType v)
        {
            _new = false;
            _value = v;
            nameBox.Text = v.Name;
            queryBox.Text = v.Query;
            return ShowDialog();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (_new)
                Value = _ws.ReportTypes.Create();

            Value.Name = nameBox.Text;
            Value.Query = queryBox.Text;

            _ws.ReportTypes.Save(Value);
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}