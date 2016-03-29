using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{

    /// <summary>
    /// Stores event argument information for the <see cref="ClientSuperSearchClickedEventArgs"/> event 
    /// handler.
    /// </summary>
    public class ClientSuperSearchClickedEventArgs : EventArgs 
    {

        private IClientSuperSearchCriteria _searchCriteria;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="searchCriteria"></param>
        public ClientSuperSearchClickedEventArgs(IClientSuperSearchCriteria searchCriteria)
        {
            _searchCriteria = searchCriteria;
        }

        /// <summary>
        /// Gets the search criteria associated with the event.
        /// </summary>
        public IClientSuperSearchCriteria SearchCriteria
        {
            get
            {
                return _searchCriteria;
            }
        }

    }

    /// <summary>
    /// Holds event argument information for a <see cref="ClientSuperSearchSelectedEventArgs"/>.
    /// </summary>
    public class ClientSuperSearchSelectedEventArgs : EventArgs
    {
        private int _legalEntityKey;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="legalEntityKey"></param>
        public ClientSuperSearchSelectedEventArgs(int legalEntityKey)
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

    public interface IClientSuperSearch : IViewBase
    {
        /// <summary>
        /// Raised when the search button is clicked.
        /// </summary>
        event EventHandler<EventArgs> SearchClientClicked;

        /// <summary>
        /// Raised when an application is clicked within client details.
        /// </summary>
        event KeyChangedEventHandler ApplicationClicked;

        /// <summary>
        /// Raised when a search result is selected.
        /// </summary>
        event EventHandler<ClientSuperSearchSelectedEventArgs> ClientSelectedClicked;

        /// <summary>
        /// Raised when the new client button is clicked.
        /// </summary>
        event EventHandler CreateNewClientClicked;

        /// <summary>
        /// Raised when the cancel button is clicked.
        /// </summary>
        event EventHandler CancelClicked;

        bool SearchButtonVisible { set;}
        bool CreateNewClientButtonVisible { set;}
        bool CancelButtonVisible { set;}
        bool AccountTypesVisible { set;}
        string CreateNewClientButtonText { set;}

        void BindAccountTypes(IDictionary<int, string> AccountTypes);
        void BindSearchResults(IEventList<ILegalEntity> legalEntities);

        /// <summary>
        /// Gets the search criteria captured.  If the search type is advanced or basic this will be populated, 
        /// for simple name searches this will return null.
        /// </summary>
        /// <seealso cref="ClientSearchCriteria"/>
        IClientSuperSearchCriteria ClientSuperSearchCriteria { get; }

        /// <summary>
        /// Gets the search criteria captured.  If the search type is by name, this will be populated, for 
        /// basic and advanced searches this will return null.
        /// </summary>
        /// <seealso cref="ClientSuperSearchCriteria"/>
        IClientSearchCriteria ClientSearchCriteria { get; }
    }
}
