using SAHL.Services.Query.Core;
using WebApi.Hal;

namespace SAHL.Services.Query.Server.Tests.Representations
{
    public class TestRepresentation : Representation, IRepresentation
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
            LinkResolver = linkResolver;
        }

        public override string Rel
        {
            get { return LinkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return LinkResolver.GetHref(this, new { id = Id }); }
        }


    }
}