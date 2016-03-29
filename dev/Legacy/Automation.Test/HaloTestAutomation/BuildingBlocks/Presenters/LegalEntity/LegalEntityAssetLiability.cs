using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;
using ObjectMaps.Pages;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WatiN.Core;

namespace BuildingBlocks.Presenters.LegalEntity
{
    public sealed class LegalEntityAssetLiability : LegalEntityAssetLiabilityControls
    {
        private readonly IWatiNService watinService;

        public LegalEntityAssetLiability()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        public void PopulateAssetLiabilityView(Automation.DataModels.LegalEntityAssetLiabilities LegalEntityAssetLiability)
        {
            if (base.ddlType.Exists && base.ddlType.Enabled)
                base.ddlType.Option(LegalEntityAssetLiability.AssetLiabilityTypeDescription).Select();
            if (base.ctl00_Main_ddlSubType.Exists && base.ctl00_Main_ddlSubType.Enabled)
                base.ctl00_Main_ddlSubType.Option(LegalEntityAssetLiability.AssetLiabilitySubTypeDescription).Select();
            if (base.ctl00_Main_dtDateRepayable.Exists && base.ctl00_Main_dtDateRepayable.Enabled)
                base.ctl00_Main_dtDateRepayable.Value = LegalEntityAssetLiability.Date.Value.ToString(Formats.DateFormat);
            if (base.dtDateAcquired.Exists && base.dtDateAcquired.Enabled)
                base.dtDateAcquired.Value = LegalEntityAssetLiability.Date.Value.ToString(Formats.DateFormat);
            if (base.ddlAddress.Exists && base.ddlAddress.Enabled)
                base.ddlAddress.Option(LegalEntityAssetLiability.Address.ToString()).Select();
            if (base.txtCompanyName.Exists && base.txtCompanyName.Enabled)
                base.txtCompanyName.Value = LegalEntityAssetLiability.CompanyName;
            if (base.txtAssetValue_txtRands.Exists && base.txtAssetValue_txtRands.Enabled)
                base.txtAssetValue_txtRands.Value = ((int)LegalEntityAssetLiability.AssetValue).ToString();
            if (base.txtLiabilityValue_txtRands.Exists && base.txtLiabilityValue_txtRands.Enabled)
                base.txtLiabilityValue_txtRands.Value = ((int)LegalEntityAssetLiability.LiabilityValue).ToString();
            if (base.ctl00_Main_txtDescription.Exists && base.ctl00_Main_txtDescription.Enabled)
                base.ctl00_Main_txtDescription.Value = LegalEntityAssetLiability.OtherDescription;
        }

