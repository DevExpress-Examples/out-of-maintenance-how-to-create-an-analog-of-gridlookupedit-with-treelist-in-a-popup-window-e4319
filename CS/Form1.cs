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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.OleDb;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid;
using DevExpress.XtraTreeList;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraEditors.Controls;

namespace TreeListLookUp
{
    public partial class Form1 : Form
    {
        DataTable dt;
        
        public Form1()
        {
            InitializeComponent();
        }

        public enum DataMode { Bound, Unbound, Virtual };

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ordersTableAdapter.Fill(this.nwindDataSet.Orders);
           
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Word", typeof(string));
            dt1.Rows.Add();
            gridControl1.DataSource = dt1;
            UseUnboundTest();
            GenerateTestDataTable();
            PopulateRadioGroup(radioGroup1, typeof(SearchMode));
            PopulateRadioGroup(radioGroup2, typeof(DataMode));

            radioGroup1.EditValue = SearchMode.AutoFilter;
            radioGroup2.EditValue = DataMode.Unbound;
        }

        private void PopulateRadioGroup(RadioGroup radioGroup, Type enumType)
        {
            Array values = Enum.GetValues(enumType);
            foreach (object val in values)
                radioGroup.Properties.Items.Add(new RadioGroupItem(val, val.ToString()));
        }

        private void OnSearchModeChanged(object sender, EventArgs e)
        {
            RadioGroup radioGroup = sender as RadioGroup;
            treeListLookUpEdit1.Properties.SearchMode =
            repositoryItemTreeListLookUpEdit1.SearchMode = (SearchMode)radioGroup.EditValue;
        }

        bool virtualMode = false; int ID = 2;
        private void OnDataModeChanged(object sender, EventArgs e)
        {
            RadioGroup radioGroup = sender as RadioGroup;
            DataMode mode = (DataMode)radioGroup.EditValue;
            if(mode == DataMode.Bound)
                PopulateBoundTree();
            else if (mode == DataMode.Unbound)
                PopulateUnboundTree();
            else
                PopulateVirtualTree();
        }

        private void PopulateBoundTree()
        {
            treeListLookUpEdit1.Properties.TreeList.Columns.Clear();
            treeListLookUpEdit1.Properties.DataSource = null;
            treeListLookUpEdit1.Properties.DataSource = ordersBindingSource;
            treeListLookUpEdit1.Properties.ValueMember = "OrderID";
            treeListLookUpEdit1.Properties.DisplayMember = "ShipName";
            virtualMode = false;
            ID = 2;
            treeListLookUpEdit1.Properties.TreeList.BeforeExpand -= new BeforeExpandEventHandler(TreeList_BeforeExpand);
        }
        private void PopulateUnboundTree()
        {
            UseUnboundTest();
            virtualMode = false;
            ID = 2;
            treeListLookUpEdit1.Properties.TreeList.BeforeExpand -= new BeforeExpandEventHandler(TreeList_BeforeExpand);
        }
        private void PopulateVirtualTree()
        {
            virtualMode = true;
            treeListLookUpEdit1.Properties.TreeList.Nodes.Clear();
            treeListLookUpEdit1.Properties.TreeList.Columns.Clear();
            treeListLookUpEdit1.Properties.DataSource = null;
            GenerateTestColumns();
            GenerateTestRootNodes();
            treeListLookUpEdit1.Properties.TreeList.BeforeExpand += new BeforeExpandEventHandler(TreeList_BeforeExpand);
        }
        private void UseUnboundTest()
        {
            treeListLookUpEdit1.Properties.TreeList.Nodes.Clear();
            treeListLookUpEdit1.Properties.TreeList.Columns.Clear();
            treeListLookUpEdit1.Properties.DataSource = null;
            GenerateTestColumns();
            GenerateTestNodes();
        }

