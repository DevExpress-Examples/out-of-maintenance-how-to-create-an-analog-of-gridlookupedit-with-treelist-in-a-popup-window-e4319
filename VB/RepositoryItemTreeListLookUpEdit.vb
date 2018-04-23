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
Imports System.Linq
Imports System.Text
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.Registrator
Imports System.ComponentModel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Popup
Imports DevExpress.XtraTreeList
Imports DevExpress.Utils
Imports DevExpress.XtraEditors.ListControls
Imports System.Windows.Forms
Imports DevExpress.XtraTreeList.Columns
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraTreeList.Nodes
Imports DevExpress.Data.Filtering
Imports System.Diagnostics
Imports System.Collections
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.ViewInfo

Namespace TreeListLookUp
	<UserRepositoryItem("Register")> _
	Public Class RepositoryItemTreeListLookUpEdit
		Inherits RepositoryItemPopupBaseAutoSearchEdit
		#Region "Register, Assign, Dispose, CheckDestroyDataSource"
		Friend Const EditorName As String = "TreeListLookUpEdit"

		Public Shared Sub Register()
            EditorRegistrationInfo.Default.Editors.Add(New EditorClassInfo(EditorName, GetType(TreeListLookUpEdit), GetType(RepositoryItemTreeListLookUpEdit), GetType(PopupBaseAutoSearchEditViewInfo), New ButtonEditPainter(), True))
		End Sub
		Shared Sub New()
			Register()
		End Sub
		Public Overrides ReadOnly Property EditorTypeName() As String
			Get
				Return EditorName
			End Get
		End Property

		Public Overrides Sub Assign(ByVal item As RepositoryItem)
			Dim source As RepositoryItemTreeListLookUpEdit = TryCast(item, RepositoryItemTreeListLookUpEdit)
			BeginUpdate()
			Try
				MyBase.Assign(item)
				Me.DisplayMember = source.DisplayMember
				Me.ValueMember = source.ValueMember
				Me.DataSource = source.DataSource
				Me.ImmediatePopupForm = source.ImmediatePopupForm
				Me.EnableFilter = source.EnableFilter
				Me.CaseSensitiveSearch = source.CaseSensitiveSearch
				Me.SearchMode = source.SearchMode
			Finally
				EndUpdate()
			End Try
		End Sub
		Protected Overrides Overloads Sub Dispose(ByVal disposing As Boolean)
			If disposing Then
				CheckDestroyDataSource()
			End If
			MyBase.Dispose(disposing)
		End Sub
		Private Sub CheckDestroyDataSource()
			Try
				If dataSource_Renamed IsNot Nothing Then
					Dim ds As IDisposable = TryCast(dataSource_Renamed, IDisposable)
					If ds IsNot Nothing Then
						ds.Dispose()
					End If
				End If

				Me.DataSource = Nothing
			Catch
			End Try
		End Sub
		#End Region
		' ==============================================================
		Private Const DefaultNullText As String = "[EditValue is null]"
		Private treeList_Renamed As TreeListInner
		' Fields...
		Private displayMember_Renamed As String
		Private valueMember_Renamed As String
		Private dataSource_Renamed As Object
		Private searchMode_Renamed As SearchMode
		Private caseSensitiveSearch_Renamed As Boolean
		Private enableFilter_Renamed As Boolean
		Private immediatePopupForm_Renamed As Boolean
        Private Shared ReadOnly _processNewValue As Object = New Object()

		Friend Property EnableFilter() As Boolean
			Get
				Return enableFilter_Renamed
			End Get
			Set(ByVal value As Boolean)
				enableFilter_Renamed = value
			End Set
		End Property

		Friend Property PrevFocusedNode() As TreeListNode

		Friend ReadOnly Property PropertiesStore() As IDictionary
			Get
				Return MyBase.PropertyStore
			End Get
		End Property

		Public ReadOnly Property TreeList() As TreeListInner
			Get
				Return treeList_Renamed
			End Get
		End Property

		#Region "Designer predefined Properties"
