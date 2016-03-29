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
using SAHL.Common.Authentication;
using Microsoft.ApplicationBlocks.UIProcess;

using SAHL.Common;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using System.Drawing;
using SAHL.Common.BusinessModel;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ApplicationHistory : SAHLCommonBaseView, IApplicationHistory
    {
        #region Private Members

        private IApplication _application;

        private bool _singleRec;
        private IApplicationInformationVariableLoan _vlInfoInitial;
        private IApplicationInformationVariableLoan _vlInfoCurrent;
        private IApplicationRepository _ar;
        private ILookupRepository _lRepo;
        private int _initialInformationKey;
        private int _currentInformationKey;
        private IApplicationInformation _initialInformationRec;
        private IApplicationInformation _currentInformationRec;
        private double _totalLoanRequiredInitial;
        private double _totalLoanRequiredCurrent;
        private double _currentDiscount;
        private double _initialDiscount;

        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!IsPostBack)
            {
                if (gridHistory.Rows.Count > 0)
                {
                    for (int x = 0; x < gridHistory.Rows.Count; x++)
                    {
                        if (gridHistory.Rows[x].Cells[1].Text == "Initial")
                        {
                            CheckBox c = gridHistory.Rows[x].Cells[6].Controls[0] as CheckBox;
                            c.Checked = true;

                        }
                    }
                    if (gridHistory.Rows.Count > 1)
                    {
                        CheckBox c = gridHistory.Rows[gridHistory.Rows.Count - 1].Cells[6].Controls[0] as CheckBox;
                        c.Checked = true;
                    }
                }
                BindControls();
            }
        }

        #region IApplicationHistory Members

        /// <summary>
        /// 
        /// </summary>
        public void BindGrid()
        {
            if (_application == null)
                return;

            List<GridItem> lstGridItems = new List<GridItem>();
            IReasonRepository RR = RepositoryFactory.GetRepository<IReasonRepository>();

            _ar = RepositoryFactory.GetRepository<IApplicationRepository>();
            List<IApplicationInformation> i = new List<IApplicationInformation>();
            IEventList<IApplicationInformation> informations = _ar.GetApplicationRevisionHistory(_application.Key);
            foreach (IApplicationInformation info in informations)
            {
                i.Add(info);
            }


            i.Sort(delegate(IApplicationInformation c1, IApplicationInformation c2)
                {
                    return c2.ApplicationInsertDate.CompareTo(c1.ApplicationInsertDate);
                });


            int revisionCount = 0;
            for (int x = i.Count - 1; x >= 0; x--)
            {
                IApplicationInformation info = i[x];
                GridItem itm = new GridItem();
                IReadOnlyEventList<IReason> reasons = RR.GetReasonByGenericTypeAndKey((int)SAHL.Common.Globals.ReasonTypes.RevisionReSubmission, info.Key);
                itm.InfoKey = info.Key.ToString();
                if (x == 0)
                {
                    itm.Revision = "Current";
                }
                else if (x == (i.Count - 1))
                {
                    itm.Revision = "Initial";
                }
                else
                {
                    revisionCount++;
                    itm.Revision = "Revision " + revisionCount.ToString();
                }
                itm.ApplicationType = _application.ApplicationType.Description;
                itm.DateRevised = info.ApplicationInsertDate.ToString(SAHL.Common.Constants.DateFormat);
                itm.Product = lookupRepo.Products.ObjectDictionary[Convert.ToString((int)i[x].ApplicationProduct.ProductType)].Description;
                if (reasons != null && reasons.Count > 0)
                {
                    itm.Reason = reasons[0].ReasonDefinition.ReasonType.Description;
                }
                lstGridItems.Add(itm);
            }

            gridHistory.AutoGenerateColumns = false;
            gridHistory.AddGridBoundColumn("InfoKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            gridHistory.AddGridBoundColumn("Revision", "Revision", Unit.Percentage(20), HorizontalAlign.Left, true);
            gridHistory.AddGridBoundColumn("DateRevised", "Application Start Date", Unit.Percentage(18), HorizontalAlign.Left, true);
            gridHistory.AddGridBoundColumn("ApplicationType", "Application Type", Unit.Percentage(20), HorizontalAlign.Left, true);
            gridHistory.AddGridBoundColumn("Product", "Product ", Unit.Percentage(13), HorizontalAlign.Left, true);
            gridHistory.AddGridBoundColumn("Reason", "Reason", Unit.Percentage(0), HorizontalAlign.Left, false);
            gridHistory.AddCheckBoxColumn("", "Select", false, Unit.Percentage(13), HorizontalAlign.Left, true);
            gridHistory.DataSource = lstGridItems;
            gridHistory.DataBind();




        }

        /// <summary>
        /// 
        /// </summary>
        public IApplication application
        {
            set { _application = value; }
        }


        #endregion


        private void BindControls()
        {
            if (_application != null && gridHistory.Rows.Count > 0 && gridHistory.SelectedIndex > -1)
            {
                _initialInformationKey = -1;
                _currentInformationKey = -1;

                _singleRec = false;
                if (gridHistory.Rows.Count == 1)
                {
                    _singleRec = true;
                    lblInitial.Visible = false;
                    lblCurrent.Visible = false;
                }
                int selectionCount = 0;
                if (_singleRec == false)
                {

                    for (int x = 0; x < gridHistory.Rows.Count; x++)
                    {
                        CheckBox box = gridHistory.Rows[x].Cells[6].Controls[0] as CheckBox;
                        if (box.Checked)
                        {
                            selectionCount++;

                            if (selectionCount == 1)
                            {
                                if (_initialInformationKey == -1)
                                    _initialInformationKey = int.Parse(gridHistory.Rows[x].Cells[0].Text);
                                foreach (IApplicationInformation ai in _application.ApplicationInformations)
                                {
                                    if (ai.Key == _initialInformationKey)
                                    {
                                        _initialInformationRec = ai;
                                       
                                    }
                                }
                                // get the initial rate overrides
                                _initialDiscount = 0D;
                                foreach (IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment in _initialInformationRec.ApplicationInformationFinancialAdjustments)
                                {
									_initialDiscount += applicationInformationFinancialAdjustment.Discount ?? 0;
                                }
                               
                            }
                            else
                            {
                                if (_currentInformationKey == -1)
                                    _currentInformationKey = int.Parse(gridHistory.Rows[x].Cells[0].Text);
                                foreach (IApplicationInformation ai in _application.ApplicationInformations)
                                {
                                    if (ai.Key == _currentInformationKey)
                                    {
                                        _currentInformationRec = ai;                                                                              

                                    }
                                }
                                // get the current rate overrides
                                _currentDiscount = 0D;
								foreach (IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment in _currentInformationRec.ApplicationInformationFinancialAdjustments)
                                {
                                    if (applicationInformationFinancialAdjustment.Discount != null)
										_currentDiscount += applicationInformationFinancialAdjustment.Discount ?? 0;
                                }
                                                               

                            }
                        }

                    }
                    if (selectionCount == 1)
                    {
                        _singleRec = true;
                        lblInitial.Visible = false;
                        lblCurrent.Visible = false;
                    }
                    if (selectionCount > 2 || selectionCount < 1)
                    {
                        if (selectionCount < 1)
                        {
                            this.Messages.Add(new Error("At least one revision must be selected", "At least one revision must be selected"));
                        }
                        else
                        {
                            this.Messages.Add(new Error("A maximum of two revisions can be selected", "A maximum of two revisions can be selected"));
                        }
                        return;
                    }
                }
                else
                {
                    _initialInformationKey = int.Parse(gridHistory.Rows[0].Cells[0].Text);
                    foreach (IApplicationInformation ai in _application.ApplicationInformations)
                    {
                        if (ai.Key == _initialInformationKey)
                        {
                            _initialInformationRec = ai;
                        }
                    }
                    // get the current rate overrides
                    _initialDiscount = 0D;
                    foreach (IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment in _initialInformationRec.ApplicationInformationFinancialAdjustments)
                    {
						_initialDiscount += applicationInformationFinancialAdjustment.Discount ?? 0;
                    }
                }

                _ar = RepositoryFactory.GetRepository<IApplicationRepository>();

                
                panelOfferInformationNew.Visible = true;

                ////display information for initial revision                
                _vlInfoInitial = null;
                if (_initialInformationKey != -1)
                {
                    _vlInfoInitial = _ar.GetApplicationInformationVariableLoan(_initialInformationKey);

                    if (_vlInfoInitial != null)
                    {
                        if (!_singleRec)
                        {
                            txtTermInitial.Text = _vlInfoInitial.Term.HasValue ? _vlInfoInitial.Term.Value.ToString() : "-";
                            if (_vlInfoInitial.MarketRate != null && _vlInfoInitial.MarketRate.HasValue)
                            {
                                txtLinkRateInitial.Text = Math.Round(_vlInfoInitial.RateConfiguration.Margin.Value * 100, 2).ToString() + " %";
                            }

                            if (_initialDiscount != 0)
                            {
                                txtDiscountedLinkRateInitial.Text = Math.Round((_initialDiscount + _vlInfoInitial.RateConfiguration.Margin.Value) * 100, 2).ToString() + " %";
                            }
                            else
                            {
                                txtDiscountedLinkRateInitial.Text = "-";
                            }
                                                        
                            if (_vlInfoInitial.RateConfiguration != null && _vlInfoInitial.RateConfiguration.Margin != null)

                                if (_initialDiscount != 0)
                                {
                                    txtEffectiveRateInitial.Text = Math.Round(((_initialDiscount + _vlInfoInitial.RateConfiguration.Margin.Value) + _vlInfoInitial.MarketRate.Value) * 100, 2).ToString() + " %";
                                }
                                else
                                {
                                    txtEffectiveRateInitial.Text = Math.Round((_vlInfoInitial.RateConfiguration.Margin.Value + _vlInfoInitial.MarketRate.Value) * 100, 2).ToString() + " %";
                                }

                            if (_vlInfoInitial.BondToRegister != null && _vlInfoInitial.BondToRegister.HasValue)
                            {
                                txtBondToRegisterInitial.Text = _vlInfoInitial.BondToRegister.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                            }
                            if (_vlInfoInitial.PropertyValuation != null && _vlInfoInitial.PropertyValuation.HasValue)
                            {
                                txtEstPropertyValueInitial.Text = _vlInfoInitial.PropertyValuation.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                            }
                            txtHouseholdIncomeInitial.Text = _vlInfoInitial.HouseholdIncome.HasValue ? _vlInfoInitial.HouseholdIncome.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                            if (_vlInfoInitial.Category != null)
                            {
                                txtCategoryInitial.Text = _vlInfoInitial.Category.Description.ToString();
                            }
                            if (_vlInfoInitial.SPV != null)
                            {
                                txtSPVNameInitial.Text = _vlInfoInitial.SPV.Description;
                            }
                        }
                        else
                        {
                            txtTermCurrent.Text = _vlInfoInitial.Term != null ? _vlInfoInitial.Term.ToString() : "-";
                            if (_vlInfoInitial.MarketRate != null && _vlInfoInitial.MarketRate.HasValue)
                            {
                                txtLinkRateCurrent.Text = Math.Round(_vlInfoInitial.RateConfiguration.Margin.Value * 100, 2).ToString() + " %";
                            }

                            if (_initialDiscount != 0)
                            {
                                txtEffectiveRateCurrentNew.Text = _vlInfoInitial.RateConfiguration != null && _vlInfoInitial.MarketRate != null && _vlInfoInitial.MarketRate.HasValue ? Math.Round(((_initialDiscount + _vlInfoInitial.RateConfiguration.Margin.Value) + _vlInfoInitial.MarketRate.Value) * 100, 2).ToString() + " %" : "-";
                            }
                            else
                            {
                                txtEffectiveRateCurrentNew.Text = _vlInfoInitial.RateConfiguration != null && _vlInfoInitial.MarketRate != null && _vlInfoInitial.MarketRate.HasValue ? Math.Round((_vlInfoInitial.RateConfiguration.Margin.Value + _vlInfoInitial.MarketRate.Value) * 100, 2).ToString() + " %" : "-";
                            }                                                 


                            if (_vlInfoInitial.BondToRegister != null && _vlInfoInitial.BondToRegister.HasValue)
                            {
                                txtBondToRegisterCurrent.Text = _vlInfoInitial.BondToRegister.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                            }

                            // only one record returned

                            if (_initialDiscount != 0)
                            {
                                txtDiscountedLinkRateCurrent.Text = Math.Round((_initialDiscount + _vlInfoInitial.RateConfiguration.Margin.Value) * 100, 2).ToString() + " %";
                            }
                            else
                            {
                                txtDiscountedLinkRateCurrent.Text = "-";
                            }

                            if (_vlInfoInitial.PropertyValuation != null && _vlInfoInitial.PropertyValuation.HasValue)
                            {
                                txtEstPropertyValueCurrent.Text = _vlInfoInitial.PropertyValuation.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                            }
                            else
                            {
                                txtEstPropertyValueCurrent.Text = "";
                            }
                            txtHouseholdIncomeCurrent.Text = _vlInfoInitial.HouseholdIncome.HasValue ? _vlInfoInitial.HouseholdIncome.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                            if (_vlInfoInitial.Category != null)
                            {
                                txtCategoryCurrent.Text = _vlInfoInitial.Category.Description.ToString();
                            }
                            else
                            {
                                txtCategoryCurrent.Text = "";
                            }
                            if (_vlInfoInitial.SPV != null)
                            {
                                txtSPVNameCurrent.Text = _vlInfoInitial.SPV.Description;
                            }
                            else
                            {
                                txtSPVNameCurrent.Text = "";
                            }
                        }
                    }
                }









                ////display information for current revision            
                _vlInfoCurrent = null;
                if (_currentInformationKey != -1 && _singleRec == false)
                {
                    _vlInfoCurrent = _ar.GetApplicationInformationVariableLoan(_currentInformationKey);

                    
                    if (_vlInfoCurrent != null)
                    {


                        if (_currentDiscount != 0)
                        {
                            txtDiscountedLinkRateCurrent.Text = Math.Round((_currentDiscount + _vlInfoCurrent.RateConfiguration.Margin.Value) * 100, 2).ToString() + " %";
                        }
                        else
                        {
                            txtDiscountedLinkRateCurrent.Text = "-";
                        }

                        txtTermCurrent.Text = _vlInfoCurrent.Term.HasValue ? _vlInfoCurrent.Term.ToString() : "-";
                        if (_vlInfoCurrent.MarketRate != null && _vlInfoCurrent.MarketRate.HasValue)
                        {
                            txtLinkRateCurrent.Text = Math.Round(_vlInfoCurrent.RateConfiguration.Margin.Value * 100, 2).ToString() + " %";
                        }

                        if (_currentDiscount != 0)
                        {
                            txtEffectiveRateCurrentNew.Text = _vlInfoCurrent.RateConfiguration != null ? Math.Round(((_currentDiscount +  _vlInfoCurrent.RateConfiguration.Margin.Value) + _vlInfoCurrent.MarketRate.Value) * 100, 2).ToString() + " %" : "-";
                        }
                        else
                        {
                            txtEffectiveRateCurrentNew.Text = _vlInfoCurrent.RateConfiguration != null ? Math.Round((_vlInfoCurrent.RateConfiguration.Margin.Value + _vlInfoCurrent.MarketRate.Value) * 100, 2).ToString() + " %" : "-";
                        }
                        
                        if (_vlInfoCurrent.BondToRegister != null && _vlInfoCurrent.BondToRegister.HasValue)
                        {
                            txtBondToRegisterCurrent.Text = _vlInfoCurrent.BondToRegister.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                        }

                        txtEstPropertyValueCurrent.Text = _vlInfoCurrent.PropertyValuation.HasValue ? _vlInfoCurrent.PropertyValuation.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                        txtHouseholdIncomeCurrent.Text = _vlInfoCurrent.HouseholdIncome.HasValue ? _vlInfoCurrent.HouseholdIncome.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                        txtCategoryCurrent.Text = _vlInfoCurrent.Category.Description.ToString();
                        if (_vlInfoCurrent.SPV != null)
                        {
                            txtSPVNameCurrent.Text = _vlInfoCurrent.SPV.Description;
                        }
                        else
                        {
                            txtSPVNameCurrent.Text = "";
                        }
                    }
                }

                #region HOC and LIFE

                //IAccountHOC hocAcc = null;
                //IAccountLifePolicy lifeAcc = null;
                //IAccountRepository accRep = RepositoryFactory.GetRepository<IAccountRepository>();
                //if (_application.Account != null)
                //{
                //    for (int x = 0; x < _application.Account.RelatedChildAccounts.Count; x++)
                //    {
                //        switch (_application.Account.RelatedChildAccounts[x].AccountType)
                //        {
                //            case SAHL.Common.Globals.AccountTypes.HOC:
                //                {
                //                    hocAcc = _application.Account.RelatedChildAccounts[x] as IAccountHOC;
                //                    break;
                //                }
                //            case SAHL.Common.Globals.AccountTypes.Life:
                //                {
                //                    lifeAcc = _application.Account.RelatedChildAccounts[x] as IAccountLifePolicy;
                //                    break;
                //                }
                //        }
                //    }
                //}

                #endregion

                IApplicationInformation initialInformationRec = null;
                IApplicationInformation currentInformationRec = null;

                foreach (IApplicationInformation info in _application.ApplicationInformations)
                {
                    if (info.Key == _initialInformationKey)
                    {
                        initialInformationRec = info;
                    }
                    if (info.Key == _currentInformationKey)
                    {
                        currentInformationRec = info;
                    }
                }

                panelInstalmentDetailsVarifix.Visible = false;


                if (_singleRec)
                {
                    txtPurchasePriceInitial.Visible = false;
                    txtPurchasePriceCurrent.Visible = true;
                    txtCashDepositCurrent.Visible = true;
                    txtCashDepositInitial.Visible = false;
                    txtFeesCurrent.Visible = true;
                    txtFeesInitial.Visible = false;
                    txtTotalLoanRequiredCurrent.Visible = true;
                    txtTotalLoanRequiredInitial.Visible = false;
                    txtTermCurrent.Visible = true;
                    txtTermInitial.Visible = false;
                    txtLinkRateCurrent.Visible = true;
                    txtLinkRateInitial.Visible = false;
                    txtDiscountedLinkRateCurrent.Visible = true;
                    txtDiscountedLinkRateInitial.Visible = false;
                    txtEffectiveRateCurrentNew.Visible = true;
                    txtEffectiveRateInitial.Visible = false;
                    txtBondToRegisterCurrent.Visible = true;
                    txtBondToRegisterInitial.Visible = false;
                    txtEstPropertyValueCurrent.Visible = true;
                    txtEstPropertyValueInitial.Visible = false;
                    txtHouseholdIncomeCurrent.Visible = true;
                    txtHouseholdIncomeInitial.Visible = false;
                    txtCategoryCurrent.Visible = true;
                    txtCategoryInitial.Visible = false;
                    txtSPVNameCurrent.Visible = true;
                    txtSPVNameInitial.Visible = false;
                    txtPTIInitial.Visible = false;
                    txtLTVInitial.Visible = false;
                    txtMonthlyInstalmentInitial.Visible = false;
                    txtPrepaymentThreshholdInitial.Visible = false;
                    txtInterimInterestCurrent.Visible = true;
                    txtInterimInterestInitial.Visible = false;
                    txtCashOutInitial.Visible = false;
                    txtCashOutCurrent.Visible = true;
                    txtExistingLoanCurrent.Visible = true;
                    txtExistingLoanInitial.Visible = false;
                    txtPTIInitialVarifix.Visible = false;
                    txtLTVInitialVarifix.Visible = false;
                    txtFixedInstalmentInitialVarifix.Visible = false;
                    txtVarifixFixedPercentageInitialVarifix.Visible = false;
                    txtVarifixVariablePercentageInitialVarifix.Visible = false;
                    txtVariableInstalmentInitialVarifix.Visible = false;
                    txtFixedPortionInitialVarifix.Visible = false;
                    txtVariablePortionInitialVarifix.Visible = false;
                }


                switch (_application.ApplicationType.Key)
                {
                    case (int)SAHL.Common.Globals.OfferTypes.SwitchLoan:
                        {

                            PopulateApplicationTypeSwitch();
                            break;
                        }
                    case (int)SAHL.Common.Globals.OfferTypes.NewPurchaseLoan:
                        {
                            PopulateApplicationTypeNewPurchase();

                            break;
                        }
                    case (int)SAHL.Common.Globals.OfferTypes.ReAdvance:
                    case (int)SAHL.Common.Globals.OfferTypes.FurtherAdvance:
                    case (int)SAHL.Common.Globals.OfferTypes.FurtherLoan:
                    case (int)SAHL.Common.Globals.OfferTypes.RefinanceLoan:
                        {
                            PopulateApplicationTypeRefinance();

                            break;
                        }
                    //  case (int)SAHL.Common.Globals.ApplicationTypes.ReAdvance:
                    // {
                    // PopulateApplicationTypeReadvance();
                    //break;
                    // }
                }

                //if products are the same
                if (_singleRec || currentInformationRec != null)
                {
                    if (_singleRec || initialInformationRec.ApplicationProduct.ProductType == currentInformationRec.ApplicationProduct.ProductType)
                    {
                        if (initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.NewVariableLoan)
                        {
                            PopulateSameProductTypeNewVariableLoan();
                        }
                        else if (initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.SuperLo)
                        {
                            PopulateSameProductTypeSuperlo();
                        }
                        else if (initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.VariFixLoan)
                        {
                            PopulateSameProductTypeVarifixLoan();
                        }
                        else if (initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.Edge)
                        {
                            PopulateSameProductTypeEdge();
                        }
                    }


                    // New Variable and VariFix
                    else if (initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.NewVariableLoan && currentInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.VariFixLoan
                            || initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.VariFixLoan && currentInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.NewVariableLoan)
                    {
                        lblPrepaymentThresholdVarifix.Visible = false;
                        rowTotalInstalmentVarifix.Visible = true;
                        if (_initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.NewVariableLoan)
                        {
                            PopulateNewVariableAndVarifix();
                        }
                        else
                        {
                            PopulateVarifixAndNewVariable();
                        }
                    }


                    //superlo and varifix
                    else if (initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.SuperLo && currentInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.VariFixLoan
         || initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.VariFixLoan && currentInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.SuperLo)
                    {
                        panelInstalmentDetailsVarifix.Visible = true;
                        rowTotalInstalmentVarifix.Visible = true;
                        if (currentInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.VariFixLoan)
                        {
                            PopulateSuperloAndVarifix();
                        }
                        else
                        {
                            PopulateVarifixAndSuperlo();
                        }
                    }

                //superlo and new variable
                    else if (initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.SuperLo && currentInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.NewVariableLoan
              || initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.NewVariableLoan && currentInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.SuperLo)
                    {

                        panelInstalmentDetails.Visible = true;
                        rowPrepaymentThreshhold.Visible = true;
                        if (initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.NewVariableLoan)
                        {
                            PopulateNewVariableAndSuperlo();
                        }
                        else
                        {
                            PopulateSuperloAndNewVariable();
                        }
                    }
                // EHL and NVL
                    else if (initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.Edge && currentInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.NewVariableLoan
           || initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.NewVariableLoan && currentInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.Edge)
                    {
                        panelInstalmentDetailsEdge.Visible = true;
                        if (_initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.Edge)
                        {
                            PopulateEdgeAndNewVariable();
                        }
                        else
                        {
                            PopulateNewVariableAndEdge();
                        }
                    }

                // EHL and VF
                    else if (initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.Edge && currentInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.VariFixLoan
           || initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.VariFixLoan && currentInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.Edge)
                    {
                        panelInstalmentDetailsEdge.Visible = true;
                        if (_initialInformationRec.ApplicationProduct.ProductType == SAHL.Common.Globals.Products.Edge)
                        {
                            PopulateEdgeAndVarifix();
                        }
                        else
                        {
                            PopulateVarifixAndEdge();
                        }
                    }

                    if (!_singleRec)
                        SetColor();
                }
            }
        }

        private void PopulateSuperloAndNewVariable()
        {
            IApplicationInformationSuperLoLoan InitialRec = _ar.GetApplicationInformationSuperLoLoan(_initialInformationKey);
            ISupportsInterestOnlyApplicationInformation interestOnlyInitial = _initialInformationRec.ApplicationProduct as ISupportsInterestOnlyApplicationInformation;
            ISupportsInterestOnlyApplicationInformation interestOnlyCurrent = _currentInformationRec.ApplicationProduct as ISupportsInterestOnlyApplicationInformation;


            //Superlo
            txtPTIInitial.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVInitial.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";
            txtMonthlyInstalmentInitial.Text = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            txtPrepaymentThreshholdInitial.Text = InitialRec.PPThresholdYr1.ToString(SAHL.Common.Constants.CurrencyFormat);

            //New Variable
            txtPTICurrent.Text = Math.Round(_vlInfoCurrent.PTI.Value * 100, 2).ToString() + " %";
            txtLTVCurrent.Text = Math.Round(_vlInfoCurrent.LTV.Value * 100, 2).ToString() + " %";
            txtMonthlyInstalmentCurrent.Text = _vlInfoCurrent.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtPrepaymentThreshholdCurrent.Text = "-";


            if (interestOnlyCurrent != null || interestOnlyInitial != null)
            {
                lblInterestOnlyInstalment.Visible = true;
                txtInterestOnlyInstalmentInitial.Visible = true;
                txtInterestOnlyInstalmentCurrent.Visible = true;
                rowInterestOnly.Visible = true;

                if (interestOnlyCurrent != null && interestOnlyCurrent.InterestOnlyInformation != null)
                {
                    txtInterestOnlyInstalmentCurrent.Text = interestOnlyCurrent.InterestOnlyInformation.Installment.HasValue ? interestOnlyCurrent.InterestOnlyInformation.Installment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                }
                else
                {
                    txtInterestOnlyInstalmentCurrent.Text = "-";
                }

                if (interestOnlyInitial != null && interestOnlyInitial.InterestOnlyInformation != null && interestOnlyInitial.InterestOnlyInformation.Installment.HasValue)
                {
                    txtInterestOnlyInstalmentInitial.Text = interestOnlyInitial.InterestOnlyInformation.Installment.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
                else
                {
                    txtInterestOnlyInstalmentInitial.Text = "-";
                }
            }
        }

        private void PopulateNewVariableAndSuperlo()
        {
            IApplicationInformationSuperLoLoan currentRec = _ar.GetApplicationInformationSuperLoLoan(_currentInformationKey);
            ISupportsInterestOnlyApplicationInformation interestOnlyInitial = _initialInformationRec.ApplicationProduct as ISupportsInterestOnlyApplicationInformation;
            ISupportsInterestOnlyApplicationInformation interestOnlyCurrent = _currentInformationRec.ApplicationProduct as ISupportsInterestOnlyApplicationInformation;

            //New Variable
            txtPTIInitial.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVInitial.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";
            txtMonthlyInstalmentInitial.Text = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            txtPrepaymentThreshholdInitial.Text = "-";

            //Superlo
            txtPTICurrent.Text = _vlInfoCurrent.PTI.HasValue ? Math.Round(_vlInfoCurrent.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVCurrent.Text = _vlInfoCurrent.LTV.HasValue ? Math.Round(_vlInfoCurrent.LTV.Value * 100, 2).ToString() + " %" : "-";
            txtMonthlyInstalmentCurrent.Text = _vlInfoCurrent.MonthlyInstalment.HasValue ? _vlInfoCurrent.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            txtPrepaymentThreshholdCurrent.Text = currentRec.PPThresholdYr1.ToString(SAHL.Common.Constants.CurrencyFormat);


            if (interestOnlyInitial != null || interestOnlyCurrent != null)
            {
                lblInterestOnlyInstalment.Visible = true;
                txtInterestOnlyInstalmentInitial.Visible = true;
                txtInterestOnlyInstalmentCurrent.Visible = true;
                rowInterestOnly.Visible = true;

                if (interestOnlyInitial != null && interestOnlyInitial.InterestOnlyInformation != null && interestOnlyInitial.InterestOnlyInformation.Installment.HasValue)
                {
                    txtInterestOnlyInstalmentInitial.Text = interestOnlyInitial.InterestOnlyInformation.Installment.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
                else
                {
                    txtInterestOnlyInstalmentInitial.Text = "-";
                }

                if (interestOnlyCurrent != null && interestOnlyCurrent.InterestOnlyInformation != null && interestOnlyCurrent.InterestOnlyInformation.Installment.HasValue)
                {
                    txtInterestOnlyInstalmentCurrent.Text = interestOnlyCurrent.InterestOnlyInformation.Installment.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
                else
                {
                    txtInterestOnlyInstalmentCurrent.Text = "-";
                }
            }
        }


        private void PopulateSuperloAndVarifix()
        {
            IApplicationInformationSuperLoLoan InitialRec = _ar.GetApplicationInformationSuperLoLoan(_initialInformationKey);
            IApplicationInformationVarifixLoan CurrentRec = _ar.GetApplicationInformationVarifixLoan(_currentInformationKey);
            ISupportsInterestOnlyApplicationInformation interestOnlyInitial = _initialInformationRec.ApplicationProduct as ISupportsInterestOnlyApplicationInformation;
            ISupportsVariFixApplicationInformation currentVarifixInfo = _currentInformationRec.ApplicationProduct as ISupportsVariFixApplicationInformation;

            //Superlo
            txtPTIInitialVarifix.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVInitialVarifix.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";
            txtFixedInstalmentInitialVarifix.Text = "-";
            txtVariableInstalmentInitialVarifix.Text = "-";

            txtTotalInstalmentInitialVarifix.Text = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            txtVarifixFixedPercentageInitialVarifix.Text = "-";
            txtVarifixVariablePercentageInitialVarifix.Text = "-";
            txtFixedPortionInitialVarifix.Text = "-";
            txtVariablePortionInitialVarifix.Text = "-";
            txtPrepaymentThresholdVarifixInitial.Text = InitialRec.PPThresholdYr1.ToString(SAHL.Common.Constants.CurrencyFormat);

            //Varifix
            txtPTICurrentVarifix.Text = _vlInfoCurrent.PTI.HasValue ? Math.Round(_vlInfoCurrent.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVCurrentVarifix.Text = _vlInfoCurrent.LTV.HasValue ? Math.Round(_vlInfoCurrent.LTV.Value * 100, 2).ToString() + " %" : "-";
            txtFixedInstalmentCurrentVarifix.Text = CurrentRec.FixedInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
            double currentVarInstallment = 0;
            currentVarInstallment = _vlInfoCurrent.MonthlyInstalment.HasValue ? _vlInfoCurrent.MonthlyInstalment.Value - currentVarifixInfo.VariFixInformation.FixedInstallment : 0;
            txtVariableInstalmentCurrentVarifix.Text = currentVarInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
            if (_vlInfoCurrent.MonthlyInstalment.HasValue)
            {
                double tot = _vlInfoCurrent.MonthlyInstalment.Value;
                txtTotalInstalmentCurrentVarifix.Text = tot.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
            txtVarifixFixedPercentageCurrentVarifix.Text = Math.Round(CurrentRec.FixedPercent * 100, 2).ToString() + " %";
            double variable = 100 - (CurrentRec.FixedPercent * 100);
            txtVarifixVariablePercentageCurrentVarifix.Text = variable.ToString() + " %";
            txtFixedPortionCurrentVarifix.Text = _vlInfoCurrent.LoanAgreementAmount.HasValue ? (_vlInfoCurrent.LoanAgreementAmount.Value * CurrentRec.FixedPercent).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            txtVariablePortionCurrentVarifix.Text = _vlInfoCurrent.LoanAgreementAmount.HasValue ? (_vlInfoCurrent.LoanAgreementAmount.Value * (100 - (CurrentRec.FixedPercent * 100)) / 100).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            txtPrepaymentThresholdVarifixCurrent.Text = "-";


            if (interestOnlyInitial != null && interestOnlyInitial.InterestOnlyInformation != null)
            {
                txtMonthlyInstalmentInitialVarifix.Visible = true;
                txtMonthlyInstalmentCurrentVarifix.Visible = true;
                lblMonthlyInstalmentVarifix.Visible = true;

                txtInterestOnlyInstalmentInitialVarifix.Visible = true;
                txtInterestOnlyInstalmentCurrentVarifix.Visible = true;
                lblInterestOnlyInstalmentVarifix.Visible = true;
                rowInterestOnlyInstalmentVarifix.Visible = true;

                txtMonthlyInstalmentCurrentVarifix.Text = "-";
                txtMonthlyInstalmentInitialVarifix.Text = _vlInfoInitial.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat);

                txtInterestOnlyInstalmentCurrentVarifix.Text = "-";
                txtInterestOnlyInstalmentInitialVarifix.Text = interestOnlyInitial.InterestOnlyInformation.Installment.HasValue ? interestOnlyInitial.InterestOnlyInformation.Installment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateVarifixAndSuperlo()
        {
            IApplicationInformationVarifixLoan initialRec = _ar.GetApplicationInformationVarifixLoan(_initialInformationKey);
            IApplicationInformationSuperLoLoan currentRec = _ar.GetApplicationInformationSuperLoLoan(_currentInformationKey);
            ISupportsInterestOnlyApplicationInformation interestOnlyCurrent = _currentInformationRec.ApplicationProduct as ISupportsInterestOnlyApplicationInformation;
            ISupportsVariFixApplicationInformation initialVarifixInfo = _initialInformationRec.ApplicationProduct as ISupportsVariFixApplicationInformation;

            //Varifix
            txtPTIInitialVarifix.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVInitialVarifix.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";
            txtFixedInstalmentInitialVarifix.Text = initialRec.FixedInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
            double initialVarInstallment = 0;
            //if (_application.HasAttribute(OfferAttributeTypes.CapitalizeFees))
            //{
            //    initialVarInstallment = _vlInfoInitial.MonthlyInstalment.HasValue && _vlInfoInitial.FeesTotal.HasValue ? _vlInfoInitial.MonthlyInstalment.Value + _vlInfoInitial.FeesTotal.Value - initialVarifixInfo.VariFixInformation.FixedInstallment : 0;
            //}
            //else
            //{
            initialVarInstallment = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value - initialVarifixInfo.VariFixInformation.FixedInstallment : 0;
            //}
            txtVariableInstalmentInitialVarifix.Text = initialVarInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
            if (_vlInfoInitial.MonthlyInstalment.HasValue)
            {
                double tot = _vlInfoInitial.MonthlyInstalment.Value;
                txtTotalInstalmentInitialVarifix.Text = tot.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
            txtVarifixFixedPercentageInitialVarifix.Text = Math.Round(initialRec.FixedPercent * 100, 2).ToString() + " %";
            double variable = 100 - (initialRec.FixedPercent * 100);
            txtVarifixVariablePercentageInitialVarifix.Text = variable.ToString() + " %";
            txtFixedPortionInitialVarifix.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? Math.Round(_vlInfoInitial.LoanAgreementAmount.Value * initialRec.FixedPercent, 2).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            txtVariablePortionInitialVarifix.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? Math.Round(_vlInfoInitial.LoanAgreementAmount.Value * (100 - (initialRec.FixedPercent * 100)) / 100, 2).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            txtPrepaymentThresholdVarifixInitial.Text = "-";

            //Superlo
            txtPTICurrentVarifix.Text = _vlInfoCurrent.PTI.HasValue ? Math.Round(_vlInfoCurrent.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVCurrentVarifix.Text = _vlInfoCurrent.LTV.HasValue ? Math.Round(_vlInfoCurrent.LTV.Value * 100, 2).ToString() + " %" : "-";
            txtFixedInstalmentCurrentVarifix.Text = "-";
            txtVariableInstalmentCurrentVarifix.Text = "-";
            txtTotalInstalmentCurrentVarifix.Text = "-";
            txtVarifixFixedPercentageCurrentVarifix.Text = "-";
            txtVarifixVariablePercentageCurrentVarifix.Text = "-";
            txtFixedPortionCurrentVarifix.Text = "-";
            txtVariablePortionCurrentVarifix.Text = "-";
            txtPrepaymentThresholdVarifixCurrent.Text = currentRec.PPThresholdYr1.ToString(SAHL.Common.Constants.CurrencyFormat);

            if (interestOnlyCurrent != null && interestOnlyCurrent.InterestOnlyInformation != null)
            {
                txtMonthlyInstalmentCurrentVarifix.Visible = true;
                txtMonthlyInstalmentInitialVarifix.Visible = true;
                lblMonthlyInstalmentVarifix.Visible = true;

                txtInterestOnlyInstalmentCurrentVarifix.Visible = true;
                txtInterestOnlyInstalmentInitialVarifix.Visible = true;
                lblInterestOnlyInstalmentVarifix.Visible = true;
                rowInterestOnlyInstalmentVarifix.Visible = true;

                txtMonthlyInstalmentInitialVarifix.Text = "-";
                txtMonthlyInstalmentCurrentVarifix.Text = _vlInfoCurrent.MonthlyInstalment.HasValue ? _vlInfoCurrent.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";

                txtInterestOnlyInstalmentInitialVarifix.Text = "-";
                txtInterestOnlyInstalmentCurrentVarifix.Text = interestOnlyCurrent.InterestOnlyInformation.Installment.HasValue ? interestOnlyCurrent.InterestOnlyInformation.Installment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateVarifixAndNewVariable()
        {
            panelInstalmentDetailsVarifix.Visible = true;
            IApplicationInformationVarifixLoan initialRec = _ar.GetApplicationInformationVarifixLoan(_initialInformationKey);
            IApplicationInformationVariableLoan currentRec = _ar.GetApplicationInformationVariableLoan(_currentInformationKey);
            ISupportsInterestOnlyApplicationInformation interestOnlyCurrent = _initialInformationRec.ApplicationProduct as ISupportsInterestOnlyApplicationInformation;

            if (initialRec == null || currentRec == null)
            {
                return;
            }

            //Varifix (initial rec)

            txtPTIInitialVarifix.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVInitialVarifix.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";
            txtFixedInstalmentInitialVarifix.Text = initialRec.FixedInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
            double variableInstalInit = 0;
            //if(_application.HasAttribute(SAHL.Common.Globals.OfferAttributeTypes.CapitalizeFees))
            //{
            //    variableInstalInit = _vlInfoInitial.MonthlyInstalment.HasValue && _vlInfoInitial.FeesTotal.HasValue ? _vlInfoInitial.MonthlyInstalment.Value + _vlInfoInitial.FeesTotal.Value - initialRec.FixedInstallment : 0;
            //}
            //else
            //{
            variableInstalInit = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value - initialRec.FixedInstallment : 0;
            //}
            txtVariableInstalmentInitialVarifix.Text = variableInstalInit.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtTotalInstalmentInitialVarifix.Text = _vlInfoInitial.MonthlyInstalment.HasValue ? (_vlInfoInitial.MonthlyInstalment.Value + initialRec.FixedInstallment).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            txtVarifixFixedPercentageInitialVarifix.Text = Math.Round(initialRec.FixedPercent * 100, 2).ToString() + " %";
            txtVarifixVariablePercentageInitialVarifix.Text = Convert.ToString(100 - (initialRec.FixedPercent * 100)) + " %";
            txtFixedPortionInitialVarifix.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? (_vlInfoInitial.LoanAgreementAmount.Value * initialRec.FixedPercent).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            txtVariablePortionInitialVarifix.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? (_vlInfoInitial.LoanAgreementAmount.Value * (100 - (initialRec.FixedPercent * 100)) / 100).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";

            // New Variable (current rec)
            txtPTICurrentVarifix.Text = _vlInfoCurrent.PTI.HasValue ? Math.Round(_vlInfoCurrent.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVCurrentVarifix.Text = Math.Round(_vlInfoCurrent.LTV.Value * 100, 2).ToString() + " %";
            txtFixedInstalmentCurrentVarifix.Text = "-";

            txtVariableInstalmentCurrentVarifix.Text = "-";
            txtTotalInstalmentCurrentVarifix.Text = (_vlInfoCurrent.MonthlyInstalment.Value).ToString(SAHL.Common.Constants.CurrencyFormat);
            txtVarifixFixedPercentageCurrentVarifix.Text = "-";
            txtVarifixVariablePercentageCurrentVarifix.Text = "-";
            txtFixedPortionCurrentVarifix.Text = "-";
            txtVariablePortionCurrentVarifix.Text = "-";

            if (interestOnlyCurrent != null && interestOnlyCurrent.InterestOnlyInformation != null)
            {
                lblMonthlyInstalmentVarifix.Visible = true;
                txtMonthlyInstalmentInitialVarifix.Visible = true;
                txtMonthlyInstalmentCurrentVarifix.Visible = true;

                lblInterestOnlyInstalmentVarifix.Visible = true;
                txtInterestOnlyInstalmentInitialVarifix.Visible = true;
                txtInterestOnlyInstalmentCurrentVarifix.Visible = true;
                rowInterestOnlyInstalmentVarifix.Visible = true;

                txtMonthlyInstalmentInitialVarifix.Text = "-";
                txtInterestOnlyInstalmentInitialVarifix.Text = "-";

                txtMonthlyInstalmentCurrentVarifix.Text = _vlInfoCurrent.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                txtInterestOnlyInstalmentCurrentVarifix.Text = interestOnlyCurrent.InterestOnlyInformation.Installment.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateNewVariableAndVarifix()
        {
            panelInstalmentDetailsVarifix.Visible = true;

            IApplicationInformationVariableLoan initialRec = _ar.GetApplicationInformationVariableLoan(_initialInformationKey);
            IApplicationInformationVarifixLoan currrentRec = _ar.GetApplicationInformationVarifixLoan(_currentInformationKey);
            ISupportsInterestOnlyApplicationInformation interestOnlyInitial = _initialInformationRec.ApplicationProduct as ISupportsInterestOnlyApplicationInformation;

            if (initialRec == null || currrentRec == null)
            {
                return;
            }

            //New variable (initial rec)
            txtPTIInitialVarifix.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVInitialVarifix.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";
            txtFixedInstalmentInitialVarifix.Text = "-";


            txtVariableInstalmentInitialVarifix.Text = "-";

            if (_vlInfoInitial.MonthlyInstalment.HasValue)
            {
                txtTotalInstalmentInitialVarifix.Text = _vlInfoInitial.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
            else
            {
                txtTotalInstalmentInitialVarifix.Text = "-";
            }

            txtVarifixFixedPercentageInitialVarifix.Text = "-";
            txtVarifixVariablePercentageInitialVarifix.Text = "-";
            txtFixedPortionInitialVarifix.Text = "-";
            txtVariablePortionInitialVarifix.Text = "-";

            // Varifix (current rec)
            txtPTICurrentVarifix.Text = _vlInfoCurrent.PTI.HasValue ? Math.Round(_vlInfoCurrent.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVCurrentVarifix.Text = _vlInfoCurrent.LTV.HasValue ? Math.Round(_vlInfoCurrent.LTV.Value * 100, 2).ToString() + " %" : "-";
            txtFixedInstalmentCurrentVarifix.Text = currrentRec.FixedInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);

            double variableInstalCurrent = 0;
            //if (_application.HasAttribute(SAHL.Common.Globals.OfferAttributeTypes.CapitalizeFees))
            //{
            //    variableInstalCurrent = _vlInfoCurrent.MonthlyInstalment.HasValue  && _vlInfoCurrent.FeesTotal.HasValue ? _vlInfoCurrent.MonthlyInstalment.Value + _vlInfoCurrent.FeesTotal.Value - currrentRec.FixedInstallment : 0;
            //}
            //else
            //{
            variableInstalCurrent = _vlInfoCurrent.MonthlyInstalment.HasValue ? _vlInfoCurrent.MonthlyInstalment.Value - currrentRec.FixedInstallment : 0;
            //}

            txtVariableInstalmentCurrentVarifix.Text = variableInstalCurrent.ToString(SAHL.Common.Constants.CurrencyFormat);

            double monthlyInstalCurrent = 0;
            if (_vlInfoCurrent.MonthlyInstalment.HasValue)
            {
                monthlyInstalCurrent = _vlInfoCurrent.MonthlyInstalment.Value;
                txtTotalInstalmentCurrentVarifix.Text = monthlyInstalCurrent.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
            else
            {
                txtTotalInstalmentCurrentVarifix.Text = "-";
            }

            txtVarifixFixedPercentageCurrentVarifix.Text = Math.Round(currrentRec.FixedPercent * 100, 2).ToString() + " %";
            txtVarifixVariablePercentageCurrentVarifix.Text = Convert.ToString(100 - (currrentRec.FixedPercent * 100)) + " %";
            txtFixedPortionCurrentVarifix.Text = _vlInfoCurrent.LoanAgreementAmount.HasValue ? ((_vlInfoCurrent.LoanAgreementAmount.Value + _vlInfoCurrent.FeesTotal.Value) * currrentRec.FixedPercent).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            txtVariablePortionCurrentVarifix.Text = _vlInfoCurrent.LoanAgreementAmount.HasValue ? ((_vlInfoCurrent.LoanAgreementAmount.Value + _vlInfoCurrent.FeesTotal.Value) * ((100 - (currrentRec.FixedPercent * 100)) / 100)).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";

            if (interestOnlyInitial.InterestOnlyInformation != null)
            {
                lblMonthlyInstalmentVarifix.Visible = true;
                txtMonthlyInstalmentInitialVarifix.Visible = true;
                txtMonthlyInstalmentCurrentVarifix.Visible = true;

                lblInterestOnlyInstalmentVarifix.Visible = true;
                txtInterestOnlyInstalmentInitialVarifix.Visible = true;
                txtInterestOnlyInstalmentCurrentVarifix.Visible = true;
                rowInterestOnlyInstalmentVarifix.Visible = true;

                txtMonthlyInstalmentCurrentVarifix.Text = "-";
                txtInterestOnlyInstalmentCurrentVarifix.Text = "-";

                txtMonthlyInstalmentInitialVarifix.Text = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                txtInterestOnlyInstalmentInitialVarifix.Text = interestOnlyInitial.InterestOnlyInformation.Installment.HasValue ? interestOnlyInitial.InterestOnlyInformation.Installment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            }
        }


        private void PopulateEdgeAndNewVariable()
        {
            IApplicationInformationEdge initialRec = _ar.GetApplicationInformationEdge(_initialInformationKey);
            IApplicationInformationVariableLoan currrentRec = _ar.GetApplicationInformationVariableLoan(_currentInformationKey);
            ISupportsInterestOnlyApplicationInformation interestOnlyInitial = _currentInformationRec.ApplicationProduct as ISupportsInterestOnlyApplicationInformation;

            if (initialRec == null || currrentRec == null)
            {
                return;
            }

            //EHL
            txtPTIInitialEHL.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVInitialEHL.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";

            txtInterestOnlyInstalmentInitialEHL.Text = initialRec.InterestOnlyInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtAmortisingInstalmentInitialEHL.Text = initialRec.AmortisationTermInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtullTermInitialEHL.Text = initialRec.FullTermInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);


            // Variable
            txtPTICurrentEHL.Text = _vlInfoCurrent.PTI.HasValue ? Math.Round(_vlInfoCurrent.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVCurrentEHL.Text = _vlInfoCurrent.LTV.HasValue ? Math.Round(_vlInfoCurrent.LTV.Value * 100, 2).ToString() + " %" : "-";

            if (interestOnlyInitial != null && interestOnlyInitial.InterestOnlyInformation != null && interestOnlyInitial.InterestOnlyInformation.Installment != null && interestOnlyInitial.InterestOnlyInformation.Installment.HasValue)
                txtInterestOnlyInstalmentCurrentEHL.Text = interestOnlyInitial.InterestOnlyInformation.Installment.Value.ToString(SAHL.Common.Constants.CurrencyFormat); 
            else
                txtInterestOnlyInstalmentCurrentEHL.Text = "-";

            txtAmortisingInstalmentCurrentEHL.Text = _vlInfoCurrent.MonthlyInstalment.HasValue ? _vlInfoCurrent.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            txtullTermCurrentEHL.Text = "-";


        }

        private void PopulateNewVariableAndEdge()
        {
            IApplicationInformationVariableLoan initialRec = _ar.GetApplicationInformationVariableLoan(_initialInformationKey);
            IApplicationInformationEdge currrentRec = _ar.GetApplicationInformationEdge(_currentInformationKey);
            ISupportsInterestOnlyApplicationInformation interestOnlyInitial = _initialInformationRec.ApplicationProduct as ISupportsInterestOnlyApplicationInformation;

            if (initialRec == null || currrentRec == null)
            {
                return;
            }

            // Variable
            txtPTIInitialEHL.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVInitialEHL.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";

            if (interestOnlyInitial != null && interestOnlyInitial.InterestOnlyInformation != null && interestOnlyInitial.InterestOnlyInformation.Installment != null && interestOnlyInitial.InterestOnlyInformation.Installment.HasValue)
                txtInterestOnlyInstalmentInitialEHL.Text = interestOnlyInitial.InterestOnlyInformation.Installment.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
            else
                txtInterestOnlyInstalmentInitialEHL.Text = "-";

            txtAmortisingInstalmentInitialEHL.Text = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoCurrent.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            txtullTermInitialEHL.Text = "-";


            //EHL 
            txtPTICurrentEHL.Text = _vlInfoCurrent.PTI.HasValue ? Math.Round(_vlInfoCurrent.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVCurrentEHL.Text = _vlInfoCurrent.LTV.HasValue ? Math.Round(_vlInfoCurrent.LTV.Value * 100, 2).ToString() + " %" : "-";


            txtInterestOnlyInstalmentCurrentEHL.Text = currrentRec.InterestOnlyInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtAmortisingInstalmentCurrentEHL.Text = currrentRec.AmortisationTermInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtullTermCurrentEHL.Text = currrentRec.FullTermInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);


        }

        private void PopulateEdgeAndVarifix()
        {
            rowVarifixEHLFixedInstalment.Visible = true;
            rowVarifixEHLVarInstalment.Visible = true;
            rowVarifixFixedPercentageEHL.Visible = true;
            rowVarifixVariablePercentageEHL.Visible = true;
            rowFixedPortionVarifixEHL.Visible = true;
            rowVariablePortionVarifixEHL.Visible = true;

            IApplicationInformationEdge initialRec = _ar.GetApplicationInformationEdge(_initialInformationKey);
            IApplicationInformationVarifixLoan currrentRec = _ar.GetApplicationInformationVarifixLoan(_currentInformationKey);

            if (initialRec == null || currrentRec == null)
            {
                return;
            }

            //EHL
            txtPTIInitialEHL.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVInitialEHL.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";

            txtInterestOnlyInstalmentInitialEHL.Text = initialRec.InterestOnlyInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtAmortisingInstalmentInitialEHL.Text = initialRec.AmortisationTermInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtullTermInitialEHL.Text = initialRec.FullTermInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);

            txtFixedInstalmentInitialVarifixEHL.Text = "-";
            txtVariableInstalmentInitialVarifixEHL.Text = "-";

            txtVarifixFixedPercentageInitialVarifixEHL.Text = "-";
            txtVarifixVariablePercentageInitialVarifixEHL.Text = "-";
            txtFixedPortionInitialVarifixEHL.Text = "-";
            txtVariablePortionInitialVarifixEHL.Text = "-";

            // Varifix
            txtPTICurrentEHL.Text = _vlInfoCurrent.PTI.HasValue ? Math.Round(_vlInfoCurrent.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVCurrentEHL.Text = _vlInfoCurrent.LTV.HasValue ? Math.Round(_vlInfoCurrent.LTV.Value * 100, 2).ToString() + " %" : "-";

            txtInterestOnlyInstalmentCurrentEHL.Text = "-";
            txtAmortisingInstalmentCurrentEHL.Text = "-";
            txtullTermCurrentEHL.Text = "-";

            txtVariableInstalmentCurrentVarifixEHL.Text = currrentRec.FixedInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtFixedInstalmentCurrentVarifixEHL.Text = Convert.ToDouble(_vlInfoCurrent.MonthlyInstalment.HasValue ? _vlInfoCurrent.MonthlyInstalment.Value - currrentRec.FixedInstallment : 0).ToString(SAHL.Common.Constants.CurrencyFormat);

            txtVarifixFixedPercentageCurrentVarifixEHL.Text = Math.Round(currrentRec.FixedPercent * 100, 2).ToString() + " %";
            txtVarifixVariablePercentageCurrentVarifixEHL.Text = Convert.ToString(100 - (currrentRec.FixedPercent * 100)) + " %";
            txtFixedPortionCurrentVarifixEHL.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? (_vlInfoCurrent.LoanAgreementAmount.Value * currrentRec.FixedPercent).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            txtVariablePortionCurrentVarifixEHL.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? (_vlInfoCurrent.LoanAgreementAmount.Value * (100 - (currrentRec.FixedPercent * 100)) / 100).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
        }

        private void PopulateVarifixAndEdge()
        {
            rowVarifixEHLFixedInstalment.Visible = true;
            rowVarifixEHLVarInstalment.Visible = true;
            rowVarifixFixedPercentageEHL.Visible = true;
            rowVarifixVariablePercentageEHL.Visible = true;
            rowFixedPortionVarifixEHL.Visible = true;
            rowVariablePortionVarifixEHL.Visible = true;

            IApplicationInformationVarifixLoan initialRec = _ar.GetApplicationInformationVarifixLoan(_initialInformationKey);
            IApplicationInformationEdge currrentRec = _ar.GetApplicationInformationEdge(_currentInformationKey);

            if (initialRec == null || currrentRec == null)
            {
                return;
            }

            // Varifix
            txtPTIInitialEHL.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVInitialEHL.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";

            txtFixedInstalmentInitialVarifixEHL.Text = initialRec.FixedInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtVariableInstalmentInitialVarifixEHL.Text = Convert.ToDouble(_vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value - initialRec.FixedInstallment : 0).ToString(SAHL.Common.Constants.CurrencyFormat);

            txtVarifixFixedPercentageInitialVarifixEHL.Text = Math.Round(initialRec.FixedPercent * 100, 2).ToString() + " %";
            txtVarifixVariablePercentageInitialVarifixEHL.Text = Convert.ToString(100 - (initialRec.FixedPercent * 100)) + " %";
            txtFixedPortionInitialVarifixEHL.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? (_vlInfoInitial.LoanAgreementAmount.Value * initialRec.FixedPercent).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            txtVariablePortionInitialVarifixEHL.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? (_vlInfoInitial.LoanAgreementAmount.Value * (100 - (initialRec.FixedPercent * 100)) / 100).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";


            txtInterestOnlyInstalmentInitialEHL.Text = "-";
            txtAmortisingInstalmentInitialEHL.Text = "-";
            txtullTermInitialEHL.Text = "-";


            // EHL 
            txtPTICurrentEHL.Text = _vlInfoCurrent.PTI.HasValue ? Math.Round(_vlInfoCurrent.PTI.Value * 100, 2).ToString() + " %" : "-";
            txtLTVCurrentEHL.Text = _vlInfoCurrent.LTV.HasValue ? Math.Round(_vlInfoCurrent.LTV.Value * 100, 2).ToString() + " %" : "-";

            txtInterestOnlyInstalmentCurrentEHL.Text = currrentRec.InterestOnlyInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtAmortisingInstalmentCurrentEHL.Text = currrentRec.AmortisationTermInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtullTermCurrentEHL.Text = currrentRec.FullTermInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);

            txtVariableInstalmentCurrentVarifixEHL.Text = "-";
            txtFixedInstalmentCurrentVarifixEHL.Text = "-";

            txtVarifixFixedPercentageCurrentVarifixEHL.Text = "-";
            txtVarifixVariablePercentageCurrentVarifixEHL.Text = "-";
            txtFixedPortionCurrentVarifixEHL.Text = "-";
            txtVariablePortionCurrentVarifixEHL.Text = "-";

        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateSameProductTypeVarifixLoan()
        {
            panelInstalmentDetailsVarifix.Visible = true;
            lblPrepaymentThresholdVarifix.Visible = false;
            txtPrepaymentThresholdVarifixCurrent.Visible = false;
            txtPrepaymentThresholdVarifixInitial.Visible = false;
            lblTotalInstalmentVarifix.Visible = false;
            txtTotalInstalmentCurrentVarifix.Visible = false;
            txtTotalInstalmentInitialVarifix.Visible = false;
            lblInitialVarifix.Visible = false;
            lblCurrentVarifix.Visible = false;

            _ar = RepositoryFactory.GetRepository<IApplicationRepository>();

            ISupportsVariFixApplicationInformation initialVarifixInfo = _initialInformationRec.ApplicationProduct as ISupportsVariFixApplicationInformation;

            ISupportsVariFixApplicationInformation currentVarifixInfo = null;
            if (_currentInformationRec != null)
            {
                currentVarifixInfo = _currentInformationRec.ApplicationProduct as ISupportsVariFixApplicationInformation;
            }

            if (!_singleRec)
            {
                txtPTIInitialVarifix.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";
                txtLTVInitialVarifix.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";

                txtFixedInstalmentInitialVarifix.Text = initialVarifixInfo.VariFixInformation.FixedInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);


                txtFixedInstalmentCurrentVarifix.Text = currentVarifixInfo.VariFixInformation.FixedInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);

                double variableInstalInit = 0;
                //if (_application.HasAttribute(SAHL.Common.Globals.OfferAttributeTypes.CapitalizeFees))
                //{
                //    variableInstalInit = _vlInfoInitial.MonthlyInstalment.HasValue && _vlInfoInitial.FeesTotal.HasValue ? _vlInfoInitial.MonthlyInstalment.Value + _vlInfoInitial.FeesTotal.Value - initialVarifixInfo.VariFixInformation.FixedInstallment : 0;
                //}
                //else
                //{
                variableInstalInit = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value - initialVarifixInfo.VariFixInformation.FixedInstallment : 0;
                //}
                txtVariableInstalmentInitialVarifix.Text = variableInstalInit.ToString(SAHL.Common.Constants.CurrencyFormat);
                txtVarifixFixedPercentageInitialVarifix.Text = Math.Round(initialVarifixInfo.VariFixInformation.FixedPercent * 100, 2).ToString() + " %";
                double varPercInit = 100 - (initialVarifixInfo.VariFixInformation.FixedPercent * 100);
                txtVarifixVariablePercentageInitialVarifix.Text = varPercInit.ToString() + " %";
                double fixedPortionInitial = _totalLoanRequiredInitial * initialVarifixInfo.VariFixInformation.FixedPercent;
                txtFixedPortionInitialVarifix.Text = fixedPortionInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
                double variablePortionInitial = _totalLoanRequiredInitial * (varPercInit / 100);
                txtVariablePortionInitialVarifix.Text = variablePortionInitial.ToString(SAHL.Common.Constants.CurrencyFormat);


                txtPTICurrentVarifix.Text = _vlInfoCurrent.PTI.HasValue ? Math.Round(_vlInfoCurrent.PTI.Value * 100, 2).ToString() + " %" : "-";
                txtLTVCurrentVarifix.Text = _vlInfoCurrent.LTV.HasValue ? Math.Round(_vlInfoCurrent.LTV.Value * 100, 2).ToString() + " %" : "-";
                txtFixedInstalmentCurrentVarifix.Text = currentVarifixInfo.VariFixInformation.FixedInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);

                double varCurrentVarifix = 0;
                //if (_application.HasAttribute(SAHL.Common.Globals.OfferAttributeTypes.CapitalizeFees))
                //{
                //    varCurrentVarifix = _vlInfoCurrent.MonthlyInstalment.HasValue &&_vlInfoCurrent.FeesTotal.HasValue ? _vlInfoCurrent.MonthlyInstalment.Value + _vlInfoCurrent.FeesTotal.Value - currentVarifixInfo.VariFixInformation.FixedInstallment : 0;
                //}
                //else
                //{
                varCurrentVarifix = _vlInfoCurrent.MonthlyInstalment.HasValue ? _vlInfoCurrent.MonthlyInstalment.Value - currentVarifixInfo.VariFixInformation.FixedInstallment : 0;
                //}

                txtVariableInstalmentCurrentVarifix.Text = varCurrentVarifix.ToString(SAHL.Common.Constants.CurrencyFormat);
                txtVarifixFixedPercentageCurrentVarifix.Text = Math.Round(currentVarifixInfo.VariFixInformation.FixedPercent * 100, 2).ToString() + " %";
                double varPercCurrent = 100 - (currentVarifixInfo.VariFixInformation.FixedPercent * 100);
                txtVarifixVariablePercentageCurrentVarifix.Text = varPercCurrent.ToString() + " %";
                double fixedPortionCurrent = _totalLoanRequiredCurrent * currentVarifixInfo.VariFixInformation.FixedPercent;
                txtFixedPortionCurrentVarifix.Text = fixedPortionCurrent.ToString(SAHL.Common.Constants.CurrencyFormat);
                double variablePortionCurrent = _totalLoanRequiredCurrent * (varPercCurrent / 100);
                txtVariablePortionCurrentVarifix.Text = variablePortionCurrent.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
            else
            {
                txtLTVCurrentVarifix.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";
                txtPTICurrentVarifix.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";

                double initialVarInstallment = 0;
                //if (_application.HasAttribute(SAHL.Common.Globals.OfferAttributeTypes.CapitalizeFees))
                //{
                //    initialVarInstallment = _vlInfoInitial.MonthlyInstalment.HasValue && _vlInfoInitial.FeesTotal.HasValue ? _vlInfoInitial.MonthlyInstalment.Value + _vlInfoInitial.FeesTotal.Value - initialVarifixInfo.VariFixInformation.FixedInstallment : 0;
                //}
                //else
                //{
                initialVarInstallment = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value - initialVarifixInfo.VariFixInformation.FixedInstallment : 0;
                //}

                txtFixedInstalmentCurrentVarifix.Text = initialVarifixInfo.VariFixInformation.FixedInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
                txtVariableInstalmentCurrentVarifix.Text = initialVarInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
                txtVarifixFixedPercentageCurrentVarifix.Text = Math.Round(initialVarifixInfo.VariFixInformation.FixedPercent * 100, 2).ToString() + " %";
                double varPercInit = 100 - (initialVarifixInfo.VariFixInformation.FixedPercent * 100);
                txtVarifixVariablePercentageCurrentVarifix.Text = varPercInit.ToString() + " %";
                double fixedPortionInitial = _totalLoanRequiredInitial * initialVarifixInfo.VariFixInformation.FixedPercent;
                txtFixedPortionCurrentVarifix.Text = fixedPortionInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
                double variablePortionInitial = _totalLoanRequiredInitial * (varPercInit / 100);
                txtVariablePortionCurrentVarifix.Text = variablePortionInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        public void PopulateApplicationTypeRefinance()
        {
            rowCashOut.Visible = true;
            lblCashOut.Visible = true;
            lblCashOut.Text = "Cash Out";


            IApplicationInformationVariableLoanForSwitchAndRefinance vlSwitchRefinanceInfoInitial = _vlInfoInitial as IApplicationInformationVariableLoanForSwitchAndRefinance;
            IApplicationInformationVariableLoanForSwitchAndRefinance vlSwitchRefinanceInfoCurrent = null;
            if (vlSwitchRefinanceInfoInitial == null)
            {
                throw new InvalidCastException();
            }

            double cashoutInitial = 0;
            double cashoutCurrent = 0;

            rowFees.Visible = true;
            lblFees.Visible = true;
            double feesInitial = 0;
            double feesCurrent = 0;

            feesInitial = _vlInfoInitial.FeesTotal.HasValue ? _vlInfoInitial.FeesTotal.Value : 0;
            cashoutInitial = vlSwitchRefinanceInfoInitial.RequestedCashAmount.HasValue ? vlSwitchRefinanceInfoInitial.RequestedCashAmount.Value : 0;
            if (_singleRec == false)
            {
                if (_vlInfoCurrent != null)
                {
                    txtCashOutInitial.Text = cashoutInitial.ToString(SAHL.Common.Constants.CurrencyFormat);

                    vlSwitchRefinanceInfoCurrent = _vlInfoCurrent as IApplicationInformationVariableLoanForSwitchAndRefinance;
                    if (_vlInfoCurrent == null)
                        throw new InvalidCastException();

                    cashoutCurrent = vlSwitchRefinanceInfoCurrent.RequestedCashAmount.HasValue ? vlSwitchRefinanceInfoCurrent.RequestedCashAmount.Value : 0;
                    txtCashOutInitial.Visible = true;
                    txtCashOutCurrent.Visible = true;
                    txtFeesInitial.Text = feesInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
                    txtCashOutCurrent.Text = cashoutCurrent.ToString(SAHL.Common.Constants.CurrencyFormat);
                    txtFeesCurrent.Visible = true;
                    feesCurrent = _vlInfoCurrent.FeesTotal.HasValue ? _vlInfoCurrent.FeesTotal.Value : 0;
                    txtFeesCurrent.Text = feesCurrent.ToString(SAHL.Common.Constants.CurrencyFormat);
                    _totalLoanRequiredCurrent = cashoutCurrent + feesCurrent;
                    txtTotalLoanRequiredCurrent.Text = _vlInfoCurrent.LoanAgreementAmount.HasValue ? _vlInfoCurrent.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                }
            }
            else
            {
                txtCashOutCurrent.Text = cashoutInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
                txtFeesCurrent.Text = feesInitial.ToString(SAHL.Common.Constants.CurrencyFormat);

                _totalLoanRequiredCurrent = cashoutCurrent + feesCurrent;
                txtTotalLoanRequiredCurrent.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? _vlInfoInitial.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            }

            lblTotalLoanRequired.Visible = true;
            _totalLoanRequiredInitial = cashoutInitial + feesInitial;
            if (_singleRec)
            {
                txtTotalLoanRequiredCurrent.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? _vlInfoInitial.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            }
            else
            {
                txtTotalLoanRequiredInitial.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? _vlInfoInitial.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateApplicationTypeSwitch()
        {
            lblExistingLoan.Visible = true;
            rowExistingLoan.Visible = true;
            lblCashOut.Visible = true;
            rowCashOut.Visible = true;
            lblInterimInterest.Visible = true;
            rowInterimInterest.Visible = true;
            rowFees.Visible = true;

            IApplicationInformationVariableLoanForSwitchAndRefinance vlSwitchRefinanceInfoInitial = null;
            IApplicationInformationVariableLoanForSwitchAndRefinance vlSwitchRefinanceInfoCurrent = null;

            vlSwitchRefinanceInfoInitial = _vlInfoInitial as IApplicationInformationVariableLoanForSwitchAndRefinance;

            if (_vlInfoInitial == null)
                throw new InvalidCastException();

            if (!_singleRec)
            {
                txtCashOutInitial.Visible = true;
                txtExistingLoanInitial.Visible = true;
                txtInterimInterestInitial.Visible = true;
                vlSwitchRefinanceInfoCurrent = _vlInfoCurrent as IApplicationInformationVariableLoanForSwitchAndRefinance;
                if (_vlInfoCurrent == null)
                    throw new InvalidCastException();

            }

            double existingLoanInitial = _vlInfoInitial.ExistingLoan.HasValue ? _vlInfoInitial.ExistingLoan.Value : 0;
            txtExistingLoanInitial.Text = existingLoanInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
            double existingLoanCurrent = 0;
            if (_singleRec == false)
            {
                txtExistingLoanCurrent.Visible = true;
                existingLoanCurrent = _vlInfoCurrent.ExistingLoan.HasValue ? _vlInfoCurrent.ExistingLoan.Value : 0;
                txtExistingLoanCurrent.Text = existingLoanCurrent.ToString(SAHL.Common.Constants.CurrencyFormat);

            }
            else
            {
                existingLoanCurrent = _vlInfoInitial.ExistingLoan.HasValue ? _vlInfoInitial.ExistingLoan.Value : 0;
                txtExistingLoanCurrent.Text = existingLoanCurrent.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            double cashoutInitial = 0;

            if (vlSwitchRefinanceInfoInitial.RequestedCashAmount != null)
            {
                cashoutInitial = vlSwitchRefinanceInfoInitial.RequestedCashAmount.HasValue ? vlSwitchRefinanceInfoInitial.RequestedCashAmount.Value : 0;
            }

            txtCashOutInitial.Text = cashoutInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
            double cashoutCurrent = 0;
            if (_singleRec == false)
            {
                txtCashOutCurrent.Visible = true;
                if (vlSwitchRefinanceInfoCurrent.RequestedCashAmount != null & vlSwitchRefinanceInfoCurrent.RequestedCashAmount.HasValue)
                {
                    cashoutCurrent = vlSwitchRefinanceInfoCurrent.RequestedCashAmount.Value;
                }
                txtCashOutCurrent.Text = cashoutCurrent.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
            else
            {
                txtCashOutCurrent.Text = cashoutInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            lblInterimInterest.Visible = true;
            double interimInterestInitial = _vlInfoInitial.InterimInterest.HasValue ? _vlInfoInitial.InterimInterest.Value : 0;
            txtInterimInterestInitial.Text = interimInterestInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
            double interimInterestCurrent = 0;
            if (_singleRec == false)
            {
                txtInterimInterestCurrent.Visible = true;
                interimInterestCurrent = _vlInfoCurrent.InterimInterest.HasValue ? _vlInfoCurrent.InterimInterest.Value : 0;
                txtInterimInterestCurrent.Text = interimInterestCurrent.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
            else
            {
                txtInterimInterestCurrent.Text = interimInterestInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            lblFees.Visible = true;
            double feesInitial = 0;
            if (_vlInfoInitial != null)
            {
                feesInitial = _vlInfoInitial.FeesTotal.HasValue ? _vlInfoInitial.FeesTotal.Value : 0;
                if (_singleRec)
                {
                    txtFeesCurrent.Text = feesInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
                else
                {
                    txtFeesInitial.Text = feesInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
            }
            double feesCurrent = 0;
            if (_singleRec == false && _vlInfoCurrent != null)
            {
                txtFeesCurrent.Visible = true;
                feesCurrent = _vlInfoCurrent.FeesTotal.HasValue ? _vlInfoCurrent.FeesTotal.Value : 0;
                txtFeesCurrent.Text = feesCurrent.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            lblTotalLoanRequired.Visible = true;
            if (_vlInfoInitial != null)
            {
                _totalLoanRequiredInitial = existingLoanInitial + cashoutInitial + interimInterestInitial + feesInitial;
                if (_singleRec)
                {
                    txtTotalLoanRequiredCurrent.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? _vlInfoInitial.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                }
                else
                {
                    txtTotalLoanRequiredInitial.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? _vlInfoInitial.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                }
            }
            if (_singleRec == false && _vlInfoCurrent != null)
            {
                txtTotalLoanRequiredCurrent.Visible = true;
                _totalLoanRequiredCurrent = existingLoanCurrent + cashoutCurrent + +interimInterestCurrent + feesCurrent;
                txtTotalLoanRequiredCurrent.Text = _vlInfoCurrent.LoanAgreementAmount.HasValue ? _vlInfoCurrent.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateApplicationTypeNewPurchase()
        {
            lblPurchasePrice.Visible = true;
            if (!_singleRec)
            {
                txtPurchasePriceInitial.Visible = true;
                txtCashDepositInitial.Visible = true;
            }
            rowPurchasePrice.Visible = true;
            rowCashDeposit.Visible = true;
            rowFees.Visible = true;
            lblExistingLoan.Visible = false;
            rowExistingLoan.Visible = false;
            txtExistingLoanCurrent.Visible = false;
            txtExistingLoanInitial.Visible = false;


            IApplicationMortgageLoanNewPurchase mlp = _application as IApplicationMortgageLoanNewPurchase;
            if (mlp == null)
                throw new InvalidCastException();

            //PP = LAA + Cashdeposit - Fees (if fees flagged as capitalised)
            double purchasePriceInitial = 0; //mlp.PurchasePrice.HasValue ? mlp.PurchasePrice.Value : 0;
            if (_vlInfoInitial != null)
                purchasePriceInitial = (_vlInfoInitial.LoanAgreementAmount.HasValue ? _vlInfoInitial.LoanAgreementAmount.Value : 0) + (_vlInfoInitial.CashDeposit.HasValue ? _vlInfoInitial.CashDeposit.Value : 0);

            if (_singleRec)
            {
                txtPurchasePriceCurrent.Text = purchasePriceInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
            else
            {
                txtPurchasePriceInitial.Text = purchasePriceInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            double purchasePriceCurrent = 0;
            if (_singleRec == false && _vlInfoCurrent != null)
            {
                purchasePriceCurrent = (_vlInfoCurrent.LoanAgreementAmount.HasValue ? _vlInfoCurrent.LoanAgreementAmount.Value : 0) + (_vlInfoCurrent.CashDeposit.HasValue ? _vlInfoCurrent.CashDeposit.Value : 0);
                //mlp.PurchasePrice.HasValue ? mlp.PurchasePrice.Value : 0;
                txtPurchasePriceCurrent.Text = purchasePriceCurrent.ToString(SAHL.Common.Constants.CurrencyFormat);
                txtPurchasePriceCurrent.Visible = true;
            }

            lblCashDeposit.Visible = true;
            double cashDepositInitial = 0;
            if (_vlInfoInitial != null)
            {
                cashDepositInitial = _vlInfoInitial.CashDeposit.HasValue ? _vlInfoInitial.CashDeposit.Value : 0;
                if (_singleRec)
                {
                    txtCashDepositCurrent.Text = cashDepositInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
                else
                {
                    txtCashDepositInitial.Text = cashDepositInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
            }

            double cashDepositCurrent = 0;
            if (_singleRec == false && _vlInfoCurrent != null)
            {
                txtCashDepositCurrent.Visible = true;
                cashDepositCurrent = _vlInfoCurrent.CashDeposit.HasValue ? _vlInfoCurrent.CashDeposit.Value : 0;
                txtCashDepositCurrent.Text = cashDepositCurrent.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            double feesInitial = 0;
            lblFees.Visible = true;
            if (_vlInfoInitial != null)
            {
                feesInitial = _vlInfoInitial.FeesTotal.HasValue ? _vlInfoInitial.FeesTotal.Value : 0;
                if (_singleRec)
                {
                    txtFeesCurrent.Text = feesInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
                else
                {
                    txtFeesInitial.Text = feesInitial.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
            }
            double feesCurrent = 0;

            if (_singleRec == false && _vlInfoCurrent != null)
            {
                txtFeesCurrent.Visible = true;
                feesCurrent = _vlInfoCurrent.FeesTotal.HasValue ? _vlInfoCurrent.FeesTotal.Value : 0;
                txtFeesCurrent.Text = feesCurrent.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            lblTotalLoanRequired.Visible = true;

            if (_vlInfoInitial != null)
            {
                _totalLoanRequiredInitial = purchasePriceInitial - cashDepositInitial + feesInitial;
                if (_singleRec)
                {
                    txtTotalLoanRequiredCurrent.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? _vlInfoInitial.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                }
                else
                {
                    txtTotalLoanRequiredInitial.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? _vlInfoInitial.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                }
            }

            if (_singleRec == false && _vlInfoCurrent != null)
            {
                txtTotalLoanRequiredCurrent.Visible = true;
                _totalLoanRequiredCurrent = purchasePriceCurrent - cashDepositCurrent + feesCurrent;
                txtTotalLoanRequiredCurrent.Text = _vlInfoCurrent.LoanAgreementAmount.HasValue ? _vlInfoCurrent.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            }

        }

        /// <summary>0
        /// 
        /// </summary>
        private void PopulateSameProductTypeSuperlo()
        {
            panelInstalmentDetails.Visible = true;
            lblPrePaymentThreshhold.Visible = true;
            txtPrepaymentThreshholdCurrent.Visible = true;
            rowPrepaymentThreshhold.Visible = true;

            lblInstalmentDetailsInitial.Visible = false;
            lblInstalmentDetailsCurrent.Visible = false;

            _ar = RepositoryFactory.GetRepository<IApplicationRepository>();
            ISupportsSuperLoApplicationInformation initialSuperloInfo = _initialInformationRec.ApplicationProduct as ISupportsSuperLoApplicationInformation;

            ISupportsInterestOnlyApplicationInformation interestOnlyInitial = _initialInformationRec.ApplicationProduct as ISupportsInterestOnlyApplicationInformation;
            ISupportsInterestOnlyApplicationInformation interestOnlyCurrent = null;

            ISupportsSuperLoApplicationInformation currentSuperloInfo = null;
            if (_currentInformationRec != null)
            {
                currentSuperloInfo = _currentInformationRec.ApplicationProduct as ISupportsSuperLoApplicationInformation;
            }
            if (!_singleRec)
            {

                interestOnlyCurrent = _currentInformationRec.ApplicationProduct as ISupportsInterestOnlyApplicationInformation;

                txtPTIInitial.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";
                txtLTVInitial.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";
                txtMonthlyInstalmentInitial.Text = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                txtPrepaymentThreshholdInitial.Text = initialSuperloInfo.SuperLoInformation.PPThresholdYr1.ToString(SAHL.Common.Constants.CurrencyFormat);

                txtPTICurrent.Text = _vlInfoCurrent.PTI.HasValue ? Math.Round(_vlInfoCurrent.PTI.Value * 100, 2).ToString() + " %" : "-";
                txtLTVCurrent.Text = _vlInfoCurrent.LTV.HasValue ? Math.Round(_vlInfoCurrent.LTV.Value * 100, 2).ToString() + " %" : "-";
                txtMonthlyInstalmentCurrent.Text = _vlInfoCurrent.MonthlyInstalment.HasValue ? _vlInfoCurrent.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                txtPrepaymentThreshholdCurrent.Text = currentSuperloInfo.SuperLoInformation.PPThresholdYr1.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
            else
            {
                txtPTICurrent.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";
                txtLTVCurrent.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";
                txtMonthlyInstalmentCurrent.Text = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                txtPrepaymentThreshholdCurrent.Text = initialSuperloInfo != null && initialSuperloInfo.SuperLoInformation != null ? initialSuperloInfo.SuperLoInformation.PPThresholdYr1.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            }

            if (interestOnlyInitial.InterestOnlyInformation != null || (interestOnlyCurrent != null && interestOnlyCurrent.InterestOnlyInformation != null))
            {
                lblInterestOnlyInstalment.Visible = true;
                lblMonthlyInstalment.Visible = false;
                txtMonthlyInstalmentCurrent.Visible = false;
                txtMonthlyInstalmentInitial.Visible = false;
                rowMonthlyInstalment.Visible = false;
                rowInterestOnly.Visible = true;
                lblAmortisingInstalment.Visible = true;
                rowAmortisingInstalment.Visible = true;
                txtInterestOnlyInstalmentCurrent.Visible = true;
                txtAmortisingInstalmentCurrent.Visible = true;
                if (!_singleRec)
                {
                    txtInterestOnlyInstalmentInitial.Visible = true;
                    txtAmortisingInstalmentInitial.Visible = true;
                    txtAmortisingInstalmentInitial.Text = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                    txtAmortisingInstalmentCurrent.Text = _vlInfoCurrent.MonthlyInstalment.HasValue ? _vlInfoCurrent.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                    if (interestOnlyInitial.InterestOnlyInformation != null && interestOnlyInitial.InterestOnlyInformation.Installment.HasValue)
                        txtInterestOnlyInstalmentInitial.Text = interestOnlyInitial.InterestOnlyInformation.Installment.HasValue ? interestOnlyInitial.InterestOnlyInformation.Installment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                    else
                        txtInterestOnlyInstalmentInitial.Text = "-";
                    if (interestOnlyCurrent.InterestOnlyInformation != null)

                        txtInterestOnlyInstalmentCurrent.Text = interestOnlyCurrent.InterestOnlyInformation.Installment.HasValue ? interestOnlyCurrent.InterestOnlyInformation.Installment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                    else
                        txtInterestOnlyInstalmentCurrent.Text = "-";
                }
                else
                {
                    if (interestOnlyInitial.InterestOnlyInformation != null)
                        txtInterestOnlyInstalmentCurrent.Text = interestOnlyInitial.InterestOnlyInformation.Installment.HasValue ? interestOnlyInitial.InterestOnlyInformation.Installment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                    else
                        txtInterestOnlyInstalmentCurrent.Text = "-";
                    txtAmortisingInstalmentCurrent.Text = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                }
            }
        }


        private void PopulateSameProductTypeEdge()
        {
            ISupportsEdgeApplicationInformation initialEHL = null;
            ISupportsEdgeApplicationInformation currentEHL = null;

            panelInstalmentDetailsEdge.Visible = true;
            if (!_singleRec)
            {
                initialEHL = _initialInformationRec.ApplicationProduct as ISupportsEdgeApplicationInformation;
                currentEHL = _currentInformationRec.ApplicationProduct as ISupportsEdgeApplicationInformation;
                if (initialEHL != null)
                {
                    txtPTIInitialEHL.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";
                    txtLTVInitialEHL.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";
                    txtTotalLoanRequiredInitial.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? _vlInfoInitial.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                    txtInterestOnlyInstalmentInitialEHL.Text = initialEHL.EdgeInformation.InterestOnlyInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
                    txtAmortisingInstalmentInitialEHL.Text = initialEHL.EdgeInformation.AmortisationTermInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
                    txtullTermInitialEHL.Text = initialEHL.EdgeInformation.FullTermInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
                }

                if (currentEHL != null)
                {
                    txtTotalLoanRequiredCurrent.Text = _vlInfoCurrent.LoanAgreementAmount.HasValue ? _vlInfoCurrent.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                    txtPTICurrentEHL.Text = _vlInfoCurrent.PTI.HasValue ? Math.Round(_vlInfoCurrent.PTI.Value * 100, 2).ToString() + " %" : "-";
                    txtLTVCurrentEHL.Text = _vlInfoCurrent.LTV.HasValue ? Math.Round(_vlInfoCurrent.LTV.Value * 100, 2).ToString() + " %" : "-";
                    txtInterestOnlyInstalmentCurrentEHL.Text = currentEHL.EdgeInformation.InterestOnlyInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
                    txtAmortisingInstalmentCurrentEHL.Text = currentEHL.EdgeInformation.AmortisationTermInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
                    txtullTermCurrentEHL.Text = currentEHL.EdgeInformation.FullTermInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
            }
            else
            {
                initialEHL = _initialInformationRec.ApplicationProduct as ISupportsEdgeApplicationInformation;
                lblInstalmentDetailsInitialEHL.Visible = false;
                lblInstalmentDetailsCurrentEHL.Visible = false;
                if (initialEHL != null)
                {
                    txtPTIInitialEHL.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";
                    txtLTVInitialEHL.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";
                    txtTotalLoanRequiredInitial.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? _vlInfoInitial.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                    txtInterestOnlyInstalmentInitialEHL.Text = initialEHL.EdgeInformation.InterestOnlyInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
                    txtAmortisingInstalmentInitialEHL.Text = initialEHL.EdgeInformation.AmortisationTermInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
                    txtullTermInitialEHL.Text = initialEHL.EdgeInformation.FullTermInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateSameProductTypeNewVariableLoan()
        {
            panelInstalmentDetails.Visible = true;
            txtPrepaymentThreshholdCurrent.Visible = false;
            txtPrepaymentThreshholdInitial.Visible = false;
            lblPrePaymentThreshhold.Visible = false;
            rowPrepaymentThreshhold.Visible = false;
            ISupportsInterestOnlyApplicationInformation interestOnlyInitial = _initialInformationRec.ApplicationProduct as ISupportsInterestOnlyApplicationInformation;
            ISupportsInterestOnlyApplicationInformation interestOnlyCurrent = null;

            if (!_singleRec)
            {
                _ar = RepositoryFactory.GetRepository<IApplicationRepository>();

                interestOnlyCurrent = _currentInformationRec.ApplicationProduct as ISupportsInterestOnlyApplicationInformation;

                txtPTIInitial.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";

                txtLTVInitial.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";

                txtMonthlyInstalmentInitial.Text = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";

                txtTotalLoanRequiredInitial.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? _vlInfoInitial.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";

                txtMonthlyInstalmentInitial.Text = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";

                txtTotalLoanRequiredCurrent.Text = _vlInfoCurrent.LoanAgreementAmount.HasValue ? _vlInfoCurrent.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";

                txtPTICurrent.Text = _vlInfoCurrent.PTI.HasValue ? Math.Round(_vlInfoCurrent.PTI.Value * 100, 2).ToString() + " %" : "-";
                txtLTVCurrent.Text = _vlInfoCurrent.LTV.HasValue ? Math.Round(_vlInfoCurrent.LTV.Value * 100, 2).ToString() + " %" : "-";
                txtMonthlyInstalmentCurrent.Text = _vlInfoCurrent.MonthlyInstalment.HasValue ? _vlInfoCurrent.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            }
            else
            {
                lblInstalmentDetailsInitial.Visible = false;
                lblInstalmentDetailsCurrent.Visible = false;
                txtLTVCurrent.Text = _vlInfoInitial.LTV.HasValue ? Math.Round(_vlInfoInitial.LTV.Value * 100, 2).ToString() + " %" : "-";
                txtPTICurrent.Text = _vlInfoInitial.PTI.HasValue ? Math.Round(_vlInfoInitial.PTI.Value * 100, 2).ToString() + " %" : "-";
                txtMonthlyInstalmentCurrent.Text = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                txtTotalLoanRequiredCurrent.Text = _vlInfoInitial.LoanAgreementAmount.HasValue ? _vlInfoInitial.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            }

            if (interestOnlyInitial.InterestOnlyInformation != null || (interestOnlyCurrent != null && interestOnlyCurrent.InterestOnlyInformation != null))
            {
                lblInterestOnlyInstalment.Visible = true;
                rowInterestOnly.Visible = true;
                lblMonthlyInstalment.Visible = false;
                txtMonthlyInstalmentCurrent.Visible = false;
                txtMonthlyInstalmentInitial.Visible = false;
                rowMonthlyInstalment.Visible = false;
                lblAmortisingInstalment.Visible = true;
                rowAmortisingInstalment.Visible = true;
                txtInterestOnlyInstalmentCurrent.Visible = true;
                txtAmortisingInstalmentCurrent.Visible = true;
                if (!_singleRec)
                {
                    txtInterestOnlyInstalmentInitial.Visible = true;
                    txtAmortisingInstalmentInitial.Visible = true;
                    txtAmortisingInstalmentInitial.Text = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                    txtAmortisingInstalmentCurrent.Text = _vlInfoCurrent.MonthlyInstalment.HasValue ? _vlInfoCurrent.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                    if (interestOnlyInitial.InterestOnlyInformation != null && interestOnlyInitial.InterestOnlyInformation.Installment != null && interestOnlyInitial.InterestOnlyInformation.Installment.HasValue)
                        txtInterestOnlyInstalmentInitial.Text = interestOnlyInitial.InterestOnlyInformation.Installment.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                    else
                        txtInterestOnlyInstalmentInitial.Text = "-";
                    if (interestOnlyCurrent.InterestOnlyInformation != null && interestOnlyCurrent.InterestOnlyInformation.Installment != null && interestOnlyCurrent.InterestOnlyInformation.Installment.HasValue)

                        txtInterestOnlyInstalmentCurrent.Text = interestOnlyCurrent.InterestOnlyInformation.Installment.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                    else
                        txtInterestOnlyInstalmentCurrent.Text = "-";
                }
                else
                {
                    if (interestOnlyInitial.InterestOnlyInformation != null && interestOnlyInitial.InterestOnlyInformation.Installment != null && interestOnlyInitial.InterestOnlyInformation.Installment.HasValue)
                        txtInterestOnlyInstalmentCurrent.Text = interestOnlyInitial.InterestOnlyInformation.Installment.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                    else
                        txtInterestOnlyInstalmentCurrent.Text = "-";
                    txtAmortisingInstalmentCurrent.Text = _vlInfoInitial.MonthlyInstalment.HasValue ? _vlInfoInitial.MonthlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void SetColor()
        {
            if (txtExistingLoanInitial.Text != txtExistingLoanCurrent.Text)
                txtExistingLoanCurrent.ForeColor = Color.Red;

            if (txtTotalLoanRequiredInitial.Text != txtTotalLoanRequiredCurrent.Text)
                txtTotalLoanRequiredCurrent.ForeColor = Color.Red;

            if (txtCashOutInitial.Text != txtCashOutCurrent.Text)
                txtCashOutCurrent.ForeColor = Color.Red;

            if (txtFeesInitial.Text != txtFeesCurrent.Text)
                txtFeesCurrent.ForeColor = Color.Red;

            if (txtInterimInterestInitial.Text != txtInterimInterestCurrent.Text)
                txtInterimInterestCurrent.ForeColor = Color.Red;

            if (txtPurchasePriceInitial.Text != txtPurchasePriceCurrent.Text)
                txtPurchasePriceCurrent.ForeColor = Color.Red;

            if (txtCashDepositInitial.Text != txtCashDepositCurrent.Text)
                txtCashDepositCurrent.ForeColor = Color.Red;

            if (txtPTICurrent.Text != txtPTIInitial.Text)
                txtPTICurrent.ForeColor = Color.Red;

            if (txtPrepaymentThresholdVarifixCurrent.Text != txtPrepaymentThreshholdInitial.Text)
                txtPrepaymentThresholdVarifixCurrent.ForeColor = Color.Red;

            if (txtLTVCurrent.Text != txtLTVInitial.Text)
                txtLTVCurrent.ForeColor = Color.Red;

            if (txtMonthlyInstalmentCurrent.Text != txtMonthlyInstalmentInitial.Text)
                txtMonthlyInstalmentCurrent.ForeColor = Color.Red;

            if (txtPrepaymentThreshholdCurrent.Text != txtPrepaymentThreshholdInitial.Text)
                txtPrepaymentThreshholdCurrent.ForeColor = Color.Red;

            if (txtTermCurrent.Text != txtTermInitial.Text)
                txtTermCurrent.ForeColor = Color.Red;

            if (txtLinkRateCurrent.Text != txtLinkRateInitial.Text)
                txtLinkRateCurrent.ForeColor = Color.Red;

            if (txtEffectiveRateCurrentNew.Text != txtEffectiveRateInitial.Text)
                txtEffectiveRateCurrentNew.ForeColor = Color.Red;

            if (txtDiscountedLinkRateCurrent.Text != txtDiscountedLinkRateInitial.Text)
                txtDiscountedLinkRateCurrent.ForeColor = Color.Red;

            if (txtBondToRegisterCurrent.Text != txtBondToRegisterInitial.Text)
                txtBondToRegisterCurrent.ForeColor = Color.Red;

            if (txtTotalInstalmentCurrentVarifix.Text != txtTotalInstalmentInitialVarifix.Text)
                txtTotalInstalmentCurrentVarifix.ForeColor = Color.Red;

            if (txtEstPropertyValueCurrent.Text != txtEstPropertyValueInitial.Text)
                txtEstPropertyValueCurrent.ForeColor = Color.Red;

            if (txtHouseholdIncomeCurrent.Text != txtHouseholdIncomeInitial.Text)
                txtHouseholdIncomeCurrent.ForeColor = Color.Red;

            if (txtCategoryCurrent.Text != txtCategoryInitial.Text)
                txtCategoryCurrent.ForeColor = Color.Red;

            if (txtSPVNameCurrent.Text != txtSPVNameInitial.Text)
                txtSPVNameCurrent.ForeColor = Color.Red;

            if (txtPTICurrentVarifix.Text != txtPTIInitialVarifix.Text)
                txtPTICurrentVarifix.ForeColor = Color.Red;

            if (txtLTVInitialVarifix.Text != txtLTVCurrentVarifix.Text)
                txtLTVCurrentVarifix.ForeColor = Color.Red;

            if (txtFixedInstalmentInitialVarifix.Text != txtFixedInstalmentCurrentVarifix.Text)
                txtFixedInstalmentCurrentVarifix.ForeColor = Color.Red;

            if (txtVariableInstalmentInitialVarifix.Text != txtVariableInstalmentCurrentVarifix.Text)
                txtVariableInstalmentCurrentVarifix.ForeColor = Color.Red;

            if (txtVarifixFixedPercentageInitialVarifix.Text != txtVarifixFixedPercentageCurrentVarifix.Text)
                txtVarifixFixedPercentageCurrentVarifix.ForeColor = Color.Red;

            if (txtVarifixVariablePercentageInitialVarifix.Text != txtVarifixVariablePercentageCurrentVarifix.Text)
                txtVarifixVariablePercentageCurrentVarifix.ForeColor = Color.Red;

            if (txtFixedPortionInitialVarifix.Text != txtFixedPortionCurrentVarifix.Text)
                txtFixedPortionCurrentVarifix.ForeColor = Color.Red;

            if (txtVariablePortionInitialVarifix.Text != txtVariablePortionCurrentVarifix.Text)
                txtVariablePortionCurrentVarifix.ForeColor = Color.Red;

            if (txtAmortisingInstalmentCurrent.Text != txtAmortisingInstalmentInitial.Text)
                txtAmortisingInstalmentCurrent.ForeColor = Color.Red;

            if (txtInterestOnlyInstalmentInitial.Text != txtInterestOnlyInstalmentCurrent.Text)
                txtInterestOnlyInstalmentCurrent.ForeColor = Color.Red;
        }

        private class GridItem
        {
            private string _infoKey;
            private string _revision;
            private string _dateRevised;
            private string _applicationType;
            private string _product;
            private string _reason;

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used for Binding to ASP.NET grid.")]
            public string InfoKey
            {
                get
                {
                    return _infoKey;
                }
                set
                {
                    _infoKey = value;
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used for Binding to ASP.NET grid.")]
            public string Revision
            {
                get
                {
                    return _revision;
                }
                set
                {
                    _revision = value;
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used for Binding to ASP.NET grid.")]
            public string DateRevised
            {
                get
                {
                    return _dateRevised;
                }
                set
                {
                    _dateRevised = value;
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used for Binding to ASP.NET grid.")]
            public string ApplicationType
            {
                get
                {
                    return _applicationType;
                }
                set
                {
                    _applicationType = value;
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used for Binding to ASP.NET grid.")]
            public string Product
            {
                get
                {
                    return _product;
                }
                set
                {
                    _product = value;
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used for Binding to ASP.NET grid.")]
            public string Reason
            {
                get
                {
                    return _reason;
                }
                set
                {
                    _reason = value;
                }
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            BindControls();
        }

        protected void gridHistory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtLinkRateInitial_TextChanged(object sender, EventArgs e)
        {

        }

        public ILookupRepository lookupRepo
        {
            get
            {
                if (_lRepo == null)
                    _lRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lRepo;
            }
        }

    }
}
