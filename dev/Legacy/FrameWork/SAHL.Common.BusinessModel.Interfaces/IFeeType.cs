using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FeeType_DAO
    /// </summary>
    public partial interface IFeeType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FeeType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FeeType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FeeType_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FeeType_DAO.FeeFrequency
        /// </summary>
        IFeeFrequency FeeFrequency
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FeeType_DAO.TransactionType
        /// </summary>
        ITransactionType TransactionType
        {
            get;
        }
    }
}