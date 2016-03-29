using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Northwoods.Go;
using SAHL.X2Designer.Items;
using SAHL.X2Designer.Misc;
using SAHL.X2Designer.Views;

namespace SAHL.X2Designer.Documents
{
    [Serializable]
    public class WorkFlow : GoSubGraph
    {
        public static int WorkFlowLeft = 20;
        public static int WorkFlowExpandedTop = 70;
        public static int WorkFlowCollapsedTop = 10;

        private List<BaseState> m_States;
        private List<BaseActivity> m_Activities;
        private List<Comment> m_Comments;

        public int lastCommentID = 1;
        private int _GenericKeyTypeKey = -1;
        private string _GenericKeyType = String.Empty;

        public string GenericKeyType { get { return _GenericKeyType; } set { _GenericKeyType = value; } }

        public int GenericKeyTypeKey { get { return _GenericKeyTypeKey; } set { _GenericKeyTypeKey = value; } }

        private List<ExternalActivityItem> m_ExternalActivityCollection;
        private List<CustomVariableItem> m_CustomVariablesCollection;
        private List<CustomFormItem> m_CustomFormsCollection;
        private PointF m_CollapsedLocation;
        private static PointF m_ExpandedLocation = new PointF(WorkFlowLeft, WorkFlowExpandedTop);

        public Dictionary<IWorkflowItem, TreeNode> hashTableVar;
        public Dictionary<IWorkflowItem, TreeNode> hashTableForm;
        public Dictionary<IWorkflowItem, TreeNode> hashTableRoles;

        private string holdFullWorkFlowName = "";

        public List<RolesCollectionItem> m_WorkFlowRoles;

        public WorkFlow(string Name, PointF collapsedLocation)
        {
            m_Activities = new List<BaseActivity>();
            m_States = new List<BaseState>();
            m_Comments = new List<Comment>();
            m_ExternalActivityCollection = new List<ExternalActivityItem>();
            m_CustomVariablesCollection = new List<CustomVariableItem>();
            m_CustomFormsCollection = new List<CustomFormItem>();
            m_CollapsedLocation = collapsedLocation;

            this.LabelSpot = GoObject.TopLeft;  // instead of centered at top
            this.Opacity = 80;  // no background color
            this.BackgroundColor = Color.Cornsilk;
            this.ComputeCollapsedSize(true);
            this.TopLeftMargin = new SizeF(0, 0);  // instead of default 4x4
            this.BottomRightMargin = new SizeF(0, 0);  // instead of default 4x4
            this.BorderPen = Pens.Cornsilk;  // instead of none by default
            this.Text = Name;

            hashTableForm = new Dictionary<IWorkflowItem, TreeNode>();
            hashTableVar = new Dictionary<IWorkflowItem, TreeNode>();
            hashTableRoles = new Dictionary<IWorkflowItem, TreeNode>();

            m_WorkFlowRoles = new List<RolesCollectionItem>();

            this.Location = m_ExpandedLocation;

            //            this.Expand();
        }

        #region Properties

        public override void Collapse()
        {
            base.Collapse();
            if (this.WorkFlowName.Length == 0)
            {
                return;
            }
            this.Location = collapsedLocation; ;

            if (this.Label != null)
            {
                if (this.Label.Text.Contains("...") == false)
                    holdFullWorkFlowName = this.Label.Text;
                if (this.Label.Text.Length > 23)
                {
                    this.Label.Text = this.Label.Text.Substring(0, 20) + "...";
                }
            }
            this.Label.Width = 125;
            if (this.View != null)
            {
                this.View.Refresh();
            }
        }

        public override void Expand()
        {
            this.Location = expandedLocation;

            base.Expand();
            if (this.WorkFlowName.Length == 0)
            {
                return;
            }

            if (holdFullWorkFlowName.Length > 0)
            {
                this.Label.Text = holdFullWorkFlowName;
            }
            if (this.View != null)
            {
                this.View.Refresh();
            }
        }

        public string WorkFlowName
        {
            get { return Text; }
            set
            {
                // first change the workflow name in all available code sections
                ProcessView V = MainForm.App.GetCurrentView();
                if (V != null)
                {
                    ProcessDocument Doc = V.Document;
                    foreach (BaseItem BI in Doc.CurrentWorkFlow.Nodes)
                    {
                        BI.UpdateCodeSectionData(base.Text, value);
                    }
                }

                Text = value;
            }
        }

