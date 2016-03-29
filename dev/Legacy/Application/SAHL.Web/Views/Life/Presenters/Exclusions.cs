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
    public class Exclusions : ExclusionsBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public Exclusions(IExclusions view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            Activity = "ExclusionsDone";
            
            _view.ConfirmationMode = false;
        }
    }
}
