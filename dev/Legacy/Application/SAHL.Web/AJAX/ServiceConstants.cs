using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SAHL.Web.AJAX
{
    /// <summary>
    /// Defines constants for the SAHL.Web web services that can be used within views.
    /// </summary>
    public sealed class ServiceConstants
    {

        private ServiceConstants()
        {
        }

        /// <summary>
        /// Provides the URL to the AdUser web service.
        /// </summary>
        public const string AdUser = "~/AJAX/AdUser.asmx";

        /// <summary>
        /// Provides the URL to the Address web service.
        /// </summary>
        public const string Address = "~/AJAX/Address.asmx";

        /// <summary>
        /// Provides the URL to the Application web service.
        /// </summary>
        public const string Application = "~/AJAX/Application.asmx";

        /// <summary>
        /// Provides the URL to the Bank web service.
        /// </summary>
        public const string Bank = "~/AJAX/Bank.asmx";

        /// <summary>
        /// Provides the URL to the Employment web service.
        /// </summary>
        public const string Employment = "~/AJAX/Employment.asmx";

        /// <summary>
        /// Provides the URL to the LegalEntity web service.
        /// </summary>
        public const string LegalEntity = "~/AJAX/LegalEntity.asmx";

        /// <summary>
        /// Provides the URL to the Diary web service.
        /// </summary>
        public const string Diary = "~/AJAX/Diary.asmx";

        /// <summary>
        /// Provides the URL to the Court web service.
        /// </summary>
        public const string Court = "~/AJAX/Court.asmx";

        /// <summary>
        /// Provides the URL to the ActiveDirectory web service.
        /// </summary>
        public const string ActiveDirectory = "~/AJAX/ActiveDirectory.asmx";

        /// <summary>
        /// Provides the URL to the Reason web service.
        /// </summary>
        public const string Reason = "~/AJAX/Reason.asmx";

        /// <summary>
        /// Provides the URL to the X2 web service.
        /// </summary>
        public const string X2 = "~/AJAX/X2.asmx";

        /// <summary>
        /// Provides the URL to the LinkRates web service.
        /// </summary>
        public const string LinkRates = "~/AJAX/LinkRates.asmx";

        /// <summary>
        /// Provides the URL to the Account web service.
        /// </summary>
        public const string Account = "~/AJAX/Account.asmx";
    }
}
