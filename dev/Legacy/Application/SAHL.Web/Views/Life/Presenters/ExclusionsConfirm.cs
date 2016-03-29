using System;
using System.Data;
using System.Configuration;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class ExclusionsConfirm : ExclusionsBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ExclusionsConfirm(IExclusions view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            Activity = "ExclusionsConfirmationDone";
            
            _view.ConfirmationMode = true;
        }
    }
}
