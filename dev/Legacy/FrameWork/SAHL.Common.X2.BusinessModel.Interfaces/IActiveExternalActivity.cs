using System;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IActiveExternalActivity : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        System.Int32 WorkFlowID
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        Int64? ActivatingInstanceID
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.DateTime ActivationTime
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String ActivityXMLData
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String WorkFlowProviderName
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
        IExternalActivity ExternalActivity
        {
            get;
            set;
        }
    }
}