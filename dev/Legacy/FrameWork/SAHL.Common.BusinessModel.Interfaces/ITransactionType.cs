using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.TransactionType_DAO
    /// </summary>
    public partial interface ITransactionType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.SPVDescription
        /// </summary>
        System.String SPVDescription
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.GLAccount
        /// </summary>
        System.String GLAccount
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.GLAccountContra
        /// </summary>
        System.String GLAccountContra
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.ReversalTransactionType
        /// </summary>
        ITransactionType ReversalTransactionType
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.TransactionGroups
        /// </summary>
        IEventList<ITransactionGroup> TransactionGroups
        {
            get;
        }
    }
}