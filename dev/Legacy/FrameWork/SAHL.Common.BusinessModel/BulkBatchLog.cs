using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BulkBatchLog_DAO
    /// </summary>
    public partial class BulkBatchLog : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BulkBatchLog_DAO>, IBulkBatchLog
    {
        public BulkBatchLog(SAHL.Common.BusinessModel.DAO.BulkBatchLog_DAO BulkBatchLog)
            : base(BulkBatchLog)
        {
            this._DAO = BulkBatchLog;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchLog_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchLog_DAO.MessageReference
        /// </summary>
        public String MessageReference
        {
            get { return _DAO.MessageReference; }
            set { _DAO.MessageReference = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchLog_DAO.MessageReferenceKey
        /// </summary>
        public String MessageReferenceKey
        {
            get { return _DAO.MessageReferenceKey; }
            set { _DAO.MessageReferenceKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchLog_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchLog_DAO.BulkBatch
        /// </summary>
        public IBulkBatch BulkBatch
        {
            get
            {
                if (null == _DAO.BulkBatch) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IBulkBatch, BulkBatch_DAO>(_DAO.BulkBatch);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.BulkBatch = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.BulkBatch = (BulkBatch_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchLog_DAO.MessageType
        /// </summary>
        public IMessageType MessageType
        {
            get
            {
                if (null == _DAO.MessageType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IMessageType, MessageType_DAO>(_DAO.MessageType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.MessageType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.MessageType = (MessageType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}