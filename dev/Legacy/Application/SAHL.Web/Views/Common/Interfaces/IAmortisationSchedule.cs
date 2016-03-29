using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.DataSets;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IAmortisationSchedule : IViewBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="finServTypeKey"></param>
        /// <param name="dt"></param>
        void BindAmortisationGrid(int finServTypeKey, LoanCalculations.AmortisationScheduleDataTable dt);

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnBackClicked;

        /// <summary>
        /// 
        /// </summary>
        double CurrentBalanceV {get;  set; }

        /// <summary>
        /// 
        /// </summary>
        double InterestRateV { get;  set; }

        /// <summary>
        /// 
        /// </summary>
        double InstalmentTotalV { get;  set; }

        /// <summary>
        /// 
        /// </summary>
        int RemainingTermV { get;  set; }

        /// <summary>
        /// 
        /// </summary>
        double CurrentBalanceF { get;  set; }

        /// <summary>
        /// 
        /// </summary>
        double InterestRateF { get;  set; }

        /// <summary>
        /// 
        /// </summary>
        double InstalmentTotalF { get;  set; }

        /// <summary>
        /// 
        /// </summary>
        int RemainingTermF { get;  set; }

        /// <summary>
        /// 
        /// </summary>
        bool DisplayLoanValues {  set; }

        /// <summary>
        /// 
        /// </summary>
        bool DisplayFixedAndVariableGrids { set; }
    }
}