        public void SelectAssetLiabilityRowByType(AssetLiabilityTypeEnum assetLiabilityType)
        {
            #region GridRowSelect

            Assert.AreNotEqual(0, base.ctl00_Main_grdAssetLiability.TableRows.Count, "The Update AssetLiability grid was empty");
            bool rowFound = false;
            foreach (TableRow row in base.ctl00_Main_grdAssetLiability.TableRows)
            {
                foreach (TableCell cell in row.TableCells)
                {
                    if (String.IsNullOrEmpty(cell.Text))
                        break;
                    switch (assetLiabilityType)
                    {
                        case AssetLiabilityTypeEnum.FixedProperty:
                            {
                                if (cell.Text.Equals(Common.Constants.AssetLiabilityType.FixedProperty.Replace(" ", "")))
                                {
                                    row.Click();
                                    rowFound = true;
                                }
                                break;
                            }
                        case AssetLiabilityTypeEnum.ListedInvestments:
                            {
                                if (cell.Text.Equals(Common.Constants.AssetLiabilityType.ListedInvestments.Replace(" ", "")))
                                {
                                    row.Click();
                                    rowFound = true;
                                }
                                break;
                            }
                        case AssetLiabilityTypeEnum.UnlistedInvestments:
                            {
                                if (cell.Text.Equals(Common.Constants.AssetLiabilityType.UnlistedInvestments.Replace(" ", "")))
                                {
                                    row.Click();
                                    rowFound = true;
                                }
                                break;
                            }
                        case AssetLiabilityTypeEnum.OtherAsset:
                            {
                                if (cell.Text.Equals(Common.Constants.AssetLiabilityType.OtherAsset.Replace(" ", "")))
                                {
                                    row.Click();
                                    rowFound = true;
                                }
                                break;
                            }
                        case AssetLiabilityTypeEnum.LifeAssurance:
                            {
                                if (cell.Text.Equals(Common.Constants.AssetLiabilityType.LifeAssurance.Replace(" ", "")))
                                {
                                    row.Click();
                                    rowFound = true;
                                }
                                break;
                            }
                        case AssetLiabilityTypeEnum.LiabilityLoan:
                            {
                                if (cell.Text.Equals(Common.Constants.AssetLiabilityType.LiabilityLoan.Replace(" ", "")))
                                {
                                    row.Click();
                                    rowFound = true;
                                }
                                break;
                            }
                        case AssetLiabilityTypeEnum.LiabilitySurety:
                            {
                                if (cell.Text.Equals(Common.Constants.AssetLiabilityType.LiabilitySurety.Replace(" ", "")))
                                {
                                    row.Click();
                                    rowFound = true;
                                }
                                break;
                            }
                        case AssetLiabilityTypeEnum.FixedLongTermInvestment:
                            {
                                if (cell.Text.Equals(Common.Constants.AssetLiabilityType.FixedLongTermInvestment.Replace(" ", "")))
                                {
                                    row.Click();
                                    rowFound = true;
                                }
                                break;
                            }
                    }
                    if (rowFound)
                        break;
                }
            }

            #endregion GridRowSelect
        }

        public void SelectAssetLiabilityType(string assetLiabilityType)
        {
            base.ddlType.Option(assetLiabilityType).Select();
        }

        public void AssertAssetLiabilityTypes(params string[] assetLiabilityTypes)
        {
            Assertions.WatiNAssertions.AssertSelectListContents(base.ddlType, new List<string>(assetLiabilityTypes));
        }

