using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AnswerType_DAO
    /// </summary>
    public partial class AnswerType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AnswerType_DAO>, IAnswerType
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AnswerType_DAO.Answers
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnAnswers_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AnswerType_DAO.Answers
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnAnswers_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AnswerType_DAO.Answers
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnAnswers_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AnswerType_DAO.Answers
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnAnswers_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}