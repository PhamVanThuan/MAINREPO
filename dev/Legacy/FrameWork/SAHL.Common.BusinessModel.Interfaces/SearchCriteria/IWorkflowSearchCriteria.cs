using System.Collections.Generic;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces.SearchCriteria
{
    public interface IWorkflowSearchCriteria
    {
        List<string> UserFilter { get; }

        List<IWorkflowSearchCriteriaWorkflowFilter> WorkflowFilter { get; }

        string ApplicationNumber { get; set; }

        List<OfferTypes> ApplicationTypes { get; set; }

        bool IncludeHistoricUsers { get; set; }

        bool IncludeSystemStates { get; set; }

        int MaxResults { get; set; }

        string CreatorUser { get; set; }

        string Firstname { get; set; }

        string Surname { get; set; }

        string IDNumber { get; set; }

        bool Cap2Search { get; set; }
    }

    public interface IWorkflowSearchCriteriaWorkflowFilter
    {
        int WorkflowID { get; }

        List<int> States { get; }
    }
}