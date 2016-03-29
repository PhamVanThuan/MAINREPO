using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.TransactionTypeDataAccess_DAO
    /// </summary>
    public partial interface ITransactionTypeDataAccess : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionTypeDataAccess_DAO.ADCredentials
        /// </summary>
        System.String ADCredentials
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionTypeDataAccess_DAO.TransactionTypeKey
        /// </summary>
        System.Int32 TransactionTypeKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionTypeDataAccess_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}