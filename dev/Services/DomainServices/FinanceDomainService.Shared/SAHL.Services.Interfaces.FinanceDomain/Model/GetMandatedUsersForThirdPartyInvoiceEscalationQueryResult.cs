
namespace SAHL.Services.Interfaces.FinanceDomain.Model
{
    public class GetMandatedUsersForThirdPartyInvoiceEscalationQueryResult
    {
        public int UserOrganisationStructureKey { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string ApproverDescription { get; set; }
    }
}
