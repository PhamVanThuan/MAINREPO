using System;
using System.Configuration;

namespace Common
{
    public static class GlobalConfiguration
    {
        static GlobalConfiguration()
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var group = config.GetSectionGroup("applicationSettings");
                var section = group.Sections["TestSuiteSettings"];
                //this is to ensure old test suites work until we can refactor their app.config
                if (section == null)
                    section = group.Sections[0];
                var propCol = section.ElementInformation.Properties;
                var propertiesInfo = new PropertyInformation[propCol.Count];
                propCol.CopyTo(propertiesInfo, 0);
                var elemCol = propertiesInfo[0].Value as ConfigurationElementCollection;
                _settings = elemCol as SettingElementCollection;
            }
            catch
            {
                throw;
            }
        }

        #region Private

        private static SettingElementCollection _settings;

        private static string GetSetting(string settingName)
        {
            try
            {
                var element = _settings.Get(settingName);
                if (element == null)
                    throw new ConfigurationErrorsException("Could not locate the SettingElement: " + settingName + " in the configuration file");
                var settingVal = element.Value;
                var valueNode = settingVal.ValueXml;
                return valueNode.InnerText;
            }
            catch (ConfigurationErrorsException e)
            {
                throw e;
            }
        }

        #endregion Private

        #region Public

        public static string DatabaseServerName
        {
            get
            {
                return GetSetting("SAHLDataBaseServer");
            }
        }

        public static string HaloURL
        {
            get
            {
                return GetSetting("HaloWebServiceURL");
            }
        }

        public static string SQLReportServer
        {
            get
            {
                return GetSetting("SQLReportServer");
            }
        }

        public static string BrowserVisibility
        {
            get
            {
                return GetSetting("BrowserVisability");
            }
        }

        public static string TestWebsite
        {
            get
            {
                return GetSetting("TestWebsite");
            }
        }

        public static string AttorneyWebsite
        {
            get
            {
                return GetSetting("AttorneyAccessLoginURL");
            }
        }

        public static string LightstoneWebsite
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public static string AttorneyWebAccess
        {
            get
            {
                return GetSetting("AttorneyWebAccess");
            }
        }

        public static string AttorneyWebAccessSearchURL
        {
            get
            {
                return GetSetting("AttorneyWebAccessSearchURL");
            }
        }

        public static string AttorneyAccessLoginURL
        {
            get
            {
                return GetSetting("AttorneyAccessLoginURL");
            }
        }

        public static string AttorneyAccessCaseSearchURL
        {
            get
            {
                return GetSetting("AttorneyAccessCaseSearchURL");
            }
        }

        public static Uri EzValWebserviceUrl
        {
            get
            {
                return new Uri(GetSetting("EzValWebserviceUrl"));
            }
        }

        public static Uri EzValInstructWebserviceUrl
        {
            get
            {
                return new Uri(GetSetting("EzValInstructWebserviceUrl"));
            }
        }

        public static Uri ClientSecureWebsiteURL
        {
            get
            {
                return new Uri(GetSetting("ClientSecureWebsiteURL"));
            }
        }

        public static Uri ClientSecureWebsiteLoanStatementURL
        {
            get
            {
                return new Uri(GetSetting("ClientSecureWebsiteLoanStatementURL"));
            }
        }

        public static Uri ClientSecureWebsiteResetPasswordURL
        {
            get
            {
                return new Uri(GetSetting("ClientSecureWebsiteResetPasswordURL"));
            }
        }

        #endregion Public
    }
}