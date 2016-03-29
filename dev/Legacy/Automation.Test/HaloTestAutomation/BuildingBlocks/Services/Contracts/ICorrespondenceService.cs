namespace BuildingBlocks.Services.Contracts
{
    public interface ICorrespondenceService
    {
        void UpdateDefaultEmailAddress(int correspondenceTemplateKey, string emailAddress);
    }
}