using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.ClientDomain.Queries
{
    public class FindClientByIdNumberQuery : ServiceQuery<ClientDetailsQueryResult>, IClientDomainQuery, ISqlServiceQuery<ClientDetailsQueryResult>
    {
        [Required]
        public string IdNumber { get; protected set; }

        public FindClientByIdNumberQuery(string IdNumber)
        {
            this.IdNumber = IdNumber;
        }
    }
}
