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
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// Stage Transition History Interface
    /// </summary>
    public interface IStageTransitionHistory : IViewBase
    {
        /// <summary>
        /// Event Handler for Cancel Button clicked
        /// </summary>
        event EventHandler onCancelButtonClicked;

        /// <summary>
        /// Show Cancel Button
        /// </summary>
        bool ShowCancelButton { set;}

        /// <summary>
        /// Show Workflow Header
        /// </summary>
        bool ShowLifeWorkFlowHeader { set;}

        /// <summary>
        /// Show the Generic Key Column.
        /// When an Account is linked to multple applications e.g. Further Lending
        /// </summary>
        bool GenericKeyColumnVisible { set;}

        #region Methods

        /// <summary>
        /// Binds History (Stage Transition)  History Grid
        /// </summary>
        /// <param name="lstStageTransitions">IList&lt;IStageTransition&gt;</param>
        // void BindHistoryGrid(IList<IStageTransition> lstStageTransitions);

        /// <summary>
        /// Binds History (Stage Transition)  History Grid
        /// </summary>
        /// <param name="dtStageTransitions"></param>
        void BindHistoryGrid(DataTable dtStageTransitions);

        /// <summary>
        /// 
        /// </summary>
        void SetUpGrid();

        #endregion


    }
}
