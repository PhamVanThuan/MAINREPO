using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CapCreditBrokerToken_DAO
    /// </summary>
    public partial class CapCreditBrokerToken : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CapCreditBrokerToken_DAO>, ICapCreditBrokerToken
    {
        public CapCreditBrokerToken(SAHL.Common.BusinessModel.DAO.CapCreditBrokerToken_DAO CapCreditBrokerToken)
            : base(CapCreditBrokerToken)
        {
            this._DAO = CapCreditBrokerToken;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapCreditBrokerToken_DAO.LastAssigned
        /// </summary>
        public Boolean LastAssigned
        {
            get { return _DAO.LastAssigned; }
            set { _DAO.LastAssigned = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapCreditBrokerToken_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapCreditBrokerToken_DAO.Broker
        /// </summary>
        public IBroker Broker
        {
            get
            {
                if (null == _DAO.Broker) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IBroker, Broker_DAO>(_DAO.Broker);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Broker = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Broker = (Broker_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}