namespace SAHL.Core.Exchange
{
    public interface IExchangeProviderCredentials
    {
        string ServerUrl { get; }
        string UserName { get; }
        string Password { get; }
        string MailBox { get; }
        string ArchiveFolder { get; }
    }
}