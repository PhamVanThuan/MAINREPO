using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Administration.Interfaces
{
    public interface IMarketingSource : IViewBase
    {
        /// <summary>
        /// Sets the Text value of the txtMarketingSourceDescription control.
        /// </summary>
        string txtMarketingSourceDescriptionText { set; get;}

        string ddlMarketingSourceStatusSelected { set; get;}

        string SubmitButtonText { set; }

        /// <summary>
        /// Gets the market rate key selected on the grid.
        /// </summary>
        int SelectedMarketingSourceKey { get; }

        bool SubmitButtonVisible { set;}
        bool SubmitButtonEnabled { set;}
        bool CancelButtonVisible { set;}
        bool UpdatePanelVisible { set;}
        /// <summary>
        /// 
        /// </summary>
        event EventHandler MarketingSourceSelected;
        event EventHandler CancelClick;
        event EventHandler SubmitClick;

        void BindMarketingSourcesGrid(IReadOnlyEventList<IApplicationSource> marketingSources);

        void BindStatusDropDown(ICollection<IGeneralStatus> generalStatus);
    }
}
