using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.UI.Configuration
{
    #region CorrespondenceSection

    public class CBOSection : ConfigurationSection
    {
        [ConfigurationProperty("CBOMenus", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CBOElementCollection))]
        public CBOElementCollection CBOMenus
        {
            get
            {
                CBOElementCollection _CBOMenus =
                (CBOElementCollection)base["CBOMenus"];
                return _CBOMenus;
            }
        }
    }

    #endregion

    #region CBOMenuElement

    public class CBOMenuElement : ConfigurationElement
    {
        public CBOMenuElement()
        {

        }

        public CBOMenuElement(int p_CBOKey, string p_NodeClass)//, string p_UINodeClass)
        {
            CBOKey = p_CBOKey;
            NodeClass = p_NodeClass;
//            UINodeClass = p_UINodeClass;
        }

        [ConfigurationProperty("CBOKey", DefaultValue = -1, IsRequired = true, IsKey = true)]
        public int CBOKey
        {
            get
            { return (int)this["CBOKey"]; }
            set
            { this["CBOKey"] = value; }

        }

        [ConfigurationProperty("NodeClass", DefaultValue = "SAHL.Common.UI.CBONode", IsRequired = true)]
        [StringValidator(InvalidCharacters = "", MinLength = 1, MaxLength = 250)]
        public string NodeClass
        {
            get
            { return (String)this["NodeClass"]; }
            set
            { this["NodeClass"] = value; }
        }

        //[ConfigurationProperty("UINodeClass", DefaultValue = "SAHL.Common.BusinessModel.CBONodes.CBONode", IsRequired = true)]
        //[StringValidator(InvalidCharacters = "", MinLength = 1, MaxLength = 250)]
        //public string UINodeClass
        //{
        //    get
        //    { return (String)this["UINodeClass"]; }
        //    set
        //    { this["UINodeClass"] = value; }
        //}
    }

    #endregion

    #region CBOElementCollection Collection

    public class CBOElementCollection : ConfigurationElementCollection
    {
        public CBOElementCollection()
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
            return new CBOMenuElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((CBOMenuElement)element).CBOKey;
        }

        public CBOMenuElement this[int index]
        {
            get
            {
                return (CBOMenuElement)BaseGet(index);
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

        new public CBOMenuElement this[string Name]
        {
            get
            {
                return (CBOMenuElement)BaseGet(Name);
            }
        }

        public int IndexOf(CBOMenuElement Element)
        {
            return BaseIndexOf(Element);
        }

        public void Add(CBOMenuElement Element)
        {
            BaseAdd(Element);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(CBOMenuElement Element)
        {
            if (BaseIndexOf(Element) >= 0)
                BaseRemove(Element.CBOKey);
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
