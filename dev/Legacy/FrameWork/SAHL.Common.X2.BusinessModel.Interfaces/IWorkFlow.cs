using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IWorkFlow : IEntityValidation
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
        System.DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String StorageTable
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String StorageKey
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Int32 GenericKeyTypeKey
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String DefaultSubject
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
        IEventList<IActivity> Activities
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
        IEventList<ISecurityGroup> SecurityGroups
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
        IEventList<IWorkFlow> WorkFlows
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        IEventList<IWorkFlowActivity> NextWorkFlowActivities
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
        IProcess Process
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IWorkFlow WorkFlowAncestor
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IWorkFlowIcon WorkFlowIcon
        {
            get;
            set;
        }
    }
}