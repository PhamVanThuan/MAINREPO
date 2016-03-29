using System;
using System.Configuration;

namespace SAHL.Common.Configuration.Security
{
    /// <summary>
    /// Represents the collection of <see cref="FeatureElement"/> elements within the <see cref="SecuritySection"/>.
    /// </summary>
    public class FeatureElementCollection : ConfigurationElementCollection
    {
        static FeatureElementCollection()
        {
        }

        public FeatureElement this[int index]
        {
            get
            {
                return (FeatureElement)base.BaseGet(index);
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
            return new FeatureElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((FeatureElement)element).Value;
        }

        new public FeatureElement this[string Name]
        {
            get
            {
                return (FeatureElement)BaseGet(Name);
            }
        }

        public int IndexOf(FeatureElement Element)
        {
            return BaseIndexOf(Element);
        }

        public void Add(FeatureElement Element)
        {
            BaseAdd(Element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(FeatureElement Element)
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