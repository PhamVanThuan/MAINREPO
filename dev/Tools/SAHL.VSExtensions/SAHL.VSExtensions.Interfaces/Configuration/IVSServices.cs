namespace SAHL.VSExtensions.Interfaces.Configuration
{
    public interface IVSServices
    {
        T GetService<T>();

        I GetService<I, T>();
    }
}