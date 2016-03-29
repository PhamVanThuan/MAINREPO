using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Rules.Property;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.Test.Property
{
    [TestFixture]
    public class Property : RuleBase
    {
        IProperty property;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            property = _mockery.StrictMock<IProperty>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [NUnit.Framework.Test]
        public void PropertyAdCheckDataProviderTestFail()
        {
            PropertyAdCheckDataProvider rule = new PropertyAdCheckDataProvider();

            IDataProvider dataprovider = _mockery.StrictMock<IDataProvider>();
            SetupResult.For(property.DataProvider).Return(dataprovider);

            SetupResult.For(dataprovider.Key).Return((int)DataProviders.AdCheck);

            ExecuteRule(rule, 1, property);
        }

        [NUnit.Framework.Test]
        public void PropertyAdCheckDataProviderTestPass()
        {
            PropertyAdCheckDataProvider rule = new PropertyAdCheckDataProvider();

            IDataProvider dataprovider = _mockery.StrictMock<IDataProvider>();
            SetupResult.For(property.DataProvider).Return(dataprovider);

            SetupResult.For(dataprovider.Key).Return((int)DataProviders.SAHL);

            ExecuteRule(rule, 0, property);
        }

        [NUnit.Framework.Test]
        public void PropertyTypeMandatoryTestFail()
        {
            PropertyTypeMandatory rule = new PropertyTypeMandatory();

            SetupResult.For(property.PropertyType).Return(null);

            ExecuteRule(rule, 1, property);
        }

        [NUnit.Framework.Test]
        public void PropertyTypeMandatoryTestPass()
        {
            PropertyTypeMandatory rule = new PropertyTypeMandatory();

            SetupResult.For(property.PropertyType).Return(_mockery.StrictMock<IPropertyType>());

            ExecuteRule(rule, 0, property);
        }

        [NUnit.Framework.Test]
        public void PropertyTitleTypeMandatoryTestFail()
        {
            PropertyTitleTypeMandatory rule = new PropertyTitleTypeMandatory();

            SetupResult.For(property.TitleType).Return(null);

            ExecuteRule(rule, 1, property);
        }

        [NUnit.Framework.Test]
        public void PropertyTitleTypeMandatoryTestPass()
        {
            PropertyTitleTypeMandatory rule = new PropertyTitleTypeMandatory();

            SetupResult.For(property.TitleType).Return(_mockery.StrictMock<ITitleType>());

            ExecuteRule(rule, 0, property);
        }

        [NUnit.Framework.Test]
        public void PropertyOccupancyTypeMandatoryTestFail()
        {
            PropertyOccupancyTypeMandatory rule = new PropertyOccupancyTypeMandatory();

            SetupResult.For(property.OccupancyType).Return(null);

            ExecuteRule(rule, 1, property);
        }

        [NUnit.Framework.Test]
        public void PropertyOccupancyTypeMandatoryTestPass()
        {
            PropertyOccupancyTypeMandatory rule = new PropertyOccupancyTypeMandatory();

            SetupResult.For(property.OccupancyType).Return(_mockery.StrictMock<IOccupancyType>());

            ExecuteRule(rule, 0, property);
        }

        [NUnit.Framework.Test]
        public void PropertyAreaClassificationMandatoryTestFail()
        {
            PropertyAreaClassificationMandatory rule = new PropertyAreaClassificationMandatory();

            SetupResult.For(property.AreaClassification).Return(null);

            ExecuteRule(rule, 1, property);
        }

        [NUnit.Framework.Test]
        public void PropertyAreaClassificationMandatoryTestPass()
        {
            PropertyAreaClassificationMandatory rule = new PropertyAreaClassificationMandatory();

            SetupResult.For(property.AreaClassification).Return(_mockery.StrictMock<IAreaClassification>());

            ExecuteRule(rule, 0, property);
        }

        [NUnit.Framework.Test]
        public void PropertyDeedsPropertyTypeMandatoryTestFail()
        {
            PropertyDeedsPropertyTypeMandatory rule = new PropertyDeedsPropertyTypeMandatory();

            SetupResult.For(property.DeedsPropertyType).Return(null);

            ExecuteRule(rule, 1, property);
        }

        [NUnit.Framework.Test]
        public void PropertyDeedsPropertyTypeMandatoryTestPass()
        {
            PropertyDeedsPropertyTypeMandatory rule = new PropertyDeedsPropertyTypeMandatory();

            SetupResult.For(property.DeedsPropertyType).Return(_mockery.StrictMock<IDeedsPropertyType>());

            ExecuteRule(rule, 0, property);
        }

        [NUnit.Framework.Test]
        public void PropertyDescription1MandatoryTestFail()
        {
            PropertyDescription1Mandatory rule = new PropertyDescription1Mandatory();

            SetupResult.For(property.PropertyDescription1).Return(null);

            ExecuteRule(rule, 1, property);
        }

        [NUnit.Framework.Test]
        public void PropertyDescription1MandatoryTestPass()
        {
            PropertyDescription1Mandatory rule = new PropertyDescription1Mandatory();

            SetupResult.For(property.PropertyDescription1).Return("Description");

            ExecuteRule(rule, 0, property);
        }

        [NUnit.Framework.Test]
        public void PropertyDescription2MandatoryTestFail()
        {
            PropertyDescription2Mandatory rule = new PropertyDescription2Mandatory();

            SetupResult.For(property.PropertyDescription2).Return(null);

            ExecuteRule(rule, 1, property);
        }

        [NUnit.Framework.Test]
        public void PropertyDescription2MandatoryTestPass()
        {
            PropertyDescription2Mandatory rule = new PropertyDescription2Mandatory();

            SetupResult.For(property.PropertyDescription2).Return("Description");

            ExecuteRule(rule, 0, property);
        }

        [NUnit.Framework.Test]
        public void PropertyDescription3MandatoryTestFail()
        {
            PropertyDescription3Mandatory rule = new PropertyDescription3Mandatory();

            SetupResult.For(property.PropertyDescription3).Return(null);

            ExecuteRule(rule, 1, property);
        }

        [NUnit.Framework.Test]
        public void PropertyDescription3MandatoryTestPass()
        {
            PropertyDescription3Mandatory rule = new PropertyDescription3Mandatory();

            SetupResult.For(property.PropertyDescription3).Return("Description");

            ExecuteRule(rule, 0, property);
        }

        //[NUnit.Framework.Test]
        //public void PropertyNoUpdateOnOpenLoanTestFail()
        //{
        //    using (new SessionScope())
        //    {
        //        PropertyNoUpdateOnOpenLoan rule = new PropertyNoUpdateOnOpenLoan();

        //        Stack<IAccount> accProperties = new Stack<IAccount>();
        //        IAccount account = _mockery.StrictMock<IAccount>();
        //        //setup a closed account status
        //        IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
        //        SetupResult.For(accountStatus.Key).Return((int)SAHL.Common.Globals.AccountStatuses.Open);
        //        SetupResult.For(account.AccountStatus).Return(accountStatus);
        //        // add the account to the collection
        //        accProperties.Push(account);

        //        IEventList<IAccount> accountProperties = new EventList<IAccount>(accProperties);
        //        SetupResult.For(property.AccountProperties).Return(accountProperties);

        //        ExecuteRule(rule, 1, property);
        //    }
        //}

        [NUnit.Framework.Test]
        public void PropertyNoUpdateOnOpenLoanTestFail()
        {
            using (new SessionScope())
            {
                PropertyNoUpdateOnOpenLoan rule = new PropertyNoUpdateOnOpenLoan();

                Stack<IFinancialService> mlProperties = new Stack<IFinancialService>();
                IFinancialService finService = _mockery.StrictMock<IFinancialService>();

                //setup a closed account status
                IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
                SetupResult.For(accountStatus.Key).Return((int)SAHL.Common.Globals.AccountStatuses.Open);
                SetupResult.For(finService.AccountStatus).Return(accountStatus);

                // add the fin service to the collection
                mlProperties.Push(finService);

                IApplication app = _mockery.StrictMock<IApplication>();
                IApplicationType appType = _mockery.StrictMock<IApplicationType>();
                SetupResult.For(appType.Key).Return((int)OfferTypes.SwitchLoan);
                SetupResult.For(app.ApplicationType).Return(appType);

                IEventList<IFinancialService> mortgageLoanProperties = new EventList<IFinancialService>(mlProperties);
                SetupResult.For(property.MortgageLoanProperties).Return(mortgageLoanProperties);

                ExecuteRule(rule, 1, property, app);
            }
        }

        [NUnit.Framework.Test]
        public void PropertyNoUpdateOnOpenLoanTestPass()
        {
            using (new SessionScope())
            {
                PropertyNoUpdateOnOpenLoan rule = new PropertyNoUpdateOnOpenLoan();

                Stack<IFinancialService> mlProperties = new Stack<IFinancialService>();
                IFinancialService finService = _mockery.StrictMock<IFinancialService>();

                //setup a closed account status
                IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
                SetupResult.For(accountStatus.Key).Return((int)SAHL.Common.Globals.AccountStatuses.Closed);
                SetupResult.For(finService.AccountStatus).Return(accountStatus);

                // add the fin service to the collection
                mlProperties.Push(finService);

                IEventList<IFinancialService> mortgageLoanProperties = new EventList<IFinancialService>(mlProperties);
                SetupResult.For(property.MortgageLoanProperties).Return(mortgageLoanProperties);

                IApplication app = _mockery.StrictMock<IApplication>();
                IApplicationType appType = _mockery.StrictMock<IApplicationType>();
                SetupResult.For(appType.Key).Return((int)OfferTypes.SwitchLoan);
                SetupResult.For(app.ApplicationType).Return(appType);

                ExecuteRule(rule, 0, property, app);
            }
        }

        [NUnit.Framework.Test]
        public void PropertyTitleDeedNumberMandatoryUsingPropertyTestFail()
        {
            PropertyTitleDeedNumberMandatory rule = new PropertyTitleDeedNumberMandatory();

            SetupResult.For(property.PropertyTitleDeeds).Return(null);

            ExecuteRule(rule, 1, property);
        }

        [NUnit.Framework.Test]
        public void PropertyTitleDeedNumberMandatoryUsingPropertyTestPass()
        {
            PropertyTitleDeedNumberMandatory rule = new PropertyTitleDeedNumberMandatory();

            IEventList<IPropertyTitleDeed> propTitleDeeds = new EventList<IPropertyTitleDeed>();
            IPropertyTitleDeed propTitleDeed = _mockery.StrictMock<IPropertyTitleDeed>();

            SetupResult.For(propTitleDeed.TitleDeedNumber).Return("123");
            SetupResult.For(propTitleDeed.Key).Return(1);
            propTitleDeeds.Add(new DomainMessageCollection(), propTitleDeed);

            SetupResult.For(property.PropertyTitleDeeds).Return(propTitleDeeds);

            ExecuteRule(rule, 0, property);
        }

        [NUnit.Framework.Test]
        public void PropertyTitleDeedNumberMandatoryUsingStringTestFail()
        {
            PropertyTitleDeedNumberMandatory rule = new PropertyTitleDeedNumberMandatory();

            //SetupResult.For(property.PropertyTitleDeeds).Return(null);

            ExecuteRule(rule, 1, property, "");
        }

        [NUnit.Framework.Test]
        public void PropertyTitleDeedNumberMandatoryUsingStringTestPass()
        {
            PropertyTitleDeedNumberMandatory rule = new PropertyTitleDeedNumberMandatory();

            //SetupResult.For(property.PropertyTitleDeeds).Return(_mockery.StrictMock<IPropertyTitleDeed>());

            ExecuteRule(rule, 0, property, "123");
        }

        #region PropertySectionalSchemeNameMandatory

        /// <summary>
        /// Property Sectional Scheme Name Mandatory
        /// </summary>
        [NUnit.Framework.Test]
        public void PropertySectionalSchemeNameMandatoryPass()
        {
            foreach (TitleTypes titleType in Enum.GetValues(typeof(TitleTypes)))
            {
                if (titleType != TitleTypes.SectionalTitle &
                   titleType != TitleTypes.SectionalTitleWithHOC)
                {
                    PropertySectionalSchemeNameMandatoryHelper(titleType, String.Empty, 0);
                }
            }
        }

        /// <summary>
        /// Property Sectional Scheme Name Mandatory
        /// </summary>
        [NUnit.Framework.Test]
        public void PropertySectionalSchemeNameMandatoryFail()
        {
            foreach (TitleTypes titleType in Enum.GetValues(typeof(TitleTypes)))
            {
                if (titleType == TitleTypes.SectionalTitle |
                   titleType == TitleTypes.SectionalTitleWithHOC)
                {
                    PropertySectionalSchemeNameMandatoryHelper(titleType, null, 1);
                }
            }
        }

        /// <summary>
        /// Property Sectional Scheme Name Mandatory
        /// </summary>
        [NUnit.Framework.Test]
        public void PropertySectionalSchemeNameMandatoryWithSectionalSchemeNamePass()
        {
            foreach (TitleTypes titleType in Enum.GetValues(typeof(TitleTypes)))
            {
                if (titleType == TitleTypes.SectionalTitle |
                   titleType == TitleTypes.SectionalTitleWithHOC)
                {
                    PropertySectionalSchemeNameMandatoryHelper(titleType, "Test", 0);
                }
            }
        }

        /// <summary>
        /// Property Sectional Scheme Name Mandatory
        /// </summary>
        [NUnit.Framework.Test]
        public void PropertySectionalSchemeNameMandatoryWithSectionalSchemeNameFail()
        {
            foreach (TitleTypes titleType in Enum.GetValues(typeof(TitleTypes)))
            {
                if (titleType == TitleTypes.SectionalTitle |
                   titleType == TitleTypes.SectionalTitleWithHOC)
                {
                    PropertySectionalSchemeNameMandatoryHelper(titleType, String.Empty, 1);
                }
            }
        }

        /// <summary>
        /// Ensure that if the property is a sectional title that the following is mandatory
        /// SectionalTitleName, SectionalTitleUnit, SectionalTitleSchemeNumber
        /// </summary>
        private void PropertySectionalSchemeNameMandatoryHelper(TitleTypes titleTypeToCheck, string sectionalSchemeName, int expectedMessageCount)
        {
            PropertySectionalSchemeNameMandatory rule = new PropertySectionalSchemeNameMandatory();
            IProperty property = _mockery.StrictMock<IProperty>();
            ITitleType titleType = _mockery.StrictMock<ITitleType>();

            SetupResult.For(titleType.Key).Return((int)titleTypeToCheck);
            SetupResult.For(property.SectionalSchemeName).Return(sectionalSchemeName);
            SetupResult.For(property.TitleType).Return(titleType);

            ExecuteRule(rule, expectedMessageCount, property);
        }

        #endregion PropertySectionalSchemeNameMandatory

        #region PropertySectionalUnitNumberMandatory

        /// <summary>
        /// Pass : Property Sectional Unit Number Mandatory
        /// </summary>
        [NUnit.Framework.Test]
        public void PropertySectionalUnitNumberMandatoryPass()
        {
            foreach (TitleTypes titleType in Enum.GetValues(typeof(TitleTypes)))
            {
                if (titleType != TitleTypes.SectionalTitle &
                   titleType != TitleTypes.SectionalTitleWithHOC)
                {
                    PropertySectionalUnitNumberMandatoryHelper(titleType, String.Empty, 0);
                }
            }
        }

        /// <summary>
        /// Fail : Property Sectional Unit Number Mandatory
        /// </summary>
        [NUnit.Framework.Test]
        public void PropertySectionalUnitNumberMandatoryFail()
        {
            foreach (TitleTypes titleType in Enum.GetValues(typeof(TitleTypes)))
            {
                if (titleType == TitleTypes.SectionalTitle |
                   titleType == TitleTypes.SectionalTitleWithHOC)
                {
                    PropertySectionalUnitNumberMandatoryHelper(titleType, null, 1);
                }
            }
        }

        /// <summary>
        /// Pass : Property Sectional Unit Number Mandatory (With a Number)
        /// </summary>
        [NUnit.Framework.Test]
        public void PropertySectionalUnitNumberMandatoryWithUnitNumberPass()
        {
            foreach (TitleTypes titleType in Enum.GetValues(typeof(TitleTypes)))
            {
                if (titleType == TitleTypes.SectionalTitle |
                   titleType == TitleTypes.SectionalTitleWithHOC)
                {
                    PropertySectionalUnitNumberMandatoryHelper(titleType, "Test", 0);
                }
            }
        }

        /// <summary>
        /// Fail : Property Sectional Unit Number Mandatory (Without a Number)
        /// </summary>
        [NUnit.Framework.Test]
        public void PropertySectionalUnitNumberMandatoryWithUnitNumberFail()
        {
            foreach (TitleTypes titleType in Enum.GetValues(typeof(TitleTypes)))
            {
                if (titleType == TitleTypes.SectionalTitle |
                   titleType == TitleTypes.SectionalTitleWithHOC)
                {
                    PropertySectionalUnitNumberMandatoryHelper(titleType, String.Empty, 1);
                }
            }
        }

        /// <summary>
        /// Ensure that if the property is a sectional title that the following is mandatory
        /// SectionalTitleName, sectionalUnitNumber, SectionalTitleSchemeNumber
        /// </summary>
        private void PropertySectionalUnitNumberMandatoryHelper(TitleTypes titleTypeToCheck, string sectionalUnitNumber, int expectedMessageCount)
        {
            PropertySectionalUnitNumberMandatory rule = new PropertySectionalUnitNumberMandatory();
            IProperty property = _mockery.StrictMock<IProperty>();
            ITitleType titleType = _mockery.StrictMock<ITitleType>();

            SetupResult.For(titleType.Key).Return((int)titleTypeToCheck);
            SetupResult.For(property.SectionalUnitNumber).Return(sectionalUnitNumber);
            SetupResult.For(property.TitleType).Return(titleType);

            ExecuteRule(rule, expectedMessageCount, property);
        }

        #endregion PropertySectionalUnitNumberMandatory

        [NUnit.Framework.Test]
        public void DetermineDuplicateApplicationTestPass()
        {
            DetermineDuplicateApplication rule = new DetermineDuplicateApplication(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            using (new SessionScope())
            {
                string sqlQuery = "Select OfferKey, PropertyKey" +
                                   " From OfferMortgageLoan" +
                                   " Where PropertyKey in" +
                                   " (" +
                                   " Select PropertyKey" +
                                   " From OfferMortgageLoan" +
                                   " Group By PropertyKey" +
                                   " Having Count(OfferKey) = 1)";
                ParameterCollection parameters = new ParameterCollection();

                DataSet dsOffers = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

                if (dsOffers != null)
                {
                    if (dsOffers.Tables.Count > 0)
                    {
                        if (dsOffers.Tables[0].Rows.Count > 0)
                        {
                            int iOfferKey = Convert.ToInt32(dsOffers.Tables[0].Rows[0]["OfferKey"]);
                            int iPropertyKey = Convert.ToInt32(dsOffers.Tables[0].Rows[0]["PropertyKey"]);

                            ExecuteRule(rule, 0, iOfferKey, iPropertyKey);
                        }
                    }
                }
                else
                {
                    Assert.Fail("No valid keys for this test");
                }
            }
        }

        #region ApplicationCaptureMinimumPropertyDataTest

        [NUnit.Framework.Test]
        public void ApplicationCaptureMinimumPropertyDataTest()
        {
            // Passing in all fields so should PASS
            ApplicationCaptureMinimumPropertyDataTestHelper(0, "Property Description 1", "Property Description 2", "Property Description 3", "Property Access Details Contact", "Property Access Details Phone", true);

            // Not Passing in all fields so should FAIL
            ApplicationCaptureMinimumPropertyDataTestHelper(1, "", "Property Description 2", "Property Description 3", "Property Access Details Contact", "Property Access Details Phone", false);
        }

        private void ApplicationCaptureMinimumPropertyDataTestHelper(int expectedMessageCount, string propDesc1, string propDesc2, string propDesc3, string propAccessDetailsContact, string propAccessDetailsPhone, bool addressExists)
        {
            using (new SessionScope())
            {
                ApplicationCaptureMinimumPropertyData rule = new ApplicationCaptureMinimumPropertyData();

                IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
                IProperty prop = _mockery.StrictMock<IProperty>();
                IAddress addr = _mockery.StrictMock<IAddress>();
                IPropertyType propType = _mockery.StrictMock<IPropertyType>();
                ITitleType titleType = _mockery.StrictMock<ITitleType>();
                IOccupancyType occType = _mockery.StrictMock<IOccupancyType>();
                IPropertyAccessDetails propAccessDetails = _mockery.StrictMock<IPropertyAccessDetails>();

                SetupResult.For(addr.Key).Return(1);
                SetupResult.For(propType.Key).Return(1);
                SetupResult.For(titleType.Key).Return(1);
                SetupResult.For(occType.Key).Return(1);

                SetupResult.For(prop.PropertyDescription1).Return(propDesc1);
                SetupResult.For(prop.PropertyDescription2).Return(propDesc3);
                SetupResult.For(prop.PropertyDescription3).Return(propDesc3);
                SetupResult.For(propAccessDetails.Contact1).Return(propAccessDetailsContact);
                SetupResult.For(propAccessDetails.Contact1Phone).Return(propAccessDetailsPhone);

                SetupResult.For(prop.PropertyAccessDetails).Return(propAccessDetails);
                SetupResult.For(prop.OccupancyType).Return(occType);
                SetupResult.For(prop.TitleType).Return(titleType);
                SetupResult.For(prop.PropertyType).Return(propType);
                if (addressExists)
                {
                    SetupResult.For(prop.Address).Return(addr);
                }
                else
                {
                    SetupResult.For(prop.Address).Return(null);
                }

                SetupResult.For(app.Property).Return(prop);
                ExecuteRule(rule, expectedMessageCount, app);
            }
        }

        #endregion ApplicationCaptureMinimumPropertyDataTest

        #region ManagementApplicationMinimumPropertyDataTest

        [NUnit.Framework.Test]
        public void ManagementApplicationMinimumPropertyDataTest()
        {
            // Passing in all fields so should PASS
            ManagementApplicationMinimumPropertyDataTestHelper(0, "Property Description 1", "Property Description 2", "Property Description 3",
                                                                "Property Access Details Contact", "Property Access Details Phone", true,
                                                                (int)DeedsPropertyTypes.Unit, "Sectional String Value", "Sectional Unit Number",
                                                                "Title Deed Number", "Erf Number", "Erf Suburb Description", "Erf Metro Description");

            // Not Passing in all fields so should FAIL
            ManagementApplicationMinimumPropertyDataTestHelper(1, "", "Property Description 2", "Property Description 3",
                                                                "Property Access Details Contact", "Property Access Details Phone", false,
                                                                (int)DeedsPropertyTypes.Unit, "Sectional String Value", "Sectional Unit Number",
                                                                "Title Deed Number", "Erf Number", "Erf Suburb Description", "Erf Metro Description");

            // Not Passing in all fields so should FAIL
            ManagementApplicationMinimumPropertyDataTestHelper(1, "Property Description 1", "Property Description 2", "Property Description 3",
                                                                "Property Access Details Contact", "Property Access Details Phone", true,
                                                                (int)DeedsPropertyTypes.Erf, "Sectional String Value", "Sectional Unit Number",
                                                                "", "Erf Number", "Erf Suburb Description", "Erf Metro Description");
        }

        private void ManagementApplicationMinimumPropertyDataTestHelper(int expectedMessageCount, string propDesc1, string propDesc2, string propDesc3, string propAccessDetailsContact,
                                                                            string propAccessDetailsPhone, bool addressExists, int dptValue, string secSchemeName, string secUnitNum,
                                                                            string titleDeedNum, string erfNum, string erfSubDesc, string erfMetdesc)
        {
            using (new SessionScope())
            {
                ManagementApplicationMinimumPropertyData rule = new ManagementApplicationMinimumPropertyData();

                IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
                IProperty prop = _mockery.StrictMock<IProperty>();
                IAddress addr = _mockery.StrictMock<IAddress>();
                IPropertyType propType = _mockery.StrictMock<IPropertyType>();
                ITitleType titleType = _mockery.StrictMock<ITitleType>();
                IOccupancyType occType = _mockery.StrictMock<IOccupancyType>();
                IPropertyAccessDetails propAccessDetails = _mockery.StrictMock<IPropertyAccessDetails>();
                IDeedsPropertyType deedsPropType = _mockery.StrictMock<IDeedsPropertyType>();

                IEventList<IPropertyTitleDeed> propTitleDeeds = new EventList<IPropertyTitleDeed>();
                IPropertyTitleDeed propTitleDeed = _mockery.StrictMock<IPropertyTitleDeed>();

                SetupResult.For(propTitleDeed.TitleDeedNumber).Return(titleDeedNum);
                SetupResult.For(propTitleDeed.Key).Return(1);
                propTitleDeeds.Add(new DomainMessageCollection(), propTitleDeed);

                SetupResult.For(prop.PropertyTitleDeeds).Return(propTitleDeeds);

                SetupResult.For(addr.Key).Return(1);
                SetupResult.For(propType.Key).Return(1);
                SetupResult.For(titleType.Key).Return(1);
                SetupResult.For(occType.Key).Return(1);

                SetupResult.For(prop.PropertyDescription1).Return(propDesc1);
                SetupResult.For(prop.PropertyDescription2).Return(propDesc3);
                SetupResult.For(prop.PropertyDescription3).Return(propDesc3);
                SetupResult.For(propAccessDetails.Contact1).Return(propAccessDetailsContact);
                SetupResult.For(propAccessDetails.Contact1Phone).Return(propAccessDetailsPhone);
                SetupResult.For(prop.ErfNumber).Return(erfNum);
                SetupResult.For(prop.ErfSuburbDescription).Return(erfSubDesc);
                SetupResult.For(prop.ErfMetroDescription).Return(erfMetdesc);

                SetupResult.For(prop.PropertyAccessDetails).Return(propAccessDetails);
                SetupResult.For(prop.OccupancyType).Return(occType);
                SetupResult.For(prop.TitleType).Return(titleType);
                SetupResult.For(prop.PropertyType).Return(propType);
                if (addressExists)
                {
                    SetupResult.For(prop.Address).Return(addr);
                }
                else
                {
                    SetupResult.For(prop.Address).Return(null);
                }

                SetupResult.For(app.Property).Return(prop);

                SetupResult.For(prop.SectionalSchemeName).Return(secSchemeName);
                SetupResult.For(prop.SectionalUnitNumber).Return(secUnitNum);

                SetupResult.For(deedsPropType.Key).Return(dptValue);
                SetupResult.For(prop.DeedsPropertyType).Return(deedsPropType);

                ExecuteRule(rule, expectedMessageCount, app);
            }
        }

        #endregion ManagementApplicationMinimumPropertyDataTest

        /// <summary>
        /// This interface is created for mocking purposes only, for rules that cast IValuation objects
        /// to IValuationDiscriminatedLightstoneAVM objects.
        /// </summary>
        public interface IValuationValuationDiscriminatedLightstoneAVM : IValuation, IValuationDiscriminatedLightstoneAVM
        {
        }

        [NUnit.Framework.Test]
        public void LightStoneValuationRecentExists()
        {
            LightStoneValuationRecentExistsHelper(DateTime.Now, 1);
            LightStoneValuationRecentExistsHelper(DateTime.Now.AddMonths(-3), 0);
        }

        private void LightStoneValuationRecentExistsHelper(DateTime dt, int msgCount)
        {
            LightStoneValuationRecent rule = new LightStoneValuationRecent();

            IValuationValuationDiscriminatedLightstoneAVM lsVal = _mockery.StrictMock<IValuationValuationDiscriminatedLightstoneAVM>();

            SetupResult.For(lsVal.Key).Return(1);
            SetupResult.For(lsVal.ValuationDate).Return(dt);

            IProperty prop = _mockery.StrictMock<IProperty>();
            SetupResult.For(prop.Key).Return(1);

            IEventList<IValuation> listVal = new EventList<IValuation>();
            listVal.Add(null, lsVal);

            SetupResult.For(prop.Valuations).Return(listVal);

            ExecuteRule(rule, msgCount, prop);
        }
    }
}