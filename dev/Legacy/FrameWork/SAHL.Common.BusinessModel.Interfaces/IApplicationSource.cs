using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// This class specifies the source of an application. ie. The Internet or Campaign etc.
    /// </summary>
    public partial interface IApplicationSource : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// A description of the application source.
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether the source is currently used.
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// The value used to identify a applicaition source.
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}