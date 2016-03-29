using SAHL.Common.BusinessModel.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class GetLegalEntityLegalNameCommand : StandardDomainServiceCommand
    {
        public GetLegalEntityLegalNameCommand(int legalEntityKey, LegalNameFormat legalNameFormat)
        {
            this.LegalEntityKey = legalEntityKey;
            this.LegalNameFormat = legalNameFormat;
        }

        public int LegalEntityKey
        {
            get;
            protected set;
        }

        public LegalNameFormat LegalNameFormat
        {
            get;
            protected set;
        }

        public string LegalNameResult { get; set; }
    }
}