using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.Misc
{
    [Serializable]
    public class CustomVariableItem : IWorkflowItem
    {
        String m_Name = "";
        String m_Description = "";
        CustomVariableType m_Type;
        int m_Length;

        public CustomVariableItem()
        {
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

        [TypeConverter(typeof(CustomVariableTypeTypeConvertor))]
        public CustomVariableType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        public int Length
        {
            get { return m_Length; }
            set { m_Length = value; }
        }

        #region IWorkflowItem Members

        public object Properties
        {
            get { return new CustomVariableItemProperties(this); }
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

        public void Copy(BaseItem newItem)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UpdateCodeSectionData(string OldValue, string NewValue)
        {
        }

        public string RefreshCodeSectionData(string CodeSectionName)
        {
            return "";
        }

        //        public event OnWorkFlowItemNameChangedHandler OnItemNameChanged;

        public WorkflowItemBaseType WorkflowItemBaseType
        {
            get { return WorkflowItemBaseType.CustomVariable; }
        }

        public WorkflowItemType WorkflowItemType
        {
            get { return WorkflowItemType.CustomVariable; }
        }

        public List<string> ValidationErrors
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void Validate()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion IWorkflowItem Members

        #region IComparable Members

        public int CompareTo(object x)
        {
            String mName = x.ToString();
            return string.Compare(this.Name, mName);
        }

        #endregion IComparable Members
    }

    public enum CustomVariableType
    {
        ctstring,
        ctinteger,
        ctdate,
        ctdecimal,
        ctbool,
        ctfloat,
        ctdouble,
        ctbigstring,
        ctlong,
        none
    }

    [Serializable]
    public class CustomVariableItemProperties
    {
        CustomVariableItem m_Owner;
        string holdType = "";

        public CustomVariableItemProperties(CustomVariableItem Owner)
        {
            m_Owner = Owner;
        }

        [Description(""), Category(CommonProps.GeneralProps)]
        public String Name
        {
            get
            {
                return ((CustomVariableItem)m_Owner).Name;
            }
            set
            {
                bool found = false;
                for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CustomVariables.Count; x++)
                {
                    if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CustomVariables[x].Name == value.ToString())
                    {
                        found = true;
                    }
                }
                if (found == false)
                {
                    ((CustomVariableItem)m_Owner).Name = value;
                }
                else
                {
                    MessageBox.Show("A custom variable of this name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        [ReadOnly(true), TypeConverter(typeof(CustomVariableTypeTypeConvertor))]
        public CustomVariableType Type
        {
            get
            {
                return ((CustomVariableItem)m_Owner).Type;
            }
            set
            {
                ((CustomVariableItem)m_Owner).Type = value;
                holdType = value.ToString().ToLower();
                if (value.ToString().ToLower() == "ctstring")
                {
                    ((CustomVariableItem)m_Owner).Length = 255;
                }
                else
                {
                    ((CustomVariableItem)m_Owner).Length = 0;
                }
            }
        }

        public int Length
        {
            get
            {
                return ((CustomVariableItem)m_Owner).Length;
            }
            set
            {
                if (holdType == "ctstring")
                {
                    if (value > 0 && value < 256)
                    {
                        ((CustomVariableItem)m_Owner).Length = value;
                    }
                    else
                    {
                        MessageBox.Show("Length must be between 1 and 255 characters!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        ((CustomVariableItem)m_Owner).Length = 255;
                    }
                }
                else
                {
                    MessageBox.Show("Length can only be set for variable type String!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ((CustomVariableItem)m_Owner).Length = 0;
                }
            }
        }
    }

    public class CustomVariableTypeTypeConvertor : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;
            else
                return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value is CustomVariableType)
            {
                CustomVariableType CTV = (CustomVariableType)value;
                if (destinationType == typeof(string))
                {
                    switch (CTV)
                    {
                        case CustomVariableType.ctbool:
                            return "bool";
                        case CustomVariableType.ctdate:
                            return "date";
                        case CustomVariableType.ctdecimal:
                            return "decimal";
                        case CustomVariableType.ctinteger:
                            return "integer";
                        case CustomVariableType.ctstring:
                            return "string";
                        case CustomVariableType.ctbigstring:
                            return "text";
                        case CustomVariableType.ctdouble:
                            return "double";
                        case CustomVariableType.ctfloat:
                            return "single";
                        case CustomVariableType.ctlong:
                            return "biginteger";
                    }
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            else
                return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            try
            {
                if (value is string)
                {
                    string val = (string)value;
                    switch (val)
                    {
                        case "bool":
                            return CustomVariableType.ctbool;
                        case "date":
                            return CustomVariableType.ctdate;
                        case "decimal":
                            return CustomVariableType.ctdecimal;
                        case "integer":
                            return CustomVariableType.ctinteger;
                        case "string":
                            return CustomVariableType.ctstring;
                        case "text":
                            return CustomVariableType.ctbigstring;
                        case "double":
                            return CustomVariableType.ctdouble;
                        case "single":
                            return CustomVariableType.ctfloat;
                        case "biginteger":
                            return CustomVariableType.ctlong;
                    }
                }
                return base.ConvertFrom(context, culture, value);
            }
            catch
            {
                return null;
            }
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            ArrayList AL = new ArrayList();
            AL.Add(CustomVariableType.ctbool);
            AL.Add(CustomVariableType.ctstring);
            AL.Add(CustomVariableType.ctbigstring);
            AL.Add(CustomVariableType.ctdate);
            AL.Add(CustomVariableType.ctinteger);
            AL.Add(CustomVariableType.ctlong);
            AL.Add(CustomVariableType.ctdecimal);
            AL.Add(CustomVariableType.ctfloat);
            AL.Add(CustomVariableType.ctdouble);
            StandardValuesCollection SVC = new StandardValuesCollection(AL);
            return SVC;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}