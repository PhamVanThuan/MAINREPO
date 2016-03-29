using Capitec.Core.Identity.Exceptions;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Capitec.Core.Identity
{
    public class AuthenticationManager : Capitec.Core.Identity.IAuthenticationManager
    {
        private IUserDataManager userDataManager;
        private IPasswordManager passwordManager;
        private IHostContext hostContext;
        private IUnitOfWorkFactory unitOfWorkFactory;

        public AuthenticationManager(IUserDataManager userDataManager, IPasswordManager passwordManager, IHostContext hostContext, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.userDataManager = userDataManager;
            this.passwordManager = passwordManager;
            this.hostContext = hostContext;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Login(string username, string password, string ipAddress)
        {
            // get the user for the username and check it exists
            UserDataModel user = this.userDataManager.GetUserFromUsername(username);

            if (user == null)
            {
                throw new UsernameDoesNotExistException(username);
            }

            if (!user.IsActive)
            {
                throw new UserIsNotActiveException(username);
            }

            // validate the password
            bool passwordMatches = this.passwordManager.VerifyHashedPassword(user.PasswordHash, password);

            // If valid
            if (passwordMatches)
            {
                UserInformationDataModel userInfo = this.userDataManager.GetUserInformationFromUser(user.Id);
                IEnumerable<RoleDataModel> roles = this.userDataManager.GetRolesFromUser(user.Id);

                // Generate the session token
                Guid token = CombGuid.Instance.Generate();

                using (var uow = this.unitOfWorkFactory.Build())
                {
                    // Update the users session token and last activity time
                    this.userDataManager.UpdateUserToken(user.Id, token, ipAddress);

                    this.userDataManager.UpdateUserLoginAndActivity(user.Id);
                    uow.Complete();
                }
                // issue the security token
                this.hostContext.IssueSecurityToken(token);

                // set the current user
                this.hostContext.SetUser(new CapitecIdentity(true, username, user.Id, string.Format("{0} {1}", userInfo.FirstName, userInfo.LastName)), roles.Select(x => x.Name).ToArray());
            }
            else
            {
                throw new PasswordDoesNotMatchException();
            }
        }

        public void Logout(Guid userId)
        {
            this.hostContext.RevokeSecurityToken();

            this.userDataManager.RemoveUserToken(userId);
        }

        public void Authenticate(Guid authenticationToken)
        {
            UserDataModel user = this.userDataManager.GetUserFromToken(authenticationToken);
            if (user != null)
            {
                UserInformationDataModel userInfo = this.userDataManager.GetUserInformationFromUser(user.Id);
                IEnumerable<RoleDataModel> roles = this.userDataManager.GetRolesFromUser(user.Id);
                this.hostContext.SetUser(new CapitecIdentity(true, user.UserName, user.Id, string.Format("{0} {1}", userInfo.FirstName, userInfo.LastName)), roles.Select(x => x.Name).ToArray());

                this.hostContext.IssueSecurityToken(authenticationToken);
            }
        }

        public string EncryptString(string textToEncrypt, byte[] key, ref byte[] initiationVector)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;

                aes.GenerateIV();
                initiationVector = aes.IV;

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    byte[] encrypted;
                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(textToEncrypt);
                        }
                        encrypted = msEncrypt.ToArray();
                        return this.ByteArrayToHexString(encrypted);
                    }
                }
            }
        }

        public string DecryptString(string textToDecrypt, byte[] key, byte[] initiationVector)
        {
            //TripleDES tdes = TripleDES.Create()
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = initiationVector;

                byte[] encrypted = this.StringToByteArray(textToDecrypt);
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encrypted))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            string plainText = srDecrypt.ReadToEnd();
                            return plainText;
                        }
                    }
                }
            }
        }

        public string ByteArrayToHexString(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", "");
        }

        public byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}