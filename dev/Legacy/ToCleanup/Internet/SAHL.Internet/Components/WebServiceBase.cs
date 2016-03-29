using System;
using System.Net;

namespace SAHL.Internet.Components
{
    using ApplicationService=SAHL.Web.Services.Application;
    using GlobalsService = SAHL.Web.Services.Globals;
    using CalculatorsService = SAHL.Web.Services.Calculators;
    using SurveyService = SAHL.Web.Services.Survey;
    using SendMailService = SAHL.Web.Services.SendMail;
    
    /// <summary>
    /// Provides access to the SA Home Loans web services.
    /// </summary>
    public class WebServiceBase : IDisposable
    {
        /// <summary>
        /// Gets the proxy server IP address.
        /// </summary>
        public static string ProxyIp
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["ProxyIp"]; }
        }

        /// <summary>
        /// Gets the proxy server port.
        /// </summary>
        public static int ProxyPort
        {
            get 
            {
                var result = System.Configuration.ConfigurationManager.AppSettings["ProxyPort"];
                return result == null ? 8080 : int.Parse(result);
            }
        }

        /// <summary>
        /// Indicates whether to use Active Directory credentials.
        /// </summary>
        public static bool UseAdCredentials
        {
            get
            {
                var result = System.Configuration.ConfigurationManager.AppSettings["useAdCredentials"];
                return result != null && bool.Parse(result);
            }
        }

        /// <summary>
        /// Indicates whether to use a proxy.
        /// </summary>
        public static bool UseProxy
        {
            get
            {
                var result = System.Configuration.ConfigurationManager.AppSettings["UseProxy"];
                return result != null && bool.Parse(result);
            }
        }

        /// <summary>
        /// Gets the proxy username.
        /// </summary>
        public static string ProxyUser
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["ProxyUser"]; }
        }

        /// <summary>
        /// Gets the proxy password.
        /// </summary>
        public static string ProxyPass
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["ProxyPass"]; }
        }

        /// <summary>
        /// Gets the proxy domain.
        /// </summary>
        public static string ProxyDomain
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["ProxyDomain"]; }
        }

        /// <summary>
        /// Gets the mail service URL.
        /// </summary>
        public static string MailServiceUrl
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["MailServiceURL"]; }
        }

        /// <summary>
        /// Gets the survey service URL.
        /// </summary>
        public static string SurveyServiceUrl
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["SurveyServiceURL"]; }
        }

        /// <summary>
        /// Gets the calculator service URL.
        /// </summary>
        public static string CalculatorsServiceUrl
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["CalculatorsServiceURL"]; }
        }

        /// <summary>
        /// Gets the global service URL.
        /// </summary>
        public static string GlobalsServiceUrl
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["GlobalsServiceURL"]; }
        }

        /// <summary>
        /// Gets the application service URL.
        /// </summary>
        public static string ApplicationServiceUrl
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["ApplicationServiceURL"]; }
        }
        
        /// <summary>
        /// Gets the global web service.
        /// </summary>
        public GlobalsService.Globals Globals
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the survey web service.
        /// </summary>
        public SurveyService.Survey Survey
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the calculator web service.
        /// </summary>
        public CalculatorsService.Calculators Calculators
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the application web service.
        /// </summary>
        public ApplicationService.Application Application
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the mail web service.
        /// </summary>
        public SendMailService.SendMail Mail
        {
            get;
            private set;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected void Dispose(bool disposing)
        {
            if (!disposing) return;

            Globals.Dispose();
            Application.Dispose();
            Calculators.Dispose();
            Survey.Dispose();
            Mail.Dispose();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceBase"/> class.
        /// </summary>
        public WebServiceBase()
        {
            // Initialize Web Services
            try
            {
                Globals = new GlobalsService.Globals { PreAuthenticate = true, UseDefaultCredentials = true, Url = GlobalsServiceUrl, Timeout = 5000 };
                Application = new ApplicationService.Application { PreAuthenticate = true, UseDefaultCredentials = true, Url = ApplicationServiceUrl, Timeout = 5000 };
                Calculators = new CalculatorsService.Calculators { PreAuthenticate = true, UseDefaultCredentials = true, Url = CalculatorsServiceUrl, Timeout = 5000 };
                Survey = new SurveyService.Survey { PreAuthenticate = true, UseDefaultCredentials = true, Url = SurveyServiceUrl, Timeout = 5000 };
                Mail = new SendMailService.SendMail { PreAuthenticate = true, UseDefaultCredentials = true, Url = MailServiceUrl, Timeout = 5000 };
            }
            catch
            {
                return;
            }

            if (!UseProxy) return;

            var proxy = new WebProxy(ProxyIp, ProxyPort);
            var credentials = new NetworkCredential(ProxyUser, ProxyPass);
            if (!string.IsNullOrWhiteSpace(ProxyDomain)) credentials.Domain = ProxyDomain;
            proxy.Credentials = credentials;

            Globals.Proxy = proxy;
            Globals.Credentials = credentials;

            Application.Proxy = proxy;
            Application.Credentials = credentials;

            Calculators.Proxy = proxy;
            Calculators.Credentials = credentials;

            Survey.Proxy = proxy;
            Survey.Credentials = credentials;

            Mail.Proxy = proxy;
            Mail.Credentials = credentials;
        }

        /// <summary>
        /// Finalizes the <see cref="WebServiceBase"/> class.
        /// </summary>
        ~WebServiceBase()
        {
            Dispose(false);
        }
    }
}
