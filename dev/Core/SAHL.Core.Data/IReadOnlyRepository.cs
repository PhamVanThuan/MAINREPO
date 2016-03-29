namespace SAHL.Core.Data
{
    public interface IReadOnlyRepository
    {
        T GetByKey<T, K>(K key);
    }
}