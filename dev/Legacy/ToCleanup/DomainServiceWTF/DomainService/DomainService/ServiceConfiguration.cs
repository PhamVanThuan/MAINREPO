using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DomainService
{
    #region DomainServiceSection

    public class DomainServiceSection : ConfigurationSection
    {
        [ConfigurationProperty("DomainServices", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ServiceElementCollection))]
        public ServiceElementCollection DomainServices
        {
            get
            {
                ServiceElementCollection _DomainServices =
                (ServiceElementCollection)base["DomainServices"];
                return _DomainServices;
            }
        }

    }

    #endregion

    #region FactoryElement

    public class ServiceElement : ConfigurationElement
    {
        public ServiceElement()
        {

        }

        public ServiceElement(string p_IP, string p_AssemblyName)
        {
            IP = p_IP;
            AssemblyName = p_AssemblyName;
        }

        [ConfigurationProperty("IP", DefaultValue = "127.0.0.1", IsRequired = true, IsKey = false)]
        public string IP
        {
            get { return (string)this["IP"]; }
            set { this["IP"] = value; }
        }

        [ConfigurationProperty("AssemblyName", DefaultValue = "DomainService", IsRequired = true, IsKey = true)]
        public string AssemblyName
        {
            get { return (string)this["AssemblyName"]; }
            set { this["AssemblyName"] = value; }
        }
    }

    #endregion

    #region ServiceElementCollection Collection

    public class ServiceElementCollection : ConfigurationElementCollection
    {
        public ServiceElementCollection()
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
            return new ServiceElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)element).AssemblyName;
        }

        public ServiceElement this[int index]
        {
            get
            {
                return (ServiceElement)BaseGet(index);
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

        new public ServiceElement this[string Name]
        {
            get
            {
                return (ServiceElement)BaseGet(Name);
            }
        }

        public int IndexOf(ServiceElement Element)
        {
            return BaseIndexOf(Element);
        }

        public void Add(ServiceElement Element)
        {
            BaseAdd(Element);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ServiceElement Element)
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

    #endregion
}
