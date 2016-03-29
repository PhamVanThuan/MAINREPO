using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ClientAnswer_DAO
    /// </summary>
    public partial interface IClientAnswer : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientAnswer_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientAnswer_DAO.ClientAnswerValue
        /// </summary>
        IClientAnswerValue ClientAnswerValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientAnswer_DAO.Answer
        /// </summary>
        IAnswer Answer
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientAnswer_DAO.QuestionnaireQuestion
        /// </summary>
        IQuestionnaireQuestion QuestionnaireQuestion
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientAnswer_DAO.ClientSurvey
        /// </summary>
        IClientQuestionnaire ClientSurvey
        {
            get;
            set;
        }
    }
}