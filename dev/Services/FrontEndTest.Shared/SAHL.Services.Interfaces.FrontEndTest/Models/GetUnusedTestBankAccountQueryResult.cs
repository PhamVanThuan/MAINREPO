using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetUnusedTestBankAccountQueryResult
    {
        public string ACBBankDescription { get; set; }

        public string ACBBranchCode { get; set; }

        public string ACBBranchDescription { get; set; }

        public string AccountNumber { get; set; }

        public ACBType ACBTypeNumber { get; set; }

        public string ACBTypeDescription { get; set; }
    }
}