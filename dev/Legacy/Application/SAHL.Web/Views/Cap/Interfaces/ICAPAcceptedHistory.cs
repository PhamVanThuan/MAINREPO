using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.Cap.Interfaces
{
    public interface ICAPAcceptedHistory : IViewBase
    {
        #region Event Handlers

        /// <summary>
        ///
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        ///
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        #endregion Event Handlers

        #region Properties

        /// <summary>
        ///
        /// </summary>
        bool CancelCapRowVisible { set; }

        /// <summary>
        ///
        /// </summary>
        bool ButtonRowVisible { set; }

        /// <summary>
        ///
        /// </summary>
        int SelectedCancellationReason { get; }

        /// <summary>
        ///
        /// </summary>
        IDictionary<int, IFinancialAdjustment> FinancialAdjustmentDict { set; }

        #endregion Properties

        /// <summary>
        ///
        /// </summary>
        /// <param name="capofferDetailList"></param>
        void BindGrid(IList<ICapApplicationDetail> capofferDetailList);

        /// <summary>
        ///
        /// </summary>
        /// <param name="capReasons"></param>
        void BindReasons(IList<ICancellationReason> capReasons);
    }
}