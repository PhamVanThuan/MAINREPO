using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    public partial interface IAssetLiabilityLiabilityLoan : IEntityValidation, IBusinessModelObject, IAssetLiability
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilityLoan_DAO.DateRepayable
        /// </summary>
        DateTime? DateRepayable
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilityLoan_DAO.FinancialInstitution
        /// </summary>
        System.String FinancialInstitution
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilityLoan_DAO.InstalmentValue
        /// </summary>
        System.Double InstalmentValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilityLoan_DAO.LiabilityValue
        /// </summary>
        System.Double LiabilityValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilityLoan_DAO.LoanType
        /// </summary>
        IAssetLiabilitySubType LoanType
        {
            get;
            set;
        }
    }
}