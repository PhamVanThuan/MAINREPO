using System;
using System.Globalization;
using Microsoft.Exchange.WebServices.Data;

namespace SAHL.Core.Exchange.Provider
{
    public static class ExchangeServiceFactory
    {
        
        public static ExchangeService Create(IExchangeProviderCredentials credentials)
        {
            return Create(credentials.ServerUrl, credentials.UserName, credentials.Password, credentials.MailBox);
        }

        public static ExchangeService Create(string serverUrl, string userName, string password, string mailBox)
        {
            var exchangeService = new ExchangeService(ExchangeVersion.Exchange2010_SP2);
            exchangeService.Url = new Uri(serverUrl);
            exchangeService.Credentials = new WebCredentials(userName, password);
            exchangeService.PreferredCulture = CultureInfo.CurrentCulture;
            return exchangeService;
        } 
    }
}