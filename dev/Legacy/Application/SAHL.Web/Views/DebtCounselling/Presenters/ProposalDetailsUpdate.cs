using System;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.DomainMessages;
using System.Data;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    /// <summary>
    /// ProposalDetails Add
    /// </summary>
    public class ProposalDetailsUpdate : ProposalDetailsBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ProposalDetailsUpdate(IProposalDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) 
                return;
            

            _view.OnAddButtonClicked += new EventHandler(_view_OnAddButtonClicked);
            _view.OnRemoveButtonClicked += new KeyChangedEventHandler(_view_OnRemoveButtonClicked);
            _view.OnSaveButtonClicked += new EventHandler(_view_OnSaveButtonClicked);
            _view.OnCompleteButtonClicked += new EventHandler(_view_OnCompleteButtonClicked);

            if (this.GlobalCacheData.ContainsKey(ViewConstants.ProposalKey))
                this.SelectedProposal = _debtCounsellingRepository.GetProposalByKey(Convert.ToInt32(this.GlobalCacheData[ViewConstants.ProposalKey]));
            else
                this.SelectedProposal = _debtCounsellingRepository.CreateEmptyProposal();        

        }


        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) 
                return;

            // setup visible buttons
            _view.ShowAddButton = true;
            _view.ShowRemoveButton = true;
            _view.ShowCancelButton = true;
            _view.ShowSaveButton = true;
            _view.ShowCompleteButton = true;
        }

        void _view_OnAddButtonClicked(object sender, EventArgs e)
        {
            // perform screen validation
            if (ValidateInput() == false)
                return;
        }

        IDictionary<int, string> GetMarketRatesToBind()
        {
            IDictionary<int, string> marketRates = new Dictionary<int, string>();

            // add our own value into the dictionary
            marketRates.Add(99, "Fixed");
            foreach (IMarketRate mr in _lookups.MarketRates)
            {
                if (mr.Key == (int)MarketRates.JIBAR91DayDiscount21 || mr.Key == (int)MarketRates.RepoRate || mr.Key == (int)MarketRates.PrimeLendingRate)
                    marketRates.Add(mr.Key, mr.Description);
            }

            return marketRates;
        }

        void _view_OnRemoveButtonClicked(object sender, EventArgs e)
        {
        }

        void _view_OnSaveButtonClicked(object sender, EventArgs e)
        {
        }

        void _view_OnCompleteButtonClicked(object sender, EventArgs e)
        {
        }

        protected bool ValidateInput()
        {
            bool valid = true;
            string errorMessage = "";

            // if we have an addressparameter then enforce address selection
            //if (!String.IsNullOrEmpty(_reportDataList[0].AddressParameterName))
            //{
            //    if (_view.SelectedAddressKey <= 0)
            //    {
            //        errorMessage = "Must select a Recipient Address. If none exist, please add before continuing.";
            //        _view.Messages.Add(new Error(errorMessage, errorMessage));
            //        valid = false;
            //    }
            //}

            return valid;
        }
        #endregion
    }
}
