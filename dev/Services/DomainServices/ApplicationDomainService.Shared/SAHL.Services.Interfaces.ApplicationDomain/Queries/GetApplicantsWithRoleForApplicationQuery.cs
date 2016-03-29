using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Queries
{
    public class GetApplicantsWithRoleForApplicationQuery : ServiceQuery<LegalEntityModel>, ISqlServiceQuery<LegalEntityModel>, IServiceQuery<IServiceQueryResult<LegalEntityModel>>, IApplicationDomainQuery, IServiceQuery, IServiceCommand
    {
        public GetApplicantsWithRoleForApplicationQuery(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        [Required]
        public int ApplicationKey { get; set; }
    }
}