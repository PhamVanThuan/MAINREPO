using System;

namespace SAHL.Internet.Components
{
    /// <summary>
    /// This is the base class to be used by DotNetNuke
    /// the purpose of this class is to create an object that can store and retrieve
    /// all required variables in the server session cache
    /// </summary>
    [Serializable]
    public class SAHLWebSession
    {
        // Variables Retrieved as soon as the user hits the site.
        // Populated by SessionVariables.ascx
        
        // TODO: change to be collection for performance reasons
        /// <summary>
        /// Gets or sets accepted types.
        /// </summary>
        public string[] AcceptTypes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the request query string.
        /// </summary>
        public string QueryString
        {
            get;
            set;
        }

        // TODO: change to be collection for performance reasons
        /// <summary>
        /// Gets or sets query string keys.
        /// </summary>
        public string[] QueryStringAllKeys
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the referring URL.
        /// </summary>
        public string URLReferrer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        public string UserAgent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the host address.
        /// </summary>
        public string HostAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user hostname.
        /// </summary>
        public string UserHostName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets supported user languages.
        /// </summary>
        public string[] UserLanguages
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        public string CurrentPage
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether an application has been submitted.
        /// </summary>
        public bool ApplicationSubmitted
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the application key.
        /// </summary>
        public int? ApplicationKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the last used calculator.
        /// </summary>
        public int LastUsedCalculator
        {
            get;
            set;
        }
    }
}