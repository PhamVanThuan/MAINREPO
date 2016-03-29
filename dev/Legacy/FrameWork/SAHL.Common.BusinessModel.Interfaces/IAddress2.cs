using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IAddress : IEntityValidation
    {
        /// <summary>
        /// Gets the address as a formatted string.
        /// </summary>
        /// <param name="Delimiter">The string used to split the various address lines.</param>
        /// <returns></returns>
        string GetFormattedDescription(AddressDelimiters Delimiter);

        /// <summary>
        /// Gets the Address Format
        /// </summary>
        IAddressFormat AddressFormat { get; }
    }
}