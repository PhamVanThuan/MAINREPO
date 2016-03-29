using System.Security.Cryptography;
using System.Text;

namespace SAHL.Core.Caching
{
    public class DefaultHashGenerator : IHashGenerator
    {
        public string GenerateHash(string keyToHash)
        {
            MD5 hasher = MD5.Create();
            return Encoding.UTF8.GetString(hasher.ComputeHash(Encoding.UTF8.GetBytes(keyToHash)));
        }
    }
}