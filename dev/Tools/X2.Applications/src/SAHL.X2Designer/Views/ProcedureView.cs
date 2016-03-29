using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Northwoods.Go;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Forms;
using SAHL.X2Designer.Items;
using SAHL.X2Designer.Misc;
using SAHL.X2Designer.XML;

namespace SAHL.X2Designer.Views
{
    public class ProcessView : GoView, ICutCopyPasteTarget
    {
        int m_CurrentVK = 0;
        int m_CurrentSC = 0;
        ProcessDocument m_Document;
        WorkflowItemType m_ActiveItem;
        IPopupMenu m_PopupMenuHandler;

        public string m_ZoomValue;
        public int m_toolStripButtonIndex;
        public bool canSelectLastProperty = false;
        private GoRectangle header;
        public BaseItem lastSelectedItem = null;

        public bool justSelectedObject = false;

        private Keys lastKeyPressed;

        private bool AllowCreateObject;

        public PointF pasteLocation = new PointF(0, 0);

        public bool clipBoardHasContents = false;

        private bool m_CreatedObject;

        // declare a delegate with arguments of type BaseItem
        public delegate void OnWorkFlowItemSelectedHandler(/*IWorkflowItem*/BaseItem SelectedItem);
        public delegate void OnWorkFlowItemUnSelectedHandler(IWorkflowItem UnSelectedItem);
        public delegate void OnObjectChangedHandler(ObjectChangeType ChangeType, GoChangedEventArgs e);

        public bool toolBarItemChecked = false;

        private GoObject activityDrawn = null;

        public bool isDrawing = false;
        private BaseItem _holdFromNode = new BaseItem();

        public BaseItem holdFromNode { get { return _holdFromNode; } set { _holdFromNode = value; } }

        private BaseItem _holdToNode = new BaseItem();

        public BaseItem holdToNode { get { return _holdToNode; } set { _holdToNode = value; } }

        Point mStartPoint = new Point();
        Point mLastPoint = new Point();

        // declare an event of the delegates type
        public event OnWorkFlowItemSelectedHandler OnWorkFlowItemSelected;
        public event OnWorkFlowItemUnSelectedHandler OnWorkFlowItemUnSelected;
        public event OnObjectChangedHandler OnObjectChanged;

        public delegate void OnPropertyChangedHandler(PropertyType propType);
        public event OnPropertyChangedHandler OnPropertyChanged;

        public delegate void OnStageTransitionMessageChangedHandler(string NewStageTransitionMessage);
        public event OnStageTransitionMessageChangedHandler OnStageTransitionMessageChanged;

        private bool myOriginalScale = true;
        private PointF myOriginalDocPosition = new PointF();
        private float myOriginalDocScale = 1.0f;

        public GoSelection holdSelection;

        private bool mustSelectWorkFlow;

        public ProcessView()
            : base()
        {
            this.m_ZoomValue = MainForm.App.cbxZoom.Text;

            GoContextMenu cm = new GoContextMenu(this);
            this.ContextMenu = cm;
            AllowLink = false;
            this.DragsRealtime = true;
            this.SmoothingMode = SmoothingMode.None;
            holdSelection = new GoSelection(this);
        }

        public new ProcessDocument Document
        {
            get
            {
                return m_Document;
            }
        }

        #region Misc

        public void FireOnPropertyChangedEvent(PropertyType propType)
        {
            if (OnPropertyChanged != null)
            {
                OnPropertyChanged(propType);
            }
        }

        public void FireOnStageTransitionMessageChangedEvent(string NewStageTransitionMessage)
        {
            if (OnStageTransitionMessageChanged != null)
            {
                OnStageTransitionMessageChanged(NewStageTransitionMessage);
            }
        }

        public void App_onActivityItemChecked(object sender)
        {
            foreach (GoObject o in m_Document)
            {
                o.Selectable = false;
            }
            toolBarItemChecked = true;
        }

        public void App_onActivityItemUnChecked(object sender)
        {
            foreach (GoObject o in m_Document)
            {
                if (o.GetType() != typeof(CustomLink))
                {
                    o.Selectable = true;
                    o.Editable = true;
                    o.Movable = true;
                }
                else
                {
                    o.Selectable = false;
                    o.Editable = false;
                    o.Movable = false;
                }
            }
            toolBarItemChecked = false;
            if (isDrawing == false)
            {
                m_ActiveItem = WorkflowItemType.None;
            }
        }

        public WorkflowItemType ActiveItemType
        {
            get
            {
                return m_ActiveItem;
            }
            set
            {
                m_ActiveItem = value;
            }
        }

