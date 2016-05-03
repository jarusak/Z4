using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.Workspace;

namespace Z3.Forms {
    public partial class BrowserForm : Form {
        public BrowserForm() {
            InitializeComponent();
        }
        
        public TreeView Tree {
            get { return treeView; }
        }
    }
}