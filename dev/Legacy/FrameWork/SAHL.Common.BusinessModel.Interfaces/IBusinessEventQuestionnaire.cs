using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BusinessEventQuestionnaire_DAO
    /// </summary>
    public partial interface IBusinessEventQuestionnaire : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BusinessEventQuestionnaire_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BusinessEventQuestionnaire_DAO.BusinessEvent
        /// </summary>
        IBusinessEvent BusinessEvent
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BusinessEventQuestionnaire_DAO.Questionnaire
        /// </summary>
        IQuestionnaire Questionnaire
        {
            get;
            set;
        }
    }
}