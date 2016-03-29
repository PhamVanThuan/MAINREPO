namespace SAHL.Batch.Common.ServiceContracts
{
    public interface IPersonalLoanLeadCreationService
    {
        bool CreatePersonalLoanLeadFromIdNumber(string idNumber, int messageId);
    }
}