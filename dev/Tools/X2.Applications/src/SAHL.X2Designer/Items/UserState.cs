using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using SAHL.X2Designer.Forms;
using SAHL.X2Designer.Misc;
using SAHL.X2Designer.UiEditors;

namespace SAHL.X2Designer.Items
{
    /// <summary>
    /// 1) WorkList
    /// </summary>
    ///
    [Serializable]
    public class UserState : BaseStateWithLists
    {
        private List<CustomFormItem> m_CustomForms;

        public UserState()
        {
            m_ItemBaseType = WorkflowItemBaseType.State;
            m_ItemType = WorkflowItemType.UserState;
            m_IconName = "user_state_1_32x32x32b";
            m_CustomForms = new List<CustomFormItem>();
        }

        #region properties

        public List<CustomFormItem> CustomForms
        {
            get
            {
                return m_CustomForms;
            }
            set
            {
                CustomForms = value;
            }
        }

        #endregion properties

        #region Overrides

        public override void Copy(BaseItem newItem)
        {
            UserState mUserState = newItem as UserState;
            if (mUserState != null)
            {
                foreach (RoleInstance i in this.WorkList)
                {
                    RoleInstance mInstance = new RoleInstance();
                    mInstance.IsChecked = i.IsChecked;
                    mInstance.RoleItem = i.RoleItem;
                    mUserState.WorkList.Add(mInstance);
                }
                foreach (RoleInstance i in this.TrackList)
                {
                    RoleInstance mInstance = new RoleInstance();
                    mInstance.IsChecked = i.IsChecked;
                    mInstance.RoleItem = i.RoleItem;
                    mUserState.TrackList.Add(mInstance);
                }

                foreach (CustomFormItem i in this.CustomForms)
                {
                    CustomFormItem mCustomForm = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.ReturnMatchingCustomForm(i);
                    {
                        if (mCustomForm != null)
                        {
                            mUserState.CustomForms.Add(mCustomForm);
                        }
                    }
                }
            }
            for (int x = 0; x < base.GetAvailableCodeSectionsInternal().Length; x++)
            {
                string codeSection = GetAvailableCodeSectionsInternal()[x];
                mUserState.SetCodeSectionData(codeSection, GetCodeSectionData(codeSection));
            }
        }

        public override object Properties
        {
            get
            {
                return new UserStateProperties(this, MainForm.App.GetCurrentView().Document);
            }
        }

        #endregion Overrides
    }

    [Serializable]
    public class UserStateProperties : BaseStateWithListsProperties
    {
        private List<CustomFormItem> m_CustomForms;

        public UserStateProperties(BaseState Owner, Documents.ProcessDocument pDoc)
            : base(Owner, pDoc)
        {
            m_CustomForms = new List<CustomFormItem>();
        }

        [Description("Custom Forms"), Category(CommonProps.SecurityProps)]
        [Editor(typeof(CustomFormPopUpEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(CheckedListConvertor))]
        public List<CustomFormItem> CustomForms
        {
            get
            {
                return ((UserState)m_Owner).CustomForms;
            }
        }
    }
}