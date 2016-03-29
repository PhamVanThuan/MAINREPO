using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.UI;
using System.Collections.Generic;
using SAHL.Common.Globals;

using SAHL.Common.DomainMessages;
using SAHL.Common.DataAccess;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class PremiumQuoteBase : SAHLCommonBasePresenter<IPremiumQuote>
    {
        private ILifeRepository _lifeRepo;
        private IApplicationRepository _applicationRepo;
        private IAccountRepository _accountRepo;
        private ILegalEntityRepository _legalEntityRepo;
        private IControlRepository _ctrlRepo;
        private ILookupRepository _lookupRepo;

        private IHOC _hocFS;
        private IAccountHOC _hocAccount;
        private double _currentSumAssured;
        private double _annualPremium;
        private double _deathBenefitPremium;
        private double _ipBenefitPremium;
        private double _monthlyInstalment;
        private int _policyStatusKey;

        private CBOMenuNode _node;

        private int _genericKey;

        public int GenericKey
        {
            get { return _genericKey; }
            set { _genericKey = value; }
        }
	

        /// <summary>
        /// 
        /// </summary>
        protected ILifeRepository LifeRepo
        {
            get
            {
                return _lifeRepo;
            }
            set
            {
                _lifeRepo = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ILookupRepository LookupRepo
        {
            get
            {
                return _lookupRepo;
            }
            set
            {
                _lookupRepo = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected IApplicationRepository ApplicationRepo
        {
            get
            {
                return _applicationRepo;
            }
            set
            {
                _applicationRepo = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected IAccountRepository AccountRepo
        {
            get
            {
                return _accountRepo;
            }
            set
            {
                _accountRepo = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ILegalEntityRepository LegalEntityRepo
        {
            get
            {
                return _legalEntityRepo;
            }
            set
            {
                _legalEntityRepo = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected IControlRepository CtrlRepo
        {
            get
            {
                return _ctrlRepo;
            }
            set
            {
                _ctrlRepo = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected IHOC HocFS
        {
            get { return _hocFS; }
            set { _hocFS = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected IAccountHOC HOCAccount
        {
            get { return _hocAccount; }
            set { _hocAccount = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PolicyStatusKey
        {
            get { return _policyStatusKey; }
            set { _policyStatusKey = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double CurrentSumAssured
        {
            get { return _currentSumAssured; }
            set { _currentSumAssured = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double AnnualPremium
        {
            get { return _annualPremium; }
            set { _annualPremium = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double DeathBenefitPremium
        {
            get { return _deathBenefitPremium; }
            set { _deathBenefitPremium = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double IPBenefitPremium
        {
            get { return _ipBenefitPremium; }
            set { _ipBenefitPremium = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double MonthlyInstalment
        {
            get { return _monthlyInstalment; }
            set { _monthlyInstalment = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PremiumQuoteBase(IPremiumQuote view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node    
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _genericKey = _node.GenericKey;
            
            _lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
            _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            _legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            _ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

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

            _view.OnAddButtonClicked += new EventHandler(OnAddButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);

            _view.IPBenefitMaxAge = Convert.ToInt32(_ctrlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.Life.IPBenefitMaxAge).ControlNumeric);

            _view.BindLifePolicyTypes(_lookupRepo.LifePolicyTypes);
        }

        void OnAddButtonClicked(object sender, EventArgs e)
        {
            // validate selection
            if (String.IsNullOrEmpty(_view.LegalEntityName))
                _view.Messages.Add(new Error("Please enter a Name.", "Please enter a Name."));
            if (_view.DateOfBirth.HasValue == false)
                _view.Messages.Add(new Error("Please enter the date of birth in the format dd/mm/ccyy.", "Please enter the date of birth in the format dd/mm/ccyy."));

            if (_view.IsValid)
            {
                // Add the Legal Entity Details to the Datasource and refresh screen
                ILegalEntityNaturalPerson LE = _legalEntityRepo.GetEmptyLegalEntity(SAHL.Common.Globals.LegalEntityTypes.NaturalPerson) as ILegalEntityNaturalPerson;
                LE.FirstNames = _view.LegalEntityName;
                LE.DateOfBirth = _view.DateOfBirth;

                _view.lstLegalEntities.Add(LE);
            }
        }

        void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool CalculatePremiums(int accountKey)
        {
            // validate selection
            if (String.IsNullOrEmpty(_view.SelectedAgeList))
                _view.Messages.Add(new Error("Must select at least one assured life.", "Must select at least one assured life."));

            if (_view.IsValid)
            {
                using (new TransactionScope())
                {
                    LifeRepo.RecalculateSALifePremiumQuote(accountKey, _view.PolicyTypeSelectedValue, true, _view.SelectedAgeList, out _currentSumAssured, out _monthlyInstalment, out _annualPremium, out _deathBenefitPremium, out _ipBenefitPremium);
                }
                
            }

            return _view.IsValid;
        }
    }
}
