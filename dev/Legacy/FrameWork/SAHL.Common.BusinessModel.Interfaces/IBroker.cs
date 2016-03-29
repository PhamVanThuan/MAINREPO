using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Broker_DAO
    /// </summary>
    public partial interface IBroker : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.ADUserName
        /// </summary>
        System.String ADUserName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.FullName
        /// </summary>
        System.String FullName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.Initials
        /// </summary>
        System.String Initials
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.TelephoneNumber
        /// </summary>
        System.String TelephoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.FaxNumber
        /// </summary>
        System.String FaxNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.EmailAddress
        /// </summary>
        System.String EmailAddress
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.Password
        /// </summary>
        System.String Password
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.PasswordQuestion
        /// </summary>
        System.String PasswordQuestion
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.PasswordAnswer
        /// </summary>
        System.String PasswordAnswer
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.ADUser
        /// </summary>
        IADUser ADUser
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.BrokerStatusNumber
        /// </summary>
        Int32? BrokerStatusNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.BrokerTypeNumber
        /// </summary>
        Int32? BrokerTypeNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.BrokerCommissionTrigger
        /// </summary>
        Int32? BrokerCommissionTrigger
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.BrokerMinimumSAHL
        /// </summary>
        Double? BrokerMinimumSAHL
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.BrokerMinimumSCMB
        /// </summary>
        Double? BrokerMinimumSCMB
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.BrokerPercentageSAHL
        /// </summary>
        Single? BrokerPercentageSAHL
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.BrokerPercentageSCMB
        /// </summary>
        Single? BrokerPercentageSCMB
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Broker_DAO.BrokerTarget
        /// </summary>
        Double? BrokerTarget
        {
            get;
            set;
        }
    }
}