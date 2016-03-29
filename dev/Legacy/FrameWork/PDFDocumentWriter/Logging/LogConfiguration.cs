using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace PDFDocumentWriter.Logging
{
    public class LogSection : ConfigurationSection
    {
        [ConfigurationProperty("Logging", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(LogElementCollection))]
        public LogElementCollection Logging
        {
            get
            {
                LogElementCollection _Logging =
                (LogElementCollection)base["Logging"];
                return _Logging;
            }
        }
    }

    public class LogElement : ConfigurationElement
    {
        public LogElement()
        {

        }

        public LogElement(string name, string level, string Lock, string path)
        {
            this.name = name;
            this.level = level;
            this.Lock = Lock;
            this.path = path;
        }

        [ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("level", DefaultValue = "1", IsRequired = true, IsKey = false)]
        public string level
        {
            get { return (string)this["level"]; }
            set { this["level"] = value; }
        }
        [ConfigurationProperty("Lock", DefaultValue = "false", IsRequired = true, IsKey = false)]
        public string Lock
        {
            get { return (string)this["Lock"]; }
            set { this["Lock"] = value; }
        }
        [ConfigurationProperty("path", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }
    }

    public class LogElementCollection : ConfigurationElementCollection
    {
        public LogElementCollection()
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
            return new LogElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((LogElement)element).name;
        }

        public LogElement this[int index]
        {
            get
            {
                return (LogElement)BaseGet(index);
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

        new public LogElement this[string Name]
        {
            get
            {
                return (LogElement)BaseGet(Name);
            }
        }

        public int IndexOf(LogElement Element)
        {
            return BaseIndexOf(Element);
        }

        public void Add(LogElement Element)
        {
            BaseAdd(Element);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(LogElement Element)
        {
            if (BaseIndexOf(Element) >= 0)
                BaseRemove(Element.name);
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
