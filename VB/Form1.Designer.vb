' Developer Express Code Central Example:
' How to create an analog of GridLookUpEdit with TreeList in a popup window
' 
' In some cases, it is useful to display data within LookUpEdit in the
' hierarchical structure. This can be introduced by creating a custom editor –
' TreeListLookUpEdit. This example illustrates how to create such an editor. Here
' we have implemented the following features:
' 
' - The
' RepositoryItemTreeListLookUpEdit.ValueMember and
' RepositoryItemTreeListLookUpEdit.DisplayMember properties. They are intended for
' the same functions as in a regular LookUpEdit/GridLookUpEdit.
' - The
' ProcessNewValue event is implemented like in LookUpEdit/GridLookUpEdit. This
' event is raised when an editor is validated and its display value does not exist
' within an inner TreeList.
' - The property SearchMode allows you to select one of
' the following modes:
' o OnlyInPopup. This mode is an analog of
' IncrementalSearch, but it also expands nodes if it is needed when you type a
' search text. This mode works only if a popup window is shown. o AutoComplete. In
' the Auto Completion mode, the text in the edit box is automatically completed if
' it matches a DisplayMember field value from drop-down nodes.
' o AutoFilter.
' This mode applies a filter to the DisplayMember column. Filter is formed when
' you type a text in the edit box. In addition, it opens a popup window and looks
' for nodes retaining paths to the root to show the context.
' For virtual data
' loading (on demand) in the inner TreeList you can handle its BeforeExpand event.
' Please refer to the following help articles:
' 
' How to: Implement Dynamic
' Loading in Unbound Mode
' (ms-help://DevExpress.NETv12.1/DevExpress.WindowsForms/CustomDocument325.htm)
' Dynamic
' Data Loading via Events
' (ms-help://DevExpress.NETv12.1/DevExpress.WindowsForms/CustomDocument5560.htm)
' How
' to: Implement a Tree Structure for a Business Object
' (ms-help://DevExpress.NETv12.1/DevExpress.WindowsForms/CustomDocument5561.htm)
' to learn which virtual Modes TreeList supports.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E4319


