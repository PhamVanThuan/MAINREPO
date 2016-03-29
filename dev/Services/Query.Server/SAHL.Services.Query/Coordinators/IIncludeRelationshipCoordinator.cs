using System.Collections.Generic;

namespace SAHL.Services.Query.Coordinators
{
    public interface IIncludeRelationshipCoordinator
    {
        void Fetch(IEnumerable<LinkQuery> urls);
    }
}