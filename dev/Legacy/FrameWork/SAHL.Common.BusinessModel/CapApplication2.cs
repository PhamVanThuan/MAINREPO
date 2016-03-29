using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class CapApplication : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CapApplication_DAO>, ICapApplication
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="Rules"></param>
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("ApplicationCAP2NextQuarter");
            Rules.Add("ApplicationCAP2QualifyStatus");
            Rules.Add("ApplicationCap2QualifyUnderCancel");
            Rules.Add("ApplicationCap2QualifyInterestOnly");
            Rules.Add("ApplicationCap2QualifyProduct");
            Rules.Add("ApplicationCap2QualifySPVLTV");
            Rules.Add("ApplicationCap2QualifySPVPTI");
            Rules.Add("ApplicationCap2QualifySPVBondAmount");
            Rules.Add("ApplicationCap2QualifySPVBondPercent");
            Rules.Add("ApplicationCap2QualifySPVLoanAgreementPercent");
            Rules.Add("ApplicationCap2QualifySPVMinBal");
            Rules.Add("ApplicationCap2QualifySPVMaxBal");
            Rules.Add("ApplicationCap2QualifyDebtCounselling");
            Rules.Add("ApplicationCAP2SPVBack2Back");
            Rules.Add("ApplicationCap2QualifyActiveCap");
            Rules.Add("ApplicationCap2DebitOrderPaymentOption");
            //Rules.Add("ApplicationCap2CheckReadvanceRequired");
            Rules.Add("ApplicationCap2AccountResetDateCheck");
            //Rules.Add("ApplicationCap2CheckLTVThreshold");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnCapApplicationDetails_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnCapApplicationDetails_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnCapApplicationDetails_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnCapApplicationDetails_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? CapitalisationDate
        {
            get { return _DAO.CapitalisationDate; }

            //This value is only set by the backend when the cap offer is capitalised
            //Done by the pBatchRaiseCapPremium stored procedure
            //set { _DAO.CapitalisationDate = value; }
        }
    }
}