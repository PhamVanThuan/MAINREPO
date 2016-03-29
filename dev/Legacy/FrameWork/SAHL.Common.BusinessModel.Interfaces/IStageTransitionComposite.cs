using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO
    /// </summary>
    public partial interface IStageTransitionComposite : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.GenericKey
        /// </summary>
        System.Int32 GenericKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.TransitionDate
        /// </summary>
        System.DateTime TransitionDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.StageTransitionReasonKey
        /// </summary>
        System.Int32 StageTransitionReasonKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.Comments
        /// </summary>
        System.String Comments
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.StageDefinitionStageDefinitionGroup
        /// </summary>
        IStageDefinitionStageDefinitionGroup StageDefinitionStageDefinitionGroup
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.StageTransition
        /// </summary>
        IStageTransition StageTransition
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.ADUser
        /// </summary>
        IADUser ADUser
        {
            get;
            set;
        }
    }
}