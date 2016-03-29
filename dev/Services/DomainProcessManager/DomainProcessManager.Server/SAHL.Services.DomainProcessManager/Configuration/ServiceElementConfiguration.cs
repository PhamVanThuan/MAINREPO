using System;
using System.Configuration;

namespace SAHL.Services.DomainProcessManager.Configuration
{
    public class ServiceElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return (ConfigurationElementCollectionType.BasicMap); }
        }

        protected override string ElementName
        {
            get { return ("Service"); }
        }

        new public ServiceElement this[string wcfServiceName]
        {
            get { return ((ServiceElement)this.BaseGet(wcfServiceName)); }
        }

        public ServiceElement this[int elementIndex]
        {
            get { return ((ServiceElement)this.BaseGet(elementIndex)); }
            set
            {
                if (this.BaseGet(elementIndex) != null) { this.BaseRemoveAt(elementIndex); }
                this.BaseAdd(elementIndex, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return (new ServiceElement());
        }

        protected override Object GetElementKey(ConfigurationElement wcfServiceElement)
        {
            return (((ServiceElement)wcfServiceElement).Name);
        }

        public void Add(ServiceElement wcfServiceElement)
        {
            this.BaseAdd(wcfServiceElement);
        }
    }
}
