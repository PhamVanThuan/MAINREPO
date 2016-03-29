namespace SAHL.Common.Factories
{
    public interface IFactoryStrategy
    {
        T CreateType<T>(params object[] Parameters);
    }
}