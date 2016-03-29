using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.DebitOrderDetailsFinancialService
{
    /// <summary>
    ///
    /// </summary>
    public class DebitOrderDetailsFSBase : SAHLCommonBasePresenter<IDebitOrderDetails>
    {
        private CBOMenuNode _node;
        private IEventList<IBankAccount> _bankAccounts;
        private IFinancialService _financialService;
        private Hashtable _fdcMap;
        private Hashtable _fdcdMap;
        private IAccount _account;

        private IEmploymentRepository _employmentRepo; 

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

        public Hashtable FDCMap
        {
            get
            {
                return _fdcMap;
            }
        }

        public Hashtable FDCDMap
        {
            get
            {
                return _fdcdMap;
            }
        }

        public IEventList<IBankAccount> BankAccounts
        {
            get
            {
                return _bankAccounts;
            }
            set
            {
                _bankAccounts = value;
            }
        }

        public IFinancialService FinancialService
        {
            get
            {
                return _financialService;
            }
            set
            {
                _financialService = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DebitOrderDetailsFSBase(IDebitOrderDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
        }

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
            _view.OnGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnGridSelectedIndexChanged);

            IFinancialServiceRepository FSR = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            IDebtCounsellingRepository dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
            _employmentRepo = RepositoryFactory.GetRepository<IEmploymentRepository>();

            switch (_node.GenericKeyTypeKey)
            {
                case (int)GenericKeyTypes.FinancialService:
                    {
                        _financialService = FSR.GetFinancialServiceByKey(_node.GenericKey);
                        _account = _financialService.Account;
                        break;
                    }
                case (int)GenericKeyTypes.DebtCounselling2AM:
                    {
                        IDebtCounselling dc = dcRepo.GetDebtCounsellingByKey(_node.GenericKey);
                        _financialService = dc.Account.FinancialServices[0];
                        _account = dc.Account;
                        break;
                    }
                case (int)GenericKeyTypes.Account:
                    {
                        IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                        IAccount acc = accRepo.GetAccountByKey(_node.GenericKey);
                        _account = acc;
                        _financialService = acc.FinancialServices[0];
                        break;
                    }
                case (int)GenericKeyTypes.ParentAccount:
                    {
                        IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                        IAccount acc = accRepo.GetAccountByKey(_node.GenericKey);
                        _account = acc;
                        var financialServices = acc.FinancialServices
                                                   .Where(x => (x.AccountStatus.Key == (int)AccountStatuses.Open || x.AccountStatus.Key == (int)AccountStatuses.Closed) && x.FinancialServiceBankAccounts.Count > 0)
                                                   .OrderByDescending(x => x.Key);
                        //there should only be one personal financial service
                        _financialService = financialServices.First();
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException("Please define behaviour for the Generic Key Type");
                    }
            }

            _view.ShowControls = true;
            _view.ForceShowBankAccountControl = true;

            int[] roleTypes = new int[2] { (int)SAHL.Common.Globals.RoleTypes.MainApplicant, (int)SAHL.Common.Globals.RoleTypes.Suretor };
            IReadOnlyEventList<ILegalEntity> legalEntities = _financialService.Account.GetLegalEntitiesByRoleType(_view.Messages, roleTypes);

            _bankAccounts = new EventList<IBankAccount>();
            for (int i = 0; i < _financialService.FinancialServiceBankAccounts.Count; i++)
            {
                if (_financialService.FinancialServiceBankAccounts[i].BankAccount != null
                    && _financialService.FinancialServiceBankAccounts[i].GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active
                    && _financialService.FinancialServiceBankAccounts[i].BankAccount.ACBType.Key != (int)SAHL.Common.Globals.ACBTypes.Bond
                    && _financialService.FinancialServiceBankAccounts[i].BankAccount.ACBType.Key != (int)SAHL.Common.Globals.ACBTypes.CreditCard)
                    _bankAccounts.Add(_view.Messages, _financialService.FinancialServiceBankAccounts[i].BankAccount);
            }

            for (int i = 0; i < legalEntities.Count; i++)
            {
                //todo check if legalEntity is active on this account

                for (int x = 0; x < legalEntities[i].LegalEntityBankAccounts.Count; x++)
                {
                    if (legalEntities[i].LegalEntityBankAccounts[x].BankAccount != null &&
                        legalEntities[i].LegalEntityBankAccounts[x].GeneralStatus.Key == (int)GeneralStatuses.Active)
                    {
                        _bankAccounts.Add(_view.Messages, legalEntities[i].LegalEntityBankAccounts[x].BankAccount);
                    }
                }
            }

            IDictionary<ILegalEntity, string> dicSalaryPaymentDates = _employmentRepo.GetSalaryPaymentDaysByGenericKey(_node.GenericKey, _node.GenericKeyTypeKey);

            _view.BindSalaryPaymentDays(dicSalaryPaymentDates);

            //Run rules
            CheckRules();
        }

        private void _view_OnGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.ShowButtons = false;
            _view.ShowControls = true;
            _view.ShowLabels = true;
        }

        private void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
        }

        protected void BuildMappingTable()
        {
            IFutureDatedChangeRepository FDC = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();
            _fdcMap = FDC.FutureDatedChangeMap(_financialService.Key);
            _fdcdMap = FDC.FutureDatedChangeDetailMap(_financialService.Key);
        }

        protected static IFutureDatedChange UpdateFixedFDC(IFutureDatedChange fdc, IFutureDatedChangeDetail fdcd)
        {
            for (int i = 0; i < fdc.FutureDatedChangeDetails.Count; i++)
            {
                if (fdc.FutureDatedChangeDetails[i].Key == fdcd.Key)
                {
                    fdc.FutureDatedChangeDetails[i] = fdcd;
                    break;
                }
            }
            return fdc;
        }

        public void CheckRules()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            svc.ExecuteRule(_view.Messages, "NaedoDebitOrderPendingWarning", _account);
        }

        //void SaveDebitOrderDetail()
        //{
        //    IFutureDatedChangeRepository FDC = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();
        //    FDC.
        //}
    }
}