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
using SAHL.Common.Utils;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class Disbursement : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Disbursement_DAO>, IDisbursement
	{

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);

            //These rules need to cater for multiple disbursements to be validated at once
            //Rules are currently in the presenter but need to be refactored into the disbursement repo
            //Rules.Add("CATSDisbursementValidateUpdateRecord");
            //Rules.Add("CATSDisbursementValidateAmount");
            //Rules.Add("CATSDisbursementValidateTypeCancRefundCurrentBalance");
            //Rules.Add("CATSDisbursementValidateReadvanceDebtCounselling");
            //Rules.Add("CATSDisbursementValidateTypeCAP2AddRecord");
            //Rules.Add("CATSDisbursementQuickCashDisbursementValidate");
        }

        public string GetBankDisplayName(BankAccountNameFormat Format)
        {
            switch (Format)
            {
                case BankAccountNameFormat.Full:
                    return StringUtils.Delimit("-",new string[] {ACBBranch.ACBBank.ACBBankDescription, ACBBranch.Key, ACBBranch.ACBBranchDescription, 
                        ACBType.ACBTypeDescription, AccountNumber, AccountName});
                case BankAccountNameFormat.Short:
                    return StringUtils.Delimit("-", new string[] {ACBBranch.ACBBank.ACBBankDescription, ACBBranch.Key, ACBBranch.ACBBranchDescription, 
                        ACBType.ACBTypeDescription, AccountNumber, AccountName});
                default:
                    return "";
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnAccountDebtSettlements_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnAccountDebtSettlements_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
        protected void OnLoanTransactions_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
        protected void OnLoanTransactions_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnAccountDebtSettlements_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnAccountDebtSettlements_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnLoanTransactions_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnLoanTransactions_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }


	}
}


