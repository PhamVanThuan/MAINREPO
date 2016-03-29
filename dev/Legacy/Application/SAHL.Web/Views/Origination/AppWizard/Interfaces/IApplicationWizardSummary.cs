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

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCalculateButtonClicked;
        
        
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnFinishedButtonClicked;
        
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnAddButtonClicked;
        
        
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnUpdateButtonClicked;
        

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstLegalEntities"></param>
        /// <param name="application"></param>
        void BindLegalEntities(IList<ILegalEntity> lstLegalEntities, IApplication application);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        void BindLoanGrid(IApplication application);

        #endregion
    }
}