        private void m_Document_Changed(object sender, GoChangedEventArgs e)
        {
            if (OnObjectChanged != null) // && MainForm.App.documentIsBeingOpened == false)
            {
                switch (e.Hint)
                {
                    case 902:
                        {
                            OnObjectChanged(ObjectChangeType.WorkFlowInserted, e);
                            if (e.GoObject is WorkFlow)
                            {
                                this.Document.m_WorkFlows.Add(e.GoObject as WorkFlow);
                                this.Document.m_CurrentWorkFlow = this.Document.m_WorkFlows[this.Document.m_WorkFlows.Count - 1];
                            }

                            return;
                        }
                    case 104:
                        {
                            break;
                        }
                    case 10023:
                        {
                            break;
                        }
                }

                switch (e.SubHint)
                {
                    case 1051:
                        {
                            OnObjectChanged(ObjectChangeType.Inserted, e);

                            if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow != null)
                            {
                                foreach (object o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                                {
                                    if (o.GetType() == typeof(CustomLink))
                                    {
                                        CustomLink l = o as CustomLink;
                                        l.CalculateStroke();
                                    }
                                }
                            }

                            if (e.NewValue is BaseActivity)
                            {
                                MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Activities.Add(e.NewValue as BaseActivity);
                            }
                            if (e.NewValue is BaseState)
                            {
                                MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States.Add(e.NewValue as BaseState);
                            }

                            break;
                        }
                    case 1052:
                        {
                            if (e.OldValue is BaseActivity)
                            {
                                for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Activities.Count; x++)
                                {
                                    if (e.OldValue == MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Activities[x])
                                    {
                                        MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Activities.RemoveAt(x);
                                        break;
                                    }
                                }
                            }
                            if (e.OldValue is BaseState)
                            {
                                for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States.Count; x++)
                                {
                                    if (e.OldValue == MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[x])
                                    {
                                        MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States.RemoveAt(x);
                                        break;
                                    }
                                }
                            }

                            WorkFlow w = e.GoObject as WorkFlow;
                            if (w != null)
                            {
                                OnObjectChanged(ObjectChangeType.WorkFlowDeleted, e);
                            }

                            break;
                        }
                    case 1501:
                        {
                            OnObjectChanged(ObjectChangeType.Renamed, e);
                            if (MainForm.App.documentIsBeingOpened == false)
                            {
                                MainForm.App.GetCurrentView().FireOnPropertyChangedEvent(PropertyType.Name);
                            }
                            IWorkflowItem itm = e.GoObject.Parent as IWorkflowItem;
                            if (itm != null)
                            {
                                switch (itm.WorkflowItemBaseType)
                                {
                                    case WorkflowItemBaseType.State:
                                        {
                                            BaseState mItem = e.GoObject.Parent as BaseState;
                                            mItem.UpdateCodeSectionData(e.OldValue.ToString(), e.NewValue.ToString());
                                            break;
                                        }
                                    case WorkflowItemBaseType.Activity:
                                        {
                                            BaseActivity mItem = e.GoObject.Parent as BaseActivity;
                                            mItem.UpdateCodeSectionData(e.OldValue.ToString(), e.NewValue.ToString());
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                }
            }

            if (e.SubHint == 1051)
            {
                BaseActivity o = e.Object as BaseActivity;
                if (o != null)
                {
                    activityDrawn = o;
                }
            }

            if (MainForm.App.documentIsBeingOpened)
                return;

            bool changesMade = false;
            if (e.Hint == 901 ||
                e.Hint == 902 ||
                e.Hint == 903)
            {
                if (e.SubHint == 1051 ||
                    e.SubHint == 1052 ||
                    e.SubHint == 1501 ||
                    e.SubHint == 2004)
                {
                    if (e.SubHint < 2702 || e.SubHint > 2722)
                    {
                        setModified(true);
                        return;
                    }
                }
            }

            Form frmP = this.Parent as Form;
            if (!frmP.Text.Contains("Version"))
            {
                UpdateTitle();
                this.Document.ComputeBounds();
            }
        }

        #endregion Misc

        #region View Information

        /// <summary>
        /// Update the title bar with the view's document's Name, and an indication
        /// of whether the document is read-only and whether it has been modified.
        /// </summary>
        public virtual void UpdateTitle()
        {
            Form win = this.Parent as Form;
            if (win != null)
            {
                String title = "";

                if (this.Document.Location.Length == 0)
                {
                    title = "New Document";
                }
                else
                    title = this.Document.Location + " Version " + XMLHandling.GetMapVersion(this.Document.Location);

                if (m_Document.IsModified && MainForm.App.newDocumentBeingCreated == false && MainForm.App.documentIsBeingOpened == false)
                    title += " *";
                win.Text = title;
            }
        }

        public bool CheckIfModified()
        {
            Form win = this.Parent as Form;
            if (win != null)
            {
                if (win.Text.Contains("*"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public void setModified(bool modified)
        {
            Form win = this.Parent as Form;
            if (win != null)
            {
                if (modified == true)
                {
                    if (!win.Text.Contains("*"))
                    {
                        String title = "";
                        if (this.Document.Location.Length > 0)
                        {
                            title = this.Document.Location + " Version " + XML.XMLHandling.GetMapVersion(this.Document.Location);
                        }
                        else
                        {
                            title = "New Document";
                        }
                        win.Text = title + " *";
                    }
                }
                else
                {
                    win.Text = win.Text.Replace(" *", "");
                }
            }
        }

        #endregion View Information

        #region Overrides

        public override GoDocument CreateDocument()
        {
            m_Document = new Documents.ProcessDocument();
            m_Document.Changed += new GoChangedEventHandler(m_Document_Changed);
            return m_Document;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey | e.KeyCode == Keys.Shift | e.KeyCode == Keys.Control | e.KeyCode == Keys.ControlKey)
            {
                return;
            }
            if (e.KeyCode == Keys.Escape)
            {
                if (MainForm.App.lstMultipleSelectionItems.Count > 0)
                {
                    for (int x = 0; x < MainForm.App.lstMultipleSelectionItems.Count; x++)
                    {
                        if (MainForm.App.lstMultipleSelectionItems[x].baseItem.Visible == false)
                        {
                            MainForm.App.lstMultipleSelectionItems[x].baseItem.Visible = true;
                        }
                    }
                    for (int x = 0; x < MainForm.App.lstMultipleSelectionLinks.Count; x++)
                    {
                        if (MainForm.App.lstMultipleSelectionLinks[x].customLink.Visible == false)
                        {
                            MainForm.App.lstMultipleSelectionLinks[x].customLink.Visible = true;
                        }
                    }
                }
                MainForm.App.lstMultipleSelectionItems.Clear();
                MainForm.App.lstMultipleSelectionLinks.Clear();
                this.Selection.RemoveAllSelectionHandles();
                this.Selection.Clear();
                this.holdSelection.RemoveAllSelectionHandles();
                this.holdSelection.Clear();
            }
            if (e.KeyCode == Keys.F4)
            {
                return;
            }

            if (this.Selection != null)
            {
                switch (e.KeyCode)
                {
                    // left
                    case Keys.Left:
                        MoveSelectionLeft(this.Selection);
                        e.Handled = true;
                        MainForm.App.GetCurrentView().UpdateTitle();
                        return;
                    // up
                    case Keys.Up:
                        MoveSelectionUp(this.Selection);
                        e.Handled = true;
                        MainForm.App.GetCurrentView().UpdateTitle();
                        return;
                    // right
                    case Keys.Right:
                        MoveSelectionRight(this.Selection);
                        e.Handled = true;
                        MainForm.App.GetCurrentView().UpdateTitle();
                        return;
                    // down
                    case Keys.Down:
                        MoveSelectionDown(this.Selection);
                        e.Handled = true;
                        MainForm.App.GetCurrentView().UpdateTitle();
                        return;
                    case Keys.Delete:
                        {
                            break;
                        }
                    case (Keys.ShiftKey | Keys.F4):
                        {
                            break;
                        }

                    default:
                        if (MainForm.App.m_PropsView != null)
                        {
                            if (MainForm.App.m_PropsView.lastSelectedProperty != null)
                            {
                                if (MainForm.App.m_PropsView.lastSelectedProperty.Length > 0)
                                {
                                    if (MainForm.App.m_PropsView.lastSelectedProperty.Length > 0 && canSelectLastProperty &&
                                         lastSelectedItem != null)
                                    {
                                        if (this.Selection.Primary != null)
                                        {
                                            lastSelectedItem = this.Selection.Primary as BaseItem;
                                        }
                                        SelectLastProperty(e.KeyCode);
                                    }
                                    else
                                    {
                                        SelectNameProperty(e.KeyCode);
                                    }
                                }

                                else
                                {
                                    SelectNameProperty(e.KeyCode);
                                }
                            }
                            e.Handled = true;
                            return;
                        }
                        break;
                }
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        private void MoveSelectionUp(GoSelection Selection)
        {
            StartTransaction();
            foreach (GoObject O in Selection)
            {
                O.Top -= 1;
            }
            FinishTransaction("MoveSelectionUp");
        }

        private void MoveSelectionDown(GoSelection Selection)
        {
            StartTransaction();
            foreach (GoObject O in Selection)
            {
                O.Top += 1;
            }
            FinishTransaction("MoveSelectionDown");
        }

        private void MoveSelectionLeft(GoSelection Selection)
        {
            StartTransaction();
            foreach (GoObject O in Selection)
            {
                O.Left -= 1;
            }
            FinishTransaction("MoveSelectionLeft");
        }

        private void MoveSelectionRight(GoSelection Selection)
        {
            StartTransaction();
            foreach (GoObject O in Selection)
            {
                O.Left += 1;
            }
            FinishTransaction("MoveSelectionRight");
        }

        protected override void OnDragOver(DragEventArgs evt)
        {
            if (evt.Y > 75)
            {
                base.OnDragOver(evt);
            }
            else
            {
                this.AllowDrop = false;
            }
        }

        protected override void OnMouseDown(MouseEventArgs evt)
        {
            GoView cView = MainForm.App.GetCurrentView();
            holdFromNode = null;
            holdToNode = null;
            Point screenPnt = new Point(evt.X, evt.Y);
            PointF docPnt = ConvertViewToDoc(screenPnt);

            if (evt.Button == MouseButtons.Right)
            {
                pasteLocation = evt.Location;
                foreach (GoObject o in Selection)
                {
                    holdSelection.Add(o);
                }
                base.OnMouseDown(evt);
                Selection.Clear();

                return;
            }
            else
            {
                holdSelection.Clear();
                this.Refresh();
            }

            if (AllowCreateObject && evt.Button == MouseButtons.Left)
            {
                bool foundObject = false;
                WorkflowItemType objectTypeFound = WorkflowItemType.None;
                foreach (GoObject o in m_Document.PickObjects(docPnt, false, null, 10))
                {
                    if (o.GetType().ToString() == "Northwoods.Go.GoImage")
                    {
                        GoImage myNodeIcon = o as GoImage;

                        if (myNodeIcon.ParentNode.GetType().BaseType != typeof(BaseActivity))
                        {
                            foundObject = true;
                            switch (myNodeIcon.ParentNode.GetType().ToString())
                            {
                                case "SAHL.X2Designer.Items.SystemState":
                                    {
                                        objectTypeFound = WorkflowItemType.SystemState;
                                        break;
                                    }
                            }
                        }

                        if (MainForm.App.isDrawingActivity || myNodeIcon.ParentNode.GetType() != typeof(ClapperBoard)
                            && (myNodeIcon.ParentNode is BaseState == false)
                            && myNodeIcon.ParentNode.GetType().BaseType != typeof(BaseActivity))
                        {
                            cView.DrawsXorMode = false;
                            if (toolBarItemChecked)
                            {
                                AllowLink = true;
                            }
                            else
                            {
                                AllowLink = false;
                            }
                            holdFromNode = myNodeIcon.ParentNode as BaseItem;
                            if (evt.Button == MouseButtons.Left && AllowLink)
                            {
                                mStartPoint = new Point(evt.X, evt.Y);
                                isDrawing = true;
                                mLastPoint.X = -1;
                                mLastPoint.Y = -1;
                            }
                        }
                        else
                        {
                            MainForm.App.unCheckToolStripButtons();
                            AllowLink = false;
                        }
                    }

                    MainForm.App.enableToolStripTools();
                }
                if ((!foundObject) && (m_ActiveItem != WorkflowItemType.ReturnWorkFlowActivity))
                {
                    if (MainForm.App.isDrawingActivity)
                    {
                        this.Selection.Clear();
                        isDrawing = false;
                        AllowCreateObject = false;
                        MessageBox.Show("An activity must be drawn from a state!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        MainForm.App.unCheckToolStripButtons();
                        MainForm.App.isDrawingActivity = false;
                    }
                }
                if ((foundObject) && (m_ActiveItem == WorkflowItemType.CallWorkFlowActivity))
                {
                    if (objectTypeFound == WorkflowItemType.SystemState)
                    {
                        this.Selection.Clear();
                        isDrawing = false;
                        AllowCreateObject = false;
                        MessageBox.Show("A Call Workflow activity cannot be drawn from a system state!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        MainForm.App.unCheckToolStripButtons();
                        MainForm.App.isDrawingActivity = false;
                    }
                }

                if (Cursor != Cursors.No)
                {
                    screenPnt = new Point(evt.X, evt.Y);
                    docPnt = ConvertViewToDoc(screenPnt);

                    switch (m_ActiveItem)
                    {
                        case WorkflowItemType.UserState:
                            m_Document.CurrentWorkFlow.CreateUserState(docPnt);

                            break;
                        case WorkflowItemType.HoldState:
                            m_Document.CurrentWorkFlow.CreateHoldState(docPnt);

                            break;
                        case WorkflowItemType.SystemState:
                            m_Document.CurrentWorkFlow.CreateSystemState(docPnt);

                            break;
                        case WorkflowItemType.SystemDecisionState:
                            m_Document.CurrentWorkFlow.CreateSystemDecisionState(docPnt);

                            break;
                        case WorkflowItemType.CommonState:
                            m_Document.CurrentWorkFlow.CreateCommonState(docPnt);
                            break;
                        case WorkflowItemType.ArchiveState:
                            m_Document.CurrentWorkFlow.CreateArchiveState(docPnt);
                            break;

                        case WorkflowItemType.Comment:
                            m_Document.CurrentWorkFlow.CreateComment(docPnt);
                            break;
                    }
                    if (isDrawing)
                    {
                        m_CreatedObject = true;
                    }
                }
                base.OnMouseDown(evt);

                if (isDrawing == false)
                {
                    m_ActiveItem = WorkflowItemType.None;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs evt)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                if (MainForm.App.lblStatus.Text.Contains("Saving"))
                {
                    this.Cursor = Cursors.WaitCursor;
                    return;
                }

                Cursor = Cursors.Default;

                if (MainForm.App.GetCurrentView().Selection.Primary != null)
                {
                    if (isDrawing)
                    {
                        if (evt.X < this.Bounds.Left)
                        {
                            Cursor.Current = Cursors.Hand;
                            this.ScrollPage(-1, 0);
                            this.Refresh();
                        }
                        else if (evt.X > this.Bounds.Right)
                        {
                            Cursor.Current = Cursors.Hand;
                            this.ScrollPage(1, 0);
                            this.Refresh();
                        }
                        if (evt.Y < this.Bounds.Top)
                        {
                            Cursor.Current = Cursors.Hand;
                            this.ScrollPage(0, -1);
                            this.Refresh();
                        }
                        else if (evt.Y > this.Bounds.Bottom)
                        {
                            Cursor.Current = Cursors.Hand;
                            this.ScrollPage(0, 1);
                            this.Refresh();
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            this.Refresh();
                        }
                    }
                }
            }

            PointF workFlowPoint = new PointF();
            if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow != null)
            {
                PointF mPoint = ConvertViewToDoc(new Point(evt.X, evt.Y));
                workFlowPoint = ConvertViewToDoc(new Point(Convert.ToInt32(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.expandedLocation.X), Convert.ToInt32(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.expandedLocation.Y)));
                if (mPoint.Y < workFlowPoint.Y)
                {
                    if (toolBarItemChecked)
                    {
                        if (mPoint.Y > header.Top && mPoint.Y < header.Bottom)
                        {
                            Cursor = Cursors.No;
                            AllowCreateObject = false;
                            this.Refresh();
                        }
                    }
                }
                else
                {
                    Cursor = Cursors.Default;
                    AllowCreateObject = true;
                }
                if (isDrawing)
                {
                    if (mLastPoint.X != -1)
                    {
                        // here first do the xor draw (use GoViews version)
                        ControlPaint.DrawReversibleLine(PointToScreen(mStartPoint), PointToScreen(mLastPoint), Color.Black);
                    }

                    if (!mLastPoint.Equals(mStartPoint))
                    {
                        mLastPoint = new Point(evt.X - 5, evt.Y - 5);
                        // doa  normal draw (use GoViews version)
                        ControlPaint.DrawReversibleLine(PointToScreen(mStartPoint), PointToScreen(mLastPoint), Color.Black);
                    }
                }
                else
                {
                    if (evt.Y < 100)
                    {
                        AllowDrop = false;
                    }
                    else
                    {
                        AllowDrop = true;

                        base.OnMouseMove(evt);
                    }
                    if (toolBarItemChecked)
                    {
                        Cursor = Cursors.Cross;
                    }
                }
            }
        }

        protected override void OnDragDrop(DragEventArgs evt)
        {
            base.OnDragDrop(evt);
            UpdateTitle();
        }

        public override void UpdateView()
        {
            bool foundBackground = false;

            if (this.Document != null)
            {
                foreach (GoObject o in this.BackgroundLayer)
                {
                    if (o.GetType() == typeof(GoRectangle))
                    {
                        foundBackground = true;
                        GoRectangle r = o as GoRectangle;
                        PointF mPoint = this.Location;
                        r.Location = new PointF(mPoint.X, r.Location.Y);
                        break;
                    }
                }
                if (foundBackground == false)
                {
                    header = new GoRectangle();
                    header.Selectable = false;

                    PointF mPoint = ConvertViewToDoc(new Point(0, 0));
                    header.Bounds = new RectangleF(mPoint.X, mPoint.Y, 10000, mPoint.Y + 50);
                    header.Pen = null;

                    header.Brush = Brushes.PowderBlue;

                    StartTransaction();
                    this.BackgroundLayer.Add(header);

                    FinishTransaction("Created Header");
                }
            }

            base.UpdateView();
        }

        public override void CopyToClipboard(IGoCollection coll)
        {
            //Stream ofile = File.Open("test.graph", FileMode.Create);
            //IFormatter oformatter = new BinaryFormatter();
            //oformatter.Serialize(ofile, this.Document);
            //ofile.Close();

            //Stream ifile = File.Open("test.graph", FileMode.Open);
            //IFormatter iformatter = new BinaryFormatter();
            //GoDocument doc = iformatter.Deserialize(ifile) as GoDocument;
            //ifile.Close();

            base.CopyToClipboard(coll);
            //          this.Selection.Clear();
        }

        public override GoCopyDictionary PasteFromClipboard()
        {
            ProcessDocument document1 = this.Document;
            if (document1 != null)
            {
                object obj1 = null;
                try
                {
                    IDataObject obj2 = Clipboard.GetDataObject();
                    if (obj2 == null)
                    {
                        return null;
                    }
                    obj1 = obj2.GetData(document1.DataFormat);
                }
                catch (SecurityException e1)
                {
                    ExceptionPolicy.HandleException(e1, "X2Designer");
                }
                catch (Exception e2)
                {
                    ExceptionPolicy.HandleException(e2, "X2Designer");
                }
                if ((obj1 != null) && (obj1 is GoDocument))
                {
                    ProcessDocument document2 = (ProcessDocument)obj1;
                    List<GoObject> lstObjects = new List<GoObject>();
                    foreach (GoObject o in document2)
                    {
                        lstObjects.Add(o);
                    }
                    for (int z = 0; z < lstObjects.Count; z++)
                    {
                        GoObject o = lstObjects[z];
                        BaseState i = o as BaseState;
                        if (i != null)
                        {
                            o.Remove();

                            this.Document.CurrentWorkFlow.Add(i);

                            float x;
                            float y;
                            if (pasteLocation == new PointF(0, 0))
                            {
                                x = i.Location.X + 50;
                                y = i.Location.Y + 50;
                            }
                            else
                            {
                                x = pasteLocation.X;
                                y = pasteLocation.Y;
                            }

                            MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States.Add(i);
                            i.Name = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.GetNextStateName();
                            i.Location = new PointF(x, y);
                            this.Selection.Add(i);
                        }
                    }
                    for (int z = 0; z < lstObjects.Count; z++)
                    {
                        GoObject o = lstObjects[z];
                        BaseActivity i = o as BaseActivity;
                        if (i != null)
                        {
                            o.Remove();

                            this.Document.CurrentWorkFlow.Add(i);

                            float x;
                            float y;
                            if (pasteLocation == new PointF(0, 0))
                            {
                                x = i.Location.X + 50;
                                y = i.Location.Y + 50;
                            }
                            else
                            {
                                x = pasteLocation.X;
                                y = pasteLocation.Y;
                            }
                            MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Activities.Add(i);
                            i.Name = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.GetNextActivityName();
                            i.Location = new PointF(x, y);
                            this.Selection.Add(i);
                        }
                    }
                    for (int z = 0; z < lstObjects.Count; z++)
                    {
                        GoObject o = lstObjects[z];

                        CustomLink i = o as CustomLink;
                        if (i != null)
                        {
                            o.Remove();
                            this.Document.CurrentWorkFlow.Add(i);

                            float x;
                            float y;
                            if (pasteLocation == new PointF(0, 0))
                            {
                                x = i.Location.X + 50;
                                y = i.Location.Y + 50;
                            }
                            else
                            {
                                x = pasteLocation.X;
                                y = pasteLocation.Y;
                            }
                            i.Location = new PointF(x, y);
                            i.CalculateStroke();
                        }
                    }

                    for (int z = 0; z < lstObjects.Count; z++)
                    {
                        GoObject o = lstObjects[z];

                        Comment i = o as Comment;
                        if (i != null)
                        {
                            o.Remove();
                            this.Document.CurrentWorkFlow.Add(i);

                            float x;
                            float y;
                            if (pasteLocation == new PointF(0, 0))
                            {
                                x = i.Location.X + 50;
                                y = i.Location.Y + 50;
                            }
                            else
                            {
                                x = pasteLocation.X;
                                y = pasteLocation.Y;
                            }
                            i.Location = new PointF(x, y);
                            i.CommentID = this.Document.CurrentWorkFlow.lastCommentID++;
                            this.Selection.Add(i);
                        }
                    }

                    MainForm.App.GetCurrentView().Refresh();
                }

                foreach (GoObject obj3 in this.Document.CurrentWorkFlow)
                {
                    if (obj3 is CustomLink)
                    {
                        CustomLink l = obj3 as CustomLink;
                        if (l.FromNode == null || l.ToNode == null)
                        {
                            if (l.ToNode != typeof(CallWorkFlowActivity))
                            {
                                l.Remove();
                            }
                        }
                    }
                }

                List<BaseActivity> lstActivites = new List<BaseActivity>();
                foreach (GoObject o in this.Document.CurrentWorkFlow)
                {
                    BaseActivity a = o as BaseActivity;
                    if (a != null)
                    {
                        int linkCount = 0;
                        foreach (CustomLink l in a.Links)
                        {
                            linkCount++;
                        }
                        if (linkCount == 1)
                        {
                            if (a.GetType() != typeof(CallWorkFlowActivity))
                            {
                                foreach (CustomLink l in a.Links)
                                {
                                    l.Remove();
                                }
                                foreach (GoPort p in a.Ports)
                                {
                                    p.Remove();
                                }
                            }
                        }
                        if (linkCount < 2)
                        {
                            if (linkCount == 1 && a is CallWorkFlowActivity)
                            {
                                continue;
                            }
                            lstActivites.Add(a);
                        }
                    }
                }
                if (lstActivites.Count > 0)
                {
                    frmMoveLink mFrmMoveLink = new frmMoveLink(lstActivites);
                    mFrmMoveLink.ShowDialog();
                    mFrmMoveLink.Dispose();
                }
            }

            return null;
        }

        protected override void OnMouseUp(MouseEventArgs evt)
        {
            bool mustSelectActivity = false;

            if (Cursor == Cursors.No)
            {
                MainForm.App.unCheckToolStripButtons();
                Cursor = Cursors.Default;
            }
            else
            {
                if (evt.Button == MouseButtons.Left)
                {
                    if (!mLastPoint.Equals(mStartPoint))
                    {
                        mLastPoint = new Point(evt.X, evt.Y);
                        // do a xor draw (use GoViews version)
                        ControlPaint.DrawReversibleLine(PointToScreen(mStartPoint), PointToScreen(mLastPoint), Color.Black);

                        mStartPoint.X = -1;
                        mStartPoint.Y = -1;
                        mLastPoint.X = -1;
                        mLastPoint.Y = -1;
                    }
                    MainForm.App.unCheckToolStripButtons();

                    GoView cView = MainForm.App.GetCurrentView(); ;
                    bool foundNode = false;
                    Point screenPnt = new Point(evt.X, evt.Y);
                    PointF docPnt = ConvertViewToDoc(screenPnt);

                    foreach (GoObject o in m_Document.PickObjects(docPnt, false, null, 10))
                    {
                        if (o.GetType().ToString() == "Northwoods.Go.GoImage")
                        {
                            GoImage myNodeIcon = o as GoImage;
                            cView.DrawsXorMode = false;
                            holdToNode = myNodeIcon.ParentNode as BaseItem;
                            if (holdFromNode != null && holdToNode != null)
                            {
                                if (holdFromNode.Text != holdToNode.Text)
                                {
                                    foundNode = true;
                                }
                            }
                        }
                    }
                    if (foundNode == false)
                    {
                        holdToNode = holdFromNode;
                    }
                    if (AllowLink)
                    {
                        DrawLink(evt);

                        isDrawing = false;
                    }

                    if (m_CreatedObject == true && activityDrawn != null)
                    {
                        mustSelectActivity = true;
                    }

                    if (m_CreatedObject == true)
                    {
                        m_CreatedObject = false;
                    }
                }

                this.Refresh();
                base.OnMouseUp(evt);
            }
            if (mustSelectActivity == true)
            {
                this.Selection.Clear();
                this.Selection.Add(activityDrawn);
                activityDrawn = null;
            }
            MainForm.App.isDrawingActivity = false;
        }

        private void SelectNameProperty(Keys keyCode)
        {
            if (MainForm.App.m_PropsView.propertyGrid.SelectedGridItem == null)
            {
                return;
            }
            GridItem p_gi = MainForm.App.m_PropsView.propertyGrid.SelectedGridItem.Parent.Parent;
            bool found = false;
            foreach (GridItem egi in p_gi.GridItems)
            {
                foreach (GridItem eegi in egi.GridItems)
                {
                    System.Diagnostics.Debug.WriteLine(eegi.Label);
                    if (eegi.Label.Contains("Name"))
                    {
                        MainForm.App.m_PropsView.Select();
                        MainForm.App.m_PropsView.propertyGrid.SelectedGridItem = eegi;
                        MainForm.App.m_PropsView.propertyGrid.Focus();

                        found = true;
                        break;
                    }
                }
                if (found == true)
                {
                    IntPtr FHandle = GetFocus();
                    PostMessage(FHandle, 0x100, m_CurrentVK, m_CurrentSC);
                    break;
                }
            }
        }

        private void SelectLastProperty(Keys keyCode)
        {
            if (MainForm.App.m_PropsView.propertyGrid.SelectedGridItem == null)
            {
                return;
            }
            GridItem p_gi = MainForm.App.m_PropsView.propertyGrid.SelectedGridItem.Parent.Parent;
            if (p_gi == null)
            {
                return;
            }
            bool found = false;
            foreach (GridItem egi in p_gi.GridItems)
            {
                foreach (GridItem eegi in egi.GridItems)
                {
                    System.Diagnostics.Debug.WriteLine(eegi.Label);
                    if (eegi.Label == MainForm.App.m_PropsView.lastSelectedProperty)
                    {
                        MainForm.App.m_PropsView.Select();
                        MainForm.App.m_PropsView.propertyGrid.SelectedGridItem = eegi;
                        MainForm.App.m_PropsView.propertyGrid.Focus();

                        found = true;
                        break;
                    }
                }

                if (found == true)
                {
                    IntPtr FHandle = GetFocus();
                    PostMessage(FHandle, 0x100, m_CurrentVK, m_CurrentSC);
                    break;
                }
            }
            if (found == false)
            {
                SelectNameProperty(keyCode);
            }
        }

        protected override void DoInternalDrag(DragEventArgs evt)
        {
            Point P = PointToClient(new Point(evt.X, evt.Y));
            if (P.Y > (this.Document.CurrentWorkFlow.Top + 32))
            {
                base.DoInternalDrag(evt);
            }
        }

        public override void DeleteSelection(GoSelection sel)
        {
            DeleteSelectionWithTransaction(sel);
        }

        public void DeleteSelectionWithTransaction(GoSelection Sel)
        {
            this.Document.StartTransaction();
            Delete(Sel);
            this.Document.FinishTransaction("Delete Selection");
        }

        private void Delete(GoSelection sel)
        {
            GoView myView = MainForm.App.GetCurrentView();
            if (myView.CanDeleteObjects() == true)
            {
                if (sel != null)
                {
                    List<GoObject> lstItemsToRemove = new List<GoObject>();
                    sel.RemoveAllSelectionHandles();
                    if (MainForm.App.m_FindReplaceView != null)
                    {
                        MainForm.App.m_FindReplaceView.listViewResults.Items.Clear();
                    }

                    if (MainForm.App.m_FindReplaceView != null)
                    {
                        for (int x = 0; x < MainForm.App.m_FindReplaceView.listViewResults.Items.Count; x++)
                        {
                            if (MainForm.App.m_FindReplaceView.listViewResults.Items[x].Tag.GetType() == typeof(UserState))
                            {
                                UserState item = MainForm.App.m_FindReplaceView.listViewResults.Items[x].Tag as UserState;
                            }
                        }
                    }

                    GoObject[] selection = sel.CopyArray();
                    for (int y = selection.Length - 1; y >= 0; y--)
                    {
                        GoObject o = selection[y];

                        if (o is CustomLink)
                        {
                            for (int x = 0; x < MainForm.App.lstMultipleSelectionLinks.Count; x++)
                            {
                                if (MainForm.App.lstMultipleSelectionLinks[x].customLink == o as CustomLink)
                                {
                                    MainForm.App.lstMultipleSelectionLinks.RemoveAt(x);
                                }
                            }
                        }
                        if (o is BaseItem)
                        {
                            for (int x = 0; x < MainForm.App.lstMultipleSelectionItems.Count; x++)
                            {
                                if (MainForm.App.lstMultipleSelectionItems[x].baseItem == o as BaseItem)
                                {
                                    MainForm.App.lstMultipleSelectionItems.RemoveAt(x);
                                }
                            }
                        }
                    }

                    foreach (GoObject o in sel)
                    {
                        if (o is BaseActivity)
                        {
                            BaseState fromNode = null;
                            BaseActivity mActivity = o as BaseActivity;
                            foreach (CustomLink l in mActivity.Links)
                            {
                                if (l.FromNode != mActivity)
                                {
                                    fromNode = l.FromNode as BaseState;
                                    break;
                                }
                            }
                            if (fromNode != null)
                            {
                                foreach (CustomLink l in fromNode.Links)
                                {
                                    if (l.ToNode != fromNode)
                                    {
                                        BaseActivity toActivity = l.ToNode as BaseActivity;
                                        if (toActivity != null)
                                        {
                                            if (toActivity.Priority > mActivity.Priority)
                                            {
                                                toActivity.Priority--;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (o is BaseActivity
                            || o is BaseState)
                        {
                            GoIconicNode myNode = new GoIconicNode();
                            myNode = o as GoIconicNode;

                            foreach (MultiPortNodePort p in myNode.Ports)
                            {
                                GoIconicNode myActivityNode = new GoIconicNode();
                                foreach (CustomLink l in p.Links)
                                {
                                    if (l.ToNode is BaseActivity ||
                                        l.FromNode is BaseActivity)
                                    {
                                        myActivityNode = l.FromNode as BaseActivity;
                                        if (myActivityNode == null)
                                        {
                                            myActivityNode = l.ToNode as BaseActivity;
                                        }
                                        if (myActivityNode != null || myActivityNode == sel.Primary as BaseActivity)
                                        {
                                            foreach (GoLink al in myActivityNode.Links)
                                            {
                                                lstItemsToRemove.Add(al);
                                                //al.Remove();
                                            }
                                            if (sel.Primary != myActivityNode as GoObject)
                                            {
                                                lstItemsToRemove.Add(myActivityNode);
                                                //myActivityNode.Remove();
                                            }
                                        }
                                    }
                                    lstItemsToRemove.Add(l);
                                    //l.Remove();
                                }
                            }

                            if (o is BaseState)
                            {
                                for (int x = 0; x < Document.CurrentWorkFlow.States.Count; x++)
                                {
                                    if (Document.CurrentWorkFlow.States[x].GetType() == typeof(CommonState))
                                    {
                                        CommonState mCommonState = Document.CurrentWorkFlow.States[x] as CommonState;
                                        AppliedStatesCollection mCollection = mCommonState.AppliesTo;
                                        for (int y = 0; y < mCollection.Count; y++)
                                        {
                                            GoIconicNode mNode = o as GoIconicNode;
                                            if (mCollection[y] == mNode as BaseState)
                                            {
                                                lstItemsToRemove.Add(mCollection[y]);
                                                //mCollection.RemoveAt(y);
                                            }
                                        }
                                    }
                                }
                            }

                            if (o is BaseActivity)
                            {
                                for (int x = 0; x < MainForm.App.GetCurrentView().Document.WorkFlows.Length; x++)
                                {
                                    foreach (GoObject obj in MainForm.App.GetCurrentView().Document.WorkFlows[x])
                                    {
                                        CallWorkFlowActivity mActivity = obj as CallWorkFlowActivity;
                                        if (mActivity != null)
                                        {
                                            BaseActivity mAct = o as BaseActivity;
                                            BaseActivity mCall = mActivity.ActivityToCall;

                                            if (mCall == mAct)
                                            {
                                                mActivity.ActivityToCall = null;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    base.DeleteSelection(sel);

                    for (int x = 0; x < lstItemsToRemove.Count; x++)
                    {
                        lstItemsToRemove[x].Remove();
                    }

                    for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Activities.Count; x++)
                    {
                        BaseActivity a = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Activities[x];
                        int linkCount = 0;
                        foreach (CustomLink l in a.Links)
                        {
                            linkCount++;
                        }
                        if (linkCount == 1 && a as CallWorkFlowActivity == null && a as ReturnWorkflowActivity == null)
                        {
                            a.Remove();
                        }
                    }
                }
                this.Document.ComputeBounds();
                if (MainForm.App.m_BrowserView != null)
                {
                    MainForm.App.m_BrowserView.PopulateBrowser();
                }
            }
        }

        public override void DoMouseMove()
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                if (this.Document.CurrentWorkFlow.Location.Y < 40)
                {
                    MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Location = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.expandedLocation;
                }
                else
                {
                    base.DoMouseMove();
                }
            }
        }

        protected override void OnObjectGotSelection(GoSelectionEventArgs evt)
        {
            // check that the event is not null
            // create an argument of the correct type
            //OnYourEvent(evt.GoObject as BaseItem);
            BaseItem BI = evt.GoObject as BaseItem;
            lastSelectedItem = BI;
            if (OnWorkFlowItemSelected != null)
            {
                if (BI != null && MainForm.App.documentIsBeingOpened == false || evt.GoObject.GetType() == typeof(ClapperBoard))
                {
                    if (!MainForm.App.PastingMultipleSelection)
                    {
                        justSelectedObject = true;
                        OnWorkFlowItemSelected(BI);

                        if (this.Selection.Primary != null)
                        {
                            this.ScrollRectangleToVisible(this.Selection.Primary.Bounds);
                        }
                    }
                }
            }
            else
            {
                if (evt.GoObject.Parent.GetType() == typeof(Comment))
                {
                    OnWorkFlowItemSelected(evt.GoObject.Parent as BaseItem);
                    this.ScrollRectangleToVisible(this.Selection.Primary.Bounds);
                }
            }

            if (MainForm.App.m_FindReplaceView != null)
            {
                MainForm.App.m_FindReplaceView.FindText();
            }
        }

        protected override void OnLinkCreated(GoSelectionEventArgs evt)
        {
            GoView myView = MainForm.App.GetCurrentView();
            PointF leftPos = new PointF(0, 0);
            PointF rightPos = new PointF(0, 0);
            MultiPortNodePort holdLeftPort = new MultiPortNodePort();
            MultiPortNodePort holdRightPort = new MultiPortNodePort();

            foreach (Object o in this.Document)
            {
                GoObject g = o as GoObject;
                if (g.GetType() == typeof(CustomLink))
                {
                    CustomLink l = g as CustomLink;

                    if (l.ToPort.Node.GetType() == typeof(ClapperBoard))
                    {
                        MessageBox.Show("A link cannot be created to the Start Process item!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        l.Remove();
                    }
                    if (l.FromPort.Node.GetType() == typeof(ClapperBoard) && l.ToPort.Node != null)
                    {
                        if (l.ToPort.Node.GetType() != typeof(ExternalActivity) && l.ToPort.Node.GetType() != typeof(UserActivity))
                        {
                            MessageBox.Show("A start node can only be linked to a external activity or user activity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            l.Remove();
                        }
                    }
                    if (l.FromPort.Node is BaseActivity && l.ToPort.Node != null)
                    {
                        if (l.ToPort.Node is BaseState)
                        {
                            l.ToArrow = true;
                        }
                    }

                    if (l.FromPort.Node is BaseActivity)
                    {
                        if (l.ToPort.Node is BaseActivity)
                        {
                            MessageBox.Show("A link cannot be created between two activities!", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            l.Remove();
                        }
                    }
                }

                base.OnLinkCreated(evt);
            }
        }

        protected override void OnObjectContextClicked(GoObjectEventArgs evt)
        {
            //
            base.OnObjectContextClicked(evt);

            this.ContextMenu = (ContextMenu)new GoContextMenu(this);
            GoContextMenu cm = this.ContextMenu as GoContextMenu;
            cm.Collapse += new EventHandler(cm_Collapse);

            if (cm.MenuItems.Count > 0)
            {
                cm.MenuItems.Add(new MenuItem("-"));
            }
            cm.MenuItems.Add(new MenuItem("Properties View", new EventHandler(this.Properties_Command)));
            cm.MenuItems.Add(new MenuItem("Code View", new EventHandler(this.CodeView_Command)));
            cm.MenuItems.Add(new MenuItem("Security View", new EventHandler(this.Security_Command)));
            if (evt.GoObject.Parent.GetType() == typeof(ClapperBoard))
            {
                cm.MenuItems.Add(new MenuItem("-"));
                cm.MenuItems.Add(new MenuItem("Delete WorkFlow", new EventHandler(this.DeleteWorkFlow_Command)));
            }

            if (evt.GoObject != null)
            {
                BaseItem BI = null;
                if (evt.GoObject is BaseItem)
                    BI = evt.GoObject as BaseItem;
                else
                    if (evt.GoObject.Parent is BaseItem)
                        BI = evt.GoObject.Parent as BaseItem;

                if (BI != null && BI is IPopupMenu)
                {
                    m_PopupMenuHandler = BI as IPopupMenu;
                    ((IPopupMenu)BI).populateMenu(cm);
                }
                cm.Show(this, evt.ViewPoint);
            }
        }

        protected override void OnBackgroundContextClicked(GoInputEventArgs evt)
        {
            base.OnBackgroundContextClicked(evt);
            // set up the background context menu
            GoContextMenu cm = new GoContextMenu(this);
            MenuItem mItem1 = null;
            MenuItem mItem2 = null;
            if (cm.MenuItems.Count > 0)
                cm.MenuItems.Add(new MenuItem("-"));
            if (holdSelection.Count > 0)
            {
                mItem1 = new MenuItem("Cut", new EventHandler(this.Cut_Command));
                mItem2 = new MenuItem("Copy", new EventHandler(this.Copy_Command));
            }
            MenuItem mItem3 = new MenuItem("Paste", new EventHandler(this.Paste_Command));
            if (clipBoardHasContents == false && MainForm.App.lstMultipleSelectionItems.Count == 0)
            {
                mItem3.Enabled = false;
            }
            else
            {
                mItem3.Enabled = true;
            }
            if (mItem1 != null && mItem2 != null)
            {
                cm.MenuItems.Add(mItem1);
                cm.MenuItems.Add(mItem2);
            }
            cm.MenuItems.Add(mItem3);
            cm.MenuItems.Add(new MenuItem("-"));
            cm.MenuItems.Add(new MenuItem("Properties View", new EventHandler(this.Properties_Command)));
            cm.MenuItems.Add(new MenuItem("Code Viewer", new EventHandler(this.CodeView_Command)));
            cm.MenuItems.Add(new MenuItem("Process Browser", new EventHandler(this.ProcessBrowser_Command)));
            cm.MenuItems.Add(new MenuItem("Security View", new EventHandler(this.Security_Command)));

            cm.Show(this, evt.ViewPoint);
            if (evt.MouseEventArgs.Button == MouseButtons.Right)
            {
                foreach (GoObject o in holdSelection)
                {
                    if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CheckIfItemExists(o as BaseItem))
                    {
                        try
                        {
                            Selection.Add(o);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            else
            {
                Selection.Clear();
                holdSelection.Clear();
            }
        }

        protected override void OnObjectLostSelection(GoSelectionEventArgs evt)
        {
            base.OnObjectLostSelection(evt);
            BaseItem BI = evt.GoObject as BaseItem;
            if (OnWorkFlowItemUnSelected != null && BI != null && MainForm.App.documentIsBeingOpened == false)
            {
                canSelectLastProperty = true;
                OnWorkFlowItemUnSelected(BI);
            }
        }

#warning Here be the FireWFSelected and FireWFUnselected commented out
        //public void FireWorkflowItemSelected(IWorkflowItem Item)
        //{
        //    if (OnWorkFlowItemSelected != null && Item != null)
        //    {
        //        OnWorkFlowItemSelected(Item);
        //    }
        //}

        //public void FireWorkflowItemUnSelected(IWorkflowItem Item)
        //{
        //    if (OnWorkFlowItemUnSelected != null && Item != null)
        //    {
        //        OnWorkFlowItemUnSelected(Item);
        //    }
        //}

        #endregion Overrides

        #region Popup Menu

        private void cm_Collapse(object sender, EventArgs e)
        {
            GoContextMenu cm = sender as GoContextMenu;
            if (m_PopupMenuHandler != null)
            {
                //               m_PopupMenuHandler.OnMenuClosed(cm);
                m_PopupMenuHandler = null;
            }
        }

        private void DrawLink(MouseEventArgs e)
        {
            this.Document.StartTransaction();
            MultiPortNodePort holdLeftPort = new MultiPortNodePort();
            MultiPortNodePort holdRightPort = new MultiPortNodePort();
            PointF leftPos = new PointF();
            PointF rightPos = new PointF();

            CustomLink nLink = new CustomLink();
            CustomLink nLink2 = null;
            if (m_ActiveItem != WorkflowItemType.CallWorkFlowActivity && m_ActiveItem != WorkflowItemType.ReturnWorkFlowActivity)
            {
                nLink2 = new CustomLink();
            }

            BaseActivity mBaseActivity = null;
            MultiPortNodePort mpnp2 = new MultiPortNodePort();
            float holdCurrentZoom = this.DocScale;
            this.ZoomNormal();

            if (holdFromNode != null && holdToNode != null)
            {
                if (holdFromNode is BaseState
                    | holdFromNode.GetType() == typeof(ClapperBoard)
                    && holdToNode.GetType() != typeof(ClapperBoard))
                {
                    if (holdToNode is BaseState)
                    {
                        bool foundPort = false;
                        if (foundPort == false)
                        {
                            MultiPortNodePort mpnp1;
                            if (holdFromNode.GetType() == typeof(ClapperBoard))
                            {
                                mpnp1 = (MultiPortNodePort)holdFromNode.AddPort(1, new SizeF(4, 4), GoObject.NoHandle);
                                mpnp1.Size = new SizeF(30, 30);
                                holdLeftPort = mpnp1;
                                nLink.FromPort = mpnp1;
                            }
                            else
                            {
                                mpnp1 = (MultiPortNodePort)holdFromNode.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                                mpnp1.Size = new SizeF(55, 55);
                                mpnp1.Center = holdFromNode.Location;
                                holdLeftPort = mpnp1;
                                nLink.FromPort = mpnp1;
                            }
                        }
                        leftPos = ConvertDocToView(new Point(Convert.ToInt32(holdLeftPort.Location.X), Convert.ToInt32(holdLeftPort.Location.Y)));

                        int myHandle = GoNode.NoHandle;
                        foundPort = false;

                        if (foundPort == false)
                        {
                            mpnp2 = (MultiPortNodePort)holdToNode.AddPort(1, new SizeF(55, 55), myHandle);
                            mpnp2.Size = new SizeF(55, 55);
                            mpnp2.Center = holdToNode.Location;
                            holdRightPort = mpnp2;
                            if (nLink2 != null)
                            {
                                nLink2.ToPort = mpnp2;
                            }
                        }
                        rightPos = ConvertViewToDoc(new Point(Convert.ToInt32(holdRightPort.Location.X), Convert.ToInt32(holdRightPort.Location.Y)));
                    }

                    bool okToCreateLink = true;
                    if (holdLeftPort.Node != null && holdRightPort.Node != null)
                    {
                        if (holdLeftPort.Node.GetType() == typeof(ClapperBoard) && holdRightPort.Node != null)
                        {
                            if (m_ActiveItem.ToString() != "ExternalActivity" && m_ActiveItem.ToString() != "UserActivity")
                            {
                                MessageBox.Show("A start node can only be linked to an external activity or user activity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                okToCreateLink = false;
                            }
                        }
                    }
                    else
                    {
                        okToCreateLink = false;
                    }
                    if (okToCreateLink == true)
                    {
                        if (holdLeftPort.Node.GetType() == typeof(ArchiveState))
                        {
                            //MessageBox.Show("An activity cannot be created from an archive state!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //okToCreateLink = false;
                        }
                        if (holdRightPort.Node.GetType() == typeof(CommonState) && holdLeftPort.Node.GetType() != typeof(CommonState))
                        {
                            MessageBox.Show("An activity cannot be linked from another state to a common state!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            okToCreateLink = false;
                        }
                    }
                    if (okToCreateLink)
                    {
                        if (leftPos != null && rightPos != null)
                        {
                            double pX;
                            string pXs;

                            double pY;
                            string pYs;
                            int y;
                            int x;

                            if (holdFromNode.Text != holdToNode.Text)
                            {
                                pX = Math.Round(((leftPos.X + rightPos.X) / 2), 0);
                                pXs = pX.ToString();
                                x = int.Parse(pXs);

                                pY = Math.Round((leftPos.Y + rightPos.Y) / 2, 0);
                                pYs = pY.ToString();
                                y = int.Parse(pYs);
                            }
                            else
                            {
                                PointF mPointConv = ConvertViewToDoc(new Point(Convert.ToInt32(e.Location.X), Convert.ToInt32(e.Location.Y)));
                                x = Convert.ToInt32(mPointConv.X - 27);
                                y = Convert.ToInt32(mPointConv.Y - 27);
                            }
                            Point mPoint = new Point(x + 20, y + 20);
                            PointF docPnt = ConvertViewToDoc(mPoint);

                            Point unconvertedLocation = new Point(Convert.ToInt32(docPnt.X), Convert.ToInt32(docPnt.Y));
                            PointF newActivityPosition = ConvertDocToView(unconvertedLocation);
                            newActivityPosition.X = newActivityPosition.X;
                            newActivityPosition.Y = newActivityPosition.Y;
                            Console.WriteLine(newActivityPosition.ToString());
                            UserActivity mUserActivity = null; ;
                            TimedActivity mTimedActivity = null;
                            ExternalActivity mExternalActivity = null;
                            ConditionalActivity mConditionalActivity = null;
                            CallWorkFlowActivity mWorkFlowActivity = null;
                            ReturnWorkflowActivity mReturnWorkFlowActivity = null;

                            switch (m_ActiveItem)
                            {
                                case WorkflowItemType.UserActivity:
                                    mUserActivity = m_Document.CurrentWorkFlow.CreateUserActivity(newActivityPosition, holdFromNode);
                                    MultiPortNodePort mpnp10;
                                    mpnp10 = (MultiPortNodePort)mUserActivity.AddPort(1, new SizeF(45, 45), GoObject.NoHandle);
                                    mpnp10.Size = new SizeF(45, 45);
                                    mpnp10.Center = mUserActivity.Location;
                                    mBaseActivity = mUserActivity as BaseActivity;
                                    break;
                                case WorkflowItemType.TimedActivity:
                                    mTimedActivity = m_Document.CurrentWorkFlow.CreateTimedActivity(newActivityPosition, holdFromNode);
                                    MultiPortNodePort mpnp13;
                                    mpnp13 = (MultiPortNodePort)mTimedActivity.AddPort(1, new SizeF(0, 2), GoObject.NoHandle);
                                    mpnp13.Size = new SizeF(45, 45);
                                    mpnp13.Center = mTimedActivity.Location;

                                    mBaseActivity = mTimedActivity as BaseActivity;
                                    break;
                                case WorkflowItemType.ExternalActivity:
                                    mExternalActivity = m_Document.CurrentWorkFlow.CreateExternalActivity(newActivityPosition, holdFromNode);
                                    MultiPortNodePort mpnp14;
                                    mpnp14 = (MultiPortNodePort)mExternalActivity.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                                    mpnp14.Size = new SizeF(45, 45);
                                    mpnp14.Center = mExternalActivity.Location;

                                    mBaseActivity = mExternalActivity as BaseActivity;
                                    break;
                                case WorkflowItemType.ConditionalActivity:
                                    mConditionalActivity = m_Document.CurrentWorkFlow.CreateConditionalActivity(newActivityPosition, holdFromNode);
                                    MultiPortNodePort mpnp15;
                                    mpnp15 = (MultiPortNodePort)mConditionalActivity.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                                    mpnp15.Size = new SizeF(45, 45);
                                    mpnp15.Center = mConditionalActivity.Location;

                                    mBaseActivity = mConditionalActivity as BaseActivity;
                                    break;
                                case WorkflowItemType.CallWorkFlowActivity:
                                    mWorkFlowActivity = m_Document.CurrentWorkFlow.CreateCallWorkFlowActivity(newActivityPosition, holdFromNode);
                                    MultiPortNodePort mpnp16;
                                    mpnp16 = (MultiPortNodePort)mWorkFlowActivity.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                                    mpnp16.Size = new SizeF(45, 45);
                                    mpnp16.Center = mWorkFlowActivity.Location;

                                    mBaseActivity = mWorkFlowActivity as BaseActivity;
                                    break;
                                case WorkflowItemType.ReturnWorkFlowActivity:
                                    mReturnWorkFlowActivity = m_Document.CurrentWorkFlow.CreateReturnWorkFlowActivity(newActivityPosition, holdFromNode);
                                    MultiPortNodePort mpnp17;
                                    mpnp17 = (MultiPortNodePort)mReturnWorkFlowActivity.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                                    mpnp17.Size = new SizeF(45, 45);
                                    mpnp17.Center = mReturnWorkFlowActivity.Location;

                                    mBaseActivity = mReturnWorkFlowActivity as BaseActivity;
                                    break;
                            }
                            if (holdFromNode.Text == holdToNode.Text)
                            {
                                holdToNode = holdFromNode;
                            }

                            if (mBaseActivity == null)
                            {
                                this.Document.FinishTransaction("Draw Link");
                                return;
                            }

                            foreach (MultiPortNodePort p in mBaseActivity.Ports)
                            {
                                mBaseActivity.Port = p;
                                break;
                            }

                            GoPort toPortToUse = null;
                            foreach (GoPort p in mBaseActivity.Ports)
                            {
                                toPortToUse = p;
                                p.PortObject = p;
                                break;
                            }
                            nLink.ToPort = toPortToUse;

                            if (nLink2 != null)
                            {
                                nLink2.FromPort = mBaseActivity.Port;
                            }

                            this.Document.CurrentWorkFlow.InsertBefore(null, nLink);

                            if (nLink2 != null)
                            {
                                nLink2.ToArrow = true;
                            }
                            if (nLink2 != null)
                            {
                                this.Document.CurrentWorkFlow.InsertBefore(null, nLink2);
                            }
                            if (nLink2 != null)
                            {
                                if (nLink.FromNode == nLink2.ToNode)
                                {
                                    nLink.ToArrow = true;
                                }
                            }
                            else
                            {
                                if (nLink.ToNode.GetType() != typeof(ReturnWorkflowActivity))
                                {
                                    nLink.ToArrow = true;
                                }
                            }
                            if (nLink.ToNode.GetType() == typeof(ReturnWorkflowActivity))
                            {
                                nLink.FromArrow = true;
                            }
                        }
                    }
                }
                holdFromNode = null;
                holdToNode = null;
                AllowLink = false;
                this.Document.FinishTransaction("Draw Link");
                MainForm.App.unCheckToolStripButtons();
            }

            if (holdCurrentZoom != 1)
            {
                this.ZoomToScale(holdCurrentZoom * 100);
            }
        }

        public void Cut_Command(Object sender, EventArgs e)
        {
            if (TestIfCanCut())
            {
                if (CopySelection())
                {
                    if (MainForm.App.m_CodeView != null)
                    {
                        MainForm.App.m_CodeView.toolStripComboBoxCodeSection.SelectedIndex = -1;
                    }
                    foreach (GoObject o in Selection)
                    {
                        o.Visible = false;
                    }
                    holdSelection.RemoveAllSelectionHandles();
                    for (int x = 0; x < MainForm.App.lstMultipleSelectionItems.Count; x++)
                    {
                        MainForm.App.lstMultipleSelectionItems[x].OperationType = CutCopyOperationType.Cut;
                    }
                    for (int x = 0; x < MainForm.App.lstMultipleSelectionLinks.Count; x++)
                    {
                        MainForm.App.lstMultipleSelectionLinks[x].OperationType = CutCopyOperationType.Cut;
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid Selection - cannot continue! \n Please ensure you select the from and to states for all activities.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                holdSelection.RemoveAllSelectionHandles();
                holdSelection.Clear();
                Selection.RemoveAllSelectionHandles();
                Selection.Clear();
                MainForm.App.lstMultipleSelectionItems.Clear();
                MainForm.App.lstMultipleSelectionLinks.Clear();
                this.Document.UndoManager = null;
                this.Document.UndoManager = new GoUndoManager();
            }
        }

        private bool TestIfCanCut()
        {
            foreach (GoObject o in Selection)
            {
                if (o is BaseState)
                {
                    BaseState b = o as BaseState;
                    foreach (CustomLink l in b.Links)
                    {
                        if (l.FromNode != o)
                        {
                            if (!Selection.Contains(l.FromNode as GoObject))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!Selection.Contains(l.ToNode as GoObject))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void Copy_Command(Object sender, EventArgs e)
        {
            CopySelection();
            for (int x = 0; x < MainForm.App.lstMultipleSelectionItems.Count; x++)
            {
                MainForm.App.lstMultipleSelectionItems[x].OperationType = CutCopyOperationType.Copy;
            }
            for (int x = 0; x < MainForm.App.lstMultipleSelectionLinks.Count; x++)
            {
                MainForm.App.lstMultipleSelectionLinks[x].OperationType = CutCopyOperationType.Copy;
            }
        }

        private bool CopySelection()
        {
            this.Document.StartTransaction();
            MainForm.App.lstMultipleSelectionItems.Clear();
            MainForm.App.lstMultipleSelectionLinks.Clear();
            foreach (GoObject o in Selection)
            {
                if (o is BaseItem)
                {
                    BaseItem i = o as BaseItem;
                    SelectionItem mItem = new SelectionItem();
                    mItem.baseItem = i;
                    mItem.OriginalName = i.Name;
                    mItem.OriginalWorkflow = this.Document.CurrentWorkFlow;
                    mItem.NewLocation = i.Location;
                    MainForm.App.lstMultipleSelectionItems.Add(mItem);
                }
            }
            if (MainForm.App.lstMultipleSelectionItems.Count == 1)
            {
                if (MainForm.App.lstMultipleSelectionItems[0].baseItem is BaseActivity)
                {
                    if (MainForm.App.lstMultipleSelectionLinks.Count < 1)
                    {
                        MessageBox.Show("Invalid Selection - Cannot copy an activity without its links!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.Selection.RemoveAllSelectionHandles();
                        MainForm.App.lstMultipleSelectionItems.Clear();
                        MainForm.App.lstMultipleSelectionLinks.Clear();
                        holdSelection.Clear();
                        this.Document.FinishTransaction("Copy Selection");
                        return false;
                    }
                }
            }
            if (MainForm.App.lstMultipleSelectionItems.Count > 1)
            {
                for (int x = 1; x < MainForm.App.lstMultipleSelectionItems.Count; x++)
                {
                    BaseItem i = MainForm.App.lstMultipleSelectionItems[x].baseItem;
                    float X1 = MainForm.App.lstMultipleSelectionItems[0].baseItem.Location.X;
                    float Y1 = MainForm.App.lstMultipleSelectionItems[0].baseItem.Location.Y;

                    float X = i.Location.X - X1;
                    float Y = i.Location.Y - Y1;

                    MainForm.App.lstMultipleSelectionItems[x].NewLocation = new PointF(X, Y);
                }

                MainForm.App.lstMultipleSelectionItems[0].NewLocation = MainForm.App.lstMultipleSelectionItems[0].baseItem.Location;

                for (int x = 0; x < MainForm.App.lstMultipleSelectionItems.Count; x++)
                {
                    int linkCount = 0;
                    BaseItem i = MainForm.App.lstMultipleSelectionItems[x].baseItem;

                    foreach (CustomLink l in i.Links)
                    {
                        linkCount++;
                        bool ContainsLink = false;
                        for (int y = 0; y < MainForm.App.lstMultipleSelectionLinks.Count; y++)
                        {
                            if (MainForm.App.lstMultipleSelectionLinks[y].customLink == l)
                            {
                                ContainsLink = true;
                                break;
                            }
                        }
                        if (!ContainsLink)
                        {
                            bool foundFromNode = false;
                            bool foundToNode = false;
                            for (int y = 0; y < MainForm.App.lstMultipleSelectionItems.Count; y++)
                            {
                                if (MainForm.App.lstMultipleSelectionItems[y].baseItem == l.FromNode || l.FromNode is ClapperBoard)
                                {
                                    foundFromNode = true;
                                }

                                if (MainForm.App.lstMultipleSelectionItems[y].baseItem == l.ToNode)
                                {
                                    foundToNode = true;
                                }
                            }

                            if (foundFromNode && foundToNode && l.FromNode is CallWorkFlowActivity == false)
                            {
                                CustomLinkItem mItem = new CustomLinkItem();
                                mItem.customLink = l;
                                MainForm.App.lstMultipleSelectionLinks.Add(mItem);
                            }
                        }
                    }
                }

                List<BaseActivity> lstActivitiesToBeRemoved = new List<BaseActivity>();
                List<CustomLink> lstLinksToBeRemoved = new List<CustomLink>();
                for (int x = 0; x < MainForm.App.lstMultipleSelectionItems.Count; x++)
                {
                    BaseActivity a = MainForm.App.lstMultipleSelectionItems[x].baseItem as BaseActivity;
                    if (a != null)
                    {
                        foreach (CustomLink l in a.Links)
                        {
                            bool foundFromNode = false;
                            bool foundToNode = false;
                            for (int y = 0; y < MainForm.App.lstMultipleSelectionItems.Count; y++)
                            {
                                if (MainForm.App.lstMultipleSelectionItems[y].baseItem == l.FromNode || l.FromNode is ClapperBoard)
                                {
                                    foundFromNode = true;
                                }

                                if (MainForm.App.lstMultipleSelectionItems[y].baseItem == l.ToNode)
                                {
                                    foundToNode = true;
                                }
                            }
                            if (!foundFromNode || !foundToNode)
                            {
                                MessageBox.Show("Invalid Selection - cannot continue! \n Please ensure you select the from and to states for all activities.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                this.Selection.RemoveAllSelectionHandles();
                                MainForm.App.lstMultipleSelectionItems.Clear();
                                MainForm.App.lstMultipleSelectionLinks.Clear();
                                holdSelection.Clear();
                                this.Document.FinishTransaction("Copy Selection");
                                return false;
                            }
                        }
                    }
                }
            }

            this.Selection.Clear();
            holdSelection.RemoveAllSelectionHandles();
            for (int x = 0; x < MainForm.App.lstMultipleSelectionItems.Count; x++)
            {
                this.Selection.Add(MainForm.App.lstMultipleSelectionItems[x].baseItem);
            }
            for (int x = 0; x < MainForm.App.lstMultipleSelectionLinks.Count; x++)
            {
                try
                {
                    this.Selection.Add(MainForm.App.lstMultipleSelectionLinks[x].customLink);
                }
                catch
                {
                }
            }
            this.Document.FinishTransaction("Copy Selection");
            return true;
        }

        public void Paste_Command(Object sender, EventArgs e)
        {
            MainForm.App.PasteSelection();
        }

        private void DeleteWorkFlow_Command(object sender, EventArgs e)
        {
            if (m_Document.WorkFlows.GetUpperBound(0) > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this workflow ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    m_Document.CurrentWorkFlow.Clear();
                    this.Selection.RemoveAllSelectionHandles();
                    this.holdSelection.RemoveAllSelectionHandles();
                    this.Selection.Clear();
                    this.holdSelection.Clear();

                    this.Document.DeleteWorkFlow(this.Document.CurrentWorkFlow);

                    for (int x = 0; x < MainForm.App.GetCurrentView().Document.WorkFlows.Length; x++)
                    {
                        if (x == 0)
                        {
                            this.Document.SelectWorkFlow(MainForm.App.GetCurrentView().Document.WorkFlows[x]);
                        }
                        foreach (BaseActivity i in MainForm.App.GetCurrentView().Document.WorkFlows[x].Activities)
                        {
                            if (i.GetType() == typeof(CallWorkFlowActivity))
                            {
                                CallWorkFlowActivity cw = i as CallWorkFlowActivity;
                                if (cw.WorkFlowToCall == MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                                {
                                    cw.ActivityToCall = null;
                                    cw.WorkFlowToCall = null;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Cannot delete the only workflow in the document!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ProcessBrowser_Command(Object sender, EventArgs e)
        {
            if (MainForm.App.mnuProcessBrowser.Checked == false)
            {
                MainForm.App.mnuProcessBrowser.Checked = true;
            }
            else
            {
                MainForm.App.mnuProcessBrowser.Checked = false;
            }

            MainForm.App.ShowProcessBrowserWindow();
        }

        private void Properties_Command(Object sender, EventArgs e)
        {
            if (MainForm.App.mnuProperties.Checked == false)
            {
                MainForm.App.mnuProperties.Checked = true;
            }
            else
            {
                MainForm.App.mnuProperties.Checked = false;
            }
            MainForm.App.ShowPropertiesWindow();
        }

        private void Security_Command(Object sender, EventArgs e)
        {
            MainForm.App.ShowSecurityWindow();
        }

        private void CodeView_Command(Object sender, EventArgs e)
        {
            if (MainForm.App.mnuItemCodeView.Checked == false)
            {
                MainForm.App.mnuItemCodeView.Checked = true;
            }
            else
            {
                MainForm.App.mnuItemCodeView.Checked = false;
            }
            MainForm.App.ShowCodeViewWindow(null, false);
        }

        #endregion Popup Menu

        #region Zoom Functions

        public virtual void ZoomIn()
        {
            myOriginalScale = true;
            float newscale = (float)(Math.Round(this.DocScale / 0.9f * 100) / 100);
            this.DocScale = newscale;
            if (this.Selection.Primary != null)
            {
                this.ScrollRectangleToVisible(this.Selection.Primary.Bounds);
            }
        }

        public virtual void ZoomOut()
        {
            myOriginalScale = true;
            float newscale = (float)(Math.Round(this.DocScale * 0.9f * 100) / 100);
            this.DocScale = newscale;
            if (this.Selection.Primary != null)
            {
                this.ScrollRectangleToVisible(this.Selection.Primary.Bounds);
            }
        }

        public virtual void ZoomNormal()
        {
            myOriginalScale = true;
            this.DocScale = 1;
            if (this.Selection.Primary != null)
            {
                this.ScrollRectangleToVisible(this.Selection.Primary.Bounds);
            }
        }

        public virtual void ZoomToFit()
        {
            if (myOriginalScale)
            {
                myOriginalDocPosition = this.DocPosition;
                myOriginalDocScale = this.DocScale;
                RescaleToFit();
            }
            else
            {
                this.DocPosition = myOriginalDocPosition;
                this.DocScale = myOriginalDocScale;
                myOriginalDocPosition = this.DocPosition;
                myOriginalDocScale = this.DocScale;
                RescaleToFit();
            }
            myOriginalScale = !myOriginalScale;
        }

        public virtual void ZoomToScale(float scale)
        {
            if (myOriginalScale)
            {
                //               ZoomToFit();
                myOriginalDocPosition = this.DocPosition;
                myOriginalDocScale = this.DocScale;
                RescaleToFit();
                this.DocScale = myOriginalDocScale / 100 * scale;
                if (this.Selection.Primary != null)
                {
                    this.ScrollRectangleToVisible(this.Selection.Primary.Bounds);
                }
            }
            else
            {
                ZoomToFit();
                this.DocPosition = myOriginalDocPosition;
                this.DocScale = myOriginalDocScale / 100 * scale;
            }
            myOriginalScale = !myOriginalScale;
        }

        #endregion Zoom Functions

        #region ICutCopyPasteTarget Members

        public void DoCut()
        {
            holdSelection.Clear();
            foreach (GoObject o in Selection)
            {
                holdSelection.Add(o);
            }
            this.Cut_Command(this, null);
        }

        public void DoCopy()
        {
            holdSelection.Clear();
            foreach (GoObject o in Selection)
            {
                holdSelection.Add(o);
            }
            Copy_Command(this, null);
        }

        public void DoPaste()
        {
            MainForm.App.PasteSelection();
        }

        public void DoDelete()
        {
            this.DeleteSelectionWithTransaction(this.Selection);
        }

        public void DoUndo()
        {
            if (this.Document.UndoManager != null)
            {
                try
                {
                    this.Undo();
                }
                catch
                {
                }
            }
            else
            {
                this.Document.UndoManager = new GoUndoManager();
            }
            if (MainForm.App.m_BrowserView != null)
            {
                MainForm.App.m_BrowserView.PopulateBrowser();
            }
        }

        public void DoRedo()
        {
            this.Redo();
            if (MainForm.App.m_BrowserView != null)
            {
                MainForm.App.m_BrowserView.PopulateBrowser();
            }
        }

        public void DoSelectAll()
        {
            if (m_Document.CurrentWorkFlow != null)
            {
                m_Document.CurrentWorkFlow.SelectAll();
            }
        }

        public void DoPrint()
        {
            this.Print();
        }

        #endregion ICutCopyPasteTarget Members

        [DllImport("user32.dll")]
        private static extern bool PostMessage(

            IntPtr hWnd, // handle to destination window
            UInt32 Msg, // message
            Int32 wParam, // first message parameter
            Int32 lParam // second message parameter

        );

        [DllImport("user32.dll")]
        private static extern IntPtr GetFocus();

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x100)
            {
                m_CurrentSC = (int)m.LParam;
                m_CurrentVK = (int)m.WParam;
            }

            base.WndProc(ref m);
        }
    }

    public class ActivityLinkItem
    {
        BaseActivity m_Activity;
        BaseState m_FromNode;
        BaseState m_ToNode;

        public BaseActivity Activity
        {
            get
            {
                return m_Activity;
            }
            set
            {
                m_Activity = value;
            }
        }

        public BaseState FromNode
        {
            get
            {
                return m_FromNode;
            }
            set
            {
                m_FromNode = value;
            }
        }

        public BaseState ToNode
        {
            get
            {
                return m_ToNode;
            }
            set
            {
                m_ToNode = value;
            }
        }
    }
}