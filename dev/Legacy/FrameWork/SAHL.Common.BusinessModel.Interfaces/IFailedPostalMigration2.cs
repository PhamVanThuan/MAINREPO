using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IFailedPostalMigration
    {
        /// <summary>
        /// Gets a formatted description of the address.
        /// </summary>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        string GetFormattedDescription(AddressDelimiters delimiter);
    }
}