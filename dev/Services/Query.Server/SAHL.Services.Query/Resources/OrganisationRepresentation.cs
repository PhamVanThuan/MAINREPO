using SAHL.Services.Query.Resources.OrganisationStructure;
using WebApi.Hal;

namespace SAHL.Services.Query.Controllers
{
    public class OrganisationRepresentation : Representation
    {
        private readonly ILinkResolver linkResolver;

        public OrganisationRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public override string Rel
        {
            get { return this.linkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return this.linkResolver.GetHref(this); }
        }

        protected override void CreateHypermedia()
        {
            base.CreateHypermedia();

            var link = this.linkResolver.GetLink(typeof (OrganisationStructureRepresentation).Name.Replace("Representation", "_root"));
            link.Rel = "structure";

            this.Links.Add(link);
        }
    }
}