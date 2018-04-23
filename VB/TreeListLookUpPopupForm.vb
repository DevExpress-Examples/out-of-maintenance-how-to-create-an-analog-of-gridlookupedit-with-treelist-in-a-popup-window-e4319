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
Imports DevExpress.XtraEditors.Popup
Imports DevExpress.XtraTreeList
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Filtering
Imports DevExpress.XtraTreeList.Nodes
Imports System.Drawing
Imports System.ComponentModel
Imports System.Windows.Forms
Imports DevExpress.XtraEditors.Controls

Namespace TreeListLookUp
	Friend Class TreeListLookUpPopupForm
		Inherits CustomBlobPopupForm
		Friend Shared SmallIndent As Integer = 10

		Public Shadows ReadOnly Property Properties() As RepositoryItemTreeListLookUpEdit
			Get
				Return TryCast(OwnerEdit.Properties, RepositoryItemTreeListLookUpEdit)
			End Get
		End Property
		Protected ReadOnly Property TreeList() As TreeList
			Get
				Return Properties.TreeList
			End Get
		End Property

		Public Sub New(ByVal ownerEdit As PopupBaseEdit)
			MyBase.New(ownerEdit)
			Me.TreeList.Bounds = New System.Drawing.Rectangle(SmallIndent\2, SmallIndent\2, 0, 0)
			Controls.Add(TreeList)
		End Sub

		Protected Overrides Sub OnResize(ByVal e As EventArgs)
			MyBase.OnResize(e)
			Dim treeListSize As New Size()
			treeListSize.Width = Me.Size.Width - SmallIndent
			treeListSize.Height = Me.Size.Height - Me.ViewInfo.SizeBarRect.Height - SmallIndent
			TreeList.Size = treeListSize
		End Sub

		Public Overrides Sub ShowPopupForm()
			MyBase.ShowPopupForm()
			If Properties.SearchMode = SearchMode.OnlyInPopup Then
				TreeList.Focus()
			End If
		End Sub

		Protected Overrides Overloads Sub Dispose(ByVal disposing As Boolean)
			TreeList.Parent = Nothing
			MyBase.Dispose(disposing)
		End Sub

		Protected Overrides Function CalcFormSizeCore() As Size
			Dim s As Size = MyBase.CalcFormSizeCore()
			Dim blobSize As Object = (TryCast(OwnerEdit.Properties, RepositoryItemTreeListLookUpEdit)).PropertiesStore("BlobSize")
			If blobSize Is Nothing Then
				s.Width = Math.Max(DefaultEmptySize.Width, OwnerEdit.Width)
			End If
			Return s
		End Function

		Protected Overrides Function QueryResultValue() As Object
			Return Properties.GetFocusedValue()
		End Function

		Public Overrides Sub ProcessKeyDown(ByVal e As KeyEventArgs)
			MyBase.ProcessKeyDown(e)
			Dim dispVal As String = (TryCast(OwnerEdit, TreeListLookUpEdit)).GetFilterText()
			Select Case e.KeyCode
				Case Keys.Enter
					If Properties.GetNodeByEditValue(dispVal) Is Nothing AndAlso dispVal <> "" Then
						Dim ee As New ProcessNewValueEventArgs(dispVal)
						Properties.RaiseProcessNewValue(ee)
						If ee.Handled Then
							OwnerEdit.EditValue = Properties.GetNodeByText(dispVal)(Properties.ValueMember)
						End If
					End If
					TryCast(Me.OwnerEdit, TreeListLookUpEdit).HidePopup()
				Case Keys.Down
					If TreeList.Focused = False Then
						TreeList.Focus()
						If TreeList.Nodes.Count <> 0 Then
							If TreeList.FocusedNode.NextNode IsNot Nothing AndAlso TreeList.FocusedNode.NextNode.Visible = True Then
								TreeList.FocusedNode = TreeList.FocusedNode.NextNode
							End If
						End If
					End If
				Case Keys.Up
					If TreeList.Focused = False Then
						TreeList.Focus()
						If TreeList.Nodes.Count <> 0 Then
							If TreeList.FocusedNode.PrevNode IsNot Nothing AndAlso TreeList.FocusedNode.PrevNode.Visible = True Then
								TreeList.FocusedNode = TreeList.FocusedNode.PrevNode
							End If
						End If
					End If
			End Select
		End Sub
	End Class
End Namespace