        public void AssertControlsMatchAssetLiability
            (
                AssetLiabilityTypeEnum assetLiabilityType,
                Automation.DataModels.LegalEntityAssetLiabilities expectedLegalEntityAssetLiability = null
            )
        {
            if (expectedLegalEntityAssetLiability != null)
            {
                Assert.That((base.AssetTypeLabel.Exists));
                Assert.That((expectedLegalEntityAssetLiability.AssetLiabilityTypeDescription.Replace(" ", "")
                              == base.AssetTypeLabel.Text), "AssetLiabilityType comparison on control failed, screen didn't load assetliability correctly");
            }
            else
            {
                Assert.That((base.ddlType.Exists && base.ddlType.Enabled));
            }

            switch (assetLiabilityType)
            {
                case AssetLiabilityTypeEnum.FixedProperty:
                    {
                        Assert.That((base.dtDateAcquired.Exists && base.dtDateAcquired.Enabled));
                        Assert.That((base.txtAssetValue_txtRands.Exists && base.txtAssetValue_txtRands.Enabled));
                        Assert.That((base.txtLiabilityValue_txtRands.Exists && base.txtLiabilityValue_txtRands.Enabled));

                        if (expectedLegalEntityAssetLiability != null)
                        {
                            Assert.That((base.AddressLabel.Exists));
                            Assert.That((expectedLegalEntityAssetLiability.Date.Value.Date.ToString(Formats.DateFormat)
                                == base.dtDateAcquired.Value), "Date comparison on control failed, screen didn't load assetliability correctly");

                            Assert.That((expectedLegalEntityAssetLiability.Address.FormattedAddress
                                == base.AddressLabel.Text), "Address comparison on control failed, screen didn't load assetliability correctly");

                            Assert.That((expectedLegalEntityAssetLiability.AssetValue.ToString()
                                == base.txtAssetValue_txtRands.Value), "Asset Value comparison on control failed, screen didn't load assetliability correctly");
                            Assert.That((expectedLegalEntityAssetLiability.LiabilityValue.ToString()
                             == base.txtLiabilityValue_txtRands.Value), "Liability Value comparison on control failed, screen didn't load assetliability correctly");
                        }
                        else
                        {
                            Assert.That((base.ddlAddress.Exists && base.ddlAddress.Enabled));
                        }
                        break;
                    }
                case AssetLiabilityTypeEnum.ListedInvestments:
                    {
                        Assert.That((base.txtCompanyName.Exists && base.txtCompanyName.Enabled));
                        Assert.That((base.txtAssetValue_txtRands.Exists && base.txtAssetValue_txtRands.Enabled));

                        if (expectedLegalEntityAssetLiability != null)
                        {
                            Assert.That((expectedLegalEntityAssetLiability.CompanyName
                                == base.txtCompanyName.Value), "Company Name comparison on control failed, screen didn't load assetliability correctly");
                            Assert.That((expectedLegalEntityAssetLiability.AssetValue.ToString()
                                == base.txtAssetValue_txtRands.Value), "Asset Value comparison on control failed, screen didn't load assetliability correctly");
                        }

                        break;
                    }
                case AssetLiabilityTypeEnum.UnlistedInvestments:
                    {
                        Assert.That((base.txtCompanyName.Exists && base.txtCompanyName.Enabled));
                        Assert.That((base.txtAssetValue_txtRands.Exists && base.txtAssetValue_txtRands.Enabled));

                        if (expectedLegalEntityAssetLiability != null)
                        {
                            Assert.That((expectedLegalEntityAssetLiability.CompanyName
                                == base.txtCompanyName.Value), "Company Name comparison on control failed, screen didn't load assetliability correctly");
                            Assert.That((expectedLegalEntityAssetLiability.AssetValue.ToString()
                                == base.txtAssetValue_txtRands.Value), "Asset Value comparison on control failed, screen didn't load assetliability correctly");
                        }

                        break;
                    }
                case AssetLiabilityTypeEnum.OtherAsset:
                    {
                        Assert.That((base.txtAssetValue_txtRands.Exists && base.txtAssetValue_txtRands.Enabled));
                        Assert.That((base.txtLiabilityValue_txtRands.Exists && base.txtLiabilityValue_txtRands.Enabled));
                        Assert.That((base.ctl00_Main_txtDescription.Exists && base.ctl00_Main_txtDescription.Enabled));

                        if (expectedLegalEntityAssetLiability != null)
                        {
                            Assert.That((expectedLegalEntityAssetLiability.OtherDescription
                                == base.ctl00_Main_txtDescription.Value), "Description comparison on control failed, screen didn't load assetliability correctly");
                            Assert.That((expectedLegalEntityAssetLiability.AssetValue.ToString()
                                == base.txtAssetValue_txtRands.Value), "Asset Value comparison on control failed, screen didn't load assetliability correctly");
                            Assert.That((expectedLegalEntityAssetLiability.LiabilityValue.ToString()
                              == base.txtLiabilityValue_txtRands.Value), "Liability Value comparison on control failed, screen didn't load assetliability correctly");
                        }

                        break;
                    }
                case AssetLiabilityTypeEnum.LifeAssurance:
                    {
                        Assert.That((base.txtCompanyName.Exists && base.txtCompanyName.Enabled));
                        Assert.That((base.ctl00_Main_txtSurrenderValue_txtRands.Exists && base.ctl00_Main_txtSurrenderValue_txtRands.Enabled));

                        if (expectedLegalEntityAssetLiability != null)
                        {
                            Assert.That((expectedLegalEntityAssetLiability.CompanyName
                                 == base.txtCompanyName.Value), "Company Name comparison on control failed, screen didn't load assetliability correctly");
                            Assert.That((expectedLegalEntityAssetLiability.AssetValue.ToString()
                                == base.ctl00_Main_txtSurrenderValue_txtRands.Value), "Asset Value comparison on control failed, screen didn't load assetliability correctly");
                        }

                        break;
                    }
                case AssetLiabilityTypeEnum.LiabilityLoan:
                    {
                        Assert.That((base.ctl00_Main_ddlSubType.Exists && base.ctl00_Main_ddlSubType.Enabled));
                        Assert.That((base.ctl00_Main_txtFinancialInstitution.Exists && base.ctl00_Main_txtFinancialInstitution.Enabled));
                        Assert.That((base.ctl00_Main_dtDateRepayable.Exists && base.ctl00_Main_dtDateRepayable.Enabled));
                        Assert.That((base.ctl00_Main_txtInstalmentValue_txtRands.Exists && base.ctl00_Main_txtInstalmentValue_txtRands.Enabled));
                        Assert.That((base.txtLiabilityValue_txtRands.Exists && base.txtLiabilityValue_txtRands.Enabled));

                        if (expectedLegalEntityAssetLiability != null)
                        {
                            Assert.That((expectedLegalEntityAssetLiability.Date.Value.Date.ToString(Formats.DateFormat)
                                 == base.ctl00_Main_dtDateRepayable.Value), "DateRepayable comparison on control failed, screen didn't load assetliability correctly");
                            Assertions.WatiNAssertions.AssertSelectListContents(base.ctl00_Main_ddlSubType,
                                new List<string> { expectedLegalEntityAssetLiability.AssetLiabilitySubTypeDescription }, performCountAssertion: false);
                            Assert.That((expectedLegalEntityAssetLiability.CompanyName
                                == base.ctl00_Main_txtFinancialInstitution.Value), "FinancialInstitution Value comparison on control failed, screen didn't load assetliability correctly");
                            Assert.That((expectedLegalEntityAssetLiability.Cost.ToString()
                                == base.ctl00_Main_txtInstalmentValue_txtRands.Value), "Installent Value comparison on control failed, screen didn't load assetliability correctly");
                            Assert.That((expectedLegalEntityAssetLiability.LiabilityValue.ToString()
                                == base.txtLiabilityValue_txtRands.Value), "Liability Value comparison on control failed, screen didn't load assetliability correctly");
                        }

                        break;
                    }
                case AssetLiabilityTypeEnum.LiabilitySurety:
                    {
                        Assert.That((base.txtAssetValue_txtRands.Exists && base.txtAssetValue_txtRands.Enabled));
                        Assert.That((base.txtLiabilityValue_txtRands.Exists && base.txtLiabilityValue_txtRands.Enabled));
                        Assert.That((base.ctl00_Main_txtDescription.Exists && base.ctl00_Main_txtDescription.Enabled));

                        if (expectedLegalEntityAssetLiability != null)
                        {
                            Assert.That((expectedLegalEntityAssetLiability.OtherDescription
                                == base.ctl00_Main_txtDescription.Value), "Description comparison on control failed, screen didn't load assetliability correctly");
                            Assert.That((expectedLegalEntityAssetLiability.AssetValue.ToString()
                                == base.txtAssetValue_txtRands.Value), "Asset Value comparison on control failed, screen didn't load assetliability correctly");
                            Assert.That((expectedLegalEntityAssetLiability.LiabilityValue.ToString()
                              == base.txtLiabilityValue_txtRands.Value), "Liability Value comparison on control failed, screen didn't load assetliability correctly");
                        }

                        break;
                    }
                case AssetLiabilityTypeEnum.FixedLongTermInvestment:
                    {
                        Assert.That((base.txtCompanyName.Exists && base.txtCompanyName.Enabled));
                        Assert.That((base.txtLiabilityValue_txtRands.Exists && base.txtLiabilityValue_txtRands.Enabled));

                        if (expectedLegalEntityAssetLiability != null)
                        {
                            Assert.That((expectedLegalEntityAssetLiability.CompanyName
                                == base.txtCompanyName.Value), "Company Name comparison on control failed, screen didn't load assetliability correctly");
                            Assert.That((expectedLegalEntityAssetLiability.LiabilityValue.ToString()
                                == base.txtLiabilityValue_txtRands.Value), "Asset Value comparison on control failed, screen didn't load assetliability correctly");
                        }

                        break;
                    }
            }
        }

