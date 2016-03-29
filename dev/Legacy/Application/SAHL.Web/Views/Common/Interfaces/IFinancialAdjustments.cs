using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Data;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IFinancialAdjustments : IViewBase
    {
		string InformationLabel { get; set; }
        string GetLoggedOnUser{ get; }

        /// <summary>
        /// Binds the list of Financial Adjustments to the grid
        /// </summary>
        /// <param name="dtRateOverrides"></param>
        void BindFinancialAdjustmentGrid(DataTable dtFinancialAdjustment);

        void BindFinancialAdjustmentGridPost(DataTable dtFinancialAdjustment);

        void SetUpFinancialAdjustmentGrid();

        void FinancialAdjustmentGridClear();

        #region Events

        event KeyChangedEventHandler OnUpdateButtonClicked;
        event KeyChangedEventHandler OnCancelButtonClicked;
        event KeyChangedEventHandler FinancialAdjustmentsGridRowUpdating;
        #endregion
    }
}
