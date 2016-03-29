namespace SAHL.Services.Interfaces.DomainQuery.Model
{
    public class GetApplicantDetailsForOfferQueryResult
    {
        public bool IsMainApplicant { get; set; }

        public string IdentityNumber { get; set; }

        public string FirstNames { get; set; }

        public string Surname { get; set; }
    }
}