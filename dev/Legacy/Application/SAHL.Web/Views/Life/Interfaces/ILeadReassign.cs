using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Life.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILeadReassign : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSearchButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnReassignButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnConfirmationButtonClicked;

        /// <summary>
        /// the text to display as the page heading
        /// </summary>
        string PageHeading { get; set; }

        /// <summary>
        /// the text to display for the account number search field and grid column heading
        /// </summary>
        string AccountHeading { get; set; }

        /// <summary>
        /// spcifies whether or not to show the parent account grid column
        /// </summary>
        bool DisplayParentAccount { get; set; }

        /// <summary>
        /// the text to display for the parent account number grid column heading
        /// </summary>
        string ParentAccountHeading { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string SearchAccountKey { get; set;}
        /// <summary>
        /// 
        /// </summary>
        OfferStatuses SearchApplicationStatus { get; set;}
        /// <summary>
        /// 
        /// </summary>
        string SearchConsultant { get; set;}
        /// <summary>
        /// 
        /// </summary>
        string SearchClientName { get; set;}

        /// <summary>
        /// The adusername of the selected consultant to reassign to
        /// </summary>
        string SelectedReassignADUserName { get; set;}

        /// <summary>
        /// The list of selected applicationskeys to reassign
        /// </summary>
        IList<int> SelectedApplicationKeys { get; set;}

        /// <summary>
        /// The max number of records the search must return 
        /// </summary>
        int MaxSearchResults { get;}

        /// <summary>
        /// Sets whether or not to show the consultant and create reassign button
        /// </summary>
        bool AllowLeadReassign { get; set;}

        /// <summary>
        /// Holds the search results
        /// </summary>
        IEventList<SAHL.Common.BusinessModel.Interfaces.IApplication> SearchResults { get; set;}

        /// <summary>
        /// Sets whether the user is an "Admin" type user 
        /// </summary>
        bool AdminUser { get; set;}

        /// <summary>
        /// spcifies whether or not to show the confirmation panel
        /// </summary>
        bool DisplayConfirmationPanel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstApplicationStatuses"></param>
        void BindApplicationStatuses(IEventList<IApplicationStatus> lstApplicationStatuses);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstConsultantsSearch">list of consultants to search for</param>
        /// <param name="lstConsultantsReassign">list of consultants to reassign to</param>
        void BindConsultants(IList<IADUser> lstConsultantsSearch, IList<IADUser> lstConsultantsReassign);
        /// <summary>
        /// 
        /// </summary>
        void BindSearchResults();
    }
}
