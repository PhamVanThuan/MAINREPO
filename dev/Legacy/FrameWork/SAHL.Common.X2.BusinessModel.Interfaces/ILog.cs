using System;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface ILog : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        System.DateTime Date
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        Int32? ProcessID
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        Int32? WorkFlowID
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        Int32? InstanceID
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
        System.String Message
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String StackTrace
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