using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Web.UI.WebControls;

namespace SAHL.Web.Views.Origination.Interfaces
{
    public interface IApplicationAttributes : IViewBase
    {
        void PopulateAttributes(IEventList<IApplicationAttributeType> applicationAttributes);

        void PopulateMarketingSource(IEventList<IApplicationSource> applicationSource);

        void BindApplication(IApplicationMortgageLoan application);

        IApplicationMortgageLoan GetUpdatedApplicationMortgageLoan(IApplicationMortgageLoan appMortgageLoan);

        ListItemCollection GetAttributeOptions { get; }

        void showControlsForDisplay();

        void showControlsForUpdate();

        bool ShowButtons { set; }

        event EventHandler onCancelButtonClicked;

        event EventHandler onUpdateButtonClicked;

    }
}
