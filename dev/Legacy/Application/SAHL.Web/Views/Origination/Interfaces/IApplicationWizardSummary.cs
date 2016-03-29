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
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Origination.Interfaces
{
    public interface IApplicationWizardSummary : IViewBase
    {

        #region Events

        event EventHandler OnCalculateButtonClicked;
        event EventHandler OnFinishedButtonClicked;
        event EventHandler OnAddButtonClicked;
        event KeyChangedEventHandler OnUpdateButtonClicked;
        

        #endregion

        #region Properties

        #endregion

        #region Methods

        void BindLegalEntities(IList<ILegalEntity> lstLegalEntities, IApplication application);

        void BindLoanGrid(IApplication application);

        #endregion
    }
}
