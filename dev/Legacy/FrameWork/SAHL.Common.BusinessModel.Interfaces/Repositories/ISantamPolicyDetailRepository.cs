namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface ISantamPolicyDetailRepository
    {
        ISANTAMPolicyTracking GetSantamPolicyByAccountKey(int AccountKey);
    }
}