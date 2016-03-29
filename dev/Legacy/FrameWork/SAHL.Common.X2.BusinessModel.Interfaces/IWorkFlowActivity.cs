using System;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IWorkFlowActivity : IEntityValidation
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
        Int32? StateID
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
        IActivity NextActivity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IWorkFlow NextWorkFlow
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
    }
}