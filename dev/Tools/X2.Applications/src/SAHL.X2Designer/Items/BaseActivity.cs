using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Northwoods.Go;
using SAHL.X2Designer.CodeGen;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Forms;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.Items
{
    /// <summary>
    /// Required Properties
    /// 1) Activity Name
    /// 2) Activity Priority
    /// 3) Message
    /// 4) Clone Folder
    /// 5) OnStart Activity
    /// 6) OnComplete Activity
    /// </summary>

    [Serializable]
    public abstract class BaseActivity : BaseItem, IPopupMenu
    {
        internal const string OnStart = "OnStart";
        internal const string OnComplete = "OnComplete";
        internal const string OnStageActivity = "OnStageActivity";
        internal const string OnAutoForward = "OnAutoForward";
        internal const string OnActivityMessage = "GetActivityMessage";
        protected const string YouCantHaveNiceThings = "// You must select and business stage transition to provide code behind.";
        protected string m_OnInvoke = "";
        protected string m_OnCommit = "";
        protected string m_StageTransition = "";
        protected string m_ActivityMessage = "";
        protected string m_StageTransitionMessage = "";
        protected bool m_CodeChanged;

        protected int m_Priority = 1;
        protected string m_Id = "";
        protected ExternalActivityItem m_RaiseExternalActivity;
        protected bool m_CloneFolder = false;
        protected string m_Message = "";
        protected string m_Description = "";
        int _sequence = int.MaxValue;
        Guid x2ID = Guid.Empty;

        public BaseActivity()
        {
            m_AvailableCodeSections = new string[] { OnStart, OnComplete, OnStageActivity, OnActivityMessage };

            this.AutoRescales = false;
        }

        #region Overrides

        public override string GetCodeSectionData(string CodeSectionName)
        {
            Debug.WriteLine(CodeSectionName);
            StringBuilder SB = new StringBuilder();
            if (CodeSectionName == OnStart)
            {
                if (m_OnInvoke == "")
                {
                    // generate the header
                    SB.Append(X2Generator.GenerateActivityStartHeader(this, MainForm.App.GetCurrentView().Document));
                    SB.AppendLine("\treturn true;");
                    SB.AppendLine("}");
                    m_OnInvoke = SB.ToString();
                }
                return m_OnInvoke;
            }
            else if (CodeSectionName == OnComplete)
            {
                if (m_OnCommit == "")
                {
                    // generate the header
                    SB.Append(X2Generator.GenerateActivityCompleteHeader(this, MainForm.App.GetCurrentView().Document));

                    // add the actual code
                    SB.AppendLine("\treturn true;");
                    SB.AppendLine("}");
                    m_OnCommit = SB.ToString();
                }
                return m_OnCommit;
            }
            else if (CodeSectionName == OnActivityMessage)
            {
                if (m_ActivityMessage == "")
                {
                    // generate the header
                    SB.Append(X2Generator.GenerateActivityMessageHeader(this, MainForm.App.GetCurrentView().Document));

                    // add the actual code
                    SB.AppendLine("\treturn string.Empty;");
                    SB.AppendLine("}");
                    m_ActivityMessage = SB.ToString();
                }
                return m_ActivityMessage;
            }
            else if (CodeSectionName == OnStageActivity)
            {
                if (m_StageTransition == "")
                {
                    if (!HasStageTransitions())
                    {
                        return YouCantHaveNiceThings;
                    }

                    List<BusinessStageItem> items = MainForm.App.GetCurrentView().Document.BusinessStages;

                    m_StageTransition = "";
                    // generate the header
                    SB.Append(X2Generator.GenerateOnStageTransitionHeader(this, MainForm.App.GetCurrentView().Document));
                    // add the actual code
                    SB.AppendLine("\treturn string.Empty;");
                    SB.AppendLine("}");
                    m_StageTransition = SB.ToString();
                }
                return m_StageTransition;
            }
            return "";
        }

        public abstract bool HasStageTransitions();

        public void UpdateStageTransitionMessage(string NewMessage)
        {
            string origData = this.GetCodeSectionData("OnStageActivity");
            int startPos = origData.IndexOf("return");
            int endPos = origData.IndexOf(";", startPos);

            string removed = origData.Remove(startPos, endPos - startPos + 1);
            string newData = removed.Insert(startPos, string.Format("return \"{0}\";", NewMessage));
            /*
            int linePos = origData.IndexOf("return");
            linePos = origData.IndexOf("null, ", linePos + 6);
            if (linePos == -1)
                return;
            int posOrigFirst = origData.IndexOf("null, ", linePos) + 6;
            int posOrigSecond = origData.IndexOf(')', posOrigFirst);
            string valOrig = origData.Substring(posOrigFirst, posOrigSecond - posOrigFirst);
            //   valOrig = valOrig.Replace("\"", string.Empty);
            if (NewMessage != valOrig)
            {
                origData = origData.Replace("X2ReturnData(null, " + valOrig + ")", "X2ReturnData(null, \"" + NewMessage + "\")");
            }
             * */
            m_CodeChanged = false;
            SetCodeSectionData("OnStageActivity", newData);
        }

        public override void SetCodeSectionData(string CodeSectionName, string SectionData)
        {
            if (CodeSectionName == OnStart)
            {
                m_OnInvoke = SectionData;
            }
            else if (CodeSectionName == OnComplete)
            {
                m_OnCommit = SectionData;
            }
            else if (CodeSectionName == OnStageActivity)
            {
                m_StageTransition = SectionData;
            }
            else if (CodeSectionName == OnActivityMessage)
            {
                m_ActivityMessage = SectionData;
            }
            else if (CodeSectionName == OnAutoForward)
            {
                string s = SectionData;
            }
        }

        public override void UpdateCodeSectionData(string OldValue, string NewValue)
        {
            if (NewValue.Length > 0 && OldValue.Length > 0)
            {
                //   MessageBox.Show(OldValue + " -> " + NewValue);
                Regex MCN = new Regex("\\b" + OldValue + "\\b");

                m_OnInvoke = MCN.Replace(m_OnInvoke, NewValue);
                X2Generator.ReplaceActivityStartHeader(OldValue, NewValue, ref m_OnInvoke);
                m_OnCommit = MCN.Replace(m_OnCommit, NewValue);
                X2Generator.ReplaceActivityCompleteHeader(OldValue, NewValue, ref m_OnCommit);
                m_StageTransition = MCN.Replace(m_StageTransition, NewValue);
                X2Generator.ReplaceStageTransitionHeader(OldValue, NewValue, ref m_StageTransition);
                m_ActivityMessage = MCN.Replace(m_ActivityMessage, NewValue);
                X2Generator.ReplaceActivityMessageHeader(OldValue, NewValue, ref m_ActivityMessage);
            }
        }

        public override string RefreshCodeSectionData(string CodeSectionName)
        {
            StringBuilder SB = new StringBuilder();
            if (CodeSectionName == OnStart)
            {
                if (m_OnInvoke != "")
                {
                    m_OnInvoke = "";
                }
                // generate the header
                SB.Append(X2Generator.GenerateActivityStartHeader(this, MainForm.App.GetCurrentView().Document));
                SB.AppendLine("\treturn true;");
                SB.AppendLine("}");
                m_OnInvoke = SB.ToString();

                return m_OnInvoke;
            }
            else if (CodeSectionName == OnComplete)
            {
                if (m_OnCommit != "")
                {
                    m_OnCommit = "";
                }
                // generate the header
                SB.Append(X2Generator.GenerateActivityCompleteHeader(this, MainForm.App.GetCurrentView().Document));

                // add the actual code
                SB.AppendLine("\treturn true;");
                SB.AppendLine("}");
                m_OnCommit = SB.ToString();

                return m_OnCommit;
            }
            else if (CodeSectionName == OnStageActivity)
            {
                m_StageTransition = "";
                // generate the header
                SB.Append(X2Generator.GenerateOnStageTransitionHeader(this, MainForm.App.GetCurrentView().Document));

                // add the actual code
                SB.AppendLine("\treturn \"" + StageTransitionMessage + "\";");
                SB.AppendLine("}");
                m_StageTransition = SB.ToString();
                return m_StageTransition;
            }
            else if (CodeSectionName == OnActivityMessage)
            {
                m_ActivityMessage = "";
                // generate the header
                SB.Append(X2Generator.GenerateActivityMessageHeader(this, MainForm.App.GetCurrentView().Document));

                // add the actual code
                SB.AppendLine("\treturn string.Empty;");
                SB.AppendLine("}");
                m_StageTransition = SB.ToString();

                return m_StageTransition;
            }
            return "";
        }

        public override object Properties
        {
            get
            {
                return new BaseActivityProperties(this, MainForm.App.GetCurrentView().Document);
            }
        }

        #endregion Overrides

        #region Properties

        public string Id
        {
            get
            {
                return m_Id;
            }
            set
            {
                // do a complex set where the other activities from the same state have their priorities reset
                m_Id = value;
            }
        }

        public string Message
        {
            get
            {
                return m_Message;
            }
            set
            {
                m_Message = value;
            }
        }

        public bool CodeChanged
        {
            get
            {
                return m_CodeChanged;
            }
            set
            {
                m_CodeChanged = value;
            }
        }

        public string StageTransitionMessage
        {
            get
            {
                return m_StageTransitionMessage;
            }
            set
            {
                m_StageTransitionMessage = value;
            }
        }

        public string Description
        {
            get
            {
                return m_Description;
            }
            set
            {
                m_Description = value;
            }
        }

        public string[] GetAvailableCodeSections
        {
            get { return m_AvailableCodeSections; }
        }

        public int Priority
        {
            get
            {
                return m_Priority;
            }
            set
            {
                // do a complex set where the other activities from the same state have their priorities reset
                m_Priority = value;
            }
        }

        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        public ExternalActivityItem RaiseExternalActivity
        {
            get
            {
                return m_RaiseExternalActivity;
            }
            set
            {
                m_RaiseExternalActivity = value;
            }
        }

        public bool SplitWorkFlow
        {
            get
            {
                return m_CloneFolder;
            }
            set
            {
                m_CloneFolder = value;
            }
        }
        public Guid X2ID
        {
            get
            {
                if (this.x2ID == null || this.x2ID == Guid.Empty)
                {
                    this.x2ID = Guid.NewGuid();
                }
                return this.x2ID;
            }
            set
            {
                this.x2ID = value;
            }
        }
        #endregion Properties

        # region PopUp Menu

        public void populateMenu(GoContextMenu e)
        {
            e.MenuItems.Add(0, new MenuItem("-"));
            e.MenuItems.Add(0, new MenuItem("Move", new EventHandler(this.cmdMove)));
            e.MenuItems.Add(0, new MenuItem("-"));
            e.MenuItems.Add(0, new MenuItem("Paste", new EventHandler(this.cmdPaste)));
            e.MenuItems.Add(0, new MenuItem("Copy", new EventHandler(this.cmdCopy)));
            e.MenuItems.Add(0, new MenuItem("Cut", new EventHandler(this.cmdCut)));
        }

        public void cmdCut(Object sender, EventArgs e)
        {
            MainForm.App.GetCurrentView().Cut_Command(this, e);
            MainForm.App.GetCurrentView().clipBoardHasContents = true;
        }

        public void cmdCopy(Object sender, EventArgs e)
        {
            MainForm.App.GetCurrentView().Copy_Command(this, e);
            MainForm.App.GetCurrentView().clipBoardHasContents = true;
        }

        public void cmdPaste(Object sender, EventArgs e)
        {
            //           MessageBox.Show("Not yet Implemented!","Information");
            MainForm.App.GetCurrentView().Paste_Command(this, e);
            MainForm.App.GetCurrentView().clipBoardHasContents = false; ;
        }

        public void cmdMove(Object sender, EventArgs e)
        {
            List<BaseActivity> mList = new List<BaseActivity>();
            mList.Add(MainForm.App.GetCurrentView().Selection.Primary as BaseActivity);
            frmMoveLink mFormMoveLink = new frmMoveLink(mList);
            mFormMoveLink.ShowDialog();
            mFormMoveLink.Dispose();
        }

        public void OnMenuClosed(GoContextMenu e)
        {
            e.MenuItems[0].Click -= new EventHandler(this.cmdCut);
            e.MenuItems[1].Click -= new EventHandler(this.cmdCopy);
            e.MenuItems[2].Click -= new EventHandler(this.cmdPaste);

            e.MenuItems.RemoveAt(0);
            e.MenuItems.RemoveAt(0);
            e.MenuItems.RemoveAt(0);
        }

        # endregion

        #region IBusinessStageTransitions Members

        #endregion IBusinessStageTransitions Members
    }

    [Serializable]
    public class BaseActivityProperties : BaseProperties
    {
        private int m_CurrentPriority;
        private ExternalActivityItem m_RaiseExternalActivity;

        public BaseActivityProperties(BaseActivity Owner, ProcessDocument pDoc)
            : base(Owner)
        {
            m_CurrentPriority = Owner.Priority;
            m_RaiseExternalActivity = Owner.RaiseExternalActivity;
        }

        [Category(CommonProps.ActivityProps)]
        public string ID
        {
            get
            {
                return ((BaseActivity)m_Owner).Id;
            }
        }

        [Description("Activity Name"), Category(CommonProps.GeneralProps)]
        public string Name
        {
            get
            {
                return ((BaseActivity)m_Owner).Name;
            }
            set
            {
                if (value.Length < 1 || value.Length > 30)
                {
                    MessageBox.Show("The length of the name must be between 1 and 30 characters!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (value.Contains("(") ||
                        value.Contains(")") ||
                        value.Contains("\\") ||
                        value.Contains("/") ||
                        value.Contains("&") ||
                        value.Contains("?") ||
                        value.Contains("%") ||
                        value.Contains("<") ||
                        value.Contains(">") ||
                        value.Contains("+") ||
                        value.Contains("!") ||
                        value.Contains("$") ||
                        value.Contains("@") ||
                        value.Contains("#") ||
                        value.Contains("*") ||
                        value.Contains("{") ||
                        value.Contains("}") ||
                        value.Contains(".") ||
                        value.Contains(",") ||
                        value.Contains("'") ||
                        value.Contains("/") ||
                        value.Contains("|") ||
                        value.Contains(":"))
                {
                    MessageBox.Show("Not updated - there are illegal characters in the name!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                bool found = false;
                if (MainForm.App.GetCurrentView() != null)
                {
                    foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                    {
                        BaseActivity mActivity = o as BaseActivity;
                        if (mActivity != null)
                        {
                            if (mActivity.Name == value)
                            {
                                found = true;
                            }
                        }
                    }
                }
                if (found == false)
                {
                    ((BaseActivity)m_Owner).Name = value;
                }
                else
                {
                    MessageBox.Show("An Activity with this name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        [Description("Indicates the priority of the activity."), Category(CommonProps.ActivityProps)]
        [EditorAttribute(typeof(UiEditors.DropDownListEditorPriority), typeof(UITypeEditor))]
        public int Priority
        {
            get
            {
                return ((BaseActivity)m_Owner).Priority;
            }
            set
            {
                string[] mStr = GetPriorities();
                bool found = false;
                for (int x = 0; x < mStr.Length; x++)
                {
                    if (mStr[x] == value.ToString())
                    {
                        found = true;
                    }
                }
                if (found == true)
                {
                    SetPriorities(m_CurrentPriority, value);
                    ((BaseActivity)m_Owner).Priority = value;
                }
                else
                {
                    MessageBox.Show("Invalid Priority! Please select a valid priority from the dropdown!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [Description("Indicates the external activity to be invoked"), Category(CommonProps.ActivityProps)]
        [EditorAttribute(typeof(UiEditors.ExternalActivityDropDownListEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(UiEditors.ExternalActivityConvertor))]
        public ExternalActivityItem RaiseExternalActivity
        {
            get
            {
                return ((BaseActivity)m_Owner).RaiseExternalActivity;
            }
            set
            {
                if (value != null || UiEditors.ExternalActivityDropDownListEditor.UserHasClicked == true)
                {
                    ((BaseActivity)m_Owner).RaiseExternalActivity = value;
                }
                else
                {
                    if (m_RaiseExternalActivity != null && UiEditors.ExternalActivityDropDownListEditor.UserHasClicked == false)
                    {
                        ((BaseActivity)m_Owner).RaiseExternalActivity = m_RaiseExternalActivity;
                    }
                }
            }
        }

        [Description("Indicates whether to split the workflow into another instance."), Category(CommonProps.ActivityProps)]
        public bool SplitWorkFlow
        {
            get
            {
                return ((BaseActivity)m_Owner).SplitWorkFlow;
            }
            set
            {
                ((BaseActivity)m_Owner).SplitWorkFlow = value;
            }
        }

        [Description("A description of the activity."), Category(CommonProps.ActivityProps)]
        public string Description
        {
            get
            {
                return ((BaseActivity)m_Owner).Description;
            }
            set
            {
                ((BaseActivity)m_Owner).Description = value;
            }
        }

        [Description("Message to be shown on the users WorkList when the activity is performed."), Category(CommonProps.ActivityProps)]
        public string Message
        {
            get
            {
                return ((BaseActivity)m_Owner).Message;
            }
            set
            {
                ((BaseActivity)m_Owner).Message = value;
            }
        }

        [Description("Message to be shown for the StageTransition of the activity."), Category(CommonProps.ActivityProps)]
        public string StageTransitionMessage
        {
            get
            {
                return ((BaseActivity)m_Owner).StageTransitionMessage;
            }
            set
            {
                ((BaseActivity)m_Owner).StageTransitionMessage = value;
            }
        }

        public string[] GetPriorities()
        {
            GoNode fromNode = null;

            foreach (GoLink l in m_Owner.Links)
            {
                if (l.FromNode != m_Owner as GoNode)
                {
                    if (fromNode == null)
                    {
                        fromNode = l.FromNode as GoNode;
                        break;
                    }
                }
                else
                {
                    if (fromNode == null)
                    {
                        break;
                    }
                }
            }
            if (fromNode.GetType() == typeof(CommonState))
            {
                return new string[] { "1" };
            }

            int numActivitiesFromSameState = 0;
            foreach (GoLink l in fromNode.Links)
            {
                if (l.ToNode != fromNode)
                {
                    BaseActivity myParent = l.ToNode as BaseActivity;
                    if (myParent != null)
                    {
                        if (!(myParent is ReturnWorkflowActivity))
                        {
                            numActivitiesFromSameState++;
                        }
                    }
                }
            }
            string[] StrPriority = new string[numActivitiesFromSameState];
            for (int x = 1; x <= numActivitiesFromSameState; x++)
            {
                StrPriority[x - 1] = x.ToString();
            }

            return StrPriority;
        }

        public void SetPriorities(int oldPriority, int newPriority)
        {
            //GoNode fromNode = null;

            //foreach (GoLink l in m_Owner.Links)
            //{
            //    if (l.FromNode != m_Owner as GoNode)
            //    {
            //        if (fromNode == null)
            //        {
            //            fromNode = l.FromNode as GoNode;
            //            break;
            //        }
            //    }
            //}

            //if (fromNode.GetType() == typeof(CommonState))
            //{
            //    foreach (GoLink l in fromNode.Links)
            //    {
            //        BaseActivity a = l.ToPort.Node as BaseActivity;
            //        a.Priority = 1;
            //    }

            //}
            //else
            //{
            //    foreach (GoLink l in fromNode.Links)
            //    {
            //        if (l.ToNode != fromNode && l.FromPort.Node as BaseActivity != m_Owner as BaseActivity)
            //        {
            //            BaseActivity myParent = l.ToPort.Node as BaseActivity;
            //            if (newPriority > oldPriority)
            //            {
            //                if (myParent.Priority <= newPriority && myParent.Priority >= oldPriority && myParent.Priority > 1)
            //                {
            //                    myParent.Priority = myParent.Priority - 1;
            //                }

            //            }
            //            else
            //            {
            //                if (myParent.Priority >= newPriority && myParent.Priority <= oldPriority)
            //                {
            //                    myParent.Priority = myParent.Priority + 1;
            //                }
            //            }
            //        }

            //    }
            //}
        }
    }
}