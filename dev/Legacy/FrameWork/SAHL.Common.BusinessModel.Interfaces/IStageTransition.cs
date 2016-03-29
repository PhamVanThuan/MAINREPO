using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.StageTransition_DAO
    /// </summary>
    public partial interface IStageTransition : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.GenericKey
        /// </summary>
        System.Int32 GenericKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.ADUser
        /// </summary>
        IADUser ADUser
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.TransitionDate
        /// </summary>
        System.DateTime TransitionDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.Comments
        /// </summary>
        System.String Comments
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.StageDefinitionStageDefinitionGroup
        /// </summary>
        IStageDefinitionStageDefinitionGroup StageDefinitionStageDefinitionGroup
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.EndTransitionDate
        /// </summary>
        DateTime? EndTransitionDate
        {
            get;
            set;
        }
    }
}