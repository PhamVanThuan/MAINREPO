namespace BuildingBlocks.Services.Contracts
{
    public interface IControlService
    {
        void SetControlNumericByControlDescription(string controldescription, int controlNumeric);

        int GetControlNumericValue(string controlDescription);

        string GetControlTextValue(string controlDescription);
    }
}