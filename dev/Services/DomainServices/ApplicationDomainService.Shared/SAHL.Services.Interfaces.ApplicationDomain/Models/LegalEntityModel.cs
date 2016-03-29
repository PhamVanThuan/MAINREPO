namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class LegalEntityModel
    {
        public LegalEntityModel(int legalEntityKey, string legalEntityDescription)
        {
            LegalEntityKey = legalEntityKey;
            LegalEntityDescription = legalEntityDescription;
        }

        public int LegalEntityKey { get; protected set; }

        public string LegalEntityDescription { get; protected set; }
    }
}