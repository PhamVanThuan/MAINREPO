using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Origination.Interfaces
{
    public interface IGenericCalculator : IViewBase
    {

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnCompanySelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnAmortisationScheduleButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        bool EnableAmortisation
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        IList<IOriginationSource> CompanyList
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        Dictionary<int, string> LinkRateList
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        double MarketRate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double InterestRate
        {
            get;
        }

        /// <summary>
        /// Dictionary containing data used to calc Amortisation Schedule
        /// </summary>
        Dictionary<string, double> CalcDict
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        int Term
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double CurrentBalance
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string CompanySelectedValue
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string LinkRateSelectedValue
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string CalcType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double InstalmentTotal
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool ReloadView
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        double MaxBondAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        int MaxTerm
        {
            get;
            set;
        }
    }
}
