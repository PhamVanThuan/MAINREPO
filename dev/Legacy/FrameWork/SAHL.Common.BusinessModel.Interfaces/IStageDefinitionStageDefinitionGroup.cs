using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Ties StageDefinition_DAO objects with StageDefinitionGroup_DAO objects.
    /// </summary>
    public partial interface IStageDefinitionStageDefinitionGroup : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageDefinitionGroup
        /// </summary>
        IStageDefinitionGroup StageDefinitionGroup
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageDefinition
        /// </summary>
        IStageDefinition StageDefinition
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.CompositeChildStageDefinitionStageDefinitionGroups
        /// </summary>
        IEventList<IStageDefinitionStageDefinitionGroup> CompositeChildStageDefinitionStageDefinitionGroups
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.CompositeParentStageDefinitionStageDefinitionGroups
        /// </summary>
        IEventList<IStageDefinitionStageDefinitionGroup> CompositeParentStageDefinitionStageDefinitionGroups
        {
            get;
        }
    }
}