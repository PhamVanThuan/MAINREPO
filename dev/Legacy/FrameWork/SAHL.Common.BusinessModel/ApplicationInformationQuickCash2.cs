using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class ApplicationInformationQuickCash : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInformationQuickCash_DAO>, IApplicationInformationQuickCash
    {
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("QuickCashUpFrontMaximum");
            Rules.Add("QuickCashUpFrontApprovalReduce");
            Rules.Add("QuickCashTotalApprovalReduce");
            Rules.Add("ApplicationInformationQuickCashValidate");
            Rules.Add("QuickCashCreditApproveAmount");
            Rules.Add("QuickUpFrontLessThanApprovedAmount");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationInformationQuickCashDetails_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationInformationQuickCashDetails_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationInformationQuickCashDetails_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationInformationQuickCashDetails_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        #region IApplicationInformationQuickCash Members

        public double GetMaximumQuickCash()
        {
            Control_DAO[] Control_DAOs;
            double QuickCashGlobalMaximumValue = 0.0;
            double QuickCashGlobalMaximumPercentage = 0.0;
            double MaximumQuickCash = 0.0;
            double valuationPercentage = 0.0;
            double totalLoanRequired = 0.0;
            double propertyValuation = 0.0;
            double valuationMaxQuickCash = 0.0;
            double requestedCashAmount = 0.0;

            Control_DAOs = Control_DAO.FindAllByProperty("ControlDescription", "QuickCash Maximum");
            if (Control_DAOs.Length == 1 && Control_DAOs[0].ControlNumeric.HasValue)
                QuickCashGlobalMaximumValue = Control_DAOs[0].ControlNumeric.Value;
            else
                throw new Exception("Control Table: Duplicate definitions or entry not found for: QuickCash Maximum.");

            Control_DAOs = Control_DAO.FindAllByProperty("ControlDescription", "Quick Cash Threshold%");
            if (Control_DAOs.Length == 1 && Control_DAOs[0].ControlNumeric.HasValue)
                QuickCashGlobalMaximumPercentage = Control_DAOs[0].ControlNumeric.Value;
            else
                throw new Exception("Control Table: Duplicate definitions or entry not found for: Quick Cash Threshold%.");

            Control_DAOs = Control_DAO.FindAllByProperty("ControlDescription", "Quick Cash Valuation Threshold%");
            if (Control_DAOs.Length == 1 && Control_DAOs[0].ControlNumeric.HasValue)
                valuationPercentage = Control_DAOs[0].ControlNumeric.Value;
            else
                throw new Exception("Control Table: Duplicate definitions or entry not found for: Quick Cash Valuation Threshold%.");

            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = this.ApplicationInformation.Application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            if (supportsVariableLoanApplicationInformation != null)
            {
                if (supportsVariableLoanApplicationInformation.VariableLoanInformation.LoanAgreementAmount.HasValue)
                    totalLoanRequired = supportsVariableLoanApplicationInformation.VariableLoanInformation.LoanAgreementAmount.Value;

                if (supportsVariableLoanApplicationInformation.VariableLoanInformation.PropertyValuation.HasValue)
                    propertyValuation = supportsVariableLoanApplicationInformation.VariableLoanInformation.PropertyValuation.Value;
            }

            // The Application has to support the IApplicationMortgageLoanWithCashOut
            IApplicationMortgageLoanWithCashOut applicationMortgageLoanWithCashOut = this.ApplicationInformation.Application as IApplicationMortgageLoanWithCashOut;
            if (applicationMortgageLoanWithCashOut != null)
            {
                if (applicationMortgageLoanWithCashOut.RequestedCashAmount.HasValue)
                    requestedCashAmount = applicationMortgageLoanWithCashOut.RequestedCashAmount.Value;

                MaximumQuickCash = QuickCashGlobalMaximumPercentage / 100 * requestedCashAmount;

                // Apply the global max quickcash rules.
                MaximumQuickCash = Math.Min(MaximumQuickCash, QuickCashGlobalMaximumValue);

                // Apply the valuation rules
                valuationMaxQuickCash = valuationPercentage * propertyValuation / 100 + requestedCashAmount - totalLoanRequired;
                if (valuationMaxQuickCash < 0) valuationMaxQuickCash = 0;
                MaximumQuickCash = Math.Min(MaximumQuickCash, valuationMaxQuickCash);

                // Round down to the nearest 1000
                MaximumQuickCash = Math.Floor(MaximumQuickCash / 1000) * 1000;
            }

            return MaximumQuickCash;
        }

        #endregion IApplicationInformationQuickCash Members
    }
}