#If DXWhidbey Then
		<DXCategory(CategoryName.Data), System.ComponentModel.DefaultValue(Me.GetType(Object), Nothing), AttributeProvider(GetType(IListSource))> _
		Public Property DataSource() As Object
#Else
        <DXCategory(CategoryName.Data), System.ComponentModel.DefaultValue(GetType(Object), Nothing), TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design")> _
        Public Property DataSource() As Object
#End If
            Get
                Return dataSource_Renamed
            End Get
            Set(ByVal value As Object)
                If DataSource IsNot value Then
                    dataSource_Renamed = value
                    If TreeList.BindingContext Is Nothing Then
                        TreeList.BindingContext = New BindingContext()
                    End If
                    If TreeList.IsHandleCreated Then
                        TreeList.DataSource = value
                    End If
                    OnPropertiesChanged()
                End If
            End Set
        End Property

		<Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
		Public Property ImmediatePopupForm() As Boolean
			Get
				Return immediatePopupForm_Renamed
			End Get
			Set(ByVal value As Boolean)
				immediatePopupForm_Renamed = value
			End Set
		End Property

		<Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
		Public Overrides Property ImmediatePopup() As Boolean
			Get
				Return ImmediatePopupForm
			End Get
			Set(ByVal value As Boolean)
				ImmediatePopupForm = value
			End Set
		End Property

		<DefaultValue(False)> _
		Public Property CaseSensitiveSearch() As Boolean
			Get
				Return caseSensitiveSearch_Renamed
			End Get
			Set(ByVal value As Boolean)
				If CaseSensitiveSearch = value Then
					Return
				End If
				caseSensitiveSearch_Renamed = value
				OnPropertiesChanged()
			End Set
		End Property

#If DXWhidbey Then
		<DXCategory(CategoryName.Data), DefaultValue(""), TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design"), Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design", GetType(System.Drawing.Design.UITypeEditor))> _
		Public Property DisplayMember() As String
#Else
		<DXCategory(CategoryName.Data), DefaultValue(""), TypeConverter("DevExpress.XtraEditors.Design.DataMemberTypeConverter, " & AssemblyInfo.SRAssemblyEditorsDesign)> _
		Public Property DisplayMember() As String
#End If
			Get
				Return displayMember_Renamed
			End Get
			Set(ByVal value As String)
				If displayMember_Renamed <> value Then
					displayMember_Renamed = value
					OnPropertiesChanged()
				End If
			End Set
		End Property

#If DXWhidbey Then
		<DXCategory(CategoryName.Data), DefaultValue(""), TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design"), Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design", GetType(System.Drawing.Design.UITypeEditor))> _
		Public Overridable Property ValueMember() As String
#Else
		<DXCategory(CategoryName.Data), DefaultValue(""), TypeConverter("DevExpress.XtraEditors.Design.DataMemberTypeConverter, " & AssemblyInfo.SRAssemblyEditorsDesign)> _
		Public Overridable Property ValueMember() As String
