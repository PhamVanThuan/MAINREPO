using System;
using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IApplicationLife : IEntityValidation, IApplication
    {
        /// <summary>
        ///
        /// </summary>
        System.Double DeathBenefit
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Double InstallmentProtectionBenefit
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Double DeathBenefitPremium
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Double InstallmentProtectionPremium
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.DateTime DateOfExpiry
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        DateTime? DateOfAcceptance
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Decimal UpliftFactor
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Decimal JointDiscountFactor
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Double MonthlyPremium
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Double YearlyPremium
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        Double SumAssured
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        DateTime? DateLastUpdated
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IADUser Consultant
        {
            get;
            //set;
        }

        /// <summary>
        ///
        /// </summary>
        Double? CurrentSumAssured
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        Double? PremiumShortfall
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IInsurer Insurer
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String ExternalPolicyNumber
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        DateTime? DateCeded
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IPriority Priority
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        ILegalEntity PolicyHolderLegalEntity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String RPARInsurer
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String RPARPolicyNumber
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String ConsultantADUserName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        ILifePolicyType LifePolicyType
        {
            get;
            set;
        }
    }
}