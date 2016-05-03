using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.Leger;
using Z3.Model;

namespace Z3 {
    public partial class NewCountableForm : Form {
        private Dictionary<int, Z3Level> _items;
        private Dictionary<int, Z3Entity> _parents;
        private bool root;

        public NewCountableForm() {
            InitializeComponent();

            List<Z3Level> levels = Z3Schema.getInstance().getCountableLevels();
            _items = new Dictionary<int, Z3Level>();
            _parents = new Dictionary<int, Z3Entity>();
            foreach (Z3Level l in levels) {
                _items[l.ID] = l;
                typeBox.Items.Add(l);
            }

            // populate first choice
            if (_items.Count == 0) return;
            typeBox.SelectedIndex = 0;
        }

        public int TypeID {
            get {
                return ((Z3Level)typeBox.SelectedItem).ID;
            }
            set {
                typeBox.SelectedItem = _items[value];
            }
        }

        public Z3Level Type {
            get {
                return ((Z3Level)typeBox.SelectedItem);
            }
        }

        public int ParentItemID {
            get {
                if (root) return 0;
                else return ((Z3Entity)parentBox.SelectedItem).ID;
            }
            set {
                if (root) return;
                parentBox.SelectedItem = _parents[value];
            }
        }

        public Z3Entity ParentItem {
            get {
                if (root) return null;
                else return ((Z3Entity)parentBox.SelectedItem);
            }
        }
        
        private void typeBox_SelectedIndexChanged(object sender, EventArgs e) {
            parentBox.Items.Clear();
            _parents.Clear();

            root = ((Z3Level)typeBox.SelectedItem).Root;
            parentBox.Enabled = !root;
            if (root) return;

            List<Z3Entity> _aparents = Z3Schema.getInstance().getBaseCountablesForLevel(((Z3Level)typeBox.SelectedItem).Parent);

            foreach (Z3Entity ze in _aparents) {
                parentBox.Items.Add(ze);
                _parents[ze.ID] = ze;
            }

            if (parentBox.Items.Count == 0) return;
            parentBox.SelectedIndex = 0;
        }
    }
}
