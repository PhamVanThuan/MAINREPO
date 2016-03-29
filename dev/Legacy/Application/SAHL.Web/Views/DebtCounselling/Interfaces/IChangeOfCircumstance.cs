using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
    public interface IChangeOfCircumstance : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        string Comment { get; }

        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        DateTime? Date { get; }
    }
}
