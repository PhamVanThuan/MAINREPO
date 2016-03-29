using System;
using System.Collections.Generic;
using SAHL.Services.Query.Metadata;
using WebApi.Hal;

namespace SAHL.Services.Query.Controllers.Test
{
    [ServiceGenerationToolExclude]
    public class TestListRepresentation : Representation
    {
        private readonly ILinkResolver linkResolver;
        private IList<Representation> itemsBacking;
        private IList<Representation> items;

        public IEnumerable<Test2Representation> Test2 { get; set; }

        public IList<Representation> Items
        {
            get { return this.items; }
            set
            {
                this.items = value;
                if (value != null)
                {
                    this.itemsBacking = value;
                }
            }
        }

        public TestListRepresentation(ILinkResolver linkResolver, IList<Representation> items)
        {
            this.linkResolver = linkResolver;
            this.items = items;
        }

        public override string Href
        {
            get
            {
                return linkResolver.GetHref(this);
            }
        }

        public override string Rel
        {
            get { return linkResolver.GetRel(this); }
        }

        protected override void CreateHypermedia()
        {
            base.CreateHypermedia();

            foreach (var item in this.itemsBacking)
            {
                Links.Add(new Link(item.Rel, item.Href));
            }
        }
    }
}