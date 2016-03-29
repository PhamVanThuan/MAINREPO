using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class ApplicationDeclarationQuestion : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO>, IApplicationDeclarationQuestion
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationDeclarations_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationDeclarations_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationDeclarationQuestionAnswers_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationDeclarationQuestionAnswers_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationDeclarationQuestionAnswerConfigurations_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationDeclarationQuestionAnswerConfigurations_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationDeclarations_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationDeclarations_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationDeclarationQuestionAnswers_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationDeclarationQuestionAnswers_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationDeclarationQuestionAnswerConfigurations_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationDeclarationQuestionAnswerConfigurations_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}