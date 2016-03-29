using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Interfaces.SearchCriteria
{
    /// <summary>
    /// Defines the criteria that can be set when searching accounts.
    /// </summary>
    public interface IAccountSearchCriteria
    {
        int? AccountKey { get; set; }

        string Surname { get; set; }

        string FirstNames { get; set; }

        bool IsEmpty { get; }

        List<SAHL.Common.Globals.Products> Products { get; }

        /// <summary>
        /// Gets/sets whether all search criteria must be matched exactly.
        /// </summary>
        bool ExactMatch { get; set; }

        /// <summary>
        /// Gets/sets whether distinct records should be returned.
        /// </summary>
        bool Distinct { get; set; }
    }
}