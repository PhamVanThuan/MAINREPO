using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO
    /// </summary>
    public partial interface ILifePolicy : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Used for Activerecord exclusively, please use Key.
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DeathBenefit
        /// </summary>
        System.Double DeathBenefit
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.InstallmentProtectionBenefit
        /// </summary>
        System.Double InstallmentProtectionBenefit
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DeathBenefitPremium
        /// </summary>
        System.Double DeathBenefitPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.InstallmentProtectionPremium
        /// </summary>
        System.Double InstallmentProtectionPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DateOfCommencement
        /// </summary>
        DateTime? DateOfCommencement
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DateOfExpiry
        /// </summary>
        System.DateTime DateOfExpiry
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DeathRetentionLimit
        /// </summary>
        System.Double DeathRetentionLimit
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.InstallmentProtectionRetentionLimit
        /// </summary>
        System.Double InstallmentProtectionRetentionLimit
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.UpliftFactor
        /// </summary>
        System.Decimal UpliftFactor
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.JointDiscountFactor
        /// </summary>
        System.Decimal JointDiscountFactor
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DateOfCancellation
        /// </summary>
        DateTime? DateOfCancellation
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DeathReassuranceRetention
        /// </summary>
        Double? DeathReassuranceRetention
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.IPBReassuranceRetention
        /// </summary>
        Double? IPBReassuranceRetention
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.YearlyPremium
        /// </summary>
        System.Double YearlyPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DateOfAcceptance
        /// </summary>
        DateTime? DateOfAcceptance
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.SumAssured
        /// </summary>
        System.Double SumAssured
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DateLastUpdated
        /// </summary>
        DateTime? DateLastUpdated
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.Consultant
        /// </summary>
        System.String Consultant
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.CurrentSumAssured
        /// </summary>
        Double? CurrentSumAssured
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.PremiumShortfall
        /// </summary>
        Double? PremiumShortfall
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.ExternalPolicyNumber
        /// </summary>
        System.String ExternalPolicyNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DateCeded
        /// </summary>
        DateTime? DateCeded
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.ClaimStatusDate
        /// </summary>
        DateTime? ClaimStatusDate
        {
            get;
            set;
        }

        /// <summary>
        /// The Primary Legal entity for this policy.
        /// </summary>
        ILegalEntity PolicyHolderLE
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.RPARInsurer
        /// </summary>
        System.String RPARInsurer
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.RPARPolicyNumber
        /// </summary>
        System.String RPARPolicyNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.Broker
        /// </summary>
        IBroker Broker
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.Insurer
        /// </summary>
        IInsurer Insurer
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.ClaimStatus
        /// </summary>
        IClaimStatus ClaimStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.ClaimType
        /// </summary>
        IClaimType ClaimType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.LifePolicyStatus
        /// </summary>
        ILifePolicyStatus LifePolicyStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.Priority
        /// </summary>
        IPriority Priority
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.LifePolicyType
        /// </summary>
        ILifePolicyType LifePolicyType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.AnniversaryDate
        /// </summary>
        DateTime? AnniversaryDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.FinancialService
        /// </summary>
        IFinancialService FinancialService
        {
            get;
            set;
        }
    }
}