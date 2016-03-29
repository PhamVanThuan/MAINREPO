using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces.SearchCriteria
{
    /// <summary>
    /// Defines the criteria that can be set when searching addresses.
    /// </summary>
    public interface IAddressSearchCriteria
    {
        AddressFormats AddressFormat { get; set; }

        string BoxNumber { get; set; }

        string Country { get; set; }

        string BuildingNumber { get; set; }

        string BuildingName { get; set; }

        string ClusterBoxNumber { get; set; }

        string FreeTextLine1 { get; set; }

        string FreeTextLine2 { get; set; }

        string FreeTextLine3 { get; set; }

        string FreeTextLine4 { get; set; }

        string FreeTextLine5 { get; set; }

        bool IsEmpty { get; }

        string PostnetSuiteNumber { get; set; }

        int? PostOfficeKey { get; set; }

        string PrivateBagNumber { get; set; }

        string Province { get; set; }

        string StreetNumber { get; set; }

        string StreetName { get; set; }

        int? SuburbKey { get; set; }

        string UnitNumber { get; set; }

        /// <summary>
        /// Gets/sets whether all search criteria must be matched exactly.
        /// </summary>
        bool ExactMatch { get; set; }
    }
}