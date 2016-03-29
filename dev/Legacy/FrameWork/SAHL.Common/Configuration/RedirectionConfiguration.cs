using System;
using System.Configuration;

namespace SAHL.Common.Configuration
{
    //<RedirectionConfiguration>
    //  <EntryPoints>
    //      <EntryPoint NavigationView="ReleaseAndVariationsConditionsUpdate" >
    //            <Redirections>
    //                <Redirection typename="" NavigationView="" />
    //                <Redirection typename="" NavigationView="" />
    //             </Redirections>
    //      </EntryPoint>
    //  </EntryPoints>
    //</RedirectionConfiguration>

    public class SAHLRedirectionSection : ConfigurationSection
    {
        public RedirectionElement GetRedirection(Type DiscriminationType, string NavigationView)
        {
            EntrypointElement EntryPoint = EntryPoints[NavigationView];
            if (EntryPoint != null)
                return EntryPoint.Redirections[DiscriminationType.Name];
            return null;
        }

        public RedirectionElement GetRedirection(string DiscriminationName, string NavigationView)
        {
            EntrypointElement EntryPoint = EntryPoints[NavigationView];
            if (EntryPoint != null)
                return EntryPoint.Redirections[DiscriminationName];
            return null;
        }

        [ConfigurationProperty("EntryPoints", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(EntrypointElement))]
        public EntrypointCollection EntryPoints
        {
            get
            {
                EntrypointCollection _EntryPoints =
                (EntrypointCollection)base["EntryPoints"];
                return _EntryPoints;
            }
        }
    }

    public class EntrypointElement : ConfigurationElement
    {
        public EntrypointElement()
        { }

        [ConfigurationProperty("NavigationView", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string NavigationNodeAndValue
        {
            get { return (string)this["NavigationView"]; }
            set { this["NavigationView"] = value; }
        }

        [ConfigurationProperty("Redirections", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(RedirectionElementCollection))]
        public RedirectionElementCollection Redirections
        {
            get
            {
                RedirectionElementCollection _EntryPoints =
                (RedirectionElementCollection)base["Redirections"];
                return _EntryPoints;
            }
        }
    }

    public class EntrypointCollection : ConfigurationElementCollection
    {
        public EntrypointCollection()
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
            return new EntrypointElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((EntrypointElement)element).NavigationNodeAndValue;
        }

        public EntrypointElement this[int index]
        {
            get
            {
                return (EntrypointElement)BaseGet(index);
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

        new public EntrypointElement this[string Name]
        {
            get
            {
                return (EntrypointElement)BaseGet(Name);
            }
        }

        public int IndexOf(EntrypointElement Element)
        {
            return BaseIndexOf(Element);
        }

        public void Add(EntrypointElement Element)
        {
            BaseAdd(Element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(EntrypointElement Element)
        {
            if (BaseIndexOf(Element) >= 0)
                BaseRemove(Element.NavigationNodeAndValue);
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

    public class RedirectionElement : ConfigurationElement
    {
        public RedirectionElement()
        {
        }

        public RedirectionElement(string TypeName, string NavigationView)
        {
            this.TypeName = TypeName;
            this.NavigationView = NavigationView;
        }

        [ConfigurationProperty("TypeName", DefaultValue = "SAHL.Common.BusinessModel.Application", IsRequired = true, IsKey = true)]
        public string TypeName
        {
            get { return (string)this["TypeName"]; }
            set { this["TypeName"] = value; }
        }

        [ConfigurationProperty("NavigationView", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string NavigationView
        {
            get { return (string)this["NavigationView"]; }
            set { this["NavigationView"] = value; }
        }
    }

    public class RedirectionElementCollection : ConfigurationElementCollection
    {
        public RedirectionElementCollection()
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
            return new RedirectionElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((RedirectionElement)element).TypeName;
        }

        public RedirectionElement this[int index]
        {
            get
            {
                return (RedirectionElement)BaseGet(index);
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

        new public RedirectionElement this[string Name]
        {
            get
            {
                return (RedirectionElement)BaseGet(Name);
            }
        }

        public int IndexOf(RedirectionElement Element)
        {
            return BaseIndexOf(Element);
        }

        public void Add(RedirectionElement Element)
        {
            BaseAdd(Element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(RedirectionElement Element)
        {
            if (BaseIndexOf(Element) >= 0)
                BaseRemove(Element.TypeName);
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