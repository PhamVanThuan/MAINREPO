using System;
using System.Collections.Generic;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.Items
{
    [Serializable]
    public class ConditionalActivity : BaseActivity, IBusinessStageTransitions
    {
        protected List<BusinessStageItem> m_BusinessStageTransitions;

        public ConditionalActivity(String ID)
        {
            m_Id = ID;
            m_ItemBaseType = WorkflowItemBaseType.Activity;
            m_ItemType = WorkflowItemType.ConditionalActivity;
            m_BusinessStageTransitions = new List<BusinessStageItem>();
            m_IconName = "decision_activity_32x32x32b";
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

        public override object Properties
        {
            get
            {
                return new ConditionalActivityProperties(this, MainForm.App.GetCurrentView().Document);
            }
        }

        public override void Copy(BaseItem newItem)
        {
            ConditionalActivity mConditionalActivity = newItem as ConditionalActivity;
            if (mConditionalActivity != null)
            {
                for (int x = 0; x < this.GetAvailableCodeSections.Length; x++)
                {
                    string codeSection = this.GetAvailableCodeSections[x];
                    string codeSectionData = this.GetCodeSectionData(codeSection);
                    mConditionalActivity.SetCodeSectionData(codeSection, codeSectionData);
                }
                ExternalActivityItem mExternalActivityItem = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.ReturnMatchingExternalActivityItem(this.RaiseExternalActivity);
                if (mExternalActivityItem != null)
                {
                    mConditionalActivity.RaiseExternalActivity = mExternalActivityItem;
                }
                mConditionalActivity.SplitWorkFlow = this.SplitWorkFlow;
                mConditionalActivity.Message = this.Message;
                mConditionalActivity.Priority = this.Priority;
            }
        }

        public override bool HasStageTransitions()
        {
            return m_BusinessStageTransitions.Count > 0 ? true : false;
        }
    }

    [Serializable]
    public class ConditionalActivityProperties : BaseActivityProperties
    {
        private string[] StrRaiseExternalActivity = { "None" };
        private List<BusinessStageItem> m_BusinessStageTransitions;

        public ConditionalActivityProperties(ConditionalActivity Owner, ProcessDocument pDoc)
            : base(Owner, pDoc)
        {
            m_BusinessStageTransitions = Owner.BusinessStageTransitions;
        }
    }
}