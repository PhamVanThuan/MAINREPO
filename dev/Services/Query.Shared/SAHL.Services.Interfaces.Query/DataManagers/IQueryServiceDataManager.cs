using System;
using System.Collections.Generic;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;

namespace SAHL.Services.Interfaces.Query.DataManagers
{
    public interface IQueryServiceDataManager
    {
        IEnumerable<IQueryDataModel> GetList(IFindQuery findQuery);
        IQueryDataModel GetById(string id, IFindQuery findQuery);
        IQueryDataModel GetOne(IFindQuery findQuery);
        int GetCount(IFindQuery findQuery);
    }
}