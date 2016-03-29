namespace SAHL.Core.Exchange.Provider
{
    public class ExchangeProviderCredentials : IExchangeProviderCredentials
    {
        public string ServerUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MailBox { get; set; }
        public string ArchiveFolder { get; set; }
    }
}