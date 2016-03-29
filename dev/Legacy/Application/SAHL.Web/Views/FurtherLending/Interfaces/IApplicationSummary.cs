using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.FurtherLending.Interfaces
{
    public interface IApplicationSummary : IViewBase
    {
        #region Events
        /// <summary>
        /// Raised when the cancel button is clicked
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// Raised when the Transition History button is clicked
        /// </summary>
        event EventHandler OnTransitionHistoryClicked;

        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vML"></param>
        /// <param name="fML"></param>
        void BindDisplay(IMortgageLoan vML, IMortgageLoan fML);

        void BindGrid(IApplication application);


        #endregion

        #region Properties
        /// <summary>
        /// Set whether the cancel button must be shown
        /// </summary>
        bool ShowCancelButton { set;}

        /// <summary>
        /// Set whether the cancel button must be shown
        /// </summary>
        bool ShowHistoryButton { set;}

        /// <summary>
        /// For 30 Year Term loans we must show the 20 year figures if there are more than 240 months remaining
        /// </summary>
        bool Show20YearFigures { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool HasArrears
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        int CurrentApplicationKey
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool SelectedApplicationOnly
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool TitleDeedOnFile
        {
            set;
        }

        bool ShowStopOrderDiscountEligibility { set; }

        #endregion
    }
}
