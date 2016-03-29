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
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Globals;
using SAHL.Web.AJAX;
using SAHL.Common.Factories;
using System.Text;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Origination
{
	public partial class ApplicationWizardCalculator : SAHLCommonBaseView, IApplicationWizardCalculator
	{
		private double _bondRequired;
		private int _productKey;
		private ILegalEntityNaturalPerson _le;
		private IApplication _application;
		bool _callStartupScript;

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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!ShouldRunPage)
				return;
			else
				base.RegisterWebService(ServiceConstants.LegalEntity);

			// We need to take into account validation on this so I will register this script after the events have
			// fired so we know whether to render the Next button as ReadOnly and allowed to be clicked.
			// Moved to PreRender.
			//if (_callStartupScript)
			//{
			//    RegisterUpdateStartupScript();
			//}
		}

		private void RegisterUpdateStartupScript()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine("setupLoanPurpose();");
			sb.AppendLine("SetupProductDisplay();");
			if (!IsPostBack || ddlNeedsIdentification.SelectedIndex < 1 || ddlProduct.SelectedIndex < 1 || !IsValid)
			{
				sb.AppendLine("SetCreateApplication(false,false);");
			}
			else
			{
				sb.AppendLine("SetCreateApplication(true,false);");
			}

			if (!Page.ClientScript.IsClientScriptBlockRegistered("MyOnLoad"))
				Page.ClientScript.RegisterStartupScript(this.GetType(), "MyOnLoad", sb.ToString(), true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (!ShouldRunPage) return;

			if (_callStartupScript)
			{
				RegisterUpdateStartupScript();
			}

			lblFirstNames.Text = _le.FirstNames;
			if (MortgageLoanApplication.ApplicationSource != null)
				lblMarketingSource.Text = MortgageLoanApplication.ApplicationSource.Description;
			lblIdentityNo.Text = _le.IDNumber;
			lblSurname.Text = _le.Surname;
			lblContact.Text = _le.HomePhoneCode + " - " + _le.HomePhoneNumber;
			lblNumApplicants.Text = MortgageLoanApplication.EstimateNumberApplicants.Value.ToString();

			switch (MortgageLoanPurpose)
			{
				case MortgageLoanPurposes.Switchloan: //2: Switch loan
					rowPurchasePrice.Attributes.Add("style", "display: none");
					rowCashDeposit.Attributes.Add("style", "display: none");
					rowCashRequired.Attributes.Add("style", "display: none");

					break;
				case MortgageLoanPurposes.Newpurchase: //3: New purchase
					rowPurchasePrice.Attributes.Add("style", "display: inline");
					rowCashDeposit.Attributes.Add("style", "display: inline");
					rowMarketValue.Attributes.Add("style", "display: none");
					rowCurrentLoan.Attributes.Add("style", "display: none");
					rowCashOut.Attributes.Add("style", "display: none");
					rowCapitaliseFees.Attributes.Add("style", "display: none");
					rowCashRequired.Attributes.Add("style", "display: none");
					rowInterimInterest.Visible = false;

					break;
				case MortgageLoanPurposes.Refinance: //4: Refinance
					rowCurrentLoan.Attributes.Add("style", "display: none");
					rowCashOut.Attributes.Add("style", "display: none");
					rowPurchasePrice.Attributes.Add("style", "display: none");
					rowCashDeposit.Attributes.Add("style", "display: none");
					rowInterimInterest.Visible = false;

					break;
				default:
					rowMarketValue.Attributes.Add("style", "display: none");
					rowCurrentLoan.Attributes.Add("style", "display: none");
					rowCashOut.Attributes.Add("style", "display: none");
					rowCapitaliseFees.Attributes.Add("style", "display: none");
					rowPurchasePrice.Attributes.Add("style", "display: none");
					rowCashDeposit.Attributes.Add("style", "display: none");
					rowCashRequired.Attributes.Add("style", "display: none");
					rowInterimInterest.Visible = false;

					break;
			}

			switch (ProductKey)
			{
				case (int)Products.VariFixLoan: //"2": //VariFix
					rowVarifix.Attributes.Add("style", "display: inline");
					rowVariFixReset.Attributes.Add("style", "display: inline");
					//rowInterestOnly.Attributes.Add("style", "display: none");
					tblStandard.Visible = false;
					rowPTIStandard.Visible = false;
					break;
				case (int)Products.Edge: //"11": //Edge
					rowVarifix.Attributes.Add("style", "display: none");
					rowVariFixReset.Attributes.Add("style", "display: none");
					//rowInterestOnly.Attributes.Add("style", "display: none");
					rowEHLIOInstal.Attributes.Add("style", "display: inline");
					rowEHLAMInstal.Attributes.Add("style", "display: inline");
					rowEHLAMInstalFull.Attributes.Add("style", "display: inline");
					rowVarInstal.Attributes.Add("style", "display: none");
					tbLoanTerm.Text = EdgeTerm.ToString();
					tbLoanTerm.Enabled = false;
					tblVariFix.Visible = false;
					rowPTIStdForFix.Visible = false;
					rowPTIFix.Visible = false;
					break;
				default: // 9 NewVariable : 5 Super Lo
                    //if (ddlNeedsIdentification.SelectedValue != "-select-" && ddlProduct.SelectedValue != "-select-")
                    //{
                    //    rowInterestOnly.Attributes.Add("style", "display: inline");
                    //}
					rowEHLIOInstal.Attributes.Add("style", "display: none");
					rowEHLAMInstal.Attributes.Add("style", "display: none");
					rowEHLAMInstalFull.Attributes.Add("style", "display: none");
					rowVarInstal.Attributes.Add("style", "display: inline");
					rowVarifix.Attributes.Add("style", "display: none");
					rowVariFixReset.Attributes.Add("style", "display: none");
					tblVariFix.Visible = false;
					rowPTIStdForFix.Visible = false;
					rowPTIFix.Visible = false;
					break;
			}

			if (chkCapitaliseFees.Checked && MortgageLoanPurpose != MortgageLoanPurposes.Newpurchase)
			{
				lblFeeInfo.Text = "** Total Fees included because you elected to have them capitalised.";
				lblFeeInfoFix.Text = "** Total Fees included because you elected to have them capitalised.";
				lblFeeInfoInd.Text = "**";
				lblFeeInfoIndFix.Text = "**";
			}
			else
			{
				lblFeeInfo.Text = "";
				lblFeeInfoFix.Text =
				lblFeeInfoInd.Text = "";
				lblFeeInfoIndFix.Text = "";
			}

			if (ddlNeedsIdentification.SelectedValue == "-select-" && ddlProduct.SelectedValue == "-select-")
			{
				rowProduct.Attributes.Add("style", "display:none");
			}
			if (ddlNeedsIdentification.SelectedValue == "-select-" || ddlProduct.SelectedValue == "-select-")
			{
				rowCapitaliseFees.Attributes.Add("style", "display:none");
				//rowInterestOnly.Attributes.Add("style", "display:none");
			}

			tbPurchasePrice.Attributes.Add("onblur", "updateVal(" + tbPurchasePrice.ClientID + ")");
			tbMarketValue.Attributes.Add("onblur", "updateVal(" + tbMarketValue.ClientID + ")");
			tbCashOut.Attributes.Add("onblur", "updateVal(" + tbCashOut.ClientID + ")");
			tbCashRequired.Attributes.Add("onblur", "updateVal(" + tbCashRequired.ClientID + ")");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnCreateApplication(object sender, EventArgs e)
		{
			OnCreateApplicationButtonClicked(sender, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnCalculate_Click(object sender, EventArgs e)
		{
			OnCalculateButtonClicked(sender, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnCancel_Click(object sender, EventArgs e)
		{
			OnCancelButtonClicked(sender, e);
		}

		protected void btnBack_Click(object sender, EventArgs e)
		{
			OnBackButtonClicked(sender, e);
		}

		#region IApplicationWizardCalculator Members

		#region methods and events

		/// <summary>
		/// Bind the Product Dropdown
		/// </summary>
		/// <param name="listProducts"></param>
		public void BindProductDropdown(ReadOnlyEventList<IProduct> listProducts)
		{
			ddlProduct.DataValueField = "Key";
			ddlProduct.DataTextField = "Description";
			ddlProduct.DataSource = listProducts;
			ddlProduct.DataBind();
		}

		/// <summary>
		/// Bind the Product Dropdown
		/// </summary>
		/// <param name="listPurpose"></param>
		public void BindPurposeDropdown(ReadOnlyEventList<IMortgageLoanPurpose> listPurpose)
		{
			ddlPurpose.DataValueField = "Key";
			ddlPurpose.DataTextField = "Description";
			ddlPurpose.DataSource = listPurpose;
			ddlPurpose.DataBind();
		}

		/// <summary>
		/// Bind the Employment type dropdown
		/// </summary>
		/// <param name="listEmploymentType"></param>
		public void BindEmploymentType(IEventList<IEmploymentType> listEmploymentType)
		{
			ddlEmploymentType.DataSource = listEmploymentType.BindableDictionary;
			ddlEmploymentType.DataBind();

			// remove the 'Unknown' employment type
			ListItem li = ddlEmploymentType.Items.FindByValue(Convert.ToString((int)SAHL.Common.Globals.EmploymentTypes.Unknown));
			if (li != null)
				ddlEmploymentType.Items.Remove(li);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="application"></param>
		public void BindControls(IApplication application)
		{

            //foreach (IFinancialAdjustment fa in application.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments)
            //{
            //    if (fa.FinancialAdjustmentTypeSource.Key == (int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.InterestOnly)
            //    {
            //        chkInterestOnly.Checked = true;

            //        break;
            //    }
            //}

			int _mlp = -1;
			switch (application.ApplicationType.Key)
			{
				case (int)OfferTypes.FurtherAdvance:
					_mlp = (int)MortgageLoanPurposes.Switchloan;
					break;
				case (int)OfferTypes.FurtherLoan:
					_mlp = (int)MortgageLoanPurposes.Switchloan;
					break;
				case (int)OfferTypes.Life:
					throw new NotSupportedException("This is not a mortgage loan.");
				case (int)OfferTypes.NewPurchaseLoan:
					_mlp = (int)MortgageLoanPurposes.Newpurchase;
					break;
				case (int)OfferTypes.ReAdvance:
					_mlp = (int)MortgageLoanPurposes.ReAdvance;
					break;
				case (int)OfferTypes.RefinanceLoan:
					_mlp = (int)MortgageLoanPurposes.Refinance;
					break;
				case (int)OfferTypes.SwitchLoan:
					_mlp = (int)MortgageLoanPurposes.Switchloan;
					break;
				default:
					throw new NotSupportedException("This application type is not supported.");
			}
			if (_mlp != -1)
			{
				for (int x = ddlPurpose.Items.Count - 1; x >= 0; x--)
				{
					if (ddlPurpose.Items[x].Value.ToString() != _mlp.ToString())
					{
						ddlPurpose.Items.RemoveAt(x);
					}
				}

				for (int x = 0; x < ddlPurpose.Items.Count; x++)
				{
					if (ddlPurpose.Items[x].Value.ToString() == _mlp.ToString())
					{
						ddlPurpose.SelectedIndex = x;
						break;
					}
				}
			}

			if (application.GetLatestApplicationInformation() != null)
			{
				IProduct prod = application.GetLatestApplicationInformation().Product;
				for (int y = 0; y < ddlProduct.Items.Count; y++)
				{
					if (ddlProduct.Items[y].Value == prod.Key.ToString())
					{
						ddlProduct.SelectedIndex = y;
						break;
					}
				}
			}

			ISupportsVariableLoanApplicationInformation supports = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
			if (supports != null)
			{
				for (int x = 0; x < ddlEmploymentType.Items.Count; x++)
				{
					if (supports.VariableLoanInformation.EmploymentType.Key.ToString() == ddlEmploymentType.Items[x].Value)
					{
						ddlEmploymentType.SelectedIndex = x;
						break;
					}
				}
			}



			ISupportsSuperLoApplicationInformation supportsSuperLo = application.CurrentProduct as ISupportsSuperLoApplicationInformation;
			if (supportsSuperLo != null)
			{

			}

			IApplicationInformationVariableLoanForSwitchAndRefinance info = supports.VariableLoanInformation as IApplicationInformationVariableLoanForSwitchAndRefinance;
			if (info != null)
			{
				if (info.RequestedCashAmount != null)
				{

					tbCashOut.Text = info.RequestedCashAmount.Value.ToString();
					tbCashRequired.Text = info.RequestedCashAmount.Value.ToString();
				}
			}

			ISupportsVariFixApplicationInformation supportsVarifix = application.CurrentProduct as ISupportsVariFixApplicationInformation;
			if (supportsVarifix != null)
			{
				tbFixPercentage.Text = (supportsVarifix.VariFixInformation.FixedPercent * 100).ToString();
				// Not required since only the 5 Yr option available
				/*
				if (supportsVarifix.VariFixInformation.MarketRate.Key == (int)SAHL.Common.Globals.MarketRates.TwentyYearFixedMortgageRate5yr)
				{
					chkVFReset5Year.Checked = true;
					chkVFReset6Month.Checked = false;
				}
				if (supportsVarifix.VariFixInformation.MarketRate.Key == (int)SAHL.Common.Globals.MarketRates.TwentyYearFixedMortgageRate6Month)
				{
					chkVFReset6Month.Checked = true;
					chkVFReset5Year.Checked = false;
				}
				*/
			}

			tbHouseholdIncome.Text = application.GetHouseHoldIncome().ToString();
			IApplicationMortgageLoanNewPurchase mlNP = application as IApplicationMortgageLoanNewPurchase;
			if (mlNP != null)
			{
				tbPurchasePrice.Text = mlNP.PurchasePrice.Value.ToString();
			}
			tbCashDeposit.Text = supports.VariableLoanInformation.CashDeposit.Value.ToString();
			tbMarketValue.Text = supports.VariableLoanInformation.PropertyValuation.Value.ToString();
			tbCurrentLoan.Text = supports.VariableLoanInformation.ExistingLoan.Value.ToString();
			tbLoanTerm.Text = supports.VariableLoanInformation.Term.Value.ToString();

			IApplicationMortgageLoanSwitch mlSwitch = application as IApplicationMortgageLoanSwitch;
			if (mlSwitch != null)
			{
				chkCapitaliseFees.Checked = mlSwitch.CapitaliseFees;
				this.CancellationFee = mlSwitch.CancellationFee;
			}

			IApplicationMortgageLoanRefinance mlRefinance = application as IApplicationMortgageLoanRefinance;
			if (mlRefinance != null)
			{
				chkCapitaliseFees.Checked = mlRefinance.CapitaliseFees;
			}

			IReasonRepository RR = RepositoryFactory.GetRepository<IReasonRepository>();
			IReadOnlyEventList<IReason> lstReasons = RR.GetReasonByGenericTypeAndKey((int)SAHL.Common.Globals.GenericKeyTypes.Offer, application.Key);
			IReason r = null;
			if (lstReasons.Count > 0)
				r = lstReasons[0];
			if (r != null)
			{
				for (int x = 0; x < ddlNeedsIdentification.Items.Count; x++)
				{
					if (ddlNeedsIdentification.Items[x].Value == r.ReasonDefinition.Key.ToString())
					{
						ddlNeedsIdentification.SelectedIndex = x;
						break;
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="DictDefs"></param>
		public void BindNeedsIdentificationDropdown(IDictionary DictDefs)
		{
			ddlNeedsIdentification.DataSource = DictDefs;
			ddlNeedsIdentification.DataBind();
		}


		/// <summary>
		/// Handles the calculate button click
		/// </summary>
		public event EventHandler OnCalculateButtonClicked;

		/// <summary>
		/// 
		/// </summary>
		public event EventHandler OnCancelButtonClicked;

		/// <summary>
		/// 
		/// </summary>
		public event EventHandler OnBackButtonClicked;

		/// <summary>
		/// Handles the create application button click
		/// </summary>
		public event EventHandler OnCreateApplicationButtonClicked;

		#endregion

		#region Properties

		#region user inputs



		/// <summary>
		/// The selected product key
		/// </summary>
		public int ProductKey
		{
			get
			{
				//this is a nasty little hack to get the application to appear the way business want
				//it to work without the extra dev overhead and technical debt
				int prodKey = Convert.ToInt16(ddlProduct.SelectedValue == "-select-" ? "0" : ddlProduct.SelectedValue);
				if (prodKey != 0)
					_productKey = prodKey;
				return _productKey;
			}
			set { _productKey = value; }
		}

		/// <summary>
		/// The Loan Purpose Key 
		/// MortgageLoanPurposeKey	Description
		/// 2	Switch loan
		/// 3	New purchase
		/// 4	Refinance
		/// </summary>
		public MortgageLoanPurposes MortgageLoanPurpose
		{
			get { return (MortgageLoanPurposes)Enum.ToObject(typeof(MortgageLoanPurposes), Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "1" : ddlPurpose.SelectedValue)); }
		}

		/// <summary>
		/// The employment type key
		/// </summary>
		public int EmploymentTypeKey
		{
			get { return Convert.ToInt16(ddlEmploymentType.SelectedValue == "-select-" ? "0" : ddlEmploymentType.SelectedValue); }
		}

		/// <summary>
		/// Market value of the property
		/// </summary>
		public double EstimatedPropertyValue
		{
			get
			{
				if (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) == (int)MortgageLoanPurposes.Newpurchase)
					return tbPurchasePrice.Text.Length > 0 ? Convert.ToDouble(tbPurchasePrice.Text) : 0;
				else
					return tbMarketValue.Text.Length > 0 ? Convert.ToDouble(tbMarketValue.Text) : 0;
			}
		}

		/// <summary>
		/// Deposit to pay for a new purchase
		/// </summary>
		public double Deposit
		{
			get
			{
				if (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) == (int)MortgageLoanPurposes.Newpurchase && tbCashDeposit.Text.Length > 0)
					return Convert.ToDouble(tbCashDeposit.Text);
				else
					return 0;
			}
		}

		/// <summary>
		/// Existing loan amount with another provider for switch loans
		/// </summary>
		public double CurrentLoan
		{
			get
			{
				if (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) == (int)MortgageLoanPurposes.Switchloan && tbCurrentLoan.Text.Length > 0)
					return Convert.ToDouble(tbCurrentLoan.Text);
				else
					return 0;
			}
		}

		/// <summary>
		/// Cash value required by the client for switch and refinance
		/// </summary>
		public double CashOut
		{
			get
			{
				if (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) == (int)MortgageLoanPurposes.Switchloan && tbCashOut.Text.Length > 0)
					return Convert.ToDouble(tbCashOut.Text);

				if (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) == (int)MortgageLoanPurposes.Refinance && tbCashRequired.Text.Length > 0)
					return Convert.ToDouble(tbCashRequired.Text);

				return 0;
			}
		}

		/// <summary>
		/// The term of the Loan in months
		/// </summary>
		public Int16 Term
		{
			get { return tbLoanTerm.Text.Length > 0 ? Convert.ToInt16(tbLoanTerm.Text) : (Int16)0; }
		}

		/// <summary>
		/// Should fees be capitalised to the Loan amount for switch and refinance
		/// </summary>
		public bool CapitaliseFees
		{
			get
			{
				if (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) != (int)MortgageLoanPurposes.Newpurchase)
					return chkCapitaliseFees.Checked;

				return false;
			}
		}

		/// <summary>
		/// Indicates interest only mortgage loan application
		/// </summary>
		public bool InterestOnly
		{
			get
			{
                //if (Convert.ToInt16(ddlProduct.SelectedValue == "-select-" ? "0" : ddlProduct.SelectedValue) != (int)Products.VariFixLoan)
                //    return chkInterestOnly.Checked;

				return false;

			}
		}

		/// <summary>
		/// Total income of the household occupants
		/// </summary>
		public double HouseholdIncome
		{
			get { return tbHouseholdIncome.Text.Length > 0 ? Convert.ToDouble(tbHouseholdIncome.Text) : 0; }
		}

		/// <summary>
		/// Loan amount required
		/// </summary>
		public double LoanAmountRequired
		{
			get
			{
				switch (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue))
				{
					case (int)MortgageLoanPurposes.Newpurchase:
						_bondRequired = (tbPurchasePrice.Text.Length > 0 ? Convert.ToDouble(tbPurchasePrice.Text) : 0) - (tbCashDeposit.Text.Length > 0 ? Convert.ToDouble(tbCashDeposit.Text) : 0);
						break;
					case (int)MortgageLoanPurposes.Switchloan:
						_bondRequired = (tbCurrentLoan.Text.Length > 0 ? Convert.ToDouble(tbCurrentLoan.Text) : 0) + (tbCashOut.Text.Length > 0 ? Convert.ToDouble(tbCashOut.Text) : 0) + (InterimInterest);
						if (chkCapitaliseFees.Checked)
							_bondRequired += TotalFee;
						break;
					case (int)MortgageLoanPurposes.Refinance:
						_bondRequired = tbCashRequired.Text.Length > 0 ? Convert.ToDouble(tbCashRequired.Text) : 0;
						if (chkCapitaliseFees.Checked)
							_bondRequired += TotalFee;
						break;
					default:
						_bondRequired = 0;
						break;
				}

				return _bondRequired;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double PurchasePrice
		{
			get { return tbPurchasePrice.Text.Length > 0 ? Convert.ToDouble(tbPurchasePrice.Text) : 0; ; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int VarifixMarketRateKey
		{
			get
			{
				if (chkVFReset5Year.Checked)
					return (int)MarketRates.FiveYearResetFixedMortgageRate;

				return (int)MarketRates.TwentyYearFixedMortgageRate;
			}
		}
		#endregion

		#region results

		/// <summary>
		/// 
		/// </summary>
		public double LTV
		{
			get { return Convert.ToDouble(tbLTV.Text); }
			set
			{
				lblLTV.Text = value.ToString(SAHL.Common.Constants.RateFormat);
				tbLTV.Text = value.ToString(); //for getter
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double PTI
		{
			get { return Convert.ToDouble(tbPTI.Text); }
			set
			{
				lblPTI.Text = value.ToString(SAHL.Common.Constants.RateFormat);
				lblVarPTI.Text = value.ToString(SAHL.Common.Constants.RateFormat);
				tbPTI.Text = value.ToString(); //to get again
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double PTIFix
		{
			set { lblFixPTI.Text = value.ToString(SAHL.Common.Constants.RateFormat); }
		}

		/// <summary>
		/// 
		/// </summary>
		public double LoanAmountTotal
		{
			set
			{
				lblSAHLTotLoan.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
				lblTotalFixLoan.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
			}
		}

		public int MarginKey
		{
			get { return Convert.ToInt16(tbMarginKey.Text); }
			set { tbMarginKey.Text = value.ToString(); }
		}

		/// <summary>
		/// 
		/// </summary>
		public double ActiveMarketRate
		{
			get { return Convert.ToDouble(tbActiveMarketRate.Text); }
			set { tbActiveMarketRate.Text = value.ToString(); }
		}

		public double LinkRate
		{
			get { return Convert.ToDouble(tbLinkRate.Text); }
			set { tbLinkRate.Text = value.ToString(); }
		}

		/// <summary>
		/// 
		/// </summary>
		public double InterestRate
		{
			set
			{
				lblSAHLIntRate.Text = value.ToString(SAHL.Common.Constants.RateFormat);
				lblVarRate.Text = value.ToString(SAHL.Common.Constants.RateFormat);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double InterestRateFix
		{
			set { lblFixRate.Text = value.ToString(SAHL.Common.Constants.RateFormat); }
		}

		/// <summary>
		/// 
		/// </summary>
		public double InstalmentTotal
		{
			get { return Convert.ToDouble(tbInstalmentTotal.Text); }
			set
			{
				lblSAHLMonthlyInst.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
				lblTotFixMonthlyInst.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
				lblAMEHLInstFull.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
				tbInstalmentTotal.Text = value.ToString(); // for getter
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double InstalmentIOTotal
		{
			get { return Convert.ToDouble(lblIOSAHLMonthlyInst.Text); }
			set
			{
				lblIOSAHLMonthlyInst.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
				lblIOEHLInst.Text = lblIOSAHLMonthlyInst.Text;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double InstalmentEHLAM
		{
			get { return Convert.ToDouble(lblAMEHLInst.Text); }
			set
			{
				lblAMEHLInst.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double InstalmentFix
		{
			get { return Convert.ToDouble(tbInstalmentFix.Text); }
			set
			{
				lblFixMonthlyInst.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
				tbInstalmentFix.Text = value.ToString();

			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double InstalmentVar
		{
			set { lblVarMonthlyInst.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); }
		}

		/// <summary>
		/// 
		/// </summary>
		public double FinanceChargesTotal
		{
			set
			{
				lblSAHLIntOverTerm.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
				lblTotFixIntPaidTerm.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double FinanceChargesIOTotal
		{
			set
			{
				lblIOSAHLIntOverTerm.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double FinanceChargesFix
		{
			set { lblIntPaidTermFix.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); }
		}

		/// <summary>
		/// 
		/// </summary>
		public double FinanceChargesVar
		{
			set { lblIntPaidTermVar.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); }
		}

		/// <summary>
		/// 
		/// </summary>
		public double CancellationFee
		{
			get { return Convert.ToDouble(tbCancellationFee.Text); }
			set
			{
				tbCancellationFee.Text = value.ToString();
				lblCancellationFee.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double RegistrationFee
		{
			get { return Convert.ToDouble(tbRegistrationFee.Text); }
			set
			{
				tbRegistrationFee.Text = value.ToString();
				lblRegFee.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double InitiationFee
		{
			get { return Convert.ToDouble(tbInitiationFee.Text); }
			set
			{
				tbInitiationFee.Text = value.ToString();
				lblBondPrepFee.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int NeedsIdentificationKey
		{
			get { return int.Parse(ddlNeedsIdentification.SelectedValue); }
		}


		/// <summary>
		/// 
		/// </summary>
		public double TotalFee
		{
			get
			{
				if (string.IsNullOrEmpty(tbTotalFees.Text))
					return 0;
				else
					return Convert.ToDouble(tbTotalFees.Text);
			}
			set
			{
				lblTotalFees.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
				tbTotalFees.Text = value.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double InterimInterest
		{
			get
			{
				if (MortgageLoanPurpose == MortgageLoanPurposes.Switchloan)
					return Convert.ToDouble(tbInterimInterest.Text.Length > 0 ? tbInterimInterest.Text : "0");

				return 0;
			}
			set
			{
				lblInterimIntProv.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
				tbInterimInterest.Text = value.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IncomeSufficient
		{
			set
			{
				if (value == false)
					lblNotQualifyMsg.Visible = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool ApplicationQualifies
		{
			set
			{
				if (value == true && ddlProduct.SelectedValue == "-select-")
				{
					lblQualifyMsg.Visible = value;
					btnCreateApplication.Enabled = false;
					pnlResults.Visible = value;
					tbValidCalc.Text = value.ToString();
					rowExtendedResults.Attributes.Add("style", "display: none");
				}
				else
				{
					lblQualifyMsg.Visible = value;
					btnCreateApplication.Enabled = value;
					pnlResults.Visible = value;
                    //if (!chkInterestOnly.Checked)
                    //{
						lblColumnHeader2.Visible = false;
						lblIOSAHLIntOverTerm.Visible = false;
						lblIOSAHLMonthlyInst.Visible = false;
					//}
					tbValidCalc.Text = value.ToString();
				}

			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double LoanAmountFix
		{
			set { lblFixLoanAmount.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); }
		}

		/// <summary>
		/// 
		/// </summary>
		public double LoanAmountVar
		{
			set { lblVarLoanAmount.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); }
		}

		#endregion

		#region other

		/// <summary>
		/// The percentage of the Loan amount to fix for VariFix product
		/// </summary>
		public double FixPercent
		{
			get
			{
				if (Convert.ToInt16(ddlProduct.SelectedValue == "-select-" ? "0" : ddlProduct.SelectedValue) == (int)Products.VariFixLoan && tbFixPercentage.Text.Length > 0)
					return Convert.ToDouble(tbFixPercentage.Text) / 100; // convert user integer to percentage

				return 0;
			}
			set
			{
				lblFixPercent.Text = value.ToString(SAHL.Common.Constants.RateFormat);
				lblFixedPercent.Text = value.ToString(SAHL.Common.Constants.RateFormat);
				lblVariablePercent.Text = (1 - value).ToString(SAHL.Common.Constants.RateFormat);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int CreditMatrixKey
		{
			get { return Convert.ToInt16(tbCreditMatrixKey.Text); }
			set { tbCreditMatrixKey.Text = value.ToString(); }
		}

		public ILegalEntity legalEntity
		{
			set { _le = value as ILegalEntityNaturalPerson; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int CategoryKey
		{
			get { return Convert.ToInt16(tbCategoryKey.Text); }
			set { tbCategoryKey.Text = value.ToString(); }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool DisableCreateApplication
		{
			set { btnCreateApplication.Visible = false; }
		}

		/// <summary>
		/// /
		/// </summary>
		public bool CreateApplicationReadyOnly
		{
			set { btnCreateApplication.Enabled = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public string CreateApplicationButtonText
		{
			set { btnCreateApplication.Text = value; }
		}


		public IApplication MortgageLoanApplication
		{
			get { return _application; }
			set { _application = value; }
		}

		public string ApplicationSource
		{
			get
			{
				return lblMarketingSource.Text;
			}
			set
			{
				lblMarketingSource.Text = value;
			}
		}

		public bool CallStartupScript
		{
			set { _callStartupScript = value; }
		}



		public bool ShowBackButton
		{
			set { btnBack.Visible = value; }
		}

		public bool IsEstateAgentDeal
		{
			set { chkEstateAgentDeal.Checked = value; }
		}


		/// <summary>
		/// 
		/// </summary>
		public int EdgeTerm
		{
			get { return _edgeTerm; }
			set { _edgeTerm = value; }
		}
		private int _edgeTerm;

		#endregion
		#endregion
		#endregion

	}
}

