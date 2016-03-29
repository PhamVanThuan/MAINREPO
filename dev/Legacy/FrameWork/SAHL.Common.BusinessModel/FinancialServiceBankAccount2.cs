using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Security;
using System.Security.Principal;

using SAHL.Common.Globals;
using SAHL.Common.DomainMessages;
using SAHL.Common.CacheData;
namespace SAHL.Common.BusinessModel
{
      
	/// <summary>
	/// Financial Service bank account extension
	/// </summary>
	public partial class FinancialServiceBankAccount : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO>, IFinancialServiceBankAccount
    {
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);

            Rules.Add("FinancialServiceBankAccountAddNewDebitOrder");
            Rules.Add("FinancialServiceBankAccountDebitOrderDayAllowedValues");
        }

        #region Properties
        /// <summary>
        /// Returns Debit Order day
        /// </summary>
		public int DebitOrderDay
		{
			get { return _DAO.DebitOrderDay; }
			set { _DAO.DebitOrderDay = value; }
		}


        #endregion

        #region Methods
        /// <summary>
        /// Sets the debit Order day if it is valid
        /// </summary>
        /// <param name="effectiveDate"></param>
        /// <param name="DebitOrderDay"></param>
        public void SetDebitOrderDay(DateTime effectiveDate, int DebitOrderDay)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            bool effectiveDateInThisMonth;
            bool CanUpdate = true;

            if (effectiveDate.Month == DateTime.Now.Month)
                effectiveDateInThisMonth = true;
            else
                effectiveDateInThisMonth = false;

            if (effectiveDate.Date >= DateTime.Now.Date)
            {
                if (this._DAO.Key > 0) // Not a new record
                {
                    if (this._DAO.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.DebitOrderPayment)
                    {
                        // If Debit Order not yet collected, there can not be 2 in the same month - effective date must change
                        if (effectiveDateInThisMonth && DebitOrderDay > this._DAO.DebitOrderDay)
                        {
                            spc.DomainMessages.Add(new Error("This update will result in two debit orders being collected in the same month - Please check effective date", ""));
                            CanUpdate = false;
                        }

                        // Can not change debit Order day on Debit Order day
                        if (effectiveDateInThisMonth && DebitOrderDay == this._DAO.DebitOrderDay)
                        {
                            spc.DomainMessages.Add(new Error("Can not change a clients debit order day on the same day of the debit order run - Please change effective date", ""));
                            CanUpdate = false;
                        }
                        // Ensure that Debit Order is never skipped for a month
                        if (DebitOrderDay < this._DAO.DebitOrderDay && (effectiveDateInThisMonth && effectiveDate.Day < (this._DAO.DebitOrderDay + 1)))
                        {
                            spc.DomainMessages.Add(new Error("The effective date must be after the " + Convert.ToInt32((this._DAO.DebitOrderDay + 1)).ToString() + " of this month", ""));
                            CanUpdate = false;
                        }
    
                        if (CanUpdate)
                            this._DAO.DebitOrderDay = DebitOrderDay;

                    } // Setting Debit Order Day from Other processes
                    else
                        throw new Exception("Method for Non_Debit Order payments not implemented");
                }
                else
                    this._DAO.DebitOrderDay = DebitOrderDay;
            }
            else
                spc.DomainMessages.Add(new Error("Effective Date must be greater than or equal to today's date", ""));
        }

        #endregion

    }
}


