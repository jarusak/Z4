using System;
using System.Collections.Generic;
using Z3.Model;
using System.Text;
using System.Windows.Forms;
using Z3.Util;

namespace Z3.View {
    public class TreeWrapper<T> : ItemContainer<T>, IDisposable where T : HierarchicalEntity
    {
        private TreeView _tree;
        private System.ComponentModel.Container components;
        private bool _disposed = false;

        /// <summary>
        /// Maps level and Container ids to TreeNodes.
        /// </summary>
        private Dictionary<int, Dictionary<int, TreeNode>> _maps;
        
        /// <summary>
        /// Holds nodes that have been added before their parents.
        /// </summary>
        private List<T> _holdq;
        
        /// <summary>
        /// Constructs a TreeWrapper given a TreeView.
        /// </summary>
        /// <param name="t">The TreeView to wrap.</param>
        public TreeWrapper(System.Windows.Forms.TreeView t, string category, string readable) {
            components = new System.ComponentModel.Container();
            InitializeComponent();

            string txt = readable;
            _tree = t;
            _tree.ContextMenuStrip = menuNoEntity;
            _tree.NodeMouseClick += new TreeNodeMouseClickEventHandler(_tree_NodeMouseClick);
            _tree.AfterSelect += new TreeViewEventHandler(_tree_AfterSelect);
            this.menuNoEntityNew.Text = "New " + txt + "...";
            this.menuEntityNew.Text = "New " + txt + "...";
            
            _holdq = new List<T>();
            _maps = new Dictionary<int, Dictionary<int, TreeNode>>();
        }

        void _tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (Selected != null)
                Selected(this, new EventArgs());
        }

