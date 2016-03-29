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
    public class DeclarationsConfirm : DeclarationsBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DeclarationsConfirm(IDeclarations view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            Activity = "DeclarationConfirmationDone";
            
            _view.ConfirmationMode = true;

        }
    }
}
