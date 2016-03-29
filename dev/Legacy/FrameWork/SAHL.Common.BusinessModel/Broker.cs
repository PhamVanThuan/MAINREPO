using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Broker_DAO
    /// </summary>
    public partial class Broker : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Broker_DAO>, IBroker
    {
        public Broker(SAHL.Common.BusinessModel.DAO.Broker_DAO Broker)
            : base(Broker)
        {
            this._DAO = Broker;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.ADUserName
        /// </summary>
        public String ADUserName
        {
            get { return _DAO.ADUserName; }
            set { _DAO.ADUserName = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.FullName
        /// </summary>
        public String FullName
        {
            get { return _DAO.FullName; }
            set { _DAO.FullName = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.Initials
        /// </summary>
        public String Initials
        {
            get { return _DAO.Initials; }
            set { _DAO.Initials = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.TelephoneNumber
        /// </summary>
        public String TelephoneNumber
        {
            get { return _DAO.TelephoneNumber; }
            set { _DAO.TelephoneNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.FaxNumber
        /// </summary>
        public String FaxNumber
        {
            get { return _DAO.FaxNumber; }
            set { _DAO.FaxNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.EmailAddress
        /// </summary>
        public String EmailAddress
        {
            get { return _DAO.EmailAddress; }
            set { _DAO.EmailAddress = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.Password
        /// </summary>
        public String Password
        {
            get { return _DAO.Password; }
            set { _DAO.Password = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.PasswordQuestion
        /// </summary>
        public String PasswordQuestion
        {
            get { return _DAO.PasswordQuestion; }
            set { _DAO.PasswordQuestion = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.PasswordAnswer
        /// </summary>
        public String PasswordAnswer
        {
            get { return _DAO.PasswordAnswer; }
            set { _DAO.PasswordAnswer = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.GeneralStatus
        /// </summary>
        public IGeneralStatus GeneralStatus
        {
            get
            {
                if (null == _DAO.GeneralStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.GeneralStatus = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.ADUser
        /// </summary>
        public IADUser ADUser
        {
            get
            {
                if (null == _DAO.ADUser) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IADUser, ADUser_DAO>(_DAO.ADUser);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ADUser = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ADUser = (ADUser_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.BrokerStatusNumber
        /// </summary>
        public Int32? BrokerStatusNumber
        {
            get { return _DAO.BrokerStatusNumber; }
            set { _DAO.BrokerStatusNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.BrokerTypeNumber
        /// </summary>
        public Int32? BrokerTypeNumber
        {
            get { return _DAO.BrokerTypeNumber; }
            set { _DAO.BrokerTypeNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.BrokerCommissionTrigger
        /// </summary>
        public Int32? BrokerCommissionTrigger
        {
            get { return _DAO.BrokerCommissionTrigger; }
            set { _DAO.BrokerCommissionTrigger = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.BrokerMinimumSAHL
        /// </summary>
        public Double? BrokerMinimumSAHL
        {
            get { return _DAO.BrokerMinimumSAHL; }
            set { _DAO.BrokerMinimumSAHL = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.BrokerMinimumSCMB
        /// </summary>
        public Double? BrokerMinimumSCMB
        {
            get { return _DAO.BrokerMinimumSCMB; }
            set { _DAO.BrokerMinimumSCMB = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.BrokerPercentageSAHL
        /// </summary>
        public Single? BrokerPercentageSAHL
        {
            get { return _DAO.BrokerPercentageSAHL; }
            set { _DAO.BrokerPercentageSAHL = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.BrokerPercentageSCMB
        /// </summary>
        public Single? BrokerPercentageSCMB
        {
            get { return _DAO.BrokerPercentageSCMB; }
            set { _DAO.BrokerPercentageSCMB = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.BrokerTarget
        /// </summary>
        public Double? BrokerTarget
        {
            get { return _DAO.BrokerTarget; }
            set { _DAO.BrokerTarget = value; }
        }
    }
}