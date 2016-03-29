using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;

namespace SAHL.Web.Views.Life.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILeadCreate : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSearchButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCreateButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        string SearchAccountKey { get; set;}
        /// <summary>
        /// 
        /// </summary>
        string SearchFirstNames { get; set;}
        /// <summary>
        /// 
        /// </summary>
        string SearchSurname { get; set;}
        /// <summary>
        /// 
        /// </summary>
        bool SearchExcludeClosedLoans { get; set;}

        /// <summary>
        /// The adusername of the selected consultant
        /// </summary>
        string SelectedConsultant { get; set;}

        /// <summary>
        /// The list of select loans for which to create the applications
        /// </summary>
        IList<int> SelectedLoanAccountKeys { get; set;}

        /// <summary>
        /// The max number of records the search must return 
        /// </summary>
        int MaxSearchResults { get;}

        /// <summary>
        /// Sets whether or not to show the consultant and create lead button
        /// </summary>
        bool AllowLeadCreate { get; set;}

        /// <summary>
        /// Holds the search results
        /// </summary>
        IEventList<SAHL.Common.BusinessModel.Interfaces.IAccount> SearchResults { get; set;}

        /// <summary>
        /// Sets whether the user is an "Admin" type user 
        /// </summary>
        bool AdminUser { get; set;}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstConsultants"></param>
        void BindConsultants(IList<IADUser> lstConsultants);
        /// <summary>
        /// 
        /// </summary>
        void BindSearchResults();
    }
}
