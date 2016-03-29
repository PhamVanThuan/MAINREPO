using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;
using System;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO
    /// </summary>
    public partial interface IDisabilityClaim : IEntityValidation, IBusinessModelObject
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
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.ClaimantLegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.DateClaimReceived
        /// </summary>
        DateTime DateClaimReceived
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.ClaimantLastWorkingDate
        /// </summary>
        DateTime? LastDateWorked
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.DateOfDiagnosis
        /// </summary>
        DateTime? DateOfDiagnosis
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.ClaimantOccupation
        /// </summary>
        string ClaimantOccupation
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.DisabilityType
        /// </summary>
        IDisabilityType DisabilityType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.OtherDisabilityComments
        /// </summary>
        string OtherDisabilityComments
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.ExpectedReturnToWorkDate
        /// </summary>
        DateTime? ExpectedReturnToWorkDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.DisabilityClaimStatus
        /// </summary>
        IDisabilityClaimStatus DisabilityClaimStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.PaymentStartDate
        /// </summary>
        DateTime? PaymentStartDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.NumberOfInstalmentsAuthorised
        /// </summary>
        int? NumberOfInstalmentsAuthorised
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.PaymentEndDate
        /// </summary>
        DateTime? PaymentEndDate
        {
            get;
            set;
        }
    }
}