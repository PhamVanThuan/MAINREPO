using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Condition_DAO
    /// </summary>
    public partial interface ICondition : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.ConditionPhrase
        /// </summary>
        System.String ConditionPhrase
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.TokenDescriptions
        /// </summary>
        System.String TokenDescriptions
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.TranslatableItem
        /// </summary>
        ITranslatableItem TranslatableItem
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.ConditionName
        /// </summary>
        System.String ConditionName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.ConditionSetConditions
        /// </summary>
        IEventList<IConditionSetCondition> ConditionSetConditions
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.ConditionTokens
        /// </summary>
        IEventList<IConditionToken> ConditionTokens
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.ConditionType
        /// </summary>
        IConditionType ConditionType
        {
            get;
            set;
        }
    }
}