#End If
			Get
				Return valueMember_Renamed
			End Get
			Set(ByVal value As String)
				If valueMember_Renamed <> value Then
					valueMember_Renamed = value
					OnPropertiesChanged()
				End If
			End Set
		End Property

		<DXCategory(CategoryName.Behavior), DefaultValue(SearchMode.OnlyInPopup)> _
		Public Property SearchMode() As SearchMode
			Get
				Return searchMode_Renamed
			End Get
			Set(ByVal value As SearchMode)
				If SearchMode = value Then
					Return
				End If
				searchMode_Renamed = value
				UpdateSearchMode()
			End Set
		End Property
		#End Region

		Public Shadows ReadOnly Property OwnerEdit() As TreeListLookUpEdit
			Get
				Return TryCast(MyBase.OwnerEdit, TreeListLookUpEdit)
			End Get
		End Property

		Public Sub New()
			If NullText Is Nothing OrElse NullText = "" Then
				NullText = DefaultNullText
			End If
			Me.displayMember_Renamed = String.Empty
			Me.valueMember_Renamed = Me.displayMember_Renamed
			treeList_Renamed = CreateTreeList()
			treeList_Renamed.OptionsBehavior.Editable = False
			TreeList.OptionsBehavior.ExpandNodesOnIncrementalSearch = True
			TreeList.OptionsFilter.FilterMode = FilterMode.Smart
			ImmediatePopup = True
			TreeList.OptionsBehavior.EnableFiltering = True
			filteringNodes = New List(Of TreeListNode)()
			UpdateSearchMode()
		End Sub

		Protected Overridable Sub UpdateSearchMode()
			EnableFilter = False
			TreeList.OptionsBehavior.AllowIncrementalSearch = False
			ImmediatePopupForm = False ' ?
			Select Case searchMode_Renamed
				Case SearchMode.AutoFilter
					EnableFilter = True
					ImmediatePopupForm = True
				Case SearchMode.AutoComplete
					SetCriteriaOperator(Nothing)
				Case SearchMode.OnlyInPopup
					TreeList.OptionsBehavior.AllowIncrementalSearch = True
					SetCriteriaOperator(Nothing)
			End Select
		End Sub

		Friend Sub SetCriteriaOperator(ByVal text As String)
			Dim cp As CriteriaOperator = CriteriaOperator.Parse(text)
			Me.TreeList.ActiveFilterCriteria = cp
		End Sub

		Private Function CreateTreeList() As TreeListInner
			Return New TreeListInner()
		End Function

		Friend Sub RefreshDataSource()
			TreeList.DataSource = DataSource
		End Sub

		Private Function IsMemberValid(ByVal member As String) As Boolean
			If TreeList.Columns(member) IsNot Nothing Then
				Return True
			End If
			Return False
		End Function

		Public Function GetNodeByEditValue(ByVal text As String) As TreeListNode
			Return GetNodeByDisplayText(TreeList.Nodes, text, ValueMember)
		End Function

		Friend Function GetNodeByText(ByVal value As String) As TreeListNode
			Return GetNodeByDisplayText(TreeList.Nodes, value, DisplayMember)
		End Function

		Friend Function GetNodeByDisplayText(ByVal nodes As TreeListNodes, ByVal text As String, ByVal member As String) As TreeListNode
			Dim nod As TreeListNode = Nothing
			If String.IsNullOrEmpty(member) Then
				Return Nothing
			End If
			If IsMemberValid(member) = False Then
				Return Nothing
			End If
			If text = "" OrElse text Is Nothing Then
				Return Nothing
			End If
			For i As Integer = 0 To nodes.Count - 1
				Dim s As String = nodes(i).GetDisplayText(member)
				s = s.Substring(0, Math.Min(text.Length, s.Length))
				If (Not CaseSensitiveSearch) Then
					text = text.ToUpper()
					s = s.ToUpper()
				End If
				If s = text Then
					nod = nodes(i)
					Exit For
				End If
				If nodes.Count <> 0 Then
					nod = GetNodeByDisplayText(nodes(i).Nodes, text, member)
				End If
				If nod IsNot Nothing Then
					Exit For
				End If
			Next i
			Return nod
		End Function

		Friend Sub GetNodesByDisplayText(ByVal nodes As TreeListNodes, ByVal text As String)
			For i As Integer = 0 To nodes.Count - 1
				Dim s As String = nodes(i).GetDisplayText(DisplayMember) ' -!
				s = s.Substring(0, Math.Min(text.Length, s.Length))
				If (Not CaseSensitiveSearch) Then
					text = text.ToUpper()
					s = s.ToUpper()
				End If
				If s = text Then
					filteringNodes.Add(nodes(i))
				End If
				If nodes.Count <> 0 Then
					GetNodesByDisplayText(nodes(i).Nodes, text)
				End If
			Next i
		End Sub

		Private Sub ShowParents(ByVal node As TreeListNode)
			If node.ParentNode IsNot Nothing Then
				ShowParents(node.ParentNode)
				node.ParentNode.Visible = True
			End If
		End Sub

		Friend Sub ShowNodePath()
			For i As Integer = 0 To filteringNodes.Count - 1
				ShowParents(filteringNodes(i))
			Next i
		End Sub

		Private filteringNodes As List(Of TreeListNode)
		Friend Sub ExpandNodesForFiltering(ByVal text As String)
			filteringNodes.Clear()
			If text = "" Then
				treeList_Renamed.CollapseAll()
				Return
			End If
			GetNodesByDisplayText(TreeList.Nodes, text)
			If filteringNodes.Count = 0 Then
				Return
			End If
			For i As Integer = 0 To filteringNodes.Count - 1
				ExpandNode(filteringNodes(i), filteringNodes(i).Level)
			Next i
		End Sub

		Private Sub ExpandNode(ByVal node As TreeListNode, ByVal level As Integer)
			If level = 0 Then
				Return
			End If
			If node.ParentNode IsNot Nothing Then
				If node.ParentNode.Expanded = False Then
					ExpandNode(node.ParentNode, level)
					node.ParentNode.Expanded = True
				End If
			Else
				node.Expanded = True
			End If
		End Sub

		Public Overridable Function GetFocusedValue() As Object
			If TreeList.Columns(ValueMember) IsNot Nothing AndAlso TreeList.FocusedNode IsNot Nothing Then
				Return TreeList.FocusedNode.GetValue(ValueMember)
			End If
			Return NullText
		End Function

		Private Function FindValueNode(ByVal editValue As Object) As TreeListNode
			Dim node As TreeListNode = Nothing
			node = TreeList.FindNodeByFieldValue(ValueMember, editValue)
			Return node
		End Function

		Private Function GetTreeLookUpDisplayText(ByVal format As FormatInfo, ByVal editValue As Object) As String
			If IsNullValue(editValue) Then
				Return GetNullText(format)
			End If
			Dim node As TreeListNode = FindValueNode(editValue)
			If node IsNot Nothing Then
				Return node.GetDisplayText(DisplayMember)
			End If
			Return ""
		End Function

		Public Overrides Overloads Function GetDisplayText(ByVal format As FormatInfo, ByVal editValue As Object) As String
			Dim displayText As String = GetTreeLookUpDisplayText(format, editValue)
			If displayText = "" Then
				If OwnerEdit Is Nothing Then
					Return MyBase.GetDisplayText(format, editValue)
				End If
				Return OwnerEdit.AutoSearchText
			End If
			Dim e As New CustomDisplayTextEventArgs(editValue, displayText)
			If format IsNot EditFormat Then
				RaiseCustomDisplayText(e)
			End If
			Return e.DisplayText
		End Function

		<DXCategory(CategoryName.Events)> _
		Public Custom Event ProcessNewValue As ProcessNewValueEventHandler
			AddHandler(ByVal value As ProcessNewValueEventHandler)
                Me.Events.AddHandler(_processNewValue, value)
			End AddHandler
			RemoveHandler(ByVal value As ProcessNewValueEventHandler)
                Me.Events.RemoveHandler(_processNewValue, value)
			End RemoveHandler
			RaiseEvent(ByVal sender As System.Object, ByVal e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs)
			End RaiseEvent
		End Event

		Protected Friend Overridable Sub RaiseProcessNewValue(ByVal e As ProcessNewValueEventArgs)
            Dim handler As ProcessNewValueEventHandler = CType(Events(_processNewValue), ProcessNewValueEventHandler)
			If handler IsNot Nothing Then
				handler(GetEventSender(), e)
			End If
		End Sub
	End Class
End Namespace