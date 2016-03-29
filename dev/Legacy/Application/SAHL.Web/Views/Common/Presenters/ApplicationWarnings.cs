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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using System.Web.UI.MobileControls;
using System.Collections.Generic;
using SAHL.Common.Globals;

using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Helpers;

namespace SAHL.Web.Views.Common.Presenters
{
    public class ApplicationWarnings : SAHLCommonBasePresenter<IApplicationWarnings>
    {
        private CBONode _node;
        private IApplication _application;

        public ApplicationWarnings(IApplicationWarnings view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal);            
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;
            //Get the application 
            IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
            _application = AR.GetApplicationByKey(int.Parse(_node.GenericKey.ToString()));

            IRuleService RS = ServiceFactory.GetService<IRuleService>();

            #region LegalEntity Rules
            // Execute the LegalEntity rules
            List<string> lstLegalEntityRules = new List<string>();

            if (_application.Account != null)
            {
                lstLegalEntityRules.Add("MortgageLoanAccountLegalEntityRoleMainApplicant");
            }
            lstLegalEntityRules.Add("LegalEntityApplicantNaturalPersonDeclarations");
            lstLegalEntityRules.Add("MortgageLoanAccountLegalEntityRoleCompanyMainApplicant");
            lstLegalEntityRules.Add("MortgageLoanAccountLegalEntityRoleCompanyMainApplicantQuantity");
            lstLegalEntityRules.Add("MortgageLoanAccountLegalEntityRoleCompanyMainApplicantNaturalPersonSuretor");
            lstLegalEntityRules.Add("LegalEntityNaturalPersonCOP");
            lstLegalEntityRules.Add("HouseholdIncomeContributorMinimum");
            //lstLegalEntityRules.Add("LegalEntityCompanyTradingName");
            lstLegalEntityRules.Add("ITCApplication");


            List<IApplicationRole> lstRoles = new List<IApplicationRole>();
            
            foreach (IApplicationRole r in _application.ApplicationRoles)
            {
                if (r.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)SAHL.Common.Globals.OfferRoleTypeGroups.Client)
                {
                    lstRoles.Add(r);
                    break;
                }
            }
            
            for (int x = 0; x < lstRoles.Count; x++)
            {
                ILegalEntityNaturalPerson le = lstRoles[x].LegalEntity as ILegalEntityNaturalPerson;

                if (le != null)
                {
                    RS.ExecuteRule(_view.Messages, "LegalEntityNaturalPersonMinAge", le);
                }

                RS.ExecuteRuleSet(_view.Messages, lstLegalEntityRules,_application);
                //RS.ExecuteRule(_view.Messages,"LegalEntityNaturalPersonIDNumberDOB",le);                

            }         

            List<string> lstLegalEntityMessages = new List<string>();
            for (int x = 0; x < _view.Messages.ErrorMessages.Count; x++)
            {
                lstLegalEntityMessages.Add(_view.Messages.ErrorMessages[x].Message);
            }
            for (int x = 0; x < _view.Messages.WarningMessages.Count; x++)
            {
                lstLegalEntityMessages.Add("WARNING: " + _view.Messages.WarningMessages[x].Message);
            }
        
            int errorCount = _view.Messages.ErrorMessages.Count;
            int warningsCount = _view.Messages.WarningMessages.Count;

            //This rule set needs the legal entity and logged in user's primary origination source
            for (int x = 0; x < lstRoles.Count; x++)
            {
                ILegalEntity le = lstRoles[x].LegalEntity;

                RS.ExecuteRule(_view.Messages, "LegalEntityOriginationSource", le, OriginationSourceHelper.PrimaryOriginationSourceKey(_view.CurrentPrincipal));

                if (_view.Messages.ErrorMessages.Count > errorCount)
                {
                    for (int y = errorCount; y < _view.Messages.ErrorMessages.Count; y++)
                    {
                        lstLegalEntityMessages.Add(_view.Messages.ErrorMessages[y].Message);
                    }
                }
                if (_view.Messages.WarningMessages.Count > warningsCount)
                {
                    for (int y = warningsCount; y < _view.Messages.WarningMessages.Count; y++)
                    {
                        lstLegalEntityMessages.Add("WARNING: " + _view.Messages.WarningMessages[y].Message);
                    }
                }
           }

            _view.PopulateLegalEntityErrors(lstLegalEntityMessages);

