using System;
using System.Configuration;
using System.Security.Principal;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Security;

namespace SAHL.Web.Services
{
    /// <summary>
    /// This is the base class to be used by DotNetNuke
    /// the purpose of this class is to create an object that can store and retrieve
    /// all required variables in the server session cache
    /// </summary>
    [Serializable]
    public class SAHLSession
    {
                /// <summary>
        ///  This is the Session's Generic Prinicial
        /// </summary>
        public GenericPrincipal genericPrincipal
        {
            get { return new GenericPrincipal(new GenericIdentity("SAHLUser"), null); }
        }

        private GenericIdentity identity;

        /// <summary>
        /// 
        /// </summary>
        public SAHLPrincipal SAHLprincipal
        {
            get
            {
                SAHLPrincipal principal =  new SAHLPrincipal(new GenericIdentity(SAHLUser));
                //principal.ADUser = ADUser;
                return principal;
            }
        }

        /// <summary>
        /// Gets the SAHL User specified in the Web.config 
        /// </summary>
        public string SAHLUser
        {
            get { return ConfigurationManager.AppSettings["DotNetNukeUser"]; }
        }

        protected IOriginationSource OriginationSource;

        /// <summary>
        /// Returns the ADUser object for the SAHLUser
        /// </summary>
        public IADUser ADUser
        {
            get
            {
                IOrganisationStructureRepository OSR = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                return OSR.GetAdUserForAdUserName(SAHLUser);
            }
        }

        
        private IDomainMessageCollection messages = new DomainMessageCollection();
        /// <summary>
        /// Get or Set the Domiain Message collection
        /// </summary>
        public IDomainMessageCollection Messages
        {
            set { messages = value; }
            get { return messages; }

        }


        private SAHLPrincipalCache principalcache;
        /// <summary>
        /// 
        /// </summary>
        public SAHLPrincipalCache PrincipalCache
        {
            get
            {
                principalcache = SAHLPrincipalCache.GetPrincipalCache(SAHLprincipal);
                return principalcache;
            }

        }




        // Variables Retrieved as soon as the user hits the site.
        // Populated by SessionVariables.ascx
        private string[] accepttypes;
        // TODO: change to be collection for performance reasons
        /// <summary>
        /// 
        /// </summary>
        public string[] AcceptTypes
        {
            get { return accepttypes; }
            set { accepttypes = value; }
        }

        private string querystring;
        /// <summary>
        /// 
        /// </summary>
        public string QueryString
        {
            get { return querystring; }
            set { querystring = value; }
        }

        private string[] queryallstringkeys;
        // TODO: change to be collection for performance reasons
        /// <summary>
        /// 
        /// </summary>
        public string[] QueryStringAllKeys
        {
            get { return queryallstringkeys; }
            set { queryallstringkeys = value; }
        }

        private string urlreferrer;
        /// <summary>
        /// 
        /// </summary>
        public string URLReferrer
        {
            get { return urlreferrer; }
            set { urlreferrer = value; }
        }

        private string useragent;
        /// <summary>
        /// 
        /// </summary>
        public string UserAgent
        {
            get { return useragent; }
            set { useragent = value; }
        }

        private string hostaddress;
        /// <summary>
        /// 
        /// </summary>
        public string HostAddress
        {
            get { return hostaddress; }
            set { hostaddress = value; }
        }

        private string userhostname;
        /// <summary>
        /// 
        /// </summary>
        public string UserHostName
        {
            get { return userhostname; }
            set { userhostname = value; }
        }

        private string[] userlanguages;
        /// <summary>
        /// 
        /// </summary>
        public string[] UserLanguages
        {
            get { return userlanguages; }
            set { userlanguages = value; }
        }

        private string currentpage;

        /// <summary>
        /// 
        /// </summary>
        public SAHLSession()
        {
            identity = new GenericIdentity(SAHLUser);
        }

        /// <summary>
        /// 
        /// </summary>
        public string CurrentPage
        {
            get { return currentpage; }
            set { currentpage = value; }
        }


    }


}


