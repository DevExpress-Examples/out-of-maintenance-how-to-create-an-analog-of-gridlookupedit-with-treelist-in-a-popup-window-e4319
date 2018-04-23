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
	Public Class TreeListInner
		Inherits TreeList
		Protected Overrides Function CreateOptionsBehavior() As TreeListOptionsBehavior
			Return New TreeLookUpOptionsBehavior()
		End Function

		<Browsable(False)> _
		Public Shadows Property DataSource() As Object
			Get
				Return MyBase.DataSource
			End Get
			Set(ByVal value As Object)
				If DataSource Is value Then
					Return
				End If
				MyBase.DataSource = value
			End Set
		End Property

		Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
			MyBase.OnMouseUp(e)
			Dim hi As TreeListHitInfo = Me.CalcHitInfo(e.Location)
			Dim ownerForm As TreeListLookUpPopupForm = (TryCast(Parent, TreeListLookUpPopupForm))
			If hi.Node IsNot Nothing AndAlso hi.Column IsNot Nothing Then
				ownerForm.Properties.OwnerEdit.HidePopup()
			Else
				ownerForm.Properties.OwnerEdit.EditValue = ownerForm.Properties.GetFocusedValue()
			End If
		End Sub
	End Class
End Namespace