        public void ClickUpdate()
        {
            base.btnAddUpdate.Click();
        }

        public void ClickCancel()
        {
            base.btnCancel.Click();
        }

        public void ClickDelete()
        {
            watinService.HandleConfirmationPopup(base.ctl00_Main_btnDelete);
        }

        public void AssertButtonsExist(bool isAddUpdateMode = false, bool isDeleteMode = false)
        {
            if (isAddUpdateMode)
                Assert.That((base.btnAddUpdate.Exists));
            if (isDeleteMode)
                Assert.That((base.ctl00_Main_btnDelete.Exists));
            Assert.That((base.btnCancel.Exists));
        }

        public void AddAssetsAndLiabilities(Automation.DataModels.LegalEntityAssetLiabilities assetsAndLiabilties)
        {
            AddAssetsAndLiabilities(assetsAndLiabilties, ButtonTypeEnum.Add);
        }

        public void UpdateAssetsAndLiabilities(Automation.DataModels.LegalEntityAssetLiabilities assetsAndLiabilties)
        {
            AddAssetsAndLiabilities(assetsAndLiabilties, ButtonTypeEnum.Update);
        }

        public void DeleteAssetsAndLiabilities(Automation.DataModels.LegalEntityAssetLiabilities assetsAndLiabilties)
        {
            AddAssetsAndLiabilities(assetsAndLiabilties, ButtonTypeEnum.Delete);
        }

