using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using System.Collections.Generic;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.Service.Interfaces;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// Presenter for RateChangeInstallmentChange presenter 
    /// </summary>
    public class RateChangeChangeTerm : RateChangeBase
    {
        /// <summary>
        /// Constructor for RateChange TermChange
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        /// 
       // int _instanceID;
        public RateChangeChangeTerm(IRateChange view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }
        /// <summary>
        /// Initialise event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            if (_mlVar != null)
                _lstMortgageLoans.Insert(_view.Messages, 0, _mlVar);


            if (_mlFixed != null)
            {
                _lstMortgageLoans.Insert(_view.Messages, 1, _mlFixed);
                _view.SetTermControlVisibilityForVarifix = true;

            }
            else
                _view.SetTermControlVisibilityForVarifix = false;



            if (_mlVar != null)
                if (_mlVar.HasInterestOnly())
                {
                    _view.BindGridTermInterestOnly(_lstMortgageLoans);
                    _view.SetTermControlVisibilityForInterestOnly = true;
                    _view.SetTermControlVisibilityForTermComment = true;

                }
                else
                {
                    _view.BindGridTermNotInterestOnly(_lstMortgageLoans);
                    _view.SetTermControlVisibilityForInterestOnly = false;
                    _view.SetTermControlVisibilityForTermComment = true;


                }

            _view.SetTermControlsVisibility = true;
            _view.SetRatesControlVisibility = false;
            _view.SetAbilityofSubmitButton = false;

            _view.SetSubmitButtonText("Process Term Change", "T");

            //if ((_lifePolicyAccount != null) && (_mlVar != null))
            if (_mlVar != null && _view.IsValid)
                _view.SubmitButtonClicked += (_view_SubmitButtonClicked);

            _view.CancelButtonClicked += (_view_CancelButtonClicked);

            _view.SetPTIVisibility = false;


        }

        void _view_SubmitButtonClicked(object sender, EventArgs e)
        {
            IMortgageLoanRepository mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
            int term = _view.CapturedTerm;
            string memoDescription = _view.MemoComments.Trim().ToString();          
            
            if (term == 0)
            {
                string err = string.Format("The remaining term must be between 1 and {0} months.", MaximumTerm);
                _view.Messages.Add(new Error(err, err));
                _view.CalculateTerms();
                return;
            }


            if (_mlVar.Account.Product.Key == 2)
            {
                string err = string.Format("Cannot process a term change request on a Varifix account.");
                _view.Messages.Add(new Error(err, err));
                _view.CalculateTerms();
                return;                
            }

            foreach (IApplication app in _mlVar.Account.Applications)
            {
                if (app.ApplicationType.Key == (int)OfferTypes.ReAdvance || app.ApplicationType.Key == (int)OfferTypes.FurtherAdvance || app.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
                {
                    if (app.ApplicationStatus.Key == (int)OfferStatuses.Open)
                    {
                        string err = string.Format("Further Lending is currently in progress. Term Change cannot be processed");
                        _view.Messages.Add(new Error(err, err));
                        _view.CalculateTerms();
                        return;
                    }
                }

                if (app.ApplicationStatus.Key == (int)OfferStatuses.Open)
                {
                    string err = "An open offer is currently in progress. Term Change cannot be processed";
                    if (app.ApplicationType.Key == (int)OfferTypes.ReAdvance || app.ApplicationType.Key == (int)OfferTypes.FurtherAdvance || app.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
                    {
                        err = string.Format("Further Lending is currently in progress. Term Change cannot be processed");
                    }
                    _view.Messages.Add(new Error(err, err));
                    _view.CalculateTerms();
                    return;
                }
            }

            bool writeMemo = true;
            //bool writeMemo = mlRepo.WillSPVChange(_mlVar.Account.Key, _view.CapturedTerm, _mlVar.SumBondLoanAgreementAmounts(), _mlVar.GetActiveValuationAmount(), 1, _mlVar.Account.Product.Key, _mlVar.Account.OriginationSource.Key, 0, 0, _mlVar.SPV.Key);

            if (writeMemo && _view.MemoComments.Trim().Length == 0)
            {
                string err = "Term Change Request, please add comment.";
                _view.Messages.Add(new Error(err, err));
                _view.CalculateTerms();
                return;
            }


            if ((term + ( _mlVar.InitialInstallments -_mlVar.RemainingInstallments)) > MaximumTerm)
            {
                string err = string.Format("The remaining term will exceed {0} months.", MaximumTerm);
                _view.Messages.Add(new Error(err, err));
                _view.CalculateTerms();
                return;
            }
            


            // Check for Open SPV movement
            foreach (IApplication app in _mlVar.Account.Applications)
            {
                if (mlRepo.LookUpPendingTermChangeByAccount(_mlVar.Account.Key))
                {
                    string err = "An application to move this account between SPV's is already in progress";
                    _view.Messages.Add(new Error(err, err));
                    _view.CalculateTerms();
                    return;

                }
            }
           
            //ILifeRepository lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();

            TransactionScope txn = new TransactionScope();

            try
            {
               
                ValidateTermAgainstAge(term);

                // this will check if a rule error/warning has been thrown
                if (!_view.IsValid)
                {
                    _view.CalculateTerms();
                    return;
                }


                //if (createWorkflow == true)
                //{
                    CreateRequestLoanAdjustments();
                //}
                //else
                //{
                //    mlRepo.TermChange(_mlVar.Account.Key, term, _view.CurrentPrincipal.Identity.Name);
                //}

                // X2 Map entry
                ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
                IOrganisationStructureRepository OSR = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IADUser adUser = OSR.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);
                IMemoRepository memoRepo = RepositoryFactory.GetRepository<IMemoRepository>();
                //DateTime ReminderDate = DateTime.Now;
                //DateTime ExpiryDate = DateTime.Now;

                SAHL.Common.BusinessModel.Interfaces.IMemo memo;

                memo = memoRepo.CreateMemo();
                memo.ADUser = adUser;
                memo.GenericKey = _mlVar.Account.Key;
                memo.GenericKeyType = lookups.GenericKeyType.ObjectDictionary["1"];
                //memo.GenericKeyType = lookups.GenericKeyType.ObjectDictionary[GenericKeyTypes.Account.ToString()];
                memo.InsertedDate = DateTime.Now;
                memo.ReminderDate = DateTime.Now;
                memo.ExpiryDate = DateTime.Now.AddDays(1) ;
                memo.Description = "Term Change: "  + memoDescription.ToString();
                memo.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Inactive];



                if (writeMemo || memoDescription.Length > 0)
                    memoRepo.SaveMemo(memo);

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
                //lifeRepo.RecalculateSALifePremium(_lifePolicyAccount, true);

                if (_view.IsValid)
                    Navigator.Navigate("Submit");

            }

        }


        private void CreateRequestLoanAdjustments()
        {
            IX2Info XI = null;
            try
            {
                Dictionary<string, string> Inputs = new Dictionary<string, string>();
                Inputs.Add("AccountKey", _mlVar.Account.Key.ToString());
                Inputs.Add("SPVKey", _mlVar.Account.SPV.Key.ToString());
                Inputs.Add("RequestUser", _view.CurrentPrincipal.Identity.Name);
                Inputs.Add("NewTerm", _view.CapturedTerm.ToString());
                Inputs.Add("OldTerm", _mlVar.RemainingInstallments.ToString());
                Inputs.Add("LoanAdjustmentType", "1");
                XI = X2Service.GetX2Info(_view.CurrentPrincipal);
                if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                    X2Service.LogIn(_view.CurrentPrincipal);

                X2Service.CreateWorkFlowInstance(_view.CurrentPrincipal, SAHL.Common.Constants.WorkFlowProcessName.LoanAdjustments, (-1).ToString(), SAHL.Common.Constants.WorkFlowName.LoanAdjustments, SAHL.Common.Constants.WorkFlowActivityName.LoanAdjustmentCreateInstance, Inputs, false);
                X2ServiceResponse response = X2Service.CreateCompleteActivity(_view.CurrentPrincipal, Inputs, false, null);
                if (response.IsError)
                {
                    if (XI != null)
                    {
                        if (XI.InstanceID > 0)
                        {
                            X2Service.CancelActivity(_view.CurrentPrincipal);
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (XI != null)
                {
                    if (XI.InstanceID > 0)
                    {
                        X2Service.CancelActivity(_view.CurrentPrincipal);
                    }
                }
            }
        }


        //TODO : This must become a rule in the rules database
        void ValidateTermAgainstAge(int termEntered)
        {
            // Find the lowest date of birth.
            DateTime dtNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime dtBirth = dtNow;

            for (int a = 0; a < _account.Roles.Count; a++)
            {
                if (_account.Roles[a].LegalEntity.LegalEntityType.Key == (int)LegalEntityTypes.NaturalPerson)
                {
                    if (((ILegalEntityNaturalPerson)_account.Roles[a].LegalEntity).DateOfBirth != null)
                    {
                        if (((ILegalEntityNaturalPerson)_account.Roles[a].LegalEntity).DateOfBirth < dtBirth)
                            dtBirth = Convert.ToDateTime(((ILegalEntityNaturalPerson)_account.Roles[a].LegalEntity).DateOfBirth);
                    }
                }
            }

            if (dtBirth != dtNow) // We actually got a date.
            {
                DateTime dt60 = dtBirth.AddYears(60);
                DateTime dtTerm = dtNow.AddMonths(termEntered);
                if (dt60 < dtTerm)
                    _view.Messages.Add(
                        new Warning(
                            "The new remaining term makes the final repayment after the legal entity’s 60th birthday. Do you still wish to proceed with the term change?",
                            "The new remaining term makes the final repayment after the legal entity’s 60th birthday. Do you still wish to proceed with the term change?"));
            }
        }

        void _view_CancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Cancel");
        }

    }
}