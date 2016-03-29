using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Testing.Common.Helpers
{
    public static class CatsAppConfigSettings
    {
        public static string CATSInputFileLocation = ConfigurationManager.AppSettings["CATSFileLocation"] + "\\IN\\";

        public static string CATSOutputFileLocation = ConfigurationManager.AppSettings["CATSFileLocation"] + "\\OUT\\";

        public static string CATSFailureFileLocation = ConfigurationManager.AppSettings["CATSFileLocation"] + "\\sendfailure\\";

        public static string CATSSuccessFileLocation = ConfigurationManager.AppSettings["CATSFileLocation"] + "\\sendsuccess\\";

        public static string CATSArchiveFileLocation = ConfigurationManager.AppSettings["CATSFileLocation"] + "\\Archive\\";
    }
}
