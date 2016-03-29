using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// OfferCondition_DAO is instantiated in order to find the Conditions which are associated to a particular Application.
    /// </summary>
    public partial interface IApplicationCondition : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The Condition which is associated to the Application.
        /// </summary>
        ICondition Condition
        {
            get;
            set;
        }

        /// <summary>
        /// Each Condition is related to a TranslatableItem, from which the translated version of the condition can be retrieved.
        /// </summary>
        ITranslatableItem TranslatableItem
        {
            get;
            set;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCondition_DAO.ApplicationConditionTokens
        /// </summary>
        IEventList<IApplicationConditionToken> ApplicationConditionTokens
        {
            get;
        }

        /// <summary>
        /// The Application Number from the Offer table
        /// </summary>
        IApplication Application
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCondition_DAO.ConditionSet
        /// </summary>
        IConditionSet ConditionSet
        {
            get;
            set;
        }
    }
}