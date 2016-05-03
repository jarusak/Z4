//=============================================================================
// Z2 Species Tree
// A "wrapper" class that turns a regular TreeView into a Species Tree.
// When I tried implementing this as a subclass of TreeView it raised issues
// so this was just easier.  This is a WRAPPER... it means there is a TreeView
// control on the form but we can access it in terms of Species and Stages
// instead of Nodes.
//=============================================================================
// Updated 5-Nov-2007: Removed the "collapsing" behavior because it was buggy.
//   Some users had requested that instead of showing 3 levels it only show 2
//   where the genus only contained one species.  Reverted to the "automatic
//   drill-down behavior" it used before.  This means when a genus is clicked
//   it expands and selects down to the first sex-stage available.
//=============================================================================
/*
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Zoopomatic2.Model;

namespace Zoopomatic2 {
    public class Z2SpeciesTree {
        public TreeNode SelectedItem;

        public Z2SpeciesTree(TreeView v) {
            tree = v;
            InitializeComponent();
            Clear();
        }

        public void SelectSpecies(int id) {
            tree.SelectedNode = speciesMap[id];
        }

        public void SelectStage(int id) {
            if (getID(tree.SelectedNode) != id)
                tree.SelectedNode = stageMap[id];
        }

        public void AddSpecies(Species s) {
            String genus = s.Name.Split(' ')[0].ToUpper();
            TreeNode chd;

            if (activeGenusMap.ContainsKey(genus)) {
                chd = activeGenusMap[genus].Nodes.Add("SPECIES" + s.ID.ToString(), "text");
            //} else if (prunedGenusMap.ContainsKey(genus)) {
            //    TreeNode gn = prunedGenusMap[genus];
            //    tree.Nodes.Remove(gn.Nodes[0]);
            //    tree.Nodes.Add(gn);
            //    prunedGenusMap.Remove(genus);
            //    activeGenusMap.Add(genus, gn);
            //    chd = gn.Nodes.Add("SPECIES" + s.ID.ToString(), "text");
            } else {
                TreeNode gn = new TreeNode(genus);
                gn.ContextMenuStrip = ctxGenus;
                //prunedGenusMap.Add(genus, gn);
                activeGenusMap.Add(genus, gn);
                chd = gn.Nodes.Add("SPECIES" + s.ID.ToString(), "text");
                tree.Nodes.Add(gn);
            }
            
            chd.Text = s.Name;
            speciesMap.Add(s.ID, chd);

            if (s.Favorite) {
                TreeNode tn = (TreeNode) chd.Clone();
                favMap.Add(s.ID, tn);
                m_favnode.Nodes.Add(tn);
                chd.ContextMenuStrip = ctxSpeciesFav;
                tn.ContextMenuStrip = ctxSpeciesFav;
            } else {
                chd.ContextMenuStrip = ctxSpeciesNormal;
            }
        }

        public void AddStage(SpeciesSexStage s) {
            String txt = s.Name;
            if (s.Hotkey != 0)
                txt += String.Format(" [{0}]", s.Hotkey);
            
            TreeNode par = speciesMap[s.Species];
            TreeNode chd = par.Nodes.Add("STAGE" + s.ID.ToString(), txt);
            chd.ContextMenuStrip = ctxSexStage;
            stageMap.Add(s.ID, chd);

            if (favMap.ContainsKey(s.Species)) {
                chd = favMap[s.Species].Nodes.Add("STAGE" + s.ID.ToString(), txt);
                chd.ContextMenuStrip = ctxSexStage;
                fstageMap.Add(s.ID, chd);
            }

            
            
        }

        public void UpdateSpecies(int id, Species s) {
            speciesMap[id].ContextMenuStrip = (s.Favorite ? ctxSpeciesFav : ctxSpeciesNormal);
            speciesMap[id].Text = s.Name;
            /*if (m_favnode.Nodes.Contains(speciesMap[id]) && !s.Favorite)
                m_favnode.Nodes.Remove(speciesMap[id]);
            if (!m_favnode.Nodes.Contains(speciesMap[id]) && s.Favorite)
                m_favnode.Nodes.Add(speciesMap[id]);*/

  /*          if (favMap.ContainsKey(id)) {
                if (!s.Favorite) {
                    favMap[id].Remove();
                    foreach (TreeNode n in favMap[id].Nodes) {
                        fstageMap.Remove(getID(n));
                    }
                    favMap.Remove(s.ID);
                } else {
                    favMap[id].ContextMenuStrip = (s.Favorite ? ctxSpeciesFav : ctxSpeciesNormal);
                    favMap[id].Text = s.Name;
                }
            } else if (s.Favorite) {
                TreeNode tn = new TreeNode(speciesMap[id].Text);
                tn.Name = speciesMap[id].Name;
                foreach (TreeNode n in speciesMap[id].Nodes) {
                    TreeNode st = (TreeNode) n.Clone();
                    tn.Nodes.Add(st);
                    fstageMap.Add(getID(st), st);
                }
                m_favnode.Nodes.Add(tn);
                favMap.Add(id, tn);
                tn.ContextMenuStrip = ctxSpeciesFav;
            }
        }

        public void UpdateStage(int id, SpeciesSexStage s) {
            TreeNode chd = stageMap[id];
            chd.Text = s.Name;

            if (s.Hotkey != 0)
                chd.Text = chd.Text + String.Format(" [{0}]", s.Hotkey);
        }

        public void RemoveSpecies(int id) {
            TreeNode chd = speciesMap[id];
            String genus = chd.Text.Split(' ')[0].ToUpper();
            if (activeGenusMap.ContainsKey(genus)) {
                TreeNode gn = activeGenusMap[genus];
                gn.Nodes.Remove(chd);
                if (gn.Nodes.Count == 0) {
                    activeGenusMap.Remove(genus);
                    tree.Nodes.Remove(gn);
                }
                //if (gn.Nodes.Count == 1) {
                //    activeGenusMap.Remove(genus);
                //    prunedGenusMap.Add(genus, gn);
                //    tree.Nodes.Remove(gn);
                //    tree.Nodes.Add(gn.Nodes[0]);
                //}
            //} else if (prunedGenusMap.ContainsKey(genus)) {
            //    TreeNode gn = prunedGenusMap[genus];
            //    tree.Nodes.Remove(chd);
            //    gn.Nodes.Remove(chd);
            //    prunedGenusMap.Remove(genus);
            }
            speciesMap.Remove(id);

            if (favMap.ContainsKey(id)) {
                favMap[id].Remove();
                favMap.Remove(id);
            }
        }

         public void RemoveStage(int id) {
            stageMap[id].Remove();
            stageMap.Remove(id);
            
            if (fstageMap.ContainsKey(id)) {
                fstageMap[id].Remove();
                fstageMap.Remove(id);
            }
        }

        public void Clear() {
            tree.Nodes.Clear();
            m_favnode = new TreeNode(" Favorites");
            m_favnode.ContextMenuStrip = ctxGenus;
            tree.Nodes.Add(m_favnode);
            speciesMap = new Dictionary<int, TreeNode>();
            stageMap = new Dictionary<int, TreeNode>();
            favMap = new Dictionary<int, TreeNode>();
            fstageMap = new Dictionary<int, TreeNode>();
            OnStageSelected(new SpeciesEventArgs(-1, false));
        }

        #region Events
        public event EventHandler CommandNewSpecies;
        public event EventHandler<SpeciesEventArgs> CommandNewStage;
        public event EventHandler<SpeciesEventArgs> CommandEditSpecies;
        public event EventHandler<SpeciesEventArgs> CommandEditStage;
        public event EventHandler<SpeciesEventArgs> CommandDeleteSpecies;
        public event EventHandler<SpeciesEventArgs> CommandDeleteStage;
        public event EventHandler<SpeciesEventArgs> FavoriteChange;
        public event EventHandler<SpeciesEventArgs> StageSelected;

        protected void OnCommandNewSpecies() {
            if (CommandNewSpecies != null)
                CommandNewSpecies(this, new EventArgs());
        }

        protected void OnCommandNewStage(SpeciesEventArgs args) {
            if (CommandNewStage != null)
                CommandNewStage(this, args);
        }

        protected void OnCommandEditSpecies(SpeciesEventArgs args) {
            if (CommandEditSpecies != null)
                CommandEditSpecies(this, args);
        }

        protected void OnCommandEditStage(SpeciesEventArgs args) {
            if (CommandEditStage != null)
                CommandEditStage(this, args);
        }

        protected void OnCommandDeleteSpecies(SpeciesEventArgs args) {
            if (CommandDeleteSpecies != null)
                CommandDeleteSpecies(this, args);
        }

        protected void OnCommandDeleteStage(SpeciesEventArgs args) {
            if (CommandDeleteStage != null)
                CommandDeleteStage(this, args);
        }

        protected void OnFavoriteChange(SpeciesEventArgs args) {
            if (FavoriteChange != null)
                FavoriteChange(this, args);
        }

        protected void OnStageSelected(SpeciesEventArgs args) {
            if (StageSelected != null)
                StageSelected(this, args);
        }



        public class SpeciesEventArgs : EventArgs {
            private int m_id;
            private bool m_fav;

            public SpeciesEventArgs(int speciesID, bool fav)
                : base() {
                m_id = speciesID;
                m_fav = fav;
            }

            public int SpeciesID {
                get {
                    return m_id;
                }
            }

            public bool Favorite {
                get {
                    return m_fav;
                }
            }
        }
        #endregion

        private void InitializeComponent() {
            this.ctxGenus = new System.Windows.Forms.ContextMenuStrip();
            this.cmiGenusNewSpecies = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxSpeciesNormal = new System.Windows.Forms.ContextMenuStrip();
            this.cmiSpeciesNNS = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSpeciesNNSS = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSpeciesNSep0 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiSpeciesNProps = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSpeciesNFav = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxSpeciesFav = new System.Windows.Forms.ContextMenuStrip();
            this.cmiSpeciesFNS = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSpeciesFNSS = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSpeciesFSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiSpeciesFProps = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSpeciesFFav = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxSexStage = new System.Windows.Forms.ContextMenuStrip();
            this.cmiStageNS = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiStageNSS = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiStageSep0 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiStageProps = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiStageDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiStageSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiStageSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxGenus.SuspendLayout();
            this.ctxSpeciesNormal.SuspendLayout();
            this.ctxSpeciesFav.SuspendLayout();
            this.ctxSexStage.SuspendLayout();
            // 
            // ctxGenus
            // 
            this.ctxGenus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiGenusNewSpecies});
            this.ctxGenus.Name = "contextMenuStrip1";
            this.ctxGenus.Size = new System.Drawing.Size(158, 26);
            this.ctxGenus.Text = "Genus";
            // 
            // cmiGenusNewSpecies
            // 
            this.cmiGenusNewSpecies.Name = "cmiGenusNewSpecies";
            this.cmiGenusNewSpecies.Size = new System.Drawing.Size(157, 22);
            this.cmiGenusNewSpecies.Text = "New Species...";
            this.cmiGenusNewSpecies.Click += new System.EventHandler(this.cmiGenusNewSpecies_Click);
            // 
            // ctxSpeciesNormal
            // 
            this.ctxSpeciesNormal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiSpeciesNNS,
            this.cmiSpeciesNNSS,
            this.cmiSpeciesNSep0,
            this.cmiSpeciesNProps,
            this.cmiSpeciesNFav});
            this.ctxSpeciesNormal.Name = "ctxSpeciesNormal";
            this.ctxSpeciesNormal.Size = new System.Drawing.Size(172, 98);
            this.ctxSpeciesNormal.Text = "Species";
            // 
            // cmiSpeciesNNS
            // 
            this.cmiSpeciesNNS.Name = "cmiSpeciesNNS";
            this.cmiSpeciesNNS.Size = new System.Drawing.Size(171, 22);
            this.cmiSpeciesNNS.Text = "New Species...";
            this.cmiSpeciesNNS.Click += new System.EventHandler(this.cmiSpeciesNNS_Click);
            // 
            // cmiSpeciesNNSS
            // 
            this.cmiSpeciesNNSS.Name = "cmiSpeciesNNSS";
            this.cmiSpeciesNNSS.Size = new System.Drawing.Size(171, 22);
            this.cmiSpeciesNNSS.Text = "New Sex/Stage...";
            this.cmiSpeciesNNSS.Click += new System.EventHandler(this.cmiSpeciesNNSS_Click);
            // 
            // cmiSpeciesNSep0
            // 
            this.cmiSpeciesNSep0.Name = "cmiSpeciesNSep0";
            this.cmiSpeciesNSep0.Size = new System.Drawing.Size(168, 6);
            // 
            // cmiSpeciesNProps
            // 
            this.cmiSpeciesNProps.Name = "cmiSpeciesNProps";
            this.cmiSpeciesNProps.Size = new System.Drawing.Size(171, 22);
            this.cmiSpeciesNProps.Text = "Properties...";
            this.cmiSpeciesNProps.Click += new System.EventHandler(this.cmiSpeciesNProps_Click);
            // 
            // cmiSpeciesNFav
            // 
            this.cmiSpeciesNFav.Name = "cmiSpeciesNFav";
            this.cmiSpeciesNFav.Size = new System.Drawing.Size(171, 22);
            this.cmiSpeciesNFav.Text = "Favorite";
            this.cmiSpeciesNFav.Click += new System.EventHandler(this.cmiSpeciesNFav_Click);
            // 
            // ctxSpeciesFav
            // 
            this.ctxSpeciesFav.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiSpeciesFNS,
            this.cmiSpeciesFNSS,
            this.cmiSpeciesFSep1,
            this.cmiSpeciesFProps,
            this.cmiSpeciesFFav});
            this.ctxSpeciesFav.Name = "ctxSpeciesFav";
            this.ctxSpeciesFav.Size = new System.Drawing.Size(172, 98);
            // 
            // cmiSpeciesFNS
            // 
            this.cmiSpeciesFNS.Name = "cmiSpeciesFNS";
            this.cmiSpeciesFNS.Size = new System.Drawing.Size(171, 22);
            this.cmiSpeciesFNS.Text = "New Species...";
            this.cmiSpeciesFNS.Click += new System.EventHandler(this.cmiSpeciesFNS_Click);
            // 
            // cmiSpeciesFNSS
            // 
            this.cmiSpeciesFNSS.Name = "cmiSpeciesFNSS";
            this.cmiSpeciesFNSS.Size = new System.Drawing.Size(171, 22);
            this.cmiSpeciesFNSS.Text = "New Sex/Stage...";
            this.cmiSpeciesFNSS.Click += new System.EventHandler(this.cmiSpeciesFNSS_Click);
            // 
            // cmiSpeciesFSep1
            // 
            this.cmiSpeciesFSep1.Name = "cmiSpeciesFSep1";
            this.cmiSpeciesFSep1.Size = new System.Drawing.Size(168, 6);
            // 
            // cmiSpeciesFProps
            // 
            this.cmiSpeciesFProps.Name = "cmiSpeciesFProps";
            this.cmiSpeciesFProps.Size = new System.Drawing.Size(171, 22);
            this.cmiSpeciesFProps.Text = "Properties...";
            this.cmiSpeciesFProps.Click += new System.EventHandler(this.cmiSpeciesFProps_Click);
            // 
            // cmiSpeciesFFav
            // 
            this.cmiSpeciesFFav.Checked = true;
            this.cmiSpeciesFFav.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmiSpeciesFFav.Name = "cmiSpeciesFFav";
            this.cmiSpeciesFFav.Size = new System.Drawing.Size(171, 22);
            this.cmiSpeciesFFav.Text = "Favorite";
            this.cmiSpeciesFFav.Click += new System.EventHandler(this.cmiSpeciesFFav_Click);
            // 
            // ctxSexStage
            // 
            this.ctxSexStage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiStageNS,
            this.cmiStageNSS,
            this.cmiStageSep0,
            this.cmiStageProps,
            this.cmiStageDelete,
            this.cmiStageSep1,
            this.cmiStageSelect});
            this.ctxSexStage.Name = "ctxSexStage";
            this.ctxSexStage.Size = new System.Drawing.Size(172, 104);
            // 
            // cmiStageNS
            // 
            this.cmiStageNS.Name = "cmiStageNS";
            this.cmiStageNS.Size = new System.Drawing.Size(171, 22);
            this.cmiStageNS.Text = "New Species...";
            this.cmiStageNS.Click += new System.EventHandler(this.cmiStageNS_Click);
            // 
            // cmiStageNSS
            // 
            this.cmiStageNSS.Name = "cmiStageNSS";
            this.cmiStageNSS.Size = new System.Drawing.Size(171, 22);
            this.cmiStageNSS.Text = "New Sex/Stage...";
            this.cmiStageNSS.Click += new System.EventHandler(this.cmiStageNSS_Click);
            // 
            // cmiStageSep0
            // 
            this.cmiStageSep0.Name = "cmiStageSep0";
            this.cmiStageSep0.Size = new System.Drawing.Size(168, 6);
            // 
            // cmiStageProps
            // 
            this.cmiStageProps.Name = "cmiStageProps";
            this.cmiStageProps.Size = new System.Drawing.Size(171, 22);
            this.cmiStageProps.Text = "Properties...";
            this.cmiStageProps.Click += new System.EventHandler(this.cmiStageProps_Click);
            // 
            // cmiStageDelete
            // 
            this.cmiStageDelete.Name = "cmiStageDelete";
            this.cmiStageDelete.Size = new System.Drawing.Size(171, 22);
            this.cmiStageDelete.Text = "Delete";
            this.cmiStageDelete.Visible = false;
            this.cmiStageDelete.Click += new System.EventHandler(this.cmiStageDelete_Click);
            // 
            // cmiStageSep1
            // 
            this.cmiStageSep1.Name = "cmiStageSep1";
            this.cmiStageSep1.Size = new System.Drawing.Size(168, 6);
            // 
            // cmiStageSelect
            // 
            this.cmiStageSelect.Name = "cmiStageSelect";
            this.cmiStageSelect.Size = new System.Drawing.Size(171, 22);
            this.cmiStageSelect.Text = "Select";
            this.cmiStageSelect.Click += new System.EventHandler(this.cmiStageSelect_Click);
            this.ctxGenus.ResumeLayout(false);
            this.ctxSpeciesNormal.ResumeLayout(false);
            this.ctxSpeciesFav.ResumeLayout(false);
            this.ctxSexStage.ResumeLayout(false);
            //
            // tree
            //
            tree.SuspendLayout();
            tree.AfterSelect += new TreeViewEventHandler(tree_AfterSelect);
            tree.NodeMouseClick += new TreeNodeMouseClickEventHandler(tree_NodeMouseClick);
            tree.ResumeLayout(false);
        }

        void tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
            TreeNode tn = e.Node;
            SelectedItem = tn;
        }

        private void tree_AfterSelect(object sender, TreeViewEventArgs e) {
            TreeNode tn = e.Node;

            while (tn.FirstNode != null) {
                tn = tn.FirstNode;
                SelectedItem = tn;
                tree.SelectedNode = tn;
            }
            SelectedItem = tree.SelectedNode;
            if (stageMap.ContainsValue(tn) || fstageMap.ContainsValue(tn))
                OnStageSelected(new SpeciesEventArgs(getID(tn), false));
        }

        private int getID(TreeNode n) {
            if (n == null || n.Name.Equals(""))
                return -1;
            if (n.Name.StartsWith("ST"))
                return Convert.ToInt32(n.Name.Substring(5));
            return Convert.ToInt32(n.Name.Substring(7));
        }

        private void cmiGenusNewSpecies_Click(object sender, EventArgs e) {
            OnCommandNewSpecies();
        }

        private void cmiSpeciesFFav_Click(object sender, EventArgs e) {
            OnFavoriteChange(new SpeciesEventArgs(getID(SelectedItem), false));
        }

        private void cmiSpeciesFNS_Click(object sender, EventArgs e) {
            OnCommandNewSpecies();
        }

        private void cmiSpeciesFNSS_Click(object sender, EventArgs e) {
            OnCommandNewStage(new SpeciesEventArgs(getID(SelectedItem), false));
        }

        private void cmiSpeciesFProps_Click(object sender, EventArgs e) {
            OnCommandEditSpecies(new SpeciesEventArgs(getID(SelectedItem), false));
        }

        private void cmiSpeciesNFav_Click(object sender, EventArgs e) {
            OnFavoriteChange(new SpeciesEventArgs(getID(SelectedItem), true));
        }

        private void cmiSpeciesNNS_Click(object sender, EventArgs e) {
            OnCommandNewSpecies();
        }

        private void cmiSpeciesNNSS_Click(object sender, EventArgs e) {
            OnCommandNewStage(new SpeciesEventArgs(getID(SelectedItem), false));
        }

        private void cmiSpeciesNProps_Click(object sender, EventArgs e) {
            OnCommandEditSpecies(new SpeciesEventArgs(getID(SelectedItem), false));
        }

        private void cmiStageNS_Click(object sender, EventArgs e) {
            OnCommandNewSpecies();
        }

        private void cmiStageNSS_Click(object sender, EventArgs e) {
            int specid = getID(SelectedItem.Parent);
            OnCommandNewStage(new SpeciesEventArgs(specid, false));
        }

        private void cmiStageProps_Click(object sender, EventArgs e) {
            OnCommandEditStage(new SpeciesEventArgs(getID(SelectedItem), false));
        }

        private void cmiStageDelete_Click(object sender, EventArgs e) {
            OnCommandDeleteStage(new SpeciesEventArgs(getID(SelectedItem), false));
        }

        private void cmiStageSelect_Click(object sender, EventArgs e) {
            OnStageSelected(new SpeciesEventArgs(getID(SelectedItem), false));
            tree.SelectedNode = SelectedItem;
        }


        private Dictionary<String, TreeNode> activeGenusMap = new Dictionary<string,TreeNode>();
        //private Dictionary<String, TreeNode> prunedGenusMap = new Dictionary<string,TreeNode>();
        private Dictionary<int, TreeNode> speciesMap;
        private Dictionary<int, TreeNode> favMap;
        private Dictionary<int, TreeNode> stageMap;
        private Dictionary<int, TreeNode> fstageMap;


        private ContextMenuStrip ctxGenus;
        private ToolStripMenuItem cmiGenusNewSpecies;
        private ContextMenuStrip ctxSpeciesNormal;
        private ToolStripMenuItem cmiSpeciesNNS;
        private ToolStripMenuItem cmiSpeciesNNSS;
        private ToolStripSeparator cmiSpeciesNSep0;
        private ToolStripMenuItem cmiSpeciesNProps;
        private ToolStripMenuItem cmiSpeciesNFav;
        private ContextMenuStrip ctxSpeciesFav;
        private ToolStripMenuItem cmiSpeciesFNS;
        private ToolStripMenuItem cmiSpeciesFNSS;
        private ToolStripSeparator cmiSpeciesFSep1;
        private ToolStripMenuItem cmiSpeciesFProps;
        private ToolStripMenuItem cmiSpeciesFFav;
        private ContextMenuStrip ctxSexStage;
        private ToolStripMenuItem cmiStageNS;
        private ToolStripMenuItem cmiStageNSS;
        private ToolStripSeparator cmiStageSep0;
        private ToolStripMenuItem cmiStageProps;
         private ToolStripMenuItem cmiStageDelete;
        private ToolStripSeparator cmiStageSep1;
        private ToolStripMenuItem cmiStageSelect;

        private TreeNode m_favnode;
        private TreeView tree;
    }
}
*/