        private TreeNode _contextTarget = null;
        void _tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            _contextTarget = e.Node;
        }

        public T ContextMenuSubject
        {
            get
            {
                if (_contextTarget == null) return default(T);
                return (T)(_contextTarget.Tag);
            }
        }

        public bool Disposed
        {
            get
            {
                return _disposed;
            }
        }

        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException("TreeWrapper");
            _disposed = true;

            foreach (Dictionary<int, TreeNode> t in _maps.Values)
            {
                t.Clear();
            }
            _maps.Clear();
            _maps = null;
            _holdq.Clear();
            _holdq = null;
            components.Dispose();
        }

        public void Update(T h)
        {
            if (_disposed) throw new ObjectDisposedException("TreeWrapper");
            _maps[h.TypeID][h.ID].Text = getDisplayedName(h);
        }

        /// <summary>
        /// Adds a Countable to the tree.
        /// </summary>
        /// <param name="c">The Countable to add.</param>
        public void Add(T c)
        {
            if (_disposed) throw new ObjectDisposedException("TreeWrapper");
            if (find(c) != null)
            {
                throw new InvalidOperationException("Object is already in tree");
            }

            TreeNodeCollection parent;
            if (c.Type.Root)
            {
                parent = _tree.Nodes;
            }
            else
            {
                if (find((T)c.Parent) == null)
                {
                    _holdq.Add(c);
                    return;
                }
                parent = _maps[c.Type.ParentID][c.ParentID].Nodes;
            }

            TreeNode n = parent.Add(getDisplayedName(c));
            n.ContextMenuStrip = menuEntity;
            n.Tag = c;
            _maps[c.Type.ID][c.ID] = n;

            // look for prospective children in the holdq
            //   clear out holdq and let them add themselves back if they
            //   can't find their parent
            //     (since we can't modify a List inside the foreach loop
            //     so we can't remove them while we iterate over it)
            if (_holdq.Count == 0) return;

            List<T> _h2 = _holdq;
            _holdq = new List<T>();
            foreach (T t in _h2)
                Add(t);
        }

        protected virtual string getDisplayedName(T c)
        {
            return c.Name;
        }

        //public void Refresh(T c) {
        //    TreeNode n = find(c);
        //    if (n == null)
        //        Add(c);
        //    else
        //        _maps[c.Type.ID][c.ID].Text = c.Name;
        //}

        public void Delete(T c) {
            if (_disposed) throw new ObjectDisposedException("TreeWrapper");
            Delete(c.Type.ID, c.ID);
        }

        private void Delete(int level, int entity) {
            if (_disposed) throw new ObjectDisposedException("TreeWrapper");
            if (!_maps.ContainsKey(level) || !_maps[level].ContainsKey(entity))
                return;

            TreeNode n = _maps[level][entity];
            _maps[level].Remove(entity);
            n.Remove();
        
            List<T> _h2 = _holdq;
            _holdq = new List<T>();

            foreach (T c in _h2) {
                if (c.Type.ID != level || c.ID != entity)
                    _holdq.Add(c);
            }
        }

        public void Clear() {
            if (_disposed) throw new ObjectDisposedException("TreeWrapper");
            _tree.Nodes.Clear();
            _maps.Clear();
            _holdq.Clear();
        }

        //public void BeginSetup() {
        //    Clear();
        //}

        //public void EndSetup() {
        //    if (_holdq.Count > 0)
        //        throw new System.Data.ConstraintException("Entities without parents exist in the database.");
        //}
        bool _programmaticUpdate = false;
        public T SelectedItem {
            get
            {
                if (_disposed) throw new ObjectDisposedException("TreeWrapper");
                if (_tree.SelectedNode == null) return default(T);
                return (T)(_tree.SelectedNode.Tag);
            }
            set
            {
                if (_disposed) throw new ObjectDisposedException("TreeWrapper");
                if (_programmaticUpdate) return;
                _programmaticUpdate = true;
                if (value == null) _tree.SelectedNode = null;
                else
                {
                        _tree.SelectedNode = _maps[value.TypeID][value.ID];
                }
                _programmaticUpdate = false;
            }
        }

        private TreeNode find(T c) {
            if (!_maps.ContainsKey(c.Type.ID))
                _maps[c.Type.ID] = new Dictionary<int, TreeNode>();
            if (!c.Type.Root && !_maps.ContainsKey(c.Type.ParentID))
                _maps[c.Type.ParentID] = new Dictionary<int, TreeNode>();

            if (!_maps[c.Type.ID].ContainsKey(c.ID))
                return null;
            else
                return _maps[c.Type.ID][c.ID];

        }


        #region Controls

        private void InitializeComponent()
        {

            this.menuEntity = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuEntityNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEntitySep0 = new System.Windows.Forms.ToolStripSeparator();
            this.menuEntityDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEntityProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNoEntity = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuNoEntityNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEntity.SuspendLayout();
            this.menuNoEntity.SuspendLayout();
            // 
            // menuNoEntity
            // 
            this.menuNoEntity.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNoEntityNew});
            this.menuNoEntity.Name = "EntityMenu";
            this.menuNoEntity.Size = new System.Drawing.Size(193, 26);
            // 
            // menuNoEntityNew
            // 
            this.menuNoEntityNew.Name = "menuNoEntityNew";
            this.menuNoEntityNew.Size = new System.Drawing.Size(192, 22);
            this.menuNoEntityNew.Text = "New...";
            this.menuNoEntityNew.Click += new EventHandler(New_DropDownItemClicked);
            // 
            // menuEntity
            // 
            this.menuEntity.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEntityNew,
            this.menuEntitySep0,
            this.menuEntityDelete,
            this.menuEntityProperties});
            this.menuEntity.Name = "EntityMenu";
            this.menuEntity.Size = new System.Drawing.Size(193, 76);
            // 
            // menuEntityNew
            // 
            this.menuEntityNew.Name = "menuEntityNew";
            this.menuEntityNew.Size = new System.Drawing.Size(192, 22);
            this.menuEntityNew.Text = "New...";
            this.menuEntityNew.Click += new EventHandler(New_DropDownItemClicked);
            // 
            // menuEntitySep0
            // 
            this.menuEntitySep0.Name = "menuEntitySep0";
            this.menuEntitySep0.Size = new System.Drawing.Size(189, 6);
            // 
            // menuEntityDelete
            // 
            this.menuEntityDelete.Name = "menuEntityDelete";
            this.menuEntityDelete.Size = new System.Drawing.Size(192, 22);
            this.menuEntityDelete.Text = "Delete";
            this.menuEntityDelete.Click += new EventHandler(Delete_DropDownItemClicked);
            // 
            // menuEntityProperties
            // 
            this.menuEntityProperties.Name = "menuEntityProperties";
            this.menuEntityProperties.Size = new System.Drawing.Size(192, 22);
            this.menuEntityProperties.Text = "Properties";
            this.menuEntityProperties.Click += new EventHandler(Properties_DropDownItemClicked);
            this.menuNoEntity.ResumeLayout(false);
            this.menuEntity.ResumeLayout(false);
        }

        void Properties_DropDownItemClicked(object sender, EventArgs e)
        {
            if (ItemEditClicked != null)
                ItemEditClicked(this, new EventArgs());
        }

        void Delete_DropDownItemClicked(object sender, EventArgs e)
        {
            if (ItemDeleteClicked != null)
                ItemDeleteClicked(this, new EventArgs());
        }

        void New_DropDownItemClicked(object sender, EventArgs e)
        {
            if (ItemNewClicked != null)
                ItemNewClicked(this, new EventArgs());
        }

        protected System.Windows.Forms.ContextMenuStrip menuNoEntity;
        private System.Windows.Forms.ToolStripMenuItem menuNoEntityNew;
        protected System.Windows.Forms.ContextMenuStrip menuEntity;
        private System.Windows.Forms.ToolStripMenuItem menuEntityNew;
        private System.Windows.Forms.ToolStripSeparator menuEntitySep0;
        private System.Windows.Forms.ToolStripMenuItem menuEntityDelete;
        private System.Windows.Forms.ToolStripMenuItem menuEntityProperties;

        #endregion

        public event EventHandler Selected;
        public event EventHandler ItemNewClicked;
        public event EventHandler ItemEditClicked;
        public event EventHandler ItemDeleteClicked;
    }

    public class SpeciesTreeWrapper : TreeWrapper<ZCountable>, CountableElements, ActiveCountableElements
    {
        public SpeciesTreeWrapper(TreeView t) : base(t, "Countable", "Countable") {
            InitializeComponent();

            this.ItemNewClicked += new EventHandler(SpeciesTreeWrapper_ItemNewClicked);
            this.ItemEditClicked += new EventHandler(SpeciesTreeWrapper_ItemEditClicked);
            this.ItemDeleteClicked += new EventHandler(SpeciesTreeWrapper_ItemDeleteClicked);
        }

        void SpeciesTreeWrapper_ItemDeleteClicked(object sender, EventArgs e)
        {
            if (CountableDeleteClicked != null)
                CountableDeleteClicked(sender, e);

        }

        void SpeciesTreeWrapper_ItemEditClicked(object sender, EventArgs e)
        {
            if (CountableEditClicked != null)
                CountableEditClicked(sender, e);
        }

        void SpeciesTreeWrapper_ItemNewClicked(object sender, EventArgs e)
        {
            if (CountableNewClicked != null)
                CountableNewClicked(sender, e);
        }

        protected override string getDisplayedName(ZCountable c)
        {
            if (Keyboard.IsMeaningful(c.Hotkey))
            {
                return base.getDisplayedName(c) + " (" + Keyboard.GetReadableKey(c.Hotkey) + ")";
            }
            else
            {
                return base.getDisplayedName(c);
            }
        }

        #region Controls

        private void InitializeComponent()
        {
            this.menuEntitySep0 = new System.Windows.Forms.ToolStripSeparator();
            this.menuEntityHotkey = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEntityClearHotkey = new System.Windows.Forms.ToolStripMenuItem();
            // 
            // menuEntity
            // 
            this.menuEntity.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEntitySep0,
            this.menuEntityHotkey,
            this.menuEntityClearHotkey});
            this.menuEntity.Name = "EntityMenu";
            this.menuEntity.Size = new System.Drawing.Size(193, 76);
            // 
            // menuEntitySep0
            // 
            this.menuEntitySep0.Name = "menuEntitySep0";
            this.menuEntitySep0.Size = new System.Drawing.Size(189, 6);
            //
            // menuEntityHotkey
            //
            this.menuEntityHotkey.Name = "menuEntityHotkey";
            this.menuEntityHotkey.Size = new System.Drawing.Size(192, 22);
            this.menuEntityHotkey.Text = "Assign Hotkey...";
            this.menuEntityHotkey.Click += new EventHandler(menuEntityHotkey_Click);
            //
            // menuEntityClearHotkey
            // 
            this.menuEntityClearHotkey.Name = "menuEntityClearHotkey";
            this.menuEntityClearHotkey.Size = new System.Drawing.Size(192, 22);
            this.menuEntityClearHotkey.Text = "Clear Hotkey";
            this.menuEntityClearHotkey.Click += new EventHandler(menuEntityClearHotkey_Click);
            this.menuEntity.ResumeLayout(false);
        }

        void menuEntityClearHotkey_Click(object sender, EventArgs e)
        {
            if (CountableClearHotkeyClicked != null)
                CountableClearHotkeyClicked(this, new EventArgs());
        }

        void menuEntityHotkey_Click(object sender, EventArgs e)
        {
            if (CountableAssignHotkeyClicked != null)
                CountableAssignHotkeyClicked(this, new EventArgs());
        }

        private System.Windows.Forms.ToolStripMenuItem menuEntityHotkey;
        private System.Windows.Forms.ToolStripMenuItem menuEntityClearHotkey;
        private System.Windows.Forms.ToolStripSeparator menuEntitySep0;
        #endregion

        #region CountableElements Members

        bool CountableElements.Enabled
        {
            set
            {
            }
        }

        public event EventHandler CountableNewClicked;

        #endregion

        #region ActiveCountableElements Members
        bool ActiveCountableElements.Enabled
        {
            set
            {
            }
        }
        public event EventHandler CountableAssignHotkeyClicked;
        public event EventHandler CountableClearHotkeyClicked;
        public event EventHandler CountableEditClicked;
        public event EventHandler CountableDeleteClicked;
        #endregion
    }
}
