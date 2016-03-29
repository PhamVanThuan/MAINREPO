using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CapPaymentOption_DAO
    /// </summary>
    public partial class CapPaymentOption : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CapPaymentOption_DAO>, ICapPaymentOption
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapPaymentOption_DAO.CapOffers
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnCapOffers_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapPaymentOption_DAO.CapOffers
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnCapOffers_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapPaymentOption_DAO.CapOffers
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnCapOffers_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapPaymentOption_DAO.CapOffers
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnCapOffers_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}