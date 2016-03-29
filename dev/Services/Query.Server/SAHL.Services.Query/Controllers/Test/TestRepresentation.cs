using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Resources;
using WebApi.Hal;

namespace SAHL.Services.Query.Controllers.Test
{
    [ServiceGenerationToolExclude]
    public class TestRepresentation : Representation, IListRepresentation
    {
        public IList<Representation> List { get; set; }
        public IPagingRepresentation _paging { get; set; }

        public int? TotalCount { get; set; }
        public int? ListCount { get; set; }

        private ILinkResolver LinkResolver { get; set; }

        public int Id { get; set; }
        public string Value { get; set; }

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

        protected override void CreateHypermedia()
        {
            base.CreateHypermedia();

            var linkingId = 10;
            //pretend we've discovered our links, and we're linking to a test2
            Links.Add(new Link(LinkResolver.GetRel(typeof(Test2Representation)), LinkResolver.GetHref(typeof(Test2Representation), new { id = linkingId  })));
        }

        
    }
}
