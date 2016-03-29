
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using System;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections;
using System.Collections.Generic;
using SAHL.Common.DomainMessages;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.SearchCriteria;


namespace SAHL.Web.Views.Common.Presenters.SettlementBanking
{

    /// <summary>
    /// 
    /// </summary>
    public class SettlementBankingDetailsBase : SAHLCommonBasePresenter<IBankingDetails>
    {
        protected const string foundBankAccounts = "FOUNDBANKACCOUNTS";
        protected const string callingView = "CALLINGVIEW";
        protected const string nextView = "NEXTVIEW";

        protected const string BankCriteria = "BANKCRITERIA";

        private IApplication _application;
        private List<IBankAccount> _lstBankAccounts;
        private CBOMenuNode _node;

        /// <summary>
        /// 
        /// </summary>
        public IApplication Application
        {
            get
            {
                return _application;
            }
            set
            {
                _application = value;
            }
        }

        public List<IBankAccount> BankAccounts
        {
            get
            {
                return _lstBankAccounts;
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        public CBOMenuNode MenuNode
        {
            get
            {
                return _node;
            }
            set
            {
                _node = value;
            }
        }
      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public SettlementBankingDetailsBase(IBankingDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

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

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            _view.OnSearchBankAccountClicked += new EventHandler(_view_OnSearchBankAccountClicked);

            if(_view.IsMenuPostBack)
              PrivateCacheData.Clear();

            _view.HideGridStatusColumn = true;
            _view.AccountTypeBondOnly = true;
            IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
            _application = AR.GetApplicationByKey(_node.GenericKey);
            _lstBankAccounts = new List<IBankAccount>();
            List<IApplicationExpense> lstApplicationExpenses = new List<IApplicationExpense>();

            for (int x = 0; x < _application.ApplicationExpenses.Count; x++)
            {
                for (int y = 0; y < _application.ApplicationExpenses[x].ApplicationDebtSettlements.Count; y++)
                {
                    if (_application.ApplicationExpenses[x].ApplicationDebtSettlements[y].BankAccount != null)
                    {
                        _lstBankAccounts.Add(_application.ApplicationExpenses[x].ApplicationDebtSettlements[y].BankAccount);
                        lstApplicationExpenses.Add(_application.ApplicationExpenses[x]);
                    }
                }
            }
            _view.BindGridForBankAccounts(lstApplicationExpenses);
        }

        /// <summary>
        /// Hooks the View's SearchBankAccountClicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnSearchBankAccountClicked(object sender, EventArgs e)
        {
            if (_view.BranchCode.Length == 0 || _view.AccountNumber.Length == 0)
            {
                _view.Messages.Add(new Error("Bank, Branch, and Account Number must be specified for a search", "Bank, Branch, and Account Number must be specified for a search"));
                return;
            }


            string branchCode = _view.BranchCode;
          //  string accountName = _view.AccountName;
            //string accountType = _view.AccountType;
            string accountNumber = _view.AccountNumber;



            //TODO : Add Real Life Time Object
            IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();

           // GlobalCacheData.Clear();

            GlobalCacheData.Add("BANK", _view.BankKey.ToString(),LifeTimes);
            GlobalCacheData.Add("BRANCHCODE", _view.BranchCode, LifeTimes);
            GlobalCacheData.Add("ACCOUNTNAME", _view.AccountName, LifeTimes);
            GlobalCacheData.Add("ACCOUNTTYPE", _view.AccountType, LifeTimes);
            GlobalCacheData.Add("ACCOUNTNUMBER", _view.AccountNumber, LifeTimes);
            GlobalCacheData.Add("ACCOUNTREFERENCE", _view.GETSETtxtReference, LifeTimes);

            IBankAccountSearchCriteria criteria = new BankAccountSearchCriteria();
            if (!string.IsNullOrEmpty(branchCode))
            {
                int res = -1;
                if (branchCode.Contains("-"))
                {
                    if (int.TryParse(branchCode.Substring(0, branchCode.IndexOf('-') - 1), out res))
                    {
                        criteria.ACBBranchKey = branchCode.Substring(0, branchCode.IndexOf('-') - 1);
                    }
                }
                else
                {
                    criteria.ACBBranchKey = branchCode;
                }
            }
            //Commented out next line because we don't want to limit the type we search on
            // criteria.ACBTypeKey = int.Parse(accountType);
           // criteria.AccountName = accountName;
            criteria.AccountNumber = accountNumber;



            //GlobalCacheData.Clear();
            if (GlobalCacheData.ContainsKey(foundBankAccounts))
                GlobalCacheData[foundBankAccounts] = criteria;
            else
            {
                GlobalCacheData.Add(foundBankAccounts, criteria, LifeTimes);
            }


            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
                GlobalCacheData[ViewConstants.ApplicationKey] = _application.Key;
            else
            {
                GlobalCacheData.Add(ViewConstants.ApplicationKey, _application.Key, LifeTimes);
            }

            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
                GlobalCacheData[ViewConstants.NavigateTo] = "SettlementBankingDetailsAdd";
            else
            {
                GlobalCacheData.Add(ViewConstants.NavigateTo, "SettlementBankingDetailsAdd", LifeTimes);
            }

          
            //navigate to BankingDetailsSearch
            _view.Navigator.Navigate("BankingDetailsSearchSettlement");

        }
    }
}