            #endregion

            #region Application/Offer Rules

            //Execute the Application/Offer Rules

            errorCount = _view.Messages.ErrorMessages.Count;
            warningsCount = _view.Messages.WarningMessages.Count;
 

            List<string> lstAppOfferRules = new List<string>();

            //Add the minimum rules"
            lstAppOfferRules.Add("ApplicationMailingAddress");
            lstAppOfferRules.Add("ApplicationConditionMandatory");
            lstAppOfferRules.Add("ApplicationProperty");
            lstAppOfferRules.Add("ApplicationCheckMinLoanAmount");
            lstAppOfferRules.Add("ApplicationCheckMinIncome");
   

            if (_application.Account != null)
            {
                if (_application.Account.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Open
                    || _application.Account.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Dormant)
                {
                    lstAppOfferRules.Add("ProductVarifixOptInLoanTransaction");
                    lstAppOfferRules.Add("ProductInterestOnlyOptInLoanTransaction");
                    lstAppOfferRules.Add("ProductSuperLoOptInLoanTransaction");
                }
            }

            //Add the maximum rules if gone through stageDefinitionStageDefinitionGroupKey 2153
            IStageDefinitionRepository SDR = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            List<string> lstAppOfferMessages = new List<string>();

            int cnt = SDR.CountCompositeStageOccurance(_application.Key, SDR.GetStageDefinitionStageDefinitionGroup((int)StageDefinitionGroups.ApplicationCapture, (int)StageDefinitions.ApplicationCaptureSubmitted).Key);
            int messageIndexToReplace = -1;
            if (cnt > 0)
            {
                lstAppOfferRules.Add("ApplicationDebitOrderBankAccount");
                lstAppOfferRules.Add("ValuationApplication");
                lstAppOfferRules.Add("HouseholdIncomeAtLeastOne");
                lstAppOfferRules.Add("LegalEntityNaturalPersonHouseholdContributorConfirmedIncome");
                double origIncome = _application.GetHouseHoldIncome();
                _application.CalculateHouseHoldIncome();
                double confirmedIncome = _application.DetermineConfirmedHouseHoldIncome();
                if (confirmedIncome != origIncome)
                {
                    if (_application.ApplicationInformations.Count > 0)
                    {
                        IApplicationInformationVariableLoan vlInfo = AR.GetApplicationInformationVariableLoan(_application.GetLatestApplicationInformation().Key);

                        lstAppOfferMessages.Add("WARNING: Some active employment records have not been confirmed!");
                        double PTIPercent = Math.Round((vlInfo.MonthlyInstalment.Value / confirmedIncome) * 100, 2);

                        if (PTIPercent.ToString().Contains("Infinity"))
                        {
                            lstAppOfferMessages.Add("Confirmed Income PTI = 0%");
                        }
                        else
                        {
                            lstAppOfferMessages.Add("Confirmed Income PTI = " + PTIPercent.ToString() + "%");
                        }
                    }
                }
                lstAppOfferMessages.Add("Currently, the confirmed income on the application is = R" + Math.Round(confirmedIncome, 2).ToString());
                lstAppOfferRules.Add("CreditDisqualificationMaxPTI");
                lstAppOfferRules.Add("CreditDisqualificationMaxLTV");
                lstAppOfferRules.Add("CreditDisqualificationMinIncome");
                lstAppOfferRules.Add("CreditDisqualificationMinValuation");
                lstAppOfferRules.Add("CreditDisqualificationMaxLoanAgreeAmount");

                lstAppOfferRules.Add("CancellationBankDetailsSwitch");
                lstAppOfferRules.Add("SellersDetailsMandatoryNewPurchase");
                lstAppOfferRules.Add("AffordabilityStatementMandatory");

                errorCount = _view.Messages.ErrorMessages.Count;
  
                RS.ExecuteRule(_view.Messages,"ApplicationHOCExists",_application);
               
                if (_view.Messages.ErrorMessages.Count > errorCount)
                {
                    messageIndexToReplace = _view.Messages.ErrorMessages.Count - 1;
                }
            }

            warningsCount = _view.Messages.WarningMessages.Count;

            RS.ExecuteRuleSet(_view.Messages, lstAppOfferRules, _application);

