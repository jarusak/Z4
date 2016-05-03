using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Z3.Forms
{
    public partial class ImportForm : Form
    {
        public ImportForm()
        {
            InitializeComponent();
        }

        public bool ImportTaxonomy
        {
            get
            {
                return taxonomyBox.Checked;
            }
            set
            {
                taxonomyBox.Checked = value;
            }
        }

        public bool ImportReports
        {
            get
            {
                return reportBox.Checked;
            }
            set
            {
                reportBox.Checked = value;
            }
        }


    }
}