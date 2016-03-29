using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
    public interface ICourtDetails : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnCommentClick;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCommentEditorSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        bool ShowMaintenancePanel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string Img_src_path { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int SelectedHearingTypeKey { get; }

        /// <summary>
        /// 
        /// </summary>
        int SelectedHearingAppearanceTypeKey { get; }

        /// <summary>
        /// 
        /// </summary>
        int SelectedCourtKey { get; }

        /// <summary>
        /// 
        /// </summary>
        string CaseNumber { get; }

        /// <summary>
        /// 
        /// </summary>
        DateTime? HearingDate { get; }

        /// <summary>
        /// 
        /// </summary>
        string Comments { get; }
        
        /// <summary>
        /// 
        /// </summary>
        string CommentEditor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int HearingDetailKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hearingDetails"></param>
        void BindHearingDetails(IEventList<IHearingDetail> hearingDetails);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hearingTypes"></param>
        void BindHearingTypes(IDictionary<int, string> hearingTypes);
    }
}
