namespace Capitec.Core.Identity
{
    public interface IPasswordManager
    {
        string HashPassword(string password);

        bool VerifyHashedPassword(string hashedPassword, string password);
    }
}