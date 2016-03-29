using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.UserProfiles;

namespace SAHL.Common.CacheData
{
    public interface ISAHLPrincipalCache
    {
        Dictionary<int, object> CBOMenus { get; }

        Dictionary<int, object> ContextMenus { get; }

        CBONodeSetType CurrentNodeSetType { get; set; }

        IDomainMessageCollection DomainMessages { get; set; }

        void SetMessageCollection(IDomainMessageCollection domainMessages);

        List<RuleExclusionSets> ExclusionSets { get; }

        List<int> FeatureKeys { get; }

        string GetCachedRolesAsStringForQuery(bool IncludeIdentityName, bool IncludeEveryone, bool IncludeQuotes = true);

        GlobalData GetGlobalData();

        PresenterData GetPresenterData();

        bool IgnoreWarnings { get; set; }

        bool IsAuthenticated { get; }

        int MenuVersion { get; set; }

        Dictionary<CBONodeSetType, object> NodeSets { get; }

        string OriginationSourceKeysStringForQuery { get; }

        SAHLPrincipal Principal { get; }

        UserProfile Profile { get; }

        List<string> Roles { get; }

        List<int> UserOriginationSourceKeys { get; }

        object UserToken { get; set; }

        object X2Info { get; set; }

        object X2Provider { get; set; }
    }
}