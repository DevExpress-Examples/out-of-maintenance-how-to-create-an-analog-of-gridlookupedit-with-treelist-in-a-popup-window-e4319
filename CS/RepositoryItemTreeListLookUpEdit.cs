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
    [UserRepositoryItem("Register")]
    public class RepositoryItemTreeListLookUpEdit : RepositoryItemPopupBaseAutoSearchEdit
    {
        #region Register, Assign, Dispose, CheckDestroyDataSource
        internal const string EditorName = "TreeListLookUpEdit";

        public static void Register()
        {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName, typeof(TreeListLookUpEdit),
                typeof(RepositoryItemTreeListLookUpEdit), typeof(PopupBaseAutoSearchEditViewInfo),
                new ButtonEditPainter(), true, null));
        }
        static RepositoryItemTreeListLookUpEdit()
        {
            Register();
        }
        public override string EditorTypeName
        {
            get { return EditorName; }
        }

        public override void Assign(RepositoryItem item)
        {
            RepositoryItemTreeListLookUpEdit source = item as RepositoryItemTreeListLookUpEdit;
            BeginUpdate();
            try
            {
                base.Assign(item);
                this.DisplayMember = source.DisplayMember;
                this.ValueMember = source.ValueMember;
                this.DataSource = source.DataSource;
                this.ImmediatePopupForm = source.ImmediatePopupForm;
                this.EnableFilter = source.EnableFilter;
                this.CaseSensitiveSearch = source.CaseSensitiveSearch;
                this.SearchMode = source.SearchMode;
            }
            finally
            {
                EndUpdate();
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CheckDestroyDataSource();
            }
            base.Dispose(disposing);
        }
        private void CheckDestroyDataSource()
        {
            try
            {
                if (dataSource != null)
                {
                    IDisposable ds = dataSource as IDisposable;
                    if (ds != null) ds.Dispose();
                }

                this.DataSource = null;
            }
            catch { }
        }
        #endregion
        // ==============================================================
        const string DefaultNullText = "[EditValue is null]";
        TreeListInner treeList;
        // Fields...
        private string displayMember;
        private string valueMember;
        private object dataSource;
        private SearchMode searchMode;
        bool caseSensitiveSearch;
        private bool enableFilter;
        private bool immediatePopupForm;
        static readonly object processNewValue = new object();

        internal bool EnableFilter
        {
            get { return enableFilter; }
            set { enableFilter = value; }
        }

        internal TreeListNode PrevFocusedNode { get; set; }
        
        internal IDictionary PropertiesStore
        {
            get { return base.PropertyStore; }
        }

        public TreeListInner TreeList { get { return treeList; } }

        #region Designer predefined Properties
        [DXCategory(CategoryName.Data), DefaultValue(null),
#if DXWhidbey
		AttributeProvider(typeof(IListSource))]
#else
 TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design")]
#endif
        public object DataSource
        {
            get { return dataSource; }
            set
            {
                if (DataSource != value)
                {
                    dataSource = value;
                    if (TreeList.BindingContext == null)
                        TreeList.BindingContext = new BindingContext();
                    if (TreeList.IsHandleCreated)
                        TreeList.DataSource = value;
                    OnPropertiesChanged();
                }
            }
        }
        
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ImmediatePopupForm
        {
            get { return immediatePopupForm; }
            set { immediatePopupForm = value; }
        }
        
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override bool ImmediatePopup { get { return ImmediatePopupForm; } set { ImmediatePopupForm = value; } }

        [DefaultValue(false)]
        public bool CaseSensitiveSearch
        {
            get { return caseSensitiveSearch; }
            set
            {
                if (CaseSensitiveSearch == value) return;
                caseSensitiveSearch = value;
                OnPropertiesChanged();
            }
        }

        [DXCategory(CategoryName.Data),
        DefaultValue(""),
#if DXWhidbey
		TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design"),
		Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))]
