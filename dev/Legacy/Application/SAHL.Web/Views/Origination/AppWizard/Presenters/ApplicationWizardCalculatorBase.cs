using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common;

namespace SAHL.Web.Views.Origination.Presenters
{
	public class ApplicationWizardCalculatorBase : SAHLCommonBasePresenter<IApplicationWizardCalculator>
	{
		#region Locals

		private ILookupRepository _lookupRepo;
		private IFinancialAdjustmentRepository _fadjRepo;
		public IApplicationRepository _appRepo;
		private IControlRepository _ctrlRepo;
		private ICreditMatrixRepository _cmRepo;
		private IX2Repository _x2Repo;
		private IRuleService _ruleServ;

		protected IApplication _application;

		protected IApplicationStatus _statusOpen;
		protected ICreditMatrix _cm;
		protected ICategory _cat;
		protected IEmploymentType _empType;
		protected IOriginationSource _os;
		protected int _marketRateKey;
		protected int _maturityMonths = 6;
		protected int _rateConfigKey;
		protected IResetConfiguration _resetConfig;

		#endregion Locals

		/// <summary>
		///
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public ApplicationWizardCalculatorBase(IApplicationWizardCalculator view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}

		/// <summary>
		/// Hook the events fired by the view and call relevant methods to bind control data
		/// </summary>
		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);

			if (!_view.ShouldRunPage)
				return;

			_view.EdgeTerm = Convert.ToInt32(CtrlRepo.GetControlByDescription("Edge Max Term").ControlNumeric);
			_view.OnCalculateButtonClicked += new EventHandler(_view_OnCalculateButtonClicked);

			// TODO: This should be changed to use enumeration values
			int[] iPurposes = new int[] { 2, 3, 4 };

			ReadOnlyEventList<IProduct> listProduct = AppRepo.GetOriginationProducts();
			ReadOnlyEventList<IMortgageLoanPurpose> listmlp = AppRepo.GetMortgageLoanPurposes(iPurposes);
			IEventList<IEmploymentType> listEmpType = LookupRepo.EmploymentTypes;

			// the code below has been removed and the 'Unknown' employment type is removed in the
			// bind method in the view. This is because the local list is actually a pointer to the lookup
			// and when the item is removed from the local list it is actually removed from the lookup aswell.
			//foreach (IEmploymentType empType in listEmpType)
			//{
			//    if (empType.Key == (int)EmploymentTypes.Unknown)
			//        listEmpType.Remove(_view.Messages, empType);
			//}

			_view.BindProductDropdown(listProduct);
			_view.BindPurposeDropdown(listmlp);
			_view.BindEmploymentType(listEmpType);

