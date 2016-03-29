using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO
    /// </summary>
    public partial interface IAssetTransfer : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO.ClientSurname
        /// </summary>
        System.String ClientSurname
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO.SPVKey
        /// </summary>
        System.Int32 SPVKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO.LoanTotalBondAmount
        /// </summary>
        System.Double LoanTotalBondAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO.LoanCurrentBalance
        /// </summary>
        System.Double LoanCurrentBalance
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO.UserName
        /// </summary>
        System.String UserName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO.TransferedYN
        /// </summary>
        System.Char TransferedYN
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}