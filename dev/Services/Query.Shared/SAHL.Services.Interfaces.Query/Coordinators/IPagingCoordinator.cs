using System;
using SAHL.Services.Interfaces.Query.Core;
using SAHL.Services.Interfaces.Query.Parsers;

namespace SAHL.Services.Interfaces.Query.Coordinators
{
    public interface IPagingCoordinator
    {
        void ApplyPaging(IListRepresentation listRepresentation, IFindQuery query, Func<int> getCount, Type type);
    }

}