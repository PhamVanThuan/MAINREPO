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

namespace SAHL.Web.Views.Cap.Interfaces
{
    public interface ICAPReAllocateOffer : IViewBase
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        int AllocatedToListSelectedValue { get;}
        /// <summary>
        /// 
        /// </summary>
        int ReAllocateToListSelectedValue { get;}
        /// <summary>
        /// 
        /// </summary>
        IList<int> SelectedOffers { get;}

        #endregion

        #region Event Handlers

        event EventHandler OnUpdateButtonClicked;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capOfferList"></param>
        void BindGrid(DataTable capOfferList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userlist"></param>
        void BindBrokerLists(IList<IBroker> userlist);




    }
}
