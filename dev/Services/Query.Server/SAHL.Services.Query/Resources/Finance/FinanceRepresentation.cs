using System.Collections;
using System.Collections.Generic;
using SAHL.Services.Query.Core;
using WebApi.Hal;

namespace SAHL.Services.Query.Controllers
{
    [DoesNotRequireAnIdProperty]
    public class FinanceRepresentation : Representation, IRepresentation
    {
        private readonly ILinkResolver linkResolver;

        public FinanceRepresentation(ILinkResolver linkResolver)
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
        }
    }
}