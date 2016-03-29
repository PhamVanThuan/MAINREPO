using System;
using SAHL.Services.Query.Core;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Finance
{
    public class ThirdPartyInvoiceAccountRepresentation : Representation, IRepresentation
    {
        private ILinkResolver linkResolver;

        public ThirdPartyInvoiceAccountRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public int Id { get; set; }
        public int? AccountKey { get; set; }
        public int? AccountStatusKey { get; set; }
        public DateTime? ChangeDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public decimal? FixedPayment { get; set; }
        public DateTime? OpenDate { get; set; }
        public int? SPVKey { get; set; }
        public string SpvDescription { get; set; }
        public string WorkflowProcess { get; set; }
        public string WorkflowStage { get; set; }
        public string AssignedTo { get; set; }

        public override string Rel
        {
            get
            { return this.linkResolver.GetRel(this); }
        }

        public override string Href
        {
            get
            { return this.linkResolver.GetHref(this, new { id = this.Id }); }
        }

    }

}