        public void AssociateAssetsLiability(AssetLiabilityTypeEnum assetLiabilityType)
        {
            string assetLiabilityTypeName = String.Empty;
            //determine asset liabilitytype
            switch (assetLiabilityType)
            {
                case AssetLiabilityTypeEnum.FixedLongTermInvestment:
                    {
                        assetLiabilityTypeName = Common.Constants.AssetLiabilityType.FixedLongTermInvestment;
                        break;
                    }
                case AssetLiabilityTypeEnum.FixedProperty:
                    {
                        assetLiabilityTypeName = Common.Constants.AssetLiabilityType.FixedProperty;
                        break;
                    }

                case AssetLiabilityTypeEnum.LiabilityLoan:
                    {
                        assetLiabilityTypeName = Common.Constants.AssetLiabilityType.LiabilityLoan;
                        break;
                    }
                case AssetLiabilityTypeEnum.LiabilitySurety:
                    {
                        assetLiabilityTypeName = Common.Constants.AssetLiabilityType.LiabilitySurety;
                        break;
                    }
                case AssetLiabilityTypeEnum.LifeAssurance:
                    {
                        assetLiabilityTypeName = Common.Constants.AssetLiabilityType.LifeAssurance;
                        break;
                    }
                case AssetLiabilityTypeEnum.ListedInvestments:
                    {
                        assetLiabilityTypeName = Common.Constants.AssetLiabilityType.ListedInvestments;
                        break;
                    }
                case AssetLiabilityTypeEnum.OtherAsset:
                    {
                        assetLiabilityTypeName = Common.Constants.AssetLiabilityType.OtherAsset;
                        break;
                    }
                case AssetLiabilityTypeEnum.UnlistedInvestments:
                    {
                        assetLiabilityTypeName = Common.Constants.AssetLiabilityType.UnlistedInvestments;
                        break;
                    }
            }
            assetLiabilityTypeName = assetLiabilityTypeName.Replace(" ", "");
            base.ctl00_Main_ddlAssociate.Option(new System.Text.RegularExpressions.Regex(assetLiabilityTypeName)).Select();
            ClickButton(ButtonTypeEnum.Add);
        }

        public void Cancel()
        {
            ClickButton(ButtonTypeEnum.Cancel);
        }

        #region Helpers

