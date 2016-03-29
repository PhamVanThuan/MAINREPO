using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.ApplicationDomain.Queries
{
    public class DoesOpenApplicationExistForPropertyAndClientQuery : ServiceQuery<bool>, IApplicationDomainQuery
    {
        [Required]
        public int PropertyKey { get; protected set; }

        [Required]
        public string ClientIDNumber { get; protected set; }

        public DoesOpenApplicationExistForPropertyAndClientQuery(int propertyKey, string clientIDNumber)
        {
            PropertyKey = propertyKey;
            ClientIDNumber = clientIDNumber;
        }
    }
}
