using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanChangeInstalment : SAHLCommonBasePresenter<IPersonalLoanChangeInstalment>
    {
        public PersonalLoanChangeInstalment(IPersonalLoanChangeInstalment view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// account interface
        /// </summary>
        protected IAccountPersonalLoan _accountPersonalLoan;

        /// <summary>
        /// Account Repository
        /// </summary>
        protected IAccountRepository _accRepo;

        public IAccountRepository AccountRepo
        {
            get { return _accRepo; }
            set { _accRepo = value; }
        }

        /// <summary>
        /// List of Personal Loan Financial Services
        /// </summary>
        protected IEventList<IFinancialService> _lstFinancialServices;

        /// <summary>
        /// Current logged on user
        /// </summary>
        protected SAHLPrincipal _principal;

        /// <summary>
        /// CBO Menu Node
        /// </summary>
        protected CBOMenuNode _node;

        /// <summary>
        /// OnView Initialised event - retrieve data for use by presenters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

            _view.EnableSubmitButton = false;
            _view.EnableCalculateButton = false;

            _view.SubmitButtonClicked += View_SubmitButtonClicked;
            _view.CancelButtonClicked += View_CancelButtonClicked;
            _view.CalculateButtonClicked += View_CalculateButtonClicked;

            _lstFinancialServices = new EventList<IFinancialService>();

            _accountPersonalLoan = _accRepo.GetAccountByKey(Convert.ToInt32(_node.GenericKey)) as IAccountPersonalLoan;

            // add the personal loan financial service to the list
            if (_accountPersonalLoan != null)
            {
                var personalLoanFinancialService = _accountPersonalLoan.GetFinancialServiceByType(FinancialServiceTypes.PersonalLoan);
                _lstFinancialServices.Insert(_view.Messages, 0, personalLoanFinancialService);
                _view.EnableCalculateButton = true;

                // get the credit life premium
                double creditLifePremium = 0; double monthlyServiceFee = 0;
                var creditProtectionPlanFinancialService = personalLoanFinancialService.FinancialServices.Where(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.SAHLCreditProtectionPlan).FirstOrDefault();
                if (creditProtectionPlanFinancialService != null)
                    creditLifePremium = creditProtectionPlanFinancialService.Payment;

                // get the service fee
                monthlyServiceFee = _accountPersonalLoan.InstallmentSummary.MonthlyServiceFee;

                // set the values
                _view.CreditLifePremium = creditLifePremium;
                _view.MonthlyServiceFee = monthlyServiceFee;
            }

            _view.BindPersonalLoansGrid(_lstFinancialServices);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (_view.Messages.ErrorMessages.Count > 0)
                _view.EnableSubmitButton = false;
        }

        void View_CancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("PL_LoanAdjustments");
        }

        void View_CalculateButtonClicked(object sender, EventArgs e)
        {
            CalculateInstalment();
        }

        void View_SubmitButtonClicked(object sender, EventArgs e)
        {
            CalculateInstalment();

            IMortgageLoanRepository mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
            string memoDescription = _view.MemoComments.Trim().ToString();

            if (String.IsNullOrEmpty(memoDescription))
            {
                string err = "Instalment Change Request, please add Comment.";
                _view.Messages.Add(new Error(err, err));
                return;
            }

            using (TransactionScope txn = new TransactionScope())
            {
                try
                {
                    // this will check if a rule error/warning has been thrown
                    if (!_view.IsValid)
                        return;

                    ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
                    IOrganisationStructureRepository OSR = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                    IADUser adUser = OSR.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);
                    IMemoRepository memoRepo = RepositoryFactory.GetRepository<IMemoRepository>();
                    IStageDefinitionRepository stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

                    // call halo api to change the term
                    IPersonalLoanRepository personalLoanRepository = RepositoryFactory.GetRepository<IPersonalLoanRepository>();
                    personalLoanRepository.ChangeInstalment(_accountPersonalLoan.Key, adUser.ADUserName);

                    // insert stage transition
                    stageDefinitionRepo.SaveStageTransition(_accountPersonalLoan.Key, StageDefinitionStageDefinitionGroups.PersonalLoanChangeInstalment, "", adUser.ADUserName);

                    // insert the memo record
                    if (memoDescription.Length > 0)
                    {
                        IMemo memo = memoRepo.CreateMemo();
                        memo.ADUser = adUser;
                        memo.GenericKey = _accountPersonalLoan.Key;
                        memo.GenericKeyType = lookups.GenericKeyType.ObjectDictionary[((int)GenericKeyTypes.Account).ToString()];
                        memo.InsertedDate = DateTime.Now;
                        memo.ReminderDate = DateTime.Now;
                        memo.ExpiryDate = DateTime.Now.AddDays(1);
                        memo.Description = "Instalment Change: " + memoDescription.ToString();
                        memo.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Inactive];

                        memoRepo.SaveMemo(memo);
                    }

                    txn.VoteCommit();
                }

                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
                finally
                {
                    Navigator.Navigate("PL_LoanAdjustments");
                }
            }
        }

        private void CalculateInstalment()
        {
            double instalment = 0;
            IFinancialService fs = _lstFinancialServices[0] as IFinancialService;
            instalment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(fs.Balance.Amount, fs.Balance.LoanBalance.InterestRate, fs.Balance.LoanBalance.RemainingInstalments, false);

            _view.NewInstalment = instalment;

            if (instalment >= 0)
                _view.EnableSubmitButton = true;
        }
    }
}