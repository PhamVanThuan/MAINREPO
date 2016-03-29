using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.SPVBalance_DAO
    /// </summary>
    public partial interface ISPVBalance : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVBalance_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVBalance_DAO.Amount
        /// </summary>
        System.Double Amount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVBalance_DAO.SPV
        /// </summary>
        ISPV SPV
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVBalance_DAO.BalanceType
        /// </summary>
        IBalanceType BalanceType
        {
            get;
            set;
        }
    }
}