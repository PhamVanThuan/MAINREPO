using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IState : IEntityValidation
    {
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
        System.String Name
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Boolean ForwardState
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IEventList<IActivity> NextActivities
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        IEventList<IActivity> Activities
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        IEventList<IInstance> Instances
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
        IEventList<IState> States
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
        IStateType StateType
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

        /// <summary>
        ///
        /// </summary>
        IEventList<IForm> Forms
        {
            get;
        }
    }
}