using System;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Globals;
using System.Data;
using DevExpress.Web.ASPxTreeList;

namespace SAHL.Web.Views.Administration.Interfaces
{
	/// <summary>
    /// ExternalOrganisationStructure Interface
	/// </summary>
    public interface IExternalOrganisationStructure : IViewBase
	{
		#region View Properties

        /// <summary>
        /// 
        /// </summary>
        DataRow GetFocusedNode { get; }

        /// <summary>
        /// 
        /// </summary>
        string SelectedNodeKey { get; set;}

        /// <summary>
        /// 
        /// </summary>
        string SelectedNodeParentKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string TreeViewHeading { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool AddButtonVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool RemoveButtonVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool UpdateButtonVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool SelectButtonVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool ViewButtonVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool CancelButtonVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool AllowNodeDragging { get;  set; }

        /// <summary>
        /// 
        /// </summary>
        bool AllowAddToCBO { get;  set; }

		/// <summary>
		/// Allow the Search to be displayed
		/// </summary>
		bool AllowSearch { get; set; }

		/// <summary>
		/// Search Service URL
		/// </summary>
		string SearchServiceMethod { get; set; }

		#endregion

		#region EventHandlers

        /// <summary>
        /// Raised when the submit button is clicked.
        /// </summary>
        event EventHandler SubmitButtonClicked;

        /// <summary>
        /// Raised when the add button is clicked.
        /// </summary>
        event EventHandler AddButtonClicked;

        /// <summary>
        /// Raised when the cancel button is clicked.
        /// </summary>
        event EventHandler CancelButtonClicked;

        /// <summary>
        /// Raised when the update button is clicked.
        /// </summary>
        event EventHandler UpdateButtonClicked;

        /// <summary>
        /// Raised when the select button is clicked.
        /// </summary>
        event EventHandler SelectButtonClicked;

        /// <summary>
        /// Raised when the remove button is clicked.
        /// </summary>
        event EventHandler RemoveButtonClicked;

        /// <summary>
        /// Raised when the View button is clicked.
        /// </summary>
        event EventHandler ViewButtonClicked;

        /// <summary>
        /// Raised when the OrgStructure Node is dragged and dropped
        /// </summary>
        event TreeListNodeDragEventHandler TreeNodeDragged;

        /// <summary>
        /// Triggered when the selected Legal Entity is added to the CBO Menu.
        /// </summary>
        event EventHandler OnAddToCBO;

		#endregion

		#region  View Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgStructLst"></param>
        void BindOrganisationStructure(DataSet orgStructLst);


		#endregion

	}
}
