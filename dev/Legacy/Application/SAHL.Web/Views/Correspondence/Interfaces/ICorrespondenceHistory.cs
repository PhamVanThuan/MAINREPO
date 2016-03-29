using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Correspondence.Interfaces
{
    public class BindableCorrespondence
    {
        public BindableCorrespondence(ICorrespondence correspondence)
        {
            Key = correspondence.Key;
            DateActioned = correspondence.DueDate.Value.ToString(SAHL.Common.Constants.DateFormat + " HH:mm");
            DateSent = correspondence.CompletedDate.HasValue ? correspondence.CompletedDate.Value.ToString(SAHL.Common.Constants.DateFormat + " HH:mm") : "";
            Document = correspondence.ReportStatement.ReportName;
            Recipient = correspondence.LegalEntity == null ? "" : correspondence.LegalEntity.GetLegalName(LegalNameFormat.InitialsOnly);
            SentTo = correspondence.CorrespondenceMedium.Description + (correspondence.DestinationValue != null ? " : " + correspondence.DestinationValue : " ");
            User = correspondence.UserID;
            HasDetail = correspondence.CorrespondenceDetail == null ? false : true;
        }

        public int Key { get; set; }
        public string DateActioned { get; set; }
        public string DateSent { get; set; }
        public string Document { get; set; }
        public string Recipient { get; set; }
        public string SentTo { get; set; }
        public string User { get; set; }
        public bool HasDetail { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public interface ICorrespondenceHistory : IViewBase
    {
        /// <summary>
        /// Sets whether to show/hide the life workflow header
        /// </summary>
        bool ShowLifeWorkFlowHeader { set;}

        /// <summary>
        /// 
        /// </summary>
        bool ShowCancelButton { set;}

        /// <summary>
        /// 
        /// </summary>
        string CorrespondenceDetailHTML { set; get; }

        /// <summary>
        /// 
        /// </summary>
        event EventHandler onCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler onCallback;

        /// <summary>
        /// Binds Account Correspondence History Grid
        /// </summary>
        /// <param name="lstCorrespondence">IEventList&lt;ICorrespondence&gt;</param>
        void BindHistoryGrid(IEventList<ICorrespondence> lstCorrespondence);
    }
}
