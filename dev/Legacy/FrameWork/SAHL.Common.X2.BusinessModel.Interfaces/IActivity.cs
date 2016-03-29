using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IActivity : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        System.String Name
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Boolean SplitWorkFlow
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Int32 Priority
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String ActivityMessage
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Int32 ActivatedByExternalActivity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String ChainedActivityName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Int32 ID
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IEventList<IInstanceActivitySecurity> InstanceActivitySecurities
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        IEventList<ILog> Logs
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        IEventList<IStageActivity> StageActivities
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        IEventList<IWorkFlowActivity> WorkFlowActivities
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        IEventList<IWorkFlowHistory> WorkFlowHistories
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        IActivityType ActivityType
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IExternalActivity ExternalActivity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IExternalActivityTarget ExternalActivityTarget
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IForm Form
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IState NextState
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IState State
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IWorkFlow WorkFlow
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IEventList<ISecurityGroup> SecurityGroups
        {
            get;
        }
    }
}