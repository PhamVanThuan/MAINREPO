using SAHL.Core.Data;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements
{
    public class GetThirdPartyInvoiceEmailSubjectStatement : ISqlStatement<ThirdPartyEmailSubjectModel>
    {
        public GetThirdPartyInvoiceEmailSubjectStatement(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }

        public int ThirdPartyInvoiceKey { get; protected set; }

        public string GetStatement()
        {
            return "select EmailSubject from [2am].datastor.ThirdPartyInvoicesSTOR where ThirdPartyInvoiceKey=@thirdPartyInvoiceKey";
        }
    } 
}