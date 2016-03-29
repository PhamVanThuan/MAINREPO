using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.Web.UI
{
    /// <summary>
    /// An interface that defines properties that are required by UIP views that
    /// are driven by 2AM accounts. UIP Views that require this information would 
    /// implement the interface.
    /// This interface allows the LighthouseBaseView to determine if a view requires 
    /// this information and automatically populate the values as the view loads. 
    /// The LighthousebaseView will also inject a ClientHeaderControl to display the 
    /// account information for any UIP view that implements this interface.
    /// </summary>
    public interface ISupportsClientInfo
    {
        /// <summary>
        /// The account key for an account in the 2AM database.
        /// </summary>
        int AccountKey { get;set;}

        /// <summary>
        /// The names of the legalentities with the role "Main Applicant" in the account
        /// defined by the account key.
        /// </summary>
        string ClientName { get;set;}
    }

    public enum ViewOperationMode
    {
        Standard,
        eWork
    }

    [Serializable]
    public class SAHLControllerState
    {
        private Dictionary<string, object> m_StateItems;

        public SAHLControllerState()
        {
            m_StateItems = new Dictionary<string,object>();
        }

        public Dictionary<string, object> StateItems
        {
            get
            {
                return m_StateItems;
            }
        }
    }

    public static class Constants
    {
        public const string CONTROLLERSTATE = "SAHLCONTROLLERSTATE";
        public const string CBOSTATE = "CBOSTATE";
    }
}
