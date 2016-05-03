using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using Z3.View;
using Z3.Leger;
using Z3.Model;
using Z3.Workspace;

/*namespace Z3.View
{
    public class EntityBrowser : IDisposable
    {
        private bool _disposed = false;
        public EntityBrowser(TreeView tree)
        {
        //    _treeView = tree;
        //    _tree = new TreeWrapper(tree, menuOKEntity, menuEntity);
        //    _treeView.ContextMenuStrip = menuNoEntity;
        //    _treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.EntityTree_NodeMouseClick);
        //    _hk_cache = new Dictionary<char,ZCountable>();
            
            //TODO Subscribe to proper Store
            throw new NotImplementedException("todo");
        }

        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException("EntityBrowser");
            _disposed = true;

            components.Dispose();
        }

//        void EntityBrowser_EntitySelected(object sender, EventArgs e) { }

        private Dictionary<char, ZCountable> _hk_cache = new Dictionary<char, ZCountable>();
        public Dictionary<char, ZCountable> Hotkeys
        {
            get
            {
                return _hk_cache;
            }
        }
        
        //private void EntityTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
        //    _lastClicked = e.Node;
        //    if (!ExplicitSelection)
        //        if (((HierarchicalEntity)_lastClicked.Tag).Type.Measurable)
        //            _updsel();
        //}

        //public HierarchicalEntity SelectedItem {
        //    get { return _selected; }
        //}

        //private Dictionary<char, ZCountable> hk_init()
        //{
        //    Dictionary<char, ZCountable> hk = new Dictionary<char,ZCountable>();
        
        //    ZCountable zc = ZCountable.FactoryInstance;
        
        //    List<ZEntity> lz = zc.all();
        
        //    foreach (ZEntity ze in lz) //_ctlMgr.SpeciesTree.Nodes)
        //    {
        //        if( !ze.GetType().Name.Equals("ZCountable") )
        //            continue;
        //        ZCountable z = (ZCountable)ze;
        //        char key = Char.ToUpper(z.Hotkey);
        //        if(z.Hotkey != '\0' && !hk.ContainsKey(key))
        //            hk.Add(key, z);
        //    }
            
        //    return hk;
        //}

        //public bool ExplicitSelection {
        //    get { return _explicitSelection; }
        //    set { _explicitSelection = value; }
        //}

        //private bool _explicitSelection;
        private TreeView _treeView;
        private TreeWrapper _tree;
        private string _category;
        private IContainer components;
        private TreeNode _lastClicked;
        
        //public TreeWrapper Items
        //{
        //    get { return _tree; }
        //    set { _tree = value; }
        //}
        
        private void deleteToolStripMenuItem_Click(object sender, EventArgs _e)
        {
            HierarchicalEntity e = (HierarchicalEntity)_lastClicked.Tag;
            _tree.DeleteWithChildren(e);
            _lastClicked = null;
            
        //    if (_selected != null && _selected is ZCountable)
        //    {
        //        ZCountable z = (ZCountable)_selected;
        //        if (_hk_cache.ContainsKey(z.Hotkey))
        //            _hk_cache.Remove(z.Hotkey);
        //    }
        }
        
        private void menuNoEntityNew_Click(object sender, EventArgs e)
        {
            newEntity(false);
        }

        private void menuEntitySelect_Click(object sender, EventArgs e)
        {
            _tree.Select((HierarchicalEntity)_lastClicked.Tag);
            if (((HierarchicalEntity)_lastClicked.Tag).Type.Measurable)
                _updsel();
        }

        private void menuEntityNew_Click(object sender, EventArgs e)
        {
            newEntity(true);
        }

        private void menuEntityEdit_Click(object sender, EventArgs _e)
        {
            HierarchicalEntity e = (HierarchicalEntity)_lastClicked.Tag;
            using (EditForm f = new EditForm(e))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    _tree.Refresh(e);
                    this.Select(e);
                    if (_selected != null && _selected.GetType().Name.Equals("ZCountable"))
                    {
                        ZCountable old = (ZCountable)_selected;

                        if (old.Hotkey != f.Hotkey) //hotkey has changed
                        {
                            if (f.Hotkey == 0) // it's not a hotkey
                            {
                                if (_hk_cache.ContainsKey(old.Hotkey))
                                    _hk_cache.Remove(old.Hotkey);
                            }
                            else
                            {
                                if (_hk_cache.ContainsKey(f.Hotkey))
                                {
                                    MessageBox.Show("Unable to change hotkey: The requested hotkey is already in use.");
                                    f.Hotkey = old.Hotkey;
                                    f.Save();
                                }
                                else
                                {
                                    _hk_cache.Add(f.Hotkey, f.Countable);
                                    if (_hk_cache.ContainsKey(old.Hotkey))
                                        _hk_cache.Remove(old.Hotkey);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void newEntity(bool intendedParent)
        {
            using (NewEntityForm nf = new NewEntityForm(_ws, _category))
            {
                if (intendedParent && !((HierarchicalEntity)_lastClicked.Tag).Type.Final)
                {
                    nf.TypeID = ((HierarchicalEntity)_lastClicked.Tag).Type.ChildID;
                    nf.ParentItemID = ((HierarchicalEntity)_lastClicked.Tag).ID;
                }
                if (nf.ShowDialog() == DialogResult.OK)
                {
                    //create new Entity
                    HierarchicalEntity c;
                    //ZEntity c = ZEntity.createNew(_category, nf.Type, (ZEntity)nf.ParentItem);
                    //TODO Create new entity
                    throw new NotImplementedException("todo");

                    //launch edit form
                    using (EditForm ef = new EditForm(c))
                    {
                        if (ef.ShowDialog() == DialogResult.OK)
                        {
                            _tree.Add(c);
                            if (c.Type.Measurable)
                            {
                                this.Select(c);
                            }

                            if (_selected != null && _selected.GetType().Name.Equals("ZCountable") && ef.Hotkey > 0)
                            {
                                if (_hk_cache.ContainsKey(ef.Hotkey))
                                {
                                    MessageBox.Show("Error when adding item: Specified hotkey is already in use.");
                                    ef.Hotkey = '\0';
                                    ef.Save();
                                }
                                else
                                {
                                    _hk_cache.Add(ef.Hotkey, ef.Countable);
                                }
                            }
                        }
                        else
                            c.Delete(); //if cancel delete Entity
                    }
                }
            }
        }

        public void Create() { newEntity(false); }

        public void Edit()
        {

            if (_lastClicked == null)
                _lastClicked = _treeView.SelectedNode;
            if (_lastClicked == null)
                MessageBox.Show("Cannot edit anything because no item is selected.", "Oops");
            else
            {
                menuEntityEdit_Click(null, null);
                //update hk_cache for Countables
                    //.
                    if (_selected != null && _selected.GetType().Name.Equals("ZCountable") )
                    {
                        ZCountable rem_z = (ZCountable)_selected;
                        char rem_key = Char.ToUpper(rem_z.Hotkey);
                        ZCountable z = (ZCountable)_selected;
                        char key = Char.ToUpper(z.Hotkey);
                        _hk_cache.Remove(rem_key);
                        _hk_cache.Add(key, z);
                    }
                    .//
            }
        }

        public void Delete()
        {
            if (_lastClicked == null)
                _lastClicked = _treeView.SelectedNode;
            if (_lastClicked == null)
                MessageBox.Show("Cannot delete anything because no item is selected.", "Oops");
            else
            {
                deleteToolStripMenuItem_Click(null, null);
                //update hk_cache for Countables
                if (_selected != null && _selected.GetType().Name.Equals("ZCountable"))
                {
                    ZCountable z = (ZCountable)_selected;
                    char key = Char.ToUpper(z.Hotkey);
                    _hk_cache.Remove(key);
                }
            }
            _lastClicked = null;
        }

        public void SetItems(List<HierarchicalEntity> cs)
        {
            try
            {
                _tree.BeginSetup();
                foreach (HierarchicalEntity c in cs)
                {
                    _tree.Add(c);
                }
                _tree.EndSetup();
            }
            catch (System.Data.ConstraintException ex)
            {
                if (MessageBox.Show("There are consistency errors in your workspace.\n\n" + ex.Message + "\n\nThis error is probably not fatal.  Would you like to use this file anyway?", "Ignore workspace errors?", MessageBoxButtons.YesNoCancel) == DialogResult.No)
                    throw ex;
            }
        }

        internal void Add()
        {
            newEntity(_lastClicked != null);
        }

        internal void Clear()
        {
            _tree.Clear();
        }
        
        internal void Select() {
            if (_lastClicked == null)
                _lastClicked = _treeView.SelectedNode;
            if (_lastClicked == null)
                MessageBox.Show("You haven't selected an item from the tree.");
            else if (!((HierarchicalEntity)_lastClicked.Tag).Type.Measurable)
                MessageBox.Show("That item is not a valid selection.  You cannot select a " + ((HierarchicalEntity)_lastClicked.Tag).Type.Name + ".");
            else
                _updsel();
        }

        internal void Select(Object o) {
            if (o != null)
            {
                _tree.Select((HierarchicalEntity)o);
                _selected = (HierarchicalEntity)o;
                EntitySelected(this, new EventArgs());
            }
        }

        private void _updsel() {
            _selected = ((HierarchicalEntity)_lastClicked.Tag);
            EntitySelected(this, new EventArgs());
        }

        private HierarchicalEntity _selected = null;
        public HierarchicalEntity Countable {
            get { return _selected; }
        }
        
        public event EventHandler EntitySelected;

        
        //void EntityWatcher.LoadBase(IEnumerable<HierarchicalEntity> e) {
        //    Clear();
        //    foreach (HierarchicalEntity z in e)
        //        _tree.Add(z);
        //}

        //void EntityWatcher.EntityAdded(HierarchicalEntity e) {
        //    _tree.Add(e);
        //}

        //void EntityWatcher.EntityDeleted(HierarchicalEntity e) {
        //    _tree.Remove(e);
        //    removeHotkeysFor(e);
        //    if (_lastClicked == null || e == _lastClicked.Tag || _treeView.Nodes.Count == 0)
        //        _lastClicked = null;
        //}

        //void EntityWatcher.EntityEdited(HierarchicalEntity e) {
        //    _tree.Refresh(e);
        //    removeHotkeysFor(e);
        //    addHotkeyFor(e);
        //}

        
        void removeHotkeysFor(HierarchicalEntity e)
        {
            if (!(e is ZCountable)) return;
            while (_hk_cache.ContainsValue((ZCountable)e))
            {
                foreach (char c in _hk_cache.Keys)
                {
                    if (_hk_cache[c].Equals(e))
                    {
                        _hk_cache.Remove(c);
                        break;
                    }
                }
            }
        }

        void addHotkeyFor(HierarchicalEntity e)
        {
            if (!(e is ZCountable)) return;
            ZCountable z = (ZCountable)e;
            if (_hk_cache.ContainsKey(z.Hotkey))
            {
                if (MessageBox.Show("The hotkey " + z.Hotkey + " is already assigned to " + _hk_cache[z.Hotkey].Name + ".  Do you want to assign it to " + e.Name + " instead?") == DialogResult.Yes)
                {
                    ZCountable old = _hk_cache[z.Hotkey];
                    _hk_cache[z.Hotkey] = z;
                    old.Hotkey = '\0';
                }
            }
        }
    }
}*/