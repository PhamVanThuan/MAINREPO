using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SAHL.Web
{
    public static class ResourceConstants
    {
        public const string TESTSTR = "Test String";

        /// <summary>
        /// Message string used by the LegalEntityEnableUpdateUpdate Presenter.
        /// </summary>
        public const string LegalEntityEnableUpdateMessage = "LEGALENTITYENABLEUPDATEMESSAGE";

        /// <summary>
        /// Question string used by the LegalEntityEnableUpdateUpdate Presenter.
        /// </summary>
        public const string LegalEntityEnableUpdateQuestion = "LEGALENTITYENABLEUPDATEQUESTION";

        /// <summary>
        /// Question string used by the LegalEntityEnableUpdateSuretorAdd Presenter.
        /// </summary>
        public const string LegalEntityEnableSuretorAddMessage = "LEGALENTITYENABLESURETORADDMESSAGE";

        /// <summary>
        /// Error string used when the key does not exist in the global cache.
        /// </summary>
        public const string GlobalCacheNullElementException = "GLOBALCACHENULLELEMENTEXCEPTION";

        /// <summary>
        /// Error string used when a node is not found in the CBO.
        /// </summary>
        public const string NodeNotFoundException = "NODENOTFOUNDEXCEPTION";



    }
}
