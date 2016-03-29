using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO
    /// </summary>
    public partial class ApplicationAggregateDecision : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO>, IApplicationAggregateDecision
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO.OfferCreditScores
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnOfferCreditScores_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO.OfferCreditScores
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnOfferCreditScores_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO.OfferCreditScores
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnOfferCreditScores_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO.OfferCreditScores
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnOfferCreditScores_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}