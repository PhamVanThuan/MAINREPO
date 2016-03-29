using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Queries
{
    public class FindClientByPassportNumberQuery : ServiceQuery<ClientDetailsQueryResult>, IClientDomainQuery, ISqlServiceQuery<ClientDetailsQueryResult>
    {
        [Required]
        public string PassportNumber { get; protected set; }

        public FindClientByPassportNumberQuery(string passportNumber)
        {
            PassportNumber = passportNumber;
        }
    }
}
