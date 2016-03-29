using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO
    /// </summary>
    public partial interface ISuperLo : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.ElectionDate
        /// </summary>
        System.DateTime ElectionDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.ConvertedDate
        /// </summary>
        System.DateTime ConvertedDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.NextPaymentDate
        /// </summary>
        System.DateTime NextPaymentDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.PPThresholdYr1
        /// </summary>
        System.Double PPThresholdYr1
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.PPThresholdYr2
        /// </summary>
        System.Double PPThresholdYr2
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.PPThresholdYr3
        /// </summary>
        System.Double PPThresholdYr3
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.PPThresholdYr4
        /// </summary>
        System.Double PPThresholdYr4
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.PPThresholdYr5
        /// </summary>
        System.Double PPThresholdYr5
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.MTDLoyaltyBenefit
        /// </summary>
        System.Double MTDLoyaltyBenefit
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.PPAllowed
        /// </summary>
        System.Double PPAllowed
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.Exclusion
        /// </summary>
        Boolean? Exclusion
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.ExclusionEndDate
        /// </summary>
        DateTime? ExclusionEndDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.ExclusionReason
        /// </summary>
        System.String ExclusionReason
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.OverPaymentAmount
        /// </summary>
        System.Double OverPaymentAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.FinancialServiceAttribute
        /// </summary>
        IFinancialServiceAttribute FinancialServiceAttribute
        {
            get;
            set;
        }
    }
}