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
    /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO
    /// </summary>
    public partial class BatchTotal : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BatchTotal_DAO>, IBatchTotal
    {
        public BatchTotal(SAHL.Common.BusinessModel.DAO.BatchTotal_DAO BatchTotal)
            : base(BatchTotal)
        {
            this._DAO = BatchTotal;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.Amount
        /// </summary>
        public Double Amount
        {
            get { return _DAO.Amount; }
            set { _DAO.Amount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.DebitOrderDate
        /// </summary>
        public DateTime DebitOrderDate
        {
            get { return _DAO.DebitOrderDate; }
            set { _DAO.DebitOrderDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.ActionDate
        /// </summary>
        public DateTime ActionDate
        {
            get { return _DAO.ActionDate; }
            set { _DAO.ActionDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.TransactionTypeKey
        /// </summary>
        public Int32 TransactionTypeKey
        {
            get { return _DAO.TransactionTypeKey; }
            set { _DAO.TransactionTypeKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.ManualDebitOrders
        /// </summary>
        private DAOEventList<ManualDebitOrder_DAO, IManualDebitOrder, ManualDebitOrder> _ManualDebitOrders;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.ManualDebitOrders
        /// </summary>
        public IEventList<IManualDebitOrder> ManualDebitOrders
        {
            get
            {
                if (null == _ManualDebitOrders)
                {
                    if (null == _DAO.ManualDebitOrders)
                        _DAO.ManualDebitOrders = new List<ManualDebitOrder_DAO>();
                    _ManualDebitOrders = new DAOEventList<ManualDebitOrder_DAO, IManualDebitOrder, ManualDebitOrder>(_DAO.ManualDebitOrders);
                    _ManualDebitOrders.BeforeAdd += new EventListHandler(OnManualDebitOrders_BeforeAdd);
                    _ManualDebitOrders.BeforeRemove += new EventListHandler(OnManualDebitOrders_BeforeRemove);
                    _ManualDebitOrders.AfterAdd += new EventListHandler(OnManualDebitOrders_AfterAdd);
                    _ManualDebitOrders.AfterRemove += new EventListHandler(OnManualDebitOrders_AfterRemove);
                }
                return _ManualDebitOrders;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.Account
        /// </summary>
        public IAccount Account
        {
            get
            {
                if (null == _DAO.Account) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAccount, Account_DAO>(_DAO.Account);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Account = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Account = (Account_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.TransactionType
        /// </summary>
        public ITransactionType TransactionType
        {
            get
            {
                if (null == _DAO.TransactionType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ITransactionType, TransactionType_DAO>(_DAO.TransactionType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.TransactionType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.TransactionType = (TransactionType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ManualDebitOrders = null;
        }
    }
}