        private void AddAssetsAndLiabilities(Automation.DataModels.LegalEntityAssetLiabilities legalEntityAssetLiability, ButtonTypeEnum button)
        {
            try
            {
                switch (button)
                {
                    case ButtonTypeEnum.Update:
                        {
                            GridRowSelect(legalEntityAssetLiability);
                            break;
                        }
                    case ButtonTypeEnum.Delete:
                        {
                            GridRowSelect(legalEntityAssetLiability);
                            ClickButton(ButtonTypeEnum.Delete);
                            return;
                        }
                }

                switch (legalEntityAssetLiability.AssetLiabilityTypeKey)
                {
                    #region FixedLongTermInvestmentAddUpdate

                    case AssetLiabilityTypeEnum.FixedLongTermInvestment:
                        {
                            switch (button)
                            {
                                case ButtonTypeEnum.Add:
                                    {
                                        base.ddlType.Option(Common.Constants.AssetLiabilityType.FixedLongTermInvestment).Select();
                                        break;
                                    }
                            }
                            base.txtCompanyName.Value = (legalEntityAssetLiability.CompanyName);
                            base.txtLiabilityValue_txtRands.TypeText(legalEntityAssetLiability.LiabilityValue.ToString());
                            ClickButton(button);
                            break;
                        }

                    #endregion FixedLongTermInvestmentAddUpdate

                    #region FixedPropertyAddUpdate

                    case AssetLiabilityTypeEnum.FixedProperty:
                        {
                            switch (button)
                            {
                                case ButtonTypeEnum.Add:
                                    {
                                        base.ddlType.Option(Common.Constants.AssetLiabilityType.FixedProperty).Select();
                                        base.dtDateAcquired.Value = legalEntityAssetLiability.DateAcquired.ToString(Formats.DateFormat);
                                        base.CaptureAddress.Click();
                                        base.Document.Page<LegalEntityAddressDetails>().AddResidentialAddress(legalEntityAssetLiability.Address);
                                        base.ddlAddress.Options[1].Select();
                                        Thread.Sleep(2000);
                                        break;
                                    }
                                case ButtonTypeEnum.Update:
                                    {
                                        base.dtDateAcquired.Value = legalEntityAssetLiability.DateAcquired.ToString(Formats.DateFormat);
                                        break;
                                    }
                            }
                            base.txtLiabilityValue_txtRands.TypeText(legalEntityAssetLiability.LiabilityValue.ToString());
                            base.txtAssetValue_txtRands.TypeText(legalEntityAssetLiability.AssetValue.ToString());
                            ClickButton(button);
                            break;
                        }

                    #endregion FixedPropertyAddUpdate

                    #region LiabilityLoanAddUpdate

                    case AssetLiabilityTypeEnum.LiabilityLoan:
                        {
                            switch (button)
                            {
                                case ButtonTypeEnum.Add:
                                    {
                                        base.ddlType.Option(Common.Constants.AssetLiabilityType.LiabilityLoan).Select();
                                        break;
                                    }
                            }

                            #region LoanType

                            switch (legalEntityAssetLiability.AssetLiabilitySubTypeKey)
                            {
                                case AssetLiabilitySubTypeEnum.PersonalLoan:
                                    {
                                        base.ctl00_Main_ddlSubType.Option(Common.Constants.LoanType.PersonalLoan).Select();
                                        break;
                                    }
                                case AssetLiabilitySubTypeEnum.StudentLoan:
                                    {
                                        base.ctl00_Main_ddlSubType.Option(Common.Constants.LoanType.StudentLoan).Select();
                                        break;
                                    }
                            }
                            Thread.Sleep(2000);

                            #endregion LoanType

                            base.ctl00_Main_txtFinancialInstitution.Value = legalEntityAssetLiability.CompanyName;
                            base.ctl00_Main_dtDateRepayable.Value = legalEntityAssetLiability.DateRepayable.ToString(Formats.DateFormat);
                            base.ctl00_Main_txtInstalmentValue_txtRands.TypeText(legalEntityAssetLiability.Cost.ToString());
                            base.txtLiabilityValue_txtRands.TypeText(legalEntityAssetLiability.LiabilityValue.ToString());
                            ClickButton(button);
                            break;
                        }

                    #endregion LiabilityLoanAddUpdate

                    #region LiabilitySurety

                    case AssetLiabilityTypeEnum.LiabilitySurety:
                        {
                            switch (button)
                            {
                                case ButtonTypeEnum.Add:
                                    {
                                        base.ddlType.Option(Common.Constants.AssetLiabilityType.LiabilitySurety).Select();
                                        break;
                                    }
                            }
                            base.txtLiabilityValue_txtRands.TypeText(legalEntityAssetLiability.LiabilityValue.ToString());
                            base.txtAssetValue_txtRands.TypeText(legalEntityAssetLiability.AssetValue.ToString());
                            base.ctl00_Main_txtDescription.Value = legalEntityAssetLiability.OtherDescription;
                            ClickButton(button);
                            break;
                        }

                    #endregion LiabilitySurety

                    #region LifeAssurance

                    case AssetLiabilityTypeEnum.LifeAssurance:
                        {
                            switch (button)
                            {
                                case ButtonTypeEnum.Add:
                                    {
                                        base.ddlType.Option(Common.Constants.AssetLiabilityType.LifeAssurance).Select();
                                        break;
                                    }
                            }
                            base.txtCompanyName.Value = legalEntityAssetLiability.CompanyName;
                            base.ctl00_Main_txtSurrenderValue_txtRands.TypeText(legalEntityAssetLiability.AssetValue.ToString());
                            ClickButton(button);
                            break;
                        }

                    #endregion LifeAssurance

                    #region ListedInvestments

                    case AssetLiabilityTypeEnum.ListedInvestments:
                        {
                            switch (button)
                            {
                                case ButtonTypeEnum.Add:
                                    {
                                        base.ddlType.Option(Common.Constants.AssetLiabilityType.ListedInvestments).Select();
                                        break;
                                    }
                            }
                            base.txtCompanyName.Value = legalEntityAssetLiability.CompanyName;
                            base.txtAssetValue_txtRands.TypeText(legalEntityAssetLiability.AssetValue.ToString());
                            ClickButton(button);
                            break;
                        }

                    #endregion ListedInvestments

                    #region OtherAsset

                    case AssetLiabilityTypeEnum.OtherAsset:
                        {
                            switch (button)
                            {
                                case ButtonTypeEnum.Add:
                                    {
                                        base.ddlType.Option(Common.Constants.AssetLiabilityType.OtherAsset).Select();
                                        break;
                                    }
                            }
                            base.txtLiabilityValue_txtRands.TypeText(legalEntityAssetLiability.LiabilityValue.ToString());
                            base.txtAssetValue_txtRands.TypeText(legalEntityAssetLiability.AssetValue.ToString());
                            base.ctl00_Main_txtDescription.Value = legalEntityAssetLiability.OtherDescription;
                            ClickButton(button);
                            break;
                        }

                    #endregion OtherAsset

                    #region UnlistedInvestments

                    case AssetLiabilityTypeEnum.UnlistedInvestments:
                        {
                            switch (button)
                            {
                                case ButtonTypeEnum.Add:
                                    {
                                        base.ddlType.Option(Common.Constants.AssetLiabilityType.UnlistedInvestments).Select();
                                        break;
                                    }
                            }
                            base.txtCompanyName.Value = legalEntityAssetLiability.CompanyName;
                            base.txtAssetValue_txtRands.TypeText(legalEntityAssetLiability.AssetValue.ToString());
                            ClickButton(button);
                            break;
                        }

                    #endregion UnlistedInvestments
                }
            }
            catch
            {
                throw;
            }
        }

