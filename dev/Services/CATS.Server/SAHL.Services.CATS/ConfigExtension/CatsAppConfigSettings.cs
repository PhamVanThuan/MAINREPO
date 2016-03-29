using System.Collections.Specialized;

namespace SAHL.Services.CATS.ConfigExtension
{
    public class CatsAppConfigSettings : ICatsAppConfigSettings
    {
        private NameValueCollection nameValueCollection;

        public CatsAppConfigSettings(NameValueCollection nameValueCollection)
        {
            this.nameValueCollection = nameValueCollection;
        }

        public string CATSInputFileLocation
        {
            get
            {
                string value = nameValueCollection["CATSFileLocation"] + "\\IN\\";
                return value ?? string.Empty;
            }
        }

        public string CATSOutputFileLocation
        {
            get
            {
                string value = nameValueCollection["CATSFileLocation"] + "\\OUT\\";
                return value ?? string.Empty;
            }
        }

        public string CATSFailureFileLocation
        {
            get
            {
                string value = nameValueCollection["CATSFileLocation"] + "\\sendfailure\\";
                return value ?? string.Empty;
            }
        }

        public string CATSSuccessFileLocation
        {
            get
            {
                string value = nameValueCollection["CATSFileLocation"] + "\\sendsuccess\\";
                return value ?? string.Empty;
            }
        }

        public string CATSArchiveFileLocation
        {
            get
            {
                string value = nameValueCollection["CATSFileLocation"] + "\\Archive\\";
                return value ?? string.Empty;
            }
        }

    }
}
