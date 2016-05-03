using System;
using System.Collections.Generic;
using Z3.Model;
using System.Text;
using System.Windows.Forms;

namespace Z3.View {
    public class Z3TreeWrapper {
        /// <summary>
        /// The TreeView being wrapped.
        /// </summary>
        private TreeView _tree;
        private ContextMenuStrip _cmenu;
        private ContextMenuStrip _cmenu2;

        /// <summary>
        /// Maps level and container ids to TreeNodes.
        /// </summary>
        private Dictionary<int, Dictionary<int, TreeNode>> _maps;
        
        /// <summary>
        /// Holds nodes that have been added before their parents.
        /// </summary>
        private List<Z3Entity> _holdq;
        
        /// <summary>
        /// Constructs a TreeWrapper given a TreeView.
        /// </summary>
        /// <param name="t">The TreeView to wrap.</param>
        public Z3TreeWrapper(System.Windows.Forms.TreeView t, ContextMenuStrip c, ContextMenuStrip c2) {
            _tree = t;
            _cmenu = c;
            _cmenu2 = c2;
            _holdq = new List<Z3Entity>();
            _maps = new Dictionary<int, Dictionary<int, TreeNode>>();
        }



        /// <summary>
        /// Adds a Countable to the tree.
        /// </summary>
        /// <param name="c">The Countable to add.</param>
        public void Add(Z3Entity c) {
            TreeNodeCollection parent = null;

            if (find(c) != null) return;
                // Already in the tree!

            // Find parent
            if (c.Type.Root) {
                parent = _tree.Nodes;
            } else {
                try {
                    parent = _maps[c.Type.ParentID][c.ParentID].Nodes;
                } catch (KeyNotFoundException) {
                    parent = null;
                }
            }

            if (parent == null) {
                // if parent not present add to holding q
                _holdq.Add(c);
            } else {
                // if parent is present create node and add to tree below parent
                TreeNode n = parent.Add(c.Name);
                if (c.Type.Measurable) {
                    n.ContextMenuStrip = _cmenu;
                } else {
                    n.ContextMenuStrip = _cmenu2;
                }
                n.Tag = c;
                _maps[c.Type.ID][c.ID] = n;

                // look for prospective children in the holdq
                //   clear out holdq and let them add themselves back if they
                //   can't find their parent
                //     (since we can't modify a List inside the foreach loop
                //     so we can't remove them while we iterate over it)
                if (_holdq.Count == 0) return;

                List<Z3Entity> _h2 = _holdq;
                _holdq = new List<Z3Entity>();
                foreach (Z3Entity t in _h2)
                    Add(t);
            }
        }

        public void Refresh(Z3Entity c) {
            TreeNode n = find(c);
            if (n == null)
                Add(c);
            else
                _maps[c.Type.ID][c.ID].Text = c.Name;
        }

        public void Remove(Z3Entity c) {
            Remove(c.Type.ID, c.ID);
        }

        public void Remove(int level, int entity) {
            if (!_maps.ContainsKey(level) || !_maps[level].ContainsKey(entity))
                return;

            TreeNode n = _maps[level][entity];
            _maps[level].Remove(entity);
            n.Remove();
        
            List<Z3Entity> _h2 = _holdq;
            _holdq = new List<Z3Entity>();

            foreach (Z3Entity c in _h2) {
                if (c.Type.ID != level || c.ID != entity)
                    _holdq.Add(c);
            }
        }

        public void Clear() {
            _tree.Nodes.Clear();
            _maps.Clear();
            _holdq.Clear();
        }

        public void BeginSetup() {
            Clear();
        }

        public void EndSetup() {
            if (_holdq.Count > 0)
                throw new System.Data.ConstraintException("Entities without parents exist in the database.");
        }

        public void Select(Z3Entity z) {
            if (_maps.ContainsKey(z.Type.ID) && _maps[z.Type.ID].ContainsKey(z.ID))
                _tree.SelectedNode = _maps[z.Type.ID][z.ID];
        }

        public void DeleteWithChildren(Z3Entity e) {
            TreeNode n = find(e);
            if (n == null) return;
            DeleteWithChildrenHelper(n);
        }

        private void DeleteWithChildrenHelper(TreeNode n) {
            ((Z3Entity)n.Tag).Delete();
            foreach (TreeNode t in n.Nodes)
                DeleteWithChildrenHelper(t);
            Remove((Z3Entity)n.Tag);
        }

        private TreeNode find(Z3Entity c) {
            if (!_maps.ContainsKey(c.Type.ID))
                _maps[c.Type.ID] = new Dictionary<int, TreeNode>();
            if (!c.Type.Root && !_maps.ContainsKey(c.Type.ParentID))
                _maps[c.Type.ParentID] = new Dictionary<int, TreeNode>();

            if (!_maps[c.Type.ID].ContainsKey(c.ID))
                return null;
            else
                return _maps[c.Type.ID][c.ID];

        }
    }
}
