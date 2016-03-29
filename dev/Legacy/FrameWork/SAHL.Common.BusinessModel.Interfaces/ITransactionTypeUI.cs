using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.TransactionTypeUI_DAO
    /// </summary>
    public partial interface ITransactionTypeUI : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionTypeUI_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionTypeUI_DAO.TransactionType
        /// </summary>
        ITransactionType TransactionType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionTypeUI_DAO.ScreenBatch
        /// </summary>
        System.Int32 ScreenBatch
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionTypeUI_DAO.HTMLColour
        /// </summary>
        System.String HTMLColour
        {
            get;
            set;
        }
    }
}