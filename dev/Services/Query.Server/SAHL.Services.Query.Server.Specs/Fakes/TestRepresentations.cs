using SAHL.Services.Query.Core;

namespace SAHL.Services.Query.Server.Specs.Fakes
{
    public class TestRepresentation : WebApi.Hal.Representation, IRepresentation
    {
        public ILinkResolver LinkResolver { get; set; }
        public int Id { get; set; }
        public int? Count { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public TestRepresentation()
        {
            
        }

        public TestRepresentation(ILinkResolver linkResolver)
        {
            this.LinkResolver = linkResolver;
        }

        public override string Rel
        {
            get { return this.LinkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return this.LinkResolver.GetHref(this, new { id = this.Id }); }
        }
    }
}