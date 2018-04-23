// Developer Express Code Central Example:
// How to create an analog of GridLookUpEdit with TreeList in a popup window
// 
// In some cases, it is useful to display data within LookUpEdit in the
// hierarchical structure. This can be introduced by creating a custom editor –
// TreeListLookUpEdit. This example illustrates how to create such an editor. Here
// we have implemented the following features:
// 
// - The
// RepositoryItemTreeListLookUpEdit.ValueMember and
// RepositoryItemTreeListLookUpEdit.DisplayMember properties. They are intended for
// the same functions as in a regular LookUpEdit/GridLookUpEdit.
// - The
// ProcessNewValue event is implemented like in LookUpEdit/GridLookUpEdit. This
// event is raised when an editor is validated and its display value does not exist
// within an inner TreeList.
// - The property SearchMode allows you to select one of
// the following modes:
// o OnlyInPopup. This mode is an analog of
// IncrementalSearch, but it also expands nodes if it is needed when you type a
// search text. This mode works only if a popup window is shown. o AutoComplete. In
// the Auto Completion mode, the text in the edit box is automatically completed if
// it matches a DisplayMember field value from drop-down nodes.
// o AutoFilter.
// This mode applies a filter to the DisplayMember column. Filter is formed when
// you type a text in the edit box. In addition, it opens a popup window and looks
// for nodes retaining paths to the root to show the context.
// For virtual data
// loading (on demand) in the inner TreeList you can handle its BeforeExpand event.
// Please refer to the following help articles:
// 
// How to: Implement Dynamic
// Loading in Unbound Mode
// (ms-help://DevExpress.NETv12.1/DevExpress.WindowsForms/CustomDocument325.htm)
// Dynamic
// Data Loading via Events
// (ms-help://DevExpress.NETv12.1/DevExpress.WindowsForms/CustomDocument5560.htm)
// How
// to: Implement a Tree Structure for a Business Object
// (ms-help://DevExpress.NETv12.1/DevExpress.WindowsForms/CustomDocument5561.htm)
// to learn which virtual Modes TreeList supports.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E4319

