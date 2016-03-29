using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.Models;

namespace SAHL.Services.Interfaces.Capitec.Queries
{
    [AuthorisedCommand(Roles = "User")]
    public class FilterCitiesByNameQuery : ServiceQuery<FilterCitiesByNameQueryResult>, ISqlServiceQuery<FilterCitiesByNameQueryResult>
    {
        public string CityNameFilter { get; protected set; }

        public FilterCitiesByNameQuery(string cityNameFilter)
        {
            this.CityNameFilter = cityNameFilter;
        }
    }
}