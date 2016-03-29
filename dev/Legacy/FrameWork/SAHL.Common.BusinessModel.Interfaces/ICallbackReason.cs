using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface ICallbackReason : IEntityValidation
    {
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
        System.Int32 CallbackReasonKey
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IEventList<ICallback> Callbacks
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        IOriginationSourceProduct OriginationSourceProduct
        {
            get;
            set;
        }
    }
}