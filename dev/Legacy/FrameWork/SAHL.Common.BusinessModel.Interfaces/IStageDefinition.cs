using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO
    /// </summary>
    public partial interface IStageDefinition : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.HasCompositeLogic
        /// </summary>
        System.Boolean HasCompositeLogic
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.Name
        /// </summary>
        System.String Name
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.StageDefinitionStageDefinitionGroups
        /// </summary>
        IEventList<IStageDefinitionStageDefinitionGroup> StageDefinitionStageDefinitionGroups
        {
            get;
        }
    }
}