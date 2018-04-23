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
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports System.Data.OleDb
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid
Imports DevExpress.XtraTreeList
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraTreeList.Nodes
Imports DevExpress.XtraEditors.Controls

Namespace TreeListLookUp
	Partial Public Class Form1
		Inherits Form
		Private dt As DataTable

		Public Sub New()
			InitializeComponent()
		End Sub

		Public Enum DataMode
			Bound
			Unbound
			Virtual
		End Enum

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			Me.ordersTableAdapter.Fill(Me.nwindDataSet.Orders)

			Dim dt1 As New DataTable()
			dt1.Columns.Add("Word", GetType(String))
			dt1.Rows.Add()
			gridControl1.DataSource = dt1
			UseUnboundTest()
			GenerateTestDataTable()
			PopulateRadioGroup(radioGroup1, GetType(SearchMode))
			PopulateRadioGroup(radioGroup2, GetType(DataMode))

			radioGroup1.EditValue = SearchMode.AutoFilter
			radioGroup2.EditValue = DataMode.Unbound
		End Sub

		Private Sub PopulateRadioGroup(ByVal radioGroup As RadioGroup, ByVal enumType As Type)
			Dim values As Array = System.Enum.GetValues(enumType)
			For Each val As Object In values
				radioGroup.Properties.Items.Add(New RadioGroupItem(val, val.ToString()))
			Next val
		End Sub

		Private Sub OnSearchModeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioGroup1.SelectedIndexChanged
			Dim radioGroup As RadioGroup = TryCast(sender, RadioGroup)
			repositoryItemTreeListLookUpEdit1.SearchMode = CType(radioGroup.EditValue, SearchMode)
			treeListLookUpEdit1.Properties.SearchMode = repositoryItemTreeListLookUpEdit1.SearchMode
		End Sub

		Private virtualMode As Boolean = False
		Private ID As Integer = 2
		Private Sub OnDataModeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioGroup2.SelectedIndexChanged
			Dim radioGroup As RadioGroup = TryCast(sender, RadioGroup)
			Dim mode As DataMode = CType(radioGroup.EditValue, DataMode)
			If mode = DataMode.Bound Then
				PopulateBoundTree()
			ElseIf mode = DataMode.Unbound Then
				PopulateUnboundTree()
			Else
				PopulateVirtualTree()
			End If
		End Sub

		Private Sub PopulateBoundTree()
			treeListLookUpEdit1.Properties.TreeList.Columns.Clear()
			treeListLookUpEdit1.Properties.DataSource = Nothing
			treeListLookUpEdit1.Properties.DataSource = ordersBindingSource
			treeListLookUpEdit1.Properties.ValueMember = "OrderID"
			treeListLookUpEdit1.Properties.DisplayMember = "ShipName"
			virtualMode = False
			ID = 2
			RemoveHandler treeListLookUpEdit1.Properties.TreeList.BeforeExpand, AddressOf TreeList_BeforeExpand
		End Sub
		Private Sub PopulateUnboundTree()
			UseUnboundTest()
			virtualMode = False
			ID = 2
			RemoveHandler treeListLookUpEdit1.Properties.TreeList.BeforeExpand, AddressOf TreeList_BeforeExpand
		End Sub
		Private Sub PopulateVirtualTree()
			virtualMode = True
			treeListLookUpEdit1.Properties.TreeList.Nodes.Clear()
			treeListLookUpEdit1.Properties.TreeList.Columns.Clear()
			treeListLookUpEdit1.Properties.DataSource = Nothing
			GenerateTestColumns()
			GenerateTestRootNodes()
			AddHandler treeListLookUpEdit1.Properties.TreeList.BeforeExpand, AddressOf TreeList_BeforeExpand
		End Sub
		Private Sub UseUnboundTest()
			treeListLookUpEdit1.Properties.TreeList.Nodes.Clear()
			treeListLookUpEdit1.Properties.TreeList.Columns.Clear()
			treeListLookUpEdit1.Properties.DataSource = Nothing
			GenerateTestColumns()
			GenerateTestNodes()
		End Sub

		Private Sub treeListLookUpEdit1_ProcessNewValue(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles treeListLookUpEdit1.ProcessNewValue
			Dim TreeList As TreeList = treeListLookUpEdit1.Properties.TreeList
			If virtualMode Then
				e.Handled = True
                ID += 1
                Dim NullNode As TreeListNode = Nothing
                TreeList.AppendNode(New Object() {e.DisplayValue, "Little-Medium", ID}, NullNode)
			End If
		End Sub

		Private Sub TreeList_BeforeExpand(ByVal sender As Object, ByVal e As BeforeExpandEventArgs)
			GenerateTestChildrenNodes(e.Node)
		End Sub
		#Region "Populating with test nodes"
		Private Sub GenerateTestNodes()
            Dim TreeList As TreeList = treeListLookUpEdit1.Properties.TreeList
            Dim NullNode As TreeListNode = Nothing
			TreeList.BeginUnboundLoad()
            Dim rootNode As TreeListNode = TreeList.AppendNode(New Object() {"Mammals", "Little-Huge", 1}, NullNode)
			TreeList.AppendNode(New Object() { "Polar Bear", "Huge", 2 }, rootNode)
			Dim cats As TreeListNode = TreeList.AppendNode(New Object() { "Cats", "Little-Medium", 3 }, rootNode)
			TreeList.AppendNode(New Object() { "Tiger", "Medium", 4 }, cats)
			TreeList.AppendNode(New Object() { "Lion", "Medium", 5 }, cats)
			TreeList.AppendNode(New Object() { "Panther", "Medium", 6 }, cats)
			TreeList.AppendNode(New Object() { "Persian cat", "Little", 7 }, cats)
			TreeList.AppendNode(New Object() { "Wolf", "Medium", 8 }, rootNode)
            Dim birds As TreeListNode = TreeList.AppendNode(New Object() {"Birds", "Little-Medium", 9}, NullNode)
			TreeList.AppendNode(New Object() { "Dove", "Little", 10 }, birds)
			TreeList.AppendNode(New Object() { "Lark", "Little", 11 }, birds)
			TreeList.AppendNode(New Object() { "Sparrow", "Little", 12 }, birds)
			TreeList.EndUnboundLoad()
			treeListLookUpEdit1.Properties.ValueMember = "AnimalID"
			treeListLookUpEdit1.Properties.DisplayMember = "Name"
		End Sub
		Private Sub GenerateTestRootNodes()
            Dim TreeList As TreeList = treeListLookUpEdit1.Properties.TreeList
            Dim NullNode As TreeListNode = Nothing
			TreeList.BeginUnboundLoad()
            Dim rootNode As TreeListNode = TreeList.AppendNode(New Object() {"Mammals", "Little-Huge", 1}, NullNode)
			rootNode.HasChildren = True
            Dim birds As TreeListNode = TreeList.AppendNode(New Object() {"Birds", "Little-Medium", 2}, NullNode)
			birds.HasChildren = True
			TreeList.EndUnboundLoad()
			treeListLookUpEdit1.Properties.ValueMember = "AnimalID"
			treeListLookUpEdit1.Properties.DisplayMember = "Name"
		End Sub
		Private Sub GenerateTestChildrenNodes(ByVal parent As TreeListNode)
			Dim TreeList As TreeList = treeListLookUpEdit1.Properties.TreeList
			If parent.GetDisplayText("Name") = "Mammals" Then
				ID += 1
				Dim node As TreeListNode = TreeList.AppendNode(New Object() { "Fox", "Medium", ID }, parent)
				node.HasChildren = True
				ID += 1
				node = TreeList.AppendNode(New Object() { "Panda", "Medium-Huge", ID }, parent)
				node.HasChildren = True
			Else
				If parent.GetDisplayText("Name") = "Birds" Then
					ID += 1
					Dim node As TreeListNode = TreeList.AppendNode(New Object() { "Eagle", "Medium", ID }, parent)
					node.HasChildren = True
					ID += 1
					node = TreeList.AppendNode(New Object() { "Sparrow", "Little", ID }, parent)
					node.HasChildren = True
				Else
					ID += 1
					Dim node As TreeListNode = TreeList.AppendNode(New Object() { "New animal 1", "Medium", ID }, parent)
					node.HasChildren = True
					ID += 1
					node = TreeList.AppendNode(New Object() { "New animal 2", "Little", ID }, parent)
					node.HasChildren = True
				End If
			End If
		End Sub
		Private Sub GenerateTestColumns()
			Dim TreeList As TreeList = treeListLookUpEdit1.Properties.TreeList
			TreeList.BeginUpdate()
			TreeList.Columns.Add()
			TreeList.Columns(0).FieldName = "Name"
			TreeList.Columns(0).VisibleIndex = 0
			TreeList.Columns.Add()
			TreeList.Columns(1).FieldName = "Size"
			TreeList.Columns(1).VisibleIndex = 1
			TreeList.Columns.Add()
			TreeList.Columns(2).FieldName = "AnimalID"
			TreeList.Columns(2).VisibleIndex = 2
			TreeList.EndUpdate()
		End Sub
		Private Sub GenerateTestDataTable()
			dt = New DataTable()
			dt.Columns.Add("ID", GetType(Integer))
			dt.Columns.Add("ParentID", GetType(Integer))
			dt.Columns.Add("Word", GetType(String))
			dt.Rows.Add(1, 0, "Hello")
			dt.Rows.Add(2, 0, "This")
			dt.Rows.Add(3, 2, "is")
			dt.Rows.Add(4, 3, "a")
			dt.Rows.Add(5, 4, "Test")
			dt.Rows.Add(6, 4, "treeListLookUpEdit")
			dt.Rows.Add(7, 6, "editor")
			repositoryItemTreeListLookUpEdit1.DataSource = dt
			repositoryItemTreeListLookUpEdit1.DisplayMember = "Word"
			repositoryItemTreeListLookUpEdit1.ValueMember = "Word"
		End Sub
		#End Region  



	End Class
End Namespace
