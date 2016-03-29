using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILegalEntityApplications : IViewBase
    {
        /// <summary>
        /// Called when the Select button is clicked
        /// </summary>
        event KeyChangedEventHandler OnSelectButtonClicked;

        /// <summary>
        /// Sets the header text on the grid
        /// </summary>
        string GridHeading { get; set;}

        /// <summary>
        /// Binds LegalEntity Applications Grid
        /// </summary>
        /// <param name="lstApplications">IEventList&lt;IApplication&gt;</param>
        void BindApplicationsGrid(IEventList<IApplication> lstApplications);
    }
}
