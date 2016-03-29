using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class LegalEntityNotificationViewAll : LegalEntityNotification
    {
        /// <summary>
        /// Constructor for LegalEntitySequestrationNotify
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
        public LegalEntityNotificationViewAll(ILegalEntityNotification view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
            _view.ReadOnlyAll = true;
        }
    }
}