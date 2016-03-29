using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Northwoods.Go;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.XML
{
    internal class XMLOpenDocument
    {
        public List<NodeItem> lstNodeItems = new List<NodeItem>();

        public XMLOpenDocument(string fileName, string xmlFile)
        {
            Dictionary<string, ReferenceItem> FilesToLoad = new Dictionary<string, ReferenceItem>();

            string Name = Path.GetFileNameWithoutExtension(fileName);

            string FolderName = Path.GetDirectoryName(fileName);

            ProcessForm canvas = new ProcessForm("open", Name);
            canvas.MdiParent = MainForm.App;
            canvas.Show();
            MainForm.App.GetCurrentView().Document.FixedSize = true;

            MainForm.App.GetCurrentView().Cursor = Cursors.WaitCursor;
            canvas.View.NewLinkPrototype = new CustomLink();
            canvas.Text = canvas.Name;

            MainForm.App.GetCurrentView().Document.Location = fileName;
            MainForm.App.GetCurrentView().Name = Path.GetFileNameWithoutExtension(MainForm.App.GetCurrentView().Document.Location);

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(fileName);

            XmlNode root = xdoc.DocumentElement;

            // set up the root folder based on whether this is a retreived document or not.....
            XmlAttribute retrieved = root.Attributes["Retrieved"];
            if (retrieved != null)
            {
                MainForm.App.isRetrieved = Convert.ToBoolean(retrieved.Value);
                if (MainForm.App.isRetrieved)
                {
                    MainForm.App.RootFolder = string.Format("{0}\\", FolderName);
                }
            }

            XmlAttribute a;
            a = root.Attributes["ProductVersion"];
            string docversion = "";
            if (a != null)
                docversion = a.Value.ToString();
            if (docversion != "0.1" && docversion != "0.2" && docversion != "0.3")
            {
                throw new NotSupportedException("For simplicity, this sample application does not handle different versions of saved documents");
            }
            a = root.Attributes["MapVersion"];
            if (a != null)
                MainForm.App.GetCurrentView().Document.MapVersion = a.Value;
            a = root.Attributes["Name"];
            if (a != null)
                canvas.Name = a.Value;

            a = root.Attributes["Legacy"];
            if (a != null)
                MainForm.App.GetCurrentView().Document.IsLegacy = Convert.ToBoolean(a.Value);
            else
                MainForm.App.GetCurrentView().Document.IsLegacy = true;

            a = root.Attributes["ViewableOnUserInterfaceVersion"];
            if (a != null)
                MainForm.App.GetCurrentView().Document.HaloV3Viewable = Convert.ToInt32(a.Value) == 3 ? true : false;

            List<AppliesToNodeItem> lstAppliesTo = new List<AppliesToNodeItem>();
            List<LinkedActivityItem> lstLinkedActivity = new List<LinkedActivityItem>();
            List<ArchiveStateItem> lstArchiveItem = new List<ArchiveStateItem>();

            List<CallWorkFlowItem> lstCallWorkFlow = new List<CallWorkFlowItem>();

            bool rolesCreated = false;

            foreach (XmlNode item in root)
            {
                if (item.NodeType != XmlNodeType.Element) continue;
                if (item.Name == "UsingStatements" || item.Name == "GlobalReferences" || item.Name == "References") continue;

                #region // Create WorkFlows

                if (item.Name == "WorkFlows")
                {
                    WorkFlow m_CurrentWorkFlow = null;
                    XmlNode mNode = root.FirstChild;

                    foreach (XmlNode workflow in mNode)
                    {
                        if (workflow.Name == "WorkFlow")
                        {
                            float locX = 0;
                            float locY = 0;
                            a = workflow.Attributes["LocationX"];
                            if (a != null)
                            {
                                locX = Convert.ToInt32(float.Parse(a.Value));
                            }
                            a = workflow.Attributes["LocationY"];
                            if (a != null)
                            {
                                locY = Convert.ToInt32(float.Parse(a.Value));
                            }
                            a = workflow.Attributes["WorkFlowName"];
                            if (a != null)
                            {
                                MainForm.App.GetCurrentView().Document.CreateWorkFlow("open", a.InnerText);

                                m_CurrentWorkFlow = MainForm.App.GetCurrentView().Document.CurrentWorkFlow;
                                m_CurrentWorkFlow.Position = new PointF(locX, locY);
                                NodeItem mItem = new NodeItem();
                                MainForm.App.GetCurrentView().Document.SelectWorkFlow(m_CurrentWorkFlow);
                                MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Position = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.expandedLocation;
                            }
                            a = workflow.Attributes["GenericKeyTypeKey"];
                            if (a != null)
                                MainForm.App.GetCurrentView().Document.CurrentWorkFlow.GenericKeyTypeKey = int.Parse(a.InnerText);
                        }
                    }
                }

                #endregion // Create WorkFlows

                #region //Roles

                if (item.Name == "Roles")
                {
                    rolesCreated = true;
                    XmlNode mNode = root.ChildNodes[1];

                    foreach (XmlNode RoleColl in mNode)
                    {
                        RolesCollectionItem mItem = new RolesCollectionItem();

                        a = RoleColl.Attributes["Role"];

                        if (a != null)
                        {
                            mItem.Name = a.InnerText;
                        }

                        a = RoleColl.Attributes["Description"];
                        if (a != null)
                        {
                            mItem.Description = a.InnerText;
                        }

                        a = RoleColl.Attributes["RoleType"];
                        if (a != null)
                        {
                            if (a.InnerText == "Global")
                            {
                                mItem.RoleType = RoleType.Global;
                            }
                            else
                            {
                                mItem.RoleType = RoleType.WorkFlow;
                            }
                        }

                        a = RoleColl.Attributes["WorkFlow"];
                        if (a != null)
                        {
                            for (int v = 0; v < MainForm.App.GetCurrentView().Document.WorkFlows.Length; v++)
                            {
                                if (MainForm.App.GetCurrentView().Document.WorkFlows[v].WorkFlowName == a.InnerText)
                                {
                                    mItem.WorkFlowItem = MainForm.App.GetCurrentView().Document.WorkFlows[v];
                                    break;
                                }
                            }
                        }

                        a = RoleColl.Attributes["IsDynamic"];
                        if (a != null)
                        {
                            mItem.IsDynamic = Convert.ToBoolean(a.InnerText);
                        }
                        MainForm.App.GetCurrentView().Document.Roles.Add(mItem);

                        foreach (XmlNode collectNode in RoleColl)
                        {
                            if (collectNode.Name == "CodeSections")
                            {
                                foreach (XmlNode codeSecNode in collectNode)
                                {
                                    a = codeSecNode.Attributes["Name"];
                                    string mCodeSection = "";
                                    if (a != null)
                                    {
                                        mCodeSection = a.InnerText;
                                    }
                                    a = codeSecNode.Attributes["Code"];
                                    string mCode = "";
                                    if (a != null)
                                    {
                                        mCode = a.InnerText;
                                    }
                                    mItem.SetCodeSectionData(mCodeSection, mCode);
                                }
                            }
                        }
                        mItem = null;
                    }
                }

                #endregion //Roles

                if (item.Name == "Roles" || docversion == "0.1")
                {
                    WorkFlow m_CurrentWorkFlow = null;
                    XmlNode mNode = root.FirstChild;
                    int workFlowIndex = 0;
                    //Loop WorkFlows
                    foreach (XmlNode workflow in mNode)
                    {
                        m_CurrentWorkFlow = MainForm.App.GetCurrentView().Document.WorkFlows[workFlowIndex];
                        MainForm.App.GetCurrentView().Document.SelectWorkFlow(m_CurrentWorkFlow);
                        PointF MapPos = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Position;
                        workFlowIndex++;
                        if (workflow.Name == "WorkFlow")
                        {
                            float locX = 0;
                            float locY = 0;
                            Guid x2id = Guid.Empty;
                            foreach (XmlNode nodeItem in workflow)
                            {
                                #region //InvisibleAnchorNode

                                if (nodeItem.Name == "InvisibleAnchorNode")
                                {
                                    locX = 0;
                                    locY = 0;
                                    a = nodeItem.Attributes["LocationX"];
                                    if (a != null)
                                    {
                                        locX = float.Parse(a.Value);
                                    }
                                    a = nodeItem.Attributes["LocationY"];
                                    if (a != null)
                                    {
                                        locY = float.Parse(a.Value);
                                    }

                                    PointF nodeLoc = new PointF(locX - MapPos.X, locY - MapPos.Y);

                                    InvisibleAnchorNode n = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateInvisibleAnchorNode(nodeLoc) as InvisibleAnchorNode;
                                    NodeItem mItem = new NodeItem();
                                    mItem.Item = n;
                                    mItem.Position = nodeLoc;
                                    lstNodeItems.Add(mItem);
                                }

                                #endregion //InvisibleAnchorNode

                                #region //Clapperboard

                                if (nodeItem.Name == "Clapperboard")
                                {
                                    locX = 0;
                                    locY = 0;
                                    a = nodeItem.Attributes["LocationX"];
                                    if (a != null)
                                    {
                                        locX = float.Parse(a.Value);
                                    }
                                    a = nodeItem.Attributes["LocationY"];
                                    if (a != null)
                                    {
                                        locY = float.Parse(a.Value);
                                    }

                                    PointF ClapperLoc = new PointF(locX - MapPos.X, locY - MapPos.Y);

                                    ClapperBoard n = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateClapperBoard(ClapperLoc) as ClapperBoard;
                                    NodeItem mItem = new NodeItem();
                                    mItem.Item = n;

                                    mItem.Position = ClapperLoc;
                                    lstNodeItems.Add(mItem);

                                    a = nodeItem.Attributes["LimitAccessTo"];
                                    if (a != null)
                                    {
                                        for (int x = 0; x < MainForm.App.GetCurrentView().Document.Roles.Count; x++)
                                        {
                                            if (MainForm.App.GetCurrentView().Document.Roles[x].Name.ToString() == a.InnerText.ToString())
                                            {
                                                n.LimitAccessTo = MainForm.App.GetCurrentView().Document.Roles[x];
                                                break;
                                            }
                                        }
                                    }

                                    a = nodeItem.Attributes["KeyVariable"];
                                    if (a != null)
                                    {
                                        for (int s = 0; s < m_CurrentWorkFlow.CustomVariables.Count; s++)
                                            if (m_CurrentWorkFlow.CustomVariables[s].Name.ToString() == a.InnerText.ToString())
                                            {
                                                n.KeyVariable = m_CurrentWorkFlow.CustomVariables[s];
                                            }
                                    }

                                    a = nodeItem.Attributes["Subject"];
                                    if (a != null)
                                    {
                                        n.Subject = a.InnerText;
                                    }
                                }

                                #endregion //Clapperboard

                                #region // States

                                Point screenPnt;
                                if (nodeItem.Name == "States")
                                {
                                    foreach (XmlNode StateItem in nodeItem)
                                    {
                                        if (StateItem.Name == "State")
                                        {
                                            a = StateItem.Attributes["Type"];
                                            if (a != null)
                                            {
                                                string holdStateType = a.InnerText;
                                                locX = 0;
                                                locY = 0;
                                                a = StateItem.Attributes["LocationX"];
                                                if (a != null)
                                                {
                                                    locX = float.Parse(a.Value);
                                                }
                                                a = StateItem.Attributes["LocationY"];
                                                if (a != null)
                                                {
                                                    locY = float.Parse(a.Value);
                                                }
                                                a = StateItem.Attributes["X2ID"];
                                                if (a != null)
                                                {
                                                    x2id = Guid.Parse(a.Value);
                                                }
                                                else
                                                {
                                                    x2id = Guid.Empty;
                                                }

                                                BaseState n = null;
                                                MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Position = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.expandedLocation;

                                                PointF LoadPoint = new PointF(locX - MapPos.X, locY - MapPos.Y);

                                                switch (holdStateType)
                                                {
                                                    case "SAHL.X2Designer.Items.SystemState":
                                                        {
                                                            n = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateSystemState(LoadPoint) as BaseState;
                                                            break;
                                                        }
                                                    case "SAHL.X2Designer.Items.SystemDecisionState":
                                                        {
                                                            n = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateSystemDecisionState(LoadPoint) as BaseState;
                                                            break;
                                                        }
                                                    case "SAHL.X2Designer.Items.UserState":
                                                        {
                                                            n = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateUserState(LoadPoint) as BaseState;
                                                            break;
                                                        }
                                                    case "SAHL.X2Designer.Items.HoldState":
                                                        {
                                                            n = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateHoldState(LoadPoint) as BaseState;
                                                            break;
                                                        }
                                                    case "SAHL.X2Designer.Items.ArchiveState":
                                                        {
                                                            n = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateArchiveState(LoadPoint) as BaseState;
                                                            ArchiveStateItem asi = new ArchiveStateItem();
                                                            if (StateItem.Attributes["ReturnActivity"] != null)
                                                                asi.ReturnActivity = StateItem.Attributes["ReturnActivity"].Value;
                                                            if (StateItem.Attributes["ReturnWorkflow"] != null)
                                                                asi.ReturnWorkflow = StateItem.Attributes["ReturnWorkflow"].Value;
                                                            asi._archivestate = (ArchiveState)n;
                                                            lstArchiveItem.Add(asi);
                                                            break;
                                                        }
                                                    case "SAHL.X2Designer.Items.CommonState":
                                                        {
                                                            n = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateCommonState(LoadPoint) as BaseState;
                                                            break;
                                                        }
                                                }
                                                n.X2ID = x2id;
                                                NodeItem mItem3 = new NodeItem();
                                                mItem3.Item = n;
                                                mItem3.Position = LoadPoint;
                                                lstNodeItems.Add(mItem3);
                                                a = StateItem.Attributes["StateName"];
                                                if (a != null)
                                                {
                                                    n.Name = a.InnerText;
                                                }
                                                a = StateItem.Attributes["UseAutoForward"];
                                                if (a != null)
                                                {
                                                    SystemState mSystemState = n as SystemState;
                                                    mSystemState.UseAutoForward = Convert.ToBoolean(a.InnerText);
                                                }
                                                foreach (XmlNode collectNode in StateItem)
                                                {
                                                    if (collectNode.Name == "CodeSections")
                                                    {
                                                        foreach (XmlNode codeSecNode in collectNode)
                                                        {
                                                            a = codeSecNode.Attributes["Name"];
                                                            string mCodeSection = "";
                                                            if (a != null)
                                                            {
                                                                mCodeSection = a.InnerText;
                                                            }
                                                            a = codeSecNode.Attributes["Code"];
                                                            string mCode = "";
                                                            if (a != null)
                                                            {
                                                                mCode = a.InnerText;
                                                            }
                                                            n.SetCodeSectionData(mCodeSection, mCode);
                                                        }
                                                    }
                                                    if (collectNode.Name == "TrackList")
                                                    {
                                                        BaseStateWithLists mBaseState = n as BaseStateWithLists;
                                                        if (mBaseState != null)
                                                        {
                                                            foreach (XmlNode watchColl in collectNode.ChildNodes)
                                                            {
                                                                a = watchColl.Attributes["TrackListCollectionItem"];
                                                                if (a != null)
                                                                {
                                                                    for (int x = 0; x < mBaseState.TrackList.Count; x++)
                                                                    {
                                                                        if (mBaseState.TrackList[x].RoleItem.Name == a.InnerText)
                                                                        {
                                                                            mBaseState.TrackList[x].IsChecked = true;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (collectNode.Name == "WorkList")
                                                    {
                                                        BaseStateWithLists mBaseState = n as BaseStateWithLists;
                                                        if (mBaseState != null)
                                                        {
                                                            foreach (XmlNode toDoColl in collectNode.ChildNodes)
                                                            {
                                                                a = toDoColl.Attributes["WorkListCollectionItem"];
                                                                if (a != null)
                                                                {
                                                                    foreach (RoleInstance r in mBaseState.WorkList)
                                                                    {
                                                                        if (r.RoleItem.Name == a.InnerText)
                                                                        {
                                                                            r.IsChecked = true;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (collectNode.Name == "AppliesTo")
                                                    {
                                                        AppliesToNodeItem mItem = new AppliesToNodeItem();
                                                        mItem.commonState = n as CommonState;
                                                        mItem.xNode = StateItem;
                                                        mItem.workFlow = MainForm.App.GetCurrentView().Document.CurrentWorkFlow;
                                                        lstAppliesTo.Add(mItem);
                                                    }

                                                    if (collectNode.Name == "CustomForms")
                                                    {
                                                        UserState mUserState = n as UserState;
                                                        if (mUserState != null)
                                                        {
                                                            foreach (XmlNode custFormColl in collectNode.ChildNodes)
                                                            {
                                                                a = custFormColl.Attributes["FormName"];
                                                                if (a != null)
                                                                {
                                                                    for (int p = 0; p < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms.Count; p++)
                                                                    {
                                                                        if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[p].Name == a.InnerText)
                                                                        {
                                                                            mUserState.CustomForms.Add(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[p]);
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            HoldState mHoldState = n as HoldState;
                                                            if (mHoldState != null)
                                                            {
                                                                foreach (XmlNode custFormColl in collectNode.ChildNodes)
                                                                {
                                                                    a = custFormColl.Attributes["FormName"];
                                                                    if (a != null)
                                                                    {
                                                                        for (int p = 0; p < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms.Count; p++)
                                                                        {
                                                                            if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[p].Name == a.InnerText)
                                                                            {
                                                                                mHoldState.CustomForms.Add(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[p]);
                                                                                break;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                n = null;
                                            }
                                        }
                                    }
                                }

                                #endregion // States

                                #region //Activities

                                if (nodeItem.Name == "Activities")
                                {
                                    foreach (XmlNode ActivityItem in nodeItem)
                                    {
                                        CallWorkFlowItem mCallWorkFlowItem = new CallWorkFlowItem();
                                        if (ActivityItem.Name == "Activity")
                                        {
                                            a = ActivityItem.Attributes["Type"];
                                            if (a != null)
                                            {
                                                string holdActionType = a.InnerText;
                                                locX = 0;
                                                locY = 0;
                                                a = ActivityItem.Attributes["LocationX"];
                                                if (a != null)
                                                {
                                                    locX = float.Parse(a.Value);
                                                }
                                                a = ActivityItem.Attributes["LocationY"];
                                                if (a != null)
                                                {
                                                    locY = float.Parse(a.Value);
                                                }
                                                a = ActivityItem.Attributes["X2ID"];
                                                if (a != null)
                                                {
                                                    x2id = Guid.Parse(a.Value);
                                                }
                                                else
                                                {
                                                    x2id = Guid.Empty;
                                                }
                                                string strFromNode = "";
                                                a = ActivityItem.Attributes["FromNode"];
                                                if (a != null)
                                                {
                                                    strFromNode = a.InnerText;
                                                }

                                                string strToNode = "";
                                                a = ActivityItem.Attributes["ToNode"];
                                                if (a != null)
                                                {
                                                    strToNode = a.InnerText;
                                                }

                                                BaseActivity n = null;

                                                PointF LoadPoint = new PointF(locX - MapPos.X, locY - MapPos.Y);

                                                if (strToNode != "")
                                                {
                                                    BaseItem mFromNode = new BaseItem();
                                                    BaseItem mToNode = new BaseItem();
                                                    foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                                                    {
                                                        BaseItem bs = null;
                                                        if (o is BaseState || o is ClapperBoard)
                                                        {
                                                            bs = o as BaseItem;
                                                            if (bs.Name == strFromNode)
                                                            {
                                                                mFromNode = bs;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                                                    {
                                                        BaseState bs = null;
                                                        if (o is BaseState)
                                                        {
                                                            bs = o as BaseState;
                                                            if (bs.Name == strToNode)
                                                            {
                                                                mToNode = bs as BaseItem;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    switch (holdActionType)
                                                    {
                                                        case "SAHL.X2Designer.Items.UserActivity":
                                                            {
                                                                n = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateUserActivity(LoadPoint, mFromNode) as BaseActivity;

                                                                UserActivity mUserAction = n as UserActivity;

                                                                a = ActivityItem.Attributes["CustomForm"];
                                                                if (a != null)
                                                                {
                                                                    for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms.Count; x++)
                                                                    {
                                                                        if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[x].Name == a.InnerText)
                                                                        {
                                                                            mUserAction.CustomForm = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[x];
                                                                            break;
                                                                        }
                                                                    }
                                                                }

                                                                break;
                                                            }
                                                        case "SAHL.X2Designer.Items.TimedActivity":
                                                            {
                                                                n = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateTimedActivity(LoadPoint, mFromNode) as BaseActivity;

                                                                break;
                                                            }
                                                        case "SAHL.X2Designer.Items.ConditionalActivity":
                                                            {
                                                                n = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateConditionalActivity(LoadPoint, mFromNode) as BaseActivity;

                                                                break;
                                                            }
                                                        case "SAHL.X2Designer.Items.ExternalActivity":
                                                            {
                                                                n = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateExternalActivity(LoadPoint, mFromNode) as BaseActivity;

                                                                break;
                                                            }
                                                        case "SAHL.X2Designer.Items.CallWorkFlowActivity":
                                                            {
                                                                n = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateCallWorkFlowActivity(LoadPoint, mFromNode) as BaseActivity;

                                                                mCallWorkFlowItem = new CallWorkFlowItem();
                                                                mCallWorkFlowItem.callWorkFlowActivity = n as CallWorkFlowActivity;
                                                                break;
                                                            }
                                                        case "SAHL.X2Designer.Items.ReturnWorkflowActivity":
                                                            {
                                                                n = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateReturnWorkFlowActivity(LoadPoint, mFromNode) as BaseActivity;
                                                                break;
                                                            }
                                                    }
                                                    n.X2ID = x2id;
                                                    NodeItem mItem2 = new NodeItem();
                                                    mItem2.Item = n;
                                                    mItem2.Position = LoadPoint;
                                                    lstNodeItems.Add(mItem2);
                                                    a = ActivityItem.Attributes["Name"];
                                                    if (a != null)
                                                    {
                                                        n.Name = a.InnerText;
                                                    }

                                                    a = ActivityItem.Attributes["Id"];
                                                    if (a != null)
                                                    {
                                                        n.Id = a.InnerText;
                                                    }

                                                    a = ActivityItem.Attributes["Message"];
                                                    if (a != null)
                                                    {
                                                        n.Message = a.InnerText;
                                                    }

                                                    a = ActivityItem.Attributes["StageTransitionMessage"];
                                                    if (a != null)
                                                    {
                                                        n.StageTransitionMessage = a.InnerText;
                                                    }

                                                    a = ActivityItem.Attributes["SplitWorkFlow"];
                                                    if (a != null)
                                                    {
                                                        n.SplitWorkFlow = Convert.ToBoolean(a.InnerText);
                                                    }

                                                    a = ActivityItem.Attributes["Description"];
                                                    if (a != null)
                                                    {
                                                        n.Description = a.InnerText;
                                                    }

                                                    a = ActivityItem.Attributes["Priority"];
                                                    if (a != null)
                                                    {
                                                        n.Priority = Convert.ToInt32(a.InnerText);
                                                    }

                                                    a = ActivityItem.Attributes["RaiseExternalActivity"];
                                                    if (a != null)
                                                    {
                                                        for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.ExternalActivityCollection.Count; x++)
                                                        {
                                                            if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.ExternalActivityCollection[x].ExternalActivity == a.InnerText)
                                                            {
                                                                n.RaiseExternalActivity = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.ExternalActivityCollection[x];
                                                            }
                                                        }
                                                    }

                                                    a = ActivityItem.Attributes["InvokedBy"];
                                                    if (a != null)
                                                    {
                                                        ExternalActivity mExternalActivity = n as ExternalActivity;
                                                        for (int s = 0; s < m_CurrentWorkFlow.ExternalActivityCollection.Count; s++)
                                                            if (m_CurrentWorkFlow.ExternalActivityCollection[s].ExternalActivity.ToString() == a.InnerText.ToString())
                                                            {
                                                                mExternalActivity.InvokedBy = m_CurrentWorkFlow.ExternalActivityCollection[s];
                                                            }
                                                    }

                                                    a = ActivityItem.Attributes["ExternalActivityRaiseFolder"];
                                                    if (a != null)
                                                    {
                                                        ExternalActivity mExternalActivity = n as ExternalActivity;
                                                        mExternalActivity.InvokeOnInstanceTarget = a.InnerText;
                                                    }

                                                    a = ActivityItem.Attributes["UseLinkedActivity"];
                                                    if (a != null)
                                                    {
                                                        UserActivity mUserActivity = n as UserActivity;
                                                        mUserActivity.UseLinkedActivity = Convert.ToBoolean(a.InnerText);
                                                    }

                                                    a = ActivityItem.Attributes["LinkedActivity"];
                                                    if (a != null)
                                                    {
                                                        UserActivity mUserActivity = n as UserActivity;
                                                        LinkedActivityItem mItem = new LinkedActivityItem();
                                                        if (a.InnerText.Length > 0)
                                                        {
                                                            mItem.activity = n.Name;
                                                            mItem.LinkedActivityValue = a.InnerText;
                                                            mItem.xNode = nodeItem;
                                                            lstLinkedActivity.Add(mItem);
                                                        }
                                                    }

                                                    a = ActivityItem.Attributes["WorkFlowToCall"];
                                                    if (a != null)
                                                    {
                                                        CallWorkFlowActivity mCallWorkFlowActivity = n as CallWorkFlowActivity;
                                                        if (mCallWorkFlowActivity != null)
                                                        {
                                                            mCallWorkFlowItem.WorkFlowToCall = a.InnerText;
                                                        }
                                                    }

                                                    a = ActivityItem.Attributes["ActivityToCall"];
                                                    if (a != null)
                                                    {
                                                        CallWorkFlowActivity mActivityToCall = n as CallWorkFlowActivity;
                                                        if (mActivityToCall != null)
                                                        {
                                                            mCallWorkFlowItem.ActivityToCall = a.InnerText;
                                                            lstCallWorkFlow.Add(mCallWorkFlowItem);
                                                        }
                                                    }
                                                    a = ActivityItem.Attributes["ReturnActivity"];
                                                    if (null != a)
                                                    {
                                                        CallWorkFlowActivity mActivityToCall = n as CallWorkFlowActivity;
                                                        if (mActivityToCall != null)
                                                        {
                                                            mCallWorkFlowItem.ReturnActivity = a.InnerText;
                                                            mCallWorkFlowItem.HostWorkflow = m_CurrentWorkFlow.WorkFlowName;
                                                        }
                                                    }

                                                    foreach (XmlNode collectNode in ActivityItem)
                                                    {
                                                        if (collectNode.Name == "CodeSections")
                                                        {
                                                            foreach (XmlNode codeSecNode in collectNode)
                                                            {
                                                                a = codeSecNode.Attributes["Name"];
                                                                string mCodeSection = "";
                                                                if (a != null)
                                                                {
                                                                    mCodeSection = a.InnerText;
                                                                }
                                                                a = codeSecNode.Attributes["Code"];
                                                                string mCode = "";
                                                                if (a != null)
                                                                {
                                                                    mCode = a.InnerText;
                                                                }
                                                                n.SetCodeSectionData(mCodeSection, mCode);
                                                            }
                                                        }

                                                        if (collectNode.Name == "BusinessStageTransitions")
                                                        {
                                                            IBusinessStageTransitions busItem = n as IBusinessStageTransitions;

                                                            if (busItem != null)
                                                            {
                                                                foreach (XmlNode businessNode in collectNode)
                                                                {
                                                                    BusinessStageItem bi = new BusinessStageItem();
                                                                    a = businessNode.Attributes["StageDefinitionKey"];
                                                                    if (a != null)
                                                                    {
                                                                        bi.SDSDGKey = a.InnerText;
                                                                    }
                                                                    a = businessNode.Attributes["DefinitionGroupDescription"];
                                                                    if (a != null)
                                                                    {
                                                                        bi.DefinitionGroupDescription = a.InnerText;
                                                                    }
                                                                    a = businessNode.Attributes["DefinitionDescription"];
                                                                    if (a != null)
                                                                    {
                                                                        bi.DefinitionDescription = a.InnerText;
                                                                    }
                                                                    busItem.BusinessStageTransitions.Add(bi);
                                                                }
                                                            }
                                                        }

                                                        if (collectNode.Name == "Access")
                                                        {
                                                            foreach (XmlNode AccessColl in collectNode.ChildNodes)
                                                            {
                                                                a = AccessColl.Attributes["AccessCollectionItem"];
                                                                if (a != null)
                                                                {
                                                                    UserActivity mUserActivity = n as UserActivity;
                                                                    if (mUserActivity != null)
                                                                    {
                                                                        RoleInstance mItem = mUserActivity.Access.GetByName(a.InnerText);
                                                                        if (mItem != null)
                                                                            mItem.IsChecked = true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    bool foundPort = false;
                                                    CustomLink nLink = new CustomLink();
                                                    GoPort holdLeftPort = null;
                                                    GoPort holdRightPort = null;
                                                    foreach (MultiPortNodePort p in mFromNode.Ports)
                                                    {
                                                        holdLeftPort = p;
                                                        foundPort = true;
                                                        break;
                                                    }
                                                    MultiPortNodePort mpnp1;
                                                    if (foundPort == false)
                                                    {
                                                        mpnp1 = (MultiPortNodePort)mFromNode.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                                                        mpnp1.Size = new SizeF(55, 55);
                                                        mpnp1.Visible = false;
                                                        mpnp1.Center = mFromNode.Location;
                                                        holdLeftPort = mpnp1;
                                                    }
                                                    foundPort = false;
                                                    foreach (MultiPortNodePort p in n.Ports)
                                                    {
                                                        holdRightPort = p;
                                                        foundPort = true;
                                                        break;
                                                    }
                                                    MultiPortNodePort mpnp2;
                                                    if (foundPort == false)
                                                    {
                                                        mpnp2 = (MultiPortNodePort)n.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                                                        mpnp2.Size = new SizeF(45, 45);
                                                        mpnp2.Visible = false;
                                                        mpnp2.Center = n.Location;
                                                        holdRightPort = mpnp2;
                                                    }
                                                    nLink.FromPort = holdLeftPort;
                                                    nLink.ToPort = holdRightPort;

                                                    MainForm.App.GetCurrentView().Document.CurrentWorkFlow.InsertBefore(null, nLink);
                                                    //                                                    MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Add(nLink);

                                                    CustomLink nLink2 = null;
                                                    if (n.WorkflowItemType != WorkflowItemType.CallWorkFlowActivity && n.WorkflowItemType != WorkflowItemType.ReturnWorkFlowActivity)
                                                    {
                                                        nLink2 = new CustomLink();
                                                        nLink2.Visible = true;
                                                    }
                                                    holdLeftPort = null;
                                                    holdRightPort = null;

                                                    foundPort = false;
                                                    foreach (MultiPortNodePort p in n.Ports)
                                                    {
                                                        holdLeftPort = p;
                                                        foundPort = true;
                                                        break;
                                                    }
                                                    if (foundPort == false)
                                                    {
                                                        mpnp1 = (MultiPortNodePort)n.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                                                        mpnp1.Size = new SizeF(45, 45);
                                                        mpnp1.Visible = false;
                                                        mpnp1.Center = n.Location;
                                                        holdLeftPort = mpnp1;
                                                    }
                                                    foundPort = false;
                                                    foreach (MultiPortNodePort p in mToNode.Ports)
                                                    {
                                                        holdRightPort = p;
                                                        foundPort = true;
                                                        break;
                                                    }
                                                    if (foundPort == false)
                                                    {
                                                        mpnp2 = (MultiPortNodePort)mToNode.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                                                        mpnp2.Size = new SizeF(55, 55);
                                                        mpnp2.Visible = false;
                                                        mpnp2.Center = mToNode.Location;
                                                        holdRightPort = mpnp2;
                                                    }
                                                    if (nLink2 != null)
                                                    {
                                                        nLink2.FromPort = holdLeftPort;
                                                        nLink2.ToPort = holdRightPort;
                                                        nLink2.ToArrow = true;
                                                        MainForm.App.GetCurrentView().Document.CurrentWorkFlow.InsertBefore(null, nLink2);
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
                                                n = null;
                                            }
                                        }
                                    }
                                }

                                #endregion //Activities

                                #region //Comments

                                if (nodeItem.Name == "Comments")
                                {
                                    foreach (XmlNode CommentColl in nodeItem.ChildNodes)
                                    {
                                        a = CommentColl.Attributes["LocationX"];
                                        if (a != null)
                                        {
                                            locX = float.Parse(a.Value);
                                        }

                                        a = CommentColl.Attributes["LocationY"];
                                        if (a != null)
                                        {
                                            locY = float.Parse(a.Value);
                                        }

                                        PointF loadPoint = new PointF(locX - MapPos.X, locY - MapPos.Y);

                                        Comment n = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateComment(loadPoint);

                                        NodeItem mItem = new NodeItem();
                                        mItem.Item = n;

                                        mItem.Position = loadPoint;
                                        lstNodeItems.Add(mItem);

                                        MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Add(n);
                                        a = CommentColl.Attributes["Name"];
                                        if (a != null)
                                        {
                                            n.Name = a.InnerText;
                                        }
                                        a = CommentColl.Attributes["CommentText"];
                                        if (a != null)
                                        {
                                            n.CommentText = a.InnerText;
                                        }
                                    }
                                }

                                #endregion //Comments

                                #region //Roles for version 0.1

                                if (docversion == "0.1")
                                {
                                    if (nodeItem.Name == "Roles")
                                    {
                                        foreach (XmlNode RoleColl in nodeItem.ChildNodes)
                                        {
                                            RolesCollectionItem mItem = new RolesCollectionItem();

                                            a = RoleColl.Attributes["Role"];
                                            bool foundRole = false;
                                            if (a != null)
                                            {
                                                for (int x = 0; x < MainForm.App.GetCurrentView().Document.Roles.Count; x++)
                                                {
                                                    if (MainForm.App.GetCurrentView().Document.Roles[x].Name == a.InnerText)
                                                    {
                                                        foundRole = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            if (foundRole == false)
                                            {
                                                if (a != null)
                                                {
                                                    mItem.Name = a.InnerText;
                                                }

                                                a = RoleColl.Attributes["Description"];
                                                if (a != null)
                                                {
                                                    mItem.Description = a.InnerText;
                                                }

                                                a = RoleColl.Attributes["RoleType"];
                                                if (a != null)
                                                {
                                                    if (a.InnerText == "Global")
                                                    {
                                                        mItem.RoleType = RoleType.Global;
                                                    }
                                                    else
                                                    {
                                                        mItem.RoleType = RoleType.WorkFlow;
                                                    }
                                                }

                                                a = RoleColl.Attributes["WorkFlow"];
                                                if (a != null)
                                                {
                                                    for (int v = 0; v < MainForm.App.GetCurrentView().Document.WorkFlows.Length; v++)
                                                    {
                                                        if (MainForm.App.GetCurrentView().Document.WorkFlows[v].WorkFlowName == a.InnerText)
                                                        {
                                                            mItem.WorkFlowItem = MainForm.App.GetCurrentView().Document.WorkFlows[v];
                                                            break;
                                                        }
                                                    }
                                                }

                                                a = RoleColl.Attributes["IsDynamic"];
                                                if (a != null)
                                                {
                                                    mItem.IsDynamic = Convert.ToBoolean(a.InnerText);
                                                }
                                                MainForm.App.GetCurrentView().Document.Roles.Add(mItem);

                                                foreach (XmlNode collectNode in RoleColl)
                                                {
                                                    if (collectNode.Name == "CodeSections")
                                                    {
                                                        foreach (XmlNode codeSecNode in collectNode)
                                                        {
                                                            a = codeSecNode.Attributes["Name"];
                                                            string mCodeSection = "";
                                                            if (a != null)
                                                            {
                                                                mCodeSection = a.InnerText;
                                                            }
                                                            a = codeSecNode.Attributes["Code"];
                                                            string mCode = "";
                                                            if (a != null)
                                                            {
                                                                mCode = a.InnerText;
                                                            }
                                                            mItem.SetCodeSectionData(mCodeSection, mCode);
                                                        }
                                                    }
                                                }
                                            }
                                            mItem = null;
                                        }
                                    }
                                }

                                #endregion //Roles for version 0.1

                                #region //External Activities

                                if (nodeItem.Name == "ExternalActivities")
                                {
                                    foreach (XmlNode ExternalActivityColl in nodeItem.ChildNodes)
                                    {
                                        ExternalActivityItem mItem = new ExternalActivityItem();

                                        a = ExternalActivityColl.Attributes["Name"];
                                        if (a != null)
                                        {
                                            mItem.ExternalActivity = a.InnerText;
                                        }
                                        a = ExternalActivityColl.Attributes["Description"];
                                        if (a != null)
                                        {
                                            mItem.Description = a.InnerText;
                                        }

                                        m_CurrentWorkFlow.ExternalActivityCollection.Add(mItem);
                                        mItem = null;
                                    }
                                }

                                #endregion //External Activities

                                #region //CustomForms

                                if (nodeItem.Name == "CustomForms")
                                {
                                    foreach (XmlNode CustomFormsColl in nodeItem.ChildNodes)
                                    {
                                        CustomFormItem mItem = new CustomFormItem();

                                        a = CustomFormsColl.Attributes["Name"];
                                        if (a != null)
                                        {
                                            mItem.Name = a.InnerText;
                                        }
                                        a = CustomFormsColl.Attributes["Description"];
                                        if (a != null)
                                        {
                                            mItem.Description = a.InnerText;
                                        }

                                        m_CurrentWorkFlow.Forms.Add(mItem);
                                        mItem = null;
                                    }
                                }

                                #endregion //CustomForms

                                #region //CustomVariables

                                if (nodeItem.Name == "CustomVariables")
                                {
                                    foreach (XmlNode VarColl in nodeItem.ChildNodes)
                                    {
                                        CustomVariableItem mItem = new CustomVariableItem();

                                        a = VarColl.Attributes["Name"];
                                        if (a != null)
                                        {
                                            mItem.Name = a.InnerText;
                                        }
                                        a = VarColl.Attributes["Type"];
                                        if (a != null)
                                        {
                                            mItem.Type = (CustomVariableType)new CustomVariableTypeTypeConvertor().ConvertFromString(a.InnerText);
                                        }

                                        a = VarColl.Attributes["Length"];
                                        if (a != null)
                                        {
                                            mItem.Length = Convert.ToInt32(a.InnerText);
                                        }

                                        m_CurrentWorkFlow.CustomVariables.Add(mItem);
                                        mItem = null;
                                    }
                                }

                                #endregion //CustomVariables
                            }
                        }
                    }
                }

                #region Roles Create

                if (rolesCreated || docversion == "0.1")
                {
                    for (int x = 0; x < lstCallWorkFlow.Count; x++)
                    {
                        if (lstCallWorkFlow[x].callWorkFlowActivity != null)
                        {
                            for (int y = 0; y < MainForm.App.GetCurrentView().Document.WorkFlows.Length; y++)
                            {
                                if (MainForm.App.GetCurrentView().Document.WorkFlows[y].WorkFlowName == lstCallWorkFlow[x].WorkFlowToCall)
                                {
                                    lstCallWorkFlow[x].callWorkFlowActivity.WorkFlowToCall = MainForm.App.GetCurrentView().Document.WorkFlows[y];
                                    break;
                                }
                            }
                            if (lstCallWorkFlow[x].callWorkFlowActivity.WorkFlowToCall != null)
                            {
                                foreach (BaseActivity ba in lstCallWorkFlow[x].callWorkFlowActivity.WorkFlowToCall.Activities)
                                {
                                    if (ba.Name == lstCallWorkFlow[x].ActivityToCall)
                                    {
                                        lstCallWorkFlow[x].callWorkFlowActivity.ActivityToCall = ba;
                                    }
                                }
                            }
                            // Go fins the containing workflow and find its states
                            if ((lstCallWorkFlow[x].callWorkFlowActivity.WorkFlowToCall != null) && (lstCallWorkFlow[x].ReturnActivity != null))
                            {
                                for (int y = 0; y < MainForm.App.GetCurrentView().Document.WorkFlows.Length; y++)
                                {
                                    if (MainForm.App.GetCurrentView().Document.WorkFlows[y].WorkFlowName == lstCallWorkFlow[x].HostWorkflow)
                                    {
                                        // find the return state in the list and set it.
                                        foreach (BaseActivity s in MainForm.App.GetCurrentView().Document.WorkFlows[y].Activities)
                                        {
                                            if (s.Name == lstCallWorkFlow[x].ReturnActivity)
                                            {
                                                lstCallWorkFlow[x].callWorkFlowActivity.ReturnActivity = s;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    for (int x = 0; x < lstArchiveItem.Count; x++)
                    {
                        ArchiveStateItem asi = lstArchiveItem[x];
                        for (int y = 0; y < MainForm.App.GetCurrentView().Document.WorkFlows.Length; y++)
                        {
                            WorkFlow w = MainForm.App.GetCurrentView().Document.WorkFlows[y];
                            if (w.WorkFlowName == asi.ReturnWorkflow)
                            {
                                asi._archivestate.WorkflowToReturnTo = w;
                                // now go get the activities on that workflow
                                foreach (BaseActivity ba in w.Activities)
                                {
                                    ReturnWorkflowActivity rwa = ba as ReturnWorkflowActivity;
                                    {
                                        if (null != rwa)
                                        {
                                            if (rwa.Name == asi.ReturnActivity)
                                            {
                                                asi._archivestate.ReturnActivity = rwa;
                                                break;
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }

                    for (int x = 0; x < lstAppliesTo.Count; x++)
                    {
                        MainForm.App.GetCurrentView().Document.SelectWorkFlow(lstAppliesTo[x].workFlow);
                        XmlNode collectNode = lstAppliesTo[x].xNode;

                        if (collectNode != null)
                        {
                            if (collectNode.Name == "State")
                            {
                                foreach (XmlNode appliesToColl in collectNode.ChildNodes)
                                {
                                    if (appliesToColl.Name == "AppliesTo")
                                    {
                                        foreach (XmlNode appliesToItem in appliesToColl.ChildNodes)
                                        {
                                            a = appliesToItem.Attributes["AppliesToCollectionItem"];
                                            if (a != null)
                                            {
                                                for (int o = 0; o < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States.Count; o++)
                                                {
                                                    if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[o] is CommonState == false)
                                                    {
                                                        BaseState mCommonState = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[o] as BaseState;
                                                        if (mCommonState.Text == a.Value.ToString())
                                                        {
                                                            CommonState lstCommonState = lstAppliesTo[x].commonState as CommonState;
                                                            if (!lstCommonState.AppliesTo.Contains(mCommonState))
                                                                lstCommonState.AppliesTo.Add(mCommonState);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    for (int x = 0; x < lstLinkedActivity.Count; x++)
                    {
                        XmlNode collectNode = lstLinkedActivity[x].xNode;

                        if (collectNode != null)
                        {
                            if (collectNode.Name == "Activities")
                            {
                                foreach (XmlNode linkedActivityColl in collectNode.ChildNodes)
                                {
                                    if (linkedActivityColl.Name == "Activity")
                                    {
                                        a = linkedActivityColl.Attributes["Name"];

                                        if (lstLinkedActivity[x].LinkedActivityValue == a.InnerText && a != null)
                                        {
                                            foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Activities)
                                            {
                                                UserActivity objectToSet = o as UserActivity;
                                                if (a != null)
                                                {
                                                    if (objectToSet.Name == lstLinkedActivity[x].activity)
                                                    {
                                                        a = linkedActivityColl.Attributes["LinkedActivity"];
                                                        string linkedActivityName = a.InnerText;
                                                        if (a != null && lstLinkedActivity[x].LinkedActivityValue.Length > 0)
                                                        {
                                                            foreach (GoObject obj in MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Activities)
                                                            {
                                                                if (obj is BaseActivity)
                                                                {
                                                                    BaseActivity mBaseActivity = obj as BaseActivity;
                                                                    if (mBaseActivity.Name == lstLinkedActivity[x].LinkedActivityValue)
                                                                    {
                                                                        objectToSet.LinkedActivity = mBaseActivity;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    foreach (WorkFlow wFlow in MainForm.App.GetCurrentView().Document)
                    {
                        wFlow.Expand();
                        foreach (GoObject o in wFlow)
                        {
                            BaseItem i = o as BaseItem;
                            if (i != null)
                            {
                                foreach (CustomLink l in i.Links)
                                {
                                    l.CalculateStroke();
                                }
                            }
                        }
                    }

                    //mZip.CloseFile();

                    MainForm.App.GetCurrentView().Cursor = Cursors.Default;
                }

                #endregion Roles Create
            }


            #region//Using Statements

            XmlNodeList xnlUsing = xdoc.SelectNodes("//UsingStatements/UsingStatement");
            foreach (XmlNode xnUsing in xnlUsing)
            {
                string Statement = xnUsing.Attributes["Statement"].Value;
                MainForm.App.GetCurrentView().Document.AddUsedUsingStatement(Statement);
            }

            #endregion Load references and using statements

            #region//Nuget Packages

            XmlNodeList xnlNuget = xdoc.SelectNodes("//NugetPackages/NugetPackage");
            foreach (XmlNode xnUsing in xnlNuget)
            {
                string packageID = xnUsing.Attributes["PackageName"].Value;
                string packageVersion = xnUsing.Attributes["Version"].Value;
				string dependsOn = xnUsing.Attributes["DependsOn"] != null ? xnUsing.Attributes["DependsOn"].Value : "Nothing";
				string dependsOnVersion = xnUsing.Attributes["DependsOnVersion"] != null ? xnUsing.Attributes["DependsOnVersion"].Value : "Nothing";
                MainForm.App.GetCurrentView().Document.AddNuGetPackage(packageID, packageVersion, dependsOn, dependsOnVersion);
            }

            #endregion Load references and using statements

            #region //References

            XmlNodeList xnlReferences = xdoc.SelectNodes("//References/Reference");
            foreach (XmlNode xnReference in xnlReferences)
            {
                ReferenceItem ri = new ReferenceItem(xnReference);
                if (FilesToLoad.ContainsKey(ri.Name))
                {
                    if (new VersionComparer().Compare(FilesToLoad[ri.Name].Version, ri.Version) > 0)
                    {
                        FilesToLoad[ri.Name] = ri;
                    }
                }
                else
                {
                    FilesToLoad[ri.Name] = ri;
                }
            }

            #endregion //References


            #region Load Asms to remote domain and update the Main collection

            // go load the files that were extracted from the zip file. Done here as if you try load during the unzip you
            // will try load a file whose dependancies potentially have not been extracted yet.
            string[] Keys = new string[FilesToLoad.Count];
            FilesToLoad.Keys.CopyTo(Keys, 0);
            for (int i = 0; i < Keys.Length; i++)
            {
                ReferenceItem ri = FilesToLoad[Keys[i]];
                MainForm.App.GetCurrentView().Document.AddReference(ri);
            }

            #endregion

            MainForm.App.GetCurrentView().Document.FixedSize = false;
            MainForm.App.GetCurrentView().Document.ComputeBounds();
        }
    }

    [Serializable]
    public class AppliesToNodeItem
    {
        private XmlNode m_XMLNode;
        private BaseState m_CommonState = new BaseState();
        private WorkFlow m_WorkFlow;

        public XmlNode xNode
        {
            get
            {
                return m_XMLNode;
            }
            set
            {
                m_XMLNode = value;
            }
        }

        public BaseState commonState
        {
            get
            {
                return m_CommonState;
            }
            set
            {
                m_CommonState = value;
            }
        }

        public WorkFlow workFlow
        {
            get
            {
                return m_WorkFlow;
            }
            set
            {
                m_WorkFlow = value;
            }
        }
    }

    [Serializable]
    public class LinkedActivityItem
    {
        private XmlNode m_XMLNode;
        private string m_Activity;
        private string m_LinkedActivityValue;

        public XmlNode xNode
        {
            get
            {
                return m_XMLNode;
            }
            set
            {
                m_XMLNode = value;
            }
        }

        public string activity
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

        public string LinkedActivityValue
        {
            get
            {
                return m_LinkedActivityValue;
            }
            set
            {
                m_LinkedActivityValue = value;
            }
        }
    }

    public class ArchiveStateItem
    {
        public string ReturnWorkflow;
        public string ReturnActivity;
        public ArchiveState _archivestate = null;
    }

    public class CallWorkFlowItem
    {
        private string m_WorkFlowToCall = null;
        private string m_ActivityToCall = null;
        string _ReturnActivity = null;
        string _HostWorkflow;
        private CallWorkFlowActivity m_CallWorkFlowActivity = null;

        public string ReturnActivity
        {
            get { return _ReturnActivity; }
            set { _ReturnActivity = value; }
        }

        public string HostWorkflow
        {
            get
            {
                return _HostWorkflow;
            }
            set { _HostWorkflow = value; }
        }

        public string WorkFlowToCall
        {
            get
            {
                return m_WorkFlowToCall;
            }
            set
            {
                m_WorkFlowToCall = value;
            }
        }

        public string ActivityToCall
        {
            get
            {
                return m_ActivityToCall;
            }
            set
            {
                m_ActivityToCall = value;
            }
        }

        public CallWorkFlowActivity callWorkFlowActivity
        {
            get
            {
                return m_CallWorkFlowActivity;
            }
            set
            {
                m_CallWorkFlowActivity = value;
            }
        }
    }

    public class NodeItem
    {
        private BaseItem m_Item;
        private PointF m_Position;

        public BaseItem Item
        {
            get
            {
                return m_Item;
            }
            set
            {
                m_Item = value;
            }
        }

        public PointF Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }
    }
}