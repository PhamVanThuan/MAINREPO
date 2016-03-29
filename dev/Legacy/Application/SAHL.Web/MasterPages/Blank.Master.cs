using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;

namespace SAHL.Web.MasterPages
{
    public partial class Blank : SAHLMasterBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region SAHLMasterBase Members

        /// <summary>
        /// Implements <see cref="SAHL.Common.Web.UI.IValidationSummary"/>.  This will currently return null as there has thus far been no need for 
        /// validation on the blank master page.
        /// </summary>
        public override SAHL.Common.Web.UI.Controls.SAHLValidationSummary ValidationSummary
        {
            get
            {
                return null;
            }
        }

        #endregion
    }
}
