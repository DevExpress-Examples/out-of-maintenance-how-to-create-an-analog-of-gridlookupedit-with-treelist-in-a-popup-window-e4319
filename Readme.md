<!-- default file list -->
*Files to look at*:

* [Form1.cs](./CS/Form1.cs) (VB: [Form1.vb](./VB/Form1.vb))
* [RepositoryItemTreeListLookUpEdit.cs](./CS/RepositoryItemTreeListLookUpEdit.cs) (VB: [RepositoryItemTreeListLookUpEdit.vb](./VB/RepositoryItemTreeListLookUpEdit.vb))
* [TreeListInner.cs](./CS/TreeListInner.cs) (VB: [TreeListInner.vb](./VB/TreeListInner.vb))
* [TreeListLookUpEdit.cs](./CS/TreeListLookUpEdit.cs) (VB: [TreeListLookUpEdit.vb](./VB/TreeListLookUpEdit.vb))
* [TreeListLookUpPopupForm.cs](./CS/TreeListLookUpPopupForm.cs) (VB: [TreeListLookUpPopupForm.vb](./VB/TreeListLookUpPopupForm.vb))
* [TreeLookUpOptionsBehavior.cs](./CS/TreeLookUpOptionsBehavior.cs) (VB: [TreeLookUpOptionsBehavior.vb](./VB/TreeLookUpOptionsBehavior.vb))
<!-- default file list end -->
# How to create an analog of GridLookUpEdit with TreeList in a popup window


<p><strong>This example is obsolete.</strong> <strong>We've included the TreeListLookUpEdit editor in our suite in version 13.1.</strong></p>
<p><br />In some cases, it is useful to display data within LookUpEdit in the hierarchical structure.</p>
<p>This can be introduced by creating a custom editor – <strong>TreeListLookUpEdit</strong>.</p>
<p>This example illustrates how to create such an editor.</p>
<p>Here we have implemented the following features:</p>
<br />
<p>- The RepositoryItemTreeListLookUpEdit.<strong>ValueMember </strong>and RepositoryItemTreeListLookUpEdit.<strong>DisplayMember </strong>properties. They are intended for the same functions as in a regular LookUpEdit/GridLookUpEdit.</p>
<p>- The <strong>ProcessNewValue </strong>event is implemented like in LookUpEdit/GridLookUpEdit. This event is raised when an editor is validated and its display value does not exist within an inner TreeList.</p>
<p>- The property <strong>SearchMode </strong>allows you to select one of the following modes:</p>
<p>     o <strong>OnlyInPopup</strong>. This mode is an analog of IncrementalSearch, but it also expands nodes if it is needed when you type a search text. This mode works only if a popup window is shown.</p>
<p>     o <strong>AutoComplete</strong>. In the Auto Completion mode, the text in the edit box is automatically completed if it matches a DisplayMember field value from drop-down nodes.</p>
<p>     o <strong>AutoFilter</strong>. This mode applies a filter to the DisplayMember column. Filter is formed when you type a text in the edit box. In addition, it opens a popup window and looks for nodes retaining paths to the root to show the context.</p>
<p>For virtual data loading (on demand) in the inner TreeList you can handle its BeforeExpand event. Please refer to the following help articles:</p>
<p><br /> <a href="http://documentation.devexpress.com/#WindowsForms/CustomDocument325"><u>How to: Implement Dynamic Loading in Unbound Mode</u></a><br /> <a href="http://documentation.devexpress.com/#WindowsForms/CustomDocument5560"><u>Dynamic Data Loading via Events</u></a><br /> <a href="http://documentation.devexpress.com/#WindowsForms/CustomDocument5561"><u>How to: Implement a Tree Structure for a Business Object</u></a></p>
<p> to learn which virtual Modes TreeList supports.</p>

<br/>


