using System;

namespace Capitec.Core.Identity
{
    public interface IAuthenticationManager
    {
        void Authenticate(Guid authenticationToken);

        string DecryptString(string textToDecrypt, byte[] key, byte[] initiationVector);

        string EncryptString(string textToEncrypt, byte[] key, ref byte[] initiationVector);

        void Login(string username, string password, string ipAddress);

        void Logout(Guid userId);
    }
}