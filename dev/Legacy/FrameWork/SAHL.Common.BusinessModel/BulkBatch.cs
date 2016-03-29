using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO
    /// </summary>
    public partial class BulkBatch : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BulkBatch_DAO>, IBulkBatch
    {
        public BulkBatch(SAHL.Common.BusinessModel.DAO.BulkBatch_DAO BulkBatch)
            : base(BulkBatch)
        {
            this._DAO = BulkBatch;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.IdentifierReferenceKey
        /// </summary>
        public Int32 IdentifierReferenceKey
        {
            get { return _DAO.IdentifierReferenceKey; }
            set { _DAO.IdentifierReferenceKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.EffectiveDate
        /// </summary>
        public DateTime EffectiveDate
        {
            get { return _DAO.EffectiveDate; }
            set { _DAO.EffectiveDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.StartDateTime
        /// </summary>
        public DateTime? StartDateTime
        {
            get { return _DAO.StartDateTime; }
            set { _DAO.StartDateTime = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.CompletedDateTime
        /// </summary>
        public DateTime? CompletedDateTime
        {
            get { return _DAO.CompletedDateTime; }
            set { _DAO.CompletedDateTime = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.FileName
        /// </summary>
        public String FileName
        {
            get { return _DAO.FileName; }
            set { _DAO.FileName = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.UserID
        /// </summary>
        public String UserID
        {
            get { return _DAO.UserID; }
            set { _DAO.UserID = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.BatchTransactions
        /// </summary>
        private DAOEventList<BatchTransaction_DAO, IBatchTransaction, BatchTransaction> _BatchTransactions;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.BatchTransactions
        /// </summary>
        public IEventList<IBatchTransaction> BatchTransactions
        {
            get
            {
                if (null == _BatchTransactions)
                {
                    if (null == _DAO.BatchTransactions)
                        _DAO.BatchTransactions = new List<BatchTransaction_DAO>();
                    _BatchTransactions = new DAOEventList<BatchTransaction_DAO, IBatchTransaction, BatchTransaction>(_DAO.BatchTransactions);
                    _BatchTransactions.BeforeAdd += new EventListHandler(OnBatchTransactions_BeforeAdd);
                    _BatchTransactions.BeforeRemove += new EventListHandler(OnBatchTransactions_BeforeRemove);
                    _BatchTransactions.AfterAdd += new EventListHandler(OnBatchTransactions_AfterAdd);
                    _BatchTransactions.AfterRemove += new EventListHandler(OnBatchTransactions_AfterRemove);
                }
                return _BatchTransactions;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.BulkBatchLogs
        /// </summary>
        private DAOEventList<BulkBatchLog_DAO, IBulkBatchLog, BulkBatchLog> _BulkBatchLogs;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.BulkBatchLogs
        /// </summary>
        public IEventList<IBulkBatchLog> BulkBatchLogs
        {
            get
            {
                if (null == _BulkBatchLogs)
                {
                    if (null == _DAO.BulkBatchLogs)
                        _DAO.BulkBatchLogs = new List<BulkBatchLog_DAO>();
                    _BulkBatchLogs = new DAOEventList<BulkBatchLog_DAO, IBulkBatchLog, BulkBatchLog>(_DAO.BulkBatchLogs);
                    _BulkBatchLogs.BeforeAdd += new EventListHandler(OnBulkBatchLogs_BeforeAdd);
                    _BulkBatchLogs.BeforeRemove += new EventListHandler(OnBulkBatchLogs_BeforeRemove);
                    _BulkBatchLogs.AfterAdd += new EventListHandler(OnBulkBatchLogs_AfterAdd);
                    _BulkBatchLogs.AfterRemove += new EventListHandler(OnBulkBatchLogs_AfterRemove);
                }
                return _BulkBatchLogs;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.BulkBatchParameters
        /// </summary>
        private DAOEventList<BulkBatchParameter_DAO, IBulkBatchParameter, BulkBatchParameter> _BulkBatchParameters;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.BulkBatchParameters
        /// </summary>
        public IEventList<IBulkBatchParameter> BulkBatchParameters
        {
            get
            {
                if (null == _BulkBatchParameters)
                {
                    if (null == _DAO.BulkBatchParameters)
                        _DAO.BulkBatchParameters = new List<BulkBatchParameter_DAO>();
                    _BulkBatchParameters = new DAOEventList<BulkBatchParameter_DAO, IBulkBatchParameter, BulkBatchParameter>(_DAO.BulkBatchParameters);
                    _BulkBatchParameters.BeforeAdd += new EventListHandler(OnBulkBatchParameters_BeforeAdd);
                    _BulkBatchParameters.BeforeRemove += new EventListHandler(OnBulkBatchParameters_BeforeRemove);
                    _BulkBatchParameters.AfterAdd += new EventListHandler(OnBulkBatchParameters_AfterAdd);
                    _BulkBatchParameters.AfterRemove += new EventListHandler(OnBulkBatchParameters_AfterRemove);
                }
                return _BulkBatchParameters;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.BulkBatchStatus
        /// </summary>
        public IBulkBatchStatus BulkBatchStatus
        {
            get
            {
                if (null == _DAO.BulkBatchStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IBulkBatchStatus, BulkBatchStatus_DAO>(_DAO.BulkBatchStatus);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.BulkBatchStatus = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.BulkBatchStatus = (BulkBatchStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatch_DAO.BulkBatchType
        /// </summary>
        public IBulkBatchType BulkBatchType
        {
            get
            {
                if (null == _DAO.BulkBatchType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IBulkBatchType, BulkBatchType_DAO>(_DAO.BulkBatchType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.BulkBatchType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.BulkBatchType = (BulkBatchType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _BatchTransactions = null;
            _BulkBatchLogs = null;
            _BulkBatchParameters = null;
        }
    }
}