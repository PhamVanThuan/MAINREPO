using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using SAHL.Common.DomainMessages;


namespace SAHL.Web.Views.Common.Presenters.Banking
{
    public class BankingDetailsUpdate : BankingDetailsBase
    {
        public BankingDetailsUpdate(IBankingDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.BankAccountGridEnabled = true;
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnCancelClicked += new EventHandler(_view_CancelClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_SubmitButtonClicked);
            _view.OnSearchBankAccountClicked += new EventHandler(_view_OnSearchBankAccountClicked);
            _view.OnBankDetailsGrid_SelectedIndexChanged += _view_OnBankDetailsGridSelectedIndexChanged;
            //ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            //_view.BindBankAccountControls(lookups.Banks, lookups.BankAccountTypes, lookups.GeneralStatuses);
            _view.SetControlsToFirstAccount = true;
            _view.IsUpdate = true;

            if (GlobalCacheData.ContainsKey("BANK"))
            {
                _view.BankValue = GlobalCacheData["BANK"] as string;
                _view.BranchCodeValue = GlobalCacheData["BRANCHCODE"] as string;
                _view.AccountTypeValue = GlobalCacheData["ACCOUNTTYPE"] as string;
                _view.AccountNumberValue = GlobalCacheData["ACCOUNTNUMBER"] as string;
                _view.AccountNameValue = GlobalCacheData["ACCOUNTNAME"] as string;
                _view.SetControlsToSearchValues = true;
                GlobalCacheData.Clear();
            }


            PrivateCacheData.Remove("BankDetails");
            PrivateCacheData.Add("BankDetails", 0);
        }

        protected virtual void _view_OnBankDetailsGridSelectedIndexChanged(object sender, EventArgs e)
        {

            PrivateCacheData.Remove("BankDetails");
            PrivateCacheData.Add("BankDetails", _view.BankDetailsGridIndex);
            BindDisplay();
        }

        private void BindDisplay()
        {
            int BankdetailsKey = Convert.ToInt32(PrivateCacheData["BankDetails"]);
            _view.BindFromGrid(BankdetailsKey);

        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.IsUpdate = true;
            _view.ControlsVisible = true;
            _view.SearchButtonVisible = true;
            _view.ShowButtons = true;
            _view.ShowStatus = true;
            if (base.BankAccounts.Count < 1)
            {
                _view.SubmitButtonEnabled = false;
            }
            _view.SubmitButtonText = "Update";

            if (_view.Messages.Count > 0)
            {
                int BankdetailsKey = Convert.ToInt32(PrivateCacheData["BankDetails"]);
                _view.BindFromGrid(BankdetailsKey);
            }
        }

        /// <summary>
        /// Hooks the View's Submit Button Clicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_SubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope ts = new TransactionScope();
            try
            {
                IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();
                ILegalEntityRepository LER = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                ILegalEntityBankAccount leba = null;

                /*
                if (_view.BranchKey.Length != 0 && _view.SelectedLegalEntityBankAccountKey != -1)
                {
                    leba = LER.GetLegalEntityBankAccountByKey(_view.SelectedLegalEntityBankAccountKey);
                    IACBBranch branch = BAR.GetACBBranchByKey(_view.BranchKey.ToString());
                    leba.BankAccount.ACBBranch = branch;
                }
                else
                {
                    _view.Messages.Add(new Error("A bank account and branch must be specified", "A bank account and branch must be specified"));
                    return;
                }
                if (_view.AccountTypeKey == -1)
                {
                    _view.Messages.Add(new Error("Account type must be specified", "Account type must be specified"));
                    return;
                }               

                IACBType type = BAR.GetACBTypeByKey(_view.AccountTypeKey);
                leba.BankAccount.ACBType = type;
                leba.BankAccount.AccountName = _view.AccountName;
                leba.BankAccount.AccountNumber = _view.AccountNumber;
                leba.ChangeDate = DateTime.Today;
                leba.UserID = spc.GetADUser(_view.CurrentPrincipal).ADUserName;
                BAR.SaveBankAccount(leba.BankAccount);
                */

                leba = LER.GetLegalEntityBankAccountByKey(_view.SelectedLegalEntityBankAccountKey);
                leba.BankAccount.AccountName = _view.AccountName;
                if (_view.AccountTypeKey == -1)
                {
                    _view.Messages.Add(new Error("Account type must be specified", "Account type must be specified"));
                    //_view.SetControlsToFirstAccount = false;
                    return;
                }
                IACBType type = BAR.GetACBTypeByKey(_view.AccountTypeKey);
                leba.BankAccount.ACBType = type;
                leba.ChangeDate = DateTime.Today;
                leba.UserID = _view.CurrentPrincipal.Identity.Name;

                ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
                leba.GeneralStatus = LR.GeneralStatuses[(GeneralStatuses)_view.GeneralStatusKey];

                BAR.SaveBankAccount(leba.BankAccount);
                LER.SaveLegalEntity(base.LegalEntity, false);

                ts.VoteCommit();
                _view.Navigator.Navigate("Submit");
            }

            catch (Exception)
            {
                ts.VoteRollBack();
                if (_view.IsValid)
                {
                    throw;
                }
            }
            finally
            {
                ts.Dispose();
            }
        }

        /// <summary>
        /// Hooks the View's SearchBankAccountClicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnSearchBankAccountClicked(object sender, EventArgs e)
        {
            base.OnSearchBankAccountClicked();

            IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();

            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
                GlobalCacheData[ViewConstants.NavigateTo] = "BankingDetailsUpdate";
            else
            {
                GlobalCacheData.Add(ViewConstants.NavigateTo, "BankingDetailsUpdate", LifeTimes);
            }
        }



        /// <summary>
        /// Hooks the View's CancelClicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_CancelClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }
    }
}
