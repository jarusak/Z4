using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

using Z3.Model;

namespace Z3.View
{
    public class DataPointElements
    {
        private Panel _panel;
        private DataPointsDisplayManager _mgr;

        public DataPointElements()
        {
            InitializeComponents();
            _mgr = new DataPointsDisplayManager(dataPoints);
        }

        public void Bind(Panel p)
        {
            _panel = p;
            _panel.Controls.Add(toolStripContainer1);
        }

        #region Controls

        private void InitializeComponents()
        {
            this.dataPoints = new System.Windows.Forms.ListView();
            this.individualColumn = new System.Windows.Forms.ColumnHeader();
            this.valColumn = new ColumnHeader();
            this.CountableColumn = new ColumnHeader();
            this.WeightColumn = new ColumnHeader();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            //
            // dataPoints
            //
            this.dataPoints.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.individualColumn, this.valColumn, this.CountableColumn, this.WeightColumn});
            this.dataPoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataPoints.Location = new System.Drawing.Point(0, 0);
            this.dataPoints.Name = "dataPoints";
            this.dataPoints.Size = new System.Drawing.Size(560, 118);
            this.dataPoints.TabIndex = 0;
            this.dataPoints.UseCompatibleStateImageBehavior = false;
            this.dataPoints.View = System.Windows.Forms.View.Details;
            this.dataPoints.MultiSelect = false;
            this.dataPoints.FullRowSelect = true;
            // 
            // individualColumn
            // 
            this.individualColumn.Text = "Individual";
            this.individualColumn.Width = 50;
            this.valColumn.Text = "Measurement";
            this.valColumn.Width = 300;
            this.CountableColumn.Text = "Countable Name";
            this.CountableColumn.Width = 300;
            this.WeightColumn.Text = "Weight";
            this.WeightColumn.Width = 50;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(560, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(150, 17);
            this.toolStripStatusLabel1.Text = "*New Individual";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.dataPoints);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(560, 390);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(560, 437);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            //
            // panel
            //
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
        }

        private System.Windows.Forms.ListView dataPoints;
        private System.Windows.Forms.ColumnHeader individualColumn;
        private System.Windows.Forms.ColumnHeader valColumn;
        private System.Windows.Forms.ColumnHeader CountableColumn;
        private ColumnHeader WeightColumn;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;

        #endregion

        public bool Enabled
        {
            set
            {
                dataPoints.Enabled = value;
            }
        }

        public string StatusBarLabel
        {
            get { return toolStripStatusLabel1.Text; }
            set { toolStripStatusLabel1.Text = value; }
        }

        public ItemContainer<ZDataPoint> DataPoints
        {
            get
            {
                return _mgr;
            }
        }
    }

    /// <summary>
    /// Manages all Data Points being displayed on a list view.
    /// </summary>
    public class DataPointsDisplayManager : ItemContainer<ZDataPoint>
    {
        private ListView _v;
        private Dictionary<int, ListViewItem> _items;
        private bool _wait = false;
        private bool _wantedTo = false;
        
        public DataPointsDisplayManager(ListView v)
        {
            _v = v;
            _v.SelectedIndexChanged += new EventHandler(_v_SelectedIndexChanged);
            _items = new Dictionary<int, ListViewItem>();
        }

        public ZDataPoint ContextMenuSubject
        {
            get
            {
                return SelectedItem;
            }
        }
        void _v_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_wait)
            {
                _wantedTo = true;
            }
            else
            {
                if (Selected != null) Selected(this, new EventArgs());
            }
        }

        public ZDataPoint SelectedItem
        {
            get
            {
                if (_v.SelectedItems.Count < 1) return null;
                return (ZDataPoint)_v.SelectedItems[0].Tag;
            }
            set
            {
                doSelect(value);
            }
        }

        private void doAddOrUpdate(ZDataPoint p)
        {
            if (_items.ContainsKey(p.ID))
                doUpdate(p);
            else
                doAdd(p.Individual, p);
        }

        public void Add(ZDataPoint p)
        {
            doAddOrUpdate(p);
        }

        public void Update(ZDataPoint p)
        {
            doAddOrUpdate(p);
        }

        private void doAdd(ZIndividual i, ZDataPoint p)
        {
            ListViewItem newItem;

            if (p.MeasurementType.Name.Equals("Length"))
            {
                int weight = i.Countable.A + i.Countable.B;
               
                newItem = new ListViewItem(new String[]{
                    i.ID.ToString(),
                    p.MeasurementType.Name + "=" + p.Value,
                    i.Countable.Name,
                    weight.ToString()
                });
            }
            else {
                newItem = new ListViewItem(new String[]{
                    i.ID.ToString(),
                    p.MeasurementType.Name + "=" + p.Value,
                    i.Countable.Name,
                    ""
                });
            }
            newItem.Tag = p;
            
            _items.Add(p.ID, newItem);
            _v.Items.Add(newItem);

            doSelect(p);
        }

        public void Delete(ZDataPoint p)
        {
            if (!_items.ContainsKey(p.ID))
                throw new ArgumentException("Cannot remove this item!  It is not there");

            _v.Items.Remove(_items[p.ID]);
            _items.Remove(p.ID);
        }

        private void doUpdate(ZDataPoint p)
        {
            if (!_items.ContainsKey(p.ID))
                throw new ArgumentException("Cannot update this item!  It is not there");

            _items[p.ID].SubItems[1].Text = p.MeasurementType.Name + "=" + p.Value;

            doSelect(p);
        }

        private void doSelect(ZDataPoint p)
        {
            _wait = true;
            _v.SelectedItems.Clear();

            if (p != null)
            {
                if (!_items.ContainsKey(p.ID))
                    throw new ArgumentException("Cannot select this item!  It is not there");

                _items[p.ID].Selected = true;
                _items[p.ID].EnsureVisible();
            }

            _wait = false;
            if (_wantedTo)
            {
                if (Selected != null) Selected(this, new EventArgs());
                _wantedTo = false;
            }
        }

        public event EventHandler Selected;

        public void Clear()
        {
            _items.Clear();
            _v.Items.Clear();
        }
    }
}
