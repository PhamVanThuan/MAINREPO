using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using SAHL.X2Designer.Forms;
using SAHL.X2Designer.Items;
using SAHL.X2Designer.UiEditors;

namespace SAHL.X2Designer.Misc
{
    public class CustomFormComparer : IComparer<CustomFormItem>
    {
        public int Compare(CustomFormItem x, CustomFormItem y)
        {
            return string.Compare(x.Name, y.Name);
        }
    }

    [Serializable]
    public class CustomFormItem : IWorkflowItem
    {
        String m_Name = "";
        String m_Description = "";
        protected CustomFormAppliedTo _AppliesTo;

        public CustomFormItem()
        {
            _AppliesTo = new CustomFormAppliedTo();
        }

        #region Properties

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

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public CustomFormAppliedTo AppliesTo
        {
            get
            {
                return _AppliesTo;
            }
            set
            {
                _AppliesTo = value;
            }
        }

        public WorkflowItemBaseType WorkflowItemBaseType
        {
            get { return WorkflowItemBaseType.CustomForm; }
        }

        public WorkflowItemType WorkflowItemType
        {
            get { return WorkflowItemType.CustomForm; }
        }

        public List<string> ValidationErrors
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void Validate()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object Properties
        {
            get { return new CustomFormItemProperties(this); }
        }

        public string[] AvailableCodeSections
        {
            get { return new string[] { }; }
        }

        public string GetCodeSectionData(string CodeSectionName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetCodeSectionData(string CodeSectionName, string SectionData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UpdateCodeSectionData(string OldValue, string NewValue)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string RefreshCodeSectionData(string CodeSectionName)
        {
            return "";
        }

        public void Copy(BaseItem newItem)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion Properties

        #region IComparable Members

        public int CompareTo(object x)
        {
            String mName = x.ToString();
            return string.Compare(this.Name, mName);
        }

        #endregion IComparable Members
    }

    [Serializable]
    public class CustomFormItemProperties
    {
        CustomFormItem m_Owner;

        public CustomFormItemProperties(CustomFormItem Owner)
        {
            m_Owner = Owner;
        }

        [Description(""), Category(CommonProps.GeneralProps)]
        public String Name
        {
            get
            {
                return ((CustomFormItem)m_Owner).Name;
            }
            set
            {
                ((CustomFormItem)m_Owner).Name = value;
            }
        }

        //      [Description(""),Category(CommonProps.CustomFormProps)]
        public String Description
        {
            get
            {
                return ((CustomFormItem)m_Owner).Description;
            }
            set
            {
                ((CustomFormItem)m_Owner).Description = value;
            }
        }

        [Description("Edits the Applies To list."), Category(CommonProps.ActivityProps)]
        [Editor(typeof(ManageFormAppliesToEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(CheckedListConvertor))]
        public CustomFormItem AppliesTo
        {
            get
            {
                return ((CustomFormItem)m_Owner);
            }
        }
    }
}