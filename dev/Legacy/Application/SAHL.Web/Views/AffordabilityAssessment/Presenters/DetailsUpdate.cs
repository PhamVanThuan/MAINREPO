using Castle.ActiveRecord;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Web.UI;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using SAHL.V3.Framework;
using SAHL.V3.Framework.Repositories;
using SAHL.Web.Views.AffordabilityAssessment.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.AffordabilityAssessment.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class DetailsUpdate : DetailsBase
    {
        private IDecisionTreeRepository decisionTreeRepository;

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DetailsUpdate(IDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        public IDecisionTreeRepository DecisionTreeRepo
        {
            get
            {
                if (decisionTreeRepository == null)
                {
                    decisionTreeRepository = ServiceManager.Get<IDecisionTreeRepository>();
                }
                return decisionTreeRepository;
            }
        }

        public IV3ServiceManager ServiceManager
        {
            get
            {
                if (v3ServiceManager == null)
                {
                    v3ServiceManager = V3ServiceManager.Instance;
                }
                return v3ServiceManager;
            }
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(OnSubmitButtonClicked);

            if (spc == null)
                spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);

            if (spc.Principal.IsInRole("Credit Manager") || spc.Principal.IsInRole("Credit Supervisor") || spc.Principal.IsInRole("Credit Exceptions") || spc.Principal.IsInRole("Credit Underwriter") ||
                spc.Principal.IsInRole("PL Credit Analyst") || spc.Principal.IsInRole("PL Credit Exceptions Manager"))
                _view.AffordabilityAssessmentMode = AffordabilityAssessmentMode.Update_Credit;
            else
                _view.AffordabilityAssessmentMode = AffordabilityAssessmentMode.Update_NonCredit;

            if (spc.Principal.IsInRole("FL Application Processor") || spc.Principal.IsInRole("FL Manager") || spc.Principal.IsInRole("FL Supervisor"))
                _view.FurtherLendingUser = true;
            else
                _view.FurtherLendingUser = false;

            _view.ButtonRowVisible = true;

            if (_view.AffordabilityAssessment != null)
            {
                if (_view.AffordabilityAssessmentMode == AffordabilityAssessmentMode.Update_Credit)
                {
                    // get a list of assessment stress factors & bind to dropdown list
                    _view.BindAssessmentStressFactors(applicationDomainService.GetAffordabilityAssessmentStressFactors(), _view.AffordabilityAssessment.StressFactorKey);
                }
                _view.BindAffordabilityAssessmentDetail();
                PopulateSAHLBondAndHOC(_view.AffordabilityAssessment.GenericKey);
            }
        }

        private void PopulateSAHLBondAndHOC(int applicationKey)
        {
            _view.SAHLBondValueRetrieved = false;
            GetBondInstalmentForApplicationQuery getBondInstalmentForApplication = new GetBondInstalmentForApplicationQuery(applicationKey);
            systemMessageCollection = applicationDomainService.PerformQuery(getBondInstalmentForApplication);
            if (!systemMessageCollection.HasErrors)
            {
                int? sahlBond = null;
                if (getBondInstalmentForApplication.Result != null && getBondInstalmentForApplication.Result.Results != null && getBondInstalmentForApplication.Result.Results.Count() > 0)
                    sahlBond = Convert.ToInt32(getBondInstalmentForApplication.Result.Results.FirstOrDefault());

                if (sahlBond.HasValue && sahlBond.Value > 0)
                {
                    _view.SAHLBond_Client = _view.SAHLBond_Credit = _view.SAHLBond_ToBe = sahlBond.Value;

                    _view.SAHLBondValueRetrieved = true;
                }
            }

            _view.SAHLHocValueRetrieved = false;
            GetHocInstalmentForApplicationQuery getHocInstalmentForApplicationQuery = new GetHocInstalmentForApplicationQuery(applicationKey);
            systemMessageCollection = applicationDomainService.PerformQuery(getHocInstalmentForApplicationQuery);
            if (!systemMessageCollection.HasErrors)
            {
                int? sahlHOC = null;
                if (getHocInstalmentForApplicationQuery.Result != null && getHocInstalmentForApplicationQuery.Result.Results != null && getHocInstalmentForApplicationQuery.Result.Results.Count() > 0)
                    sahlHOC = Convert.ToInt32(getHocInstalmentForApplicationQuery.Result.Results.FirstOrDefault());

                if (sahlHOC.HasValue && sahlHOC.Value > 0)
                {
                    _view.HOC_Client = _view.HOC_Credit = _view.HOC_ToBe = sahlHOC.Value;

                    _view.SAHLHocValueRetrieved = true;
                }
            }
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        private void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            ValidateScreenInput();

            if (_view.IsValid)
            {
                decimal minimumMonthlyFixedExpenses = DecisionTreeRepo.DetermineNCRGuidelineMinMonthlyFixedExpenses(_view.GrossMonthlyIncome_Client.HasValue ? _view.GrossMonthlyIncome_Client.Value : 0);

                TransactionScope txn = new TransactionScope();

                try
                {
                    AffordabilityAssessmentDetailModel affordabilityAssessmentDetail = _view.AffordabilityAssessment.AffordabilityAssessmentDetail;

                    // populate affordability assessment detail
                    affordabilityAssessmentDetail.MinimumMonthlyFixedExpenses = Convert.ToInt32(minimumMonthlyFixedExpenses);
                    _view.AffordabilityAssessment.StressFactorKey = _view.AssessmentStressFactorKey;

                    // populate client fields
                    affordabilityAssessmentDetail.Income.BasicGrossSalary_Drawings.ClientValue = _view.BasicGrossSalary_Drawings_Client;
                    affordabilityAssessmentDetail.Income.Commission_Overtime.ClientValue = _view.Commission_Overtime_Client;
                    affordabilityAssessmentDetail.Income.Net_Rental.ClientValue = _view.Net_Rental_Client;
                    affordabilityAssessmentDetail.Income.Investments.ClientValue = _view.Investments_Client;
                    affordabilityAssessmentDetail.Income.OtherIncome1.ClientValue = _view.OtherIncome1_Client;
                    affordabilityAssessmentDetail.Income.OtherIncome2.ClientValue = _view.OtherIncome2_Client;

                    affordabilityAssessmentDetail.IncomeDeductions.PayrollDeductions.ClientValue = _view.PayrollDeductions_Client;

                    affordabilityAssessmentDetail.NecessaryExpenses.AccommodationExpenses_Rental.ClientValue = _view.Accomodation_Client;
                    affordabilityAssessmentDetail.NecessaryExpenses.Transport.ClientValue = _view.Transport_Client;
                    affordabilityAssessmentDetail.NecessaryExpenses.Food.ClientValue = _view.Food_Client;
                    affordabilityAssessmentDetail.NecessaryExpenses.Education.ClientValue = _view.Education_Client;
                    affordabilityAssessmentDetail.NecessaryExpenses.Medical.ClientValue = _view.Medical_Client;
                    affordabilityAssessmentDetail.NecessaryExpenses.Utilities.ClientValue = _view.Utilities_Client;
                    affordabilityAssessmentDetail.NecessaryExpenses.ChildSupport.ClientValue = _view.ChildSupport_Client;

                    affordabilityAssessmentDetail.PaymentObligations.OtherBonds.ClientValue = _view.OtherBonds_Client;
                    affordabilityAssessmentDetail.PaymentObligations.Vehicle.ClientValue = _view.Vehicle_Client;
                    affordabilityAssessmentDetail.PaymentObligations.CreditCards.ClientValue = _view.CreditCards_Client;
                    affordabilityAssessmentDetail.PaymentObligations.PersonalLoans.ClientValue = _view.PersonalLoans_Client;
                    affordabilityAssessmentDetail.PaymentObligations.RetailAccounts.ClientValue = _view.RetailAccounts_Client;
                    affordabilityAssessmentDetail.PaymentObligations.OtherDebtExpenses.ClientValue = _view.OtherDebtExpenses_Client;

                    affordabilityAssessmentDetail.SAHLPaymentObligations.SAHLBond.ClientValue = _view.SAHLBond_Client;
                    affordabilityAssessmentDetail.SAHLPaymentObligations.HOC.ClientValue = _view.HOC_Client;

                    affordabilityAssessmentDetail.OtherExpenses.DomesticSalary.ClientValue = _view.DomesticSalary_Client;
                    affordabilityAssessmentDetail.OtherExpenses.InsurancePolicies.ClientValue = _view.InsurancePolicies_Client;
                    affordabilityAssessmentDetail.OtherExpenses.CommittedSavings.ClientValue = _view.CommittedSavings_Client;
                    affordabilityAssessmentDetail.OtherExpenses.Security.ClientValue = _view.Security_Client;
                    affordabilityAssessmentDetail.OtherExpenses.Telephone_TV.ClientValue = _view.TelephoneTV_Client;
                    affordabilityAssessmentDetail.OtherExpenses.Other.ClientValue = _view.Other_Client;

                    // populate credit fields
                    affordabilityAssessmentDetail.Income.BasicGrossSalary_Drawings.CreditValue = _view.BasicGrossSalary_Drawings_Credit;
                    affordabilityAssessmentDetail.Income.Commission_Overtime.CreditValue = _view.Commission_Overtime_Credit;
                    affordabilityAssessmentDetail.Income.Net_Rental.CreditValue = _view.Net_Rental_Credit;
                    affordabilityAssessmentDetail.Income.Investments.CreditValue = _view.Investments_Credit;
                    affordabilityAssessmentDetail.Income.OtherIncome1.CreditValue = _view.OtherIncome1_Credit;
                    affordabilityAssessmentDetail.Income.OtherIncome2.CreditValue = _view.OtherIncome2_Credit;

                    affordabilityAssessmentDetail.IncomeDeductions.PayrollDeductions.CreditValue = _view.PayrollDeductions_Credit;

                    affordabilityAssessmentDetail.NecessaryExpenses.AccommodationExpenses_Rental.CreditValue = _view.Accomodation_Credit;
                    affordabilityAssessmentDetail.NecessaryExpenses.Transport.CreditValue = _view.Transport_Credit;
                    affordabilityAssessmentDetail.NecessaryExpenses.Food.CreditValue = _view.Food_Credit;
                    affordabilityAssessmentDetail.NecessaryExpenses.Education.CreditValue = _view.Education_Credit;
                    affordabilityAssessmentDetail.NecessaryExpenses.Medical.CreditValue = _view.Medical_Credit;
                    affordabilityAssessmentDetail.NecessaryExpenses.Utilities.CreditValue = _view.Utilities_Credit;
                    affordabilityAssessmentDetail.NecessaryExpenses.ChildSupport.CreditValue = _view.ChildSupport_Credit;

                    affordabilityAssessmentDetail.PaymentObligations.OtherBonds.CreditValue = _view.OtherBonds_Credit;
                    affordabilityAssessmentDetail.PaymentObligations.Vehicle.CreditValue = _view.Vehicle_Credit;
                    affordabilityAssessmentDetail.PaymentObligations.CreditCards.CreditValue = _view.CreditCards_Credit;
                    affordabilityAssessmentDetail.PaymentObligations.PersonalLoans.CreditValue = _view.PersonalLoans_Credit;
                    affordabilityAssessmentDetail.PaymentObligations.RetailAccounts.CreditValue = _view.RetailAccounts_Credit;
                    affordabilityAssessmentDetail.PaymentObligations.OtherDebtExpenses.CreditValue = _view.OtherDebtExpenses_Credit;

                    affordabilityAssessmentDetail.SAHLPaymentObligations.SAHLBond.CreditValue = _view.SAHLBond_Credit;
                    affordabilityAssessmentDetail.SAHLPaymentObligations.HOC.CreditValue = _view.HOC_Credit;

                    affordabilityAssessmentDetail.OtherExpenses.DomesticSalary.CreditValue = _view.DomesticSalary_Credit;
                    affordabilityAssessmentDetail.OtherExpenses.InsurancePolicies.CreditValue = _view.InsurancePolicies_Credit;
                    affordabilityAssessmentDetail.OtherExpenses.CommittedSavings.CreditValue = _view.CommittedSavings_Credit;
                    affordabilityAssessmentDetail.OtherExpenses.Security.CreditValue = _view.Security_Credit;
                    affordabilityAssessmentDetail.OtherExpenses.Telephone_TV.CreditValue = _view.TelephoneTV_Credit;
                    affordabilityAssessmentDetail.OtherExpenses.Other.CreditValue = _view.Other_Credit;

                    // populate debt to consolidate fields
                    affordabilityAssessmentDetail.PaymentObligations.OtherBonds.ConsolidationValue = _view.OtherBonds_Consolidate;
                    affordabilityAssessmentDetail.PaymentObligations.Vehicle.ConsolidationValue = _view.Vehicle_Consolidate;
                    affordabilityAssessmentDetail.PaymentObligations.CreditCards.ConsolidationValue = _view.CreditCards_Consolidate;
                    affordabilityAssessmentDetail.PaymentObligations.PersonalLoans.ConsolidationValue = _view.PersonalLoans_Consolidate;
                    affordabilityAssessmentDetail.PaymentObligations.RetailAccounts.ConsolidationValue = _view.RetailAccounts_Consolidate;
                    affordabilityAssessmentDetail.PaymentObligations.OtherDebtExpenses.ConsolidationValue = _view.OtherDebtExpenses_Consolidate;

                    // populate comments
                    affordabilityAssessmentDetail.Income.BasicGrossSalary_Drawings.ItemNotes = _view.BasicGrossSalary_Drawings_Comments;
                    affordabilityAssessmentDetail.Income.Commission_Overtime.ItemNotes = _view.Commission_Overtime_Comments;
                    affordabilityAssessmentDetail.Income.Net_Rental.ItemNotes = _view.Net_Rental_Comments;
                    affordabilityAssessmentDetail.Income.Investments.ItemNotes = _view.Investments_Comments;
                    affordabilityAssessmentDetail.Income.OtherIncome1.ItemNotes = _view.OtherIncome1_Comments;
                    affordabilityAssessmentDetail.Income.OtherIncome2.ItemNotes = _view.OtherIncome2_Comments;

                    affordabilityAssessmentDetail.IncomeDeductions.PayrollDeductions.ItemNotes = _view.PayrollDeductions_Comments;

                    affordabilityAssessmentDetail.NecessaryExpenses.AccommodationExpenses_Rental.ItemNotes = _view.Accomodation_Comments;
                    affordabilityAssessmentDetail.NecessaryExpenses.Transport.ItemNotes = _view.Transport_Comments;
                    affordabilityAssessmentDetail.NecessaryExpenses.Food.ItemNotes = _view.Food_Comments;
                    affordabilityAssessmentDetail.NecessaryExpenses.Education.ItemNotes = _view.Education_Comments;
                    affordabilityAssessmentDetail.NecessaryExpenses.Medical.ItemNotes = _view.Medical_Comments;
                    affordabilityAssessmentDetail.NecessaryExpenses.Utilities.ItemNotes = _view.Utilities_Comments;
                    affordabilityAssessmentDetail.NecessaryExpenses.ChildSupport.ItemNotes = _view.ChildSupport_Comments;

                    affordabilityAssessmentDetail.PaymentObligations.OtherBonds.ItemNotes = _view.OtherBonds_Comments;
                    affordabilityAssessmentDetail.PaymentObligations.Vehicle.ItemNotes = _view.Vehicle_Comments;
                    affordabilityAssessmentDetail.PaymentObligations.CreditCards.ItemNotes = _view.CreditCards_Comments;
                    affordabilityAssessmentDetail.PaymentObligations.PersonalLoans.ItemNotes = _view.PersonalLoans_Comments;
                    affordabilityAssessmentDetail.PaymentObligations.RetailAccounts.ItemNotes = _view.RetailAccounts_Comments;
                    affordabilityAssessmentDetail.PaymentObligations.OtherDebtExpenses.ItemNotes = _view.OtherDebtExpenses_Comments;

                    affordabilityAssessmentDetail.SAHLPaymentObligations.SAHLBond.ItemNotes = _view.SAHLBond_Comments;
                    affordabilityAssessmentDetail.SAHLPaymentObligations.HOC.ItemNotes = _view.HOC_Comments;

                    affordabilityAssessmentDetail.OtherExpenses.DomesticSalary.ItemNotes = _view.DomesticSalary_Comments;
                    affordabilityAssessmentDetail.OtherExpenses.InsurancePolicies.ItemNotes = _view.InsurancePolicies_Comments;
                    affordabilityAssessmentDetail.OtherExpenses.CommittedSavings.ItemNotes = _view.CommittedSavings_Comments;
                    affordabilityAssessmentDetail.OtherExpenses.Security.ItemNotes = _view.Security_Comments;
                    affordabilityAssessmentDetail.OtherExpenses.Telephone_TV.ItemNotes = _view.TelephoneTV_Comments;
                    affordabilityAssessmentDetail.OtherExpenses.Other.ItemNotes = _view.Other_Comments;

                    // save the affordability assessment
                    applicationDomainService.AmendAffordabilityAssessment(_view.AffordabilityAssessment);

                    txn.VoteCommit();
                    CBOManager.RefreshInstanceNode(_view.CurrentPrincipal);
                    _view.Navigator.Navigate("Submit");
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
                }
            }
        }

        private void ValidateScreenInput()
        {
            string message = "";

            // validate the at all the required comments have been entered
            if (_view.CommentsValid == false)
            {
                message = "Comments must be entered where Client values have changed.</br><b>(see 'Red' icons)</b>.";
                _view.Messages.Add(new Error(message, message));
            }

            // warn user if they are saving a confirmed assessment that a copy will be made.
            if (_view.AffordabilityAssessment.AffordabilityAssessmentStatus == AffordabilityAssessmentStatus.Confirmed)
            {
                message = "Confirmed Assessments cannot be updated. If you continue, a new Unconfirmed copy will be made and this Assessment will be Archived.";
                _view.Messages.Add(new Warning(message, message));
            }
        }
    }
}