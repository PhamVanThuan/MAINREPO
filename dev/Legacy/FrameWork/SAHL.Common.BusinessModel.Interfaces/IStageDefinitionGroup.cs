using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO
    /// </summary>
    public partial interface IStageDefinitionGroup : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.StageDefinitionStageDefinitionGroups
        /// </summary>
        IEventList<IStageDefinitionStageDefinitionGroup> StageDefinitionStageDefinitionGroups
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.GenericKeyType
        /// </summary>
        IGenericKeyType GenericKeyType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.ParentStageDefinitionGroup
        /// </summary>
        IStageDefinitionGroup ParentStageDefinitionGroup
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.ChildStageDefinitionGroups
        /// </summary>
        IEventList<IStageDefinitionGroup> ChildStageDefinitionGroups
        {
            get;
        }
    }
}