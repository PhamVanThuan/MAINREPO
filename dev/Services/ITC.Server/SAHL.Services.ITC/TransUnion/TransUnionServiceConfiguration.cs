using System.Configuration;

namespace SAHL.Services.ITC.TransUnion
{
    public class TransUnionServiceConfiguration : ITransUnionServiceConfiguration
    {
        public TransUnionServiceConfiguration()
        {
            this.ClientReference = ConfigurationManager.AppSettings["ClientReference"].ToString();
            this.SecurityCode = ConfigurationManager.AppSettings["SecurityCode"].ToString();
            this.SubscriberCode = ConfigurationManager.AppSettings["SubscriberCode"].ToString();
            this.ContactName = ConfigurationManager.AppSettings["ContactName"].ToString();
            this.ContactNumber = ConfigurationManager.AppSettings["ContactNumber"].ToString();
            this.Destination = ConfigurationManager.AppSettings["Destination"].ToString();
        }

        public string ClientReference
        {
            get;
            protected set;
        }

        public string SecurityCode
        {
            get;
            protected set;
        }

        public string SubscriberCode
        {
            get;
            protected set;
        }

        public string ContactName
        {
            get;
            protected set;
        }

        public string ContactNumber
        {
            get;
            protected set;
        }

        public string Destination
        {
            get;
            protected set;
        }
    }
}