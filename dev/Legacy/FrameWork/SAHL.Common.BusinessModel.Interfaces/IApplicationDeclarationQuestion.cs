using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO
    /// </summary>
    public partial interface IApplicationDeclarationQuestion : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO.DisplayQuestionDate
        /// </summary>
        System.Boolean DisplayQuestionDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO.DisplaySequence
        /// </summary>
        System.Int32 DisplaySequence
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO.ApplicationDeclarationQuestionAnswers
        /// </summary>
        IEventList<IApplicationDeclarationQuestionAnswer> ApplicationDeclarationQuestionAnswers
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO.ApplicationDeclarationQuestionAnswerConfigurations
        /// </summary>
        IEventList<IApplicationDeclarationQuestionAnswerConfiguration> ApplicationDeclarationQuestionAnswerConfigurations
        {
            get;
        }
    }
}