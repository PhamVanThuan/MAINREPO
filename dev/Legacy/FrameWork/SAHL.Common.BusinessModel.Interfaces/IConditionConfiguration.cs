using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ConditionConfiguration_DAO
    /// </summary>
    public partial interface IConditionConfiguration : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionConfiguration_DAO.GenericColumnDefinitionValue
        /// </summary>
        System.Int32 GenericColumnDefinitionValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionConfiguration_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionConfiguration_DAO.GenericKeyType
        /// </summary>
        IGenericKeyType GenericKeyType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionConfiguration_DAO.GenericColumnDefinition
        /// </summary>
        IGenericColumnDefinition GenericColumnDefinition
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionConfiguration_DAO.ConditionSets
        /// </summary>
        IEventList<IConditionSet> ConditionSets
        {
            get;
        }
    }
}