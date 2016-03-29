using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.Web.UI;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.CacheData;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using System.Linq;
using System.Collections.Generic;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Origination.Presenters
{
	public class ApplicationLoanDetailsDeclineCounterRateExceptionAction : ApplicationLoanDetailsBase
	{
		/// <summary>
		/// Initializes a new ApplicationLoanDetailsDeclineCounterRateExceptionAction
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public ApplicationLoanDetailsDeclineCounterRateExceptionAction(IApplicationLoanDetails view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
			_view.OnUpdateClicked += new EventHandler(OnUpdateRevisionClicked);
		}

		/// <summary>
		/// On View Loaded
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewLoaded(object sender, EventArgs e)
		{
			base.OnViewLoaded(sender, e);
			if (!_view.ShouldRunPage)
				return;
			// Hide the readonly controls and show the editable ones
			_view.IsQuickPayFeeReadOnly = false;
			_view.IsReadOnly = true;
			_view.IsPropertyValueReadOnly = true;
			_view.IsQuickCashDetailsReadOnly = false;
			_view.IsButtonsPanelReadonly = false;
			_view.IsSPVReadOnly = true;
			_view.IsCashDepositReadOnly = true;
			//This presenter enables the Bond to Register and allows it to be the 
			//same as the LAA
			_view.IsBondToRegisterExceptionAction = false;

			// QC not available on New Purchases
			if (Application.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.NewPurchaseLoan)
				_view.IsQuickCashVisible = false;
			else
				_view.IsQuickCashVisible = true;

			//except for VF, discount must be editable
			//if (Application.CurrentProduct.ProductType != SAHL.Common.Globals.Products.VariFixLoan)
			_view.IsDiscountReadonly = true;

			_view.IsCashDepositReadOnly = true;
			_view.IsCashOutReadOnly = true;
			_view.CanShowQuickCash = true;

			_view.IsLinkRateReadOnly = true;
			_view.IsQuickPayFeeReadOnly = true;

			_view.ShowRateAdjustment = true;

			ApplyRateAdjustment();
			//Bind the Application Details
			_view.BindApplicationDetails(Application);
		}

		/// <summary>
		/// On View Initialized
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			//Ge the Application from the CBO 
			GetApplicationFromCBO();

			BindLookups();

			//Run the Required rule
			//Get the Rate Adjustment Elements
			IApplicationInformation applicationInformation = Application.GetLatestApplicationInformation();
			Dictionary<int, string> rateAdjustmentElements = GetRateAdjustmentsForApplication(Application);
			int? selectedRateAdjustmentElementKey = null;
			if (rateAdjustmentElements.Count > 0)
			{
				//Get the Application Rate Override
				var applicationRateOverride = (from financialAdjustment in applicationInformation.ApplicationInformationFinancialAdjustments
											   where financialAdjustment.FinancialAdjustmentTypeSource.Key == (int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.CounterRate
											   select financialAdjustment).FirstOrDefault();
				//Get the Application Rate Adjustment
				if (applicationRateOverride != null)
				{
					var appInfoRateAdjustment = (from applicationInformationRateAdjustment in applicationRateOverride.ApplicationInformationAppliedRateAdjustments
												 select applicationInformationRateAdjustment).FirstOrDefault();
					selectedRateAdjustmentElementKey = appInfoRateAdjustment.RateAdjustmentElement.Key;
				}
				//Working on the assumption that we can only have one counter rate
				_view.BindRateAdjustments(rateAdjustmentElements, selectedRateAdjustmentElementKey);
			}

			//None of the Rate Adjustments can be applied, display a warning message to the user and disable the buttons
			//The first item in the list is the "None"
			if (rateAdjustmentElements.Count == 1)
			{
				if (selectedRateAdjustmentElementKey.HasValue)
				{
					_view.IsSubmitButtonEnabled = true;
					_view.IsRecalculateButtonEnabled = true;
				}
				else
				{
					_view.IsSubmitButtonEnabled = false;
					_view.IsRecalculateButtonEnabled = false;
				}
				_view.Messages.Add(new Warning("This application does not qualify for any rate adjustments", "This application does not qualify for any rate adjustments"));
			}

			_view.BindProduct(base.LookupRepository.Products.BindableDictionary[Convert.ToString((int)Application.CurrentProduct.ProductType)]);

			base.OnViewInitialised(sender, e);

			if (!_view.ShouldRunPage)
				return;
		}

		/// <summary>
		/// Get Rate Adjustments for Application
		/// </summary>
		/// <param name="application"></param>
		/// <returns></returns>
		private Dictionary<int, string> GetRateAdjustmentsForApplication(IApplication application)
		{
			IRateAdjustmentGroup rateAdjustmentGroup = _applicationRepository.GetRateAdjustmentGroupByKey((int)SAHL.Common.Globals.RateAdjustmentGroups.CounterRate);
			IRuleService ruleService = ServiceFactory.GetService<IRuleService>();
			Dictionary<int, string> rateAdjustmentElements = new Dictionary<int, string>();
			//Please note, that before you remove this element, please ensure that the logic is amended
			rateAdjustmentElements.Add(-1, "None");

			if (rateAdjustmentGroup != null)
			{
				int rulePassed = 1;

				//Run the rules for the Rate Adjustments to check if the application qualifies for concessions
				foreach (IRateAdjustmentElement rateAdjustmentElement in rateAdjustmentGroup.ActiveRateAdjustmentElements)
				{
					rulePassed = ruleService.ExecuteRule(_view.Messages, rateAdjustmentElement.RuleItem.Name, application, rateAdjustmentElement);
					if (rulePassed > 0)
					{
						rateAdjustmentElements.Add(rateAdjustmentElement.Key, String.Format("({0}) {1}", rateAdjustmentElement.RateAdjustmentElementType.Description, rateAdjustmentElement.RateAdjustmentValue.ToString(SAHL.Common.Constants.RateFormat)));
					}
				}
			}
			return rateAdjustmentElements;
		}

		/// <summary>
		/// On Update Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnUpdateClicked(object sender, EventArgs e)
		{
			ApplyRateAdjustment();
			base.OnUpdateClicked(sender, e);
		}

		/// <summary>
		/// On Recalculate Application
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnRecalculateApplication(object sender, EventArgs e)
		{
			ApplyRateAdjustment();
			base.OnRecalculateApplication(sender, e);
		}

		/// <summary>
		/// On Rate Adjustment Selection Changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnRateAdjustmentSelectionChanged(object sender, EventArgs e)
		{
			ApplyRateAdjustment();
			base.OnRecalculateApplication(sender, e);
		}

		/// <summary>
		/// Apply Rate Adjustment
		/// </summary>
		private void ApplyRateAdjustment()
		{
			if (_view.SelectedRateAdjustmentKey == null)
			{
				return;
			}
			ISecurityRepository securityRepository = RepositoryFactory.GetRepository<ISecurityRepository>();

			IApplicationInformation applicationInformation = Application.GetLatestApplicationInformation();

			//Get the Counter Rate Adjustments Group
			IRateAdjustmentGroup rateAdjustmentGroup = _applicationRepository.GetRateAdjustmentGroupByKey((int)SAHL.Common.Globals.RateAdjustmentGroups.CounterRate);

			// Remove all the Rate Adjustments
			foreach (var rateAdjustmentElement in rateAdjustmentGroup.RateAdjustmentElements)
			{
				foreach (var applicationInformationFinancialAdjustment in applicationInformation.ApplicationInformationFinancialAdjustments)
				{
					if (applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource.Key == rateAdjustmentElement.FinancialAdjustmentTypeSource.Key)
					{
						applicationInformation.ApplicationInformationFinancialAdjustments.Remove(_view.Messages, applicationInformationFinancialAdjustment);
					}
				}
			}

			if (_view.SelectedRateAdjustmentKey > 0)
			{
				//Get the Rate Adjustment Element To Add
				var rateAdjustmentElementToAdd = (from rateAdjustment in rateAdjustmentGroup.ActiveRateAdjustmentElements
												  where rateAdjustment.Key == _view.SelectedRateAdjustmentKey
												  select rateAdjustment).First();

				//Add the Rate Override
				var applicationInformationFinancialAdjustment = _applicationRepository.GetEmptyApplicationInformationFinancialAdjustment();
				applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource = rateAdjustmentElementToAdd.FinancialAdjustmentTypeSource;
				applicationInformationFinancialAdjustment.Discount = rateAdjustmentElementToAdd.RateAdjustmentValue;
				applicationInformationFinancialAdjustment.Term = -1;
				applicationInformationFinancialAdjustment.FromDate = DateTime.Now;

				//2 Way Binding
				applicationInformationFinancialAdjustment.ApplicationInformation = applicationInformation;
				applicationInformation.ApplicationInformationFinancialAdjustments.Add(_view.Messages, applicationInformationFinancialAdjustment);

				//Add the Rate Adjustment
				var applicationInformationAppliedRateAdjustment = _applicationRepository.GetEmptyApplicationInformationAppliedRateAdjustment();
				applicationInformationAppliedRateAdjustment.ApplicationElementValue = rateAdjustmentElementToAdd.RateAdjustmentValue.ToString();
				applicationInformationAppliedRateAdjustment.RateAdjustmentElement = rateAdjustmentElementToAdd;
				applicationInformationAppliedRateAdjustment.ADUser = securityRepository.GetADUserByPrincipal(SAHLPrincipal.GetCurrent());
				applicationInformationAppliedRateAdjustment.ChangeDate = DateTime.Now;

				//2 Way Binding
				applicationInformationAppliedRateAdjustment.ApplicationInformationFinancialAdjustment = applicationInformationFinancialAdjustment;
				applicationInformationFinancialAdjustment.ApplicationInformationAppliedRateAdjustments.Add(_view.Messages, applicationInformationAppliedRateAdjustment);
			}
		}
	}
}
