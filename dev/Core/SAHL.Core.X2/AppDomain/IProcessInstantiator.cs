namespace SAHL.Core.X2.AppDomain
{
    public interface IProcessInstantiator
    {
        IX2Process GetProcess(int processId);

        void UnloadAll();
    }
}