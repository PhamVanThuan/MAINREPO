using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Web.Views.Administration.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// Workflow Super Search Interface.
    /// </summary>
    public interface IWorkflowSuperSearch : IViewBase
    {
        #region Event Handlers

        /// <summary>
        /// Event declaration for the "Search In" drop down list selected index change event.
        /// </summary>
        event KeyChangedEventHandler SearchInSelectedIndexChanged;

        /// <summary>
        /// Event declaration for the "Saved Search" drop down list selected index changed event.
        /// </summary>
        event KeyChangedEventHandler SavedSearchSelectedIndexChanged;

        /// <summary>
        /// Event declaration for the "Save Seach" button click event.
        /// </summary>
        event EventHandler SaveSearchButtonClicked;

        /// <summary>
        /// Event declaration for the "Manage Search" button click event.
        /// </summary>
        event EventHandler ManageSearchButtonClicked;

        /// <summary>
        /// Event declaration for the "Search" button click.
        /// </summary>
        event EventHandler SearchButtonClicked;

        event KeyChangedEventHandler SearchResultsDoubleClick;

        event EventHandler AddUserClicked;

        event KeyChangedEventHandler UserSelected;

        #endregion

        #region Properties

        /// <summary>
        /// Determines whether the "Include open application I created checkbox is visible.
        /// </summary>
        bool MyCreatedCasesFilterVisible { set;}

        /// <summary>
        /// Gets/sets whether the OrganisationalStructure filter is visible.
        /// </summary>
        bool OrganisationalStructureFilterVisible { get; set;}

        /// <summary>
        /// Gets a list of organisation structure keys selected.  This should be used in 
        /// conjunction with <see cref="OrganisationStructureUserFilters"/>.
        /// </summary>
        IList<int> OrganisationStructureFilters { get; }

        /// <summary>
        /// Gets a list of ADUser keys selected from the org structure.  This should be used in 
        /// conjunction with <see cref="OrganisationStructureFilters"/>.
        /// </summary>
        IList<int> OrganisationStructureUserFilters { get; }

        /// <summary>
        /// Determines whether controls that allow saving and management of saved search criteria are visible.
        /// </summary>
        bool SavedSearchPanelVisible { set;}

        /// <summary>
        /// The index of the currently selected search type.
        /// </summary>
        int SelectedSearchType { get;set;}

        /// <summary>
        /// The index of the currently selected saved search.
        /// </summary>
        int SelectedSavedSearchIndex { get;}

		bool SetSelectedSearchTypeEnabled { set;}

		bool SetIncludeHistoricUsersEnabled { set;}

        //int SelectedWorkflowIndex { get;set;}

        //int SelectedWorkflowStateIndex { get;set;}

        //int SelectedWorkflowStateFilterIndex { get;set;}

        int? SelectedApplicationTypeKey { get;}

        string ApplicationNo { get;set;}

        string IDNumber { get; set;}

        string Firstname { get; set;}

        string Surname { get; set;}

		string ccdCategory { get; set;}

        bool IncludeHistoricUsers { get; set;}
       
        bool IncludeMyCreatedCases { get;set;}

        /// <summary>
        /// Gets the maximum number of rows to return.
        /// </summary>
        int MaximumRowCount { get; }

        /// <summary>
        /// Gets/sets whether the search results tab is displayed.  This is automatically set to true when 
        /// the Search button is clicked.
        /// </summary>
        bool ShowSearchResults { get; set; }

        /// <summary>
        /// Gets a list of work flow filters selected on the view.  This is a dictionary containing 
        /// a list of items where the key is the IWorkflow ID, and the value a list of state keys selected 
        /// for that workflow.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        IDictionary<int, IList<int>> WorkflowFilters { get; }

        #endregion


        #region Methods

        void BindApplicationTypes(IEventList<IApplicationType> ApplicationTypes);
        void BindWorkflows(IEventList<IWorkFlow> Workflows);

        /// <summary>
        /// Binds a list of instances to the search results.
        /// </summary>
        /// <param name="instances"></param>
        void BindSearchResult(IList<IInstance> instances);
        
        void BindUserFilter(List<IADUser> ADUsers);
        /// <summary>
        /// Binds a list of ORganisationSTructures to a tree view. The TopLevel key is the the parentkey value that the
        /// view will accept as top level tree nodes.
        /// </summary>
        /// <param name="OrganisationStructure"></param>
        void BindOrganisationStructure(List<IBindableTreeItem> OrganisationStructure);

        #endregion
    }
}
