using System;
using System.Configuration;

namespace SAHL.Common.Configuration.Security
{
    /// <summary>
    /// Represents the collection of <see cref="ViewElement"/> elements within the <see cref="SecuritySection"/>.
    /// </summary>
    public class ViewElementCollection : ConfigurationElementCollection
    {
        static ViewElementCollection()
        {
        }

        public ViewElement this[int index]
        {
            get
            {
                return (ViewElement)base.BaseGet(index);
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
            return new ViewElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ViewElement)element).Value;
        }

        new public ViewElement this[string Name]
        {
            get
            {
                return (ViewElement)BaseGet(Name);
            }
        }

        public int IndexOf(ViewElement Element)
        {
            return BaseIndexOf(Element);
        }

        public void Add(ViewElement Element)
        {
            BaseAdd(Element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ViewElement Element)
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