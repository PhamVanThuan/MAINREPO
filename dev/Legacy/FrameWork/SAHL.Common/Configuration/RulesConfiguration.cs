using System;
using System.Configuration;

namespace SAHL.Common.Configuration
{
    public class SAHLRulesSection : ConfigurationSection
    {
        [ConfigurationProperty("Assemblies", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(RuleElementCollection))]
        public RuleElementCollection Assemblies
        {
            get
            {
                RuleElementCollection _Assemblies =
                (RuleElementCollection)base["Assemblies"];
                return _Assemblies;
            }
        }

        [ConfigurationProperty("Enabled", DefaultValue = true, IsRequired = false)]
        public bool Enabled
        {
            get
            {
                return (bool)this["Enabled"];
            }
        }
    }

    public class RuleElement : ConfigurationElement
    {
        public RuleElement()
        {
        }

        public RuleElement(string AssemblyName, string Location)
        {
            this.AssemblyName = AssemblyName;
            this.Location = Location;
        }

        [ConfigurationProperty("AssemblyName", DefaultValue = "SAHL.Rules.Dll", IsRequired = true, IsKey = true)]
        public string AssemblyName
        {
            get { return (string)this["AssemblyName"]; }
            set { this["AssemblyName"] = value; }
        }

        [ConfigurationProperty("Location", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Location
        {
            get { return (string)this["Location"]; }
            set { this["Location"] = value; }
        }
    }

    public class RuleElementCollection : ConfigurationElementCollection
    {
        public RuleElementCollection()
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
            return new RuleElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((RuleElement)element).AssemblyName;
        }

        public RuleElement this[int index]
        {
            get
            {
                return (RuleElement)BaseGet(index);
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

        new public RuleElement this[string Name]
        {
            get
            {
                return (RuleElement)BaseGet(Name);
            }
        }

        public int IndexOf(RuleElement Element)
        {
            return BaseIndexOf(Element);
        }

        public void Add(RuleElement Element)
        {
            BaseAdd(Element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(RuleElement Element)
        {
            if (BaseIndexOf(Element) >= 0)
                BaseRemove(Element.AssemblyName);
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
}