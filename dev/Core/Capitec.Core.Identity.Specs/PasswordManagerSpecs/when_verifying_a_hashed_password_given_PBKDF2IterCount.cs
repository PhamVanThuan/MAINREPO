using Machine.Fakes;
using Machine.Specifications;

namespace Capitec.Core.Identity.Specs.PasswordManagerSpecs
{
    public class when_verifying_a_hashed_password_given_PBKDF2IterCount : WithFakes
    {
        static PasswordManager passwordManager;
        private static string password;
        private static string hashedPassword;
        private static bool result;

        Establish context = () =>
        {
            password = "password";
            passwordManager = new PasswordManager();
            hashedPassword = "AG9+Q0npRJVm4bXbbPYj4nJBzbWSCywkmdDj09gMTsTnVpzBnbQJ0yVmaVnEAkimQA==";
        };

        Because of = () =>
        {
            result = passwordManager.VerifyHashedPassword(hashedPassword, password);
        };

        It should_have_a_verified_hashed_password = () =>
        {
            result.ShouldBeTrue();
        };
    }
}