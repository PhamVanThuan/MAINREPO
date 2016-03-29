using System;
using SAHL.Services.Query.Core;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Attorney
{
    public class AttorneyContactRepresentation : Representation, IRepresentation
    {

        public int Id { get; set; }
        public int? AttorneyKey { get; set; }
        public Guid? AttorneyId { get; set; }
        public string CellPhoneNumber { get; set; }
        public string HomePhoneCode { get; set; }
        public string HomePhoneNumber { get; set; }
        public string WorkPhoneCode { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string FaxCode { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }

        private ILinkResolver LinkResolver { get; set; }

        public AttorneyContactRepresentation(ILinkResolver linkResolver)
        {
            this.LinkResolver = linkResolver;
        }
        
        public override string Rel
        {
            get { return this.LinkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return this.LinkResolver.GetHref(this, new { attorneyid = this.AttorneyId }); }
        }
        
    }


}