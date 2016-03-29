using System.Collections.Generic;
using SAHL.Core.Extensions;

namespace SAHL.Services.Query.Models.Core
{
    public class RelationshipDefinitionNavigationEqualityComparer : IEqualityComparer<RelationshipDefinitionNavigation>
    {
        public bool Equals(RelationshipDefinitionNavigation x, RelationshipDefinitionNavigation y)
        {
            var areEqual = HashCodeExtensions.GetHashCode(x) == HashCodeExtensions.GetHashCode(y);
            return areEqual;
        }

        public int GetHashCode(RelationshipDefinitionNavigation obj)
        {
            var hash = HashCodeExtensions.GetHashCode(obj.Source, obj.Target);
            return hash;
        }
    }
}