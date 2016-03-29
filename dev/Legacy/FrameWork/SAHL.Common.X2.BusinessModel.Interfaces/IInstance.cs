using System;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IInstance : IEntityValidation
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
        System.String Subject
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String WorkFlowProvider
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
        DateTime? StateChangeDate
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
        DateTime? ActivityDate
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String ActivityADUserName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        Int32? ActivityID
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
        System.Int64 ID
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
        IEventList<IWorkList> WorkLists
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        IInstance ParentInstance
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
        Int64? SourceInstanceID
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        Int32? ReturnActivityID
        {
            get;
            set;
        }
    }
}