using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Text;
using System.Linq;

namespace SAHL.Web.Views.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ApplicationSummary : SAHLCommonBaseView, IApplicationSummary
    {
        private IApplication _application;
        private bool _showCancelButton;
        private bool _showHistoryButton;
        private bool _showCapitecDetails;
        private bool _showComcorpDetails;
        private bool _showIsGEPFDetails;
        private bool _showStopOrderDiscountEligibility;
        private IReasonRepository _reasonRepo;
        private IStageDefinitionRepository _stageRepo;
        private IApplicationRepository _appRepo;
        private IOrganisationStructureRepository _orgRepo;
        private IEstateAgentRepository _estateRepo;
        private IMemoRepository _memoRepo;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!ShouldRunPage)
                return;

            BindGrid();
            BindControls();

            btnCancel.Visible = _showCancelButton;
            btnTransitionHistory.Visible = _showHistoryButton;

            lblCapitecBranchLabel.Visible = _showCapitecDetails;
            lblCapitecBranch.Visible = _showCapitecDetails;
            lblCapitecConsultantLabel.Visible = _showCapitecDetails;
            lblCapitecConsultant.Visible = _showCapitecDetails;

            lblComcorpVendorLabel.Visible = _showComcorpDetails;
            lblComcorpVendor.Visible = _showComcorpDetails;

            lblIsGEPFLabel.Visible = _showIsGEPFDetails;
            lblIsGEPF.Visible = _showIsGEPFDetails;

            lblStopOrderDiscountEligibilityLabel.Visible = lblStopOrderDiscountEligibility.Visible = _showStopOrderDiscountEligibility;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        /// <summary>
        /// Navigate to the transition history
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTransitionHistory_Click(object sender, EventArgs e)
        {
            OnTransitionHistoryClicked(sender, e);
        }

        private void BindGrid()
        {
            AddTrace(this, "BindGrid_Start");
            if (_application == null)
                return;

            List<GridRowItem> lstGridItems = new List<GridRowItem>();

            switch (_application.ApplicationType.Key)
            {
                case (int)SAHL.Common.Globals.OfferTypes.NewPurchaseLoan:
                case (int)SAHL.Common.Globals.OfferTypes.ReAdvance:
                case (int)SAHL.Common.Globals.OfferTypes.RefinanceLoan:
                case (int)SAHL.Common.Globals.OfferTypes.SwitchLoan:
                case (int)SAHL.Common.Globals.OfferTypes.Unknown:
                    IReadOnlyEventList<IApplicationRole> roles = _application.GetApplicationRolesByGroup(OfferRoleTypeGroups.Client);
                    AddTrace(this, "BindGrid_Start_A");
                    foreach (IApplicationRole r in roles)
                    {
                        GridRowItem itm = new GridRowItem();
                        itm.ApplicationKey = _application.Key.ToString();
                        itm.LegalEntityName = r.LegalEntity.DisplayName;
                        ILegalEntityNaturalPerson leNP = r.LegalEntity as ILegalEntityNaturalPerson;
                        if (leNP != null)
                        {
                            itm.IDCompanyNo = leNP.IDNumber == null ? (leNP.PassportNumber == null ? "" : leNP.PassportNumber) : leNP.IDNumber;
                            itm.MaritalStatus = leNP.MaritalStatus != null ? leNP.MaritalStatus.Description : "";
                        }
                        else
                        {
                            ILegalEntityGenericCompany leC = r.LegalEntity as ILegalEntityGenericCompany;
                            itm.IDCompanyNo = leC.RegistrationNumber;
                            itm.MaritalStatus = "N/A";
                        }
                        itm.Role = r.ApplicationRoleType.Description;
                        bool foundIncomeContributor = false;
                        foreach (IApplicationRoleAttribute attribute in r.ApplicationRoleAttributes)
                        {
                            if (attribute.OfferRoleAttributeType.Key == (int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor)
                            {
                                foundIncomeContributor = true;
                                break;
                            }
                        }
                        AddTrace(this, "BindGrid_Start_B");
                        itm.IncomeContributor = foundIncomeContributor ? "Yes" : "No";

                        ILegalEntityRepository LER = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                        IEmploymentType employType = LER.GetLegalEntityEmploymentTypeForApplication(r.LegalEntity, _application);
                        itm.EmploymentType = employType != null ? employType.Description : "-";

                        double employIncome = LER.GetLegalEntityIncomeForApplication(r.LegalEntity, _application);

                        itm.Income = employIncome.ToString(SAHL.Common.Constants.CurrencyFormat);

                        if (r.LegalEntity.Employment != null && r.LegalEntity.Employment.Count > 0)
                        {
                            IEmployment employment = r.LegalEntity.Employment.Where(x => x.EmploymentStatus.Key == (int)SAHL.Common.Globals.EmploymentStatuses.Current).FirstOrDefault();
                            if (employment != null)
                            {
                                if (employment.UnionMember == null)
                                    itm.UnionMember = "Unknown";
                                else if (employment.UnionMember == true)
                                    itm.UnionMember = "Yes";
                                else
                                    itm.UnionMember = "No";
                            }
                            else { itm.UnionMember = "Unknown"; }
                        }
                        else { itm.UnionMember = "Unknown"; }

                        lstGridItems.Add(itm);
                    }
                    break;
                default:
                    break;
            }

            gridSummary.AutoGenerateColumns = false;
            gridSummary.AddGridBoundColumn("ApplicationKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            gridSummary.AddGridBoundColumn("LegalEntityName", "Legal Entity Name", Unit.Percentage(21), HorizontalAlign.Left, true);
            gridSummary.AddGridBoundColumn("IDCompanyNo", "ID/Company No", Unit.Percentage(12), HorizontalAlign.Left, true);
            gridSummary.AddGridBoundColumn("Role", "Role", Unit.Percentage(15), HorizontalAlign.Left, true);
            gridSummary.AddGridBoundColumn("MaritalStatus", "Marital Status", Unit.Percentage(21), HorizontalAlign.Left, true);
            gridSummary.AddGridBoundColumn("IncomeContributor", "Income Cont", Unit.Percentage(10), HorizontalAlign.Left, true);
            gridSummary.AddGridBoundColumn("Income", "Income", Unit.Percentage(10), HorizontalAlign.Left, true);
            gridSummary.AddGridBoundColumn("EmploymentType", "Employ Type", Unit.Percentage(18), HorizontalAlign.Left, true);
            gridSummary.AddGridBoundColumn("UnionMember", "Union Member", Unit.Percentage(10), HorizontalAlign.Left, true);
            gridSummary.DataSource = lstGridItems;
            gridSummary.DataBind();

            AddTrace(this, "BindGrid_End");
        }

        #region IApplicationSummary Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnTransitionHistoryClicked;

        /// <summary>
        /// 
        /// </summary>
        public bool ShowCancelButton
        {
            set { _showCancelButton = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowHistoryButton
        {
            set { _showHistoryButton = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowCapitecDetails
        {
            set { _showCapitecDetails = value; }
        }

        public bool ShowComcorpDetails
        {
            set { _showComcorpDetails = value; }
        }

        public bool ShowIsGEPFDetails
        {
            set { _showIsGEPFDetails = value; }
        }

        public bool ShowStopOrderDiscountEligibility
        {
            set { _showStopOrderDiscountEligibility = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public IApplication application
        {
            set { _application = value; }
        }

        protected void gridSummary_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        public void BindControls()
        {
            AddTrace(this, "BindControls_Start");

            if (_application == null)
                return;

            if (_application.ApplicationInformations.Count < 1 || _application.CurrentProduct == null)
            {
                if (_application.ApplicationInformations.Count < 1)
                {
                    lblApplicationNumber.Text = _application.Key.ToString();
                    lblApplicationType.Text = "LEAD";

                    // originating branch consultant
                    IApplicationRole operatorRole = _application.GetFirstApplicationRoleByType(OfferRoleTypes.BranchConsultantD);
                    lblOriginatingConsultant.Text = GetOperatorName(operatorRole, false, OrgRepo);
                    // current branch consultant
                    operatorRole = _application.GetLatestApplicationRoleByType(OfferRoleTypes.BranchConsultantD);
                    lblCurrentConsultant.Text = GetOperatorName(operatorRole, false, OrgRepo);
                    if (operatorRole != null)
                    {
                        // get branch for current consultant
                        IOrganisationStructure orgStruct = OrgRepo.GetBranchForConsultant(operatorRole);
                        lblBranch.Text = orgStruct != null ? (orgStruct.Parent != null ? orgStruct.Parent.Description : orgStruct.Description) : "-";
                    }

                    // Commissionable Consultant
                    IApplicationRole ccAppRole = _application.GetLatestApplicationRoleByType(OfferRoleTypes.CommissionableConsultant);
                    lblCurrentCommissionableConsultant.Text = ccAppRole != null ? GetOperatorName(ccAppRole, false, OrgRepo) : "-";
                }
                return;
            }

            bool lifeExists = false, hocExists = false, quickcashExists = false, ntuReasonsExist = false, creditReasonsExist = false, declineReasonsExist = false, estateAgentExist = false;
            lblApplicationType.Text = _application.ApplicationType.Description;
            switch (_application.ApplicationType.Key)
            {
                //Further Lending
                case (int)SAHL.Common.Globals.OfferTypes.ReAdvance:
                case (int)SAHL.Common.Globals.OfferTypes.FurtherAdvance:
                case (int)SAHL.Common.Globals.OfferTypes.FurtherLoan:
                    break;

                case (int)SAHL.Common.Globals.OfferTypes.NewPurchaseLoan:
                case (int)SAHL.Common.Globals.OfferTypes.SwitchLoan:
                case (int)SAHL.Common.Globals.OfferTypes.RefinanceLoan:
                    ISupportsVariableLoanApplicationInformation loan = _application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                    if (loan != null)
                    {
                        IApplicationMortgageLoan ml = _application as IApplicationMortgageLoan;
                        if (ml != null && ml.ApplicantType != null)
                        {
                            lblApplicantType.Text = ml.ApplicantType.Description;
                        }

                        lblApplicationNumber.Text = _application.Key.ToString();
                        lblHouseholdIncome.Text = _application.GetHouseHoldIncome().ToString(SAHL.Common.Constants.CurrencyFormat);

                        IApplicationMortgageLoanNewPurchase mlNP = _application as IApplicationMortgageLoanNewPurchase;
                        if (mlNP != null)
                        {
                            lblPurchasePrice.Text = mlNP.PurchasePrice.HasValue ? mlNP.PurchasePrice.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                        }
                        else
                        {
                            lblPurchasePrice.Text = loan.VariableLoanInformation.ExistingLoan.HasValue ? loan.VariableLoanInformation.ExistingLoan.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                        }

                        lblCashDeposit.Text = loan.VariableLoanInformation != null && loan.VariableLoanInformation.CashDeposit != null && loan.VariableLoanInformation.CashDeposit.HasValue ? loan.VariableLoanInformation.CashDeposit.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";

                        IApplicationMortgageLoanRefinance refinance = _application as IApplicationMortgageLoanRefinance;
                        if (refinance != null)
                        {
                            lblCashDeposit.Text = refinance.CashOut.HasValue ? refinance.CashOut.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                        }

                        IApplicationMortgageLoanWithCashOut switchWithCashOut = _application as IApplicationMortgageLoanWithCashOut;
                        if (switchWithCashOut != null)
                        {
                            lblCashDeposit.Text = switchWithCashOut.RequestedCashAmount.HasValue ? switchWithCashOut.RequestedCashAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                        }

                        if (string.IsNullOrEmpty(lblCashDeposit.Text))
                            lblCashDeposit.Text = "R 0.00";

                        lblValuationAmount.Text = loan.VariableLoanInformation.PropertyValuation.HasValue ? loan.VariableLoanInformation.PropertyValuation.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";

                        lblSPV.Text = loan.VariableLoanInformation.SPV != null ? loan.VariableLoanInformation.SPV.Description : "-";

                        if (ml != null && ml.Property != null && ml.Property.OccupancyType != null)
                            lblOccupancyType.Text = ml.Property.OccupancyType.Description.ToString();

                        lblLoanAmount.Text = loan.VariableLoanInformation.LoanAgreementAmount.HasValue ? loan.VariableLoanInformation.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";

                        lblLTV.Text = loan.VariableLoanInformation.LTV.HasValue ? loan.VariableLoanInformation.LTV.Value.ToString(SAHL.Common.Constants.RateFormat): "-";
                        lblPTI.Text = loan.VariableLoanInformation.PTI.HasValue ? loan.VariableLoanInformation.PTI.Value.ToString(SAHL.Common.Constants.RateFormat): "-";

                        // originating branch consultant
                        IApplicationRole operatorRole = _application.GetFirstApplicationRoleByType(OfferRoleTypes.BranchConsultantD);
                        AddTrace(this, "GetOperatorName_Start");
                        lblOriginatingConsultant.Text = GetOperatorName(operatorRole, false, OrgRepo);
                        AddTrace(this, "GetOperatorName_End");
                        // current branch consultant
                        operatorRole = _application.GetLatestApplicationRoleByType(OfferRoleTypes.BranchConsultantD);
                        lblCurrentConsultant.Text = GetOperatorName(operatorRole, false, OrgRepo);

                        // Commissionable Consultant
                        IApplicationRole ccAppRole = _application.GetLatestApplicationRoleByType(OfferRoleTypes.CommissionableConsultant);
                        lblCurrentCommissionableConsultant.Text = ccAppRole != null ? GetOperatorName(ccAppRole, false, OrgRepo) : "-";

                        AddTrace(this, "BindControls_Start_A");
                        if (operatorRole != null)
                        {
                            // get branch for current consultant
                            IOrganisationStructure orgStruct = OrgRepo.GetBranchForConsultant(operatorRole);
                            lblBranch.Text = orgStruct != null ? (orgStruct.Parent != null ? orgStruct.Parent.Description : orgStruct.Description) : "-";
                        }
                        AddTrace(this, "BindControls_Start_B");

                        // Associated Branch Admin
                        operatorRole = _application.GetLatestApplicationRoleByType(OfferRoleTypes.BranchAdminD);
                        lblAssocBranchAdmin.Text = GetOperatorName(operatorRole, false, OrgRepo);

                        AddTrace(this, "BindControls_Start_C");
                        // New Business Processor
                        operatorRole = _application.GetLatestApplicationRoleByType(OfferRoleTypes.NewBusinessProcessorD);
                        lblNewBusinessProcessor.Text = GetOperatorName(operatorRole, false, OrgRepo);

                        AddTrace(this, "BindControls_Start_D");
                        
                        // employment type
                        lblEmploymentType.Text = loan.VariableLoanInformation.EmploymentType != null ? loan.VariableLoanInformation.EmploymentType.Description : "-";

                        if (loan.VariableLoanInformation != null)
                        {
                            if (loan.VariableLoanInformation.EmploymentType != null)
                            {
                                if (_application.HasAttribute(OfferAttributeTypes.ManuallySelectedEmploymentType))
                                {
                                    var confirmedEmploymentStageTransitions = StageRepo.GetStageTransitionList(_application.Key, (int)GenericKeyTypes.Offer, new List<int>() { (int)StageDefinitionStageDefinitionGroups.ApplicationEmploymentTypeCreditConfirmed, (int)StageDefinitionStageDefinitionGroups.ApplicationEmploymentTypeManageApplicationConfirmed });
                                    var latestConfrimedEmploymentStageTransition = confirmedEmploymentStageTransitions.OrderByDescending(x => x.TransitionDate).FirstOrDefault();

                                    lblEmploymentType.Text = lblEmploymentType.Text + " (" + latestConfrimedEmploymentStageTransition.ADUser.ADUserName + ")";
                                }
                                else
                                {
                                    lblEmploymentType.Text = lblEmploymentType.Text + " (System)";
                                }
                            }
                        }

                        // transferring attorney
                        lblTransferringAttorney.Text = String.IsNullOrEmpty(ml.TransferringAttorney) ? "-" : ml.TransferringAttorney;

                        // conveyencing attorney
                        operatorRole = _application.GetLatestApplicationRoleByType(OfferRoleTypes.ConveyanceAttorney);
                        lblConveyancingAttorney.Text = operatorRole != null ? operatorRole.LegalEntity.GetLegalName(LegalNameFormat.Full) : "-";

                        AddTrace(this, "BindControls_Start_E");
                        //Fees
                        lblFeesTotal.Text = (loan.VariableLoanInformation.FeesTotal.HasValue ? loan.VariableLoanInformation.FeesTotal.Value : 0).ToString(SAHL.Common.Constants.CurrencyFormat);

                        //Link Rate
                        lblLinkRate.Text = loan.VariableLoanInformation.RateConfiguration.Margin.Value.ToString(SAHL.Common.Constants.RateFormat);

                        //TODO: Discounted link rate


                        IApplicationProductMortgageLoan applicationProductMortgageLoan = _application.CurrentProduct as IApplicationProductMortgageLoan;
                        if (applicationProductMortgageLoan != null)
                        {
                            if (applicationProductMortgageLoan.ManualDiscount != null)
                            {
                                lblDiscount.Text = applicationProductMortgageLoan.ManualDiscount.Value.ToString(SAHL.Common.Constants.RateFormat);
                                lblDiscountedRate.Text = applicationProductMortgageLoan.DiscountedLinkRate.Value.ToString(SAHL.Common.Constants.RateFormat);
                            }
                            else
                            {
                                lblDiscount.Text = "-";
                                lblDiscountedRate.Text = "-";
                            }
                        }


                        //lblDiscountedRate.Text = 
                        #region quick cash information
                        ISupportsQuickCashApplicationInformation qcai = _application as ISupportsQuickCashApplicationInformation;
                        if (qcai != null && qcai.ApplicationInformationQuickCash != null)
                        {
                            IApplicationInformationQuickCash applicationInformationQuickCash = qcai.ApplicationInformationQuickCash;

                            IReadOnlyEventList<IReason> reasonList = ReasonRepo.GetReasonByGenericKeyAndReasonTypeKey(applicationInformationQuickCash.ApplicationInformation.Key, (int)ReasonTypes.QuickCashDecline);

                            gridQCDeclineReasons.BindData(reasonList);

                            if (reasonList != null && reasonList.Count > 0)
                            {
                                trQCDecline.Visible = true;
                                quickcashExists = true;
                            }
                            else
                            {
                                if (applicationInformationQuickCash.CreditApprovedAmount > 0 || applicationInformationQuickCash.CreditUpfrontApprovedAmount > 0)
                                {
                                    quickcashExists = true;
                                    lblCreditApprovedAmount.Text = applicationInformationQuickCash.CreditApprovedAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                                    lblUpfrontCreditApprovedAmount.Text = applicationInformationQuickCash.CreditUpfrontApprovedAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                                }
                            }
                        }
                        #endregion

                        #region related accounts (HOC & Life)
                        foreach (IAccount relatedAccount in _application.RelatedAccounts)
                        {
                            switch (relatedAccount.AccountType)
                            {
                                case SAHL.Common.Globals.AccountTypes.HOC:
                                    hocExists = true;
                                    IAccountHOC hoc = relatedAccount as IAccountHOC;
                                    if (hoc != null && hoc.HOC != null)
                                    {
                                        if (hoc.HOC.Ceded == true)
                                        {
                                            lblHOCCeded.Text = "Yes";
                                        }
                                        else
                                        {
                                            lblHOCCeded.Text = "No";
                                        }
                                    }
                                    else
                                    {
                                        lblHOCCeded.Text = "-";
                                    }
                                    //lblHOCCeded.Text = hoc != null && hoc.HOC != null  ? hoc.HOC.Ceded.ToString() : "-";
                                    lblHOCConstruction.Text = hoc != null && hoc.HOC != null && hoc.HOC.HOCConstruction != null ? hoc.HOC.HOCConstruction.Description : "-";
                                    lblHOCInsurer.Text = hoc != null && hoc.HOC != null && hoc.HOC.HOCInsurer != null ? hoc.HOC.HOCInsurer.Description : "-";
                                    lblHOCMonthlyPremium.Text = hoc != null && hoc.HOC != null && hoc.HOC.HOCMonthlyPremium.HasValue ? hoc.HOC.HOCMonthlyPremium.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "R0.00";
                                    lblHOCPolicyNumber.Text = hoc != null && hoc.HOC != null ? hoc.HOC.HOCPolicyNumber.ToString() : "-";
                                    lblHOCRoofType.Text = hoc != null && hoc.HOC != null && hoc.HOC.HOCRoof != null ? hoc.HOC.HOCRoof.Description : "-";
                                    lblHOCSubsidence.Text = hoc != null && hoc.HOC != null && hoc.HOC.HOCSubsidence != null ? hoc.HOC.HOCSubsidence.Description : "-";
                                    break;
                                case SAHL.Common.Globals.AccountTypes.Life:
                                    lifeExists = true;
                                    IAccountLifePolicy lifeAcc = relatedAccount as IAccountLifePolicy;

                                    if (lifeAcc.LifePolicy != null)
                                    {
                                        // get data from the Life Policy 
                                        lblLifePolicyNumber.Text = lifeAcc.Key.ToString();
                                        lblBroker.Text = lifeAcc.LifePolicy.Broker != null ? lifeAcc.LifePolicy.Broker.ADUserName : "-";
                                        lblLifeConsultant.Text = lifeAcc.LifePolicy.Consultant;
                                        lblLifeDateLastUpdated.Text = lifeAcc.LifePolicy.DateLastUpdated.HasValue ? lifeAcc.LifePolicy.DateLastUpdated.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
                                        lblLifeInsurer.Text = lifeAcc.LifePolicy.Insurer != null ? lifeAcc.LifePolicy.Insurer.Description : "-";
                                        lblLifeMonthlyPremium.Text = lifeAcc.LifePolicy.MonthlyPremium.ToString(SAHL.Common.Constants.CurrencyFormat);
                                        lblLifeSumAssured.Text = lifeAcc.LifePolicy.SumAssured.ToString(SAHL.Common.Constants.CurrencyFormat);
                                        lblLifeYearlyPremium.Text = lifeAcc.LifePolicy.YearlyPremium.ToString(SAHL.Common.Constants.CurrencyFormat);
                                    }
                                    else
                                    {
                                        // get data from the Life Application 
                                        IApplicationLife lifeApplication = lifeAcc.CurrentLifeApplication;
                                        if (lifeApplication != null)
                                        {
                                            lblLifePolicyNumber.Text = lifeApplication.ReservedAccount != null ? lifeApplication.ReservedAccount.Key.ToString() : "-";
                                            lblBroker.Text = lifeApplication.Consultant != null ? lifeApplication.Consultant.ADUserName : "-";
                                            lblLifeConsultant.Text = lifeApplication.Consultant != null ? lifeApplication.Consultant.ADUserName : "-";
                                            lblLifeDateLastUpdated.Text = lifeApplication.DateLastUpdated.HasValue ? lifeApplication.DateLastUpdated.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
                                            lblLifeInsurer.Text = lifeApplication.Insurer != null ? lifeApplication.Insurer.Description : "-";
                                            lblLifeMonthlyPremium.Text = lifeApplication.MonthlyPremium.ToString(SAHL.Common.Constants.CurrencyFormat);
                                            lblLifeSumAssured.Text = lifeApplication.SumAssured.ToString(SAHL.Common.Constants.CurrencyFormat);
                                            lblLifeYearlyPremium.Text = lifeApplication.YearlyPremium.ToString(SAHL.Common.Constants.CurrencyFormat);
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        #endregion

                        // build genericKeylist for getting reasons
                        IApplicationInformation latestApplicationInformation = _application.GetLatestApplicationInformation();
                        List<int> genericKeys = new List<int>();
                        genericKeys.Add(_application.Key);
                        //if (latestApplicationInformation != null)
                        //    genericKeys.Add(latestApplicationInformation.Key);
                        //Get all offerinfo keys for all reasons
                        foreach (IApplicationInformation appInfo in _application.ApplicationInformations)
                        {
                            genericKeys.Add(appInfo.Key);
                        }


                        #region NTU Reasons
                        if (_application.ApplicationStatus.Key == (int)OfferStatuses.NTU)
                        {
                            ntuReasonsExist = true;
                            //GET NTU reasons  - using offerkey and offerinformationkey (if exists)
                            IReadOnlyEventList<IReason> reasonList = ReasonRepo.GetReasonByGenericKeyListAndReasonTypeGroupKey(genericKeys, (int)ReasonTypeGroups.NTU);

                            gridNTUReasons.BindData(reasonList);
                        }
                        #endregion

                        #region CreditDecision

                        if (latestApplicationInformation.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer || _application.ApplicationStatus.Key == (int)OfferStatuses.Declined)
                        {
                            //Get Credit user and decision and comments
                            int aduserKey;
                            string decision;
                            DateTime decisionDate;

                            AppRepo.GetCurrentCreditDecision(_application.Key, out decision, out aduserKey, out decisionDate);

                            if (aduserKey >= 0) // aduser = -1 if no descision has been made
                            {
                                creditReasonsExist = true;

                                IADUser adUser = aduserKey == 0 ? null : OrgRepo.GetADUserByKey(aduserKey);

                                //set what sections need to display
                                if (_application.ApplicationStatus.Key == (int)OfferStatuses.Declined)
                                {
                                    trDeclineReason.Visible = true;

                                    IReadOnlyEventList<IReason> reasonList = ReasonRepo.GetReasonByGenericKeyListAndReasonTypeKey(genericKeys, (int)ReasonTypes.CreditDecline);
                                    gridCreditDeclineReasons.BindData(reasonList);

                                    lblCreditDecision.Text = "Declined.";
                                }
                                else
                                {
                                    trComment.Visible = true;

                                    BindCreditComments(_application.Key, decisionDate, adUser);
                                    lblCreditDecision.Text = decision;
                                }

                                if (adUser != null && adUser.LegalEntity != null)
                                    lblCreditUser.Text = adUser.LegalEntity.DisplayName;
                            }
                        }

                        #endregion

                        #region AdminDeclineReasons
                        //TRAC # 11313
                        //Always display decline reasons, even if a credit decision has been made
                        //Always show decline reasons if they exists, even if the offer is not declined
                        //if (!creditReasonsExist && _application.ApplicationStatus.Key == (int)OfferStatuses.Declined)
                        //{
                        //GET Decline reasons  - using offerkey and offerinformationkeys (if exists)
                        IReadOnlyEventList<IReason> adminList = ReasonRepo.GetReasonByGenericKeyListAndReasonTypeKey(genericKeys, (int)ReasonTypes.AdministrativeDecline);
                        IReadOnlyEventList<IReason> branchList = ReasonRepo.GetReasonByGenericKeyListAndReasonTypeKey(genericKeys, (int)ReasonTypes.BranchDecline);
                        IEventList<IReason> rsList = new SAHL.Common.Collections.EventList<IReason>();

                        foreach (IReason r in adminList)
                        {
                            rsList.Add(null, r);
                        }

                        foreach (IReason r in branchList)
                        {
                            rsList.Add(null, r);
                        }

                        IReadOnlyEventList<IReason> rList = new SAHL.Common.Collections.ReadOnlyEventList<IReason>(rsList);

                        if (rList.Count > 0)
                        {
                            declineReasonsExist = true;
                            grdAdminDeclineReasons.BindData(rList);
                        }
                        //}


                        #endregion

                        #region PricingForRiskDiscount

                        lblPricingAdjustment.Text = _application.GetRateAdjustments().ToString(SAHL.Common.Constants.RateFormat);

                        #endregion

                        #region EstateAgentInfo

                        IApplicationMortgageLoan appML = (IApplicationMortgageLoan)_application;

                        if (appML != null)
                        {
                            ILegalEntity estateAgent = null;
                            ILegalEntity company = null;
                            ILegalEntity branch = null;
                            ILegalEntity principal = null;

                            appML.GetEsateAgentDetails(out estateAgent, out company, out branch, out principal);

                            #region EstateAgent
                            if (estateAgent != null)
                            {
                                //set display for the accordian section for EA details
                                estateAgentExist = true;
                                //Populate Agent info
                                lblAgentsName.Text = estateAgent.DisplayName;
                                string contactNo;
                                if (!String.IsNullOrEmpty(estateAgent.WorkPhoneNumber))
                                    contactNo = estateAgent.WorkPhoneCode + " " + estateAgent.WorkPhoneNumber;
                                else
                                    contactNo = estateAgent.CellPhoneNumber;

                                lblAgentsPhone.Text = contactNo;
                            }
                            #endregion

                            #region Agency/Company

                            //Get Agency/Company Name
                            if (company != null)
                            {
                                lblAgencyName.Text = company.DisplayName;
                                lblAgency.Text = company.DisplayName;
                            }

                            #endregion

                            #region Branch

                            //Get Branch Name
                            if (branch != null)
                            {
                                lblBranchName.Text = branch.DisplayName;
                            }

                            #endregion

                            #region Principal

                            if (principal != null)
                            {
                                lblPrincipalName.Text = principal.DisplayName;

                                string principleNo;
                                if (!String.IsNullOrEmpty(principal.WorkPhoneNumber))
                                    principleNo = principal.WorkPhoneCode + " " + principal.WorkPhoneNumber;
                                else
                                    principleNo = principal.CellPhoneNumber;

                                lblPrincipalPhone.Text = principleNo;
                            }

                            #endregion
                        }
                        #endregion
                    }
                    break;
            }
            // set the visibility of the accordian details
            if (quickcashExists)
                apQuickCash.Visible = true;
            if (ntuReasonsExist)
                apNTU.Visible = true;
            if (creditReasonsExist)
                apCredit.Visible = true;
            if (lifeExists)
                apLife.Visible = true;
            if (hocExists)
                apHOC.Visible = true;
            if (declineReasonsExist)
                apAdminDecline.Visible = true;
            if (estateAgentExist)
                apEstateAgent.Visible = true;



            if (!quickcashExists && !ntuReasonsExist && !creditReasonsExist && !lifeExists && !hocExists && !declineReasonsExist)
                lblTip.Visible = false;

            #region Application Attributes

            lblQuickPayLoan.Text = _application.HasAttribute(OfferAttributeTypes.QuickPayLoan) ? "Yes" : "No";

            #endregion

			//Show Rate Adjustment
			//Get the Application Rate Override
			lblRateAdjustment.Visible = false;
			lblRateAdjustmentValue.Visible = false;
			//lblRateAdjustmentValue.Visible = false;
			var applicationInformation = _application.GetLatestApplicationInformation();
			if (applicationInformation != null)
			{
				var applicationFinancialAdjustment = (from financialAdjustment in applicationInformation.ApplicationInformationFinancialAdjustments
											   where financialAdjustment.FinancialAdjustmentTypeSource.Key == (int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.CounterRate
											   select financialAdjustment).FirstOrDefault();
				//Get the Application Rate Adjustment
				if (applicationFinancialAdjustment != null)
				{
					var appInfoRateAdjustment = (from applicationInformationRateAdjustment in applicationFinancialAdjustment.ApplicationInformationAppliedRateAdjustments
												 select applicationInformationRateAdjustment).FirstOrDefault();
					lblRateAdjustment.Visible = true;
					lblRateAdjustmentValue.Visible = true;
					lblRateAdjustmentValue.Text = String.Format("({0}) {1}", appInfoRateAdjustment.RateAdjustmentElement.RateAdjustmentElementType.Description, appInfoRateAdjustment.RateAdjustmentElement.RateAdjustmentValue.ToString(SAHL.Common.Constants.RateFormat));
				}
			}

            if (_showCapitecDetails)
            {
                IApplicationCapitecDetail applicationCapitecDetail = AppRepo.GetApplicationCapitecDetail(_application.Key);

                if (applicationCapitecDetail != null)
                {
                    lblCapitecBranch.Text = applicationCapitecDetail.Branch;
                    lblCapitecConsultant.Text = applicationCapitecDetail.Consultant;
                }
                else
                {
                    lblCapitecBranch.Text = "Unknown";
                    lblCapitecConsultant.Text = "Unknown";
                }
            }

            if(_showComcorpDetails)
            {
                IVendor vendor = _application.GetComcorpVendor();
                if (vendor != null)
                {
                    lblComcorpVendor.Text = vendor.LegalEntity.DisplayName;
                }
                else
                {
                    lblComcorpVendor.Text = "Unknown";
                }
            }

            AddTrace(this, "BindControls_End");
        }

        private void BindCreditComments(int appKey, DateTime dt, IADUser adUser)
        {
            AddTrace(this, "BindCreditComments_Start");

            if (adUser == null)
                return;

            IEventList<SAHL.Common.BusinessModel.Interfaces.IMemo> memoList = MemoRepo.GetMemoByGenericKeyADUserAndDate(appKey, (int)GenericKeyTypes.Offer, dt, adUser);

            gridCreditComments.BindData(memoList);
            AddTrace(this, "BindCreditComments_End");

        }

        private static string GetOperatorName(IApplicationRole applicationRole, bool includeFullName, IOrganisationStructureRepository osRepo)
        {

            string operatorName = "";

            ILegalEntityNaturalPerson legalEntityNaturalPerson = applicationRole != null ? applicationRole.LegalEntity as ILegalEntityNaturalPerson : null;

            if (legalEntityNaturalPerson != null)
            {
                IADUser adUser = osRepo.GetAdUserByLegalEntityKey(legalEntityNaturalPerson.Key);

                if (includeFullName)
                {
                    operatorName = (
                        (!String.IsNullOrEmpty(legalEntityNaturalPerson.FirstNames) ? legalEntityNaturalPerson.FirstNames : "")
                        + " "
                        + (!String.IsNullOrEmpty(legalEntityNaturalPerson.Surname) ? (legalEntityNaturalPerson.Surname != legalEntityNaturalPerson.FirstNames ? legalEntityNaturalPerson.Surname : "") : "")
                        + (adUser != null ? " (" + adUser.ADUserName + ")" : "")
                        ).ToString().Trim();
                }
                else
                    operatorName = adUser.ADUserName;
            }

            if (String.IsNullOrEmpty(operatorName))
                operatorName = "-";

            return operatorName;
        }

        #endregion

        #region GridRowItem
        /// <summary>
        /// 
        /// </summary>
        internal class GridRowItem
        {
            private string _applicationKey;
            private string _legalEntityName;
            private string _idCompanyNo;
            private string _role;
            private string _maritalStatus;
            private string _incomeContributor;
            private string _income;
            private string _employmentType;

            /// <summary>
            /// 
            /// </summary>
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string ApplicationKey
            {
                get { return _applicationKey; }
                set { _applicationKey = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string LegalEntityName
            {
                get { return _legalEntityName; }
                set { _legalEntityName = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string IDCompanyNo
            {
                get { return _idCompanyNo; }
                set { _idCompanyNo = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string Role
            {
                get { return _role; }
                set { _role = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string MaritalStatus
            {
                get { return _maritalStatus; }
                set { _maritalStatus = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string IncomeContributor
            {
                get { return _incomeContributor; }
                set { _incomeContributor = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            /// 
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string Income
            {
                get { return _income; }
                set { _income = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string EmploymentType
            {
                get { return _employmentType; }
                set { _employmentType = value; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string UnionMember
            {
                get;
                set;
            }

        }
        #endregion

        #region Repos


        protected IReasonRepository ReasonRepo
        {
            get
            {
                if (_reasonRepo == null)
                    _reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();

                return _reasonRepo;
            }
        }
        protected IMemoRepository MemoRepo
        {
            get
            {
                if (_memoRepo == null)
                    _memoRepo = RepositoryFactory.GetRepository<IMemoRepository>();

                return _memoRepo;
            }
        }

        protected IStageDefinitionRepository StageRepo
        {
            get
            {
                if (_stageRepo == null)
                    _stageRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

                return _stageRepo;
            }
        }

        protected IApplicationRepository AppRepo
        {
            get
            {
                if (_appRepo == null)
                    _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                return _appRepo;
            }
        }

        protected IOrganisationStructureRepository OrgRepo
        {
            get
            {
                if (_orgRepo == null)
                    _orgRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

                return _orgRepo;
            }
        }


        protected IEstateAgentRepository EstateRepo
        {
            get
            {
                if (_estateRepo == null)
                    _estateRepo = RepositoryFactory.GetRepository<IEstateAgentRepository>();

                return _estateRepo;
            }
        }



        #endregion
    }


}