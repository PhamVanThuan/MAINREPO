using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Answer_DAO
    /// </summary>
    public partial interface IAnswer : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Answer_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Answer_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Answer_DAO.AnswerImages
        /// </summary>
        IEventList<IAnswerImage> AnswerImages
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Answer_DAO.QuestionnaireQuestionAnswers
        /// </summary>
        IEventList<IQuestionnaireQuestionAnswer> QuestionnaireQuestionAnswers
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Answer_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Answer_DAO.AnswerType
        /// </summary>
        IAnswerType AnswerType
        {
            get;
            set;
        }
    }
}