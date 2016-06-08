using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.Model;
using Z3.Workspace;

namespace Z3.Forms {
    public partial class NewEntityForm : Form {
        private Dictionary<int, ZLevel> _items;
        private Dictionary<int, HierarchicalEntity> _parents;
        private bool _root;
        private bool _recursing;
        private string _table;
        private IWorkspace _ws;

        public NewEntityForm(IWorkspace workspace, string table) {
            InitializeComponent();
            _table = table;
            _ws = workspace;
            
            ICollection<ZLevel> levels = _ws.Levels.Types(table);
            _items = new Dictionary<int, ZLevel>();
            _parents = new Dictionary<int, HierarchicalEntity>();
            foreach (ZLevel l in levels) {
                _items[l.ID] = l;
                typeBox.Items.Add(l);
            }

            // populate first choice
            if (_items.Count == 0) return;
            typeBox.SelectedIndex = 0;

            this.Text = "New " + table.Substring(0, 1).ToUpper() + table.Substring(1);
        }

        public int TypeID {
            get {
                return ((ZLevel)typeBox.SelectedItem).ID;
            }
            set {
                typeBox.SelectedItem = _items[value];
            }
        }

        public ZLevel Type {
            get {
                return ((ZLevel)typeBox.SelectedItem);
            }
        }

        public int ParentItemID {
            get {
                if (_root) return 0;
                else return ((HierarchicalEntity)parentBox.SelectedItem).ID;
            }
            set {
                if (_root) return;
                parentBox.SelectedItem = _parents[value];
            }
        }

        public HierarchicalEntity ParentItem {
            get {
                if (_root) return null;
                else return ((HierarchicalEntity)parentBox.SelectedItem);
            }
        }
        
        private void typeBox_SelectedIndexChanged(object sender, EventArgs e) {
            parentBox.Items.Clear();
            _parents.Clear();

            ZLevel _curlev = ((ZLevel)typeBox.SelectedItem);
            _root = _curlev.Root;
            parentBox.Enabled = !_root;
            if (!_root) {
                //TODO Load Parents with new data model: Does This Work?
                List<HierarchicalEntity> _aparents = _ws.Store(_table).AllFromLevel(_curlev.Parent);
                //= ZSchema.getInstance().getBaseEntitiesForLevel(_table, _curlev.Parent);

                if (_aparents.Count == 0) {
                    if (!_recursing) MessageBox.Show("You can not create a " + _curlev.Name + " until there is at least one " + _curlev.Parent.Name + ".");
                    _recursing = true;
                    typeBox.SelectedIndex--;
                } else {
                    foreach (HierarchicalEntity ze in _aparents) {
                        parentBox.Items.Add(ze);
                        _parents[ze.ID] = ze;
                    }

                    if (parentBox.Items.Count == 0) return;
                    parentBox.SelectedIndex = 0;
                }
            }
            _recursing = false;
        }
    }
}
