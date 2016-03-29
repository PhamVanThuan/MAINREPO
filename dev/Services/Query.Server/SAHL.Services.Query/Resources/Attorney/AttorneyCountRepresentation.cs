using System;
using SAHL.Services.Query.Core;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Attorney
{
    public class AttorneyCountRepresentation : Representation, IRepresentation
    {

        private ILinkResolver linkResolver;

        public AttorneyCountRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public int Count { get; set; }

        public override string Rel
        {
            get { return this.linkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return this.linkResolver.GetHref(this); }
        }
        
    }

}