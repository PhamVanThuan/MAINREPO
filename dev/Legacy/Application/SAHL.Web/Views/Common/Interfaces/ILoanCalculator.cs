using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface ILoanCalculator : IViewBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListFinancialService"></param>
        void BindFinancialServiceGrid(IList<BindableFinancialService> ListFinancialService);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListMargin"></param>
        void BindLinkRates(IEventList<IMargin> ListMargin);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vml"></param>
        /// <param name="fml"></param>
        /// <param name="splitV"></param>
        /// <param name="CapitalisedLife"></param>
        void BindMortgageLoans(IMortgageLoan vml, IMortgageLoan fml, double splitV, double CapitalisedLife);

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnAmortisationScheduleClicked;

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
        void ChangedCalcType();

        /// <summary>
        /// 
        /// </summary>
        bool ShowLoanPercSplit { set;}

        /// <summary>
        /// 
        /// </summary>
        double MaxBondAmount
        {
            get;
            set;
        }
    }

    public class BindableFinancialService
    {
        internal string _service;
        public string Service { get { return _service; } }
        internal double _rate;
        public double Rate { get { return _rate; } }
        internal int _term;
        public int Term { get { return _term; } }
        internal double _loanSplit;
        public double LoanSplit { get { return _loanSplit; } }
        internal int _period;
        public int Period { get { return _period; } }
        internal double _currentBalance;
        public double CurrentBalance { get { return _currentBalance; } }
        internal double _instalmentCapital;
        public double InstalmentCapital { get { return _instalmentCapital; } }
        internal double _instalmentInterest;
        public double InstalmentInterest { get { return _instalmentInterest; } }
        internal double _payment;
        public double Payment { get { return _payment; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ml"></param>
        /// <param name="split"></param>
        /// <param name="capitalisedLife"></param>
        public BindableFinancialService(IMortgageLoan ml, double split, double capitalisedLife)
        {
            this._service = ml.FinancialServiceType.Description.ToString();
            this._rate = ml.InterestRate;
            this._term = ml.RemainingInstallments;
            this._loanSplit = split;
            //Period is the no of months the account has been open, not the same as remaining instalments
            //this._period = ml.InitialInstallments - ml.RemainingInstallments;
            DateTime od = ml.OpenDate.Value;
            DateTime now = DateTime.Now;
            this._period = now.Month - od.Month + (12 * (now.Year - od.Year)); // -((now.Day < od.Day) ? 1 : 0);

            this._currentBalance = ml.CurrentBalance;
            this._instalmentInterest = (ml.InterestRate / 12) * (ml.CurrentBalance - capitalisedLife);
            this._instalmentCapital = ml.Payment - _instalmentInterest;
            this._payment = ml.Payment;
        }

    }
}
