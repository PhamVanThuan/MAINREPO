using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Life.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBenefit : IViewBase
    {
        /// <summary>
        /// Event raised when next button is clicked
        /// </summary>
        event EventHandler OnNextButtonClicked;

        /// <summary>
        /// Get/Set whether the Activity has been completed
        /// </summary>
        bool ActivityDone { get; set;}

        LifePolicyTypes LifePolicyType { get; set; }

        bool ProceedWithPolicy { get; set; }

        bool RefusedPolicy { get; set; }

        /// <summary>
        /// Binds the Exclusions to the Exclusions table
        /// </summary>
        /// <param name="textItems">A collection of ITextStatements</param>
        void BindBenefit(IReadOnlyEventList<ITextStatement> textItems);

    }
}
