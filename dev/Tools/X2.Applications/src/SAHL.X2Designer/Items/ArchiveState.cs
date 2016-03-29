using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.Text.RegularExpressions;
using SAHL.X2Designer.CodeGen;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.UiEditors;

namespace SAHL.X2Designer.Items
{
    /// <summary>
    /// 1) WorkList
    /// </summary>
    public class ArchiveState : BaseState
    {
        internal static string OnReturn = "OnReturn";
        protected string CodeOnReturn = "";
        internal WorkFlow _Host;
        private WorkFlow _WorkFlowToReturnTo;
        private ReturnWorkflowActivity _ReturnActivity;

        public ArchiveState(WorkFlow Host)
        {
            m_AvailableCodeSections = new string[] { OnEnter, OnExit, OnReturn };
            m_ItemBaseType = WorkflowItemBaseType.State;
            m_ItemType = WorkflowItemType.ArchiveState;
            m_IconName = "archive_state_32x32x32b";
            _Host = Host;
        }

        public override object Properties
        {
            get
            {
                return new ArchiveProperties(this, MainForm.App.GetCurrentView().Document);
            }
        }

        public override string GetCodeSectionData(string CodeSectionName)
        {
            StringBuilder SB = new StringBuilder();
            if (CodeSectionName == OnReturn)
            {
                if (CodeOnReturn == "")
                {
                    // generate the header
                    SB.Append(X2Generator.GenerateReturnHeader(this, MainForm.App.GetCurrentView().Document));
                    SB.AppendLine("\treturn true;");
                    SB.AppendLine("}");
                    CodeOnReturn = SB.ToString();
                }
                return CodeOnReturn;
            }
            else
                return base.GetCodeSectionData(CodeSectionName);
        }

        public override string RefreshCodeSectionData(string CodeSectionName)
        {
            StringBuilder SB = new StringBuilder();
            if (CodeSectionName == OnReturn)
            {
                if (CodeSectionName != "")
                {
                    CodeSectionName = "";
                }
                // generate the header
                SB.Append(X2Generator.GenerateReturnHeader(this, MainForm.App.GetCurrentView().Document));
                SB.AppendLine("return new X2ReturnData(null, true);");
                SB.AppendLine("}");
                CodeSectionName = SB.ToString();
                return CodeSectionName;
            }
            else
                return base.RefreshCodeSectionData(CodeSectionName);
        }

        public override void SetCodeSectionData(string CodeSectionName, string SectionData)
        {
            if (CodeSectionName == OnReturn)
            {
                CodeOnReturn = SectionData;
            }
            else
                base.SetCodeSectionData(CodeSectionName, SectionData);
        }

        public override void UpdateCodeSectionData(string OldValue, string NewValue)
        {
            base.UpdateCodeSectionData(OldValue, NewValue);
            if (NewValue.Length > 0 && OldValue.Length > 0)
            {
                Regex MCN = new Regex("\\b" + OldValue + "\\b");
                CodeOnReturn = MCN.Replace(CodeOnReturn, NewValue);
                X2Generator.ReplaceReturnHeader(OldValue, NewValue, ref CodeOnReturn);
            }
        }

        public WorkFlow WorkflowToReturnTo
        {
            get { return _WorkFlowToReturnTo; }
            set { _WorkFlowToReturnTo = value; }
        }

        public ReturnWorkflowActivity ReturnActivity
        { get { return _ReturnActivity; } set { _ReturnActivity = value; } }
    }

    [Serializable]
    public class ArchiveProperties : NamedStateProperties
    {
        ArchiveState mOwner;
        public WorkFlow _ReturnWorkflow;
        public ReturnWorkflowActivity _ActivityToReturnTo;

        public ArchiveProperties(ArchiveState Owner, ProcessDocument pDoc)
            : base(Owner, pDoc)
        {
            mOwner = Owner;
            WorkFlow Current = pDoc.CurrentWorkFlow;
            WorkFlow[] All = pDoc.WorkFlows;
            for (int i = 0; i < All.Length; i++)
            {
                if (Current.WorkFlowName != All[i].WorkFlowName)
                {
                    _ReturnWorkflow = All[i];
                    break;
                }
            }
        }

        [Description("Indicates the workflow to which the action must be linked."), Category(CommonProps.ActivityProps)]
        [EditorAttribute(typeof(UiEditors.WorkFlowToCallDropDownEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(UiEditors.WorkFlowToCallConvertor))]
        public WorkFlow WorkFlowToReturnTo
        {
            get
            {
                return ((ArchiveState)m_Owner).WorkflowToReturnTo;
            }
            set
            {
                if (value != null)
                {
                    ((ArchiveState)m_Owner).WorkflowToReturnTo = value;
                    _ReturnWorkflow = value;
                }
                else
                {
                    if (_ReturnWorkflow != null)
                    {
                        ((ArchiveState)m_Owner).WorkflowToReturnTo = _ReturnWorkflow;
                    }
                }
                ReturnActivityConvertor.workFlowToSearch = _ReturnWorkflow;
            }
        }

        [Description("Indicates which action must be called on the selected workflow."), Category(CommonProps.ActivityProps)]
        [EditorAttribute(typeof(UiEditors.ReturnActivityConvertor), typeof(UITypeEditor))]
        [TypeConverter(typeof(UiEditors.ExternalActivityConvertor))]
        public ReturnWorkflowActivity ActivityToReturnTo
        {
            get
            {
                UiEditors.ActivityToCallDropDownEditor.workFlowToSearch = _ReturnWorkflow;
                if (null != ((ArchiveState)m_Owner).ReturnActivity)
                    return ((ArchiveState)m_Owner).ReturnActivity;
                return null;
            }
            set
            {
                if (value != null)
                {
                    ((ArchiveState)m_Owner).ReturnActivity = value;
                }
                else
                {
                    if (_ActivityToReturnTo != null)
                    {
                        ((ArchiveState)m_Owner).ReturnActivity = _ActivityToReturnTo;
                    }
                }
            }
        }
    }
}