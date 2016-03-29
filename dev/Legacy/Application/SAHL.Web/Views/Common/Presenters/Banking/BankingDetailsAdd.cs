using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Presenters.Banking
{
    /// <summary>
    /// 
    /// </summary>
    public class BankingDetailsAdd : BankingDetailsBase
    {
 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public BankingDetailsAdd(IBankingDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (_view.IsMenuPostBack)
            {
                GlobalCacheData.Clear();
            }
          
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnCancelClicked += new EventHandler(_view_CancelClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_SubmitButtonClicked);
            _view.OnSearchBankAccountClicked += new EventHandler(_view_OnSearchBankAccountClicked);
            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            _view.BindBankAccountControls(lookups.Banks, lookups.BankAccountTypes, lookups.GeneralStatuses.Values);
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
           
            
        }

        void _view_OnSearchBankAccountClicked(object sender, EventArgs e)
        {
            base.OnSearchBankAccountClicked();

            IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();

            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
                GlobalCacheData[ViewConstants.NavigateTo] = "BankingDetailsAdd";
            else
            {
                GlobalCacheData.Add(ViewConstants.NavigateTo, "BankingDetailsAdd", LifeTimes);
            }
        }


        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.ControlsVisible = true;
            _view.ShowButtons = true;
            _view.ShowStatus = false;
            _view.SubmitButtonText = "Add";
            _view.SearchButtonVisible = true;
            _view.BankAccountGridEnabled = false;
        }
       

        /// <summary>
        /// Hooks the View's SubmitBankAccountClicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_SubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope ts = new TransactionScope();
            try
            {
                IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();
                ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
                ILegalEntityRepository LER= RepositoryFactory.GetRepository<ILegalEntityRepository>();
                IBankAccount ba = BAR.GetEmptyBankAccount();
                ILegalEntityBankAccount leba = LER.GetEmptyLegalEntityBankAccount();
                if (_view.BranchKey.Length != 0)
                {
                    IACBBranch branch = BAR.GetACBBranchByKey(_view.BranchKey.ToString());
                    ba.ACBBranch = branch;
                }
                else
                {
                    _view.Messages.Add(new Error("A bank and branch must be specified", "A bank and branch must be specified"));
                    _view.SetControlsToFirstAccount = false;
                    return;
                }
                if (_view.AccountTypeKey == -1)
                {
                    _view.Messages.Add(new Error("Account type must be specified", "Account type must be specified"));
                    _view.SetControlsToFirstAccount = false;
                    return;
                }

                IACBType type = BAR.GetACBTypeByKey(_view.AccountTypeKey);               
                
                ba.ACBType = type;
                ba.AccountName = _view.AccountName;
                ba.AccountNumber = _view.AccountNumber;

                leba.GeneralStatus = LR.GeneralStatuses[GeneralStatuses.Active];

                ba.ChangeDate = DateTime.Today;
                ba.UserID = _view.CurrentPrincipal.Identity.Name;

                BAR.SaveBankAccount(ba);
                leba.BankAccount = ba; 
                leba.LegalEntity = base.LegalEntity;
                base.LegalEntity.LegalEntityBankAccounts.Add(_view.Messages,leba);
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
