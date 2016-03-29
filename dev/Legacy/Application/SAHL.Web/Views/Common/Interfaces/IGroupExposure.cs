using SAHL.Common.BusinessModel.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IGroupExposure : IViewBase
    {
        #region Properties

        /// <summary>
        /// Text to appear above the legal entity grid
        /// </summary>
        string LegalEntityGridText { set; }

        /// <summary>
        /// Text to appear above the group exposure grid
        /// </summary>
        string GroupExposureGridText { set; }

        /// <summary>
        /// Sets the visibility of the Debt Counselling Grid
        /// </summary>
        bool DebtCounsellingGridVisible { set; }

        /// <summary>
        /// Gets the index of the selected legal entity
        /// </summary>
        int LegalEntityGridSelectedIndex { get; }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        ///
        /// </summary>
        event KeyChangedEventHandler OnLegalEntityGridSelectedIndexChanged;

        /// <summary>
        ///
        /// </summary>
        event KeyChangedEventHandler OnGroupExposureGridSelectedIndexChanged;

        #endregion Event Handlers

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityDT"></param>
        void BindLegalEntityGrid(DataTable legalEntityDT);

        /// <summary>
        /// Populated the grid with the debt counselling detail types
        /// </summary>
        /// <param name="detailList"></param>
        void BindDebtCounsellingGrid(List<IDetail> combinedDetailList);

        void BindGroupExposureGrid(LegalEntityGroupExposure groupExposureCollection);
    }
}