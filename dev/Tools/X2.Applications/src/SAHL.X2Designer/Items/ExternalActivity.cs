using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.Items
{
    [Serializable]
    public class ExternalActivity : BaseActivity, IBusinessStageTransitions
    {
        private ExternalActivityItem m_InvokedBy;
        private string m_InvokeOnInstance;
        protected List<BusinessStageItem> m_BusinessStageTransitions;

        public ExternalActivity(String ID)
        {
            m_Id = ID;
            m_ItemBaseType = WorkflowItemBaseType.Activity;
            m_ItemType = WorkflowItemType.ExternalActivity;
            m_BusinessStageTransitions = new List<BusinessStageItem>();
            m_IconName = "external_activity_32x32x32b";
        }

        #region Properties

        public ExternalActivityItem InvokedBy
        {
            get
            {
                return m_InvokedBy;
            }

            set
            {
                m_InvokedBy = value;
            }
        }

        public string InvokeOnInstanceTarget
        {
            get
            {
                return m_InvokeOnInstance;
            }
            set
            {
                m_InvokeOnInstance = value;
            }
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

        #endregion Properties

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

        public override object Properties
        {
            get
            {
                return new ExternalActivityProperties(this, MainForm.App.GetCurrentView().Document);
            }
        }

        public override void Copy(BaseItem newItem)
        {
            ExternalActivity mExternalActivity = newItem as ExternalActivity;
            if (mExternalActivity != null)
            {
                for (int x = 0; x < this.GetAvailableCodeSections.Length; x++)
                {
                    string codeSection = this.GetAvailableCodeSections[x];
                    string codeSectionData = this.GetCodeSectionData(codeSection);
                    mExternalActivity.SetCodeSectionData(codeSection, codeSectionData);
                }
                ExternalActivityItem mExternalActivityItem = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.ReturnMatchingExternalActivityItem(this.RaiseExternalActivity);
                if (mExternalActivityItem != null)
                {
                    mExternalActivity.RaiseExternalActivity = mExternalActivityItem;
                }
                mExternalActivity.SplitWorkFlow = this.SplitWorkFlow;
                mExternalActivity.Message = this.Message;
                mExternalActivity.Priority = this.Priority;
                ExternalActivityItem mInvokedBy = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.ReturnMatchingExternalActivityItem(this.InvokedBy);
                if (mInvokedBy != null)
                {
                    mExternalActivity.InvokedBy = mInvokedBy;
                }
                mExternalActivity.InvokeOnInstanceTarget = this.InvokeOnInstanceTarget;
            }
        }

        #endregion Overrides

        public override bool HasStageTransitions()
        {
            return m_BusinessStageTransitions.Count > 0 ? true : false;
        }
    }

    [Serializable]
    public class ExternalActivityProperties : BaseActivityProperties
    {
        private string[] StrInvokeOnInstanceTarget;
        private string CurrentRaiseOnInstanceTarget = " ";
        private ExternalActivityItem m_InvokedBy = null;
        private List<BusinessStageItem> m_BusinessStageTransitions;

        public ExternalActivityProperties(ExternalActivity Owner, ProcessDocument pDoc)
            : base(Owner, pDoc)
        {
            CurrentRaiseOnInstanceTarget = Owner.InvokeOnInstanceTarget;
            m_BusinessStageTransitions = Owner.BusinessStageTransitions;
            m_InvokedBy = Owner.InvokedBy;
            StrInvokeOnInstanceTarget = new string[4];
            StrInvokeOnInstanceTarget[0] = "Any Instance";
            StrInvokeOnInstanceTarget[1] = "This Instance";
            StrInvokeOnInstanceTarget[2] = "Parent Instance";
            StrInvokeOnInstanceTarget[3] = "Child Instance";
        }

        [Description("Indicates the external activity to be invoked"), Category(CommonProps.ActivityProps)]
        [EditorAttribute(typeof(UiEditors.ExternalActivityDropDownListEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(UiEditors.ExternalActivityConvertor))]
        public ExternalActivityItem InvokedBy
        {
            get
            {
                return ((ExternalActivity)m_Owner).InvokedBy;
            }
            set
            {
                if (value != null || UiEditors.ExternalActivityDropDownListEditor.UserHasClicked == true)
                {
                    ((ExternalActivity)m_Owner).InvokedBy = value;
                    m_InvokedBy = value;
                }
                else
                {
                    if (UiEditors.ExternalActivityDropDownListEditor.UserHasClicked == false)
                    {
                        ((ExternalActivity)m_Owner).InvokedBy = m_InvokedBy;
                    }
                }
            }
        }

        [Description("Indicates the external activity to be raised when the activity is performed."), Category(CommonProps.ActivityProps)]
        [EditorAttribute(typeof(UiEditors.DropDownListEditor), typeof(UITypeEditor))]
        public string InvokeOnInstanceTarget
        {
            get
            {
                UiEditors.DropDownListEditor.strList = StrInvokeOnInstanceTarget;
                return ((ExternalActivity)m_Owner).InvokeOnInstanceTarget;
            }
            set
            {
                if (value != null || UiEditors.DropDownListEditor.UserHasClicked == true)
                {
                    ((ExternalActivity)m_Owner).InvokeOnInstanceTarget = value;
                    CurrentRaiseOnInstanceTarget = value;
                }
                else
                {
                    if (UiEditors.DropDownListEditor.UserHasClicked == false)
                    {
                        ((ExternalActivity)m_Owner).InvokeOnInstanceTarget = CurrentRaiseOnInstanceTarget;
                    }
                }
            }
        }
    }
}