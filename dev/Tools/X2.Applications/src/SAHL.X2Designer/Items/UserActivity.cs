using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Forms;
using SAHL.X2Designer.Misc;
using SAHL.X2Designer.UiEditors;

namespace SAHL.X2Designer.Items
{
    /// <summary>
    /// Has one inport and one outport that connects between 2 states
    /// </summary>
    ///
    [Serializable]
    public class UserActivity : BaseActivity, ISecurity_HasAccessList, IBusinessStageTransitions
    {
        private bool m_UseLinkedActivity;
        private CustomFormItem m_CustomForm;
        protected RoleInstanceCollection m_Access;
        private BaseActivity m_LinkedActivity;
        protected List<BusinessStageItem> m_BusinessStageTransitions;
        private int _WorkListOrder = -1;

        public UserActivity(String ID)
        {
            m_Id = ID;
            m_ItemBaseType = WorkflowItemBaseType.Activity;
            m_ItemType = WorkflowItemType.UserActivity;
            m_IconName = "user_activity_1_32x32x32b";
            m_Access = new RoleInstanceCollection();
            m_BusinessStageTransitions = new List<BusinessStageItem>();

            this.Access.RefreshRoles();
        }

        #region Properties

        public bool UseLinkedActivity
        {
            get
            {
                return m_UseLinkedActivity;
            }
            set
            {
                m_UseLinkedActivity = value;
            }
        }

        public CustomFormItem CustomForm
        {
            get
            {
                return m_CustomForm;
            }
            set
            {
                m_CustomForm = value;
            }
        }

        public BaseActivity LinkedActivity
        {
            get
            {
                return m_LinkedActivity;
            }
            set
            {
                m_LinkedActivity = value;
            }
        }

        public RoleInstanceCollection Access
        {
            get
            {
                return m_Access;
            }
            set
            {
                m_Access = value;
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
                return new UserActivityProperties(this, MainForm.App.GetCurrentView().Document, m_CustomForm);
            }
        }

        public override void Copy(BaseItem newItem)
        {
            UserActivity mUserActivity = newItem as UserActivity;
            if (mUserActivity != null)
            {
                foreach (RoleInstance i in this.Access)
                {
                    RoleInstance mInstance = new RoleInstance();
                    mInstance.IsChecked = i.IsChecked;
                    mInstance.RoleItem = i.RoleItem;
                    mUserActivity.Access.Add(mInstance);
                }

                ExternalActivityItem mExternalActivityItem = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.ReturnMatchingExternalActivityItem(this.RaiseExternalActivity);
                if (mExternalActivityItem != null)
                {
                    mUserActivity.RaiseExternalActivity = mExternalActivityItem;
                }
                mUserActivity.SplitWorkFlow = this.SplitWorkFlow;
                mUserActivity.Message = this.Message;
                mUserActivity.UseLinkedActivity = this.UseLinkedActivity;
                if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CheckIfItemExists(LinkedActivity as BaseItem))
                {
                    mUserActivity.LinkedActivity = this.LinkedActivity;
                }
                mUserActivity.Priority = this.Priority;
            }
        }

        #endregion Overrides

        public override bool HasStageTransitions()
        {
            return m_BusinessStageTransitions.Count > 0 ? true : false;
        }
    }

    [Serializable]
    public class UserActivityProperties : BaseActivityProperties
    {
        //        private string[] strCustomForm;
        private static CustomFormItem m_OriginalCustomForm;
        private List<BusinessStageItem> m_BusinessStageTransitions;

        public UserActivityProperties(UserActivity Owner, ProcessDocument pDoc, CustomFormItem originalCustomForm)
            : base(Owner, pDoc)
        {
            m_OriginalCustomForm = originalCustomForm;
            m_BusinessStageTransitions = Owner.BusinessStageTransitions;
        }

        [Description("Indicates the ExternalActivity to be raised when the activity is performed."), Category(CommonProps.ActivityProps)]
        [EditorAttribute(typeof(UiEditors.CustomFormDropDownEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(UiEditors.CustomFormConvertor))]
        public CustomFormItem CustomForm
        {
            get
            {
                CustomFormItem mCustomForm = ((UserActivity)m_Owner).CustomForm;
                if (mCustomForm != null || UiEditors.CustomFormDropDownEditor.UserHasClicked == true)
                {
                    m_OriginalCustomForm = mCustomForm;
                    return mCustomForm;
                }
                else
                {
                    if (UiEditors.CustomFormDropDownEditor.UserHasClicked == false)
                    {
                        return m_OriginalCustomForm;
                    }
                    else
                    {
                        return mCustomForm;
                    }
                }
            }
            set
            {
                ((UserActivity)m_Owner).CustomForm = value;
            }
        }

        [Description("Use Linked Activity"), Category(CommonProps.LinkedActivityProps)]
        public bool UseLinkedActivity
        {
            get
            {
                return ((UserActivity)m_Owner).UseLinkedActivity;
            }
            set
            {
                ((UserActivity)m_Owner).UseLinkedActivity = value;
                if (value == false)
                {
                    if (((UserActivity)m_Owner).LinkedActivity != null)
                    {
                        ((UserActivity)m_Owner).LinkedActivity = null;
                    }
                }
            }
        }

        //[Description("Select Business Stage Transitions"), Category(CommonProps.ActivityProps)]
        //[EditorAttribute(typeof(UiEditors.BusinessStageTransitionsEditor), typeof(UITypeEditor))]
        //[TypeConverter(typeof(UiEditors.BusinessStageTransitionsConvertor))]
        //public List<BusinessStageItem> BusinessStageTransitions
        //{
        //    get
        //    {
        //        return ((UserActivity)m_Owner).BusinessStageTransitions;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            ((UserActivity)m_Owner).BusinessStageTransitions = value;
        //        }
        //        else
        //        {
        //            ((UserActivity)m_Owner).BusinessStageTransitions = m_BusinessStageTransitions;
        //        }
        //    }
        //}

        [Description("Indicates the Linked Activity"), Category(CommonProps.LinkedActivityProps)]
        [EditorAttribute(typeof(UiEditors.LinkedActivityDropDownEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(UiEditors.LinkedActivityConvertor))]
        public BaseActivity LinkedActivity
        {
            get
            {
                LinkedActivityDropDownEditor.mActivity = m_Owner as UserActivity;
                return ((UserActivity)m_Owner).LinkedActivity;
            }
            set
            {
                if (value != null || UiEditors.LinkedActivityDropDownEditor.UserHasClicked == true)
                {
                    ((UserActivity)m_Owner).LinkedActivity = value;
                }
                else
                {
                    if (this.LinkedActivity != null || UiEditors.LinkedActivityDropDownEditor.UserHasClicked == false)
                    {
                        ((UserActivity)m_Owner).LinkedActivity = this.LinkedActivity;
                    }
                }
            }
        }

        [Description("Edits the Access list."), Category(CommonProps.SecurityProps)]
        [Editor(typeof(ManageRolesEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(CheckedListConvertor))]
        public RoleInstanceCollection Access
        {
            get
            {
                return ((UserActivity)m_Owner).Access;
            }
        }
    }
}