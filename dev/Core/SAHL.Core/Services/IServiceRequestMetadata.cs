using System.Collections.Generic;

namespace SAHL.Core.Services
{
    public interface IServiceRequestMetadata : IDictionary<string, string>
    {
        string UserName { get; }

        int? UserOrganisationStructureKey { get; }

        string CurrentUserRole { get; }

        string[] CurrentUserCapabilities { get; }
    }
}