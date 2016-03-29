using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.DomainMessages;

using SAHL.Common.UI;

namespace SAHL.Web.Views.Common.Presenters.Banking
{
    public class BankingDetailsBase : SAHLCommonBasePresenter<IBankingDetails>
    {
        protected const string foundBankAccounts = "FOUNDBANKACCOUNTS";
        protected const string callingView = "CALLINGVIEW";
        protected const string nextView = "NEXTVIEW";

        protected const string BankCriteria = "BANKCRITERIA";

        protected CBOMenuNode _node;

        protected ILegalEntity _legalEntity;

        private List<ILegalEntityBankAccount> _lstBankAccounts;


        public List<ILegalEntityBankAccount> BankAccounts
        {
            get
            {
                return _lstBankAccounts;
            }          
        }

        public ILegalEntity LegalEntity
        {
            get
            {
                return _legalEntity;
            }
            set
            {
                _legalEntity = value;
            }
        }

        public BankingDetailsBase(IBankingDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;                     

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            
            if (!_view.ShouldRunPage) return;

            if (_view.IsMenuPostBack)
            {
                PrivateCacheData.Clear();
            }

           // _view.OnSearchBankAccountClicked += new EventHandler(_view_OnSearchBankAccountClicked);
            ILegalEntityRepository LER = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            _legalEntity = LER.GetLegalEntityByKey(_node.GenericKey);
            ////_legalEntity = LER.GetLegalEntityByKey(55840);
            IEventList<ILegalEntityBankAccount> lstLEBanksAccounts = _legalEntity.LegalEntityBankAccounts;
            _lstBankAccounts = new List<ILegalEntityBankAccount>();
            for (int x = 0; x < lstLEBanksAccounts.Count; x++)
            {
                _lstBankAccounts.Add(lstLEBanksAccounts[x]);
            }
            _view.BindGridForLegalEntityBankAccounts(_lstBankAccounts);

            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            _view.BindBankAccountControls(lookups.Banks, lookups.BankAccountTypes, new List<IGeneralStatus>(lookups.GeneralStatuses.Values));
        }

       
        protected void OnSearchBankAccountClicked()
        {
            if (_view.BranchCode.Length == 0 ||_view.AccountNumber.Length == 0 )
            {
                _view.Messages.Add(new Error("Bank, Branch, and Account Number must be specified for a search", "Bank, Branch, and Account Number must be specified for a search"));
                return;
            }
           

            string branchCode = _view.BranchCode;
           // string accountName = _view.AccountName;
            //string accountType = _view.AccountType;
            string accountNumber = _view.AccountNumber;


            IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();

          //  GlobalCacheData.Clear();

            GlobalCacheData.Add("BANK", _view.BankKey,LifeTimes);
            GlobalCacheData.Add("BRANCHCODE", _view.BranchCode, LifeTimes);
            GlobalCacheData.Add("ACCOUNTNAME", _view.AccountName, LifeTimes);
            GlobalCacheData.Add("ACCOUNTTYPE", _view.AccountType, LifeTimes);
            GlobalCacheData.Add("ACCOUNTNUMBER", _view.AccountNumber, LifeTimes);



            IBankAccountSearchCriteria criteria = new BankAccountSearchCriteria();
            if (!string.IsNullOrEmpty(branchCode))
            {
                if (branchCode.Contains("-"))
                {
                    int res;
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



         

           // GlobalCacheData.Clear();
            if (GlobalCacheData.ContainsKey(foundBankAccounts))
                GlobalCacheData[foundBankAccounts] = criteria;
            else
            {
                GlobalCacheData.Add(foundBankAccounts, criteria, LifeTimes);
            }         

            if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntity))
                GlobalCacheData[ViewConstants.LegalEntity] = _legalEntity.Key;
            else
            {
                GlobalCacheData.Add(ViewConstants.LegalEntity, _legalEntity.Key, LifeTimes);
            }

            //navigate to BankingDetailsSearch
            _view.Navigator.Navigate("BankingDetailsSearchLegalEntity");

        }
    }


}
