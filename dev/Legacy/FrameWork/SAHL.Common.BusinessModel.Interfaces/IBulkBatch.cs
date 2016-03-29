using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO
    /// </summary>
    public partial interface IBulkBatch : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.IdentifierReferenceKey
        /// </summary>
        System.Int32 IdentifierReferenceKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.EffectiveDate
        /// </summary>
        System.DateTime EffectiveDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.StartDateTime
        /// </summary>
        DateTime? StartDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.CompletedDateTime
        /// </summary>
        DateTime? CompletedDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.FileName
        /// </summary>
        System.String FileName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.BatchTransactions
        /// </summary>
        IEventList<IBatchTransaction> BatchTransactions
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.BulkBatchLogs
        /// </summary>
        IEventList<IBulkBatchLog> BulkBatchLogs
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.BulkBatchParameters
        /// </summary>
        IEventList<IBulkBatchParameter> BulkBatchParameters
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.BulkBatchStatus
        /// </summary>
        IBulkBatchStatus BulkBatchStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.BulkBatchType
        /// </summary>
        IBulkBatchType BulkBatchType
        {
            get;
            set;
        }
    }
}