namespace SAHL.Services.Query.Server.Specs.Fakes
{
    public class TestWithALongNameRepresentation : WebApi.Hal.Representation
    {
        private readonly ILinkResolver linkResolver;

        public int? Id { get; set; }
        public string Value { get; set; }

        public TestWithALongNameRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public override string Rel
        {
            get { return this.linkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return this.linkResolver.GetHref(this, new { id = this.Id }); }
        }
    }
}