using System;
using System.Configuration;

namespace SAHL.Common.Configuration.Security
{
    /// <summary>
    /// Represents the collection of <see cref="ADUserElement"/> elements within the <see cref="SecuritySection"/>.
    /// </summary>
    public class ADUserElementCollection : ConfigurationElementCollection
    {
        private SecurityElementRestrictions _restriction = SecurityElementRestrictions.Include;

        static ADUserElementCollection()
        {
        }

        public ADUserElement this[int index]
        {
            get
            {
                return (ADUserElement)base.BaseGet(index);
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
            return new ADUserElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ADUserElement)element).Value;
        }

        new public ADUserElement this[string Name]
        {
            get
            {
                return (ADUserElement)BaseGet(Name);
            }
        }

        public int IndexOf(ADUserElement Element)
        {
            return BaseIndexOf(Element);
        }

        public void Add(ADUserElement Element)
        {
            BaseAdd(Element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ADUserElement Element)
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

        /// <summary>
        /// Gets/sets the restriction type on the group.
        /// </summary>
        public SecurityElementRestrictions Restriction
        {
            get
            {
                return _restriction;
            }
            set
            {
                _restriction = value;
            }
        }

        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            if (name == "restriction")
            {
                Restriction = (SecurityElementRestrictions)Enum.Parse(typeof(SecurityElementRestrictions), value, true);
                return true;
            }
            return base.OnDeserializeUnrecognizedAttribute(name, value);
        }
    }
}