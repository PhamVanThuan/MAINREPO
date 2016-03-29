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
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
namespace SAHL.Web.Views.Cap.Interfaces
{
    public interface ICapCreateSearch : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSearchButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnGridSelectDoubleClick;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SearchResults"></param>
        void BindSearchResultsGrid(DataTable SearchResults);
        /// <summary>
        /// 
        /// </summary>
        void BindAccountTypeDropdown();
        /// <summary>
        /// 
        /// </summary>
        void SearchViewCustomSetUp();
        /// <summary>
        /// 
        /// </summary>
        int AccountNumber { get;}
        /// <summary>
        /// 
        /// </summary>
        int AccountTypeDropDown { set;}
        /// <summary>
        /// 
        /// </summary>
        bool AccountTypeEnabled { set;}
        /// <summary>
        /// 
        /// </summary>
        bool SubmitButtonEnabled { set;}
    }
}
