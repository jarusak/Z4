namespace Z3SchemaEditor
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.moveUpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.containerTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.countableTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.measurementTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertyValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.containerTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.countableTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.measurementTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.lTERSoftwareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutZ3SchemaEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.containersPage = new System.Windows.Forms.TabPage();
            this.containerList = new System.Windows.Forms.ListView();
            this.nameColumn = new System.Windows.Forms.ColumnHeader();
            this.countablesPage = new System.Windows.Forms.TabPage();
            this.countableList = new System.Windows.Forms.ListView();
            this.countNameColumn = new System.Windows.Forms.ColumnHeader();
            this.measurementsPage = new System.Windows.Forms.TabPage();
            this.mtypeList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.propertiesPage = new System.Windows.Forms.TabPage();
            this.propsList = new System.Windows.Forms.ListView();
            this.propNameColumn = new System.Windows.Forms.ColumnHeader();
            this.propValueColumn = new System.Windows.Forms.ColumnHeader();
            this.reportsPage = new System.Windows.Forms.TabPage();
            this.reportList = new System.Windows.Forms.ListView();
            this.reportNameColumn = new System.Windows.Forms.ColumnHeader();
            this.editDeleteMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editDeleteUpDownMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.containersPage.SuspendLayout();
            this.countablesPage.SuspendLayout();
            this.measurementsPage.SuspendLayout();
            this.propertiesPage.SuspendLayout();
            this.reportsPage.SuspendLayout();
            this.editDeleteMenu.SuspendLayout();
            this.editDeleteUpDownMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem2,
            this.createToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(562, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(120, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem2
            // 
            this.editToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem3,
            this.toolStripMenuItem6,
            this.moveUpToolStripMenuItem1,
            this.moveDownToolStripMenuItem1,
            this.toolStripMenuItem7,
            this.deleteToolStripMenuItem2});
            this.editToolStripMenuItem2.Name = "editToolStripMenuItem2";
            this.editToolStripMenuItem2.Size = new System.Drawing.Size(37, 20);
            this.editToolStripMenuItem2.Text = "Edit";
            // 
            // editToolStripMenuItem3
            // 
            this.editToolStripMenuItem3.Name = "editToolStripMenuItem3";
            this.editToolStripMenuItem3.Size = new System.Drawing.Size(152, 22);
            this.editToolStripMenuItem3.Text = "Edit";
            this.editToolStripMenuItem3.Click += new System.EventHandler(this.editToolStripMenuItem3_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(149, 6);
            // 
            // moveUpToolStripMenuItem1
            // 
            this.moveUpToolStripMenuItem1.Name = "moveUpToolStripMenuItem1";
            this.moveUpToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.moveUpToolStripMenuItem1.Text = "Move Up";
            this.moveUpToolStripMenuItem1.Click += new System.EventHandler(this.moveUpToolStripMenuItem1_Click);
            // 
            // moveDownToolStripMenuItem1
            // 
            this.moveDownToolStripMenuItem1.Name = "moveDownToolStripMenuItem1";
            this.moveDownToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.moveDownToolStripMenuItem1.Text = "Move Down";
            this.moveDownToolStripMenuItem1.Click += new System.EventHandler(this.moveDownToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(149, 6);
            // 
            // deleteToolStripMenuItem2
            // 
            this.deleteToolStripMenuItem2.Name = "deleteToolStripMenuItem2";
            this.deleteToolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStripMenuItem2.Text = "Delete";
            this.deleteToolStripMenuItem2.Click += new System.EventHandler(this.deleteToolStripMenuItem2_Click);
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.containerTypeToolStripMenuItem,
            this.countableTypeToolStripMenuItem,
            this.reportTypeToolStripMenuItem,
            this.measurementTypeToolStripMenuItem,
            this.propertyValueToolStripMenuItem});
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.createToolStripMenuItem.Text = "Create";
            // 
            // containerTypeToolStripMenuItem
            // 
            this.containerTypeToolStripMenuItem.Name = "containerTypeToolStripMenuItem";
            this.containerTypeToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.containerTypeToolStripMenuItem.Text = "Container Type";
            this.containerTypeToolStripMenuItem.Click += new System.EventHandler(this.containerTypeToolStripMenuItem_Click);
            // 
            // countableTypeToolStripMenuItem
            // 
            this.countableTypeToolStripMenuItem.Name = "countableTypeToolStripMenuItem";
            this.countableTypeToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.countableTypeToolStripMenuItem.Text = "Countable Type";
            this.countableTypeToolStripMenuItem.Click += new System.EventHandler(this.countableTypeToolStripMenuItem_Click);
            // 
            // reportTypeToolStripMenuItem
            // 
            this.reportTypeToolStripMenuItem.Name = "reportTypeToolStripMenuItem";
            this.reportTypeToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.reportTypeToolStripMenuItem.Text = "Report Type";
            this.reportTypeToolStripMenuItem.Visible = false;
            // 
            // measurementTypeToolStripMenuItem
            // 
            this.measurementTypeToolStripMenuItem.Name = "measurementTypeToolStripMenuItem";
            this.measurementTypeToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.measurementTypeToolStripMenuItem.Text = "Measurement Type";
            this.measurementTypeToolStripMenuItem.Click += new System.EventHandler(this.measurementTypeToolStripMenuItem_Click);
            // 
            // propertyValueToolStripMenuItem
            // 
            this.propertyValueToolStripMenuItem.Name = "propertyValueToolStripMenuItem";
            this.propertyValueToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.propertyValueToolStripMenuItem.Text = "Property Value";
            this.propertyValueToolStripMenuItem.Click += new System.EventHandler(this.propertyValueToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.containerTypesToolStripMenuItem,
            this.countableTypesToolStripMenuItem,
            this.reportTypesToolStripMenuItem,
            this.measurementTypesToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // containerTypesToolStripMenuItem
            // 
            this.containerTypesToolStripMenuItem.Name = "containerTypesToolStripMenuItem";
            this.containerTypesToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.containerTypesToolStripMenuItem.Text = "Container Types";
            this.containerTypesToolStripMenuItem.Click += new System.EventHandler(this.containerTypesToolStripMenuItem_Click);
            // 
            // countableTypesToolStripMenuItem
            // 
            this.countableTypesToolStripMenuItem.Name = "countableTypesToolStripMenuItem";
            this.countableTypesToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.countableTypesToolStripMenuItem.Text = "Countable Types";
            this.countableTypesToolStripMenuItem.Click += new System.EventHandler(this.countableTypesToolStripMenuItem_Click);
            // 
            // reportTypesToolStripMenuItem
            // 
            this.reportTypesToolStripMenuItem.Name = "reportTypesToolStripMenuItem";
            this.reportTypesToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.reportTypesToolStripMenuItem.Text = "Report Types";
            this.reportTypesToolStripMenuItem.Click += new System.EventHandler(this.reportTypesToolStripMenuItem_Click);
            // 
            // measurementTypesToolStripMenuItem
            // 
            this.measurementTypesToolStripMenuItem.Name = "measurementTypesToolStripMenuItem";
            this.measurementTypesToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.measurementTypesToolStripMenuItem.Text = "Measurement Types";
            this.measurementTypesToolStripMenuItem.Click += new System.EventHandler(this.measurementTypesToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.optionsToolStripMenuItem.Text = "Properties";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.toolStripMenuItem2,
           // this.lTERSoftwareToolStripMenuItem,
           // this.toolStripMenuItem3,
            this.aboutZ3SchemaEditorToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.contentsToolStripMenuItem.Text = "Z4 Schema Editor Documentation";
            this.contentsToolStripMenuItem.Click += new System.EventHandler(this.contentsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(216, 6);
            // 
            // lTERSoftwareToolStripMenuItem
            // 
            //this.lTERSoftwareToolStripMenuItem.Name = "lTERSoftwareToolStripMenuItem";
            //this.lTERSoftwareToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            //this.lTERSoftwareToolStripMenuItem.Text = "NTL LTER Software Website";
            //this.lTERSoftwareToolStripMenuItem.Click += new System.EventHandler(this.lTERSoftwareToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(216, 6);
            // 
            // aboutZ3SchemaEditorToolStripMenuItem
            // 
            this.aboutZ3SchemaEditorToolStripMenuItem.Name = "aboutZ3SchemaEditorToolStripMenuItem";
            this.aboutZ3SchemaEditorToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.aboutZ3SchemaEditorToolStripMenuItem.Text = "About Z4 Schema Editor...";
            this.aboutZ3SchemaEditorToolStripMenuItem.Click += new System.EventHandler(this.aboutZ3SchemaEditorToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.containersPage);
            this.tabControl1.Controls.Add(this.countablesPage);
            this.tabControl1.Controls.Add(this.measurementsPage);
            this.tabControl1.Controls.Add(this.propertiesPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(562, 300);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.Visible = false;
            // 
            // containersPage
            // 
            this.containersPage.Controls.Add(this.containerList);
            this.containersPage.Location = new System.Drawing.Point(4, 22);
            this.containersPage.Name = "containersPage";
            this.containersPage.Padding = new System.Windows.Forms.Padding(3);
            this.containersPage.Size = new System.Drawing.Size(554, 274);
            this.containersPage.TabIndex = 0;
            this.containersPage.Text = "Containers";
            this.containersPage.UseVisualStyleBackColor = true;
            // 
            // containerList
            // 
            this.containerList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn});
            this.containerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerList.FullRowSelect = true;
            this.containerList.Location = new System.Drawing.Point(3, 3);
            this.containerList.Name = "containerList";
            this.containerList.Size = new System.Drawing.Size(548, 268);
            this.containerList.TabIndex = 0;
            this.containerList.UseCompatibleStateImageBehavior = false;
            this.containerList.View = System.Windows.Forms.View.Details;
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 150;
            // 
            // countablesPage
            // 
            this.countablesPage.Controls.Add(this.countableList);
            this.countablesPage.Location = new System.Drawing.Point(4, 22);
            this.countablesPage.Name = "countablesPage";
            this.countablesPage.Padding = new System.Windows.Forms.Padding(3);
            this.countablesPage.Size = new System.Drawing.Size(554, 274);
            this.countablesPage.TabIndex = 1;
            this.countablesPage.Text = "Countables";
            this.countablesPage.UseVisualStyleBackColor = true;
            // 
            // countableList
            // 
            this.countableList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.countNameColumn});
            this.countableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.countableList.FullRowSelect = true;
            this.countableList.Location = new System.Drawing.Point(3, 3);
            this.countableList.Name = "countableList";
            this.countableList.Size = new System.Drawing.Size(548, 268);
            this.countableList.TabIndex = 0;
            this.countableList.UseCompatibleStateImageBehavior = false;
            this.countableList.View = System.Windows.Forms.View.Details;
            // 
            // countNameColumn
            // 
            this.countNameColumn.Text = "Name";
            this.countNameColumn.Width = 150;
            // 
            // measurementsPage
            // 
            this.measurementsPage.Controls.Add(this.mtypeList);
            this.measurementsPage.Location = new System.Drawing.Point(4, 22);
            this.measurementsPage.Name = "measurementsPage";
            this.measurementsPage.Padding = new System.Windows.Forms.Padding(3);
            this.measurementsPage.Size = new System.Drawing.Size(554, 274);
            this.measurementsPage.TabIndex = 4;
            this.measurementsPage.Text = "Measurement Types";
            this.measurementsPage.UseVisualStyleBackColor = true;
            // 
            // mtypeList
            // 
            this.mtypeList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.mtypeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtypeList.FullRowSelect = true;
            this.mtypeList.Location = new System.Drawing.Point(3, 3);
            this.mtypeList.Name = "mtypeList";
            this.mtypeList.Size = new System.Drawing.Size(548, 268);
            this.mtypeList.TabIndex = 1;
            this.mtypeList.UseCompatibleStateImageBehavior = false;
            this.mtypeList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 150;
            // 
            // propertiesPage
            // 
            this.propertiesPage.Controls.Add(this.propsList);
            this.propertiesPage.Location = new System.Drawing.Point(4, 22);
            this.propertiesPage.Name = "propertiesPage";
            this.propertiesPage.Padding = new System.Windows.Forms.Padding(3);
            this.propertiesPage.Size = new System.Drawing.Size(554, 274);
            this.propertiesPage.TabIndex = 3;
            this.propertiesPage.Text = "Properties";
            this.propertiesPage.UseVisualStyleBackColor = true;
            // 
            // propsList
            // 
            this.propsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.propNameColumn,
            this.propValueColumn});
            this.propsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propsList.FullRowSelect = true;
            this.propsList.Location = new System.Drawing.Point(3, 3);
            this.propsList.Name = "propsList";
            this.propsList.Size = new System.Drawing.Size(548, 268);
            this.propsList.TabIndex = 0;
            this.propsList.UseCompatibleStateImageBehavior = false;
            this.propsList.View = System.Windows.Forms.View.Details;
            // 
            // propNameColumn
            // 
            this.propNameColumn.Text = "Name";
            this.propNameColumn.Width = 150;
            // 
            // propValueColumn
            // 
            this.propValueColumn.Text = "Value";
            this.propValueColumn.Width = 150;
            // 
            // reportsPage
            // 
            this.reportsPage.Controls.Add(this.reportList);
            this.reportsPage.Location = new System.Drawing.Point(4, 22);
            this.reportsPage.Name = "reportsPage";
            this.reportsPage.Padding = new System.Windows.Forms.Padding(3);
            this.reportsPage.Size = new System.Drawing.Size(554, 274);
            this.reportsPage.TabIndex = 2;
            this.reportsPage.Text = "Report Types";
            this.reportsPage.UseVisualStyleBackColor = true;
            // 
            // reportList
            // 
            this.reportList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.reportNameColumn});
            this.reportList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportList.FullRowSelect = true;
            this.reportList.Location = new System.Drawing.Point(3, 3);
            this.reportList.Name = "reportList";
            this.reportList.Size = new System.Drawing.Size(548, 268);
            this.reportList.TabIndex = 0;
            this.reportList.UseCompatibleStateImageBehavior = false;
            this.reportList.View = System.Windows.Forms.View.Details;
            // 
            // reportNameColumn
            // 
            this.reportNameColumn.Text = "Name";
            this.reportNameColumn.Width = 150;
            // 
            // editDeleteMenu
            // 
            this.editDeleteMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.editDeleteMenu.Name = "contextMenuStrip1";
            this.editDeleteMenu.Size = new System.Drawing.Size(117, 48);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // editDeleteUpDownMenu
            // 
            this.editDeleteUpDownMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem1,
            this.toolStripMenuItem4,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.toolStripMenuItem5,
            this.deleteToolStripMenuItem1});
            this.editDeleteUpDownMenu.Name = "editDeleteUpDownMenu";
            this.editDeleteUpDownMenu.Size = new System.Drawing.Size(142, 104);
            // 
            // editToolStripMenuItem1
            // 
            this.editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            this.editToolStripMenuItem1.Size = new System.Drawing.Size(141, 22);
            this.editToolStripMenuItem1.Text = "Edit";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(138, 6);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(138, 6);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(141, 22);
            this.deleteToolStripMenuItem1.Text = "Delete";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Z3 Schemas|*.z3s|All files|*.*";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Z3 Schemas|*.z3s|All files|*.*";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 324);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Z3 Schema Editor";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.containersPage.ResumeLayout(false);
            this.countablesPage.ResumeLayout(false);
            this.measurementsPage.ResumeLayout(false);
            this.propertiesPage.ResumeLayout(false);
            this.reportsPage.ResumeLayout(false);
            this.editDeleteMenu.ResumeLayout(false);
            this.editDeleteUpDownMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem containerTypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem countableTypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportTypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem lTERSoftwareToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem aboutZ3SchemaEditorToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage containersPage;
        private System.Windows.Forms.TabPage countablesPage;
        private System.Windows.Forms.TabPage reportsPage;
        private System.Windows.Forms.TabPage propertiesPage;
        private System.Windows.Forms.ListView containerList;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ListView countableList;
        private System.Windows.Forms.ColumnHeader countNameColumn;
        private System.Windows.Forms.ListView reportList;
        private System.Windows.Forms.ColumnHeader reportNameColumn;
        private System.Windows.Forms.ListView propsList;
        private System.Windows.Forms.ColumnHeader propNameColumn;
        private System.Windows.Forms.ColumnHeader propValueColumn;
        private System.Windows.Forms.TabPage measurementsPage;
        private System.Windows.Forms.ListView mtypeList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolStripMenuItem measurementTypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem containerTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem countableTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem measurementTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertyValueToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip editDeleteMenu;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip editDeleteUpDownMenu;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem2;
    }
}

