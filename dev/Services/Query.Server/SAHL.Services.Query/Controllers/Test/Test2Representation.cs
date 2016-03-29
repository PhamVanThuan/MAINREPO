using SAHL.Services.Query.Metadata;
using WebApi.Hal;

namespace SAHL.Services.Query.Controllers.Test
{
    [ServiceGenerationToolExclude]
    public class Test2Representation : Representation
    {
        private readonly ILinkResolver linkResolver;
        public int Id { get; set; }
        public string Type { get; set; }

        private int? testIdBacking;
        private int? testId;
        public int? TestId
        {
            get
            {
                return this.testId;
            }
            set
            {
                this.testId = value;
                if (value != null)
                {
                    this.testIdBacking = value;
                }
            }
        }

        public Test2Representation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public override string Rel
        {
            get { return linkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return linkResolver.GetHref(this, new { id = Id }); }
        }

        protected override void CreateHypermedia()
        {
            base.CreateHypermedia();

            Links.Add(new Link(linkResolver.GetRel(typeof(TestRepresentation)), linkResolver.GetHref(typeof(TestRepresentation), new { id = this.testIdBacking })));
        }
    }
}