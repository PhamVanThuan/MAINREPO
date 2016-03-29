using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface ISecurityGroup : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        System.Boolean IsDynamic
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
        System.String Description
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
        IProcess Process
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
        IEventList<IActivity> Activities
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
    }
}