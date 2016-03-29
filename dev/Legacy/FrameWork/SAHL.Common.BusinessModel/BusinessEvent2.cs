using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BusinessEvent_DAO
    /// </summary>
    public partial class BusinessEvent : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BusinessEvent_DAO>, IBusinessEvent
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BusinessEvent_DAO.BusinessEventQuestionnaires
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnBusinessEventQuestionnaires_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BusinessEvent_DAO.BusinessEventQuestionnaires
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnBusinessEventQuestionnaires_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BusinessEvent_DAO.BusinessEventQuestionnaires
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnBusinessEventQuestionnaires_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BusinessEvent_DAO.BusinessEventQuestionnaires
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnBusinessEventQuestionnaires_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}