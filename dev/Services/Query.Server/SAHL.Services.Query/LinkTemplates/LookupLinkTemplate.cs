using WebApi.Hal;

namespace SAHL.Services.Query.LinkTemplates
{
    public static class LookupLinkTemplate
    {
        public static Link GetLookups { get { return new Link("lookup", "~/api/lookup"); } }

        public static Link GetLookupType(string lookup)
        {
            return new Link(lookup, "~/api/lookup/" + lookup);
        }

        public static Link GetLookup(string lookupType, int id)
        {
            return GetLookup(lookupType, id.ToString());
        }
        
        public static Link GetLookup(string lookupType, string id)
        {
            return new Link(lookupType, "~/api/lookup/" + lookupType + "/" + id);
        }
    }

}