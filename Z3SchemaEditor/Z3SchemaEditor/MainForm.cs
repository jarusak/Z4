using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.Workspace;
using System.IO;
using Z3.Model;

namespace Z3SchemaEditor
{
    public partial class MainForm : Form
    {
        private WorkspaceInternals schema;

        public MainForm()
        {
            InitializeComponent();
            schema = null;
        }

        private void fileLoaded(bool value)
        {
            tabControl1.Visible = value;
            createToolStripMenuItem.Visible = value;
            viewToolStripMenuItem.Visible = value;
            editToolStripMenuItem2.Visible = value;
            
            refreshPage();
        }

        private void refreshPage()
        {
            reportList.Items.Clear();
            mtypeList.Items.Clear();
            countableList.Items.Clear();
            propsList.Items.Clear();
            containerList.Items.Clear();

            if (schema != null)
            {
                schema.Levels.forceReload();
                populateLevels("Container", containerList);
                populateLevels("Countable", countableList);
                populateMeasurements();
                populateProperties();
            }
        }

        private void populateMeasurements()
        {
            mtypeList.Items.Clear();
            schema.refreshMeasurementTypes();

            foreach (ZMeasurement m in schema.MeasurementTypes)
            {
                ListViewItem i = mtypeList.Items.Add(m.Name);
                i.Tag = m;
            }
        }

        private void populateProperties()
        {
            propsList.Items.Clear();
            Dictionary<string, string> props = schema.getProperties();
            
            foreach (string m in props.Keys)
            {
                ListViewItem i = propsList.Items.Add(m);
                i.SubItems.Add(props[m]);
                i.Tag = props[m];
            }
        }

        private void populateLevels(string table, ListView list)
        {
            ZLevel cont = schema.Levels.getRootType(table);
            while (cont != null)
            {
                ListViewItem i = list.Items.Add(cont.Name);
                i.Tag = cont;
                cont = cont.Child;
            }
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://lter.limnology.wisc.edu/software/Z3");
        }

        private void lTERSoftwareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://lter.limnology.wisc.edu/software/");
        }

