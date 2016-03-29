using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.Web.UI.Configuration
{

    #region WebAppSection

    /// <summary>
    /// A Custom Configuration Section handler to store information about a SAHL web application.
    /// This will allow a number of applications to be defined with their own settings and a single 
    /// configuration change to be required to switch settings, which could be used to define settings
    /// for different products or for different companies using a SAHL web application.
    /// </summary>
    public class WebAppSection : ConfigurationSection
    {

        public WebAppSection()
        {
            SelectedApplication = "RCS";
        }

        /// <summary>
        /// Constructor for custom WebApplication Section
        /// </summary>
        /// <param name="selectedApplication"></param>
        public WebAppSection(string selectedApplication)
        {
            SelectedApplication = selectedApplication;
        }

        [ConfigurationProperty("SelectedApplication", DefaultValue = "SelectedApp", IsRequired = true)]
        [StringValidator(InvalidCharacters = "", MinLength = 1, MaxLength = 250)]
        public string SelectedApplication
        {
            get
            { return (String)this["SelectedApplication"]; }
            set
            { this["SelectedApplication"] = value; }
        }

        // Declare the urls collection property.
        // Note: the "IsDefaultCollection = false" instructs 
        // .NET Framework to build a nested section of 
        // the kind <WebApplications> ...</WebApplications>.
        [ConfigurationProperty("WebApplications", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(WebApplicationCollection))]
        public WebApplicationCollection WebApplications
        {
            get
            {
                WebApplicationCollection WebApplications =
                (WebApplicationCollection)base["WebApplications"];
                return WebApplications;
            }
        }
    }

    #endregion  

    #region WebAppSectionElement

    public class WebAppSectionElement : ConfigurationElement
    {
        public WebAppSectionElement()
        {
            ApplicationName = "App1";
            CSSFile = "Css";
            Title = "SomeTitle";
            ForDomains = "ForDomain";
        }

        public WebAppSectionElement(string applicationName, string cssFile, string title, string forDomains)
        {
            ApplicationName = applicationName;
            CSSFile = cssFile;
            Title = title;
            ForDomains = forDomains;
        }

        [ConfigurationProperty("ApplicationName", DefaultValue = "MyApplication", IsRequired = true, IsKey = true)]
        [StringValidator(InvalidCharacters = "", MinLength = 1, MaxLength = 250)]
        public string ApplicationName
        {
            get
            { return (String)this["ApplicationName"]; }
            set
            { this["ApplicationName"] = value; }

        }

        [ConfigurationProperty("CSSFile", DefaultValue = "Default.css", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 100)]
        public string CSSFile
        {
            get
            { return (String)this["CSSFile"]; }
            set
            { this["CSSFile"] = value; }

        }

        [ConfigurationProperty("Title", DefaultValue = "Title", IsRequired = true)]
        [StringValidator(InvalidCharacters = "", MinLength = 1, MaxLength = 250)]
        public string Title
        {
            get
            { return (String)this["Title"]; }
            set
            { this["Title"] = value; }

        }

        [ConfigurationProperty("ForDomains", DefaultValue = "ForDomains", IsRequired = false)]
        [StringValidator(InvalidCharacters = "", MinLength = 0, MaxLength = 250)]
        public string ForDomains
        {
            get
            { return (String)this["ForDomains"]; }
            set
            { this["ForDomains"] = value; }

        }
    }

    #endregion

    #region WebApplication Collection

    // Define the WebApplicationCollection that will contain the WebAppSectionElement
    // elements.
    public class WebApplicationCollection : ConfigurationElementCollection
    {
        public WebApplicationCollection()
        {
            WebAppSectionElement webApp = (WebAppSectionElement)CreateNewElement();
            Add(webApp);
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
            return new WebAppSectionElement("MyApplication" + this.Count.ToString(), "Default.css", "Title","");
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((WebAppSectionElement)element).ApplicationName;
        }

        public WebAppSectionElement this[int index]
        {
            get
            {
                return (WebAppSectionElement)BaseGet(index);
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

        new public WebAppSectionElement this[string name]
        {
            get
            {
                return (WebAppSectionElement)BaseGet(name);
            }
        }

        public int IndexOf(WebAppSectionElement webApplication)
        {
            return BaseIndexOf(webApplication);
        }

        public void Add(WebAppSectionElement webApplication)
        {
            BaseAdd(webApplication);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(WebAppSectionElement webApplication)
        {
            if (BaseIndexOf(webApplication) >= 0)
                BaseRemove(webApplication.ApplicationName);
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
