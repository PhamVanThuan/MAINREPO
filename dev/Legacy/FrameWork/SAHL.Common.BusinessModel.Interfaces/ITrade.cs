using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Trade_DAO
    /// </summary>
    public partial interface ITrade : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Trade_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Trade_DAO.ResetConfiguration
        /// </summary>
        IResetConfiguration ResetConfiguration
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Trade_DAO.TradeType
        /// </summary>
        System.String TradeType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Trade_DAO.Company
        /// </summary>
        System.String Company
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Trade_DAO.TradeDate
        /// </summary>
        System.DateTime TradeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Trade_DAO.StartDate
        /// </summary>
        System.DateTime StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Trade_DAO.EndDate
        /// </summary>
        System.DateTime EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Trade_DAO.StrikeRate
        /// </summary>
        System.Double StrikeRate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Trade_DAO.TradeBalance
        /// </summary>
        System.Double TradeBalance
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Trade_DAO.CapBalance
        /// </summary>
        System.Double CapBalance
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Trade_DAO.Premium
        /// </summary>
        Double? Premium
        {
            get;
            set;
        }
    }
}