#else
 TypeConverter("DevExpress.XtraEditors.Design.DataMemberTypeConverter, " + AssemblyInfo.SRAssemblyEditorsDesign)]
#endif
        public string DisplayMember
        {
            get { return displayMember; }
            set
            {
                if (displayMember != value)
                {
                    displayMember = value;
                    OnPropertiesChanged();
                }
            }
        }

        [DXCategory(CategoryName.Data),
        DefaultValue(""),
#if DXWhidbey
		TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design"),
		Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))]
#else
 TypeConverter("DevExpress.XtraEditors.Design.DataMemberTypeConverter, " + AssemblyInfo.SRAssemblyEditorsDesign)]
#endif
        public virtual string ValueMember
        {
            get { return valueMember; }
            set
            {
                if (valueMember != value)
                {
                    valueMember = value;
                    OnPropertiesChanged();
                }
            }
        }

        [DXCategory(CategoryName.Behavior), DefaultValue(SearchMode.OnlyInPopup)]
        public SearchMode SearchMode
        {
            get { return searchMode; }
            set
            {
                if (SearchMode == value) return;
                searchMode = value;
                UpdateSearchMode();
            }
        }
        #endregion

        public new TreeListLookUpEdit OwnerEdit { get { return base.OwnerEdit as TreeListLookUpEdit; } }

        public RepositoryItemTreeListLookUpEdit()
        {
            if (NullText == null || NullText == "")
                NullText = DefaultNullText;
            this.valueMember = this.displayMember = string.Empty;
            treeList = CreateTreeList();
            treeList.OptionsBehavior.Editable = false;
            TreeList.OptionsBehavior.ExpandNodesOnIncrementalSearch = true;
            TreeList.OptionsFilter.FilterMode = FilterMode.Smart;
            ImmediatePopup = true;
            TreeList.OptionsBehavior.EnableFiltering = true;
            filteringNodes = new List<TreeListNode>();
            UpdateSearchMode();
        }

        protected virtual void UpdateSearchMode()
        {
            EnableFilter = false;
            TreeList.OptionsBehavior.AllowIncrementalSearch = false;
            ImmediatePopupForm = false; // ?
            switch (searchMode)
            {
                case SearchMode.AutoFilter:
                    EnableFilter = true;
                    ImmediatePopupForm = true;
                    break;
                case SearchMode.AutoComplete:
                    SetCriteriaOperator(null);
                    break;
                case SearchMode.OnlyInPopup:
                    TreeList.OptionsBehavior.AllowIncrementalSearch = true;
                    SetCriteriaOperator(null);
                    break;
            }
        }

        internal void SetCriteriaOperator(string text)
        {
            CriteriaOperator cp = CriteriaOperator.Parse(text);
            this.TreeList.ActiveFilterCriteria = cp;
        }

        private TreeListInner CreateTreeList() { return new TreeListInner(); }

        internal void RefreshDataSource()
        {
            TreeList.DataSource = DataSource;
        }

        bool IsMemberValid(string member)
        {
            if (TreeList.Columns[member] != null)
                return true;
            return false;
        }

        public TreeListNode GetNodeByEditValue(string text)
        {
            return GetNodeByDisplayText(TreeList.Nodes, text, ValueMember);
        }

        internal TreeListNode GetNodeByText(string value)
        {
            return GetNodeByDisplayText(TreeList.Nodes, value, DisplayMember);
        }

        internal TreeListNode GetNodeByDisplayText(TreeListNodes nodes, string text, string member)
        {
            TreeListNode nod = null;
            if (string.IsNullOrEmpty(member)) return null;
            if (IsMemberValid(member) == false) return null;
            if (text == "" || text == null) return null;
            for (int i = 0; i < nodes.Count; i++)
            {
                string s = nodes[i].GetDisplayText(member);
                s = s.Substring(0, Math.Min(text.Length, s.Length));
                if (!CaseSensitiveSearch)
                {
                    text = text.ToUpper();
                    s = s.ToUpper();
                }
                if (s == text)
                {
                    nod = nodes[i];
                    break;
                }
                if (nodes.Count != 0)
                    nod = GetNodeByDisplayText(nodes[i].Nodes, text, member);
                if (nod != null) break;
            }
            return nod;
        }

        internal void GetNodesByDisplayText(TreeListNodes nodes, string text)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                string s = nodes[i].GetDisplayText(DisplayMember); // -!
                s = s.Substring(0, Math.Min(text.Length, s.Length));
                if (!CaseSensitiveSearch)
                {
                    text = text.ToUpper();
                    s = s.ToUpper();
                }
                if (s == text)
                    filteringNodes.Add(nodes[i]);
                if (nodes.Count != 0)
                    GetNodesByDisplayText(nodes[i].Nodes, text);
            }
        }

        void ShowParents(TreeListNode node)
        {
            if (node.ParentNode != null)
            {
                ShowParents(node.ParentNode);
                node.ParentNode.Visible = true;
            }
        }

        internal void ShowNodePath()
        {
            for (int i = 0; i < filteringNodes.Count; i++)
            {
                ShowParents(filteringNodes[i]);
            }
        }

        List<TreeListNode> filteringNodes;
        internal void ExpandNodesForFiltering(string text)
        {
            filteringNodes.Clear();
            if (text == "")
            {
                treeList.CollapseAll();
                return;
            }
            GetNodesByDisplayText(TreeList.Nodes, text);
            if (filteringNodes.Count == 0) return;
            for (int i = 0; i < filteringNodes.Count; i++)
            {
                ExpandNode(filteringNodes[i], filteringNodes[i].Level);
            }
        }

        void ExpandNode(TreeListNode node, int level)
        {
            if (level == 0) return;
            if (node.ParentNode != null)
            {
                if (node.ParentNode.Expanded == false)
                {
                    ExpandNode(node.ParentNode, level);
                    node.ParentNode.Expanded = true;
                }
            }
            else
                node.Expanded = true;
        }

        public virtual object GetFocusedValue()
        {
            if (TreeList.Columns[ValueMember] != null && TreeList.FocusedNode != null)
                return TreeList.FocusedNode.GetValue(ValueMember);
            return NullText;
        }

        private TreeListNode FindValueNode(object editValue)
        {
            TreeListNode node = null;
            node = TreeList.FindNodeByFieldValue(ValueMember, editValue);
            return node;
        }

        private string GetTreeLookUpDisplayText(FormatInfo format, object editValue)
        {
            if (IsNullValue(editValue)) return GetNullText(format);
            TreeListNode node = FindValueNode(editValue);
            if (node != null)
                return node.GetDisplayText(DisplayMember);
            return "";
        }

        public override string GetDisplayText(FormatInfo format, object editValue)
        {
            string displayText = GetTreeLookUpDisplayText(format, editValue);
            if (displayText == "")
            {
                if (OwnerEdit == null)
                {
                    return base.GetDisplayText(format, editValue);
                }
                return OwnerEdit.AutoSearchText;
            }
            CustomDisplayTextEventArgs e = new CustomDisplayTextEventArgs(editValue, displayText);
            if (format != EditFormat)
                RaiseCustomDisplayText(e);
            return e.DisplayText;
        }

        [DXCategory(CategoryName.Events)]
        public event ProcessNewValueEventHandler ProcessNewValue
        {
            add { this.Events.AddHandler(processNewValue, value); }
            remove { this.Events.RemoveHandler(processNewValue, value); }
        }

        protected internal virtual void RaiseProcessNewValue(ProcessNewValueEventArgs e)
        {
            ProcessNewValueEventHandler handler = (ProcessNewValueEventHandler)Events[processNewValue];
            if (handler != null) handler(GetEventSender(), e);
        }
    }
}