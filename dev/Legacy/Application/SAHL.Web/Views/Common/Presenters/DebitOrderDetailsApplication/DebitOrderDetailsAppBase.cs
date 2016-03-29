using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using System.Collections.Generic;


namespace SAHL.Web.Views.Common.Presenters.DebitOrderDetailsApplication
{
    /// <summary>
    /// 
    /// </summary>
    public class DebitOrderDetailsAppBase : SAHLCommonBasePresenter<IDebitOrderDetails>
    {
        protected CBOMenuNode _node;
        protected IEventList<IBankAccount> _bankAccounts;
        protected IApplication _application;
        protected ILegalEntityRepository legalEntityRepository;
        protected IEmploymentRepository employmentRepo;

      

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

        public ILegalEntityRepository LegalEntityRepository
        {
            get
            {
                if (legalEntityRepository == null)
                {
                    legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                }
                return legalEntityRepository;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DebitOrderDetailsAppBase(IDebitOrderDetails view, SAHLCommonBaseController controller)
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

            _view.OnGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnGridSelectedIndexChanged);
            IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
            employmentRepo = RepositoryFactory.GetRepository<IEmploymentRepository>();
            _application = AR.GetApplicationByKey(int.Parse(_node.GenericKey.ToString()));
            _view.gridPostBackType = GridPostBackType.None;

            _view.ShowControls = true;
            _view.ForceShowBankAccountControl = true;
            _view.HideEffectiveDate = true;

            _bankAccounts = new EventList<IBankAccount>();
            IReadOnlyEventList<ILegalEntity> legalEntities = null;

            // TODO: Nasty hack for any application per unsecured lending
            if (_application.ApplicationType.Key < (int)OfferTypes.UnsecuredLending)
            {
                SAHL.Common.Globals.OfferRoleTypes[] roleTypes = new SAHL.Common.Globals.OfferRoleTypes[4];

                roleTypes[0] = SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant;
                roleTypes[1] = SAHL.Common.Globals.OfferRoleTypes.LeadSuretor;
                roleTypes[2] = SAHL.Common.Globals.OfferRoleTypes.MainApplicant;
                roleTypes[3] = SAHL.Common.Globals.OfferRoleTypes.Suretor;

                legalEntities = _application.GetLegalEntitiesByRoleType(roleTypes, GeneralStatusKey.Active);
            }
            else
            {
                legalEntities = LegalEntityRepository.GetLegalEntitiesByExternalRoleTypeGroup(_node.GenericKey, _node.GenericKeyTypeKey, ExternalRoleTypeGroups.Client, GeneralStatuses.Active);
            }

            for (int i = 0; i < legalEntities.Count; i++)
            {
                //todo check if legalEntity is active on this account

                for (int x = 0; x < legalEntities[i].LegalEntityBankAccounts.Count; x++)
                {
                    if (legalEntities[i].LegalEntityBankAccounts[x].BankAccount != null
                        && legalEntities[i].LegalEntityBankAccounts[x].GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active
                         && legalEntities[i].LegalEntityBankAccounts[x].BankAccount.ACBType.Key != (int)SAHL.Common.Globals.ACBTypes.Bond
                         && legalEntities[i].LegalEntityBankAccounts[x].BankAccount.ACBType.Key != (int)SAHL.Common.Globals.ACBTypes.CreditCard)
                    {
                        _bankAccounts.Add(_view.Messages, legalEntities[i].LegalEntityBankAccounts[x].BankAccount);
                    }
                }
            }

            IDictionary<ILegalEntity, string> dicSalaryPaymentDates = employmentRepo.GetSalaryPaymentDaysByGenericKey(_node.GenericKey, _node.GenericKeyTypeKey);

            _view.BindSalaryPaymentDays(dicSalaryPaymentDates);
        }

        void _view_OnGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            int row = int.Parse(e.Key.ToString());
            
            _view.BindLabels(row);
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

    }
}
