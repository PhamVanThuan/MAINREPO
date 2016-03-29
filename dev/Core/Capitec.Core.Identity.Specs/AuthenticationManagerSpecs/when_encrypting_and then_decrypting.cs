using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Identity;
using System.Text;

namespace Capitec.Core.Identity.Specs.AuthenticationManagerSpecs
{
    public class when_encrypting_and_then_decrypting : WithFakes
    {
        private static IAuthenticationManager authenticationManager;
        private static IUserDataManager userDataManager;
        private static IPasswordManager passwordManager;
        private static IHostContext hostContext;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static string originalString;
        private static string encryptedString;
        private static string decryptedString;
        private static byte[] key;
        private static byte[] iv;

        private Establish context = () =>
        {
            key = Encoding.UTF8.GetBytes("293CE3575B14531C"); // NB MUST BE 16 CHARACTERS

            originalString = "Some Secret Sauce!";
            userDataManager = An<IUserDataManager>();
            passwordManager = An<IPasswordManager>();
            hostContext = An<IHostContext>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            authenticationManager = new AuthenticationManager(userDataManager, passwordManager, hostContext, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            encryptedString = authenticationManager.EncryptString(originalString, key, ref iv);
            decryptedString = authenticationManager.DecryptString(encryptedString, key, iv);
        };

        private It should_dosomething = () =>
        {
            decryptedString.ShouldEqual(originalString);
        };
    }
}