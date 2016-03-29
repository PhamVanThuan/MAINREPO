using SAHL.Core.Extensions;
using SAHL.Services.Interfaces.Query.Models.Core;

namespace SAHL.Services.Query.Models.Core
{
    public class RelationshipDefinitionNavigation
    {
        public RelationshipDefinitionNavigation(IRelationshipDefinition source, IRelationshipDefinition target)
        {
            this.Source = source;
            this.Target = target;
        }

        public IRelationshipDefinition Source { get; set; }
        public IRelationshipDefinition Target { get; set; }

        public override int GetHashCode()
        {
            return HashCodeExtensions.GetHashCode(this.Source, this.Target);
        }

        public override string ToString()
        {
            return GetNameForDefinition(this.Source) + " -> " + GetNameForDefinition(this.Target);
        }

        private string GetNameForDefinition(IRelationshipDefinition definition)
        {
            return definition == null
                ? "null"
                : definition.GetName();
        }
    }
}