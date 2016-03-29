using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionnaireEmail_DAO
    /// </summary>
    public partial interface IQuestionnaireQuestionnaireEmail : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionnaireEmail_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionnaireEmail_DAO.InternalEmail
        /// </summary>
        System.Int32 InternalEmail
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionnaireEmail_DAO.Questionnaire
        /// </summary>
        IQuestionnaire Questionnaire
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionnaireEmail_DAO.QuestionnaireEmail
        /// </summary>
        IQuestionnaireEmail QuestionnaireEmail
        {
            get;
            set;
        }
    }
}