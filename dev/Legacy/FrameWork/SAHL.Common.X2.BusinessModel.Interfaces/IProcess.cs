using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IProcess : IEntityValidation
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
        System.String Version
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
        System.Int32 ID
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IEventList<IProcess> Processes
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
        IEventList<IWorkFlow> WorkFlows
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        IProcess ProcessAncestor
        {
            get;
            set;
        }
    }
}