			IReasonRepository RR = RepositoryFactory.GetRepository<IReasonRepository>();
			IReadOnlyEventList<IReasonDefinition> lstDefs = RR.GetReasonDefinitionsByReasonTypeGroup(SAHL.Common.Globals.ReasonTypeGroups.ClientNeedAnalysisCategory);
			Dictionary<string, string> dictDefs = new Dictionary<string, string>();
			foreach (IReasonDefinition def in lstDefs)
			{
				dictDefs.Add(def.Key.ToString(), def.ReasonDescription.Description);
			}
			_view.BindNeedsIdentificationDropdown(dictDefs);
		}

		protected void _view_OnCalculateButtonClicked(object sender, EventArgs e)
		{
			// Business rule checks, populate the messages and return if any have been added.
			AppRepo.CreditDisqualifications(false, 0, 0, _view.HouseholdIncome, _view.LoanAmountRequired, _view.EstimatedPropertyValue, _view.EmploymentTypeKey, false, _view.Term, false);
			CheckBusinessRules();

			if (!_view.IsValid)
				return;

            

			try
			{
                if (_application.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.Unknown)
                {
                    UpdateApplication(); //persist
                }
                else
                {
                    CreateNewApplication();
                }
			}
			catch
			{
				if (!_view.IsValid)
				{
					return;
				}
			}

			// Instead of all this old stuff, just use the common recalc method
			double initiationFee = 0;
			double registrationFee = 0;
			double cancelFee = 0;
			double interimInterest = 0;
			//double bondToRegister;
			double baseRate = 0;
			double baseRateFix = 0;
			double loanAmount = 0;
			double loanAmountVar = 0;
			double loanAmountFix = 0;
			double linkRate = 0;
			double interestRate = 0;
			double interestRateFix = 0;
			double instalmentTotal = 0;
			double instalmentIOTotal = 0;
			double instalmentVar = 0;
			double instalmentFix = 0;
			double instalmentEHLAM = 0D;
			double pti = 1D;
			double ptiFix = 0;
			double interestOverTermIO = 0;
			double interestOverTermVar = 0;
			double interestOverTermFix = 0;
			double minIncome;
			double ltv = 2D;
			double totalFees;
			double percentFix = 0;

			IEventList<IMarketRate> mRates = LookupRepo.MarketRates;
			foreach (IMarketRate mR in mRates)
			{
				if (mR.Key == 4) //18th reset rate
					baseRate = mR.Value;

				if (mR.Key == _view.VarifixMarketRateKey) //VF 6month rate
					baseRateFix = mR.Value;

				if (mR.Key == _view.VarifixMarketRateKey) //VF 5yr rate
					baseRateFix = mR.Value;
			}

			IApplicationMortgageLoan appML = _application as IApplicationMortgageLoan;
			ISupportsVariableLoanApplicationInformation sVLAppInfo = appML.CurrentProduct as ISupportsVariableLoanApplicationInformation;
			ISupportsInterestOnlyApplicationInformation ioAppInfo = appML.CurrentProduct as ISupportsInterestOnlyApplicationInformation;
			IApplicationInformationVariableLoan aivl = null;
			ICreditCriteria cc = null;

			if (sVLAppInfo != null)
			{
				aivl = sVLAppInfo.VariableLoanInformation;
			}

			if (aivl != null)
			{
				loanAmount = aivl.LoanAgreementAmount.HasValue ? aivl.LoanAgreementAmount.Value : 0;
				interimInterest = aivl.InterimInterest.HasValue ? aivl.InterimInterest.Value : 0D;
				ltv = aivl.LTV.HasValue ? aivl.LTV.Value : 2D;
				pti = aivl.PTI.HasValue ? aivl.PTI.Value : 1D;
				cc = aivl.CreditCriteria;
				linkRate = cc.Margin.Value;
				interestRate = aivl.MarketRate.Value + aivl.RateConfiguration.Margin.Value;
				interestRateFix = cc.Margin.Value + baseRateFix;
				instalmentVar = aivl.MonthlyInstalment.HasValue ? aivl.MonthlyInstalment.Value : 0D;
				instalmentTotal = instalmentVar;

				if ((_view.InterestOnly || _view.ProductKey == (int)Products.Edge) && ioAppInfo != null && ioAppInfo.InterestOnlyInformation != null)
					instalmentIOTotal = ioAppInfo.InterestOnlyInformation.Installment.HasValue ? ioAppInfo.InterestOnlyInformation.Installment.Value : 0D;
			}

			foreach (IApplicationExpense applicationExpense in appML.ApplicationExpenses)
			{
				switch (applicationExpense.ExpenseType.Key)
				{
					case (int)ExpenseTypes.InitiationFeeBondPreparationFee:
						initiationFee = applicationExpense.TotalOutstandingAmount;
						break;
					case (int)ExpenseTypes.RegistrationFee:
						registrationFee = applicationExpense.TotalOutstandingAmount;
						break;
					case (int)ExpenseTypes.CancellationFee:
						cancelFee = applicationExpense.TotalOutstandingAmount;
						break;
					default:
						break;
				}
			}
			totalFees = initiationFee + registrationFee + cancelFee;

			if (cc == null)
				_view.Messages.Add(new Error("The values captured do not fit our lending criteria. Try a lower loan amount.", "The values captured do not fit our lending criteria. Try a lower loan amount."));

			if (_view.Messages.Count > 0)
				return;

			_view.CreditMatrixKey = cc.CreditMatrix.Key;
			_view.CategoryKey = cc.Category.Key;

			//rates set
			_view.MarginKey = cc.Margin.Key;
			_view.ActiveMarketRate = baseRate;
			_view.LinkRate = linkRate;
			_view.InterestRateFix = interestRateFix;
			_view.InterestRate = interestRate;

			//Calculate interest over term
			//Need to do extra work for EHL here, some IO, some Var
			if (_view.ProductKey == (int)Products.Edge)
			{
				IApplicationProductEdge appEHL = appML.CurrentProduct as IApplicationProductEdge;
				int InterestOnlyTerm = appEHL.EdgeInformation.InterestOnlyTerm;

				instalmentEHLAM = appEHL.EdgeInformation.AmortisationTermInstalment;

				interestOverTermVar = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm(loanAmount, interestRate, _view.EdgeTerm - InterestOnlyTerm, false);
				interestOverTermVar += SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm(loanAmount, interestRate, InterestOnlyTerm, true);
			}
			else
			{
				interestOverTermVar = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm(loanAmount, interestRate, _view.Term, false);
				if (_view.InterestOnly)
					interestOverTermIO = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm(loanAmount, interestRate, _view.Term, _view.InterestOnly);
			}

			//Calculate fixed amounts
			if (_view.ProductKey == (int)Products.VariFixLoan)
			{
				IApplicationProductVariFixLoan appVF = appML.CurrentProduct as IApplicationProductVariFixLoan;
				IApplicationInformationVarifixLoan aivf = null;
				if (appVF != null)
					aivf = appVF.VariFixInformation;

				percentFix = _view.FixPercent;
				loanAmountFix = (loanAmount * percentFix);
				loanAmountVar = (loanAmount * (1 - percentFix));

				//instalment VF accounts can not be interest only
				//fix
				instalmentFix = aivf.FixedInstallment;
				//var
				if (loanAmountVar > 0)
					instalmentVar = (aivl.MonthlyInstalment.HasValue ? aivl.MonthlyInstalment.Value : 0D) - aivf.FixedInstallment;
				else
					instalmentVar = 0;

				//pti
				ptiFix = pti;
				pti = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculatePTI(SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(loanAmount, interestRate, _view.Term, false), _view.HouseholdIncome);

				//interest over term
				//Fix
				interestOverTermFix = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm(loanAmountFix, interestRateFix, _view.Term, false);
				//Var
				if (loanAmountVar > 0)
					interestOverTermVar = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm(loanAmountVar, interestRate, _view.Term, false);
				else
					interestOverTermVar = 0;
			}

			//Calculate min income against the CC max pti
			minIncome = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateMinimumIncomeRequired(instalmentTotal, cc.PTI.Value);

			// set display of the form
			//Warnings:
			if (minIncome > _view.HouseholdIncome)
				_view.IncomeSufficient = false;

			//ltv pti
			_view.LTV = ltv;
			_view.PTI = pti;
			_view.PTIFix = ptiFix;

			//Percentages
			_view.FixPercent = _view.FixPercent;

			//Amounts
			_view.LoanAmountTotal = loanAmount;
			_view.LoanAmountFix = loanAmountFix;
			_view.LoanAmountVar = loanAmountVar;

			//instalment
			_view.InstalmentVar = instalmentVar;
			_view.InstalmentFix = instalmentFix;
			_view.InstalmentTotal = instalmentVar + instalmentFix;
			_view.InstalmentIOTotal = instalmentIOTotal;
			_view.InstalmentEHLAM = instalmentEHLAM;

			//finance charges
			_view.FinanceChargesVar = interestOverTermVar;
			_view.FinanceChargesFix = interestOverTermFix;
			_view.FinanceChargesIOTotal = interestOverTermIO;
			_view.FinanceChargesTotal = interestOverTermVar + interestOverTermFix;

			//Fees
			_view.CancellationFee = cancelFee;
			_view.RegistrationFee = registrationFee;
			_view.InitiationFee = initiationFee;
			_view.TotalFee = totalFees;
			_view.InterimInterest = interimInterest;

			AppRepo.CreditDisqualifications(true, ltv, pti, _view.HouseholdIncome, loanAmount, _view.EstimatedPropertyValue, _view.EmploymentTypeKey, false, _view.Term, false);
			CheckBusinessRules();

            RunRule(Rules.ApplicationProductEdgeLTVWarning, _application);
			RunRule("CalculatorAlphaHousingLoanMustBeNewVariableLoan", _view.ProductKey, ltv, _view.HouseholdIncome, _view.EmploymentTypeKey);
			RunRule("CalculatorAlphaHousingLoanMustNotBeInterestOnlyLoan", _view.ProductKey, ltv, _view.HouseholdIncome, _view.EmploymentTypeKey, _view.InterestOnly);
			if (!_view.IsValid)
				return;

			//if we got here the application qualifies
			_view.ApplicationQualifies = true;
		}

		/// <summary>
		///
		/// </summary>
		protected void ApplicationCreateDefaults()
		{
			int osKey = OriginationSourceHelper.PrimaryOriginationSourceKey(_view.CurrentPrincipal);

			_os = AppRepo.GetOriginationSource((OriginationSources)osKey);
			_resetConfig = AppRepo.GetApplicationDefaultResetConfiguration();
			_statusOpen = LookupRepo.ApplicationStatuses.ObjectDictionary["1"];
			//_cm = CMRepo.GetCreditMatrixByKey(_view.CreditMatrixKey);
			//_cat = CMRepo.GetCategoryByKey(_view.CategoryKey);

			// Market rate key is either 18th NACM or Varifix
			//if (_view.ProductKey == (int)SAHL.Common.Globals.Products.VariFixLoan)
			//    _marketRateKey = 3;
			//else
			_marketRateKey = 4;

			if (LookupRepo.EmploymentTypes.ObjectDictionary.ContainsKey(_view.EmploymentTypeKey.ToString()))
				_empType = LookupRepo.EmploymentTypes.ObjectDictionary[_view.EmploymentTypeKey.ToString()];
		}

		protected void ApplicationCreateProductSetup()
		{
			//Set the product if required

			if (_application.GetLatestApplicationInformation().Product.Key != _view.ProductKey)
			{
				// IApplicationProductMortgageLoan applicationProduct = _app.CurrentProduct as IApplicationProductMortgageLoan;

				switch ((OfferTypes)_application.ApplicationType.Key)
				{
					case OfferTypes.NewPurchaseLoan:
						{
							IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = _application as IApplicationMortgageLoanNewPurchase;
							switch (_view.ProductKey)
							{
								case (int)SAHL.Common.Globals.Products.VariableLoan:
									{
										applicationMortgageLoanNewPurchase.SetProduct((ProductsNewPurchase.VariableLoan));
										break;
									}
								case (int)SAHL.Common.Globals.Products.SuperLo:
									{
										applicationMortgageLoanNewPurchase.SetProduct((ProductsNewPurchase.SuperLo));
										break;
									}
								case (int)SAHL.Common.Globals.Products.VariFixLoan:
									{
										applicationMortgageLoanNewPurchase.SetProduct((ProductsNewPurchase.VariFixLoan));
										break;
									}
								case (int)SAHL.Common.Globals.Products.Edge:
									{
										applicationMortgageLoanNewPurchase.SetProduct((ProductsNewPurchase.Edge));
										break;
									}
							}
							break;
						}
					case OfferTypes.RefinanceLoan:
						{
							IApplicationMortgageLoanRefinance applicationMortgageLoanRefinance = _application as IApplicationMortgageLoanRefinance;

							switch (_view.ProductKey)
							{
								case (int)SAHL.Common.Globals.Products.VariableLoan:
									{
										applicationMortgageLoanRefinance.SetProduct((ProductsRefinance.VariableLoan));
										break;
									}
								case (int)SAHL.Common.Globals.Products.SuperLo:
									{
										applicationMortgageLoanRefinance.SetProduct((ProductsRefinance.SuperLo));
										break;
									}
								case (int)SAHL.Common.Globals.Products.VariFixLoan:
									{
										applicationMortgageLoanRefinance.SetProduct((ProductsRefinance.VariFixLoan));
										break;
									}
								case (int)SAHL.Common.Globals.Products.Edge:
									{
										applicationMortgageLoanRefinance.SetProduct((ProductsRefinance.Edge));
										break;
									}
							}
							break;
						}
					case OfferTypes.SwitchLoan:
						{
							IApplicationMortgageLoanSwitch applicationMortgageLoanSwitch = _application as IApplicationMortgageLoanSwitch;

							switch (_view.ProductKey)
							{
								case (int)SAHL.Common.Globals.Products.VariableLoan:
									{
										applicationMortgageLoanSwitch.SetProduct((ProductsSwitchLoan.VariableLoan));

										break;
									}								
								case (int)SAHL.Common.Globals.Products.SuperLo:
									{
										applicationMortgageLoanSwitch.SetProduct((ProductsSwitchLoan.SuperLo));

										break;
									}
								case (int)SAHL.Common.Globals.Products.VariFixLoan:
									{
										applicationMortgageLoanSwitch.SetProduct((ProductsSwitchLoan.VariFixLoan));

										break;
									}
								case (int)SAHL.Common.Globals.Products.Edge:
									{
										applicationMortgageLoanSwitch.SetProduct((ProductsSwitchLoan.Edge));

										break;
									}
							}

							break;
						}
				}
			}

			//do the product specific stuff
			switch (_view.ProductKey)
			{
				case (int)Products.NewVariableLoan:
					SetupNVL(_application.CurrentProduct);
					break;
				case (int)Products.SuperLo:
					SetupSuperLo(_application.CurrentProduct);
					break;
				case (int)Products.VariFixLoan:
					SetupVariFix(_application.CurrentProduct);
					break;
				case (int)Products.Edge:
					SetupEHL(_application.CurrentProduct);
					break;
				default:
					break;
			}

			//Mortgage Loan details
			IApplicationMortgageLoan appML = (IApplicationMortgageLoan)_application;
			appML.ApplicationStartDate = DateTime.Now;
			appML.ApplicationStatus = _statusOpen;
			appML.ClientEstimatePropertyValuation = _view.EstimatedPropertyValue;
			appML.ResetConfiguration = _resetConfig;

            appML.CalculateApplicationDetail(false, false);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void _view_OnWorkFlowCancelButtonClicked(object sender, EventArgs e)
		{
			IX2Info XI = X2Service.GetX2Info(_view.CurrentPrincipal);
			if (XI == null || String.IsNullOrEmpty(XI.SessionID))
				X2Service.LogIn(_view.CurrentPrincipal);

			X2Service.CancelActivity(_view.CurrentPrincipal);

			X2Service.WorkflowNavigate(_view.CurrentPrincipal, Navigator);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
		{
			//if (!GlobalCacheData.ContainsKey("INSTANCEID"))
			//    CreateWorkflowCase(0);
			Navigator.Navigate("Cancel");
		}

		/// <summary>
		///
		/// </summary>
		protected void CreateNewApplication()
		{
			ApplicationCreateDefaults();

			//Create application
			//If we enter here with an unknown application, the _app will be updated and saved
			//(without offerinformation structures) - we will need to re-save once that work is done
			int AppTypeKey = 0;
			if (_application != null)
				AppTypeKey = _application.ApplicationType.Key;

			if (_view.ProductKey == 0)
				_view.ProductKey = (int)Products.NewVariableLoan;

			if (_application == null || _application.Key == 0 || AppTypeKey == (int)OfferTypes.Unknown)
			{
				switch (_view.MortgageLoanPurpose)
				{
					case MortgageLoanPurposes.Newpurchase:
						_application = AppRepo.CreateNewPurchaseApplication(_os, (ProductsNewPurchaseAtCreation)_view.ProductKey, null);
						//IApplicationMortgageLoanNewPurchase anp = _app as IApplicationMortgageLoanNewPurchase;
						//anp.SetInitiationFee(_view.InitiationFee, false);
						//anp.SetRegistrationFee(_view.RegistrationFee, false);
						break;
					case MortgageLoanPurposes.Refinance:
						_application = AppRepo.CreateRefinanceApplication(_os, (ProductsRefinanceAtCreation)_view.ProductKey, null);
						IApplicationMortgageLoanRefinance ar = _application as IApplicationMortgageLoanRefinance;
						//ar.SetInitiationFee(_view.InitiationFee, false);
						//ar.SetRegistrationFee(_view.RegistrationFee, false);
						ar.CapitaliseFees = _view.CapitaliseFees;
						break;
					case MortgageLoanPurposes.Switchloan:
						_application = AppRepo.CreateSwitchLoanApplication(_os, (ProductsSwitchLoanAtCreation)_view.ProductKey, null);
						IApplicationMortgageLoanSwitch asw = _application as IApplicationMortgageLoanSwitch;
						//asw.SetCancellationFee(_view.CancellationFee, false);
						//asw.SetInitiationFee(_view.InitiationFee, false);
						//asw.SetRegistrationFee(_view.RegistrationFee, false);
						//asw.InterimInterest = _view.InterimInterest;
						asw.CapitaliseFees = _view.CapitaliseFees;
						break;
					default:
						break;
				}
			}

			//do the product specific stuff
			ApplicationCreateProductSetup();

			//if the application came in as unknown we need to persist the remaining data structures
            if (AppTypeKey == (int)OfferTypes.Unknown)
            {
                using (new TransactionScope())
                {
                    AppRepo.SaveApplication(_application);
                }
            }
		}

		/// <summary>
		///
		/// </summary>
		protected void UpdateApplication()
		{
			TransactionScope ts = new TransactionScope();
			try
			{
				ApplicationCreateDefaults();

				MortgageLoanPurposes purp = MortgageLoanPurposes.Unknown;

				if (_application is IApplicationMortgageLoanNewPurchase)
					purp = MortgageLoanPurposes.Newpurchase;

				if (_application is IApplicationMortgageLoanSwitch)
					purp = MortgageLoanPurposes.Switchloan;

				if (_application is IApplicationMortgageLoanRefinance)
					purp = MortgageLoanPurposes.Refinance;

				//Create application
				switch (_view.MortgageLoanPurpose)
				{
					case MortgageLoanPurposes.Newpurchase:
						if (_application is IApplicationUnknown)
							_application = AppRepo.CreateNewPurchaseApplication(_os, (ProductsNewPurchaseAtCreation)_view.ProductKey, (IApplicationUnknown)_application);

						if (purp != _view.MortgageLoanPurpose && purp != MortgageLoanPurposes.Unknown)
							throw new Exception("Application is not Newpurchase, please change to " + purp.ToString() + "in the calculator.");
						break;
					case MortgageLoanPurposes.Refinance:
						if (_application is IApplicationUnknown)
							_application = AppRepo.CreateRefinanceApplication(_os, (ProductsRefinanceAtCreation)_view.ProductKey, (IApplicationUnknown)_application);

						if (purp != _view.MortgageLoanPurpose && purp != MortgageLoanPurposes.Unknown)
							throw new Exception("Application is not Refinance, please change to " + purp.ToString() + "in the calculator.");

						IApplicationMortgageLoanRefinance ar = _application as IApplicationMortgageLoanRefinance;
						ar.CapitaliseFees = _view.CapitaliseFees;
						break;
					case MortgageLoanPurposes.Switchloan:
						if (_application is IApplicationUnknown)
							_application = AppRepo.CreateSwitchLoanApplication(_os, (ProductsSwitchLoanAtCreation)_view.ProductKey, (IApplicationUnknown)_application);
						if (purp != _view.MortgageLoanPurpose && purp != MortgageLoanPurposes.Unknown)
							throw new Exception("Application is not Switchloan, please change to " + purp.ToString() + "in the calculator.");

						IApplicationMortgageLoanSwitch asw = _application as IApplicationMortgageLoanSwitch;
						asw.CapitaliseFees = _view.CapitaliseFees;
						break;
					default:
						break;
				}

				IReasonRepository RR = RepositoryFactory.GetRepository<IReasonRepository>();
				IReason reason = RR.CreateEmptyReason();
				reason.GenericKey = _application.Key;

				IReasonDefinition rd = RR.GetReasonDefinitionByKey(_view.NeedsIdentificationKey);
				reason.ReasonDefinition = rd;

				//do the product specific stuff
				ApplicationCreateProductSetup();

				RR.SaveReason(reason);

				//
				if (_view.Messages.WarningMessages.Count > 0)
					return;

				AppRepo.SaveApplication((IApplication)_application);
				ts.VoteCommit();
			}
			catch (Exception)
			{
				ts.VoteRollBack();
				if (_view.IsValid)
					throw;
			}
			finally
			{
				ts.Dispose();
			}
		}

		//private void RunRules()
		//{
		//    IRuleService RS = ServiceFactory.GetService<IRuleService>();
		//    List<string> lstRules = new List<string>();
		//    lstRules.Add("ApplicationCheckMinLoanAmount");
		//    lstRules.Add("ApplicationCheckMinIncome");
		//    RS.ExecuteRuleSet(_view.Messages, lstRules, _app);
		//}

		protected void X2CompleteNavigate()
		{
			IX2Info XI = X2Service.GetX2Info(_view.CurrentPrincipal);
			if (XI == null || String.IsNullOrEmpty(XI.SessionID))
				X2Service.LogIn(_view.CurrentPrincipal);

			X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);

			X2Service.WorkflowNavigate(_view.CurrentPrincipal, Navigator);
		}

		protected void SaveAndMoveOn()
		{
			TransactionScope txn = new TransactionScope();

			try
			{
				if (_application.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.Unknown)
				{
					UpdateApplication(); //persist
				}
				else
				{
					//This must be called to update any changes to the calculator
					UpdateApplication(); //persist
					//This probably does not need to be called at all, does not do the required work
					UpdateExistingApplication();
				}

				X2CompleteNavigate(); //workflow complete and navigate

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
			}
		}

		protected void UpdateExistingApplication()
		{
			TransactionScope ts = new TransactionScope();
			try
			{
				IApplicationProductMortgageLoan applicationProduct = _application.CurrentProduct as IApplicationProductMortgageLoan;

				ISupportsVariableLoanApplicationInformation vl = _application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
				if (vl != null)
				{
					if (!_view.HouseholdIncome.Equals(vl.VariableLoanInformation.HouseholdIncome.Value))
					{
						vl.VariableLoanInformation.HouseholdIncome = _view.HouseholdIncome;
					}
				}

				switch ((OfferTypes)_application.ApplicationType.Key)
				{
					case OfferTypes.NewPurchaseLoan:
						{
							IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = _application as IApplicationMortgageLoanNewPurchase;
							if (!_view.ProductKey.ToString().Equals(Convert.ToString((int)applicationProduct.ProductType)))
							{
								applicationMortgageLoanNewPurchase.SetProduct((ProductsNewPurchase)_view.ProductKey);
							}

							if (!_view.PurchasePrice.Equals(applicationMortgageLoanNewPurchase.PurchasePrice.Value))
							{
								applicationMortgageLoanNewPurchase.PurchasePrice = _view.PurchasePrice;
							}

							if (applicationMortgageLoanNewPurchase.CashDeposit == null
								|| !_view.Deposit.Equals(applicationMortgageLoanNewPurchase.CashDeposit.Value))
							{
								applicationMortgageLoanNewPurchase.CashDeposit = _view.Deposit;
							}
						}

						break;
					case OfferTypes.RefinanceLoan:
						IApplicationMortgageLoanRefinance applicationMortgageLoanRefinance = _application as IApplicationMortgageLoanRefinance;
						if (!_view.ProductKey.ToString().Equals(Convert.ToString((int)applicationProduct.ProductType)))
						{
							applicationMortgageLoanRefinance.SetProduct((ProductsRefinance)_view.ProductKey);
						}

						if (!_view.CashOut.Equals(applicationMortgageLoanRefinance.CashOut.Value))
						{
							applicationMortgageLoanRefinance.CashOut = _view.CashOut;
						}

						if (!_view.CapitaliseFees.Equals(applicationMortgageLoanRefinance.CapitaliseFees))
						{
							applicationMortgageLoanRefinance.CapitaliseFees = _view.CapitaliseFees;
						}
						ISupportsVariableLoanApplicationInformation _vlInfo = _application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
						if (_vlInfo != null)
						{
							if (_vlInfo.VariableLoanInformation.PropertyValuation.Value != _view.EstimatedPropertyValue)
								_vlInfo.VariableLoanInformation.PropertyValuation = _view.EstimatedPropertyValue;
						}

						break;
					case OfferTypes.SwitchLoan:
						IApplicationMortgageLoanSwitch applicationMortgageLoanSwitch = _application as IApplicationMortgageLoanSwitch;
						if (!_view.ProductKey.ToString().Equals((int)applicationProduct.ProductType))
						{
							applicationMortgageLoanSwitch.SetProduct((ProductsSwitchLoan)Convert.ToInt32(_view.ProductKey));
						}

						if (!_view.CurrentLoan.Equals(applicationMortgageLoanSwitch.ExistingLoan.Value))
						{
							applicationMortgageLoanSwitch.ExistingLoan = _view.CurrentLoan;
						}

						if (!_view.CashOut.Equals(applicationMortgageLoanSwitch.CashOut.Value))
						{
							applicationMortgageLoanSwitch.CashOut = _view.CashOut;
						}

						if (!_view.CapitaliseFees.Equals(applicationMortgageLoanSwitch.CapitaliseFees))
						{
							applicationMortgageLoanSwitch.CapitaliseFees = _view.CapitaliseFees;
						}

						applicationMortgageLoanSwitch.SetCancellationFee(_view.CancellationFee, false);

						ISupportsVariableLoanApplicationInformation _vlInfoSwitch = _application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
						if (_vlInfoSwitch != null)
						{
							if (_vlInfoSwitch.VariableLoanInformation.PropertyValuation.Value != _view.EstimatedPropertyValue)
								_vlInfoSwitch.VariableLoanInformation.PropertyValuation = _view.EstimatedPropertyValue;
						}
						break;
					default:
						break;
				}

				//check for interest only changes and add or remove if necessary

				if (_view.InterestOnly)
				{
					IApplicationInformation info = _application.GetLatestApplicationInformation();
					bool found = false;
					for (int x = 0; x < info.ApplicationInformationFinancialAdjustments.Count; x++)
					{
						if (info.ApplicationInformationFinancialAdjustments[x].FinancialAdjustmentTypeSource.Key == (int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.InterestOnly)
						{
							found = true;
							break;
						}
					}
					if (!found)
					{
						IApplicationInformationFinancialAdjustment afa = _appRepo.GetEmptyApplicationInformationFinancialAdjustment();

						afa.ApplicationInformation = _application.GetLatestApplicationInformation();

						afa.FinancialAdjustmentTypeSource = FADJRepo.GetFinancialAdjustmentTypeSourceByKey((int)FinancialAdjustmentTypeSources.InterestOnly);

						_application.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments.Add(_view.Messages, afa);
					}
				}
				else
				{
					foreach (IApplicationInformationFinancialAdjustment applicationInformationFinancialAdj in _application.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments)
					{
						if (applicationInformationFinancialAdj.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.InterestOnly)
						{
							_application.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments.Remove(_view.Messages, applicationInformationFinancialAdj);

							return;
						}
					}
				}

				ISupportsVariFixApplicationInformation supportsVarifix = _application.CurrentProduct as ISupportsVariFixApplicationInformation;
				if (supportsVarifix != null)
				{
					supportsVarifix.VariFixInformation.FixedPercent = _view.FixPercent;
					ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();

					supportsVarifix.VariFixInformation.MarketRate = LR.MarketRates.ObjectDictionary[_view.VarifixMarketRateKey.ToString()];
				}

				//  RunRules();

				if (_view.Messages.WarningMessages.Count > 0)
				{
					return;
				}

				AppRepo.SaveApplication(_application);

				IReasonRepository RR = RepositoryFactory.GetRepository<IReasonRepository>();

				IReadOnlyEventList<IReason> lstReasons = RR.GetReasonByGenericTypeAndKey((int)SAHL.Common.Globals.GenericKeyTypes.Offer, _application.Key);
				if (lstReasons.Count > 0)
				{
					IReason reason = lstReasons[0];

					IReasonDefinition newDef = RR.GetReasonDefinitionByKey(_view.NeedsIdentificationKey);

					if (newDef != null)
					{
						reason.ReasonDefinition = newDef;
						RR.SaveReason(reason);
					}
				}
				ts.VoteCommit();
			}
			catch (Exception)
			{
				ts.VoteRollBack();
				if (_view.IsValid)
					throw;
			}
			finally
			{
				ts.Dispose();
			}
		}

		/// <summary>
		///
		/// </summary>
		protected void CheckBusinessRules()
		{
			//TODO: get these values from the CM or CC or DB
			IControl ctrl = CtrlRepo.GetControlByDescription("Calc - minValuation");
			double minPurchasePrice = (ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 0);
			double minFixFinServiceAmount = 50000;
			double minFixTotalLoanAmount = 200000;

			// For the application wizard. Default to NewVariable if no product has been selected

			int productKey = _view.ProductKey;
			if (productKey == 0)
			{
				productKey = (int)SAHL.Common.Globals.Products.NewVariableLoan;
			}

			if (productKey == 0)
			{
				_view.Messages.Add(new Error("No product was selected.", "No product was selected."));
			}

			// Loan Term Validation
			if (productKey > 0)
			{
				IRuleService validateRules = ServiceFactory.GetService<IRuleService>();
				validateRules.ExecuteRule(_view.Messages, "ApplicationProductMortgageLoanTerm", _view.Term, (Products)productKey);
			}

			if ((int)_view.MortgageLoanPurpose == 1)
				_view.Messages.Add(new Error("No loan purpose was selected.", "No loan purpose was selected."));

			if (_view.EmploymentTypeKey == 0)
				_view.Messages.Add(new Error("No employment type was selected.", "No employment type was selected."));

			switch (_view.MortgageLoanPurpose)
			{
				case MortgageLoanPurposes.Switchloan: //2: Switch loan
					if (_view.CurrentLoan <= 0)
						RunRule("SwitchCurrentLoanAmountMinimum", null);

					break;

				default:
					break;
			}

			if (productKey == (int)Products.VariFixLoan)
			{
				if (_view.LoanAmountRequired < minFixTotalLoanAmount)
					RunRule("VarifixMinimumLoanAmount", minFixTotalLoanAmount);

				if (_view.LoanAmountRequired * _view.FixPercent < minFixFinServiceAmount)
					RunRule("VarifixMinimumFixAmount", (_view.LoanAmountRequired * _view.FixPercent), minFixFinServiceAmount);
			}
		}

		protected DateTime MaturityDate(DateTime fromDate, int Term)
		{
			DateTime maturity = fromDate.AddMonths(Term + _maturityMonths);

			if (maturity.Day > 15) //get the next month
				maturity.AddMonths(1);

			// return the last of this month
			return new DateTime(maturity.Year, maturity.Month, 1).AddMonths(1).AddDays(-1);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="aivl"></param>
		protected void SetupVariableInformation(IApplicationInformationVariableLoan aivl)
		{
			if (aivl != null)
			{
				//aivl.BondToRegister = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateBondAmount(_view.LoanAmountRequired);
				aivl.CashDeposit = _view.Deposit;
				//aivl.Category = _cat;
				//aivl.CreditMatrix = _cm;
				aivl.EmploymentType = _empType;
				aivl.ExistingLoan = _view.CurrentLoan; //this should always be 0 for a new purchase loan
				//aivl.FeesTotal = _view.TotalFee;
				aivl.HouseholdIncome = _view.HouseholdIncome;
				//aivl.InterimInterest = _view.InterimInterest;
				aivl.LoanAmountNoFees = _view.LoanAmountRequired - (_view.CapitaliseFees ? _view.TotalFee : 0D);
				//aivl.LTV = _view.LTV;
				//aivl.MarketRate = _view.ActiveMarketRate;
				//aivl.MonthlyInstalment = _view.InstalmentTotal;
				aivl.PropertyValuation = _view.EstimatedPropertyValue;
				//aivl.PTI = _view.PTI;
				//aivl.RateConfiguration = CMRepo.GetRateConfigurationByMarginKeyAndMarketRateKey(_view.MarginKey, _marketRateKey);
				//_rateConfigKey = aivl.RateConfiguration.Key;
				//aivl.SPV = NewSPVFromLTV(_view.LTV);
				aivl.Term = _view.Term;
			}

			if (_view.MortgageLoanPurpose == MortgageLoanPurposes.Switchloan || _view.MortgageLoanPurpose == MortgageLoanPurposes.Refinance)
			{
				IApplicationInformationVariableLoanForSwitchAndRefinance aivlsr = (IApplicationInformationVariableLoanForSwitchAndRefinance)aivl;
				if (aivlsr != null)
					aivlsr.RequestedCashAmount = _view.CashOut;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="aiio"></param>
		protected void SetupInterestOnly(IApplicationInformationInterestOnly aiio, bool isEHL)
		{
			if (_view.InterestOnly || isEHL)
			{
				if (aiio != null)
				{
					//aiio.Installment = _view.InstalmentTotal;
					if (isEHL)
						aiio.MaturityDate = DateTime.Now.AddMonths(36);
					else
						aiio.MaturityDate = MaturityDate(DateTime.Now, _view.Term);

					IApplicationInformationFinancialAdjustment aifa = AppRepo.GetEmptyApplicationInformationFinancialAdjustment();
					aifa.Discount = 0;

					if (isEHL)
						aifa.Term = 36;
					else
						aifa.Term = -1;

					if (isEHL)
					{
						aifa.FinancialAdjustmentTypeSource = FADJRepo.GetFinancialAdjustmentTypeSourceByKey((int)FinancialAdjustmentTypeSources.Edge);
					}
					else
					{
						aifa.FinancialAdjustmentTypeSource = FADJRepo.GetFinancialAdjustmentTypeSourceByKey((int)FinancialAdjustmentTypeSources.InterestOnly);
					}

					aifa.ApplicationInformation = aiio.ApplicationInformation;
					aiio.ApplicationInformation.ApplicationInformationFinancialAdjustments.Add(_view.Messages, aifa);
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="prod"></param>
		/// <returns></returns>
		protected IApplicationProductNewVariableLoan SetupNVL(IApplicationProduct prod)
		{
			IApplicationProductNewVariableLoan nvl = prod as IApplicationProductNewVariableLoan;
			if (null != nvl)
			{
				IApplicationMortgageLoanNewPurchase npml = nvl.Application as IApplicationMortgageLoanNewPurchase;
				if (npml != null)
				{
					npml.PurchasePrice = _view.PurchasePrice;
				}

				//nvl.LoanAgreementAmount = _view.LoanAmountRequired;
				nvl.LoanAmountNoFees = _view.LoanAmountRequired;
				if (_view.CapitaliseFees)
					nvl.LoanAmountNoFees -= _view.TotalFee;

				nvl.Term = _view.Term;

				SetupVariableInformation(nvl.VariableLoanInformation);
				SetupInterestOnly(nvl.InterestOnlyInformation, false);
				return nvl;
			}
			else
				throw new InvalidCastException("Not a IApplicationProductNewVariableLoan");
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="prod"></param>
		/// <returns></returns>
		protected IApplicationProductEdge SetupEHL(IApplicationProduct prod)
		{
			IApplicationProductEdge ehl = (IApplicationProductEdge)prod;

			IApplicationMortgageLoanNewPurchase npml = ehl.Application as IApplicationMortgageLoanNewPurchase;
			if (npml != null)
			{
				npml.PurchasePrice = _view.PurchasePrice;
			}

			ehl.LoanAmountNoFees = _view.LoanAmountRequired;
			if (_view.CapitaliseFees)
				ehl.LoanAmountNoFees -= _view.TotalFee;

			SetupVariableInformation(ehl.VariableLoanInformation);
			ehl.Term = _view.EdgeTerm;
			SetupInterestOnly(ehl.InterestOnlyInformation, true);
			return ehl;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="prod"></param>
		/// <returns></returns>
		protected IApplicationProductSuperLoLoan SetupSuperLo(IApplicationProduct prod)
		{
			IApplicationProductSuperLoLoan sl = prod as IApplicationProductSuperLoLoan;
			if (null != sl)
			{
				IApplicationMortgageLoanNewPurchase npml = sl.Application as IApplicationMortgageLoanNewPurchase;
				if (npml != null)
				{
					npml.PurchasePrice = _view.PurchasePrice;
				}

				sl.LoanAmountNoFees = _view.LoanAmountRequired;
				if (_view.CapitaliseFees)
					sl.LoanAmountNoFees -= _view.TotalFee;

				sl.Term = _view.Term;

				SetupVariableInformation(sl.VariableLoanInformation);
				SetupInterestOnly(sl.InterestOnlyInformation, false);

				IApplicationInformationSuperLoLoan sli = sl.SuperLoInformation;
				double annualThreshold = _view.LoanAmountRequired * 0.04;
				sli.PPThresholdYr1 = annualThreshold;
				sli.PPThresholdYr2 = annualThreshold;
				sli.PPThresholdYr3 = annualThreshold;
				sli.PPThresholdYr4 = annualThreshold;
				sli.PPThresholdYr5 = annualThreshold;
				sli.ElectionDate = DateTime.Now;

				IApplicationInformationFinancialAdjustment slfa = AppRepo.GetEmptyApplicationInformationFinancialAdjustment();

				IMarginProduct mp = CMRepo.GetMarginProductByRateConfigAndOSP(_rateConfigKey, _os.Key, _view.ProductKey);
				if (mp != null)
					slfa.Discount = mp.Discount;
				else
					slfa.Discount = 0;

				slfa.Term = -1;
				slfa.FinancialAdjustmentTypeSource = FADJRepo.GetFinancialAdjustmentTypeSourceByKey((int)FinancialAdjustmentTypeSources.SuperLo);

				slfa.ApplicationInformation = sli.ApplicationInformation;
				sli.ApplicationInformation.ApplicationInformationFinancialAdjustments.Add(_view.Messages, slfa);

				return sl;
			}
			else
				throw new InvalidCastException("Not a IApplicationProductSuperLoLoan");
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="prod"></param>
		/// <returns></returns>
		protected IApplicationProductVariFixLoan SetupVariFix(IApplicationProduct prod)
		{
			IApplicationProductVariFixLoan vfl = prod as IApplicationProductVariFixLoan;
			if (null != vfl)
			{
				IApplicationMortgageLoanNewPurchase npml = vfl.Application as IApplicationMortgageLoanNewPurchase;
				if (npml != null)
				{
					npml.PurchasePrice = _view.PurchasePrice;
				}
				//vfl.LoanAgreementAmount = _view.LoanAmountRequired;
				vfl.LoanAmountNoFees = _view.LoanAmountRequired;
				if (_view.CapitaliseFees)
					vfl.LoanAmountNoFees -= _view.TotalFee;
				vfl.Term = _view.Term;

				SetupVariableInformation(vfl.VariableLoanInformation);

				IApplicationInformationVarifixLoan aivf = vfl.VariFixInformation;
				//aivf.FixedInstallment = _view.InstalmentFix;
				aivf.FixedPercent = _view.FixPercent;
				aivf.ElectionDate = DateTime.Now;
				aivf.MarketRate = LookupRepo.MarketRates.ObjectDictionary[_view.VarifixMarketRateKey.ToString()];

				return vfl;
			}
			else
				throw new InvalidCastException("Not a IApplicationProductVariFixLoan");
		}

		protected ILookupRepository LookupRepo
		{
			get
			{
				if (_lookupRepo == null)
					_lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

				return _lookupRepo;
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

		protected IControlRepository CtrlRepo
		{
			get
			{
				if (_ctrlRepo == null)
					_ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();

				return _ctrlRepo;
			}
		}

		protected ICreditMatrixRepository CMRepo
		{
			get
			{
				if (_cmRepo == null)
					_cmRepo = RepositoryFactory.GetRepository<ICreditMatrixRepository>();

				return _cmRepo;
			}
		}

		protected IX2Repository X2Repo
		{
			get
			{
				if (_x2Repo == null)
					_x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

				return _x2Repo;
			}
		}

		protected IFinancialAdjustmentRepository FADJRepo
		{
			get
			{
				if (_fadjRepo == null)
				{
					_fadjRepo = RepositoryFactory.GetRepository<IFinancialAdjustmentRepository>();
				}

				return _fadjRepo;
			}
		}

		private IRuleService RuleServ
		{
			get
			{
				if (_ruleServ == null)
					_ruleServ = ServiceFactory.GetService<IRuleService>();

				return _ruleServ;
			}
		}

		private void RunRule(string Rule, params object[] prms)
		{
			RuleServ.ExecuteRule(_view.Messages, Rule, prms);
		}
	}
}