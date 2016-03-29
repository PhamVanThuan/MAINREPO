using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.LifePolicyClaim_DAO
    /// </summary>
    public partial interface ILifePolicyClaim : IEntityValidation, IBusinessModelObject
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
        /// SAHL.Common.BusinessModel.DAO.LifePolicyClaim_DAO.ClaimStatus
        /// </summary>
        IClaimStatus ClaimStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicyClaim_DAO.ClaimType
        /// </summary>
        IClaimType ClaimType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicyClaim_DAO.ClaimDate
        /// </summary>
        DateTime ClaimDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicyClaim_DAO.FinancialService
        /// </summary>
        IFinancialService FinancialService
        {
            get;
            set;
        }
    }
}