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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;
using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraTreeList;
using DevExpress.Utils;
using DevExpress.XtraEditors.ListControls;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.Data.Filtering;
using System.Diagnostics;
using System.Collections;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.ViewInfo;

namespace TreeListLookUp
{
    public class TreeListLookUpEdit : PopupBaseAutoSearchEdit
    {
        static TreeListLookUpEdit()
        {
            RepositoryItemTreeListLookUpEdit.Register();
        }

        public TreeListLookUpEdit() {}

        public override string EditorTypeName
        {
            get { return RepositoryItemTreeListLookUpEdit.EditorName; }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new RepositoryItemTreeListLookUpEdit Properties
        {
            get { return base.Properties as RepositoryItemTreeListLookUpEdit; }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Properties.TreeList.CreateControl();
            Properties.RefreshDataSource();
        }

        protected override void OnPopupShown()
        {
            base.OnPopupShown();
            if (Properties.SearchMode == SearchMode.OnlyInPopup)
                Properties.TreeList.Focus();
        }

        string filterText = "";
        internal string GetFilterText() { return filterText; }
        protected override void ProcessFindItem(KeyPressHelper helper, char pressedKey)
        {
            filterText = helper.Text;
            if (filterText == "" || filterText == null)
                Properties.TreeList.FocusedNode = Properties.PrevFocusedNode;
            base.ProcessFindItem(helper, pressedKey);
            if (filterText != "" && Properties.SearchMode != SearchMode.OnlyInPopup)
            {
                TreeListNode node = Properties.GetNodeByDisplayText(Properties.TreeList.Nodes, filterText, Properties.DisplayMember);
                if (node != null)
                {
                    string nodeText = node.GetDisplayText(Properties.DisplayMember);
                    EditValue = node[Properties.ValueMember];
                    AutoSearchText = filterText;
                    this.SelectionStart = helper.GetCorrectedAutoSearchSelectionStart(Text, pressedKey);
                    this.SelectionLength = nodeText.Length - this.SelectionStart;
                    if (Properties.TreeList.FocusedNode != null)
                        if (Properties.TreeList.FocusedNode != Properties.PrevFocusedNode)
                            Properties.PrevFocusedNode = Properties.TreeList.FocusedNode;
                    Properties.TreeList.FocusedNode = node;
                    LayoutChanged();
                }
            }
        }

        protected override void OnEditValueChanged()
        {
            base.OnEditValueChanged();
            Properties.SetCriteriaOperator(null);
            if (Properties.EnableFilter && Properties.TreeList.Columns[Properties.DisplayMember] != null &&
                                           Properties.TreeList.Columns[Properties.ValueMember] != null &&
                                           filterText != "")
            {
                Properties.ExpandNodesForFiltering(filterText);
                if (Convert.ToString(EditValue) == "" || EditValue == null)
                    filterText = "";
                if (filterText != null && filterText != "")
                {
                    string criteriaString = "StartsWith([" + Properties.TreeList.Columns[Properties.DisplayMember].FieldName + "], '" + filterText + "')";
                    Properties.SetCriteriaOperator(criteriaString);
                }
                Properties.ShowNodePath();
            }
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            if (Properties.GetNodeByText(filterText) == null && filterText != "")
            {
                ProcessNewValueEventArgs ee = new ProcessNewValueEventArgs(filterText);
                Properties.RaiseProcessNewValue(ee);
                if (ee.Handled) { EditValue = Properties.GetNodeByText(filterText)[Properties.ValueMember]; }
            }
            HidePopup();
        }

        protected override void DoImmediatePopup(int itemIndex, char pressedKey)
        {
            if (pressedKey != '\b' && Properties.ImmediatePopupForm)
                ShowPopup();
        }

        public void HidePopup()
        {
            ClosePopup();
        }

        protected override void DoClosePopup(PopupCloseMode closeMode)
        {
            base.DoClosePopup(closeMode);
            Properties.TreeList.StopIncrementalSearch();
        }

    [DXCategory(CategoryName.Events)]
		public event ProcessNewValueEventHandler ProcessNewValue {
			add { this.Properties.ProcessNewValue += value; }
			remove { this.Properties.ProcessNewValue -= value; }
		}

        protected override PopupBaseForm CreatePopupForm()
        {
            return new TreeListLookUpPopupForm(this);
        }
    }
}
