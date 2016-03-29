using System;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IWorkFlowHistory : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        System.Int64 InstanceID
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String CreatorADUserName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.DateTime CreationDate
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.DateTime StateChangeDate
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        DateTime? DeadlineDate
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.DateTime ActivityDate
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String ADUserName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        Int32? Priority
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
        IActivity Activity
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
    }
}