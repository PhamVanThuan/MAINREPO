namespace SAHL.Core.Caching
{
    public interface IHashGenerator
    {
        string GenerateHash(string keyToHash);
    }
}