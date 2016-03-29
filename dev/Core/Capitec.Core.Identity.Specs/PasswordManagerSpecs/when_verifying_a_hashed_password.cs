using Machine.Fakes;
using Machine.Specifications;

namespace Capitec.Core.Identity.Specs.PasswordManagerSpecs
{
    public class when_verifying_a_hashed_password : WithFakes
    {
        static PasswordManager passwordManager;
        private static string password;
        private static string hashedPassword;
        private static bool result;

        Establish context = () =>
        {
            password = "password";
            passwordManager = new PasswordManager();
            hashedPassword = "1F400.ACD1M0GA90CFyB6H82d1lVhIeD3s4IdUC3XBibekwTI9tRYYvoaBkSXlv0SVq9MFFQ==";
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