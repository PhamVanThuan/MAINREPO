using System;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProposalSummary : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnAddButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnUpdateButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnViewButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnPrintButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnCopyToDraftButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnCreateCounterProposalClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnSetActiveButtonClicked;


		/// <summary>
		/// 
		/// </summary>
		event KeyChangedEventHandler OnDeleteButtonClicked;

		event KeyChangedEventHandler OnViewAmortisationScheduleClicked;

        /// <summary>
        /// Reasons Click
        /// </summary>
        event KeyChangedEventHandler ReasonsClicked;

        /// <summary>
        /// 
        /// </summary>
        bool ShowCancelButton { set;}
        
        /// <summary>
        /// 
        /// </summary>
        bool ShowAddButton { set;}

        /// <summary>
        /// 
        /// </summary>
        bool ShowUpdateButton { set;}

        /// <summary>
        /// 
        /// </summary>
        bool ShowViewButton { set; }

        /// <summary>
        /// 
        /// </summary>
        bool ShowPrintButton { set; }

        /// <summary>
        /// 
        /// </summary>
        bool ShowCopyToDraftButton { set; }

        /// <summary>
        /// 
        /// </summary>
        bool ShowSetActiveButton { set; }

		/// <summary>
		/// 
		/// </summary>
		bool ShowDeleteButton { set; }

        /// <summary>
        /// Show Reasons Button
        /// </summary>
        bool ShowReasonsButton { set; }

        /// <summary>
        /// 
        /// </summary>
        bool ShowCreateCounterProposalButton { set; }

        /// <summary>
        /// Binds the Premium History Grid data
        /// </summary>
        /// <param name="lstDebtCounsellingProposalSummary"></param>
        void BindProposalSummaryGrid(List<IProposal> lstDebtCounsellingProposalSummary);
		void SendPDFToClient(string pdfFilePath);

        /// <summary>
        /// 
        /// </summary>
        ProposalTypeDisplays ShowProposals { get; set; }

        byte[] AmmorisationSchedulePDF { get; set; }
    }

    /// <summary>
    /// enum List of 
    /// The possible Proposals the view can display
    /// </summary>
    public enum ProposalTypeDisplays
    {
        All = 0,
        Proposal = 1,
        CounterProposal = 2
    }
}
