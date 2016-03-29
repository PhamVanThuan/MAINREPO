using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Globals;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class Bond : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Bond_DAO>, IBond
    {
        /// <summary>
        ///
        /// </summary>
        public DateTime BondRegistrationDate
        {
            get { return _DAO.BondRegistrationDate; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnMortgageLoans_BeforeAdd(ICancelDomainArgs args, object Item)
        {
            // Business Rule: A bond may not be created against a closed mortgage loan.
            if (Item is IMortgageLoan)
            {
                IMortgageLoan mortgageLoan = Item as IMortgageLoan;

                if (mortgageLoan.AccountStatus.Key == (int)AccountStatuses.Closed)
                {
                    //SAHLPrincipal currentPrincipal = new SAHLPrincipal(WindowsIdentity.GetCurrent());
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                    spc.DomainMessages.Add(new Error("A Bond cannot be created against a closed Mortgage Loan.", "A Bond cannot be created aginst a closed Mortgage Loan."));

                    args.Cancel = true;
                }
            }

            //If we are cancelling a request, throw an error so that we know it has been canccelled.
            //This needs to be done after all items have been validated so that multiple/all messages are
            //reported to the UI
            if (args.Cancel)
                throw new SAHL.Common.Exceptions.DomainValidationException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnMortgageLoans_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnLoanAgreements_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnLoanAgreements_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnMortgageLoans_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnMortgageLoans_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnLoanAgreements_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnLoanAgreements_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Rules"></param>
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);

            Rules.Add("BondRegistrationAmount");
            Rules.Add("BondRegistrationNumberUnique");
            Rules.Add("BondRegistrationNumberUpdateMandatory");
        }
    }
}