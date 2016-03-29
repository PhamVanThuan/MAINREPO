using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanCalculator : SAHLCommonBasePresenter<IPersonalLoanCalculator>
    {
        private const string PersonalLoanOptionsCacheKey = "PersonalLoanOptions";
        private const string PersonalLoanUserTermsCacheKey = "PersonalLoanUserTerms";
        private IApplicationRepository applicationRepository;
        private IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository;
        private ICreditCriteriaUnsecuredLendingRepository creditCriteriaUnsecuredLendingRepository;
        private ILookupRepository lookUpRepository;
        private IMarketRateRepository marketRateRepository;
        private List<ICacheObjectLifeTime> lifeTimes;

        private CBOMenuNode node;
        private int applicationKey;

        public ILookupRepository LookUpRepository
        {
            get
            {
                if (lookUpRepository == null)
                {
                    lookUpRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                }
                return lookUpRepository;
            }
        }

        public IApplicationUnsecuredLendingRepository ApplicationUnsecuredLendingRepository
        {
            get
            {
                if (applicationUnsecuredLendingRepository == null)
                {
                    applicationUnsecuredLendingRepository = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();
                }
                return applicationUnsecuredLendingRepository;
            }
        }

        public ICreditCriteriaUnsecuredLendingRepository CreditCriteriaUnsecuredLendingRepository
        {
            get
            {
                if (creditCriteriaUnsecuredLendingRepository == null)
                {
                    creditCriteriaUnsecuredLendingRepository = RepositoryFactory.GetRepository<ICreditCriteriaUnsecuredLendingRepository>();
                }
                return creditCriteriaUnsecuredLendingRepository;
            }
        }

        public IMarketRateRepository MarketRateRepository
        {
            get
            {
                if (marketRateRepository == null)
                {
                    marketRateRepository = RepositoryFactory.GetRepository<IMarketRateRepository>();
                }
                return marketRateRepository;
            }
        }

        public IApplicationRepository ApplicationRepository
        {
            get
            {
                if (applicationRepository == null)
                {
                    applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
                }
                return applicationRepository;
            }
        }

        protected List<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add("WF_PersonalLoanCalculator");
                    lifeTimes = new List<ICacheObjectLifeTime>();
                    lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }
                return lifeTimes;
            }
        }

        public PersonalLoanCalculator(IPersonalLoanCalculator view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.OnCalculateButtonClicked += new EventHandler(OnCalculateClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelClicked);
            _view.OnCreateApplicationButtonClicked += new EventHandler<PersonalLoanOptionSelectedEventArgs>(OnCreateApplicationClicked);
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage) return;

            node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            applicationKey = node.GenericKey;

            //Get the application
            var applicationUnsecuredLending = ApplicationUnsecuredLendingRepository.GetApplicationByKey(applicationKey);
            if (applicationUnsecuredLending.GetLatestApplicationInformation() == null)
            {
                return;
            }

            _view.SetTextOnCreateApplicationButton = "Update Application";

            // The postback will be more likely be caused by a button click event
            // which can mean different values have been entered
            // hence we don't rebind => rather read, calc and bind
            if (_view.IsPostBack)
            {
                return;
            }

            var applicationProductPersonalLoan = applicationUnsecuredLending.CurrentProduct as IApplicationProductPersonalLoan;
            var applicationInformationPersonalLoan = applicationProductPersonalLoan.ApplicationInformationPersonalLoan;

            var loanAmount = applicationInformationPersonalLoan.LoanAmount;
            var term = applicationInformationPersonalLoan.Term;
            var lifePremium = applicationInformationPersonalLoan.LifePremium;
            var hasLifePremium = lifePremium > 0;

            Calculate(loanAmount, term, hasLifePremium);
            _view.BindApplication(applicationInformationPersonalLoan);
        }

        private void OnCreateApplicationClicked(object sender, PersonalLoanOptionSelectedEventArgs e)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
                    IApplication application = ApplicationRepository.GetApplicationByKey(cboNode.GenericKey);

                    IApplicationUnsecuredLending applicationUnsecuredLending = application as IApplicationUnsecuredLending;
                    if (ShouldCreateRevision(applicationUnsecuredLending))
                    {
                        applicationUnsecuredLending.CreateRevision();
                    }

                    var result = ((IResult)GlobalCacheData[PersonalLoanOptionsCacheKey]);
                    var selectedPersonalLoanOption = result.CalculatedItems.First(x => x.ID == e.SelectedOptionKey);

                    applicationUnsecuredLendingRepository.SetupPersonalLoanApplication(_view.Messages, application, _view.CreditLifePolicy, result, selectedPersonalLoanOption);

                    // Save Application
                    ApplicationRepository.SaveApplication(applicationUnsecuredLending);

                    if (_view.IsValid)
                    {
                        CompleteActivity();
                        transactionScope.VoteCommit();
                    }
                }
                catch (Exception)
                {
                    transactionScope.VoteRollBack();

                    if (_view.IsValid)
                        throw;
                }
            }
        }

        private static bool ShouldCreateRevision(IApplicationUnsecuredLending applicationUnsecuredLending)
        {
            if (applicationUnsecuredLending.GetLatestApplicationInformation() == null)
                return false;

            var applicationProductPersonalLoan = applicationUnsecuredLending.CurrentProduct as IApplicationProductPersonalLoan;
            var latestApplicationInformation = applicationUnsecuredLending.GetLatestApplicationInformation();

            if (applicationProductPersonalLoan != null &&
                latestApplicationInformation != null &&
                latestApplicationInformation.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
            {
                return true;
            }
            return false;
        }

        private void CompleteActivity()
        {
            try
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                this.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);
                this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }
            catch (Exception)
            {
                // we must cancel the activity here, otherwise if the user navigates to another node and
                // tries to perform a workflow action, X2 may try to perform the action on the wrong
                // activity
                if (_view.IsValid)
                {
                    this.X2Service.CancelActivity(_view.CurrentPrincipal);
                    throw;
                }
            }
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            _view.CreditLifePolicy = true;
            Navigator.Navigate(ViewConstants.CancelView);
        }

        private void OnCalculateClicked(object sender, EventArgs e)
        {
            Calculate(_view.LoanAmount, _view.Term, _view.CreditLifePolicy);
        }

        private void Calculate(double loanAmount, int? term, bool creditLifePolicy)
        {
            //Rules for max/min amount and max/min term.
            IRuleService svcRule = ServiceFactory.GetService<IRuleService>();

            if (term.HasValue)
            {
                svcRule.ExecuteRule(_view.Messages, "CheckPersonalLoanTerm", term.Value);
            }

            svcRule.ExecuteRule(_view.Messages, "CheckPersonalLoanAmount", loanAmount);
            if (_view.IsValid)
            {
                //cache user input terms
                List<int> userInputTerms = null;
                if (GlobalCacheData.ContainsKey(PersonalLoanUserTermsCacheKey))
                {
                    userInputTerms = (List<int>)GlobalCacheData[PersonalLoanUserTermsCacheKey];
                    if (term.HasValue && !userInputTerms.Contains(term.Value))
                    {
                        userInputTerms.Add(term.Value);
                    }
                    GlobalCacheData[PersonalLoanUserTermsCacheKey] = userInputTerms;
                }
                else
                {
                    userInputTerms = new List<int>();
                    if (term.HasValue)
                    {
                        userInputTerms.Add(term.Value);
                    }
                    GlobalCacheData.Add(PersonalLoanUserTermsCacheKey, userInputTerms, LifeTimes);
                }

                // Calculate the result
                IResult result = ApplicationUnsecuredLendingRepository.CalculateUnsecuredLending(loanAmount, userInputTerms, creditLifePolicy);

                // Cache the calculated result
                if (GlobalCacheData.ContainsKey(PersonalLoanOptionsCacheKey))
                {
                    GlobalCacheData[PersonalLoanOptionsCacheKey] = result;
                }
                else
                {
                    GlobalCacheData.Add(PersonalLoanOptionsCacheKey, result, LifeTimes);
                }

                // Bind the result
                _view.BindResult(result);
            }
        }
    }
}