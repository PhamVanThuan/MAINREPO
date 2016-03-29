using System;
using System.Configuration;

namespace SAHL.Common.Configuration
{
    #region SAHLFactoriesSection

    public class SAHLFactoriesSection : ConfigurationSection
    {
        [ConfigurationProperty("Factories", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(FactoryElementCollection))]
        public FactoryElementCollection Factories
        {
            get
            {
                FactoryElementCollection _Factories =
                (FactoryElementCollection)base["Factories"];
                return _Factories;
            }
        }

        [ConfigurationProperty("CreationStrategy", DefaultValue = "SAHL.Common.Service.dll", IsRequired = true)]
        public string CreationStratergy
        {
            get { return (string)this["CreationStrategy"]; }
            set { this["CreationStrategy"] = value; }
        }
    }

    #endregion SAHLFactoriesSection

    #region FactoryElement

    public class FactoryElement : ConfigurationElement
    {
        public FactoryElement()
        {
        }

        public FactoryElement(string p_AssemblyName, string p_Usage)
        {
            AssemblyName = p_AssemblyName;
            Usage = p_Usage;
        }

        [ConfigurationProperty("AssemblyName", DefaultValue = "SAHL.Common.Service.dll", IsRequired = true, IsKey = true)]
        public string AssemblyName
        {
            get { return (string)this["AssemblyName"]; }
            set { this["AssemblyName"] = value; }
        }

        [ConfigurationProperty("Usage", DefaultValue = "Factory", IsRequired = false, IsKey = false)]
        public string Usage
        {
            get { return (string)this["Usage"]; }
            set { this["Usage"] = value; }
        }
    }

    #endregion FactoryElement

    #region FactoryElementCollection Collection

    public class FactoryElementCollection : ConfigurationElementCollection
    {
        public FactoryElementCollection()
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
            return new FactoryElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((FactoryElement)element).AssemblyName;
        }

        public FactoryElement this[int index]
        {
            get
            {
                return (FactoryElement)BaseGet(index);
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

        new public FactoryElement this[string Name]
        {
            get
            {
                return (FactoryElement)BaseGet(Name);
            }
        }

        public int IndexOf(FactoryElement Element)
        {
            return BaseIndexOf(Element);
        }

        public void Add(FactoryElement Element)
        {
            BaseAdd(Element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(FactoryElement Element)
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

    #endregion FactoryElementCollection Collection
}