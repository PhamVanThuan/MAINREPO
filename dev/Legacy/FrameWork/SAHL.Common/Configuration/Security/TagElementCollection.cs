using System;
using System.Configuration;

namespace SAHL.Common.Configuration.Security
{
    /// <summary>
    /// Represents the collection of <see cref="TagElement"/> elements within the <see cref="SecuritySection"/>.
    /// </summary>
    public class TagElementCollection : ConfigurationElementCollection
    {
        // private static readonly ConfigurationProperty _objects;

        static TagElementCollection()
        {
            // SAHLSecurityTagElementCollection._objects = new ConfigurationProperty(null, typeof(SAHLSecurityTagElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
        }

        public TagElementCollection()
        {
            this.AddElementName = "tag";
        }

        public TagElement this[int index]
        {
            get
            {
                return (TagElement)base.BaseGet(index);
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
            return new TagElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((TagElement)element).Name;
        }

        new public TagElement this[string Name]
        {
            get
            {
                return (TagElement)BaseGet(Name);
            }
        }

        public int IndexOf(TagElement Element)
        {
            return BaseIndexOf(Element);
        }

        public void Add(TagElement Element)
        {
            BaseAdd(Element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(TagElement Element)
        {
            if (BaseIndexOf(Element) >= 0)
                BaseRemove(Element.Name);
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