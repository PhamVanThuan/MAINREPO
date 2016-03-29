using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Models.Core;

namespace SAHL.Services.Interfaces.Query.Models
{
    public interface IQueryDataModel : IDataModel
    {
        //TODO: this needs to be an IEnumerable rather than a list 
        //however there is another conflicting interface on the implementers 
        //with a property that currently prevents this
        List<IRelationshipDefinition> Relationships { get; set; }
    }
}
