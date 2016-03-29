using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IApplicationInformation : IEntityValidation
    {
        IApplicationProduct ApplicationProduct
        {
            get;
        }

        /// <summary>
        /// Gets the ApplicationInformationType when the object is first loaded.  This will not change during
        /// the lifetime of the object.  If you wish to change the status of the application and
        /// persist, use <see cref="ApplicationInformationType"/>.  For newly created applications, this will
        /// always be null.
        /// </summary>
        IApplicationInformationType ApplicationInformationTypePrevious { get; }
    }
}