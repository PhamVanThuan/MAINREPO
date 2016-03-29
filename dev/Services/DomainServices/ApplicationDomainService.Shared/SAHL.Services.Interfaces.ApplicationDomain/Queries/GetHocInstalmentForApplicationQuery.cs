using SAHL.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Queries
{
    public class GetHocInstalmentForApplicationQuery : ServiceQuery<double?>, IApplicationDomainQuery
    {
        public GetHocInstalmentForApplicationQuery(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        [Required]
        public int ApplicationKey { get; protected set; }
    }
}