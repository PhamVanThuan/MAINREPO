using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IFailedStreetMigration : IEntityValidation
    {
        /// <summary>
        /// Gets a formatted description of the address.
        /// </summary>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        string GetFormattedDescription(AddressDelimiters delimiter);
    }
}