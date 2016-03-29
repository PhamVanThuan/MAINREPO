
using SAHL.Services.Query.Core;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Treasury
{

    public class SPVRepresentation : Representation, IRepresentation
    {
        private ILinkResolver linkResolver;

        public SPVRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public int Id { get; set; }
        public string SPVDescription { get; set; }
        public string ReportDescription { get; set; }
        public string CreditProviderNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public int? BankAccountKey { get; set; }
        public int? GeneralStatusKey { get; set; }
        public int? ParentSPVKey { get; set; }
        public string GeneralStatus { get; set; }
        public string AccountName  { get; set; }
        public string BranchCode { get; set; }
        public string AccountNumber { get; set; }

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