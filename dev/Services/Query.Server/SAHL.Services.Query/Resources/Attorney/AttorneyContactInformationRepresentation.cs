using System;
using SAHL.Services.Query.Core;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Attorney
{
    public class AttorneyContactInformationRepresentation : Representation, IRepresentation
    {

        public int Id { get; set; }
        public Guid? AttorneyId { get; set; }
        public int? AttorneyKey { get; set; }
        public string CellPhoneNumber { get; set; }
        public string HomePhoneCode { get; set; }
        public string HomePhoneNumber { get; set; }
        public string WorkPhoneCode { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string FaxCode { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }

        private ILinkResolver linkResolver { get; set; }

        public AttorneyContactInformationRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }
        
        public override string Rel
        {
            get { return this.linkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return this.linkResolver.GetHref(this, new { id = this.AttorneyId }); }
        }
        
    }


}
