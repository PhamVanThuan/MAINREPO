using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SAHL.Common.UI.Configuration
{
    #region CBOSecurityFilterSection

    public class CBOSecurityFilterSection : ConfigurationSection
    {
        [ConfigurationProperty("CBOSecurityFilters", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CBOSecurityFilterElementCollection))]
        public CBOSecurityFilterElementCollection CBOSecurityFilters
        {
            get
            {
                CBOSecurityFilterElementCollection _CBOSecurityFilters =
                (CBOSecurityFilterElementCollection)base["CBOSecurityFilters"];
                return _CBOSecurityFilters;
            }
        }
    }

    #endregion

    #region CBOSecurityFilterElement

    public class CBOSecurityFilterElement : ConfigurationElement
    {
        public CBOSecurityFilterElement()
        {

        }

        public CBOSecurityFilterElement(int p_ID, string p_ProcessName, string p_WorkflowName, string p_ClassType)
        {
            ID = p_ID;
            ProcessName = p_ProcessName;
            WorkflowName = p_WorkflowName;
            ClassType = p_ClassType;
        }

        [ConfigurationProperty("ID", DefaultValue = -1, IsRequired = true, IsKey = true)]
        public int ID
        {
            get
            { return (int)this["ID"]; }
            set
            { this["ID"] = value; }

        }

        [ConfigurationProperty("ProcessName", DefaultValue = "Process", IsRequired = true)]
        [StringValidator(InvalidCharacters = "", MinLength = 1, MaxLength = 250)]
        public string ProcessName
        {
            get
            { return (String)this["ProcessName"]; }
            set
            { this["ProcessName"] = value; }
        }

        [ConfigurationProperty("WorkflowName", DefaultValue = "Workflow", IsRequired = true)]
        [StringValidator(InvalidCharacters = "", MinLength = 1, MaxLength = 250)]
        public string WorkflowName
        {
            get
            { return (String)this["WorkflowName"]; }
            set
            { this["WorkflowName"] = value; }
        }

        [ConfigurationProperty("ClassType", DefaultValue = "ClassType", IsRequired = true)]
        [StringValidator(InvalidCharacters = "", MinLength = 1, MaxLength = 250)]
        public string ClassType
        {
            get
            { return (String)this["ClassType"]; }
            set
            { this["ClassType"] = value; }
        }
    }

    #endregion

    #region CBOSecurityFilterElementCollection Collection

    public class CBOSecurityFilterElementCollection : ConfigurationElementCollection
    {
        public CBOSecurityFilterElementCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new CBOSecurityFilterElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((CBOSecurityFilterElement)element).ID;
        }

        public CBOSecurityFilterElement this[int index]
        {
            get
            {
                return (CBOSecurityFilterElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public CBOSecurityFilterElement this[string Name]
        {
            get
            {
                return (CBOSecurityFilterElement)BaseGet(Name);
            }
        }

        public int IndexOf(CBOSecurityFilterElement Element)
        {
            return BaseIndexOf(Element);
        }

        public void Add(CBOSecurityFilterElement Element)
        {
            BaseAdd(Element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(CBOSecurityFilterElement Element)
        {
            if (BaseIndexOf(Element) >= 0)
                BaseRemove(Element.ID);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }

    #endregion
}
