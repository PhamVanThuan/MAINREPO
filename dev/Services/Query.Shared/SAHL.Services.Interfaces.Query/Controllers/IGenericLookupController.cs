using System.Collections.Generic;
using System.Linq;
using SAHL.Services.Interfaces.Query.Models;

namespace SAHL.Services.Interfaces.Query.Controllers
{
    public interface IGenericLookupController
    {
        IEnumerable<ILookupDataModel> Get(string lookUpType);
        ILookupDataModel Get(string lookUpType, int id);
    }
}