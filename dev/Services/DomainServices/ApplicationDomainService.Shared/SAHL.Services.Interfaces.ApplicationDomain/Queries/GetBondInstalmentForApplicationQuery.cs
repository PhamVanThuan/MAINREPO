using SAHL.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Queries
{
    public class GetBondInstalmentForApplicationQuery : ServiceQuery<double?>, IApplicationDomainQuery
    {
        public GetBondInstalmentForApplicationQuery(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        [Required]
        public int ApplicationKey { get; protected set; }
    }
}