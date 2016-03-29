namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetFirstThirdPartyInvoiceWhereFileExistsQueryResult
    {
        public GetFirstThirdPartyInvoiceWhereFileExistsQueryResult(string guid)
        {
            this.Guid = guid;
        }

        public string Guid { get; set; }
    }
}