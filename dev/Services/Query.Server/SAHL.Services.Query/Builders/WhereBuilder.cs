using System.Dynamic;
using System.Web.Helpers;
using Newtonsoft.Json;
using SAHL.Core.Serialisation;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Builders.Core;

namespace SAHL.Services.Query.Builders
{

    public interface IWhereBuilder
    {
        string BuildWhereFilter(IRelationshipDefinition relationshipDefinition);
    }

    public class WhereBuilder : IWhereBuilder
    {

        public string BuildWhereFilter(IRelationshipDefinition relationshipDefinition)
        {
            Filter jsonFilter = new Filter();

            foreach (var relatedField in relationshipDefinition.RelatedFields)
            {
                jsonFilter.Where.Add(relatedField.RelatedKey, relatedField.Value);
            }

            string json = JsonConvert.SerializeObject(jsonFilter);
            return "filter=" + json.ToLower();
        }

    }

}