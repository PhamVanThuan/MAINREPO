namespace SAHL.VSExtensions.Interfaces
{
    public interface ISAHLProject
    {
        bool Build();

        string GetLatestBuildLocation();
    }
}