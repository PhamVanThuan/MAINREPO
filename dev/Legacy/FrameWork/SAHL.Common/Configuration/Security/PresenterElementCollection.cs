using System;
using System.Configuration;

namespace SAHL.Common.Configuration.Security
{
    /// <summary>
    /// Represents the collection of <see cref="PresenterElement"/> elements within the <see cref="SecuritySection"/>.
    /// </summary>
    public class PresenterElementCollection : ConfigurationElementCollection
    {
        static PresenterElementCollection()
        {
        }

        public PresenterElement this[int index]
        {
            get
            {
                return (PresenterElement)base.BaseGet(index);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
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
            return new PresenterElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((PresenterElement)element).Value;
        }

        new public PresenterElement this[string Name]
        {
            get
            {
                return (PresenterElement)BaseGet(Name);
            }
        }

        public int IndexOf(PresenterElement Element)
        {
            return BaseIndexOf(Element);
        }

        public void Add(PresenterElement Element)
        {
            BaseAdd(Element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(PresenterElement Element)
        {
            if (BaseIndexOf(Element) >= 0)
                BaseRemove(Element.Value);
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