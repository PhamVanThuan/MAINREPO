using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO
    /// </summary>
    public partial interface IQuestionnaire : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.BusinessAreaGenericKey
        /// </summary>
        System.Int32 BusinessAreaGenericKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.BusinessAreaGenericKeyType
        /// </summary>
        IGenericKeyType BusinessAreaGenericKeyType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.Questions
        /// </summary>
        IEventList<IQuestionnaireQuestion> Questions
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.BusinessEventQuestionnaires
        /// </summary>
        IEventList<IBusinessEventQuestionnaire> BusinessEventQuestionnaires
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.QuestionnaireQuestionnaireEmails
        /// </summary>
        IEventList<IQuestionnaireQuestionnaireEmail> QuestionnaireQuestionnaireEmails
        {
            get;
        }
    }
}