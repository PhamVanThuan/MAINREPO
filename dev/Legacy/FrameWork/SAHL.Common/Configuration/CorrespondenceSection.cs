using SAHL.Common.Globals;
using System;
using System.Configuration;

namespace SAHL.Common.Configuration
{
    #region CorrespondenceSection

    /// <summary>
    ///
    /// </summary>
    public class CorrespondenceSection : ConfigurationSection
    {
        /// <summary>
        ///
        /// </summary>
        [ConfigurationProperty("Views", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CorrespondenceViewElementCollection))]
        public CorrespondenceViewElementCollection Views
        {
            get
            {
                CorrespondenceViewElementCollection _Views =
                (CorrespondenceViewElementCollection)base["Views"];
                return _Views;
            }
        }
    }

    #endregion CorrespondenceSection

    #region CorrespondenceViewElement

    /// <summary>
    ///
    /// </summary>
    public class CorrespondenceViewElement : ConfigurationElement
    {
        /// <summary>
        ///
        /// </summary>
        public CorrespondenceViewElement()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="batchPrint"></param>
        /// <param name="allowPreview"></param>
        /// <param name="dataStorName"></param>
        /// <param name="updateConditions"></param>
        /// <param name="sendUserConfirmationEmail"></param>
        /// <param name="emailProcessedPDFtoConsultant"></param>
        /// <param name="correspondenceTemplate"></param>
        /// <param name="combineDocumentsIfEmailing"></param>
        public CorrespondenceViewElement(string viewName, bool batchPrint, bool allowPreview, string dataStorName, bool updateConditions, bool sendUserConfirmationEmail, bool emailProcessedPDFtoConsultant, CorrespondenceTemplates correspondenceTemplate, bool combineDocumentsIfEmailing)
        {
            CorrespondenceViewName = viewName;
            BatchPrint = batchPrint;
            DataStorName = dataStorName;
            AllowPreview = allowPreview;
            UpdateConditions = updateConditions;
            SendUserConfirmationEmail = sendUserConfirmationEmail;
            EmailProcessedPDFtoConsultant = emailProcessedPDFtoConsultant;
            CorrespondenceTemplate = correspondenceTemplate;
            CombineDocumentsIfEmailing = combineDocumentsIfEmailing;
        }

        /// <summary>
        ///
        /// </summary>
        [ConfigurationProperty("Name", DefaultValue = "ViewName", IsRequired = true, IsKey = true)]
        [StringValidator(InvalidCharacters = "", MinLength = 1, MaxLength = 250)]
        public string CorrespondenceViewName
        {
            get
            { return (String)this["Name"]; }
            set
            { this["Name"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [ConfigurationProperty("BatchPrint", DefaultValue = false, IsRequired = false)]
        public bool BatchPrint
        {
            get
            { return (bool)this["BatchPrint"]; }
            set
            { this["BatchPrint"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [ConfigurationProperty("AllowPreview", DefaultValue = false, IsRequired = false)]
        public bool AllowPreview
        {
            get
            { return (bool)this["AllowPreview"]; }
            set
            { this["AllowPreview"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [ConfigurationProperty("DataStorName", DefaultValue = "", IsRequired = false)]
        public string DataStorName
        {
            get
            { return (String)this["DataStorName"]; }
            set
            { this["DataStorName"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [ConfigurationProperty("UpdateConditions", DefaultValue = false, IsRequired = false)]
        public bool UpdateConditions
        {
            get
            { return (bool)this["UpdateConditions"]; }
            set
            { this["UpdateConditions"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [ConfigurationProperty("SendUserConfirmationEmail", DefaultValue = true, IsRequired = false)]
        public bool SendUserConfirmationEmail
        {
            get
            { return (bool)this["SendUserConfirmationEmail"]; }
            set
            { this["SendUserConfirmationEmail"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [ConfigurationProperty("EmailProcessedPDFtoConsultant", DefaultValue = false, IsRequired = false)]
        public bool EmailProcessedPDFtoConsultant
        {
            get
            { return (bool)this["EmailProcessedPDFtoConsultant"]; }
            set
            { this["EmailProcessedPDFtoConsultant"] = value; }
        }

        /// <summary>
        /// Values can be 'SAHL','HOC','LIFE','ATTORNEY','VALUER','INTERNAL','GENERIC', 'DEBTCOUNSELLING'
        /// </summary>
        [ConfigurationProperty("CorrespondenceTemplate", DefaultValue = CorrespondenceTemplates.EmailCorrespondenceSAHL, IsRequired = false)]
        public CorrespondenceTemplates CorrespondenceTemplate
        {
            get
            { return (CorrespondenceTemplates)this["CorrespondenceTemplate"]; }
            set
            { this["CorrespondenceTemplate"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [ConfigurationProperty("CombineDocumentsIfEmailing", DefaultValue = false, IsRequired = false)]
        public bool CombineDocumentsIfEmailing
        {
            get
            { return (bool)this["CombineDocumentsIfEmailing"]; }
            set
            { this["CombineDocumentsIfEmailing"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [ConfigurationProperty("Reports", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CorrespondenceReportElementCollection))]
        public CorrespondenceReportElementCollection Reports
        {
            get
            {
                CorrespondenceReportElementCollection _Reports =
                (CorrespondenceReportElementCollection)base["Reports"];
                return _Reports;
            }
        }
    }

    #endregion CorrespondenceViewElement

    #region CorrespondenceViewElement Collection

    public class CorrespondenceViewElementCollection : ConfigurationElementCollection
    {
        public CorrespondenceViewElementCollection()
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
            return new CorrespondenceViewElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((CorrespondenceViewElement)element).CorrespondenceViewName;
        }

        public CorrespondenceViewElement this[int index]
        {
            get
            {
                return (CorrespondenceViewElement)BaseGet(index);
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

        new public CorrespondenceViewElement this[string Name]
        {
            get
            {
                return (CorrespondenceViewElement)BaseGet(Name);
            }
        }

        public int IndexOf(CorrespondenceViewElement Element)
        {
            return BaseIndexOf(Element);
        }

        public void Add(CorrespondenceViewElement Element)
        {
            BaseAdd(Element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(CorrespondenceViewElement Element)
        {
            if (BaseIndexOf(Element) >= 0)
                BaseRemove(Element.CorrespondenceViewName);
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

    #endregion CorrespondenceViewElement Collection

    #region CorrespondenceReportElement

    public class CorrespondenceReportElement : ConfigurationElement
    {
        public CorrespondenceReportElement()
        {
        }

        public CorrespondenceReportElement(string p_ReportName, string p_GenericKeyParameterName, string p_LegalEntityParameterName, string p_AddressParameterName, string p_MailingTypeParameterName, string p_LanguageKeyParameterName, bool excludeFromDataSTOR)
        {
            ReportName = p_ReportName;
            GenericKeyParameterName = p_GenericKeyParameterName;
            LegalEntityParameterName = p_LegalEntityParameterName;
            AddressParameterName = p_AddressParameterName;
            MailingTypeParameterName = p_MailingTypeParameterName;
            LanguageKeyParameterName = p_LanguageKeyParameterName;
            ExcludeFromDataSTOR = excludeFromDataSTOR;
        }

        [ConfigurationProperty("Name", DefaultValue = "ABC", IsRequired = true, IsKey = true)]
        [StringValidator(InvalidCharacters = "", MinLength = 1, MaxLength = 250)]
        public string ReportName
        {
            get
            { return (String)this["Name"]; }
            set
            { this["Name"] = value; }
        }

        [ConfigurationProperty("GenericKeyParameterName", DefaultValue = "", IsRequired = false)]
        [StringValidator(MinLength = 0, MaxLength = 250)]
        public string GenericKeyParameterName
        {
            get
            { return (String)this["GenericKeyParameterName"]; }
            set
            { this["GenericKeyParameterName"] = value; }
        }

        [ConfigurationProperty("MailingTypeParameterName", DefaultValue = "", IsRequired = false)]
        [StringValidator(MinLength = 0, MaxLength = 250)]
        public string MailingTypeParameterName
        {
            get
            { return (String)this["MailingTypeParameterName"]; }
            set
            { this["MailingTypeParameterName"] = value; }
        }

        [ConfigurationProperty("LegalEntityParameterName", DefaultValue = "", IsRequired = false)]
        [StringValidator(MinLength = 0, MaxLength = 250)]
        public string LegalEntityParameterName
        {
            get
            { return (String)this["LegalEntityParameterName"]; }
            set
            { this["LegalEntityParameterName"] = value; }
        }

        [ConfigurationProperty("AddressParameterName", DefaultValue = "", IsRequired = false)]
        [StringValidator(MinLength = 0, MaxLength = 250)]
        public string AddressParameterName
        {
            get
            { return (String)this["AddressParameterName"]; }
            set
            { this["AddressParameterName"] = value; }
        }

        [ConfigurationProperty("LanguageKeyParameterName", DefaultValue = "", IsRequired = false)]
        [StringValidator(MinLength = 0, MaxLength = 250)]
        public string LanguageKeyParameterName
        {
            get
            { return (String)this["LanguageKeyParameterName"]; }
            set
            { this["LanguageKeyParameterName"] = value; }
        }

        [ConfigurationProperty("ExcludeFromDataSTOR", DefaultValue = false, IsRequired = false)]
        public bool ExcludeFromDataSTOR
        {
            get
            { return (bool)this["ExcludeFromDataSTOR"]; }
            set
            { this["ExcludeFromDataSTOR"] = value; }
        }
    }

    #endregion CorrespondenceReportElement

    #region CorrespondenceReportElement Collection

    public class CorrespondenceReportElementCollection : ConfigurationElementCollection
    {
        public CorrespondenceReportElementCollection()
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
            return new CorrespondenceReportElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((CorrespondenceReportElement)element).ReportName;
        }

        public CorrespondenceReportElement this[int index]
        {
            get
            {
                return (CorrespondenceReportElement)BaseGet(index);
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

        new public CorrespondenceReportElement this[string Name]
        {
            get
            {
                return (CorrespondenceReportElement)BaseGet(Name);
            }
        }

        public int IndexOf(CorrespondenceReportElement Element)
        {
            return BaseIndexOf(Element);
        }

        public void Add(CorrespondenceReportElement Element)
        {
            BaseAdd(Element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(CorrespondenceReportElement Element)
        {
            if (BaseIndexOf(Element) >= 0)
                BaseRemove(Element.ReportName);
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

    #endregion CorrespondenceReportElement Collection
}