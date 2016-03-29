using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common
{

    /// <summary>
    /// Legal entity asset/liability view.
    /// </summary>
    public partial class Views_LegalEntityAssetLiability : SAHLCommonBaseView, ILegalEntityAssetLiabilityDetails
    {
        bool _showUpdate;
        bool _isAddUpdate;
        private bool _applicationSummaryMode;

        double assetValue;
        double liabilityValue;
        ILookupRepository _lookups;
        IEventList<ILegalEntityAssetLiability> leAssetLiabilityDuplicates;

        protected void Page_Load(object sender, EventArgs e)
        {
            _lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            rowType.Visible = _isAddUpdate;
            lblAssetType.Visible = !_showUpdate;
            ddlType.Visible = _showUpdate;
            btnDelete.Attributes["onclick"] = "if(!confirm('Are you sure you would like to delete this record ?')) return false";
        }

        public bool IsAddUpdate
        {
            set { _isAddUpdate = value; }
        }

        public IEventList<ILegalEntityAssetLiability> AssetLiabilityDuplicates
        {
            set { leAssetLiabilityDuplicates = value; }
        }

        public bool ApplicationSummaryMode
        {
            set { _applicationSummaryMode = value; }
        }

        public bool ShowUpdate
        {
            set
            {
                _showUpdate = value;
            }
        }

        public bool ShowCancelButton
        {
            set
            {
                btnCancel.Visible = value;
            }
        }

        public bool ShowUpdateButton
        {
            set
            {
                btnAddUpdate.Visible = value;
            }
        }

        public bool ShowDeleteButton
        {
            set
            {
                btnDelete.Visible = value;
            }
        }

        public void ShowpnlAssociate(bool ShowDisplay)
        {
            pnlAssociate.Visible = true;

            if (ShowDisplay)
                lblAssociate.Visible = true;
            else
                ddlAssociate.Visible = true;

        }

        public DateTime DateAcquired
        {
            get { return Convert.ToDateTime(dtDateAcquired.Date); }
            set { dtDateAcquired.Date = value; }
        }

        public double AssetValue
        {
            get
            {
                if (txtAssetValue.Text.Length > 0)
                    return Convert.ToDouble(txtAssetValue.Amount);

                return 0;
            }
            set { txtAssetValue.Amount = value; }
        }

        public double LiabilityValue
        {
            get
            {
                if (txtLiabilityValue.Text.Length > 0)
                    return Convert.ToDouble(txtLiabilityValue.Text);

                return 0;
            }
            set { txtLiabilityValue.Amount = value; }
        }

        public void RestoreAssetTypeValueForFixedProperty(int AssetLiabilityType)
        {
            if (AssetLiabilityType > 0)
                ddlType.SelectedValue = AssetLiabilityType.ToString();
        }

        public void SetControlsForAddUpdateFixedProperty()
        {
            rowDateAcquired.Visible = true;
            rowAddress.Visible = true;
            rowAssetValue.Visible = true;
            rowLiabilityValue.Visible = true;
            lblAddress.Visible = false;
            if (ddlAddress.SelectedValue == "-select-")
                btnAssociateAddress.Visible = true;
        }

        public void SetControlsForAddInvestment()
        {
            rowCompanyName.Visible = true;
            rowAssetValue.Visible = true;
        }

        public void SetControlsForAddUpdateLiabilityLoan()
        {
            rowDateRepayable.Visible = true;
            rowFinancialInstitution.Visible = true;
            rowInstalmentValue.Visible = true;
            rowLiabilityValue.Visible = true;
            rowSubType.Visible = true;
        }

        public void SetControlsForAddUpdateLiabilitySurtey()
        {
            rowDescription.Visible = true;
            rowLiabilityValue.Visible = true;
            rowAssetValue.Visible = true;
        }

        public void SetControlsForAddUpdateLifeAssurance()
        {
            rowCompanyName.Visible = true;
            rowSurrenderValue.Visible = true;
        }

        public void SetControlsForAddUpdateOther()
        {
            rowAssetValue.Visible = true;
            rowDescription.Visible = true;
            rowLiabilityValue.Visible = true;
        }

        public void SetControlsForAddUpdateFixedLongTermInvestment()
        {
            rowCompanyName.Visible = true;
            rowLiabilityValue.Visible = true;
        }

        public void BindAssetLiabilityTypes(IEventList<IAssetLiabilityType> assetLiablilityTypes)
        {
            ddlType.DataValueField = "Key";
            ddlType.DataTextField = "Description";
            ddlType.DataSource = assetLiablilityTypes;
            ddlType.DataBind();

            ddlType.Visible = true;
        }

        public void BindAssetLiabilitySubTypes(IEventList<IAssetLiabilitySubType> assetLiablilitySubTypes)
        {
            ddlSubType.DataValueField = "Key";
            ddlSubType.DataTextField = "Description";
            ddlSubType.DataSource = assetLiablilitySubTypes;
            ddlSubType.DataBind();
        }

        public void BindAssociatedAssets(IEventList<ILegalEntityAssetLiability> leAssets)
        {
            Dictionary<int, string> leAssetsDict = new Dictionary<int, string>();

            if (leAssets != null)
                for (int e = 0; e < leAssets.Count; e++)
                {
                    string description = "";

                    if (leAssets[e].AssetLiability is IAssetLiabilityFixedProperty)
                        description += AssetLiabilityTypes.FixedProperty.ToString();
                    if (leAssets[e].AssetLiability is IAssetLiabilityInvestmentPrivate)
                        description += AssetLiabilityTypes.UnlistedInvestments.ToString();
                    if (leAssets[e].AssetLiability is IAssetLiabilityInvestmentPublic)
                        description += AssetLiabilityTypes.ListedInvestments.ToString();
                    if (leAssets[e].AssetLiability is IAssetLiabilityLiabilityLoan)
                        description += AssetLiabilityTypes.LiabilityLoan.ToString();
                    if (leAssets[e].AssetLiability is IAssetLiabilityLiabilitySurety)
                        description += AssetLiabilityTypes.LiabilitySurety.ToString();
                    if (leAssets[e].AssetLiability is IAssetLiabilityLifeAssurance)
                        description += AssetLiabilityTypes.LifeAssurance.ToString();
                    if (leAssets[e].AssetLiability is IAssetLiabilityFixedLongTermInvestment)
                        description += AssetLiabilityTypes.FixedLongTermInvestment.ToString();
                    if (leAssets[e].AssetLiability is IAssetLiabilityOther)
                        description += AssetLiabilityTypes.OtherAsset.ToString();
          
                    description += ", " + leAssets[e].LegalEntity.DisplayName;

                    if (!leAssetsDict.ContainsKey(leAssets[e].Key))
                        leAssetsDict.Add(leAssets[e].Key, description);
                }

            ddlAssociate.DataValueField = "Key";
            ddlAssociate.DataTextField = "Description";
            ddlAssociate.DataSource = leAssetsDict;
            ddlAssociate.DataBind();
        }


        public void BindlegalEntityAddress(IDictionary<string, string> leAddressLst)
        {
            ddlAddress.DataValueField = "Key";
            ddlAddress.DataTextField = "Values";
            ddlAddress.DataSource = leAddressLst;
            ddlAddress.DataBind();
        }

        //public void BindlegalEntityAddress(IEventList<ILegalEntityAddress> leAddresses)
        //{
        //    IDictionary<string, string> leAddressLst = new Dictionary<string, string>();

        //    foreach (ILegalEntityAddress legalEntityAddress in leAddresses)
        //    {
        //        if (legalEntityAddress.AddressType.Key == (int)AddressTypes.Residential)
        //        {
        //            leAddressLst.Add(legalEntityAddress.Address.Key.ToString(), legalEntityAddress.Address.GetFormattedDescription(AddressDelimiters.Comma));
        //        }
        //    }

        //    ddlAddress.DataValueField = "Key";
        //    ddlAddress.DataTextField = "Values";
        //    ddlAddress.DataSource = leAddressLst;
        //    ddlAddress.DataBind();
        //}

        // TODO: This viewName parameter is BS!  Change this method signature to do the right thing!
        public void BindAssetLiabilityGrid(string viewName, IEventList<ILegalEntityAssetLiability> assetLiabilities)
        {
            if (_applicationSummaryMode)
                grdAssetLiability.HeaderCaption = "Assets & Liabilities";
            else
            {
                if (assetLiabilities == null || assetLiabilities.Count == 0)
                {
                    grdAssetLiability.HeaderCaption = "Assets & Liabilities";
                    grdAssetLiability.NullDataSetMessage = "There are no assets & liabilities ";
                }
                else
                    grdAssetLiability.HeaderCaption = "Assets & Liabilities : " + assetLiabilities[0].LegalEntity.DisplayName;
            }

            grdAssetLiability.AddGridBoundColumn("", "Type", Unit.Percentage(20), HorizontalAlign.Left, true);
            grdAssetLiability.AddGridBoundColumn("", "Legal Entity", Unit.Percentage(40), HorizontalAlign.Left, _applicationSummaryMode);
            grdAssetLiability.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Asset Value", false, Unit.Percentage(20), HorizontalAlign.Right, true);
            grdAssetLiability.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Liability Value", false, Unit.Percentage(20), HorizontalAlign.Right, true);
            grdAssetLiability.DataSource = assetLiabilities;

            if (viewName != "LegalEntityAssetLiabilityAdd" || viewName != "LegalEntityAssetLiabilityAssociate")
                grdAssetLiability.PostBackType = GridPostBackType.SingleClick;

            grdAssetLiability.DataBind();
        }

        public IAssetLiability GetAssetLiablityForAdd(IAssetLiability assetLiability)
        {
            IAssetLiabilityFixedProperty alFixedProperty = assetLiability as IAssetLiabilityFixedProperty;

            if (alFixedProperty != null)
            {
                IAddressRepository addressRepo = RepositoryFactory.GetRepository<IAddressRepository>();

                if (ddlAddress.SelectedValue != "-select-" && Convert.ToInt32(ddlAddress.SelectedValue) > 0)
                {
                    IAddress address = addressRepo.GetAddressByKey(Convert.ToInt32(ddlAddress.SelectedValue));
                    alFixedProperty.Address = address;
                }
                alFixedProperty.AssetValue = Convert.ToDouble(txtAssetValue.Amount);
                alFixedProperty.DateAcquired = dtDateAcquired.Date;
                alFixedProperty.LiabilityValue = Convert.ToDouble(txtLiabilityValue.Text);
            }

            IAssetLiabilityLifeAssurance alLifeAssurance = assetLiability as IAssetLiabilityLifeAssurance;

            if (alLifeAssurance != null)
            {
                alLifeAssurance.CompanyName = txtCompanyName.Text.Trim();
                alLifeAssurance.SurrenderValue = Convert.ToDouble(txtSurrenderValue.Amount);
                assetLiability = alLifeAssurance;
            }

            IAssetLiabilityInvestmentPrivate alInvestmentPrivate = assetLiability as IAssetLiabilityInvestmentPrivate;

            if (alInvestmentPrivate != null)
            {
                alInvestmentPrivate.AssetValue = Convert.ToDouble(txtAssetValue.Amount);
                alInvestmentPrivate.CompanyName = txtCompanyName.Text.Trim();
            }

            IAssetLiabilityInvestmentPublic alInvestmentPublic = assetLiability as IAssetLiabilityInvestmentPublic;

            if (alInvestmentPublic != null)
            {
                alInvestmentPublic.AssetValue = Convert.ToDouble(txtAssetValue.Amount);
                alInvestmentPublic.CompanyName = txtCompanyName.Text.Trim();
            }

            IAssetLiabilityLiabilityLoan alLiabilityLoan = assetLiability as IAssetLiabilityLiabilityLoan;

            if (alLiabilityLoan != null)
            {
                alLiabilityLoan.DateRepayable = dtDateRepayable.Date;
                alLiabilityLoan.FinancialInstitution = txtFinancialInstitution.Text.Trim();
                alLiabilityLoan.InstalmentValue = Convert.ToDouble(txtInstalmentValue.Amount);
                alLiabilityLoan.LiabilityValue = Convert.ToDouble(txtLiabilityValue.Text);
                if (ddlSubType.SelectedValue != "-select-")
                    alLiabilityLoan.LoanType = _lookups.AssetLiabilitySubTypes.ObjectDictionary[ddlSubType.SelectedValue];
            }

            IAssetLiabilityLiabilitySurety alLiabilitySurety = assetLiability as IAssetLiabilityLiabilitySurety;

            if (alLiabilitySurety != null)
            {
                alLiabilitySurety.AssetValue = Convert.ToDouble(txtAssetValue.Amount);
                alLiabilitySurety.Description = txtDescription.Text.Trim();
                alLiabilitySurety.LiabilityValue = Convert.ToDouble(txtLiabilityValue.Text);
            }

            IAssetLiabilityOther alOther = assetLiability as IAssetLiabilityOther;

            if (alOther != null)
            {
                alOther.AssetValue = Convert.ToDouble(txtAssetValue.Amount);
                alOther.Description = txtDescription.Text.Trim();
                alOther.LiabilityValue = Convert.ToDouble(txtLiabilityValue.Text);
            }

            IAssetLiabilityFixedLongTermInvestment assetLongTerm = assetLiability as IAssetLiabilityFixedLongTermInvestment;

            if (assetLongTerm != null)
            {
                assetLongTerm.CompanyName = txtCompanyName.Text.Trim();
                assetLongTerm.LiabilityValue = Convert.ToDouble(txtLiabilityValue.Text);
            }

            return assetLiability;
        }

        protected void grdAssetLiability_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            ILegalEntityAssetLiability al = e.Row.DataItem as ILegalEntityAssetLiability;

            if (e.Row.DataItem != null)
            {
                if (al != null)
                {
                    cells[1].Text = al.LegalEntity.DisplayName;

                    if (_applicationSummaryMode && leAssetLiabilityDuplicates != null) // Concatenate names for shared assets
                    {
                        for (int i = 0; i < leAssetLiabilityDuplicates.Count; i++)
                        {
                            if (leAssetLiabilityDuplicates[i].AssetLiability.Key == al.AssetLiability.Key)
                                cells[1].Text += " & " + leAssetLiabilityDuplicates[i].LegalEntity.DisplayName;
                        }
                    }

                    IAssetLiabilityFixedProperty fixedProperty = al.AssetLiability as IAssetLiabilityFixedProperty;


                    if (fixedProperty != null)
                    {
                        cells[0].Text = AssetLiabilityTypes.FixedProperty.ToString();
                        cells[2].Text = fixedProperty.AssetValue.ToString();
                        cells[3].Text = fixedProperty.LiabilityValue.ToString();

                        assetValue += fixedProperty.AssetValue;
                        liabilityValue += fixedProperty.LiabilityValue;
                    }

                    IAssetLiabilityInvestmentPrivate investmentPrivate = al.AssetLiability as IAssetLiabilityInvestmentPrivate;

                    if (investmentPrivate != null)
                    {
                        cells[0].Text = AssetLiabilityTypes.UnlistedInvestments.ToString();
                        cells[2].Text = investmentPrivate.AssetValue.ToString();
                        cells[3].Text = "0";

                        assetValue += investmentPrivate.AssetValue;
                    }

                    IAssetLiabilityInvestmentPublic investmentPublic = al.AssetLiability as IAssetLiabilityInvestmentPublic;

                    if (investmentPublic != null)
                    {
                        cells[0].Text = AssetLiabilityTypes.ListedInvestments.ToString();
                        cells[2].Text = investmentPublic.AssetValue.ToString();
                        cells[3].Text = "0";

                        assetValue += investmentPublic.AssetValue;
                    }

                    IAssetLiabilityLiabilityLoan liabilityLoan = al.AssetLiability as IAssetLiabilityLiabilityLoan;

                    if (liabilityLoan != null)
                    {
                        cells[0].Text = AssetLiabilityTypes.LiabilityLoan.ToString();
                        cells[2].Text = "0";
                        cells[3].Text = liabilityLoan.LiabilityValue.ToString();

                        liabilityValue += liabilityLoan.LiabilityValue;
                    }

                    IAssetLiabilityLiabilitySurety loanSurety = al.AssetLiability as IAssetLiabilityLiabilitySurety;

                    if (loanSurety != null)
                    {
                        cells[0].Text = AssetLiabilityTypes.LiabilitySurety.ToString();
                        cells[2].Text = loanSurety.AssetValue.ToString();
                        cells[3].Text = loanSurety.LiabilityValue.ToString();

                        liabilityValue += loanSurety.LiabilityValue.Value;
                        assetValue += loanSurety.AssetValue.Value;
                    }

                    IAssetLiabilityLifeAssurance lifeAssurance = al.AssetLiability as IAssetLiabilityLifeAssurance;

                    if (lifeAssurance != null)
                    {
                        cells[0].Text = AssetLiabilityTypes.LifeAssurance.ToString();
                        cells[2].Text = lifeAssurance.SurrenderValue.ToString();
                        cells[3].Text = "0";

                        assetValue += lifeAssurance.SurrenderValue;
                    }

                    IAssetLiabilityOther other = al.AssetLiability as IAssetLiabilityOther;

                    if (other != null)
                    {
                        cells[0].Text = AssetLiabilityTypes.OtherAsset.ToString();
                        cells[2].Text = other.AssetValue.ToString();
                        cells[3].Text = other.LiabilityValue.ToString();

                        assetValue += other.AssetValue;
                        liabilityValue += other.LiabilityValue;
                    }

                    IAssetLiabilityFixedLongTermInvestment assetFixedLongTerm = al.AssetLiability as IAssetLiabilityFixedLongTermInvestment;

                    if (assetFixedLongTerm != null)
                    {
                        cells[0].Text = AssetLiabilityTypes.FixedLongTermInvestment.ToString();
                        cells[2].Text = "0";
                        cells[3].Text = assetFixedLongTerm.LiabilityValue.ToString();

                        liabilityValue += assetFixedLongTerm.LiabilityValue;
                    }
                }

            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                grdAssetLiability.FooterStyle.Wrap = true;

                const string cellTextDescription = "Total";
                cells[0].Text = cellTextDescription;
                cells[2].Text = assetValue.ToString(SAHL.Common.Constants.CurrencyFormat);
                cells[2].HorizontalAlign = HorizontalAlign.Right;
                double netAssets = assetValue - liabilityValue;
                string cellAmount = liabilityValue.ToString(SAHL.Common.Constants.CurrencyFormat);
                cells[3].Text = cellAmount;
                cells[3].HorizontalAlign = HorizontalAlign.Right;

                lblFooter1.Text = "Net Assets (Liabilities) ";
                lblFooter3.Text = netAssets.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        public void BindDisplayPanel(ILegalEntityAssetLiability leAssetLiability)
        {
            IAssetLiabilityFixedProperty fixedProperty = leAssetLiability.AssetLiability as IAssetLiabilityFixedProperty;
            if (fixedProperty != null)
            {
                pnlFixedProperty.Visible = true;
                pnlInvestmentProperty.Visible = false;
                pnlLiabilityLoan.Visible = false;
                pnlLiabilitySurety.Visible = false;
                pnlLifeAssurance.Visible = false;
                pnlOther.Visible = false;

                fpType.Text = AssetLiabilityTypes.FixedProperty.ToString();
                if (fixedProperty.Address != null)
                    fpAddress.Text = fixedProperty.Address.GetFormattedDescription(AddressDelimiters.Comma);
                fpDateAcquired.Text = Convert.ToDateTime(fixedProperty.DateAcquired).ToString(SAHL.Common.Constants.DateFormat);
                fpAssetValue.Text = fixedProperty.AssetValue.ToString(SAHL.Common.Constants.CurrencyFormat);
                fpLiabilityValue.Text = fixedProperty.LiabilityValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            IAssetLiabilityInvestmentPrivate investmentPrivate = leAssetLiability.AssetLiability as IAssetLiabilityInvestmentPrivate;

            if (investmentPrivate != null)
            {
                pnlFixedProperty.Visible = false;
                pnlInvestmentProperty.Visible = true;
                pnlLiabilityLoan.Visible = false;
                pnlLiabilitySurety.Visible = false;
                pnlLifeAssurance.Visible = false;
                pnlOther.Visible = false;

                InvestmentPropertyType.Text = AssetLiabilityTypes.UnlistedInvestments.ToString();
                InvestmentpropertyCompanyName.Text = investmentPrivate.CompanyName;
                InvestmentpropertyAssetValue.Text = investmentPrivate.AssetValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            IAssetLiabilityInvestmentPublic investmentPublic = leAssetLiability.AssetLiability as IAssetLiabilityInvestmentPublic;

            if (investmentPublic != null)
            {
                pnlFixedProperty.Visible = false;
                pnlInvestmentProperty.Visible = true;
                pnlLiabilityLoan.Visible = false;
                pnlLiabilitySurety.Visible = false;
                pnlLifeAssurance.Visible = false;
                pnlOther.Visible = false;

                InvestmentPropertyType.Text = AssetLiabilityTypes.ListedInvestments.ToString();
                InvestmentpropertyCompanyName.Text = investmentPublic.CompanyName;
                InvestmentpropertyAssetValue.Text = investmentPublic.AssetValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            IAssetLiabilityLiabilityLoan liabilityLoan = leAssetLiability.AssetLiability as IAssetLiabilityLiabilityLoan;

            if (liabilityLoan != null)
            {
                pnlFixedProperty.Visible = false;
                pnlInvestmentProperty.Visible = false;
                pnlLiabilityLoan.Visible = true;
                pnlLiabilitySurety.Visible = false;
                pnlLifeAssurance.Visible = false;
                pnlOther.Visible = false;

                llType.Text = AssetLiabilityTypes.LiabilityLoan.ToString();
                llDateRepayable.Text = liabilityLoan.DateRepayable == null ? "-" : Convert.ToDateTime(liabilityLoan.DateRepayable).ToString(SAHL.Common.Constants.DateFormat);
                llFinInstitution.Text = liabilityLoan.FinancialInstitution;
                llInstallmentValue.Text = liabilityLoan.InstalmentValue.ToString(SAHL.Common.Constants.CurrencyFormat);
                llLiabilityValue.Text = liabilityLoan.LiabilityValue.ToString(SAHL.Common.Constants.CurrencyFormat);
                if (liabilityLoan.LoanType != null)
                    llLoanType.Text = liabilityLoan.LoanType.Description;
            }

            IAssetLiabilityLiabilitySurety loanSurety = leAssetLiability.AssetLiability as IAssetLiabilityLiabilitySurety;

            if (loanSurety != null)
            {
                pnlFixedProperty.Visible = false;
                pnlInvestmentProperty.Visible = false;
                pnlLiabilityLoan.Visible = false;
                pnlLiabilitySurety.Visible = true;
                pnlLifeAssurance.Visible = false;
                pnlOther.Visible = false;

                lsType.Text = AssetLiabilityTypes.LiabilitySurety.ToString();
                lsAssetValue.Text = Convert.ToDouble(loanSurety.AssetValue).ToString(SAHL.Common.Constants.CurrencyFormat);
                lsDescription.Text = loanSurety.Description;
                lsLiabilityValue.Text = Convert.ToDouble(loanSurety.LiabilityValue).ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            IAssetLiabilityLifeAssurance lifeAssurance = leAssetLiability.AssetLiability as IAssetLiabilityLifeAssurance;

            if (lifeAssurance != null)
            {
                pnlFixedProperty.Visible = false;
                pnlInvestmentProperty.Visible = false;
                pnlLiabilityLoan.Visible = false;
                pnlLiabilitySurety.Visible = false;
                pnlLifeAssurance.Visible = true;
                pnlOther.Visible = false;

                laType.Text = AssetLiabilityTypes.LifeAssurance.ToString();
                laCompanyName.Text = lifeAssurance.CompanyName;
                laSurrenderValue.Text = lifeAssurance.SurrenderValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            IAssetLiabilityOther other = leAssetLiability.AssetLiability as IAssetLiabilityOther;

            if (other != null)
            {
                pnlFixedProperty.Visible = false;
                pnlInvestmentProperty.Visible = false;
                pnlLiabilityLoan.Visible = false;
                pnlLiabilitySurety.Visible = false;
                pnlLifeAssurance.Visible = false;
                pnlOther.Visible = true;

                otherType.Text = AssetLiabilityTypes.OtherAsset.ToString();
                otherDescription.Text = other.Description;
                otherAssetValue.Text = other.AssetValue.ToString(SAHL.Common.Constants.CurrencyFormat);
                otherLiabilityValue.Text = other.LiabilityValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            IAssetLiabilityFixedLongTermInvestment fixedLongTermInvestment = leAssetLiability.AssetLiability as IAssetLiabilityFixedLongTermInvestment;

            if (fixedLongTermInvestment != null)
            {
                pnlFixedProperty.Visible = false;
                pnlInvestmentProperty.Visible = false;
                pnlLiabilityLoan.Visible = false;
                pnlLiabilitySurety.Visible = false;
                pnlLifeAssurance.Visible = false;
                pnlOther.Visible = false;
                pnlFixedLongTermInvestment.Visible = true;

                lblFixedLongTermInvestment.Text = AssetLiabilityTypes.FixedLongTermInvestment.ToString();
                lblCompanyFixedLongTermInvestment.Text = fixedLongTermInvestment.CompanyName;
                lblLiabilityFixedLongTermInvestment.Text = fixedLongTermInvestment.LiabilityValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        public void BindUpdatePanel(ILegalEntityAssetLiability leAssetLiabilityUpdate)
        {
            lblAddress.Visible = true;

            IAssetLiabilityFixedProperty fixedProperty = leAssetLiabilityUpdate.AssetLiability as IAssetLiabilityFixedProperty;
            if (fixedProperty != null)
            {
                btnAssociateAddress.Visible = false;
                lblAssetType.Text = AssetLiabilityTypes.FixedProperty.ToString();
                if (fixedProperty.Address != null)
                    lblAddress.Text = fixedProperty.Address.GetFormattedDescription(AddressDelimiters.Comma);
                ddlAddress.Visible = false;
                dtDateAcquired.Date = Convert.ToDateTime(fixedProperty.DateAcquired);
                txtAssetValue.Amount = fixedProperty.AssetValue;
                txtLiabilityValue.Amount = fixedProperty.LiabilityValue;
            }

            IAssetLiabilityInvestmentPrivate investmentPrivate = leAssetLiabilityUpdate.AssetLiability as IAssetLiabilityInvestmentPrivate;

            if (investmentPrivate != null)
            {
                lblAssetType.Text = AssetLiabilityTypes.UnlistedInvestments.ToString();
                txtCompanyName.Text = investmentPrivate.CompanyName;
                txtAssetValue.Amount = investmentPrivate.AssetValue;
            }

            IAssetLiabilityInvestmentPublic investmentPublic = leAssetLiabilityUpdate.AssetLiability as IAssetLiabilityInvestmentPublic;

            if (investmentPublic != null)
            {
                lblAssetType.Text = AssetLiabilityTypes.ListedInvestments.ToString();
                txtCompanyName.Text = investmentPublic.CompanyName;
                txtAssetValue.Amount = investmentPublic.AssetValue;
            }

            IAssetLiabilityLiabilityLoan liabilityLoan = leAssetLiabilityUpdate.AssetLiability as IAssetLiabilityLiabilityLoan;

            if (liabilityLoan != null)
            {
                lblAssetType.Text = AssetLiabilityTypes.LiabilityLoan.ToString();
                dtDateRepayable.Date = Convert.ToDateTime(liabilityLoan.DateRepayable);

                txtFinancialInstitution.Text = liabilityLoan.FinancialInstitution;
                txtInstalmentValue.Amount = liabilityLoan.InstalmentValue;
                txtLiabilityValue.Amount = liabilityLoan.LiabilityValue;
                ddlSubType.SelectedValue = liabilityLoan.LoanType.Key.ToString();
            }

            IAssetLiabilityLiabilitySurety loanSurety = leAssetLiabilityUpdate.AssetLiability as IAssetLiabilityLiabilitySurety;

            if (loanSurety != null)
            {
                lblAssetType.Text = AssetLiabilityTypes.LiabilitySurety.ToString();
                txtAssetValue.Amount = Convert.ToDouble(loanSurety.AssetValue);
                txtDescription.Text = loanSurety.Description;
                txtLiabilityValue.Amount = Convert.ToDouble(loanSurety.LiabilityValue);
            }

            IAssetLiabilityLifeAssurance lifeAssurance = leAssetLiabilityUpdate.AssetLiability as IAssetLiabilityLifeAssurance;

            if (lifeAssurance != null)
            {
                lblAssetType.Text = AssetLiabilityTypes.LifeAssurance.ToString();
                txtCompanyName.Text = lifeAssurance.CompanyName;
                txtSurrenderValue.Amount = lifeAssurance.SurrenderValue;
            }

            IAssetLiabilityOther other = leAssetLiabilityUpdate.AssetLiability as IAssetLiabilityOther;

            if (other != null)
            {
                lblAssetType.Text = AssetLiabilityTypes.OtherAsset.ToString();
                txtDescription.Text = other.Description;
                txtAssetValue.Amount = other.AssetValue;
                txtLiabilityValue.Amount = other.LiabilityValue;
            }

            IAssetLiabilityFixedLongTermInvestment assetFixedLongTerm = leAssetLiabilityUpdate.AssetLiability as IAssetLiabilityFixedLongTermInvestment;

            if (assetFixedLongTerm != null)
            {
                lblAssetType.Text = AssetLiabilityTypes.FixedLongTermInvestment.ToString();
                txtCompanyName.Text = assetFixedLongTerm.CompanyName;
                txtLiabilityValue.Amount = assetFixedLongTerm.LiabilityValue;
            }
        }

        /// <summary>
        /// Trims and checks string values for Zero Length
        /// </summary>
        /// <returns></returns>
        public bool CheckStringsForZeroLength(IAssetLiability assetLiability)
        {

            //IAssetLiabilityLifeAssurance alLifeAssurance = assetLiability as IAssetLiabilityLifeAssurance;

            //if (alLifeAssurance != null)
            //{
            //    txtCompanyName.Text = txtCompanyName.Text.Trim();
            //    if (txtCompanyName.Text.Length == 0)
            //        return false;
            //}

            //IAssetLiabilityInvestmentPrivate alInvestmentPrivate = assetLiability as IAssetLiabilityInvestmentPrivate;

            //if (alInvestmentPrivate != null)
            //{
            //    txtCompanyName.Text = txtCompanyName.Text.Trim();
            //    if (txtCompanyName.Text.Length == 0)
            //        return false;
            //}

            //IAssetLiabilityInvestmentPublic alInvestmentPublic = assetLiability as IAssetLiabilityInvestmentPublic;

            //if (alInvestmentPublic != null)
            //{
            //    txtCompanyName.Text = txtCompanyName.Text.Trim();
            //    if (txtCompanyName.Text.Length == 0)
            //        return false;
            //}

            //IAssetLiabilityLiabilityLoan alLiabilityLoan = assetLiability as IAssetLiabilityLiabilityLoan;

            //if (alLiabilityLoan != null)
            //{

            //    txtFinancialInstitution.Text = txtFinancialInstitution.Text.Trim();
            //    if (txtFinancialInstitution.Text.Length == 0)
            //        return false;
            //}

            //IAssetLiabilityLiabilitySurety alLiabilitySurety = assetLiability as IAssetLiabilityLiabilitySurety;

            //if (alLiabilitySurety != null)
            //{
            //    txtDescription.Text = txtDescription.Text.Trim();
            //    if (txtDescription.Text.Length == 0)
            //        return false;
            //}

            //IAssetLiabilityOther alOther = assetLiability as IAssetLiabilityOther;

            //if (alOther != null)
            //{
            //    txtDescription.Text = txtDescription.Text.Trim();
            //    if (txtDescription.Text.Length == 0)
            //        return false;
            //}

            //IAssetLiabilityFixedLongTermInvestment assetLongTerm = assetLiability as IAssetLiabilityFixedLongTermInvestment;

            //if (assetLongTerm != null)
            //{
            //    txtCompanyName.Text = txtCompanyName.Text.Trim();
            //    if (txtCompanyName.Text.Length == 0)
            //        return false;
            //}


            return true;
        }

        public IAssetLiability GetAssetLiablityForUpdate(IAssetLiability assetLiability)
        {
            IAssetLiabilityFixedProperty alFixedProperty = assetLiability as IAssetLiabilityFixedProperty;

            if (alFixedProperty != null)
            {
                alFixedProperty.AssetValue = Convert.ToDouble(txtAssetValue.Amount);
                alFixedProperty.DateAcquired = dtDateAcquired.Date;
                alFixedProperty.LiabilityValue = Convert.ToDouble(txtLiabilityValue.Text);
            }

            IAssetLiabilityLifeAssurance alLifeAssurance = assetLiability as IAssetLiabilityLifeAssurance;

            if (alLifeAssurance != null)
            {
                alLifeAssurance.CompanyName = txtCompanyName.Text.Trim();
                alLifeAssurance.SurrenderValue = Convert.ToDouble(txtSurrenderValue.Amount);
                assetLiability = alLifeAssurance;
            }

            IAssetLiabilityInvestmentPrivate alInvestmentPrivate = assetLiability as IAssetLiabilityInvestmentPrivate;

            if (alInvestmentPrivate != null)
            {
                alInvestmentPrivate.AssetValue = Convert.ToDouble(txtAssetValue.Amount);
                alInvestmentPrivate.CompanyName = txtCompanyName.Text.Trim();
            }

            IAssetLiabilityInvestmentPublic alInvestmentPublic = assetLiability as IAssetLiabilityInvestmentPublic;

            if (alInvestmentPublic != null)
            {
                alInvestmentPublic.AssetValue = Convert.ToDouble(txtAssetValue.Amount);
                alInvestmentPublic.CompanyName = txtCompanyName.Text.Trim();
            }

            IAssetLiabilityLiabilityLoan alLiabilityLoan = assetLiability as IAssetLiabilityLiabilityLoan;

            if (alLiabilityLoan != null)
            {
                alLiabilityLoan.DateRepayable = dtDateRepayable.Date;
                alLiabilityLoan.FinancialInstitution = txtFinancialInstitution.Text.Trim();
                alLiabilityLoan.InstalmentValue = Convert.ToDouble(txtInstalmentValue.Amount);
                alLiabilityLoan.LiabilityValue = Convert.ToDouble(txtLiabilityValue.Text);
                alLiabilityLoan.LoanType = _lookups.AssetLiabilitySubTypes.ObjectDictionary[ddlSubType.SelectedValue];
            }

            IAssetLiabilityLiabilitySurety alLiabilitySurety = assetLiability as IAssetLiabilityLiabilitySurety;

            if (alLiabilitySurety != null)
            {
                alLiabilitySurety.AssetValue = Convert.ToDouble(txtAssetValue.Amount);
                alLiabilitySurety.Description = txtDescription.Text.Trim();
                alLiabilitySurety.LiabilityValue = Convert.ToDouble(txtLiabilityValue.Text);
            }

            IAssetLiabilityOther alOther = assetLiability as IAssetLiabilityOther;

            if (alOther != null)
            {
                alOther.AssetValue = Convert.ToDouble(txtAssetValue.Amount);
                alOther.Description = txtDescription.Text.Trim();
                alOther.LiabilityValue = Convert.ToDouble(txtLiabilityValue.Text);
            }

            IAssetLiabilityFixedLongTermInvestment assetLongTerm = assetLiability as IAssetLiabilityFixedLongTermInvestment;

            if (assetLongTerm != null)
            {
                assetLongTerm.CompanyName = txtCompanyName.Text.Trim();
                assetLongTerm.LiabilityValue = Convert.ToDouble(txtLiabilityValue.Text);
            }

            return assetLiability;
        }

        public void SetUpdateButtonText(string descriptionText)
        {
            btnAddUpdate.Text = descriptionText;
        }

        public int GetSelectedAssociatedAsset
        {
            get
            { return ddlAssociate.SelectedIndex; }
        }

        public int GetSelectedIndexOnGrid
        {
            get { return grdAssetLiability.SelectedIndex; }
        }

        protected void ddlAddressSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAddress.SelectedValue != "-select-")
                btnAssociateAddress.Visible = false;
            else
                btnAssociateAddress.Visible = true;

        }

        protected void grdAssetLiability_SelectedIndexChanged(Object s, EventArgs e)
        {
            if (OngrdAssetLiabilitySelectedIndexChanged != null && grdAssetLiability.SelectedIndex >= 0)
                OngrdAssetLiabilitySelectedIndexChanged(s, new KeyChangedEventArgs(grdAssetLiability.SelectedIndex));
        }

        protected void ddlType_SelectedIndexChanged(Object s, EventArgs e)
        {
            if (OnddlTypeSelectedIndexChanged != null && ddlType.SelectedIndex >= 0)
                OnddlTypeSelectedIndexChanged(s, new KeyChangedEventArgs(ddlType.SelectedValue));
        }

        protected void ddlAssociate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OndddlAssociateSelectedIndexChanged != null && ddlAssociate.SelectedValue != "-select-")
                OndddlAssociateSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlAssociate.SelectedIndex));
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (OnAddButtonClicked != null)
                OnAddButtonClicked(sender, e);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
                OnCancelButtonClicked(sender, e);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (OnDeleteButtonClicked != null)
                OnDeleteButtonClicked(sender, e);
        }

        protected void btnAssociateAddress_Click(object sender, EventArgs e)
        {
            if (OnAddAddressButtonClicked != null)
                OnAddAddressButtonClicked(sender, e);
        }

        public event EventHandler OnCancelButtonClicked;
        public event EventHandler OnDeleteButtonClicked;
        public event KeyChangedEventHandler OngrdAssetLiabilitySelectedIndexChanged;
        public event KeyChangedEventHandler OnddlTypeSelectedIndexChanged;
        public event EventHandler OnAddButtonClicked;
        public event EventHandler OnAddAddressButtonClicked;
        public event KeyChangedEventHandler OndddlAssociateSelectedIndexChanged;
    }
}