        public PointF collapsedLocation
        {
            get
            {
                return m_CollapsedLocation;
            }
            set
            {
                m_CollapsedLocation = value;
            }
        }

        public PointF expandedLocation
        {
            get
            {
                return m_ExpandedLocation;
            }
        }

        public ProcessDocument Process
        {
            get
            {
                return this.Document as ProcessDocument;
            }
        }

        public List<BaseState> States
        {
            get { return m_States; }
        }

        public List<BaseActivity> Activities
        {
            get { return m_Activities; }
        }

        public List<ExternalActivityItem> ExternalActivityCollection
        {
            get { return m_ExternalActivityCollection; }
        }

        public List<CustomVariableItem> CustomVariables
        {
            get { return m_CustomVariablesCollection; }
        }

        public List<CustomFormItem> Forms
        {
            get
            {
                m_CustomFormsCollection.Sort(new CustomFormComparer());
                return m_CustomFormsCollection;
            }
        }

        public List<RolesCollectionItem> WorkFlowRoles
        {
            get
            {
                m_WorkFlowRoles.Sort(new RolesComparer());
                return m_WorkFlowRoles;
            }
        }

        #endregion Properties

        #region Overrides

        protected override GoText CreateLabel()
        {
            return new WorkFlowLabel("");
        }

        public override bool OnHover(GoInputEventArgs evt, GoView view)
        {
            Point screenPnt = new Point(evt.ViewPoint.X, evt.ViewPoint.Y);
            PointF docPnt = MainForm.App.GetCurrentView().ConvertViewToDoc(screenPnt);

            foreach (GoObject o in MainForm.App.GetCurrentView().Document.PickObjects(docPnt, false, null, 10))
                if (o.Parent.GetType() == typeof(Comment))
                {
                    Comment i = o.Parent as Comment;
                    i.ToolTipText = i.CommentText;
                }
            return base.OnHover(evt, view);
        }

        protected override GoPort CreatePort()
        {
            GoPort p = new GoPort();
            p.IsValidFrom = false;  // users can only draw a new link "to" this port
            p.IsValidTo = false;
            p.Style = GoPortStyle.None;  // not rendered
            p.FromSpot = NoSpot;
            p.ToSpot = NoSpot;
            p.Selectable = false;
            p.Movable = false;
            p.Reshapable = false;
            p.Resizable = false;
            return p;
        }

        protected override void FinishExpand(PointF hpos)
        {
            base.FinishExpand(hpos);

            if (this.Document != null)
                ((ProcessDocument)this.Document).SelectWorkFlow(this);
        }

        public override bool CanSelect()
        {
            return false;
        }

        public override bool CanMove()
        {
            return false;
        }

        public override bool CanReshape()
        {
            return false;
        }

        public override bool CanResize()
        {
            return false;
        }

        protected override void OnBoundsChanged(RectangleF old)
        {
            base.OnBoundsChanged(old);
            foreach (GoObject o in this)
            {
                if (o is GoBasicNode && o.Size == new SizeF(5, 5) && MainForm.App.GetCurrentView() != null)
                {
                    GoBasicNode mNode = o as GoBasicNode;
                    PointF NodeLoc = new PointF(25, 25);
                    int saveX = Convert.ToInt32(NodeLoc.X);
                    int saveY = Convert.ToInt32(NodeLoc.Y);
                    Point LoadPoint = MainForm.App.GetCurrentView().ConvertDocToView(new Point(saveX, saveY));
                    o.Location = this.Location;
                    o.Visible = false;
                    break;
                }
            }
        }

        #endregion Overrides

        #region Create WorkFlowItems

        public BaseState CreateSystemDecisionState(PointF Location)
        {
            this.Document.StartTransaction();
            SystemDecisionState n = new SystemDecisionState();
            n.Initialize(null, "", "");
            n.Image.Size = new System.Drawing.Size(32, 32);

            this.Add(n);
            //m_States.Add(n);
            n.Name = GetNextStateName();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                n.Position = new Point(Convert.ToInt32(Location.X) - 16, Convert.ToInt32(Location.Y) - 16);
            }
            else
            {
                n.Position = Location;
            }

            MainForm.App.unCheckToolStripButtons();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                MainForm.App.GetCurrentView().Selection.Clear();
                MainForm.App.GetCurrentView().Selection.Add(n);
            }
            this.Document.FinishTransaction("Create System Decision");

