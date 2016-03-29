using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
    public interface IProposalDetails : IViewBase
    {
        #region Properties

        Int32 SelectedItemKey { get; }

        bool ShowCancelButton { set; }

        bool ShowAddButton { set; }

        bool ShowRemoveButton { set; }

        bool ShowSaveButton { set; }

        bool ReadOnlyMode { set; }

        string SelectedMarketRate { get; }

        string CounterProposalNote { get; set; }

        DateTime? GetReviewDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        ProposalTypeDisplays ShowProposals { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="proposal"></param>
        void SetProposalRemainingTerm(IProposal proposal);

        #endregion Properties

        #region Events

        event EventHandler OnAddButtonClicked;

        event KeyChangedEventHandler OnRemoveButtonClicked;

        event EventHandler OnCancelButtonClicked;

        event EventHandler OnSaveButtonClicked;

        event EventHandler OnLifeInclSelectedIndexChanged;

        event EventHandler OnHOCInclSelectedIndexChanged;

        void BindMarketRates(IDictionary<int, string> marketRates);

        void BindHOCAndLife(IDictionary<int, string> inclusiveExclusive);

        void BindProposalHeader(IProposal proposal);

        void ProposalGridUnselectAll();

        void BindProposalItemsGrid(IEventList<IProposalItem> proposalItems);

        void PopulateProposalFromScreen(IProposal proposal);

        void PopulateProposalItemFromScreen(IProposalItem proposalItem, bool update);

        void ResetInputFields();

        void RenderProposalGraph(int proposalKey);

        void RenderAmortisationSchedule(int proposalKey);

        void RenderCounterProposalGraph(int proposalKey);

        void BindAccountSummary(IAccount acc, double hocMonthlyPremium, double lifePolicyMonthlyPremium, double lifeBalance);

        #endregion Events
    }
}