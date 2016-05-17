using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.Workspace;
using Z3.Forms;
using Z3.Util;

namespace Z3.Forms
{
    public partial class ExportForm : Form
    {
        private IWorkspace _ws;
        public ExportForm(IWorkspace ws)
        {
            InitializeComponent();
            _ws = ws;
            refreshList();
        }

        private void refreshList()
        {
            reportList.Items.Clear();
            List<ReportType> rt = _ws.ReportTypes.All;
            foreach (ReportType r in rt)
            {
                ListViewItem lvi = new ListViewItem(r.Name);
                lvi.Tag = r;
                reportList.Items.Add(lvi);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (ReportTypeForm rf = new ReportTypeForm(_ws))
            {
                rf.TopMost = TopMost;
                rf.ShowNewDialog();
            }
            updateListFromDatabase();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (reportList.Items.Count == 0 || reportList.SelectedItems.Count == 0) return;
            using (ReportTypeForm rf = new ReportTypeForm(_ws))
            {
                rf.TopMost = TopMost;
                rf.ShowEditDialog((ReportType)reportList.SelectedItems[0].Tag);
            }
            updateListFromDatabase();
        }

        private void updateListFromDatabase()
        {
            refreshList();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (reportList.Items.Count == 0 || reportList.SelectedItems.Count == 0) return;
            _ws.ReportTypes.Delete(((ReportType)reportList.SelectedItems[0].Tag));
            updateListFromDatabase();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (reportList.SelectedItems.Count < 1)
            {
                MessageBox.Show("No report selected!", "Unable to export");
            }
            else
            {
                if (exportDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileExporter ex = new FileExporter((ReportType)reportList.SelectedItems[0].Tag);
                        ex.export(exportDialog.FileName, headerBox.Checked);
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred:\n\n" + ex.Message, "Export Failed");
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://lter.limnology.wisc.edu/software/Reports_for_Z3_Sample_Schema");
        }
    }
}