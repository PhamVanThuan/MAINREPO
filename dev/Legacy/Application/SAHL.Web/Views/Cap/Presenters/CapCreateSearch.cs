using System;
using System.Data;
using SAHL.Web.Views.Cap.Interfaces;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Cap.Presenters
{
    public class CapCreateSearch : SAHLCommonBasePresenter<ICapCreateSearch>
    {
        IAccountRepository _accRepo;
        ICapRepository _capRepo;
        IAccount _account;

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CapCreateSearch(ICapCreateSearch view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSearchButtonClicked += new EventHandler(_view_OnSearchButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            _view.OnGridSelectDoubleClick += new KeyChangedEventHandler(_view_OnGridSelectDoubleClick);
            _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            _capRepo = RepositoryFactory.GetRepository<ICapRepository>();
            _view.SearchViewCustomSetUp();
            _view.BindAccountTypeDropdown();
            _view.AccountTypeDropDown = (int)AccountTypes.MortgageLoan;
            _view.AccountTypeEnabled = false;

            if (_view.AccountNumber != -1)
            {
                _account = _accRepo.GetAccountByKey(_view.AccountNumber);
                if (_account != null)
                {
                    _view.SubmitButtonEnabled = false;
                    BindSearchGrid();
                }
            }
            else
                _view.SubmitButtonEnabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _view_OnCancelButtonClicked(object sender, EventArgs e) 
        {
            _view.Navigator.Navigate("ClientSuperSearch");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _view_OnSearchButtonClicked(object sender, EventArgs e)
        {
            if (_view.AccountNumber != -1)
            {
                List<IAccount> accLst = _capRepo.CapAccountSearch(_view.AccountNumber);
                if (accLst.Count > 0)
                    _account = accLst[0];
                //_account = _accRepo.GetAccountByKey(_view.AccountNumber);

                if (_account != null)
                {
                    BindSearchGrid();
                    _view.SubmitButtonEnabled = true;
                }
                else
                    _view.SubmitButtonEnabled = false;
            }
            else
                _view.SubmitButtonEnabled = false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _view_OnSubmitButtonClicked(object sender, EventArgs e) 
        {
            _account = _accRepo.GetAccountByKey(_view.AccountNumber);
            if (_account != null)
            {

                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                IADUser adUser = secRepo.GetADUserByPrincipal(_view.CurrentPrincipal);

                // clear the legalentity out of global cache if exists
                if (GlobalCacheData.ContainsKey("CapAccountKey"))
                    GlobalCacheData.Remove("CapAccountKey");

                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                List<string> rulesToRun = new List<string>();
                rulesToRun.Add("ApplicationCap2AccountResetDateCheck");
                rulesToRun.Add("ApplicationCAP2QualifyStatus");
                rulesToRun.Add("ApplicationCap2QualifyUnderCancel");
                rulesToRun.Add("ApplicationCap2QualifyProduct");
                rulesToRun.Add("ApplicationCap2CurrentBalance");
                rulesToRun.Add("ApplicationCap2CapTypeConfig");
                rulesToRun.Add("ApplicationCAP2VerifyCapBroker");
                rulesToRun.Add("ApplicationCap2QualifyDebtCounselling");
                svc.ExecuteRuleSet(spc.DomainMessages, rulesToRun, _account, adUser);

                IMortgageLoanAccount mla = _account as IMortgageLoanAccount;
                svc.ExecuteRule(spc.DomainMessages, "FinancialAdjustmentPending", mla.SecuredMortgageLoan, FinancialAdjustmentTypeSources.CAP2);

                if (_view.Messages.Count == 0)
                {   
                    GlobalCacheData.Add("CapAccountKey", _account.Key, new List<ICacheObjectLifeTime>());
                    base.Navigator.Navigate("EntitySelected");
                }
                else
                    _view.SubmitButtonEnabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _view_OnGridSelectDoubleClick(object sender, KeyChangedEventArgs e)
        {
            _view_OnSubmitButtonClicked(null, null);
        }

        #region Helper Methods

        private void BindSearchGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("LegalEntityName", typeof(String)));
            dt.Columns.Add(new DataColumn("LENumber", typeof(String)));
            dt.Columns.Add(new DataColumn("AccountNumber", typeof(int)));
            dt.Columns.Add(new DataColumn("RoleType", typeof(String)));
            dt.Columns.Add(new DataColumn("AccountType", typeof(String)));

            foreach (IRole role in _account.Roles)
            {
                string leNumber = string.Empty;
                ILegalEntityNaturalPerson leNp = role.LegalEntity as ILegalEntityNaturalPerson;
                ILegalEntityGenericCompany leGC = role.LegalEntity as ILegalEntityGenericCompany;

                if (leNp != null)
                    leNumber = leNp.IDNumber;
                else
                    leNumber = leGC.RegistrationNumber;

                dt.Rows.Add(CreateSearchResultDataRow(role.LegalEntity.DisplayName,
                    leNumber,
                    _account.Key,
                    role.RoleType.Description,
                    AccountTypes.MortgageLoan.ToString(),
                    dt
                    ));
            }

            _view.BindSearchResultsGrid(dt);
        }

        private static DataRow CreateSearchResultDataRow(string LegalEntityName,
                                                string LENumber,
                                                int AccountNumber,
                                                string RoleType,
                                                string AccountType,
                                                DataTable data)
        {
            DataRow dr = data.NewRow();
            dr["LegalEntityName"] = LegalEntityName;
            dr["LENumber"] = LENumber;
            dr["AccountNumber"] = AccountNumber;
            dr["RoleType"] = RoleType;
            dr["AccountType"] = AccountType;
            return dr;
        }


        #endregion
    }
}
