namespace SAHL.Core.Caching
{
    public interface ICacheKeyGeneratorFactory<T>
    {
        string GetKey<U>(T context);
    }
}