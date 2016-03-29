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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IRelatedLegalEntity : IViewBase
    {
        /// <summary>
        /// Called when the grid is double-clicked
        /// </summary>
        event KeyChangedEventHandler OnSelectLegalEntity;
        event KeyChangedEventHandler OnRemoveButtonClicked;
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// Bind a list of related LegalEntities
        /// </summary>
        /// <param name="LegalEntityRoles"></param>
        void BindLegalEntityGrid(IEventList<IRole> LegalEntityRoles);
         
        /// <summary>
        /// Set whether or not the Add To Menu Button is Enabled
        /// </summary>
        bool AddToMenuButtonEnabled { set; }

        /// <summary>
        /// Set whether or not the Remove Button is Enabled
        /// </summary>
        bool RemoveButtonEnabled { set; }

        /// <summary>
        /// Set whether or not the Cancel Button is Enabled
        /// </summary>
        bool CancelButtonEnabled { set; }

        /// <summary>
        /// Set whether or not to select on the grid
        /// </summary>
        bool AllowGridSelect { set; }

        /// <summary>
        /// Set whether or not to allow double click select on the grid
        /// </summary>
        bool AllowGridDoubleClick { set; }

    }
}
