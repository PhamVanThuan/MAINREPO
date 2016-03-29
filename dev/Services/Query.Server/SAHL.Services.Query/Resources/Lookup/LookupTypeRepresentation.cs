using SAHL.Services.Query.LinkTemplates;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Lookup
{
    public class LookupTypeRepresentation : Representation
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public override string Rel
        {
            get { return LookupLinkTemplate.GetLookupType(Id).Rel; }
        }

        public override string Href
        {
            get { return LookupLinkTemplate.GetLookupType(Id).Href; }
        }

        protected override void CreateHypermedia()
        {
            base.CreateHypermedia();
            Links.Add(new Link { Href = LookupLinkTemplate.GetLookups.Href, Rel = "parent" });
        }

    }
}