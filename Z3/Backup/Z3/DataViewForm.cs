using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Z3
{
    public partial class DataViewForm : Form
    {
        public DataViewForm()
        {
            InitializeComponent();
        }

        public ListView Data
        {
            get
            {
                return this.listView1;
            }
        }

        public void showError(string message)
        {
            this.errorBox.Visible = true;
            this.errorBox.Text = message;
        }

        public void hideError()
        {
            this.errorBox.Visible = false;
        }

        private void DataViewForm_Load(object sender, EventArgs e)
        {
            new Z3.View.Util.WindowSizeMemory(this);
        }
    }
}