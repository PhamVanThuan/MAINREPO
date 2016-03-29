using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BulkBatchLog_DAO
    /// </summary>
    public partial interface IBulkBatchLog : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchLog_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchLog_DAO.MessageReference
        /// </summary>
        System.String MessageReference
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchLog_DAO.MessageReferenceKey
        /// </summary>
        System.String MessageReferenceKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchLog_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchLog_DAO.BulkBatch
        /// </summary>
        IBulkBatch BulkBatch
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchLog_DAO.MessageType
        /// </summary>
        IMessageType MessageType
        {
            get;
            set;
        }
    }
}