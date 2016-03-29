using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO
    /// </summary>
    public partial interface IQuestionnaireEmail : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO.EmailSubject
        /// </summary>
        System.String EmailSubject
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO.EmailBody
        /// </summary>
        System.String EmailBody
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO.ContentType
        /// </summary>
        IContentType ContentType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO.QuestionnaireQuestionnaireEmails
        /// </summary>
        IEventList<IQuestionnaireQuestionnaireEmail> QuestionnaireQuestionnaireEmails
        {
            get;
        }
    }
}