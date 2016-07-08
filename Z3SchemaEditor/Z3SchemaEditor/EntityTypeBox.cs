using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.Model;

namespace Z3SchemaEditor
{
    public partial class EntityTypeBox : Form
    {
        private bool Switch;
        private bool lastItemPlaceBottom;
        private string text;

        public EntityTypeBox()
        {
            InitializeComponent();
            fieldsList.ItemDrag += new ItemDragEventHandler(listView_ItemDrag);
            fieldsList.DragEnter += new DragEventHandler(listView_DragEnter);
            fieldsList.DragOver += new DragEventHandler(listView_DragOver);
            fieldsList.DragLeave += new EventHandler(listView_DragLeave);
            fieldsList.DragDrop += new DragEventHandler(listView_DragDrop);
        }

        bool privateDrag;

        private void listView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            privateDrag = true;
            DoDragDrop(e.Item, DragDropEffects.Move);
            privateDrag = false;
        }

        private void listView_DragEnter(object sender, DragEventArgs e)
        {
           if (privateDrag) e.Effect = e.AllowedEffect;
        }

        private void listView_DragOver(object sender, DragEventArgs e)
        {
            Point targetPoint = fieldsList.PointToClient(new Point(e.X, e.Y));         
            int targetIndex = fieldsList.InsertionMark.NearestIndex(targetPoint);

            Debug.WriteLine(targetIndex);
            if (targetIndex == fieldsList.Items.Count - 1)
            {
                Rectangle itemBounds = fieldsList.GetItemRect(targetIndex);

                if (targetPoint.Y > itemBounds.Bottom)
                {
                    fieldsList.InsertionMark.AppearsAfterItem = true;
                    lastItemPlaceBottom = true;
                } else
                {
                    lastItemPlaceBottom = false;
                }
            } else {
                fieldsList.InsertionMark.AppearsAfterItem = false;
            }

            // Set the location of the insertion mark. If the mouse is
            // over the dragged item, the targetIndex value is -1 and
            // the insertion mark disappears.
            fieldsList.InsertionMark.Index = targetIndex;
        }

        // Removes the insertion mark when the mouse leaves the control.
        private void listView_DragLeave(object sender, EventArgs e)
        {
            fieldsList.InsertionMark.Index = -1;
        }

        private void listView_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the index of the insertion mark;
            int targetIndex = fieldsList.InsertionMark.Index;
            
            // If the insertion mark is not visible, exit the method.
            if (targetIndex == -1)
            {
                return;
            }

            // Retrieve the dragged item.
            ListViewItem draggedItem =
                (ListViewItem)e.Data.GetData(typeof(ListViewItem));

            // Insert a copy of the dragged item at the target index.
            // A copy must be inserted before the original item is removed
            // to preserve item index values. 
            
            if (!lastItemPlaceBottom)
            {
                fieldsList.Items.Insert(
                    targetIndex, (ListViewItem)draggedItem.Clone());
            } else
            {
                fieldsList.Items.Insert(
                    targetIndex + 1, (ListViewItem)draggedItem.Clone());
            }

            // Remove the original copy of the dragged item.
            fieldsList.Items.Remove(draggedItem);

            Switch = true;
        }

        public ZLevel Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                refreshFields();
            }
        }

        private ZLevel _value;

        private void refreshFields()
        {
            nameBox.Text = _value.Name;
            measurableBox.Checked = _value.Measurable;

            fieldsList.Items.Clear();
            foreach (ZField f in _value.Fields)
            {
                ListViewItem i = fieldsList.Items.Add(f.Name);
                i.Tag = f;
            }
        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {
            _value.Name = nameBox.Text;
        }

        private void measurableBox_CheckedChanged(object sender, EventArgs e)
        {
            _value.Measurable = measurableBox.Checked;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (fieldsList.SelectedItems.Count > 0)
            {
                using (EntityFieldBox f = new EntityFieldBox(fieldsList))
                {
                    f.Value = ((ZField)fieldsList.SelectedItems[0].Tag);
                    f.ShowDialog();
                    Value.SaveField(f.Value);
                }
                refreshFields();
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            nameBox_TextChanged(null, null);
            measurableBox_CheckedChanged(null, null);

            if (Switch) Value.SwitchField(Value.Fields, fieldsList);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            int i = 0;
            text = "NewField" + fieldsList.Items.Count;

            // adds distict item
            while (fieldsList.FindItemWithText(text) != null)
            {
                text = "NewField" + (fieldsList.Items.Count + ++i);
            }

            Value.AddField(text);
            refreshFields();

            // highlights added item
            fieldsList.FindItemWithText(text).Selected = true;
            fieldsList.HideSelection = false;
        }

        private void delButton_Click(object sender, EventArgs e)
        {
            if (fieldsList.SelectedItems.Count > 0)
            {
                Value.DeleteField((ZField)fieldsList.SelectedItems[0].Tag);

                refreshFields();
            }
        }
    }
}