using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SAHL.Services.Query.Controllers;
using SAHL.Services.Query.Metadata;
using WebApi.Hal;
using WebApi.Hal.Interfaces;

namespace SAHL.Services.Query.Resources.OrganisationStructure
{
    [ServiceGenerationToolExclude]
    public class OrganisationStructureRepresentation : Representation
    {
        private readonly ILinkResolver linkResolver;
        private Link link;

        public int? Id { get; private set; }

        public string Name { get; private set; }

        public IEnumerable<OrganisationStructureRepresentation> Children { get; set; }
        public IEnumerable<OrganisationTypeRepresentation> Type { get; set; }

        public OrganisationStructureRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        [Obsolete("Use ValueInjecter to set properties")]
        internal OrganisationStructureRepresentation(ILinkResolver linkResolver, int? id, string name, int? typeId, string type, IList<OrganisationStructureRepresentation> children = null)
        {
            this.linkResolver = linkResolver;
            this.Id = id;
            this.Name = name;

            this.Children = children;

            if (typeId != null)
            {
                this.Type = new OrganisationTypeRepresentation(linkResolver, typeId.Value, type).ToSingleItemEnumerable();
            }
        }

        public override string Rel
        {
            get { return linkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return linkResolver.GetHref(this, new { id = Id }); }
        }
    }
}
