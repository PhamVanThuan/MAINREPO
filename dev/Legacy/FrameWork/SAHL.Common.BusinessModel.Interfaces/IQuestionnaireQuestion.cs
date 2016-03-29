using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO
    /// </summary>
    public partial interface IQuestionnaireQuestion : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO.Sequence
        /// </summary>
        System.Int32 Sequence
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO.QuestionAnswers
        /// </summary>
        IEventList<IQuestionnaireQuestionAnswer> QuestionAnswers
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO.Question
        /// </summary>
        IQuestion Question
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO.Questionnaire
        /// </summary>
        IQuestionnaire Questionnaire
        {
            get;
            set;
        }
    }
}