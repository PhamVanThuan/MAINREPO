using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ConditionSet_DAO
    /// </summary>
    public partial interface IConditionSet : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSet_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSet_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSet_DAO.ConditionSetConditions
        /// </summary>
        IEventList<IConditionSetCondition> ConditionSetConditions
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSet_DAO.ConditionConfigurations
        /// </summary>
        IEventList<IConditionConfiguration> ConditionConfigurations
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSet_DAO.ConditionSetUIStatements
        /// </summary>
        IEventList<IConditionSetUIStatement> ConditionSetUIStatements
        {
            get;
        }
    }
}