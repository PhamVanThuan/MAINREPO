using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CapCreditBrokerToken_DAO
    /// </summary>
    public partial interface ICapCreditBrokerToken : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapCreditBrokerToken_DAO.LastAssigned
        /// </summary>
        System.Boolean LastAssigned
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapCreditBrokerToken_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapCreditBrokerToken_DAO.Broker
        /// </summary>
        IBroker Broker
        {
            get;
            set;
        }
    }
}