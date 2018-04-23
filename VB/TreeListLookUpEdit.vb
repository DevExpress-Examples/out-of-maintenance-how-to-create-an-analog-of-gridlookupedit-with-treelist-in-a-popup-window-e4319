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
	Public Class TreeListLookUpEdit
		Inherits PopupBaseAutoSearchEdit
		Shared Sub New()
			RepositoryItemTreeListLookUpEdit.Register()
		End Sub

		Public Sub New()
		End Sub

		Public Overrides ReadOnly Property EditorTypeName() As String
			Get
				Return RepositoryItemTreeListLookUpEdit.EditorName
			End Get
		End Property
		<DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
		Public Shadows ReadOnly Property Properties() As RepositoryItemTreeListLookUpEdit
			Get
				Return TryCast(MyBase.Properties, RepositoryItemTreeListLookUpEdit)
			End Get
		End Property

		Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
			MyBase.OnHandleCreated(e)
			Properties.TreeList.CreateControl()
			Properties.RefreshDataSource()
		End Sub

		Protected Overrides Sub OnPopupShown()
			MyBase.OnPopupShown()
			If Properties.SearchMode = SearchMode.OnlyInPopup Then
				Properties.TreeList.Focus()
			End If
		End Sub

		Private filterText As String = ""
		Friend Function GetFilterText() As String
			Return filterText
		End Function
		Protected Overrides Sub ProcessFindItem(ByVal helper As KeyPressHelper, ByVal pressedKey As Char)
			filterText = helper.Text
			If filterText = "" OrElse filterText Is Nothing Then
				Properties.TreeList.FocusedNode = Properties.PrevFocusedNode
			End If
			MyBase.ProcessFindItem(helper, pressedKey)
			If filterText <> "" AndAlso Properties.SearchMode <> SearchMode.OnlyInPopup Then
				Dim node As TreeListNode = Properties.GetNodeByDisplayText(Properties.TreeList.Nodes, filterText, Properties.DisplayMember)
				If node IsNot Nothing Then
					Dim nodeText As String = node.GetDisplayText(Properties.DisplayMember)
					EditValue = node(Properties.ValueMember)
					AutoSearchText = filterText
					Me.SelectionStart = helper.GetCorrectedAutoSearchSelectionStart(Text, pressedKey)
					Me.SelectionLength = nodeText.Length - Me.SelectionStart
					If Properties.TreeList.FocusedNode IsNot Nothing Then
						If Properties.TreeList.FocusedNode IsNot Properties.PrevFocusedNode Then
							Properties.PrevFocusedNode = Properties.TreeList.FocusedNode
						End If
					End If
					Properties.TreeList.FocusedNode = node
					LayoutChanged()
				End If
			End If
		End Sub

		Protected Overrides Sub OnEditValueChanged()
			MyBase.OnEditValueChanged()
			Properties.SetCriteriaOperator(Nothing)
			If Properties.EnableFilter AndAlso Properties.TreeList.Columns(Properties.DisplayMember) IsNot Nothing AndAlso Properties.TreeList.Columns(Properties.ValueMember) IsNot Nothing AndAlso filterText <> "" Then
				Properties.ExpandNodesForFiltering(filterText)
				If Convert.ToString(EditValue) = "" OrElse EditValue Is Nothing Then
					filterText = ""
				End If
				If filterText IsNot Nothing AndAlso filterText <> "" Then
					Dim criteriaString As String = "StartsWith([" & Properties.TreeList.Columns(Properties.DisplayMember).FieldName & "], '" & filterText & "')"
					Properties.SetCriteriaOperator(criteriaString)
				End If
				Properties.ShowNodePath()
			End If
		End Sub

		Protected Overrides Sub OnValidating(ByVal e As CancelEventArgs)
			MyBase.OnValidating(e)
			If Properties.GetNodeByText(filterText) Is Nothing AndAlso filterText <> "" Then
				Dim ee As New ProcessNewValueEventArgs(filterText)
				Properties.RaiseProcessNewValue(ee)
				If ee.Handled Then
					EditValue = Properties.GetNodeByText(filterText)(Properties.ValueMember)
				End If
			End If
			HidePopup()
		End Sub

		Protected Overrides Sub DoImmediatePopup(ByVal itemIndex As Integer, ByVal pressedKey As Char)
			If pressedKey <> ControlChars.Back AndAlso Properties.ImmediatePopupForm Then
				ShowPopup()
			End If
		End Sub

		Public Sub HidePopup()
			ClosePopup()
		End Sub

		Protected Overrides Sub DoClosePopup(ByVal closeMode As PopupCloseMode)
			MyBase.DoClosePopup(closeMode)
			Properties.TreeList.StopIncrementalSearch()
		End Sub

	<DXCategory(CategoryName.Events)> _
	Public Custom Event ProcessNewValue As ProcessNewValueEventHandler
			AddHandler(ByVal value As ProcessNewValueEventHandler)
				AddHandler Me.Properties.ProcessNewValue, value
			End AddHandler
			RemoveHandler(ByVal value As ProcessNewValueEventHandler)
				RemoveHandler Me.Properties.ProcessNewValue, value
			End RemoveHandler
			RaiseEvent(ByVal sender As System.Object, ByVal e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs)
			End RaiseEvent
	End Event

		Protected Overrides Function CreatePopupForm() As PopupBaseForm
			Return New TreeListLookUpPopupForm(Me)
		End Function
	End Class
End Namespace