        public void AssertInactiveAssetLiabilityNotDisplay(Automation.DataModels.LegalEntityAssetLiabilities legalentityAssetLiability)
        {
            var allCells = from row in base.ctl00_Main_grdAssetLiability.TableRows
                           select row.TableCells.ToArray();

            //all the rowscells that contain the assetliablity type
            allCells = from cell in allCells
                       where Array.Exists<TableCell>(cell, (c) =>
                           {
                               if (c.Text.Equals(legalentityAssetLiability.AssetLiabilityTypeDescription.RemoveWhiteSpace()))
                                   return true;
                               return false;
                           })
                       select cell;

            Assert.AreEqual(0, allCells.Count(), "Inactive AssetLiablity {0} was displayed on screen", legalentityAssetLiability.AssetLiabilityTypeDescription);
        }

        /// <summary>
        /// This method will assert that the selected legal entity can only be associated to the fixedproperty of the related legal entity.
        /// </summary>
        public void AssertOnlyFixedPropertyAvailable()
        {
            if (base.ctl00_Main_ddlAssociate.AllContents.Count == 1 &&
                base.ctl00_Main_ddlAssociate.Options[0].Text.Contains(Common.Constants.AssetLiabilityType.FixedProperty.Replace(" ", "")))
                return;
            else
                throw new NUnit.Framework.AssertionException("The AssetLiability dropdown contains more than just FixedProperty for non COP marriage.");
        }

