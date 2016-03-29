using System;
using System.Configuration;

namespace SAHL.Common.Configuration
{
    public class SAHLMandateSection : ConfigurationSection
    {
        [ConfigurationProperty("Assemblies", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MandateElementCollection))]
        public MandateElementCollection Assemblies
        {
            get
            {
                MandateElementCollection _Assemblies =
                (MandateElementCollection)base["Assemblies"];
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

    public class MandateElement : ConfigurationElement
    {
        public MandateElement()
        {
        }

        public MandateElement(string AssemblyName, string Location)
        {
            this.AssemblyName = AssemblyName;
            this.Location = Location;
        }

        [ConfigurationProperty("AssemblyName", DefaultValue = "SAHL.Mandate.Dll", IsRequired = true, IsKey = true)]
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

    public class MandateElementCollection : ConfigurationElementCollection
    {
        public MandateElementCollection()
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
            return new MandateElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((MandateElement)element).AssemblyName;
        }

        public MandateElement this[int index]
        {
            get
            {
                return (MandateElement)BaseGet(index);
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

        new public MandateElement this[string Name]
        {
            get
            {
                return (MandateElement)BaseGet(Name);
            }
        }

        public int IndexOf(MandateElement Element)
        {
            return BaseIndexOf(Element);
        }

        public void Add(MandateElement Element)
        {
            BaseAdd(Element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(MandateElement Element)
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