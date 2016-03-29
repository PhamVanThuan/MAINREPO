using SAHL.Services.Query.Core;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Process
{
    [ServiceGenerationToolExclude]
    public class ProcessRepresentation: Representation, IRepresentation
    {
        
        private ILinkResolver linkResolver;

        public ProcessRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public string Id { get; set; }
        public string Process { get; set; }
        public string Stage { get; set; }
        public string AccountKey { get; set; }
      
        public override string Rel
        {
            get { return this.linkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return this.linkResolver.GetHref(this, new { id = this.AccountKey }); }
        }
        
    }

}