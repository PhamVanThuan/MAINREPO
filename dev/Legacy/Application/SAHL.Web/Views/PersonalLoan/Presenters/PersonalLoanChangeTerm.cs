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
    public class PersonalLoanChangeTerm : SAHLCommonBasePresenter<IPersonalLoanChangeTerm>
    {
        public PersonalLoanChangeTerm(IPersonalLoanChangeTerm view, SAHLCommonBaseController controller)
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
        ///
        /// </summary>
        protected IControlRepository _controlRepo;

        public IControlRepository ControlRepo
        {
            get { return _controlRepo; }
            set { _controlRepo = value; }
        }

        private int _maximumTerm;

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
            _controlRepo = RepositoryFactory.GetRepository<IControlRepository>();

            _view.EnableSubmitButton = false;
            _view.EnableCalculateButton = false;

            _view.SubmitButtonClicked += _view_SubmitButtonClicked;
            _view.CancelButtonClicked += _view_CancelButtonClicked;
            _view.CalculateButtonClicked += _view_CalculateButtonClicked;

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

            // get the max term allowed
            _maximumTerm = Convert.ToInt32(_controlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.PersonalLoan.MaxPersonalLoanTerm).ControlNumeric);

            _view.BindPersonalLoansGrid(_lstFinancialServices);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (_view.Messages.ErrorMessages.Count > 0)
                _view.EnableSubmitButton = false;
        }

        void _view_CancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Cancel");
        }

        void _view_CalculateButtonClicked(object sender, EventArgs e)
        {
            if (ValidateTerm() == false)
                return;

            CalculateInstalment();
        }

        void _view_SubmitButtonClicked(object sender, EventArgs e)
        {
            if (ValidateTerm() == false)
                return;

            CalculateInstalment();

            IMortgageLoanRepository mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
            int term = _view.CapturedTerm;
            string memoDescription = _view.MemoComments.Trim().ToString();

            if (String.IsNullOrEmpty(memoDescription))
            {
                string err = "Term Change Request, please add Comment.";
                _view.Messages.Add(new Error(err, err));
                return;
            }

            TransactionScope txn = new TransactionScope();

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
                personalLoanRepository.ChangeTerm(_accountPersonalLoan.Key, term, adUser.ADUserName);

                // insert stage transition
                stageDefinitionRepo.SaveStageTransition(_accountPersonalLoan.Key, StageDefinitionStageDefinitionGroups.PersonalLoanChangeTerm, "", adUser.ADUserName);

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
                    memo.Description = "Term Change: " + memoDescription.ToString();
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
                txn.Dispose();

                if (_view.IsValid)
                    Navigator.Navigate("Submit");
            }
        }

        private bool ValidateTerm()
        {
            if (_view.CapturedTerm == _accountPersonalLoan.GetFinancialServiceByType(FinancialServiceTypes.PersonalLoan).Balance.LoanBalance.RemainingInstalments)
            {
                string err = "The new remaining term cannot be the same as the current term";
                _view.Messages.Add(new Error(err, err));
                return false;
            }

            if (_view.CapturedTerm < 1 || _view.CapturedTerm > _maximumTerm)
            {
                string err = string.Format("The remaining term must be between 1 and {0} months.", _maximumTerm);
                _view.Messages.Add(new Error(err, err));
                return false;
            }

            if (_view.CapturedTerm > _accountPersonalLoan.MaxNewRemainingInstalmentsAllowed)
            {
                string err = string.Format("The new remaining term cannot exceed {0} months.", _accountPersonalLoan.MaxNewRemainingInstalmentsAllowed);
                _view.Messages.Add(new Error(err, err));
                return false;
            }

            return true;
        }

        private void CalculateInstalment()
        {
            double instalment = 0;
            IFinancialService fs = _lstFinancialServices[0] as IFinancialService;
            instalment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallmentAtEndOfPeriod(fs.Balance.LoanBalance.InterestRate, 12, fs.Balance.Amount, _view.CapturedTerm);

            _view.NewInstalment = instalment;

            if (instalment >= 0)
                _view.EnableSubmitButton = true;
        }
    }
}