Imports Microsoft.VisualBasic
Imports System.ComponentModel
Namespace TreeListLookUp
	Partial Public Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Me.ordersBindingSource = New System.Windows.Forms.BindingSource(Me.components)
			Me.nwindDataSet = New TreeListLookUp.nwindDataSet()
			Me.treeListLookUpEdit1 = New TreeListLookUp.TreeListLookUpEdit()
			Me.gridLookUpEdit1 = New DevExpress.XtraEditors.GridLookUpEdit()
			Me.gridLookUpEdit1View = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.colOrderID = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.colCustomerID = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.colEmployeeID = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.colOrderDate = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.colRequiredDate = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.colShippedDate = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.colShipVia = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.colFreight = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.colShipName = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.colShipAddress = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.colShipCity = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.colShipRegion = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.colShipPostalCode = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.colShipCountry = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.gridControl1 = New DevExpress.XtraGrid.GridControl()
			Me.gridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.gridColumn1 = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.repositoryItemTreeListLookUpEdit1 = New TreeListLookUp.RepositoryItemTreeListLookUpEdit()
			Me.groupBox1 = New System.Windows.Forms.GroupBox()
			Me.radioGroup2 = New DevExpress.XtraEditors.RadioGroup()
			Me.groupBox2 = New System.Windows.Forms.GroupBox()
			Me.radioGroup1 = New DevExpress.XtraEditors.RadioGroup()
			Me.ordersTableAdapter = New TreeListLookUp.nwindDataSetTableAdapters.OrdersTableAdapter()
			CType(Me.ordersBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.nwindDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.treeListLookUpEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gridLookUpEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gridLookUpEdit1View, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gridView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.repositoryItemTreeListLookUpEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.groupBox1.SuspendLayout()
			CType(Me.radioGroup2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.groupBox2.SuspendLayout()
			CType(Me.radioGroup1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' ordersBindingSource
			' 
			Me.ordersBindingSource.DataMember = "Orders"
			Me.ordersBindingSource.DataSource = Me.nwindDataSet
			' 
			' nwindDataSet
			' 
			Me.nwindDataSet.DataSetName = "nwindDataSet"
			Me.nwindDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
			' 
			' treeListLookUpEdit1
			' 
			Me.treeListLookUpEdit1.EditValue = ""
			Me.treeListLookUpEdit1.Location = New System.Drawing.Point(421, 12)
			Me.treeListLookUpEdit1.Name = "treeListLookUpEdit1"
			Me.treeListLookUpEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() { New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.treeListLookUpEdit1.Properties.DisplayMember = "ShipName"
			Me.treeListLookUpEdit1.Properties.ImmediatePopup = True
			Me.treeListLookUpEdit1.Properties.ImmediatePopupForm = True
			Me.treeListLookUpEdit1.Properties.NullText = "123"
			Me.treeListLookUpEdit1.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter
			Me.treeListLookUpEdit1.Properties.ValueMember = "OrderID"
			Me.treeListLookUpEdit1.Size = New System.Drawing.Size(375, 20)
			Me.treeListLookUpEdit1.TabIndex = 2
'			Me.treeListLookUpEdit1.ProcessNewValue += New DevExpress.XtraEditors.Controls.ProcessNewValueEventHandler(Me.treeListLookUpEdit1_ProcessNewValue);
			' 
			' gridLookUpEdit1
			' 
			Me.gridLookUpEdit1.Location = New System.Drawing.Point(157, 192)
			Me.gridLookUpEdit1.Name = "gridLookUpEdit1"
			Me.gridLookUpEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() { New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.gridLookUpEdit1.Properties.DisplayMember = "ShipName"
			Me.gridLookUpEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
			Me.gridLookUpEdit1.Properties.View = Me.gridLookUpEdit1View
			Me.gridLookUpEdit1.Size = New System.Drawing.Size(248, 20)
			Me.gridLookUpEdit1.TabIndex = 4
			' 
			' gridLookUpEdit1View
			' 
			Me.gridLookUpEdit1View.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() { Me.colOrderID, Me.colCustomerID, Me.colEmployeeID, Me.colOrderDate, Me.colRequiredDate, Me.colShippedDate, Me.colShipVia, Me.colFreight, Me.colShipName, Me.colShipAddress, Me.colShipCity, Me.colShipRegion, Me.colShipPostalCode, Me.colShipCountry})
			Me.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gridLookUpEdit1View.Name = "gridLookUpEdit1View"
			Me.gridLookUpEdit1View.OptionsBehavior.AllowIncrementalSearch = True
			Me.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gridLookUpEdit1View.OptionsView.ShowGroupPanel = False
			' 
			' colOrderID
			' 
			Me.colOrderID.FieldName = "OrderID"
			Me.colOrderID.Name = "colOrderID"
			Me.colOrderID.OptionsColumn.ReadOnly = True
			Me.colOrderID.Visible = True
			Me.colOrderID.VisibleIndex = 0
			' 
			' colCustomerID
			' 
			Me.colCustomerID.FieldName = "CustomerID"
			Me.colCustomerID.Name = "colCustomerID"
			Me.colCustomerID.Visible = True
			Me.colCustomerID.VisibleIndex = 1
			' 
			' colEmployeeID
			' 
			Me.colEmployeeID.FieldName = "EmployeeID"
			Me.colEmployeeID.Name = "colEmployeeID"
			Me.colEmployeeID.Visible = True
			Me.colEmployeeID.VisibleIndex = 2
			' 
			' colOrderDate
			' 
			Me.colOrderDate.FieldName = "OrderDate"
			Me.colOrderDate.Name = "colOrderDate"
			Me.colOrderDate.Visible = True
			Me.colOrderDate.VisibleIndex = 3
			' 
			' colRequiredDate
			' 
			Me.colRequiredDate.FieldName = "RequiredDate"
			Me.colRequiredDate.Name = "colRequiredDate"
			Me.colRequiredDate.Visible = True
			Me.colRequiredDate.VisibleIndex = 4
			' 
			' colShippedDate
			' 
			Me.colShippedDate.FieldName = "ShippedDate"
			Me.colShippedDate.Name = "colShippedDate"
			Me.colShippedDate.Visible = True
			Me.colShippedDate.VisibleIndex = 5
			' 
			' colShipVia
			' 
			Me.colShipVia.FieldName = "ShipVia"
			Me.colShipVia.Name = "colShipVia"
			Me.colShipVia.Visible = True
			Me.colShipVia.VisibleIndex = 6
			' 
			' colFreight
			' 
			Me.colFreight.FieldName = "Freight"
			Me.colFreight.Name = "colFreight"
			Me.colFreight.Visible = True
			Me.colFreight.VisibleIndex = 7
			' 
			' colShipName
			' 
			Me.colShipName.FieldName = "ShipName"
			Me.colShipName.Name = "colShipName"
			Me.colShipName.Visible = True
			Me.colShipName.VisibleIndex = 8
			' 
			' colShipAddress
			' 
			Me.colShipAddress.FieldName = "ShipAddress"
			Me.colShipAddress.Name = "colShipAddress"
			Me.colShipAddress.Visible = True
			Me.colShipAddress.VisibleIndex = 9
			' 
			' colShipCity
			' 
			Me.colShipCity.FieldName = "ShipCity"
			Me.colShipCity.Name = "colShipCity"
			Me.colShipCity.Visible = True
			Me.colShipCity.VisibleIndex = 10
			' 
			' colShipRegion
			' 
			Me.colShipRegion.FieldName = "ShipRegion"
			Me.colShipRegion.Name = "colShipRegion"
			Me.colShipRegion.Visible = True
			Me.colShipRegion.VisibleIndex = 11
			' 
			' colShipPostalCode
			' 
			Me.colShipPostalCode.FieldName = "ShipPostalCode"
			Me.colShipPostalCode.Name = "colShipPostalCode"
			Me.colShipPostalCode.Visible = True
			Me.colShipPostalCode.VisibleIndex = 12
			' 
			' colShipCountry
			' 
			Me.colShipCountry.FieldName = "ShipCountry"
			Me.colShipCountry.Name = "colShipCountry"
			Me.colShipCountry.Visible = True
			Me.colShipCountry.VisibleIndex = 13
			' 
			' gridControl1
			' 
			Me.gridControl1.Location = New System.Drawing.Point(15, 15)
			Me.gridControl1.MainView = Me.gridView1
			Me.gridControl1.Name = "gridControl1"
			Me.gridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() { Me.repositoryItemTreeListLookUpEdit1})
			Me.gridControl1.Size = New System.Drawing.Size(400, 200)
			Me.gridControl1.TabIndex = 11
			Me.gridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() { Me.gridView1})
			' 
			' gridView1
			' 
			Me.gridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() { Me.gridColumn1})
			Me.gridView1.GridControl = Me.gridControl1
			Me.gridView1.Name = "gridView1"
			' 
			' gridColumn1
			' 
			Me.gridColumn1.Caption = "Word"
			Me.gridColumn1.ColumnEdit = Me.repositoryItemTreeListLookUpEdit1
			Me.gridColumn1.FieldName = "Word"
			Me.gridColumn1.Name = "gridColumn1"
			Me.gridColumn1.Visible = True
			Me.gridColumn1.VisibleIndex = 0
			' 
			' repositoryItemTreeListLookUpEdit1
			' 
			Me.repositoryItemTreeListLookUpEdit1.AutoHeight = False
			Me.repositoryItemTreeListLookUpEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() { New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.repositoryItemTreeListLookUpEdit1.DisplayMember = "Word"
			Me.repositoryItemTreeListLookUpEdit1.ImmediatePopup = True
			Me.repositoryItemTreeListLookUpEdit1.ImmediatePopupForm = True
			Me.repositoryItemTreeListLookUpEdit1.Name = "repositoryItemTreeListLookUpEdit1"
			Me.repositoryItemTreeListLookUpEdit1.NullText = "[EditValue is null]"
			Me.repositoryItemTreeListLookUpEdit1.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter
			Me.repositoryItemTreeListLookUpEdit1.ValueMember = "ID"
			' 
			' groupBox1
			' 
			Me.groupBox1.Controls.Add(Me.radioGroup2)
			Me.groupBox1.Location = New System.Drawing.Point(802, 115)
			Me.groupBox1.Name = "groupBox1"
			Me.groupBox1.Size = New System.Drawing.Size(105, 100)
			Me.groupBox1.TabIndex = 16
			Me.groupBox1.TabStop = False
			Me.groupBox1.Text = "Mode:"
			' 
			' radioGroup2
			' 
			Me.radioGroup2.Dock = System.Windows.Forms.DockStyle.Fill
			Me.radioGroup2.Location = New System.Drawing.Point(3, 16)
			Me.radioGroup2.Name = "radioGroup2"
			Me.radioGroup2.Size = New System.Drawing.Size(99, 81)
			Me.radioGroup2.TabIndex = 19
'			Me.radioGroup2.SelectedIndexChanged += New System.EventHandler(Me.OnDataModeChanged);
			' 
			' groupBox2
			' 
			Me.groupBox2.Controls.Add(Me.radioGroup1)
			Me.groupBox2.Location = New System.Drawing.Point(802, 15)
			Me.groupBox2.Name = "groupBox2"
			Me.groupBox2.Size = New System.Drawing.Size(105, 100)
			Me.groupBox2.TabIndex = 17
			Me.groupBox2.TabStop = False
			Me.groupBox2.Text = "Search Mode:"
			' 
			' radioGroup1
			' 
			Me.radioGroup1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.radioGroup1.Location = New System.Drawing.Point(3, 16)
			Me.radioGroup1.Name = "radioGroup1"
			Me.radioGroup1.Size = New System.Drawing.Size(99, 81)
			Me.radioGroup1.TabIndex = 18
'			Me.radioGroup1.SelectedIndexChanged += New System.EventHandler(Me.OnSearchModeChanged);
			' 
			' ordersTableAdapter
			' 
			Me.ordersTableAdapter.ClearBeforeFill = True
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(931, 229)
			Me.Controls.Add(Me.groupBox2)
			Me.Controls.Add(Me.groupBox1)
			Me.Controls.Add(Me.gridControl1)
			Me.Controls.Add(Me.gridLookUpEdit1)
			Me.Controls.Add(Me.treeListLookUpEdit1)
			Me.Name = "Form1"
			Me.Text = "TreeListLookUpEdit Example"
'			Me.Load += New System.EventHandler(Me.Form1_Load);
			CType(Me.ordersBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.nwindDataSet, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.treeListLookUpEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gridLookUpEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gridLookUpEdit1View, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gridControl1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gridView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.repositoryItemTreeListLookUpEdit1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.groupBox1.ResumeLayout(False)
			CType(Me.radioGroup2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.groupBox2.ResumeLayout(False)
			CType(Me.radioGroup1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private WithEvents treeListLookUpEdit1 As TreeListLookUpEdit
		Private gridLookUpEdit1 As DevExpress.XtraEditors.GridLookUpEdit
		Private gridLookUpEdit1View As DevExpress.XtraGrid.Views.Grid.GridView
		Private colOrderID As DevExpress.XtraGrid.Columns.GridColumn
		Private colCustomerID As DevExpress.XtraGrid.Columns.GridColumn
		Private colEmployeeID As DevExpress.XtraGrid.Columns.GridColumn
		Private colOrderDate As DevExpress.XtraGrid.Columns.GridColumn
		Private colRequiredDate As DevExpress.XtraGrid.Columns.GridColumn
		Private colShippedDate As DevExpress.XtraGrid.Columns.GridColumn
		Private colShipVia As DevExpress.XtraGrid.Columns.GridColumn
		Private colFreight As DevExpress.XtraGrid.Columns.GridColumn
		Private colShipName As DevExpress.XtraGrid.Columns.GridColumn
		Private colShipAddress As DevExpress.XtraGrid.Columns.GridColumn
		Private colShipCity As DevExpress.XtraGrid.Columns.GridColumn
		Private colShipRegion As DevExpress.XtraGrid.Columns.GridColumn
		Private colShipPostalCode As DevExpress.XtraGrid.Columns.GridColumn
		Private colShipCountry As DevExpress.XtraGrid.Columns.GridColumn
		Private gridControl1 As DevExpress.XtraGrid.GridControl
		Private gridView1 As DevExpress.XtraGrid.Views.Grid.GridView
		Private gridColumn1 As DevExpress.XtraGrid.Columns.GridColumn
		Private repositoryItemTreeListLookUpEdit1 As RepositoryItemTreeListLookUpEdit
		Private ordersBindingSource As System.Windows.Forms.BindingSource
		Private groupBox1 As System.Windows.Forms.GroupBox
		Private groupBox2 As System.Windows.Forms.GroupBox
		Private WithEvents radioGroup1 As DevExpress.XtraEditors.RadioGroup
		Private WithEvents radioGroup2 As DevExpress.XtraEditors.RadioGroup
		Private nwindDataSet As nwindDataSet
		Private ordersTableAdapter As nwindDataSetTableAdapters.OrdersTableAdapter

	End Class
End Namespace

