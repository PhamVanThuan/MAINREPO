namespace SQLQuerying
{
    using System;
    using System.Configuration;
    using System.Xml;

    /// <summary>
    /// Inherits the configuration from the calling dlls.
    /// </summary>
    public abstract class DataContextConfig
    {
        static DataContextConfig()
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                ConfigurationSectionGroup group = config.GetSectionGroup("applicationSettings");
                ConfigurationSection section = group.Sections["TestSuiteSettings"];
                //this is to ensure old test suites work until we can refactor their app.config
                if (section == null)
                    section = group.Sections[0];
                PropertyInformationCollection propCol = section.ElementInformation.Properties;
                PropertyInformation[] propertiesInfo = new PropertyInformation[propCol.Count];
                propCol.CopyTo(propertiesInfo, 0);
                ConfigurationElementCollection elemCol = propertiesInfo[0].Value as ConfigurationElementCollection;
                settings = elemCol as SettingElementCollection;
            }
            catch
            {
                throw;
            }
        }

        #region Private
        private static SettingElementCollection settings;
        private static string GetSetting(string settingName)
        {
            try
            {
                SettingElement element = settings.Get(settingName);
                if (element == null)
                    throw new ConfigurationErrorsException("Could not locate the SettingElement: " + settingName + " in the configuration file");
                SettingValueElement settingVal = element.Value;
                XmlNode valueNode = settingVal.ValueXml;
                return valueNode.InnerText;
            }
            catch (ConfigurationErrorsException e)
            {
                throw e;
            }
        }
        #endregion Private

        #region Public
        internal string DatabaseServerName
        {
            get
            {
                return GetSetting("SAHLDataBaseServer");
            }
        }
        internal string SQLReportServer
        {
            get
            {
                return GetSetting("SQLReportServer");
            }
        }
        #endregion Public
    }
}