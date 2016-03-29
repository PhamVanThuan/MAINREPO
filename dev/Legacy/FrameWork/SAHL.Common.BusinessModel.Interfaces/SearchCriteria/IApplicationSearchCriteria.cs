using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Interfaces.SearchCriteria
{
    /// <summary>
    /// Defines the criteria that can be set when searching applications.
    /// </summary>
    public interface IApplicationSearchCriteria
    {
        int? AccountKey { get; set; }

        string ClientName { get; set; }

        string ConsultantADUserName { get; set; }

        bool ApplicationHasAccount { get; set; }

        bool IsEmpty { get; }

        List<SAHL.Common.Globals.OfferTypes> ApplicationTypes { get; }

        Dictionary<string, string> WorkflowsAndProcesses { get; }

        List<SAHL.Common.Globals.OfferRoleTypes> ConsultantRoleTypes { get; }

        List<SAHL.Common.Globals.OfferStatuses> ApplicationStatuses { get; }

        /// <summary>
        /// Gets/sets whether all search criteria must be matched exactly.
        /// </summary>
        bool ExactMatch { get; set; }
    }
}