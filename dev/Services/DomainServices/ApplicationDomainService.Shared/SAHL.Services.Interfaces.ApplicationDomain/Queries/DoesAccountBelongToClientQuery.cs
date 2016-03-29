using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Queries
{
    public class DoesAccountBelongToClientQuery : ServiceQuery<DoesAccountBelongToClientQueryResult>, IApplicationDomainQuery, ISqlServiceQuery<DoesAccountBelongToClientQueryResult>
    {
        [Required]
        public int AccountKey { get; protected set; }

        [Required]
        public string IdNumber { get; protected set; }

        public DoesAccountBelongToClientQuery(int accountKey, string idNumber)
        {
            AccountKey = accountKey;
            IdNumber = idNumber;
        }
    }
}
