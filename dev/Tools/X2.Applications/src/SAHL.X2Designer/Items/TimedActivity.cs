using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SAHL.X2Designer.CodeGen;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.Items
{
    [Serializable]
    public class TimedActivity : BaseActivity, IBusinessStageTransitions
    {
        protected string m_OnTimedActivity = "";
        protected List<BusinessStageItem> m_BusinessStageTransitions;

        public TimedActivity(String ID)
        {
            m_Id = ID;
            m_ItemBaseType = WorkflowItemBaseType.Activity;
            m_ItemType = WorkflowItemType.TimedActivity;
            m_BusinessStageTransitions = new List<BusinessStageItem>();
            m_IconName = "timer_activity_32x32x32b";
            base.m_AvailableCodeSections = new string[] { OnStart, OnComplete, OnStageActivity, "OnTimedActivity" };
        }

        public List<BusinessStageItem> BusinessStageTransitions
        {
            get
            {
                return m_BusinessStageTransitions;
            }
            set
            {
                m_BusinessStageTransitions = value;
            }
        }

        List<BusinessStageItem> IBusinessStageTransitions.BusinessStageTransitions
        {
            get { return m_BusinessStageTransitions; }
            set
            {
                m_BusinessStageTransitions = value;
                if (m_BusinessStageTransitions.Count == 0)
                    m_StageTransition = YouCantHaveNiceThings;

                if (m_StageTransition == YouCantHaveNiceThings)
                    m_StageTransition = "";
            }
        }

        #region Overrides

        public override string GetCodeSectionData(string CodeSectionName)
        {
            StringBuilder SB = new StringBuilder();
            if (CodeSectionName == "OnTimedActivity")
            {
                if (m_OnTimedActivity == "")
                {
                    // generate the header
                    SB.Append(X2Generator.GenerateActivityTimerHeader(this, MainForm.App.GetCurrentView().Document));
                    // add the actual code
                    SB.AppendLine("\treturn DateTime.Now;");
                    SB.AppendLine("}");
                    m_OnTimedActivity = SB.ToString();
                }
                return m_OnTimedActivity;
            }
            else
                return base.GetCodeSectionData(CodeSectionName);
        }

        public override void SetCodeSectionData(string CodeSectionName, string SectionData)
        {
            if (CodeSectionName == "OnTimedActivity")
            {
                m_OnTimedActivity = SectionData;
            }
            else
                base.SetCodeSectionData(CodeSectionName, SectionData);
        }

        public override void UpdateCodeSectionData(string OldValue, string NewValue)
        {
            if (NewValue.Length > 0 && OldValue.Length > 0)
            {
                base.UpdateCodeSectionData(OldValue, NewValue);

                Regex MCN = new Regex("\\b" + OldValue + "\\b");

                m_OnTimedActivity = MCN.Replace(m_OnTimedActivity, NewValue);
                X2Generator.ReplaceActivityTimerHeader(OldValue, NewValue, ref m_OnTimedActivity);
            }
        }

        public override object Properties
        {
            get
            {
                return new TimedActivityProperties(this, MainForm.App.GetCurrentView().Document);
            }
        }

        public override void Copy(BaseItem newItem)
        {
            TimedActivity mTimedActivity = newItem as TimedActivity;
            if (mTimedActivity != null)
            {
                for (int x = 0; x < this.GetAvailableCodeSections.Length; x++)
                {
                    string codeSection = this.GetAvailableCodeSections[x];
                    string codeSectionData = this.GetCodeSectionData(codeSection);
                    mTimedActivity.SetCodeSectionData(codeSection, codeSectionData);
                }
                ExternalActivityItem mExternalActivity = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.ReturnMatchingExternalActivityItem(this.RaiseExternalActivity);
                if (mExternalActivity != null)
                {
                    mTimedActivity.RaiseExternalActivity = mExternalActivity;
                }
                mTimedActivity.SplitWorkFlow = this.SplitWorkFlow;
                mTimedActivity.Message = this.Message;
                mTimedActivity.Priority = this.Priority;
            }
        }

        #endregion Overrides

        public override bool HasStageTransitions()
        {
            return m_BusinessStageTransitions.Count > 0 ? true : false;
        }
    }

    [Serializable]
    public class TimedActivityProperties : BaseActivityProperties
    {
        private List<BusinessStageItem> m_BusinessStageTransitions;

        public TimedActivityProperties(TimedActivity Owner, ProcessDocument pDoc)
            : base(Owner, pDoc)
        {
            m_BusinessStageTransitions = Owner.BusinessStageTransitions;
        }
    }
}