        private void aboutZ3SchemaEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutBox1 a = new AboutBox1())
            {
                a.ShowDialog();
            }
        }

        private void containerTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = containersPage;
        }

        private void countableTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = countablesPage;
        }

        private void reportTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = reportsPage;
        }

        private void measurementTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = measurementsPage;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = propertiesPage;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    createFile(saveFileDialog1.FileName);
                    schema = openFile(saveFileDialog1.FileName);
                    fileLoaded(true);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Unable to load file:\n\n" + ex.Message, "Error");
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    schema = openFile(openFileDialog1.FileName);
                    fileLoaded(true);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Unable to load file:\n\n" + ex.Message, "Error");
                }
            }
        }

        private void createFile(string filename)
        {
            Z3.Workspace.Factory.CreateSchema(filename);
        }

        private WorkspaceInternals openFile(string filename)
        {
            return Z3.Workspace.Factory.Load(filename);
        }

        private void containerTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (schema != null)
            {
                newLevel("Container");
            }
        }

        private void newLevel(string table)
        {
            schema.Levels.Create(table);
            refreshPage();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            fileLoaded(false);
        }

        private void editToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == containersPage)
            {
                if (containerList.SelectedItems.Count > 0)
                {
                    using (EntityTypeBox f = new EntityTypeBox())
                    {
                        f.Value = ((ZLevel)containerList.SelectedItems[0].Tag);
                        f.ShowDialog();
                        f.Value.Save();
                    }

                    refreshPage();
                }
            }
            else if (tabControl1.SelectedTab == countablesPage)
            {
                if (countableList.SelectedItems.Count > 0)
                {
                    using (EntityTypeBox f = new EntityTypeBox())
                    {
                        f.Value = ((ZLevel)countableList.SelectedItems[0].Tag);
                        f.ShowDialog();
                        f.Value.Save();
                    }

                    refreshPage();
                }
            }
            else if (tabControl1.SelectedTab == measurementsPage)
            {
                if (mtypeList.SelectedItems.Count > 0)
                {
                    using (MTypeForm f = new MTypeForm())
                    {
                        f.Value = ((ZMeasurement)mtypeList.SelectedItems[0].Tag);
                        f.ShowDialog();
                        f.Value.Save();
                        populateMeasurements();
                    }
                }
            }
            else if (tabControl1.SelectedTab == reportsPage)
            {

            }
            else if (tabControl1.SelectedTab == propertiesPage)
            {
                if (propsList.SelectedItems.Count > 0) {
                    using (PropertyForm f = new PropertyForm())
                    {
                        f.PropertyName = propsList.SelectedItems[0].Text;
                        f.PropertyValue = propsList.SelectedItems[0].Tag.ToString();
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            schema.setProperty(f.PropertyName, propsList.SelectedItems[0].Text, f.PropertyValue);
                            populateProperties();
                        }
                    }
                }
            }
        }
        
        private void countableTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (schema != null)
            {
                newLevel("Countable");
            }
        }

        private void moveUpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == containersPage)
            {
                if (containerList.SelectedItems.Count > 0)
                {
                    moveUp((ZLevel)containerList.SelectedItems[0].Tag);
                }
            }
            else if (tabControl1.SelectedTab == countablesPage)
            {
                if (countableList.SelectedItems.Count > 0)
                {
                    moveUp((ZLevel)countableList.SelectedItems[0].Tag);
                }
            }
        }

        private void moveUp(ZLevel zl)
        {
            if (zl.Root) return;
            swap(zl.Parent, zl);
            schema.Levels.forceReload();
            refreshPage();
        }

        private void swap(ZLevel a, ZLevel b)
        {
            if (a.Root)
            {
                b.Root = true;
                b.ParentID = 0;
                a.Root = false;
            }
            else
            {
                b.ParentID = a.ParentID;
                b.Parent.ChildID = b.ID;
            }
            a.ParentID = b.ID;

            if (b.Final)
            {
                a.Final = true;
                a.ChildID = 0;
                b.Final = false;
            }
            else
            {
                a.ChildID = b.ChildID;
                a.Child.ParentID = a.ID;
            }
            b.ChildID = a.ID;

            a.Save();
            b.Save();
        }

        private void moveDownToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == containersPage)
            {
                if (containerList.SelectedItems.Count > 0)
                {
                    moveDown((ZLevel)containerList.SelectedItems[0].Tag);
                }
            }
            else if (tabControl1.SelectedTab == countablesPage)
            {
                if (countableList.SelectedItems.Count > 0)
                {
                    moveDown((ZLevel)countableList.SelectedItems[0].Tag);
                }
            }
        }

        private void moveDown(ZLevel zl)
        {
            if (zl.Final) return;
            swap(zl, zl.Child);
            schema.Levels.forceReload();
            refreshPage();
        }

        private void propertyValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PropertyForm f = new PropertyForm())
            {
                f.PropertyName = "New Property";
                f.PropertyValue = "Value";
                f.Rename = true;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    schema.addProperty(f.PropertyName, f.PropertyValue);
                    populateProperties();
                }
            }
        }

        private void measurementTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            schema.addMeasurementType("New MType");
            populateMeasurements();
        }

        private void deleteToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == containersPage)
            {
                if (containerList.SelectedItems.Count > 0)
                {
                    deleteLevel((ZLevel)containerList.SelectedItems[0].Tag);
                    refreshPage();
                }
            }
            else if (tabControl1.SelectedTab == countablesPage)
            {
                if (countableList.SelectedItems.Count > 0)
                {
                    deleteLevel((ZLevel)countableList.SelectedItems[0].Tag);
                    refreshPage();
                }
            }
            else if (tabControl1.SelectedTab == measurementsPage)
            {
                if (mtypeList.Items.Count > 0)
                {
                    schema.deleteMeasurementType((ZMeasurement)mtypeList.SelectedItems[0].Tag);
                    populateMeasurements();
                }
            }
            else if (tabControl1.SelectedTab == reportsPage)
            {

            }
            else if (tabControl1.SelectedTab == propertiesPage)
            {
                if (propsList.SelectedItems.Count > 0)
                {
                    using (PropertyForm f = new PropertyForm())
                    {
                        schema.deleteProperty(propsList.SelectedItems[0].Text);
                        populateProperties();
                    }
                }
            }
        }

        private void deleteLevel(ZLevel zLevel)
        {
            zLevel.Delete();
        }
    }
}