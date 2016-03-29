using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO
    /// </summary>
    public partial class BatchTotal : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BatchTotal_DAO>, IBatchTotal
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.ManualDebitOrders
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnManualDebitOrders_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.ManualDebitOrders
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnManualDebitOrders_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.ManualDebitOrders
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnManualDebitOrders_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.ManualDebitOrders
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnManualDebitOrders_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}