        private void treeListLookUpEdit1_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            TreeList TreeList = treeListLookUpEdit1.Properties.TreeList;
            if (virtualMode)
            {
                e.Handled = true;
                ID++;
                TreeList.AppendNode(new object[] { e.DisplayValue, "Little-Medium", ID }, null);
            }
        }

        void TreeList_BeforeExpand(object sender, BeforeExpandEventArgs e)
        {
            GenerateTestChildrenNodes(e.Node);
        }
        #region Populating with test nodes
        private void GenerateTestNodes()
        {
            TreeList TreeList = treeListLookUpEdit1.Properties.TreeList;
            TreeList.BeginUnboundLoad();
            TreeListNode rootNode = TreeList.AppendNode(new object[] { "Mammals", "Little-Huge", 1 }, null);
            TreeList.AppendNode(new object[] { "Polar Bear", "Huge", 2 }, rootNode);
            TreeListNode cats = TreeList.AppendNode(new object[] { "Cats", "Little-Medium", 3 }, rootNode);
            TreeList.AppendNode(new object[] { "Tiger", "Medium", 4 }, cats);
            TreeList.AppendNode(new object[] { "Lion", "Medium", 5 }, cats);
            TreeList.AppendNode(new object[] { "Panther", "Medium", 6 }, cats);
            TreeList.AppendNode(new object[] { "Persian cat", "Little", 7 }, cats);
            TreeList.AppendNode(new object[] { "Wolf", "Medium", 8 }, rootNode);
            TreeListNode birds = TreeList.AppendNode(new object[] { "Birds", "Little-Medium", 9 }, null);
            TreeList.AppendNode(new object[] { "Dove", "Little", 10 }, birds);
            TreeList.AppendNode(new object[] { "Lark", "Little", 11 }, birds);
            TreeList.AppendNode(new object[] { "Sparrow", "Little", 12 }, birds);
            TreeList.EndUnboundLoad();
            treeListLookUpEdit1.Properties.ValueMember = "AnimalID";
            treeListLookUpEdit1.Properties.DisplayMember = "Name";
        }
        private void GenerateTestRootNodes()
        {
            TreeList TreeList = treeListLookUpEdit1.Properties.TreeList;
            TreeList.BeginUnboundLoad();
            TreeListNode rootNode = TreeList.AppendNode(new object[] { "Mammals", "Little-Huge", 1 }, null);
            rootNode.HasChildren = true;
            TreeListNode birds = TreeList.AppendNode(new object[] { "Birds", "Little-Medium", 2 }, null);
            birds.HasChildren = true;
            TreeList.EndUnboundLoad();
            treeListLookUpEdit1.Properties.ValueMember = "AnimalID";
            treeListLookUpEdit1.Properties.DisplayMember = "Name";
        }
        private void GenerateTestChildrenNodes(TreeListNode parent)
        {
            TreeList TreeList = treeListLookUpEdit1.Properties.TreeList;
            if (parent.GetDisplayText("Name") == "Mammals")
            {
                ID++;
                TreeListNode node = TreeList.AppendNode(new object[] { "Fox", "Medium", ID }, parent);
                node.HasChildren = true;
                ID++;
                node = TreeList.AppendNode(new object[] { "Panda", "Medium-Huge", ID }, parent);
                node.HasChildren = true;
            } else
                if (parent.GetDisplayText("Name") == "Birds")
                {
                    ID++;
                    TreeListNode node = TreeList.AppendNode(new object[] { "Eagle", "Medium", ID }, parent);
                    node.HasChildren = true;
                    ID++;
                    node = TreeList.AppendNode(new object[] { "Sparrow", "Little", ID }, parent);
                    node.HasChildren = true;
                }
                else
                {
                    ID++;
                    TreeListNode node = TreeList.AppendNode(new object[] { "New animal 1", "Medium", ID }, parent);
                    node.HasChildren = true;
                    ID++;
                    node = TreeList.AppendNode(new object[] { "New animal 2", "Little", ID }, parent);
                    node.HasChildren = true;
                }
        }
        private void GenerateTestColumns()
        {
            TreeList TreeList = treeListLookUpEdit1.Properties.TreeList;
            TreeList.BeginUpdate();
            TreeList.Columns.Add();
            TreeList.Columns[0].FieldName = "Name";
            TreeList.Columns[0].VisibleIndex = 0;
            TreeList.Columns.Add();
            TreeList.Columns[1].FieldName = "Size";
            TreeList.Columns[1].VisibleIndex = 1;
            TreeList.Columns.Add();
            TreeList.Columns[2].FieldName = "AnimalID";
            TreeList.Columns[2].VisibleIndex = 2;
            TreeList.EndUpdate();
        }
        private void GenerateTestDataTable()
        {
            dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("ParentID", typeof(int));
            dt.Columns.Add("Word", typeof(string));
            dt.Rows.Add(1, 0, "Hello");
            dt.Rows.Add(2, 0, "This");
            dt.Rows.Add(3, 2, "is");
            dt.Rows.Add(4, 3, "a");
            dt.Rows.Add(5, 4, "Test");
            dt.Rows.Add(6, 4, "treeListLookUpEdit");
            dt.Rows.Add(7, 6, "editor");
            repositoryItemTreeListLookUpEdit1.DataSource = dt;
            repositoryItemTreeListLookUpEdit1.DisplayMember = "Word";
            repositoryItemTreeListLookUpEdit1.ValueMember = "Word";
        }
        #endregion  

    
   
    }
}
