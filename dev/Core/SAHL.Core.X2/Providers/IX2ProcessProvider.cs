namespace SAHL.Core.X2.Providers
{
    public interface IX2ProcessProvider
    {
        void Initialise();

        IX2Process GetProcessForInstance(long instanceID);
    }
}