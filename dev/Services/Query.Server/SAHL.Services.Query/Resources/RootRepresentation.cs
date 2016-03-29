using System.Collections.Generic;
using StructureMap;
using WebApi.Hal;

namespace SAHL.Services.Query.Controllers
{
    public class RootRepresentation : Representation
    {
        private readonly ILinkResolver linkResolver;

        public RootRepresentation(ILinkResolver linkResolver)
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

            //TODO: Inject this?
            var links = ObjectFactory.GetNamedInstance<IEnumerable<Link>>("RootRepresentationInstances");
            foreach (var item in links)
            {
                this.Links.Add(item);
            }
        }
    }
}