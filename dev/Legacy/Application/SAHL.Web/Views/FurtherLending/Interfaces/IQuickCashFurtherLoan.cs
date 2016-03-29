using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.FurtherLending.Interfaces
{
    public interface IQuickCashFurtherLoan : IViewBase
    {
        /// <summary>
        /// Binds all the quick cash details to the display controls.
        /// </summary>
        /// <param name="Application"></param>
        void BindQuickCashDetails(IApplication Application);

        /// <summary>
        /// Get all the updated QC details from the user controls.
        /// </summary>
        /// <param name="appInfoQC"></param>
        void GetQuickCashDetails(IApplicationInformationQuickCash appInfoQC);

        /// <summary>
        /// Sets whether the Quick Cash Details can be edited,
        /// </summary>
        bool HasQuickCashDeclineReasons { get; set;}

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSaveButtonClicked;

        /// <summary>
        /// Event raised when the quick cash decline button is clicked.
        /// </summary>
        event EventHandler OnQCDeclineReasons;
    }
}