using System.ComponentModel;
namespace TreeListLookUp
{
    partial class Form1
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
            this.ordersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nwindDataSet = new TreeListLookUp.nwindDataSet();
            this.treeListLookUpEdit1 = new TreeListLookUp.TreeListLookUpEdit();
            this.gridLookUpEdit1 = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colOrderID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCustomerID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmployeeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrderDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRequiredDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShippedDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShipVia = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFreight = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShipName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShipAddress = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShipCity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShipRegion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShipPostalCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShipCountry = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTreeListLookUpEdit1 = new TreeListLookUp.RepositoryItemTreeListLookUpEdit();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioGroup2 = new DevExpress.XtraEditors.RadioGroup();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.ordersTableAdapter = new TreeListLookUp.nwindDataSetTableAdapters.OrdersTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.ordersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nwindDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListLookUpEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTreeListLookUpEdit1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup2.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ordersBindingSource
            // 
            this.ordersBindingSource.DataMember = "Orders";
            this.ordersBindingSource.DataSource = this.nwindDataSet;
            // 
            // nwindDataSet
            // 
            this.nwindDataSet.DataSetName = "nwindDataSet";
            this.nwindDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // treeListLookUpEdit1
            // 
            this.treeListLookUpEdit1.EditValue = "";
            this.treeListLookUpEdit1.Location = new System.Drawing.Point(421, 12);
            this.treeListLookUpEdit1.Name = "treeListLookUpEdit1";
            this.treeListLookUpEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.treeListLookUpEdit1.Properties.DisplayMember = "ShipName";
            this.treeListLookUpEdit1.Properties.ImmediatePopup = true;
            this.treeListLookUpEdit1.Properties.ImmediatePopupForm = true;
            this.treeListLookUpEdit1.Properties.NullText = "123";
            this.treeListLookUpEdit1.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
            this.treeListLookUpEdit1.Properties.ValueMember = "OrderID";
            this.treeListLookUpEdit1.Size = new System.Drawing.Size(375, 20);
            this.treeListLookUpEdit1.TabIndex = 2;
            this.treeListLookUpEdit1.ProcessNewValue += new DevExpress.XtraEditors.Controls.ProcessNewValueEventHandler(this.treeListLookUpEdit1_ProcessNewValue);
            // 
            // gridLookUpEdit1
            // 
            this.gridLookUpEdit1.Location = new System.Drawing.Point(157, 192);
            this.gridLookUpEdit1.Name = "gridLookUpEdit1";
            this.gridLookUpEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridLookUpEdit1.Properties.DisplayMember = "ShipName";
            this.gridLookUpEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.gridLookUpEdit1.Properties.View = this.gridLookUpEdit1View;
            this.gridLookUpEdit1.Size = new System.Drawing.Size(248, 20);
            this.gridLookUpEdit1.TabIndex = 4;
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colOrderID,
            this.colCustomerID,
            this.colEmployeeID,
            this.colOrderDate,
            this.colRequiredDate,
            this.colShippedDate,
            this.colShipVia,
            this.colFreight,
            this.colShipName,
            this.colShipAddress,
            this.colShipCity,
            this.colShipRegion,
            this.colShipPostalCode,
            this.colShipCountry});
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsBehavior.AllowIncrementalSearch = true;
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // colOrderID
            // 
            this.colOrderID.FieldName = "OrderID";
            this.colOrderID.Name = "colOrderID";
            this.colOrderID.OptionsColumn.ReadOnly = true;
            this.colOrderID.Visible = true;
            this.colOrderID.VisibleIndex = 0;
            // 
            // colCustomerID
            // 
            this.colCustomerID.FieldName = "CustomerID";
            this.colCustomerID.Name = "colCustomerID";
            this.colCustomerID.Visible = true;
            this.colCustomerID.VisibleIndex = 1;
            // 
            // colEmployeeID
            // 
            this.colEmployeeID.FieldName = "EmployeeID";
            this.colEmployeeID.Name = "colEmployeeID";
            this.colEmployeeID.Visible = true;
            this.colEmployeeID.VisibleIndex = 2;
            // 
            // colOrderDate
            // 
            this.colOrderDate.FieldName = "OrderDate";
            this.colOrderDate.Name = "colOrderDate";
            this.colOrderDate.Visible = true;
            this.colOrderDate.VisibleIndex = 3;
            // 
            // colRequiredDate
            // 
            this.colRequiredDate.FieldName = "RequiredDate";
            this.colRequiredDate.Name = "colRequiredDate";
            this.colRequiredDate.Visible = true;
            this.colRequiredDate.VisibleIndex = 4;
            // 
            // colShippedDate
            // 
            this.colShippedDate.FieldName = "ShippedDate";
            this.colShippedDate.Name = "colShippedDate";
            this.colShippedDate.Visible = true;
            this.colShippedDate.VisibleIndex = 5;
            // 
            // colShipVia
            // 
            this.colShipVia.FieldName = "ShipVia";
            this.colShipVia.Name = "colShipVia";
            this.colShipVia.Visible = true;
            this.colShipVia.VisibleIndex = 6;
            // 
            // colFreight
            // 
            this.colFreight.FieldName = "Freight";
            this.colFreight.Name = "colFreight";
            this.colFreight.Visible = true;
            this.colFreight.VisibleIndex = 7;
            // 
            // colShipName
            // 
            this.colShipName.FieldName = "ShipName";
            this.colShipName.Name = "colShipName";
            this.colShipName.Visible = true;
            this.colShipName.VisibleIndex = 8;
            // 
            // colShipAddress
            // 
            this.colShipAddress.FieldName = "ShipAddress";
            this.colShipAddress.Name = "colShipAddress";
            this.colShipAddress.Visible = true;
            this.colShipAddress.VisibleIndex = 9;
            // 
            // colShipCity
            // 
            this.colShipCity.FieldName = "ShipCity";
            this.colShipCity.Name = "colShipCity";
            this.colShipCity.Visible = true;
            this.colShipCity.VisibleIndex = 10;
            // 
            // colShipRegion
            // 
            this.colShipRegion.FieldName = "ShipRegion";
            this.colShipRegion.Name = "colShipRegion";
            this.colShipRegion.Visible = true;
            this.colShipRegion.VisibleIndex = 11;
            // 
            // colShipPostalCode
            // 
            this.colShipPostalCode.FieldName = "ShipPostalCode";
            this.colShipPostalCode.Name = "colShipPostalCode";
            this.colShipPostalCode.Visible = true;
            this.colShipPostalCode.VisibleIndex = 12;
            // 
            // colShipCountry
            // 
            this.colShipCountry.FieldName = "ShipCountry";
            this.colShipCountry.Name = "colShipCountry";
            this.colShipCountry.Visible = true;
            this.colShipCountry.VisibleIndex = 13;
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(15, 15);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTreeListLookUpEdit1});
            this.gridControl1.Size = new System.Drawing.Size(400, 200);
            this.gridControl1.TabIndex = 11;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Word";
            this.gridColumn1.ColumnEdit = this.repositoryItemTreeListLookUpEdit1;
            this.gridColumn1.FieldName = "Word";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // repositoryItemTreeListLookUpEdit1
            // 
            this.repositoryItemTreeListLookUpEdit1.AutoHeight = false;
            this.repositoryItemTreeListLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemTreeListLookUpEdit1.DisplayMember = "Word";
            this.repositoryItemTreeListLookUpEdit1.ImmediatePopup = true;
            this.repositoryItemTreeListLookUpEdit1.ImmediatePopupForm = true;
            this.repositoryItemTreeListLookUpEdit1.Name = "repositoryItemTreeListLookUpEdit1";
            this.repositoryItemTreeListLookUpEdit1.NullText = "[EditValue is null]";
            this.repositoryItemTreeListLookUpEdit1.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
            this.repositoryItemTreeListLookUpEdit1.ValueMember = "ID";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioGroup2);
            this.groupBox1.Location = new System.Drawing.Point(802, 115);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(105, 100);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mode:";
            // 
            // radioGroup2
            // 
            this.radioGroup2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioGroup2.Location = new System.Drawing.Point(3, 16);
            this.radioGroup2.Name = "radioGroup2";
            this.radioGroup2.Size = new System.Drawing.Size(99, 81);
            this.radioGroup2.TabIndex = 19;
            this.radioGroup2.SelectedIndexChanged += new System.EventHandler(this.OnDataModeChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioGroup1);
            this.groupBox2.Location = new System.Drawing.Point(802, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(105, 100);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Search Mode:";
            // 
            // radioGroup1
            // 
            this.radioGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioGroup1.Location = new System.Drawing.Point(3, 16);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(99, 81);
            this.radioGroup1.TabIndex = 18;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.OnSearchModeChanged);
            // 
            // ordersTableAdapter
            // 
            this.ordersTableAdapter.ClearBeforeFill = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 229);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.gridLookUpEdit1);
            this.Controls.Add(this.treeListLookUpEdit1);
            this.Name = "Form1";
            this.Text = "TreeListLookUpEdit Example";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ordersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nwindDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListLookUpEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTreeListLookUpEdit1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup2.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TreeListLookUpEdit treeListLookUpEdit1;
        private DevExpress.XtraEditors.GridLookUpEdit gridLookUpEdit1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn colOrderID;
        private DevExpress.XtraGrid.Columns.GridColumn colCustomerID;
        private DevExpress.XtraGrid.Columns.GridColumn colEmployeeID;
        private DevExpress.XtraGrid.Columns.GridColumn colOrderDate;
        private DevExpress.XtraGrid.Columns.GridColumn colRequiredDate;
        private DevExpress.XtraGrid.Columns.GridColumn colShippedDate;
        private DevExpress.XtraGrid.Columns.GridColumn colShipVia;
        private DevExpress.XtraGrid.Columns.GridColumn colFreight;
        private DevExpress.XtraGrid.Columns.GridColumn colShipName;
        private DevExpress.XtraGrid.Columns.GridColumn colShipAddress;
        private DevExpress.XtraGrid.Columns.GridColumn colShipCity;
        private DevExpress.XtraGrid.Columns.GridColumn colShipRegion;
        private DevExpress.XtraGrid.Columns.GridColumn colShipPostalCode;
        private DevExpress.XtraGrid.Columns.GridColumn colShipCountry;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private RepositoryItemTreeListLookUpEdit repositoryItemTreeListLookUpEdit1;
        private System.Windows.Forms.BindingSource ordersBindingSource;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.RadioGroup radioGroup2;
        private nwindDataSet nwindDataSet;
        private nwindDataSetTableAdapters.OrdersTableAdapter ordersTableAdapter;

    }
}

