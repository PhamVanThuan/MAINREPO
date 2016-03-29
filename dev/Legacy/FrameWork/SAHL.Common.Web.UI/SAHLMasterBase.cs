using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI.Controls;
using System.Web.UI;

namespace SAHL.Common.Web.UI
{

    /// <summary>
    /// The SAHL base master page - all SAHL master pages should inherit from this.
    /// </summary>
    public abstract class SAHLMasterBase : MasterPage
    {

        /// <summary>
        /// Gets/sets the validation summary control on the page.
        /// </summary>
        public abstract SAHLValidationSummary ValidationSummary { get; }
    }
}