            return n;
        }

        public BaseState CreateCommonState(PointF Location)
        {
            this.Document.StartTransaction();
            CommonState n = new CommonState();
            n.Initialize(null, "", "");
            n.Image.Size = new System.Drawing.Size(32, 32);
            this.Add(n);
            //m_States.Add(n);
            n.Name = GetNextStateName();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                n.Position = new Point(Convert.ToInt32(Location.X) - 16, Convert.ToInt32(Location.Y) - 16);
            }
            else
            {
                n.Position = Location;
            }

            MainForm.App.unCheckToolStripButtons();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                MainForm.App.GetCurrentView().Selection.Clear();
                MainForm.App.GetCurrentView().Selection.Add(n);
            }
            this.Document.FinishTransaction("Create Common State");
            return n;
        }

        public BaseItem CreateClapperBoard(PointF Position)
        {
            this.Document.StartTransaction();
            ClapperBoard n = new ClapperBoard();
            n.Initialize(null, "", "");
            n.Image.Size = new System.Drawing.Size(32, 32);
            n.Selectable = true;
            n.Deletable = false;
            foreach (MultiPortNodePort p in n.Ports)
            {
                p.Remove();
                break;
            }
            MultiPortNodePort mPort = (MultiPortNodePort)n.AddPort(1, new SizeF(0, 0), GoObject.NoHandle);
            mPort.Size = new SizeF(40, 40);

            this.Add(n);

            n.Position = new PointF(Position.X, Position.Y);

            this.Document.FinishTransaction("Create Clapperboard");
            return n;
        }

        public BaseState CreateUserState(PointF Position)
        {
            this.Document.StartTransaction();
            UserState n = new UserState();
            n.Initialize(null, "", "");
            n.Image.Size = new System.Drawing.Size(32, 32);
            this.Add(n);

            //m_States.Add(n);
            n.Name = GetNextStateName();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                n.Position = new Point(Convert.ToInt32(Position.X) - 16, Convert.ToInt32(Position.Y) - 16);
            }
            else
            {
                n.Position = new PointF(Position.X, Position.Y);
            }
            MainForm.App.unCheckToolStripButtons();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                MainForm.App.GetCurrentView().Selection.Clear();
                MainForm.App.GetCurrentView().Selection.Add(n);
            }
            this.Document.FinishTransaction("create user state");
            return n;
        }

        public BaseState CreateHoldState(PointF Position)
        {
            this.Document.StartTransaction();
            HoldState n = new HoldState();
            n.Initialize(null, "", "");
            n.Image.Size = new System.Drawing.Size(32, 32);
            this.Add(n);

            //m_States.Add(n);
            n.Name = GetNextStateName();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                n.Position = new Point(Convert.ToInt32(Position.X) - 16, Convert.ToInt32(Position.Y) - 16);
            }
            else
            {
                n.Position = new PointF(Position.X, Position.Y);
            }
            MainForm.App.unCheckToolStripButtons();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                MainForm.App.GetCurrentView().Selection.Clear();
                MainForm.App.GetCurrentView().Selection.Add(n);
            }
            this.Document.FinishTransaction("create hold state");
            return n;
        }

        public BaseState CreateSystemState(PointF Location)
        {
            this.Document.StartTransaction();
            SystemState n = new SystemState();
            n.Initialize(null, "", "");
            n.Image.Size = new System.Drawing.Size(32, 32);

            this.Add(n);
            //m_States.Add(n);
            n.Name = GetNextStateName();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                n.Position = new Point(Convert.ToInt32(Location.X) - 16, Convert.ToInt32(Location.Y) - 16);
            }
            else
            {
                n.Position = new PointF(Location.X, Location.Y);
            }
            MainForm.App.unCheckToolStripButtons();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                MainForm.App.GetCurrentView().Selection.Clear();
                MainForm.App.GetCurrentView().Selection.Add(n);
            }
            this.Document.FinishTransaction("Create System State");
            return n;
        }

        public UserActivity CreateUserActivity(PointF Position, GoNode fromNode)
        {
            UserActivity n = new UserActivity(GetNextActivityId());
            n.Initialize(null, "", "");
            n.Priority = getNextActivityPriority(n, fromNode);
            n.Image.Size = new System.Drawing.Size(24, 24);
            this.Add(n);
            //m_Activities.Add(n);
            n.Name = GetNextActivityName();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                n.Position = new Point(Convert.ToInt32(Position.X) - 12, Convert.ToInt32(Position.Y) - 12);
            }
            else
            {
                n.Position = Position;
            }
            MainForm.App.unCheckToolStripButtons();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                MainForm.App.GetCurrentView().Selection.Clear();
                MainForm.App.GetCurrentView().Selection.Add(n);
            }
            foreach (RoleInstance i in n.Access)
            {
                if (fromNode != null)
                {
                    if (i.RoleItem.Name.ToLower() == "worklist" && fromNode.GetType() != typeof(ClapperBoard) && MainForm.App.documentIsBeingOpened == false)
                    {
                        i.IsChecked = true;
                        break;
                    }
                }
                else
                {
                    if (i.RoleItem.Name.ToLower() == "worklist" && MainForm.App.documentIsBeingOpened == false)
                    {
                        i.IsChecked = true;
                        break;
                    }
                }
            }
            return n;
        }

        public BaseItem CreateInvisibleAnchorNode(PointF Position)
        {
            this.Document.StartTransaction();
            InvisibleAnchorNode n = new InvisibleAnchorNode();
            n.Initialize(null, "", "      ");
            n.Image.Size = new System.Drawing.Size(0, 0);
            n.Selectable = true;
            n.Deletable = false;
            n.Visible = true;
            n.Label.Visible = true;
            n.Image.Visible = false;
            foreach (MultiPortNodePort p in n.Ports)
            {
                p.Remove();
                break;
            }
            n.Position = new PointF(Position.X, Position.Y);
            this.Add(n);
            this.Document.FinishTransaction("Create Invisible Anchor Node");
            return n;
        }

        public ExternalActivity CreateExternalActivity(PointF Location, GoNode fromNode)
        {
            ExternalActivity n = new ExternalActivity(GetNextActivityId());
            n.Initialize(null, "", "");
            n.Priority = getNextActivityPriority(n, fromNode);
            n.Image.Size = new System.Drawing.Size(24, 24);
            this.Add(n);
            //m_Activities.Add(n);
            n.Name = GetNextActivityName();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                n.Position = new Point(Convert.ToInt32(Location.X) - 12, Convert.ToInt32(Location.Y) - 12);
            }
            else
            {
                n.Position = Location;
            }
            MainForm.App.unCheckToolStripButtons();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                MainForm.App.GetCurrentView().Selection.Clear();
                MainForm.App.GetCurrentView().Selection.Add(n);
            }
            return n;
        }

        public BaseState CreateArchiveState(PointF Location)
        {
            this.Document.StartTransaction();
            ArchiveState n = new ArchiveState(this);
            n.Initialize(null, "", "");
            n.Image.Size = new System.Drawing.Size(32, 32);

            this.Add(n);
            //m_States.Add(n);
            n.Name = GetNextStateName();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                n.Position = new Point(Convert.ToInt32(Location.X) - 16, Convert.ToInt32(Location.Y) - 16);
            }
            else
            {
                n.Position = Location;
            }
            MainForm.App.unCheckToolStripButtons();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                MainForm.App.GetCurrentView().Selection.Clear();
                MainForm.App.GetCurrentView().Selection.Add(n);
            }
            this.Document.FinishTransaction("Create Archive State");
            return n;
        }

        public TimedActivity CreateTimedActivity(PointF Location, GoNode fromNode)
        {
            TimedActivity n = new TimedActivity("");
            n.Initialize(null, "", "");
            n.Priority = getNextActivityPriority(n, fromNode);
            n.Image.Size = new System.Drawing.Size(24, 24);
            this.Add(n);
            //m_Activities.Add(n);
            n.Name = GetNextActivityName();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                n.Position = new Point(Convert.ToInt32(Location.X) - 12, Convert.ToInt32(Location.Y) - 12);
            }
            else
            {
                n.Position = Location;
            }
            MainForm.App.unCheckToolStripButtons();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                MainForm.App.GetCurrentView().Selection.Clear();
                MainForm.App.GetCurrentView().Selection.Add(n);
            }
            return n;
        }

        public ConditionalActivity CreateConditionalActivity(PointF Location, GoNode fromNode)
        {
            ConditionalActivity n = new ConditionalActivity(GetNextActivityId());
            n.Initialize(null, "", "");
            n.Priority = getNextActivityPriority(n, fromNode);
            n.Image.Size = new System.Drawing.Size(24, 24);
            this.Add(n);
            //m_Activities.Add(n);
            n.Name = GetNextActivityName();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                n.Position = new Point(Convert.ToInt32(Location.X) - 12, Convert.ToInt32(Location.Y) - 12);
            }
            else
            {
                n.Position = Location;
            }
            MainForm.App.unCheckToolStripButtons();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                MainForm.App.GetCurrentView().Selection.Clear();
                MainForm.App.GetCurrentView().Selection.Add(n);
            }
            return n;
        }

        public CallWorkFlowActivity CreateCallWorkFlowActivity(PointF Location, GoNode fromNode)
        {
            CallWorkFlowActivity n = new CallWorkFlowActivity(GetNextActivityId());
            n.Initialize(null, "", "");
            n.Priority = getNextActivityPriority(n, fromNode);
            n.Image.Size = new System.Drawing.Size(24, 24);
            this.Add(n);
            //            m_Activities.Add(n);
            n.Name = GetNextActivityName();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                n.Position = new Point(Convert.ToInt32(Location.X) - 12, Convert.ToInt32(Location.Y) - 12);
            }
            else
            {
                n.Position = Location;
            }
            MainForm.App.unCheckToolStripButtons();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                MainForm.App.GetCurrentView().Selection.Clear();
                MainForm.App.GetCurrentView().Selection.Add(n);
            }
            return n;
        }

        public ReturnWorkflowActivity CreateReturnWorkFlowActivity(PointF Location, GoNode fromNode)
        {
            ReturnWorkflowActivity n = new ReturnWorkflowActivity(GetNextActivityId());
            n.Initialize(null, "", "");
            n.Priority = getNextActivityPriority(n, fromNode);
            n.Image.Size = new System.Drawing.Size(24, 24);
            this.Add(n);
            //            m_Activities.Add(n);
            n.Name = GetNextActivityName();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                n.Position = new Point(Convert.ToInt32(Location.X) - 12, Convert.ToInt32(Location.Y) - 12);
            }
            else
            {
                n.Position = Location;
            }
            MainForm.App.unCheckToolStripButtons();
            if (MainForm.App.documentIsBeingOpened == false)
            {
                MainForm.App.GetCurrentView().Selection.Clear();
                MainForm.App.GetCurrentView().Selection.Add(n);
            }
            return n;
        }

        public Comment CreateComment(PointF Location)
        {
            this.Document.StartTransaction();
            Comment comment = new Comment();
            comment.Initialize(null, "", "");
            GoText mText = new GoText();
            comment.Image.Size = new SizeF(0, 0);
            comment.Label.Multiline = true;
            comment.Label.Editable = true;
            comment.Label.Text = "Enter your comment here,\r\non multiple lines.";
            comment.Selectable = true;
            comment.Label.Bordered = true;
            comment.Label.BackgroundColor = Color.White;
            comment.Label.Shadowed = true;
            comment.Label.TextColor = Color.Black;
            comment.Label.Wrapping = true;
            comment.Label.Resizable = true;

            comment.CommentID = lastCommentID++;
            this.Add(comment);
            m_Comments.Add(comment);
            comment.Location = Location;
            MainForm.App.unCheckToolStripButtons();
            this.Document.FinishTransaction("Create Comment");
            return comment;
        }

        #endregion Create WorkFlowItems

        #region Misc

        public void DeleteWorkFlow()
        {
        }

        public string GetNextStateName()
        {
            bool createdName = false;
            int ID = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States.Count;

            while (createdName == false)
            {
                ID++;
                if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States.Count == 1)
                {
                    return "State1";
                }
                bool foundName = false;
                foreach (BaseState b in MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States)
                {
                    int nextID = ID + 1;
                    int prevID = ID - 1;
                    if (b.Name == "State" + ID.ToString()
                        || b.Name == "State" + nextID.ToString()
                        || b.Name == "State" + prevID.ToString())
                    {
                        foundName = true;
                        break;
                    }
                }
                if (foundName == false)
                {
                    for (int x = 0; x < MainForm.App.lstMultipleSelectionItems.Count; x++)
                    {
                        if (MainForm.App.lstMultipleSelectionItems[x].baseItem is BaseState)
                        {
                            BaseState b = MainForm.App.lstMultipleSelectionItems[x].baseItem as BaseState;
                            if (b.Name == "State" + (ID - 1).ToString())
                            {
                                foundName = true;
                                break;
                            }
                        }
                    }
                }
                if (foundName == false)
                {
                    createdName = true;
                }
            }
            return "State" + (ID - 1);
        }

        private string GetNextStateId()
        {
            return "";
        }

        public string GetNextActivityName()
        {
            bool createdName = false;
            int ID = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Activities.Count;

            while (createdName == false)
            {
                ID++;
                if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Activities.Count == 1)
                {
                    return "Activity1";
                }
                bool foundName = false;
                foreach (BaseActivity b in MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Activities)
                {
                    int nextID = ID + 1;
                    int prevID = ID - 1;
                    if (b.Name == "Activity" + ID.ToString()
                        || b.Name == "Activity" + nextID.ToString()
                        || b.Name == "Activity" + prevID.ToString())
                    {
                        foundName = true;
                        break;
                    }
                }
                if (foundName == false)
                {
                    for (int x = 0; x < MainForm.App.lstMultipleSelectionItems.Count; x++)
                    {
                        if (MainForm.App.lstMultipleSelectionItems[x].baseItem is BaseActivity)
                        {
                            BaseActivity b = MainForm.App.lstMultipleSelectionItems[x].baseItem as BaseActivity;
                            if (b.Name == "Activity" + (ID - 1).ToString())
                            {
                                foundName = true;
                                break;
                            }
                        }
                    }
                }

                if (foundName == false)
                {
                    createdName = true;
                }
            }
            return "Activity" + (ID - 1);
        }

        private string GetNextActivityId()
        {
            int activityCount = 0;
            foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
            {
                if (o is BaseActivity)
                {
                    activityCount++;
                }
            }
            activityCount++;
            return activityCount.ToString();
        }

        public int getNextActivityPriority(GoNode mActivity, GoNode fromNode)
        {
            if (fromNode == null)
            {
                return 1;
            }
            int numActivitiesFromSameState = 0;

            if (fromNode.GetType() == typeof(CommonState))
            {
                return 1;
            }

            foreach (GoLink l in fromNode.Links)
            {
                if (l.ToNode is BaseActivity)
                {
                    numActivitiesFromSameState++;
                }
            }

            return numActivitiesFromSameState + 1;
        }

        public bool CheckIfItemExists(BaseItem i)
        {
            if (i == null)
            {
                return true;
            }
            foreach (GoObject o in this)
            {
                BaseItem a = o as BaseItem;
                if (a != null && a.WorkflowItemType != WorkflowItemType.None)
                {
                    if (a.WorkflowItemType == i.WorkflowItemType && a.Name == i.Name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public ExternalActivityItem ReturnMatchingExternalActivityItem(ExternalActivityItem i)
        {
            foreach (ExternalActivityItem exItem in this.ExternalActivityCollection)
            {
                if (exItem == i)
                {
                    return exItem;
                }
            }
            return null;
        }

        public CustomFormItem ReturnMatchingCustomForm(CustomFormItem i)
        {
            foreach (CustomFormItem cf in this.Forms)
            {
                if (cf.Name == i.Name)
                {
                    return cf;
                }
            }
            return null;
        }

        public void SelectAll()
        {
            foreach (GoObject o in this)
            {
                if (o.GetType() != typeof(WorkFlowLabel) &&
                    o.GetType() != typeof(GoSubGraphHandle) &&
                    o.GetType() != typeof(CustomLink))
                {
                    if (o.GetType().ToString() == "Northwoods.Go.GoBasicNode")
                    {
                        GoBasicNode mNode = o as GoBasicNode;
                        if (mNode.Text != "")
                        {
                            MainForm.App.GetCurrentView().Selection.Add(o);
                        }
                    }
                    else
                    {
                        MainForm.App.GetCurrentView().Selection.Add(o);
                    }
                }
            }
        }

        #endregion Misc
    }

    [Serializable]
    public class WorkFlowLabel : GoText
    {
        public WorkFlowLabel(string WorkFlowName)
        {
            Text = WorkFlowName;
            this.Selectable = false;
            this.Movable = false;
        }

        public override bool CanEdit()
        {
            return false;
        }
    }
}