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
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class FutureDatedChange : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO>, IFutureDatedChange
	{
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);

            Rules.Add("FinancialServiceDOEffectiveDateBusinessDay");
            Rules.Add("FinancialServiceDOEffectiveDatePreviousDayBusinessDay");
            Rules.Add("FinancialServiceDebitOrderUpdateDayMonthDone");
            Rules.Add("FinancialServiceBankAccountAddNewFixedDebitChangeReference");
            Rules.Add("FinancialServiceBankAccountAddNewDebitOrderChangeReference");
            Rules.Add("FutureDatedChangeEffectiveDateMinimum");
            Rules.Add("FutureDatedChangeEffectiveDateMaximum");
            Rules.Add("FutureDatedChangeSinglePaymentCheck");
            Rules.Add("FutureDatedChangeEffectiveDateCheck");
            Rules.Add("FutureDatedChangeEffectiveDateDODayCheck");
        }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnFutureDatedChangeDetails_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnFutureDatedChangeDetails_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnFutureDatedChangeDetails_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnFutureDatedChangeDetails_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }
	}
}