        private void GridRowSelect(Automation.DataModels.LegalEntityAssetLiabilities assetsAndLiabiltiesParams)
        {
            try
            {
                #region GridRowSelect

                Assert.AreNotEqual(0, base.ctl00_Main_grdAssetLiability.TableRows.Count, "The Update AssetLiability grid was empty");
                bool rowFound = false;
                foreach (TableRow row in base.ctl00_Main_grdAssetLiability.TableRows)
                {
                    foreach (TableCell cell in row.TableCells)
                    {
                        if (String.IsNullOrEmpty(cell.Text))
                            break;
                        switch (assetsAndLiabiltiesParams.AssetLiabilityTypeKey)
                        {
                            case AssetLiabilityTypeEnum.FixedProperty:
                                {
                                    if (cell.Text.Equals(Common.Constants.AssetLiabilityType.FixedProperty.RemoveWhiteSpace()))
                                    {
                                        row.Click();
                                        rowFound = true;
                                    }
                                    break;
                                }
                            case AssetLiabilityTypeEnum.ListedInvestments:
                                {
                                    if (cell.Text.Equals(Common.Constants.AssetLiabilityType.ListedInvestments.RemoveWhiteSpace()))
                                    {
                                        row.Click();
                                        rowFound = true;
                                    }
                                    break;
                                }
                            case AssetLiabilityTypeEnum.UnlistedInvestments:
                                {
                                    if (cell.Text.Equals(Common.Constants.AssetLiabilityType.UnlistedInvestments.RemoveWhiteSpace()))
                                    {
                                        row.Click();
                                        rowFound = true;
                                    }
                                    break;
                                }
                            case AssetLiabilityTypeEnum.OtherAsset:
                                {
                                    if (cell.Text.Equals(Common.Constants.AssetLiabilityType.OtherAsset.RemoveWhiteSpace()))
                                    {
                                        row.Click();
                                        rowFound = true;
                                    }
                                    break;
                                }
                            case AssetLiabilityTypeEnum.LifeAssurance:
                                {
                                    if (cell.Text.Equals(Common.Constants.AssetLiabilityType.LifeAssurance.RemoveWhiteSpace()))
                                    {
                                        row.Click();
                                        rowFound = true;
                                    }
                                    break;
                                }
                            case AssetLiabilityTypeEnum.LiabilityLoan:
                                {
                                    if (cell.Text.Equals(Common.Constants.AssetLiabilityType.LiabilityLoan.RemoveWhiteSpace()))
                                    {
                                        row.Click();
                                        rowFound = true;
                                    }
                                    break;
                                }
                            case AssetLiabilityTypeEnum.LiabilitySurety:
                                {
                                    if (cell.Text.Equals(Common.Constants.AssetLiabilityType.LiabilitySurety.RemoveWhiteSpace()))
                                    {
                                        row.Click();
                                        rowFound = true;
                                    }
                                    break;
                                }
                            case AssetLiabilityTypeEnum.FixedLongTermInvestment:
                                {
                                    if (cell.Text.Equals(Common.Constants.AssetLiabilityType.FixedLongTermInvestment.RemoveWhiteSpace()))
                                    {
                                        row.Click();
                                        rowFound = true;
                                    }
                                    break;
                                }
                        }
                        if (rowFound)
                            break;
                    }
                }

                #endregion GridRowSelect
            }
            catch
            {
                throw;
            }
        }

        private void ClickButton(ButtonTypeEnum button)
        {
            #region ClickButton

            try
            {
                switch (button)
                {
                    case ButtonTypeEnum.Add:
                        base.btnAddUpdate.Click();
                        break;

                    case ButtonTypeEnum.Update:
                        base.btnAddUpdate.Click();
                        break;

                    case ButtonTypeEnum.Delete:
                        {
                            watinService.HandleConfirmationPopup(base.ctl00_Main_btnDelete);
                            break;
                        }
                    case ButtonTypeEnum.Cancel:
                        base.btnCancel.Click();
                        break;
                }
            }
            catch
            {
                throw;
            }

            #endregion ClickButton
        }

        #endregion Helpers
    }
}