            if (_view.Messages.ErrorMessages.Count > errorCount)
            {
                for (int x = errorCount; x < _view.Messages.ErrorMessages.Count; x++)
                {
                    if (x != messageIndexToReplace)
                    {
                        lstAppOfferMessages.Add(_view.Messages.ErrorMessages[x].Message);
                    }
                    else
                    {
                        lstAppOfferMessages.Add("WARNING: Application must have an HOC Account before submission to credit.");
                    }

                }
            }

            if (_view.Messages.WarningMessages.Count > warningsCount)
            {
                for (int x = warningsCount; x < _view.Messages.WarningMessages.Count; x++)
                {
                    lstAppOfferMessages.Add("WARNING: " + _view.Messages.WarningMessages[x].Message);
                }
            }



            errorCount = _view.Messages.ErrorMessages.Count;
            warningsCount = _view.Messages.WarningMessages.Count;

            lstAppOfferRules.Clear();


            IApplicationFurtherLending flApp = _application as IApplicationFurtherLending;
            if (flApp != null)
            {
                lstAppOfferRules.Add("AccountDebtCounseling");
                lstAppOfferRules.Add("LegalEntitiesUnderDebtCounsellingForAccount");
                lstAppOfferRules.Add("ApplicationFurtherLendingAccountCancellation");
                lstAppOfferRules.Add("ApplicationFurtherLendingAccountForeClosure");

                RS.ExecuteRuleSet(_view.Messages, lstAppOfferRules, flApp);


                if (_view.Messages.ErrorMessages.Count > errorCount)
                {
                    for (int x = errorCount + 1; x < _view.Messages.ErrorMessages.Count; x++)
                    {
                        lstAppOfferMessages.Add(_view.Messages.ErrorMessages[x].Message);
                    }
                }

                if (_view.Messages.WarningMessages.Count > warningsCount)
                {
                    for (int x = warningsCount + 1; x < _view.Messages.WarningMessages.Count; x++)
                    {
                        lstAppOfferMessages.Add("WARNING: " + _view.Messages.WarningMessages[x].Message);
                    }
                }

            }

            _view.PopulateApplicationOfferRules(lstAppOfferMessages);


            #endregion

          
            #region Credit Rules

            errorCount = _view.Messages.ErrorMessages.Count;
            warningsCount = _view.Messages.WarningMessages.Count;
 
            IApplicationMortgageLoan ml = _application as IApplicationMortgageLoan;
            if (ml != null)
            {
                List<string> lstCreditRules = new List<string>();
                lstCreditRules.Add("CreditMatrixInvestmentPropertySecondary");
                lstCreditRules.Add("CreditMatrixRefinanceLoans");
                lstCreditRules.Add("CreditMatrixRiskCategory5");                
                //lstCreditRules.Add("QualifyPTI");
                //lstCreditRules.Add("QualifyMinIncome");
                //lstCreditRules.Add("QualifyLoanAmount");

                RS.ExecuteRuleSet(_view.Messages, lstCreditRules, ml);

                List<string> lstCreditMessages = new List<string>();

                if (_view.Messages.ErrorMessages.Count > errorCount)
                {
                    for (int x = errorCount; x < _view.Messages.ErrorMessages.Count; x++)
                    {
                        lstCreditMessages.Add(_view.Messages.ErrorMessages[x].Message);
                    }
                }

                if (_view.Messages.WarningMessages.Count > warningsCount)
                {
                    for (int x = warningsCount; x < _view.Messages.WarningMessages.Count; x++)
                    {
                        lstCreditMessages.Add("WARNING: " + _view.Messages.WarningMessages[x].Message);
                    }
                }
               

                _view.PopulateCreditMatrixRules(lstCreditMessages);

            }
            #endregion


            IAccountRepository AccRep = RepositoryFactory.GetRepository<IAccountRepository>();
            if (_application.Account != null)
            {
                IReadOnlyEventList<IDetail> lstDetails = AccRep.GetAccountDetailForFurtherLending(_application.Account.Key);
                if (lstDetails.Count > 0)
                {
                    _view.ShowDetailTypes = true;
                    List<string> details = new List<string>();
                    for (int x = 0; x < lstDetails.Count; x++)
                    {
                        details.Add(lstDetails[x].DetailType.Key + " - " + lstDetails[x].DetailType.Description);
                    }
                    _view.PopulateDetailTypes(details);
                }
            }
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

        }
    }
}
