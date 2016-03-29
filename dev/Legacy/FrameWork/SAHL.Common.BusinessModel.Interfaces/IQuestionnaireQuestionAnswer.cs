using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionAnswer_DAO
    /// </summary>
    public partial interface IQuestionnaireQuestionAnswer : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionAnswer_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionAnswer_DAO.Sequence
        /// </summary>
        System.Int32 Sequence
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionAnswer_DAO.Answer
        /// </summary>
        IAnswer Answer
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionAnswer_DAO.QuestionnaireQuestion
        /// </summary>
        IQuestionnaireQuestion QuestionnaireQuestion
        {
            get;
            set;
        }
    }
}