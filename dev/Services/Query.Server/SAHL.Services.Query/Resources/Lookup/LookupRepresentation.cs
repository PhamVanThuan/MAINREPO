
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using SAHL.Services.Query.LinkTemplates;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Lookup
{
    public class LookupRepresentation : Representation
    {
        public int Id { get; set; }
        [JsonIgnore]
        public string LookupType { get; set; }
        public string Description { get; set; }

        public override string Rel
        {
            get { return LookupLinkTemplate.GetLookup(LookupType, Id).Rel; }
        }

        public override string Href
        {
            get { return LookupLinkTemplate.GetLookup(LookupType, Id).Href; }
        }

        protected override void CreateHypermedia()
        {
            base.CreateHypermedia();
            Links.Add(new Link { Href = LookupLinkTemplate.GetLookupType(LookupType).Href, Rel = "parent" });
        }

    }

}