using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using NHibernate.Expression;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Common.Interfaces
{
    #region Event Classes

    /// <summary>
    /// Holds event argument information for a <see cref="ClientSearchSelectedEventHandler"/>.
    /// </summary>
    public class ClientSearchSelectedEventArgs : EventArgs
    {
        private int _legalEntityKey;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="legalEntityKey"></param>
        public ClientSearchSelectedEventArgs(int legalEntityKey)
        {
            _legalEntityKey = legalEntityKey;
        }

        /// <summary>
        /// Gets the legal entity key for the raised event.
        /// </summary>
        public int LegalEntityKey 
        {
            get 
            {
                return _legalEntityKey;
            }
        }

    }

    /// <summary>
    /// Holds event argument information for a <see cref="ClientSearchClickedEventHandler"/>.
    /// </summary>
    public class ClientSearchClickedEventArgs : EventArgs
    {
        private IClientSearchCriteria _searchCriteria;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="searchCriteria"></param>
        public ClientSearchClickedEventArgs(IClientSearchCriteria searchCriteria)
        {
            _searchCriteria = searchCriteria;
        }

        /// <summary>
        /// Gets the search criteria for the raised event.
        /// </summary>
        public IClientSearchCriteria SearchCriteria
        {
            get
            {
                return _searchCriteria;
            }
        }

    }

    #endregion

    public interface IClientSearch : IViewBase
    {
        /// <summary>
        /// Raised when the search button is clicked, pass in the list of search criteria.
        /// </summary>
        event EventHandler<ClientSearchClickedEventArgs> SearchClientClicked;

        /// <summary>
        /// Raised when a search result is selected.
        /// </summary>
        event EventHandler<ClientSearchSelectedEventArgs> ClientSelectedClicked;

        /// <summary>
        /// Raised when the new client button is clicked.
        /// </summary>
        event EventHandler CreateNewClientClicked;

        /// <summary>
        /// Raised when the cancel button is clicked.
        /// </summary>
        event EventHandler CancelClicked;



        event EventHandler TMPClickHandler;

        bool SearchButtonVisible { set;}
        // bool ClientSelectButtonVisible { set;}
        bool CreateNewClientButtonVisible { set;}
        bool CancelButtonVisible { set;}

        void BindAccountTypes(IDictionary<int, string> AccountTypes);
        void BindSearchResults(IEventList<ILegalEntity> legalEntities, ClientSearchType searchType);
    }
}
