using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Rules.Products;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Test.Products
{
    [TestFixture]
    public class MortgageLoan : RuleBase
    {
        public interface IApplicationProductSupportsVariableLoanApplicationInformation : IApplicationProduct, ISupportsVariableLoanApplicationInformation
        {
        }

        [SetUp]
        public new void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public new void TearDown()
        {
            base.TearDown();
        }

        #region Application MortgageLoan Spec

        #region ApplicationAssetLiabilityRequiredLoanAmount

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationAssetLiabilityRequiredLoanAmountNoArgumentsPassed()
        {
            ApplicationAssetLiabilityRequiredLoanAmount rule = new ApplicationAssetLiabilityRequiredLoanAmount();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationAssetLiabilityRequiredLoanAmountIncorrectArgumentsPassed()
        {
            ApplicationAssetLiabilityRequiredLoanAmount rule = new ApplicationAssetLiabilityRequiredLoanAmount();

            // Setup an incorrect Argumnt to pass along
            IApplication application = _mockery.StrictMock<IApplication>();

            ExecuteRule(rule, 0, application);
        }

        /// <summary>
        /// Expects 0 Domain messages.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationAssetLiabilityRequiredLoanAmountSuccess()
        {
            ApplicationAssetLiabilityRequiredLoanAmount rule = new ApplicationAssetLiabilityRequiredLoanAmount();

            // Setup an incorrect Argumnt to pass along
            IApplicationMortgageLoan mortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductMortgageLoan applicationProductMortgageLoan = _mockery.StrictMock<IApplicationProductMortgageLoan>();
            SetupResult.For(mortgageLoan.CurrentProduct).Return(applicationProductMortgageLoan);

            // Setup LoanAgreementAmount
            SetupResult.For(applicationProductMortgageLoan.LoanAgreementAmount).Return(1600000.00);

            // Setup ILegalEntity.LegalEntityAssetLiabilities
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            ILegalEntityAssetLiability legalEntityAssetLiability = _mockery.StrictMock<ILegalEntityAssetLiability>();
            IEventList<ILegalEntityAssetLiability> legalEntityAssetLiabilities = new EventList<ILegalEntityAssetLiability>();
            legalEntityAssetLiabilities.Add(Messages, legalEntityAssetLiability);
            SetupResult.For(legalEntity.LegalEntityAssetLiabilities).Return(legalEntityAssetLiabilities);

            // Setup IApplication.ApplicationRoles
            IApplication application = _mockery.CreateMultiMock<IApplication>();
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> applicationRoleStack = new Stack<IApplicationRole>();
            applicationRoleStack.Push(applicationRole);
            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationRoleStack);

            SetupResult.For(applicationRole.LegalEntity).Return(legalEntity);
            SetupResult.For(application.ApplicationRoles).Return(applicationRoles);

            // Setup applicationProductMortgageLoan.Application
            SetupResult.For(applicationProductMortgageLoan.Application).Return(application);

            ExecuteRule(rule, 0, mortgageLoan);
        }

        /// <summary>
        /// Expects 1 Domain message.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationAssetLiabilityRequiredLoanAmountFailure()
        {
            ApplicationAssetLiabilityRequiredLoanAmount rule = new ApplicationAssetLiabilityRequiredLoanAmount();

            // Setup an incorrect Argumnt to pass along
            IApplicationMortgageLoan mortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductMortgageLoan applicationProductMortgageLoan = _mockery.StrictMock<IApplicationProductMortgageLoan>();
            SetupResult.For(mortgageLoan.CurrentProduct).Return(applicationProductMortgageLoan);

            // Setup LoanAgreementAmount
            SetupResult.For(applicationProductMortgageLoan.LoanAgreementAmount).Return(1600000.00);

            // Setup ILegalEntity.LegalEntityAssetLiabilities
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            IEventList<ILegalEntityAssetLiability> legalEntityAssetLiabilities = new EventList<ILegalEntityAssetLiability>();
            SetupResult.For(legalEntity.LegalEntityAssetLiabilities).Return(legalEntityAssetLiabilities);

            // Setup IApplication.ApplicationRoles
            IApplication application = _mockery.CreateMultiMock<IApplication>();
            IApplicationRole applicationRole = _mockery.CreateMultiMock<IApplicationRole>();
            IEventList<IApplicationRole> appRoles = new EventList<IApplicationRole>();
            appRoles.Add(Messages, applicationRole);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);
            SetupResult.For(applicationRole.LegalEntity).Return(legalEntity);
            SetupResult.For(application.ApplicationRoles).Return(applicationRoles);

            // Setup applicationProductMortgageLoan.Application
            SetupResult.For(applicationProductMortgageLoan.Application).Return(application);

            ExecuteRule(rule, 1, mortgageLoan);
        }

        #endregion ApplicationAssetLiabilityRequiredLoanAmount

        #region ApplicationAssetLiabilityRequiredEmploymentType

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationAssetLiabilityRequiredEmploymentTypeNoArgumentsPassed()
        {
            ApplicationAssetLiabilityRequiredEmploymentType rule = new ApplicationAssetLiabilityRequiredEmploymentType();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationAssetLiabilityRequiredEmploymentTypeIncorrectArgumentsPassed()
        {
            ApplicationAssetLiabilityRequiredEmploymentType rule = new ApplicationAssetLiabilityRequiredEmploymentType();

            // Setup an incorrect Argumnt to pass along
            IApplicationMailingAddress application = _mockery.StrictMock<IApplicationMailingAddress>();
            ExecuteRule(rule, 0, application);
        }

        /// <summary>
        /// Expects Messages.Count <> 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationAssetLiabilityRequiredEmploymentTypeFailure()
        {
            ApplicationAssetLiabilityRequiredEmploymentType rule = new ApplicationAssetLiabilityRequiredEmploymentType();

            // Setup a correct Argument to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(applicationMortgageLoan.Key).Return(1);

            // Setup ILegalEntity.LegalEntityAssetLiabilities
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            IEventList<ILegalEntityAssetLiability> legalEntityAssetLiabilities = new EventList<ILegalEntityAssetLiability>();
            SetupResult.For(legalEntity.LegalEntityAssetLiabilities).Return(legalEntityAssetLiabilities);

            // Setup ILegalEntity.Employment
            IEventList<IEmployment> employments = new EventList<IEmployment>();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentType employmentType = _mockery.StrictMock<IEmploymentType>();
            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Current);
            SetupResult.For(employmentType.Key).Return((int)EmploymentTypes.SelfEmployed);
            SetupResult.For(employment.EmploymentType).Return(employmentType);
            SetupResult.For(employment.EmploymentStatus).Return(employmentStatus);
            employments.Add(Messages, employment);
            SetupResult.For(legalEntity.Employment).Return(employments);

            // Setup IApplication.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            IEventList<IApplicationRole> appRoles = new EventList<IApplicationRole>();
            appRoles.Add(Messages, applicationRole);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);
            SetupResult.For(applicationRole.LegalEntity).Return(legalEntity);
            SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(applicationRoles);

            IApplicationProductVariFixLoan appProduct = _mockery.StrictMock<IApplicationProductVariFixLoan>();
            IApplicationInformationVariableLoan appInfoVar = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(appInfoVar.EmploymentType).Return(employmentType);
            SetupResult.For(appProduct.VariableLoanInformation).Return(appInfoVar);
            SetupResult.For(applicationMortgageLoan.CurrentProduct).Return(appProduct);

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationAssetLiabilityRequiredEmploymentTypeSuccess()
        {
            ApplicationAssetLiabilityRequiredEmploymentType rule = new ApplicationAssetLiabilityRequiredEmploymentType();

            // Setup a correct Argumnt to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductSupportsVariableLoanApplicationInformation appProd = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
            IApplicationInformationVariableLoan aivl = _mockery.StrictMock<IApplicationInformationVariableLoan>();

            // Setup applicationMortgageLoan.GetEmploymentType(false)
            //TODO:
            //SetupResult.For(applicationMortgageLoan.GetEmploymentType(false)).Return(EmploymentTypes.SelfEmployed);

            // Setup ILegalEntity.LegalEntityAssetLiabilities
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            IEventList<ILegalEntityAssetLiability> legalEntityAssetLiabilities = new EventList<ILegalEntityAssetLiability>();
            ILegalEntityAssetLiability legalEntityAssetLiability = _mockery.StrictMock<ILegalEntityAssetLiability>();
            legalEntityAssetLiabilities.Add(Messages, legalEntityAssetLiability);
            SetupResult.For(legalEntity.LegalEntityAssetLiabilities).Return(legalEntityAssetLiabilities);

            // Setup ILegalEntity.Employment
            IEventList<IEmployment> employments = new EventList<IEmployment>();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentType employmentType = _mockery.StrictMock<IEmploymentType>();
            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Current);
            SetupResult.For(employmentType.Key).Return((int)EmploymentTypes.SelfEmployed);
            SetupResult.For(employment.EmploymentType).Return(employmentType);
            SetupResult.For(employment.EmploymentStatus).Return(employmentStatus);
            employments.Add(Messages, employment);
            SetupResult.For(legalEntity.Employment).Return(employments);

            // Setup IApplication.ApplicationRoles
            IApplicationRole applicationRole = _mockery.CreateMultiMock<IApplicationRole>();
            IEventList<IApplicationRole> appRoles = new EventList<IApplicationRole>();
            appRoles.Add(Messages, applicationRole);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);

            SetupResult.For(applicationRole.LegalEntity).Return(legalEntity);
            SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(applicationRoles);
            SetupResult.For(applicationMortgageLoan.Key).Return(1);
            SetupResult.For(aivl.EmploymentType).Return(employmentType);
            SetupResult.For(aivl.LTV).Return(new double?(2D));
            SetupResult.For(appProd.VariableLoanInformation).Return(aivl);
            SetupResult.For(applicationMortgageLoan.CurrentProduct).Return(appProd);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        #endregion ApplicationAssetLiabilityRequiredEmploymentType

        #region ApplicationProperty

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationPropertyNoArgumentsPassed()
        {
            ApplicationProperty rule = new ApplicationProperty();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationPropertyIncorrectArgumentsPassed()
        {
            ApplicationProperty rule = new ApplicationProperty();

            // Setup an incorrect Argumnt to pass along
            IApplicationMailingAddress application = _mockery.StrictMock<IApplicationMailingAddress>();

            ExecuteRule(rule, 0, application);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationPropertySuccess()
        {
            ApplicationProperty rule = new ApplicationProperty();

            // Setup a correct Argumnt to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(applicationMortgageLoan.Key).Return(10);

            // Setup applicationMortgageLoan.property
            IProperty property = _mockery.StrictMock<IProperty>();
            SetupResult.For(applicationMortgageLoan.Property).Return(property);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count <> 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationPropertyFailure()
        {
            ApplicationProperty rule = new ApplicationProperty();

            // Setup a correct Argumnt to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.property
            SetupResult.For(applicationMortgageLoan.Key).Return(10);
            SetupResult.For(applicationMortgageLoan.Property).Return(null);

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        #endregion ApplicationProperty

        #region MortgageLoanLegalEntityEmploymentActive

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void MortgageLoanLegalEntityEmploymentActiveNoArgumentsPassed()
        {
            MortgageLoanLegalEntityEmploymentActive rule = new MortgageLoanLegalEntityEmploymentActive();
            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void MortgageLoanLegalEntityEmploymentActiveIncorrectArgumentsPassed()
        {
            MortgageLoanLegalEntityEmploymentActive rule = new MortgageLoanLegalEntityEmploymentActive();

            // Setup an incorrect Argumnt to pass along
            IEmployer emp = _mockery.StrictMock<IEmployer>();

            ExecuteRule(rule, 0, emp);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanLegalEntityEmploymentActiveSuccess()
        {
            MortgageLoanLegalEntityEmploymentActive rule = new MortgageLoanLegalEntityEmploymentActive();

            // Setup a correct Argumnt to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(applicationMortgageLoan.Key).Return(10);

            // Setup applicationMortgageLoan.GetEmploymentType(false)
            //TODO:
            //SetupResult.For(applicationMortgageLoan.GetEmploymentType(false)).Return(EmploymentTypes.SelfEmployed);

            // Setup ILegalEntity.LegalEntityAssetLiabilities
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            IEventList<ILegalEntityAssetLiability> legalEntityAssetLiabilities = new EventList<ILegalEntityAssetLiability>();
            ILegalEntityAssetLiability legalEntityAssetLiability = _mockery.StrictMock<ILegalEntityAssetLiability>();
            legalEntityAssetLiabilities.Add(Messages, legalEntityAssetLiability);
            SetupResult.For(legalEntity.LegalEntityAssetLiabilities).Return(legalEntityAssetLiabilities);

            // Setup ILegalEntity.Employment
            IEventList<IEmployment> employments = new EventList<IEmployment>();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentType employmentType = _mockery.StrictMock<IEmploymentType>();
            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Current);
            SetupResult.For(employmentType.Key).Return((int)EmploymentTypes.SelfEmployed);
            SetupResult.For(employment.EmploymentType).Return(employmentType);
            SetupResult.For(employment.EmploymentStatus).Return(employmentStatus);
            employments.Add(Messages, employment);
            SetupResult.For(legalEntity.Employment).Return(employments);

            // Setup IApplication.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> appRoles = new Stack<IApplicationRole>();
            appRoles.Push(applicationRole);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);
            SetupResult.For(applicationRole.LegalEntity).Return(legalEntity);
            SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(applicationRoles);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count <> 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanLegalEntityEmploymentActiveFail()
        {
            MortgageLoanLegalEntityEmploymentActive rule = new MortgageLoanLegalEntityEmploymentActive();

            // Setup a correct Argumnt to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(applicationMortgageLoan.Key).Return(10);

            // Setup applicationMortgageLoan.GetEmploymentType(false)
            //TODO:
            //SetupResult.For(applicationMortgageLoan.GetEmploymentType()).Return(EmploymentTypes.SelfEmployed);

            // Setup ILegalEntity.LegalEntityAssetLiabilities
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            IEventList<ILegalEntityAssetLiability> legalEntityAssetLiabilities = new EventList<ILegalEntityAssetLiability>();
            ILegalEntityAssetLiability legalEntityAssetLiability = _mockery.StrictMock<ILegalEntityAssetLiability>();
            legalEntityAssetLiabilities.Add(Messages, legalEntityAssetLiability);
            SetupResult.For(legalEntity.LegalEntityAssetLiabilities).Return(legalEntityAssetLiabilities);

            // Setup ILegalEntity.Employment
            IEventList<IEmployment> employments = new EventList<IEmployment>();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentType employmentType = _mockery.StrictMock<IEmploymentType>();
            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Previous);
            SetupResult.For(employmentType.Key).Return((int)EmploymentTypes.SelfEmployed);
            SetupResult.For(employment.EmploymentType).Return(employmentType);
            SetupResult.For(employment.EmploymentStatus).Return(employmentStatus);
            employments.Add(Messages, employment);
            SetupResult.For(legalEntity.Employment).Return(employments);

            // Setup IApplication.ApplicationRoles
            IApplicationRole applicationRole = _mockery.CreateMultiMock<IApplicationRole>();
            Stack<IApplicationRole> appRole = new Stack<IApplicationRole>();
            appRole.Push(applicationRole);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRole);
            SetupResult.For(applicationRole.LegalEntity).Return(legalEntity);
            SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(applicationRoles);

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        #endregion MortgageLoanLegalEntityEmploymentActive

        #region AccountMailingAddressAddressFormatFreeText

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void AccountMailingAddressAddressFormatFreeTextNoArgumentsPassed()
        {
            AccountMailingAddressAddressFormatFreeText rule = new AccountMailingAddressAddressFormatFreeText();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void AccountMailingAddressAddressFormatFreeTextIncorrectArgumentsPassed()
        {
            AccountMailingAddressAddressFormatFreeText rule = new AccountMailingAddressAddressFormatFreeText();

            // Setup an incorrect Argumnt to pass along
            IApplicationMailingAddress application = _mockery.StrictMock<IApplicationMailingAddress>();

            ExecuteRule(rule, 0, application);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AccountMailingAddressAddressFormatFreeTextSuccess()
        {
            AccountMailingAddressAddressFormatFreeText rule = new AccountMailingAddressAddressFormatFreeText();

            // Setup a correct Argumnt to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.IApplicationMailingAddresses
            IEventList<IApplicationMailingAddress> applicationMailingAddresses = new EventList<IApplicationMailingAddress>();
            SetupResult.For(applicationMortgageLoan.ApplicationMailingAddresses).Return(applicationMailingAddresses);

            // Setup IApplicationMailingAddress
            IApplicationMailingAddress applicationMailingAddress = _mockery.StrictMock<IApplicationMailingAddress>();
            IAddressBox addressBox = _mockery.StrictMock<IAddressBox>();
            IAddressFormat addressFormat = _mockery.StrictMock<IAddressFormat>();
            SetupResult.For(addressFormat.Key).Return((int)AddressFormats.PrivateBag);
            SetupResult.For(applicationMailingAddress.Address).Return(addressBox);
            SetupResult.For(addressBox.AddressFormat).Return(addressFormat);
            applicationMailingAddresses.Add(Messages, applicationMailingAddress);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count <> 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AccountMailingAddressAddressFormatFreeTextFail()
        {
            AccountMailingAddressAddressFormatFreeText rule = new AccountMailingAddressAddressFormatFreeText();

            // Setup a correct Argumnt to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.IApplicationMailingAddresses
            IEventList<IApplicationMailingAddress> applicationMailingAddresses = new EventList<IApplicationMailingAddress>();
            SetupResult.For(applicationMortgageLoan.ApplicationMailingAddresses).Return(applicationMailingAddresses);

            // Setup IApplicationMailingAddress
            IApplicationMailingAddress applicationMailingAddress = _mockery.StrictMock<IApplicationMailingAddress>();
            IAddressBox addressBox = _mockery.StrictMock<IAddressBox>();
            IAddressFormat addressFormat = _mockery.StrictMock<IAddressFormat>();
            SetupResult.For(addressFormat.Key).Return((int)AddressFormats.FreeText);
            SetupResult.For(applicationMailingAddress.Address).Return(addressBox);
            SetupResult.For(addressBox.AddressFormat).Return(addressFormat);
            applicationMailingAddresses.Add(Messages, applicationMailingAddress);

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        #endregion AccountMailingAddressAddressFormatFreeText

        #region ApplicationMortgageLoanPurchasePrice

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationMortgageLoanPurchasePriceNoArgumentsPassed()
        {
            ApplicationMortgageLoanPurchasePrice rule = new ApplicationMortgageLoanPurchasePrice();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationMortgageLoanPurchasePriceIncorrectArgumentsPassed()
        {
            ApplicationMortgageLoanPurchasePrice rule = new ApplicationMortgageLoanPurchasePrice();

            // Setup an incorrect Argumnt to pass along
            IApplication application = _mockery.StrictMock<IApplication>();

            ExecuteRule(rule, 0, application);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMortgageLoanPurchasePriceSuccess()
        {
            ApplicationMortgageLoanPurchasePrice rule = new ApplicationMortgageLoanPurchasePrice();

            // Setup a correct Argumnt to pass along
            IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = _mockery.StrictMock<IApplicationMortgageLoanNewPurchase>();

            // Setup applicationMortgageLoanNewPurchase.PurchasePrice
            SetupResult.For(applicationMortgageLoanNewPurchase.PurchasePrice).Return(500000.0);

            ExecuteRule(rule, 0, applicationMortgageLoanNewPurchase);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMortgageLoanPurchasePriceFail()
        {
            ApplicationMortgageLoanPurchasePrice rule = new ApplicationMortgageLoanPurchasePrice();

            // Setup a correct Argumnt to pass along
            IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = _mockery.StrictMock<IApplicationMortgageLoanNewPurchase>();

            // Setup applicationMortgageLoanNewPurchase.PurchasePrice
            SetupResult.For(applicationMortgageLoanNewPurchase.PurchasePrice).Return(null);

            ExecuteRule(rule, 1, applicationMortgageLoanNewPurchase);
        }

        #endregion ApplicationMortgageLoanPurchasePrice

        #region ApplicationMortgageLoanHouseholdIncome

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationMortgageLoanHouseholdIncomeNoArgumentsPassed()
        {
            ApplicationMortgageLoanHouseholdIncome rule = new ApplicationMortgageLoanHouseholdIncome();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationMortgageLoanHouseholdIncomeIncorrectArgumentsPassed()
        {
            ApplicationMortgageLoanHouseholdIncome rule = new ApplicationMortgageLoanHouseholdIncome();

            // Setup an incorrect Argumnt to pass along
            IApplication application = _mockery.StrictMock<IApplication>();

            ExecuteRule(rule, 0, application);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMortgageLoanHouseholdIncomeSuccess()
        {
            ApplicationMortgageLoanHouseholdIncome rule = new ApplicationMortgageLoanHouseholdIncome();

            // Setup a correct Argumnt to pass along
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = _mockery.StrictMock<ISupportsVariableLoanApplicationInformation>();

            // Setup supportsVariableLoanApplicationInformation.VariableLoanInformation
            IApplicationInformationVariableLoan variableLoanInformation = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(supportsVariableLoanApplicationInformation.VariableLoanInformation).Return(variableLoanInformation);

            // setup supportsVariableLoanApplicationInformation.VariableLoanInformation.HouseholdIncome
            SetupResult.For(variableLoanInformation.HouseholdIncome).Return(50000.0);

            ExecuteRule(rule, 0, supportsVariableLoanApplicationInformation);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMortgageLoanHouseholdIncomeFail()
        {
            ApplicationMortgageLoanHouseholdIncome rule = new ApplicationMortgageLoanHouseholdIncome();

            // Setup a correct Argumnt to pass along
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = _mockery.StrictMock<ISupportsVariableLoanApplicationInformation>();

            // Setup supportsVariableLoanApplicationInformation.VariableLoanInformation
            IApplicationInformationVariableLoan variableLoanInformation = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(supportsVariableLoanApplicationInformation.VariableLoanInformation).Return(variableLoanInformation);

            // setup supportsVariableLoanApplicationInformation.VariableLoanInformation.HouseholdIncome
            SetupResult.For(variableLoanInformation.HouseholdIncome).Return(null);

            ExecuteRule(rule, 1, supportsVariableLoanApplicationInformation);
        }

        #endregion ApplicationMortgageLoanHouseholdIncome

        #region ApplicationMortgageLoanBondToRegister

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationMortgageLoanBondToRegisterNoArgumentsPassed()
        {
            ApplicationMortgageLoanBondToRegister rule = new ApplicationMortgageLoanBondToRegister();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationMortgageLoanBondToRegisterIncorrectArgumentsPassed()
        {
            ApplicationMortgageLoanBondToRegister rule = new ApplicationMortgageLoanBondToRegister();

            // Setup an incorrect Argumnt to pass along
            IApplication application = _mockery.StrictMock<IApplication>();

            ExecuteRule(rule, 0, application);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMortgageLoanBondToRegisterSuccess()
        {
            ApplicationMortgageLoanBondToRegister rule = new ApplicationMortgageLoanBondToRegister();

            // Setup a correct Argumnt to pass along
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = _mockery.StrictMock<ISupportsVariableLoanApplicationInformation>();

            // Setup supportsVariableLoanApplicationInformation.VariableLoanInformation
            IApplicationInformationVariableLoan variableLoanInformation = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(supportsVariableLoanApplicationInformation.VariableLoanInformation).Return(variableLoanInformation);

            // setup supportsVariableLoanApplicationInformation.VariableLoanInformation.HouseholdIncome
            SetupResult.For(variableLoanInformation.BondToRegister).Return(160000.0);

            ExecuteRule(rule, 0, supportsVariableLoanApplicationInformation);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMortgageLoanBondToRegisterFail()
        {
            ApplicationMortgageLoanBondToRegister rule = new ApplicationMortgageLoanBondToRegister();

            // Setup a correct Argumnt to pass along
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = _mockery.StrictMock<ISupportsVariableLoanApplicationInformation>();

            // Setup supportsVariableLoanApplicationInformation.VariableLoanInformation
            IApplicationInformationVariableLoan variableLoanInformation = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(supportsVariableLoanApplicationInformation.VariableLoanInformation).Return(variableLoanInformation);

            // setup supportsVariableLoanApplicationInformation.VariableLoanInformation.HouseholdIncome
            SetupResult.For(variableLoanInformation.BondToRegister).Return(null);

            ExecuteRule(rule, 1, supportsVariableLoanApplicationInformation);
        }

        #endregion ApplicationMortgageLoanBondToRegister

        #region ApplicationCreateLegalEntityMinimum

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationCreateLegalEntityMinimumNoArgumentsPassed()
        {
            ApplicationCreateLegalEntityMinimum rule = new ApplicationCreateLegalEntityMinimum();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationCreateLegalEntityMinimumIncorrectArgumentsPassed()
        {
            ApplicationCreateLegalEntityMinimum rule = new ApplicationCreateLegalEntityMinimum();

            // Setup an incorrect Argumnt to pass along
            IApplicationAttributeType application = _mockery.StrictMock<IApplicationAttributeType>();

            ExecuteRule(rule, 0, application);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCreateLegalEntityMinimumSuccess()
        {
            // Setup a correct Argumnt to pass along
            // setup supportsVariableLoanApplicationInformation.VariableLoanInformation.HouseholdIncome
            // SetupResult.For(applicationMortgageLoan.NumApplicants).Return(2);

            ApplicationCreateLegalEntityMinimum rule = new ApplicationCreateLegalEntityMinimum();
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationRole appRoleLMA = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleTypeLMA = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleTypeLMA.Key).Return((int)OfferRoleTypes.LeadMainApplicant);
            SetupResult.For(appRoleLMA.ApplicationRoleType).Return(roleTypeLMA);

            //
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(appRoleLMA.GeneralStatus).Return(generalStatus);

            //
            Stack<IApplicationRole> stackAppRole = new Stack<IApplicationRole>();
            stackAppRole.Push(appRoleLMA);
            IReadOnlyEventList<IApplicationRole> appRoleList = new ReadOnlyEventList<IApplicationRole>(stackAppRole);
            SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(appRoleList);
            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCreateLegalEntityMinimumFail()
        {
            //  Setup a correct Argumnt to pass along
            //  IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            //  setup supportsVariableLoanApplicationInformation.VariableLoanInformation.HouseholdIncome
            //  SetupResult.For(applicationMortgageLoan.NumApplicants).Return(0);

            ApplicationCreateLegalEntityMinimum rule = new ApplicationCreateLegalEntityMinimum();
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationRole appRoleS = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleTypeLS = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleTypeLS.Key).Return((int)OfferRoleTypes.Suretor);
            SetupResult.For(appRoleS.ApplicationRoleType).Return(roleTypeLS);

            //
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(appRoleS.GeneralStatus).Return(generalStatus);

            //
            Stack<IApplicationRole> stackAppRole = new Stack<IApplicationRole>();
            stackAppRole.Push(appRoleS);
            IReadOnlyEventList<IApplicationRole> appRoleList = new ReadOnlyEventList<IApplicationRole>(stackAppRole);
            SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(appRoleList);
            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        #endregion ApplicationCreateLegalEntityMinimum

        #region ApplicationMailingAddress

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationMailingAddressNoArgumentsPassed()
        {
            SAHL.Common.BusinessModel.Rules.Products.ApplicationMailingAddress rule = new SAHL.Common.BusinessModel.Rules.Products.ApplicationMailingAddress();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationMailingAddressIncorrectArgumentsPassed()
        {
            SAHL.Common.BusinessModel.Rules.Products.ApplicationMailingAddress rule = new SAHL.Common.BusinessModel.Rules.Products.ApplicationMailingAddress();

            // Setup an incorrect Argumnt to pass along
            IApplicationMailingAddress application = _mockery.StrictMock<IApplicationMailingAddress>();

            ExecuteRule(rule, 0, application);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMailingAddressLeadMainApplicantSuccess()
        {
            SAHL.Common.BusinessModel.Rules.Products.ApplicationMailingAddress rule = new SAHL.Common.BusinessModel.Rules.Products.ApplicationMailingAddress();

            // Setup a correct Argumnt to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(applicationMortgageLoan.Key).Return(1);

            // Setup applicationMortgageLoan.IApplicationMailingAddresses
            IEventList<IApplicationMailingAddress> applicationMailingAddresses = new EventList<IApplicationMailingAddress>();
            SetupResult.For(applicationMortgageLoan.ApplicationMailingAddresses).Return(applicationMailingAddresses);

            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            SetupResult.For(applicationMortgageLoan.ApplicationType).Return(appType);
            SetupResult.For(appType.Key).Return((int)OfferTypes.RefinanceLoan);

            // Setup IApplicationMailingAddress[0] (IApplicationMailingAddress)
            IApplicationMailingAddress applicationMailingAddress = _mockery.StrictMock<IApplicationMailingAddress>();
            applicationMailingAddresses.Add(Messages, applicationMailingAddress);

            // Setup IApplicationMailingAddress.Address
            IAddressBox address = _mockery.StrictMock<IAddressBox>();
            SetupResult.For(applicationMailingAddress.Address).Return(address);

            // Setup Address.Key
            SetupResult.For(address.Key).Return(1);

            // Setup IApplication.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> applicationRoleStack = new Stack<IApplicationRole>();
            applicationRoleStack.Push(applicationRole);
            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationRoleStack);
            SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(applicationRoles);

            // Setup IApplicationRole.ApplicationRoleType
            IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(applicationRoleType.Key).Return((int)OfferRoleTypes.LeadMainApplicant);
            SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);

            // Setup IApplicationRole.GeneralStatus
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(applicationRole.GeneralStatus).Return(generalStatus);

            // Setup applicationRole.LegalEntity
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(applicationRole.LegalEntity).Return(legalEntity);

            // Setup legalEntity.LegalEntityAddresses
            IEventList<ILegalEntityAddress> legalEntityAddresses = new EventList<ILegalEntityAddress>();
            SetupResult.For(legalEntity.LegalEntityAddresses).Return(legalEntityAddresses);
            ILegalEntityAddress legalEntityAddress = _mockery.StrictMock<ILegalEntityAddress>();
            legalEntityAddresses.Add(Messages, legalEntityAddress);

            // Setup legalEntityAddress.Address
            SetupResult.For(legalEntityAddress.Address).Return(address);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        [NUnit.Framework.Test]
        public void ApplicationMailingAddressMainApplicantSuccess()
        {
            SAHL.Common.BusinessModel.Rules.Products.ApplicationMailingAddress rule = new SAHL.Common.BusinessModel.Rules.Products.ApplicationMailingAddress();

            // Setup a correct Argumnt to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(applicationMortgageLoan.Key).Return(1);

            // Setup applicationMortgageLoan.IApplicationMailingAddresses
            IEventList<IApplicationMailingAddress> applicationMailingAddresses = new EventList<IApplicationMailingAddress>();
            SetupResult.For(applicationMortgageLoan.ApplicationMailingAddresses).Return(applicationMailingAddresses);

            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            SetupResult.For(applicationMortgageLoan.ApplicationType).Return(appType);
            SetupResult.For(appType.Key).Return((int)OfferTypes.RefinanceLoan);

            // Setup IApplicationMailingAddress[0] (IApplicationMailingAddress)
            IApplicationMailingAddress applicationMailingAddress = _mockery.StrictMock<IApplicationMailingAddress>();
            applicationMailingAddresses.Add(Messages, applicationMailingAddress);

            // Setup IApplicationMailingAddress.Address
            IAddressBox address = _mockery.StrictMock<IAddressBox>();
            SetupResult.For(applicationMailingAddress.Address).Return(address);

            // Setup Address.Key
            SetupResult.For(address.Key).Return(1);

            // Setup IApplication.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> applicationRoleStack = new Stack<IApplicationRole>();
            applicationRoleStack.Push(applicationRole);
            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationRoleStack);
            SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(applicationRoles);

            // Setup IApplicationRole.ApplicationRoleType
            IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(applicationRoleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);

            // Setup IApplicationRole.GeneralStatus
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(applicationRole.GeneralStatus).Return(generalStatus);

            // Setup applicationRole.LegalEntity
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(applicationRole.LegalEntity).Return(legalEntity);

            // Setup legalEntity.LegalEntityAddresses
            IEventList<ILegalEntityAddress> legalEntityAddresses = new EventList<ILegalEntityAddress>();
            SetupResult.For(legalEntity.LegalEntityAddresses).Return(legalEntityAddresses);
            ILegalEntityAddress legalEntityAddress = _mockery.StrictMock<ILegalEntityAddress>();
            legalEntityAddresses.Add(Messages, legalEntityAddress);

            // Setup legalEntityAddress.Address
            SetupResult.For(legalEntityAddress.Address).Return(address);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        [NUnit.Framework.Test]
        public void ApplicationMailingAddressFurtherLendingTestPass()
        {
            SAHL.Common.BusinessModel.Rules.Products.ApplicationMailingAddress rule = new SAHL.Common.BusinessModel.Rules.Products.ApplicationMailingAddress();

            // Setup a correct Argumnt to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(applicationMortgageLoan.Key).Return(1);

            // Setup applicationMortgageLoan.IApplicationMailingAddresses
            IEventList<IApplicationMailingAddress> applicationMailingAddresses = new EventList<IApplicationMailingAddress>();
            SetupResult.For(applicationMortgageLoan.ApplicationMailingAddresses).Return(applicationMailingAddresses);

            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            SetupResult.For(applicationMortgageLoan.ApplicationType).Return(appType);
            SetupResult.For(appType.Key).Return((int)OfferTypes.FurtherAdvance);

            // Setup IApplicationMailingAddress[0] (IApplicationMailingAddress)
            IApplicationMailingAddress applicationMailingAddress = _mockery.StrictMock<IApplicationMailingAddress>();
            applicationMailingAddresses.Add(Messages, applicationMailingAddress);

            // Setup IApplicationMailingAddress.Address
            IAddressBox address = _mockery.StrictMock<IAddressBox>();
            SetupResult.For(applicationMailingAddress.Address).Return(address);

            // Setup Address.Key
            SetupResult.For(address.Key).Return(1);

            // Setup IApplication.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> applicationRoleStack = new Stack<IApplicationRole>();
            applicationRoleStack.Push(applicationRole);
            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationRoleStack);
            SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(applicationRoles);

            // Setup IApplicationRole.ApplicationRoleType
            IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(applicationRoleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);

            // Setup IApplicationRole.GeneralStatus
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(applicationRole.GeneralStatus).Return(generalStatus);

            // Setup applicationRole.LegalEntity
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(applicationRole.LegalEntity).Return(legalEntity);

            // Setup legalEntity.LegalEntityAddresses
            IEventList<ILegalEntityAddress> legalEntityAddresses = new EventList<ILegalEntityAddress>();
            SetupResult.For(legalEntity.LegalEntityAddresses).Return(legalEntityAddresses);
            ILegalEntityAddress legalEntityAddress = _mockery.StrictMock<ILegalEntityAddress>();
            legalEntityAddresses.Add(Messages, legalEntityAddress);

            // Setup legalEntityAddress.Address
            SetupResult.For(legalEntityAddress.Address).Return(address);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count <> 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMailingAddressFail()
        {
            SAHL.Common.BusinessModel.Rules.Products.ApplicationMailingAddress rule = new SAHL.Common.BusinessModel.Rules.Products.ApplicationMailingAddress();

            // Setup a correct Argumnt to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(applicationMortgageLoan.Key).Return(1);

            // Setup applicationMortgageLoan.IApplicationMailingAddresses
            IEventList<IApplicationMailingAddress> applicationMailingAddresses = new EventList<IApplicationMailingAddress>();
            SetupResult.For(applicationMortgageLoan.ApplicationMailingAddresses).Return(applicationMailingAddresses);

            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            SetupResult.For(applicationMortgageLoan.ApplicationType).Return(appType);
            SetupResult.For(appType.Key).Return((int)OfferTypes.RefinanceLoan);

            // Setup IApplicationMailingAddress[0] (IApplicationMailingAddress)
            IApplicationMailingAddress applicationMailingAddress = _mockery.StrictMock<IApplicationMailingAddress>();
            applicationMailingAddresses.Add(Messages, applicationMailingAddress);

            // Setup IApplicationMailingAddress.Address
            IAddressBox address = _mockery.StrictMock<IAddressBox>();
            SetupResult.For(applicationMailingAddress.Address).Return(address);

            // Setup Address.Key
            SetupResult.For(address.Key).Return(1);

            // Setup IApplication.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> applicationRoleStack = new Stack<IApplicationRole>();
            applicationRoleStack.Push(applicationRole);
            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationRoleStack);
            SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(applicationRoles);

            // Setup IApplicationRole.ApplicationRoleType
            IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(applicationRoleType.Key).Return((int)OfferRoleTypes.LeadMainApplicant);
            SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);

            // Setup IApplicationRole.GeneralStatus
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(applicationRole.GeneralStatus).Return(generalStatus);

            // Setup applicationRole.LegalEntity
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(applicationRole.LegalEntity).Return(legalEntity);

            // Setup legalEntity.LegalEntityAddresses
            IEventList<ILegalEntityAddress> legalEntityAddresses = new EventList<ILegalEntityAddress>();
            SetupResult.For(legalEntity.LegalEntityAddresses).Return(legalEntityAddresses);
            ILegalEntityAddress legalEntityAddress = _mockery.StrictMock<ILegalEntityAddress>();
            legalEntityAddresses.Add(Messages, legalEntityAddress);

            // Setup legalEntityAddress.Address (Make it different to the Application one)
            IAddress addressLE = _mockery.StrictMock<IAddress>();
            SetupResult.For(addressLE.Key).Return(-1);
            SetupResult.For(legalEntityAddress.Address).Return(addressLE);

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        #endregion ApplicationMailingAddress

        #region ApplicationMortgageLoanExistingLoanAmount

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationMortgageLoanExistingLoanAmountNoArgumentsPassed()
        {
            ApplicationMortgageLoanExistingLoanAmount rule = new ApplicationMortgageLoanExistingLoanAmount();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ApplicationMortgageLoanExistingLoanAmountIncorrectArgumentsPassed()
        {
            ApplicationMortgageLoanExistingLoanAmount rule = new ApplicationMortgageLoanExistingLoanAmount();

            // Setup an incorrect Argumnt to pass along
            IApplication application = _mockery.StrictMock<IApplication>();

            ExecuteRule(rule, 0, application);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMortgageLoanExistingLoanAmountSuccess()
        {
            ApplicationMortgageLoanExistingLoanAmount rule = new ApplicationMortgageLoanExistingLoanAmount();

            // Setup the correct object to pass along
            IApplicationMortgageLoanSwitch applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoanSwitch>();

            // Setup applicationMortgageLoan.CurrentProduct
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = _mockery.StrictMock<ISupportsVariableLoanApplicationInformation>();
            IApplicationProductNewVariableLoan applicationProductNewVariableLoan = _mockery.StrictMock<IApplicationProductNewVariableLoan>();
            SetupResult.For(applicationMortgageLoan.CurrentProduct).Return(applicationProductNewVariableLoan);

            // Setup applicationProductNewVariableLoan.VariableLoanInformation
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductNewVariableLoan.VariableLoanInformation).Return(applicationInformationVariableLoan);

            // setup supportsVariableLoanApplicationInformation.VariableLoanInformation.HouseholdIncome
            SetupResult.For(applicationInformationVariableLoan.ExistingLoan).Return(500000.0);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMortgageLoanExistingLoanAmountFail()
        {
            ApplicationMortgageLoanExistingLoanAmount rule = new ApplicationMortgageLoanExistingLoanAmount();

            // Setup the correct object to pass along
            IApplicationMortgageLoanSwitch applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoanSwitch>();

            // Setup applicationMortgageLoan.CurrentProduct
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = _mockery.StrictMock<ISupportsVariableLoanApplicationInformation>();
            IApplicationProductNewVariableLoan applicationProductNewVariableLoan = _mockery.StrictMock<IApplicationProductNewVariableLoan>();
            SetupResult.For(applicationMortgageLoan.CurrentProduct).Return(applicationProductNewVariableLoan);

            // Setup applicationProductNewVariableLoan.VariableLoanInformation
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductNewVariableLoan.VariableLoanInformation).Return(applicationInformationVariableLoan);

            // setup supportsVariableLoanApplicationInformation.VariableLoanInformation.HouseholdIncome
            SetupResult.For(applicationInformationVariableLoan.ExistingLoan).Return(null);

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        #endregion ApplicationMortgageLoanExistingLoanAmount

        #region MortgageLoanMultipleApplication

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void MortgageLoanMultipleApplicationNoArgumentsPassed()
        {
            MortgageLoanMultipleApplication rule = new MortgageLoanMultipleApplication();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void MortgageLoanMultipleApplicationIncorrectArgumentsPassed()
        {
            MortgageLoanMultipleApplication rule = new MortgageLoanMultipleApplication();

            // Setup an incorrect Argumnt to pass along
            IApplicationMailingAddress application = _mockery.StrictMock<IApplicationMailingAddress>();

            ExecuteRule(rule, 0, application);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanMultipleApplicationSuccess()
        {
            MortgageLoanMultipleApplication rule = new MortgageLoanMultipleApplication();

            // Setup the correct object to pass along
            IApplicationMortgageLoanSwitch applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoanSwitch>();

            // Setup applicationMortgageLoan.Key
            SetupResult.For(applicationMortgageLoan.Key).Return(1);

            // Setup IApplicationMortgageLoan.Account
            IAccount account = _mockery.StrictMock<IAccount>();
            SetupResult.For(applicationMortgageLoan.Account).Return(account);

            // Setup account.Applications
            IEventList<IApplication> applications = new EventList<IApplication>();
            IApplicationMortgageLoanSwitch application = _mockery.StrictMock<IApplicationMortgageLoanSwitch>();
            SetupResult.For(application.Key).Return(1);
            applications.Add(Messages, application);
            SetupResult.For(account.Applications).Return(applications);

            // Setup application.ApplicationStatus
            IApplicationStatus applicationStatus = _mockery.StrictMock<IApplicationStatus>();
            SetupResult.For(applicationStatus.Key).Return((int)OfferStatuses.Open);
            SetupResult.For(application.ApplicationStatus).Return(applicationStatus);
            SetupResult.For(applicationMortgageLoan.ApplicationStatus).Return(applicationStatus);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanMultipleApplicationFail()
        {
            MortgageLoanMultipleApplication rule = new MortgageLoanMultipleApplication();

            // Setup the correct object to pass along
            IApplicationMortgageLoanSwitch applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoanSwitch>();

            // Setup applicationMortgageLoan.Key
            SetupResult.For(applicationMortgageLoan.Key).Return(1);

            // Setup IApplicationMortgageLoan.Account
            IAccount account = _mockery.StrictMock<IAccount>();
            SetupResult.For(applicationMortgageLoan.Account).Return(account);

            // Setup account.Applications
            IEventList<IApplication> applications = new EventList<IApplication>();
            IApplicationMortgageLoanSwitch application = _mockery.StrictMock<IApplicationMortgageLoanSwitch>();
            SetupResult.For(application.Key).Return(-1);
            applications.Add(Messages, application);
            SetupResult.For(account.Applications).Return(applications);

            // Setup application.ApplicationStatus
            IApplicationStatus applicationStatus = _mockery.StrictMock<IApplicationStatus>();
            SetupResult.For(applicationStatus.Key).Return((int)OfferStatuses.Open);
            SetupResult.For(application.ApplicationStatus).Return(applicationStatus);
            SetupResult.For(applicationMortgageLoan.ApplicationStatus).Return(applicationStatus);

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        #endregion MortgageLoanMultipleApplication

        #region MortgageLoanLegalEntityBankAccount

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void MortgageLoanLegalEntityBankAccountNoArgumentsPassed()
        {
            MortgageLoanLegalEntityBankAccount rule = new MortgageLoanLegalEntityBankAccount();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void MortgageLoanLegalEntityBankAccountIncorrectArgumentsPassed()
        {
            MortgageLoanLegalEntityBankAccount rule = new MortgageLoanLegalEntityBankAccount();

            // Setup an incorrect Argumnt to pass along
            IApplicationMailingAddress application = _mockery.StrictMock<IApplicationMailingAddress>();

            ExecuteRule(rule, 0, application);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanLegalEntityBankAccountSuccess()
        {
            MortgageLoanLegalEntityBankAccount rule = new MortgageLoanLegalEntityBankAccount();

            // Setup the correct object to pass along
            IApplicationMortgageLoanSwitch applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoanSwitch>();
            SetupResult.For(applicationMortgageLoan.Key).Return(1);

            // Setup applicationMortgageLoan.ApplicationDebitOrders
            IEventList<IApplicationDebitOrder> applicationDebitOrders = new EventList<IApplicationDebitOrder>();
            IApplicationDebitOrder applicationDebitOrder = _mockery.StrictMock<IApplicationDebitOrder>();
            applicationDebitOrders.Add(Messages, applicationDebitOrder);
            SetupResult.For(applicationMortgageLoan.ApplicationDebitOrders).Return(applicationDebitOrders);

            // Setup applicationDebitOrder.BankAccount.Key
            IBankAccount bankAccount = _mockery.StrictMock<IBankAccount>();
            SetupResult.For(applicationDebitOrder.BankAccount).Return(bankAccount);
            SetupResult.For(bankAccount.Key).Return(2);

            // Setup IApplication.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> applicationRoleStack = new Stack<IApplicationRole>();
            applicationRoleStack.Push(applicationRole);
            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationRoleStack);
            SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(applicationRoles);

            // Setup applicationRole.LegalEntity
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(applicationRole.LegalEntity).Return(legalEntity);

            // Setup legalEntity.LegalEntityBankAccounts
            ILegalEntityBankAccount legalEntityBankAccount = _mockery.StrictMock<ILegalEntityBankAccount>();
            IEventList<ILegalEntityBankAccount> legalEntityBankAccounts = new EventList<ILegalEntityBankAccount>();
            legalEntityBankAccounts.Add(Messages, legalEntityBankAccount);
            SetupResult.For(legalEntity.LegalEntityBankAccounts).Return(legalEntityBankAccounts);

            // Setup legalEntityBankAccount.BankAccount.Key
            SetupResult.For(legalEntityBankAccount.BankAccount).Return(bankAccount);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanLegalEntityBankAccountFail()
        {
            MortgageLoanLegalEntityBankAccount rule = new MortgageLoanLegalEntityBankAccount();

            // Setup the correct object to pass along
            IApplicationMortgageLoanSwitch applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoanSwitch>();
            SetupResult.For(applicationMortgageLoan.Key).Return(1);

            // Setup applicationMortgageLoan.ApplicationDebitOrders
            IApplicationDebitOrder applicationDebitOrder = _mockery.StrictMock<IApplicationDebitOrder>();
            IEventList<IApplicationDebitOrder> applicationDebitOrders = new EventList<IApplicationDebitOrder>();
            applicationDebitOrders.Add(Messages, applicationDebitOrder);
            SetupResult.For(applicationMortgageLoan.ApplicationDebitOrders).Return(applicationDebitOrders);

            // Setup applicationDebitOrder.BankAccount.Key
            IBankAccount bankAccount = _mockery.StrictMock<IBankAccount>();
            SetupResult.For(applicationDebitOrder.BankAccount).Return(bankAccount);
            SetupResult.For(bankAccount.Key).Return(2);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>();
            SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(applicationRoles);

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        #endregion MortgageLoanLegalEntityBankAccount

        #region ApplicationMortgageLoanNewPurchaseNewLightstoneValuationRequired

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMortgageLoanNewPurchaseNewLightstoneValuationRequiredSuccess()
        {
            ApplicationMortgageLoanNewPurchaseNewLightstoneValuationRequired rule = new ApplicationMortgageLoanNewPurchaseNewLightstoneValuationRequired();

            // Setup the correct object to pass along
            IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = _mockery.StrictMock<IApplicationMortgageLoanNewPurchase>();
            SetupResult.For(applicationMortgageLoanNewPurchase.Key).Return(1);

            // Setup applicationMortgageLoanNewPurchase.Property
            IProperty property = _mockery.StrictMock<IProperty>();
            SetupResult.For(applicationMortgageLoanNewPurchase.Property).Return(property);

            // Property.Valuations
            IValuationDiscriminatedLightstoneAVM valuation = _mockery.StrictMock<IValuationDiscriminatedLightstoneAVM>();
            IEventList<IValuation> valuations = new EventList<IValuation>();
            valuations.Add(Messages, valuation);
            SetupResult.For(property.Valuations).Return(valuations);

            // valuation.ValuationDateSetp valuation.ValuationDate
            SetupResult.For(valuation.ValuationDate).Return(DateTime.Today.AddMonths(-11)); // Under 12 months shoudl succeed

            ExecuteRule(rule, 0, applicationMortgageLoanNewPurchase);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMortgageLoanNewPurchaseNewLightstoneValuationRequiredFail()
        {
            ApplicationMortgageLoanNewPurchaseNewLightstoneValuationRequired rule = new ApplicationMortgageLoanNewPurchaseNewLightstoneValuationRequired();

            // Setup the correct object to pass along
            IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = _mockery.StrictMock<IApplicationMortgageLoanNewPurchase>();
            SetupResult.For(applicationMortgageLoanNewPurchase.Key).Return(1);

            // Setup applicationMortgageLoanNewPurchase.Property
            IProperty property = _mockery.StrictMock<IProperty>();
            SetupResult.For(applicationMortgageLoanNewPurchase.Property).Return(property);

            // Property.Valuations
            IValuationDiscriminatedLightstoneAVM valuation = _mockery.StrictMock<IValuationDiscriminatedLightstoneAVM>();
            IEventList<IValuation> valuations = new EventList<IValuation>();
            valuations.Add(Messages, valuation);
            SetupResult.For(property.Valuations).Return(valuations);

            // valuation.ValuationDateSetp valuation.ValuationDate
            SetupResult.For(valuation.ValuationDate).Return(DateTime.Today.AddMonths(-13)); // Under 12 months shoudl succeed

            ExecuteRule(rule, 1, applicationMortgageLoanNewPurchase);
        }

        #endregion ApplicationMortgageLoanNewPurchaseNewLightstoneValuationRequired

        #region ApplicationMortgageLoanSwitchNewLightstoneValuationRequired

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMortgageLoanSwitchNewLightstoneValuationRequiredSuccess()
        {
            ApplicationMortgageLoanSwitchNewLightstoneValuationRequired rule = new ApplicationMortgageLoanSwitchNewLightstoneValuationRequired();

            // Setup the correct object to pass along
            IApplicationMortgageLoanSwitch applicationMortgageLoanSwitch = _mockery.StrictMock<IApplicationMortgageLoanSwitch>();
            SetupResult.For(applicationMortgageLoanSwitch.Key).Return(1);

            // Setup applicationMortgageLoanSwitch.Property
            IProperty property = _mockery.StrictMock<IProperty>();
            SetupResult.For(applicationMortgageLoanSwitch.Property).Return(property);

            // Property.Valuations
            IValuationDiscriminatedLightstoneAVM valuation = _mockery.StrictMock<IValuationDiscriminatedLightstoneAVM>();
            IEventList<IValuation> valuations = new EventList<IValuation>();
            valuations.Add(Messages, valuation);
            SetupResult.For(property.Valuations).Return(valuations);

            // valuation.ValuationDateSetp valuation.ValuationDate
            SetupResult.For(valuation.ValuationDate).Return(DateTime.Today.AddMonths(-13)); // Under 12 months should fail

            ExecuteRule(rule, 1, applicationMortgageLoanSwitch);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMortgageLoanSwitchNewLightstoneValuationRequiredFail()
        {
            ApplicationMortgageLoanSwitchNewLightstoneValuationRequired rule = new ApplicationMortgageLoanSwitchNewLightstoneValuationRequired();

            // Setup the correct object to pass along
            IApplicationMortgageLoanSwitch applicationMortgageLoanSwitch = _mockery.StrictMock<IApplicationMortgageLoanSwitch>();
            SetupResult.For(applicationMortgageLoanSwitch.Key).Return(1);

            // Setup applicationMortgageLoanSwitch.Property
            IProperty property = _mockery.StrictMock<IProperty>();
            SetupResult.For(applicationMortgageLoanSwitch.Property).Return(property);

            // Property.Valuations
            IValuationDiscriminatedLightstoneAVM valuation = _mockery.StrictMock<IValuationDiscriminatedLightstoneAVM>();
            IEventList<IValuation> valuations = new EventList<IValuation>();
            valuations.Add(Messages, valuation);
            SetupResult.For(property.Valuations).Return(valuations);

            // valuation.ValuationDateSetp valuation.ValuationDate
            SetupResult.For(valuation.ValuationDate).Return(DateTime.Today.AddMonths(-13)); // Under 12 months shoudl succeed

            ExecuteRule(rule, 1, applicationMortgageLoanSwitch);
        }

        #endregion ApplicationMortgageLoanSwitchNewLightstoneValuationRequired

        #region ApplicationMortgageLoanNewPurchaseNewAdCheckValuationRequired

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMortgageLoanNewPurchaseNewAdCheckValuationRequiredSuccess()
        {
            ApplicationMortgageLoanNewPurchaseNewAdCheckValuationRequired rule = new ApplicationMortgageLoanNewPurchaseNewAdCheckValuationRequired();

            // Setup the correct object to pass along
            IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = _mockery.StrictMock<IApplicationMortgageLoanNewPurchase>();
            SetupResult.For(applicationMortgageLoanNewPurchase.Key).Return(1);

            // Setup applicationMortgageLoanNewPurchase.Property
            IProperty property = _mockery.StrictMock<IProperty>();
            SetupResult.For(applicationMortgageLoanNewPurchase.Property).Return(property);

            // Property.Valuations
            IValuationDiscriminatedAdCheckPhysical valuation = _mockery.StrictMock<IValuationDiscriminatedAdCheckPhysical>();
            IEventList<IValuation> valuations = new EventList<IValuation>();
            valuations.Add(Messages, valuation);
            SetupResult.For(property.Valuations).Return(valuations);

            // valuation.ValuationDateSetp valuation.ValuationDate
            SetupResult.For(valuation.ValuationDate).Return(DateTime.Today.AddMonths(-11)); // Under 12 months shoudl succeed

            ExecuteRule(rule, 0, applicationMortgageLoanNewPurchase);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMortgageLoanNewPurchaseNewAdCheckValuationRequiredFail()
        {
            ApplicationMortgageLoanNewPurchaseNewAdCheckValuationRequired rule = new ApplicationMortgageLoanNewPurchaseNewAdCheckValuationRequired();

            // Setup the correct object to pass along
            IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = _mockery.StrictMock<IApplicationMortgageLoanNewPurchase>();
            SetupResult.For(applicationMortgageLoanNewPurchase.Key).Return(1);

            // Setup applicationMortgageLoanNewPurchase.Property
            IProperty property = _mockery.StrictMock<IProperty>();
            SetupResult.For(applicationMortgageLoanNewPurchase.Property).Return(property);

            // Property.Valuations
            IValuationDiscriminatedAdCheckPhysical valuation = _mockery.StrictMock<IValuationDiscriminatedAdCheckPhysical>();
            IEventList<IValuation> valuations = new EventList<IValuation>();
            valuations.Add(Messages, valuation);
            SetupResult.For(property.Valuations).Return(valuations);

            // valuation.ValuationDateSetp valuation.ValuationDate
            SetupResult.For(valuation.ValuationDate).Return(DateTime.Today.AddMonths(-13)); // Over 12 months shoudl faile

            ExecuteRule(rule, 1, applicationMortgageLoanNewPurchase);
        }

        #endregion ApplicationMortgageLoanNewPurchaseNewAdCheckValuationRequired

        #region ApplicationMortgageLoanSwitchNewAdCheckValuationRequired

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMortgageLoanSwitchNewAdCheckValuationRequiredSuccess()
        {
            ApplicationMortgageLoanSwitchNewAdCheckValuationRequired rule = new ApplicationMortgageLoanSwitchNewAdCheckValuationRequired();

            // Setup the correct object to pass along
            IApplicationMortgageLoanSwitch applicationMortgageLoanSwitch = _mockery.StrictMock<IApplicationMortgageLoanSwitch>();
            SetupResult.For(applicationMortgageLoanSwitch.Key).Return(1);

            // Setup applicationMortgageLoanSwitch.Property
            IProperty property = _mockery.StrictMock<IProperty>();
            SetupResult.For(applicationMortgageLoanSwitch.Property).Return(property);

            // Property.Valuations
            IValuationDiscriminatedAdCheckPhysical valuation = _mockery.StrictMock<IValuationDiscriminatedAdCheckPhysical>();
            IEventList<IValuation> valuations = new EventList<IValuation>();
            valuations.Add(Messages, valuation);
            SetupResult.For(property.Valuations).Return(valuations);

            // valuation.ValuationDateSetp valuation.ValuationDate
            SetupResult.For(valuation.ValuationDate).Return(DateTime.Today.AddMonths(-11)); // Under 12 months should fail

            ExecuteRule(rule, 0, applicationMortgageLoanSwitch);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationMortgageLoanSwitchNewAdCheckValuationRequiredFail()
        {
            ApplicationMortgageLoanSwitchNewAdCheckValuationRequired rule = new ApplicationMortgageLoanSwitchNewAdCheckValuationRequired();

            // Setup the correct object to pass along
            IApplicationMortgageLoanSwitch applicationMortgageLoanSwitch = _mockery.StrictMock<IApplicationMortgageLoanSwitch>();
            SetupResult.For(applicationMortgageLoanSwitch.Key).Return(1);

            // Setup applicationMortgageLoanSwitch.Property
            IProperty property = _mockery.StrictMock<IProperty>();
            SetupResult.For(applicationMortgageLoanSwitch.Property).Return(property);

            // Property.Valuations
            IValuationDiscriminatedAdCheckPhysical valuation = _mockery.StrictMock<IValuationDiscriminatedAdCheckPhysical>();
            IEventList<IValuation> valuations = new EventList<IValuation>();
            valuations.Add(Messages, valuation);
            SetupResult.For(property.Valuations).Return(valuations);

            // valuation.ValuationDateSetp valuation.ValuationDate
            SetupResult.For(valuation.ValuationDate).Return(DateTime.Today.AddMonths(-13)); // Over 12 months shoudl fail

            ExecuteRule(rule, 1, applicationMortgageLoanSwitch);
        }

        #endregion ApplicationMortgageLoanSwitchNewAdCheckValuationRequired

        #region QuickCashCreditApproveAmount

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void QuickCashCreditApproveAmountSuccess()
        {
            QuickCashCreditApproveAmount rule = new QuickCashCreditApproveAmount();

            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);

            // Setup the correct object to pass along
            IApplicationInformationQuickCash applicationInformationQuickCash = _mockery.StrictMock<IApplicationInformationQuickCash>();

            // Setup ILookupRepository
            ILookupRepository lookupRepository = _mockery.StrictMock<ILookupRepository>();
            MockCache.Add((typeof(ILookupRepository)).ToString(), lookupRepository);

            // Setup IReasonRepository
            IReasonRepository reasonRepository = _mockery.StrictMock<IReasonRepository>();
            MockCache.Add((typeof(IReasonRepository)).ToString(), reasonRepository);

            IApplicationInformation appInfo = _mockery.StrictMock<IApplicationInformation>();
            SetupResult.For(appInfo.Key).Return(1);
            SetupResult.For(applicationInformationQuickCash.ApplicationInformation).Return(appInfo);
            IGenericKeyType genKeyType = _mockery.StrictMock<IGenericKeyType>();
            SetupResult.For(genKeyType.Key).Return(5);

            IReasonType reasonType = _mockery.StrictMock<IReasonType>();
            SetupResult.For(reasonType.Key).Return((int)ReasonTypes.QuickCashDecline);
            SetupResult.For(reasonType.GenericKeyType).Return(genKeyType);
            SetupResult.For(reasonRepository.GetReasonTypeByKey((int)ReasonTypes.QuickCashDecline)).IgnoreArguments().Return(reasonType);

            //IReason reason1 = _mockery.StrictMock<IReason>();
            //SetupResult.For(reason1.Key).Return(1);
            IReason reason = _mockery.StrictMock<IReason>();
            IReasonDefinition reasonDef = _mockery.StrictMock<IReasonDefinition>();
            SetupResult.For(reasonDef.ReasonType).Return(reasonType);
            SetupResult.For(reason.ReasonDefinition).Return(reasonDef);

            IList<IReason> reasons = new List<IReason>();

            //reasons.Add(reason);
            // no reasons for success...count must be zero.
            IReadOnlyEventList<IReason> list = new ReadOnlyEventList<IReason>(reasons);

            SetupResult.For(reasonRepository.GetReasonByGenericKeyAndReasonTypeKey(1, 1)).IgnoreArguments().Return(list);

            // Setup lookupRepository.Controls.ObjectDictionary
            IEventList<IControl> controls = _mockery.StrictMock<IEventList<IControl>>();
            IDictionary<string, IControl> objectDictionary = new Dictionary<string, IControl>();
            SetupResult.For(controls.ObjectDictionary).Return(objectDictionary);
            SetupResult.For(lookupRepository.Controls).Return(controls);

            IControl control = _mockery.StrictMock<IControl>();
            objectDictionary.Add(new KeyValuePair<string, IControl>(SAHL.Common.Constants.ControlTable.QuickCash.MinimumQuickCash, control));

            // Setup control.ControlNumeric
            SetupResult.For(control.ControlNumeric).Return(1000.0);

            // applicationInformationQuickCash.CreditApprovedAmount
            SetupResult.For(applicationInformationQuickCash.CreditApprovedAmount).Return(5000.0);

            // applicationInformationQuickCash.GetMaximumQuickCash()
            SetupResult.For(applicationInformationQuickCash.GetMaximumQuickCash()).Return(200000.0);

            ExecuteRule(rule, 0, applicationInformationQuickCash);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void QuickCashCreditApproveAmountFail()
        {
            QuickCashCreditApproveAmount rule = new QuickCashCreditApproveAmount();

            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);

            // Setup the correct object to pass along
            IApplicationInformationQuickCash applicationInformationQuickCash = _mockery.StrictMock<IApplicationInformationQuickCash>();

            // Setup ILookupRepository
            ILookupRepository lookupRepository = _mockery.StrictMock<ILookupRepository>();
            MockCache.Add((typeof(ILookupRepository)).ToString(), lookupRepository);

            // Setup IReasonRepository
            IReasonRepository reasonRepository = _mockery.StrictMock<IReasonRepository>();
            MockCache.Add((typeof(IReasonRepository)).ToString(), reasonRepository);

            IApplicationInformation appInfo = _mockery.StrictMock<IApplicationInformation>();
            SetupResult.For(appInfo.Key).Return(1);
            SetupResult.For(applicationInformationQuickCash.ApplicationInformation).Return(appInfo);
            IGenericKeyType genKeyType = _mockery.StrictMock<IGenericKeyType>();
            SetupResult.For(genKeyType.Key).Return(5);

            IReasonType reasonType = _mockery.StrictMock<IReasonType>();
            SetupResult.For(reasonType.Key).Return((int)ReasonTypes.QuickCashDecline);
            SetupResult.For(reasonType.GenericKeyType).Return(genKeyType);
            SetupResult.For(reasonRepository.GetReasonTypeByKey((int)ReasonTypes.QuickCashDecline)).IgnoreArguments().Return(reasonType);

            //IReason reason1 = _mockery.StrictMock<IReason>();
            //SetupResult.For(reason1.Key).Return(1);
            IReason reason = _mockery.StrictMock<IReason>();
            IReasonDefinition reasonDef = _mockery.StrictMock<IReasonDefinition>();
            SetupResult.For(reasonDef.ReasonType).Return(reasonType);
            SetupResult.For(reason.ReasonDefinition).Return(reasonDef);

            IList<IReason> reasons = new List<IReason>();
            reasons.Add(reason);

            IReadOnlyEventList<IReason> list = new ReadOnlyEventList<IReason>(reasons);

            SetupResult.For(reasonRepository.GetReasonByGenericKeyAndReasonTypeKey(1, 1)).IgnoreArguments().Return(list);

            // Setup lookupRepository.Controls.ObjectDictionary
            IEventList<IControl> controls = _mockery.StrictMock<IEventList<IControl>>();
            IDictionary<string, IControl> objectDictionary = new Dictionary<string, IControl>();
            SetupResult.For(controls.ObjectDictionary).Return(objectDictionary);
            SetupResult.For(lookupRepository.Controls).Return(controls);

            IControl control = _mockery.StrictMock<IControl>();
            objectDictionary.Add(new KeyValuePair<string, IControl>(SAHL.Common.Constants.ControlTable.QuickCash.MinimumQuickCash, control));

            // Setup control.ControlNumeric
            SetupResult.For(control.ControlNumeric).Return(1000.0);

            // applicationInformationQuickCash.CreditApprovedAmount
            SetupResult.For(applicationInformationQuickCash.CreditApprovedAmount).Return(250000.0);

            // applicationInformationQuickCash.GetMaximumQuickCash()
            SetupResult.For(applicationInformationQuickCash.GetMaximumQuickCash()).Return(200000.0);

            ExecuteRule(rule, 1, applicationInformationQuickCash);
        }

        #endregion QuickCashCreditApproveAmount

        #region QuickCashUpFrontMaximum

        [NUnit.Framework.Test]
        public void QuickCashUpFrontMaximumExceededTestFail()
        {
            QuickCashUpFrontMaximum rule = new QuickCashUpFrontMaximum();

            // Setup the correct object to pass along
            IApplicationInformationQuickCash applicationInformationQuickCash = _mockery.StrictMock<IApplicationInformationQuickCash>();

            // applicationInformationQuickCash.CreditApprovedAmount
            double cashUpfront = 80000;
            SetupResult.For(applicationInformationQuickCash.CreditUpfrontApprovedAmount).Return(cashUpfront);

            ExecuteRule(rule, 1, applicationInformationQuickCash);
        }

        [NUnit.Framework.Test]
        public void QuickCashUpFrontMaximumExceededTestPass()
        {
            QuickCashUpFrontMaximum rule = new QuickCashUpFrontMaximum();

            // Setup the correct object to pass along
            IApplicationInformationQuickCash applicationInformationQuickCash = _mockery.StrictMock<IApplicationInformationQuickCash>();

            // applicationInformationQuickCash.CreditApprovedAmount
            double cashUpfront = 75000;
            SetupResult.For(applicationInformationQuickCash.CreditUpfrontApprovedAmount).Return(cashUpfront);

            ExecuteRule(rule, 0, applicationInformationQuickCash);
        }

        //-------------------------

        [NUnit.Framework.Test]
        public void QuickUpFrontLessThanApprovedAmountTest()
        {
            QuickUpFrontLessThanApprovedAmount rule = new QuickUpFrontLessThanApprovedAmount();

            // Setup the correct object to pass along
            IApplicationInformationQuickCash applicationInformationQuickCash = _mockery.StrictMock<IApplicationInformationQuickCash>();

            SetupResult.For(applicationInformationQuickCash.CreditUpfrontApprovedAmount).Return(1000D);
            SetupResult.For(applicationInformationQuickCash.CreditApprovedAmount).Return(500D);

            ExecuteRule(rule, 1, applicationInformationQuickCash);

            SetupResult.For(applicationInformationQuickCash.CreditUpfrontApprovedAmount).Return(500D);
            SetupResult.For(applicationInformationQuickCash.CreditApprovedAmount).Return(1000D);

            ExecuteRule(rule, 0, applicationInformationQuickCash);
        }

        //---------------------------

        [NUnit.Framework.Test]
        public void QuickCashUpFrontApprovalReduceNoDisbursementTestPass()
        {
            QuickCashUpFrontApprovalReduce rule = new QuickCashUpFrontApprovalReduce();

            // Setup the correct object to pass along
            IApplicationInformationQuickCash applicationInformationQuickCash = _mockery.StrictMock<IApplicationInformationQuickCash>();

            // applicationInformationQuickCash.CreditApprovedAmount
            double cashUpfront = 75000;
            SetupResult.For(applicationInformationQuickCash.CreditUpfrontApprovedAmount).Return(cashUpfront);

            IEventList<IApplicationInformationQuickCashDetail> qcDetails = new EventList<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail qcDet = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();
            SetupResult.For(qcDet.Disbursed).Return(null);
            double requestedamt = 0;
            SetupResult.For(qcDet.RequestedAmount).Return(requestedamt);
            qcDetails.Add(Messages, qcDet);

            SetupResult.For(applicationInformationQuickCash.ApplicationInformationQuickCashDetails).Return(qcDetails);

            ExecuteRule(rule, 0, applicationInformationQuickCash);
        }

        [NUnit.Framework.Test]
        public void QuickCashUpFrontApprovalReduceDisbursedTestPass()
        {
            QuickCashUpFrontApprovalReduce rule = new QuickCashUpFrontApprovalReduce();

            // Setup the correct object to pass along
            IApplicationInformationQuickCash applicationInformationQuickCash = _mockery.StrictMock<IApplicationInformationQuickCash>();

            // applicationInformationQuickCash.CreditApprovedAmount
            double cashUpfront = 75000;
            SetupResult.For(applicationInformationQuickCash.CreditUpfrontApprovedAmount).Return(cashUpfront);

            IEventList<IApplicationInformationQuickCashDetail> qcDetails = new EventList<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail qcDet = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();
            SetupResult.For(qcDet.Disbursed).Return(true);
            double requestedamt = 50000;
            SetupResult.For(qcDet.RequestedAmount).Return(requestedamt);
            IQuickCashPaymentType qcPaymentType = _mockery.StrictMock<IQuickCashPaymentType>();
            SetupResult.For(qcDet.QuickCashPaymentType).Return(qcPaymentType);
            SetupResult.For(qcPaymentType.Key).Return((int)QuickCashPaymentTypes.UpfrontPayment);
            qcDetails.Add(Messages, qcDet);

            SetupResult.For(applicationInformationQuickCash.ApplicationInformationQuickCashDetails).Return(qcDetails);

            ExecuteRule(rule, 0, applicationInformationQuickCash);
        }

        [NUnit.Framework.Test]
        public void QuickCashUpFrontApprovalReduceDisbursedTestFail()
        {
            QuickCashUpFrontApprovalReduce rule = new QuickCashUpFrontApprovalReduce();

            // Setup the correct object to pass along
            IApplicationInformationQuickCash applicationInformationQuickCash = _mockery.StrictMock<IApplicationInformationQuickCash>();

            // applicationInformationQuickCash.CreditApprovedAmount
            double cashUpfront = 45000;
            SetupResult.For(applicationInformationQuickCash.CreditUpfrontApprovedAmount).Return(cashUpfront);

            IEventList<IApplicationInformationQuickCashDetail> qcDetails = new EventList<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail qcDet = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();
            SetupResult.For(qcDet.Disbursed).Return(true);
            double requestedamt = 50000;
            SetupResult.For(qcDet.RequestedAmount).Return(requestedamt);
            IQuickCashPaymentType qcPaymentType = _mockery.StrictMock<IQuickCashPaymentType>();
            SetupResult.For(qcDet.QuickCashPaymentType).Return(qcPaymentType);
            SetupResult.For(qcPaymentType.Key).Return((int)QuickCashPaymentTypes.UpfrontPayment);
            qcDetails.Add(Messages, qcDet);

            SetupResult.For(applicationInformationQuickCash.ApplicationInformationQuickCashDetails).Return(qcDetails);

            ExecuteRule(rule, 1, applicationInformationQuickCash);
        }

        [NUnit.Framework.Test]
        public void QuickCashTotalApprovalReduceNoDisbursementTestPass()
        {
            QuickCashTotalApprovalReduce rule = new QuickCashTotalApprovalReduce();

            // Setup the correct object to pass along
            IApplicationInformationQuickCash applicationInformationQuickCash = _mockery.StrictMock<IApplicationInformationQuickCash>();

            double cashUpfront = 75000;
            SetupResult.For(applicationInformationQuickCash.CreditApprovedAmount).Return(cashUpfront);

            IEventList<IApplicationInformationQuickCashDetail> qcDetails = new EventList<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail qcDet = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();
            SetupResult.For(qcDet.Disbursed).Return(null);
            double requestedamt = 0;
            SetupResult.For(qcDet.RequestedAmount).Return(requestedamt);
            qcDetails.Add(Messages, qcDet);

            SetupResult.For(applicationInformationQuickCash.ApplicationInformationQuickCashDetails).Return(qcDetails);

            ExecuteRule(rule, 0, applicationInformationQuickCash);
        }

        [NUnit.Framework.Test]
        public void QuickCashTotalApprovalReduceDisbursedTestPass()
        {
            QuickCashTotalApprovalReduce rule = new QuickCashTotalApprovalReduce();

            // Setup the correct object to pass along
            IApplicationInformationQuickCash applicationInformationQuickCash = _mockery.StrictMock<IApplicationInformationQuickCash>();

            // applicationInformationQuickCash.CreditApprovedAmount
            double cashUpfront = 75000;
            SetupResult.For(applicationInformationQuickCash.CreditApprovedAmount).Return(cashUpfront);

            IEventList<IApplicationInformationQuickCashDetail> qcDetails = new EventList<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail qcDet = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();
            SetupResult.For(qcDet.Disbursed).Return(true);
            double requestedamt = 50000;
            SetupResult.For(qcDet.RequestedAmount).Return(requestedamt);
            qcDetails.Add(Messages, qcDet);

            SetupResult.For(applicationInformationQuickCash.ApplicationInformationQuickCashDetails).Return(qcDetails);

            ExecuteRule(rule, 0, applicationInformationQuickCash);
        }

        [NUnit.Framework.Test]
        public void QuickCashTotalApprovalReduceDisbursedTestFail()
        {
            QuickCashTotalApprovalReduce rule = new QuickCashTotalApprovalReduce();

            // Setup the correct object to pass along
            IApplicationInformationQuickCash applicationInformationQuickCash = _mockery.StrictMock<IApplicationInformationQuickCash>();

            // applicationInformationQuickCash.CreditApprovedAmount
            double cashUpfront = 45000;
            SetupResult.For(applicationInformationQuickCash.CreditApprovedAmount).Return(cashUpfront);

            IEventList<IApplicationInformationQuickCashDetail> qcDetails = new EventList<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail qcDet = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();
            SetupResult.For(qcDet.Disbursed).Return(true);
            double requestedamt = 50000;
            SetupResult.For(qcDet.RequestedAmount).Return(requestedamt);
            qcDetails.Add(Messages, qcDet);

            SetupResult.For(applicationInformationQuickCash.ApplicationInformationQuickCashDetails).Return(qcDetails);

            ExecuteRule(rule, 1, applicationInformationQuickCash);
        }

        [NUnit.Framework.Test]
        public void QuickCashCashOutReduceTestDoesNotSupportQuickCashTestPass()
        {
            QuickCashCashOutReduce rule = new QuickCashCashOutReduce();

            using (new TransactionScope(TransactionMode.New))
            {
                string HQL = @"from Application_DAO a where a.ApplicationType.Key = 7
                and a.ApplicationStatus.Key = 1 and a.Key in
                (select distinct ai.Application.Key from ApplicationInformation_DAO ai where
                ai.ApplicationInformationType.Key = 3)";

                SimpleQuery<Application_DAO> q = new SimpleQuery<Application_DAO>(HQL);
                q.SetQueryRange(1);
                Application_DAO[] res = q.Execute();

                if (res.Length > 0)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    IApplication app = BMTM.GetMappedType<IApplication>(res[0]);
                    ExecuteRule(rule, 0, app);
                }
            }
        }

        #endregion QuickCashUpFrontMaximum

        #region LegalEntityApplicationValidate

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        //[NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        //public void LegalEntityApplicationValidateNoArgumentsPassed()
        //{
        //    LegalEntityApplicationValidate rule = new LegalEntityApplicationValidate();

        //    ExecuteRule(rule, 0);
        //}

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        //[NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        //public void LegalEntityApplicationValidateIncorrectArgumentsPassed()
        //{
        //    LegalEntityApplicationValidate rule = new LegalEntityApplicationValidate();

        //    // Setup an incorrect Argumnt to pass along
        //    IApplication application = _mockery.StrictMock<IApplication>();

        //    ExecuteRule(rule, 0, application);
        //}

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        //[NUnit.Framework.Test]
        //public void LegalEntityApplicationValidateSuccess()
        //{
        //    LegalEntityApplicationValidate rule = new LegalEntityApplicationValidate();

        //    // Setup a correct Argumnt to pass along
        //    IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

        //    // Setup IApplication.ApplicationRoles
        //    IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
        //    Stack<IApplicationRole> applicationRoleStack = new Stack<IApplicationRole>();
        //    applicationRoleStack.Push(applicationRole);
        //    IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationRoleStack);
        //    SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(applicationRoles);

        //    // Setup IApplicationRole.ApplicationRoleType
        //    IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
        //    SetupResult.For(applicationRoleType.Key).Return((int)OfferRoleTypes.MainApplicant);
        //    SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);

        //    // Setup IApplicationRole.GeneralStatus
        //    IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
        //    SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
        //    SetupResult.For(applicationRole.GeneralStatus).Return(generalStatus);

        //    // Setup applicationRole.LegalEntity
        //    ILegalEntityNaturalPerson legalEntityNaturalPerson = _mockery.StrictMock<ILegalEntityNaturalPerson>();
        //    SetupResult.For(applicationRole.LegalEntity).Return(legalEntityNaturalPerson);
        //    SetupResult.For(legalEntityNaturalPerson.GetLegalName(LegalNameFormat.Full)).Return("Rodders");

        //    SetupResult.For(legalEntityNaturalPerson.Salutation).Return(_mockery.StrictMock<ISalutation>());
        //    SetupResult.For(legalEntityNaturalPerson.Initials).Return("SR");
        //    SetupResult.For(legalEntityNaturalPerson.FirstNames).Return("Siphumelele Rodney");
        //    SetupResult.For(legalEntityNaturalPerson.Surname).Return("Majola");
        //    SetupResult.For(legalEntityNaturalPerson.PreferredName).Return("Siphumelele");
        //    SetupResult.For(legalEntityNaturalPerson.Gender).Return(_mockery.StrictMock<IGender>());
        //    SetupResult.For(legalEntityNaturalPerson.MaritalStatus).Return(_mockery.StrictMock<IMaritalStatus>());
        //    SetupResult.For(legalEntityNaturalPerson.PopulationGroup).Return(_mockery.StrictMock<IPopulationGroup>());
        //    SetupResult.For(legalEntityNaturalPerson.Education).Return(_mockery.StrictMock<IEducation>());
        //    SetupResult.For(legalEntityNaturalPerson.CitizenType).Return(_mockery.StrictMock<ICitizenType>());
        //    SetupResult.For(legalEntityNaturalPerson.IDNumber).Return("7705201254084");
        //    SetupResult.For(legalEntityNaturalPerson.DateOfBirth).Return(new DateTime(77, 05, 20));

        //    SetupResult.For(legalEntityNaturalPerson.EmailAddress).Return("majolar@gmail.com");
        //    SetupResult.For(legalEntityNaturalPerson.CellPhoneNumber).Return(null);
        //    SetupResult.For(legalEntityNaturalPerson.FaxNumber).Return(null);
        //    SetupResult.For(legalEntityNaturalPerson.FaxCode).Return(null);
        //    SetupResult.For(legalEntityNaturalPerson.HomePhoneNumber).Return(null);
        //    SetupResult.For(legalEntityNaturalPerson.HomePhoneCode).Return(null);
        //    SetupResult.For(legalEntityNaturalPerson.WorkPhoneNumber).Return(null);
        //    SetupResult.For(legalEntityNaturalPerson.WorkPhoneCode).Return(null);

        //    ExecuteRule(rule, 0, applicationMortgageLoan);

        //}

        /// <summary>
        /// Expects Messages.Count <> 0.
        /// </summary>
        //[NUnit.Framework.Test]
        //public void LegalEntityApplicationValidateFail()
        //{
        //    LegalEntityApplicationValidate rule = new LegalEntityApplicationValidate();

        //    // Setup a correct Argumnt to pass along
        //    IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

        //    // Setup IApplication.ApplicationRoles
        //    IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
        //    Stack<IApplicationRole> applicationRoleStack = new Stack<IApplicationRole>();
        //    applicationRoleStack.Push(applicationRole);
        //    IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationRoleStack);
        //    SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(applicationRoles);

        //    // Setup IApplicationRole.ApplicationRoleType
        //    IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
        //    SetupResult.For(applicationRoleType.Key).Return((int)OfferRoleTypes.MainApplicant);
        //    SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);

        //    // Setup IApplicationRole.GeneralStatus
        //    IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
        //    SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
        //    SetupResult.For(applicationRole.GeneralStatus).Return(generalStatus);

        //    // Setup applicationRole.LegalEntity
        //    ILegalEntityNaturalPerson legalEntityNaturalPerson = _mockery.StrictMock<ILegalEntityNaturalPerson>();
        //    SetupResult.For(applicationRole.LegalEntity).Return(legalEntityNaturalPerson);
        //    SetupResult.For(legalEntityNaturalPerson.GetLegalName(LegalNameFormat.Full)).Return("Rodders");

        //    SetupResult.For(legalEntityNaturalPerson.Salutation).Return(_mockery.StrictMock<ISalutation>());
        //    SetupResult.For(legalEntityNaturalPerson.Initials).Return("SR");
        //    SetupResult.For(legalEntityNaturalPerson.FirstNames).Return("Siphumelele Rodney");
        //    SetupResult.For(legalEntityNaturalPerson.Surname).Return("Majola");
        //    SetupResult.For(legalEntityNaturalPerson.PreferredName).Return("Siphumelele");
        //    // Should cause it to fail
        //    SetupResult.For(legalEntityNaturalPerson.Gender).Return(null);
        //    SetupResult.For(legalEntityNaturalPerson.MaritalStatus).Return(_mockery.StrictMock<IMaritalStatus>());
        //    SetupResult.For(legalEntityNaturalPerson.PopulationGroup).Return(_mockery.StrictMock<IPopulationGroup>());
        //    SetupResult.For(legalEntityNaturalPerson.Education).Return(_mockery.StrictMock<IEducation>());
        //    SetupResult.For(legalEntityNaturalPerson.CitizenType).Return(_mockery.StrictMock<ICitizenType>());
        //    SetupResult.For(legalEntityNaturalPerson.IDNumber).Return("7705201254084");
        //    SetupResult.For(legalEntityNaturalPerson.DateOfBirth).Return(new DateTime(77, 05, 20));

        //    SetupResult.For(legalEntityNaturalPerson.EmailAddress).Return("majolar@gmail.com");
        //    SetupResult.For(legalEntityNaturalPerson.CellPhoneNumber).Return(null);
        //    SetupResult.For(legalEntityNaturalPerson.FaxNumber).Return(null);
        //    SetupResult.For(legalEntityNaturalPerson.FaxCode).Return(null);
        //    SetupResult.For(legalEntityNaturalPerson.HomePhoneNumber).Return(null);
        //    SetupResult.For(legalEntityNaturalPerson.HomePhoneCode).Return(null);
        //    SetupResult.For(legalEntityNaturalPerson.WorkPhoneNumber).Return(null);
        //    SetupResult.For(legalEntityNaturalPerson.WorkPhoneCode).Return(null);

        //    ExecuteRule(rule, 1, applicationMortgageLoan);
        //}

        #endregion LegalEntityApplicationValidate

        #region HOCCollectionMinimum

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void HOCCollectionMinimumNoArgumentsPassed()
        {
            HOCCollectionMinimum rule = new HOCCollectionMinimum();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void HOCCollectionMinimumIncorrectArgumentsPassed()
        {
            HOCCollectionMinimum rule = new HOCCollectionMinimum();

            // Setup an incorrect Argumnt to pass along
            IApplicationMailingAddress application = _mockery.StrictMock<IApplicationMailingAddress>();

            ExecuteRule(rule, 0, application);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void HOCCollectionMinimumSuccess()
        {
            HOCCollectionMinimum rule = new HOCCollectionMinimum();

            // Setup the correct object to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(applicationMortgageLoan.Key).Return(1);

            // Setup applicationMortgageLoan.Property
            IProperty property = _mockery.StrictMock<IProperty>();
            SetupResult.For(applicationMortgageLoan.Property).Return(property);

            // applicationMortgageLoan.Property.HOC

            // HOCCollectionMinimum rule no longer gets the HOC from Property.HOC

            //IHOC hoc = _mockery.StrictMock<IHOC>();
            //SetupResult.For(property.HOC).Return(hoc);

            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            IHOCRepository hocRepo = _mockery.StrictMock<IHOCRepository>();
            MockCache.Add(typeof(IHOCRepository).ToString(), hocRepo);

            IAccountHOC accHoc = _mockery.StrictMock<IAccountHOC>();

            SetupResult.For(hocRepo.RetrieveHOCByOfferKey(1)).IgnoreArguments().Return(accHoc);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void HOCCollectionMinimumFail()
        {
            HOCCollectionMinimum rule = new HOCCollectionMinimum();

            // Setup the correct object to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(applicationMortgageLoan.Key).Return(1);

            // Setup applicationMortgageLoan.Property
            IProperty property = _mockery.StrictMock<IProperty>();

            //  SetupResult.For(applicationMortgageLoan.Property).Return(property);

            SetupResult.For(applicationMortgageLoan.Property).Return(null);

            // applicationMortgageLoan.Property.HOC
            //   SetupResult.For(property.HOC).Return(null);     // This should cause failure

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        #endregion HOCCollectionMinimum

        #region ValuationExists

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ValuationExistsSuccess()
        {
            using (new SessionScope())
            {
                ValuationExists rule = new ValuationExists(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                string sql = @"select top 1 offerkey from
                        offermortgageloan oml (nolock)
                        join [Property] p (nolock) on oml.PropertyKey = p.PropertyKey
                        join Valuation v (nolock) on v.PropertyKey = p.PropertyKey
                        where v.IsActive = 1";
                DataTable dt = base.GetQueryResults(sql);
                int offerKey = Convert.ToInt32(dt.Rows[0][0]);
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                IApplication app = appRepo.GetApplicationByKey(offerKey);

                ExecuteRule(rule, 0, app);
            }
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ValuationExistsFail()
        {
            using (new SessionScope())
            {
                ValuationExists rule = new ValuationExists(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                string sql = @"select top 1 oml.OfferKey
                    from offermortgageloan oml (nolock)
                    where oml.PropertyKey is null";
                DataTable dt = base.GetQueryResults(sql);
                int offerKey = Convert.ToInt32(dt.Rows[0][0]);
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                IApplication app = appRepo.GetApplicationByKey(offerKey);

                ExecuteRule(rule, 1, app);
            }
        }

        #endregion ValuationExists

        #region CancellationBankDetailsSwitch

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void CancellationBankDetailsSwitchSuccess()
        {
            CancellationBankDetailsSwitch rule = new CancellationBankDetailsSwitch();

            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationType applicationType = _mockery.StrictMock<IApplicationType>();

            SetupResult.For(applicationMortgageLoan.ApplicationType).Return(applicationType);
            SetupResult.For(applicationType.Key).Return((int)SAHL.Common.Globals.OfferTypes.SwitchLoan);

            // set up the list to contain a decent expense type
            IEventList<IApplicationExpense> expenses = new EventList<IApplicationExpense>();
            IApplicationExpense applicationExpense = _mockery.StrictMock<IApplicationExpense>();
            IExpenseType expenseType = _mockery.StrictMock<IExpenseType>();

            SetupResult.For(applicationMortgageLoan.Key).Return(1);
            SetupResult.For(applicationMortgageLoan.ApplicationExpenses).Return(expenses);
            SetupResult.For(applicationExpense.ExpenseType).Return(expenseType);
            SetupResult.For(expenseType.Key).Return((int)ExpenseTypes.Existingmortgageamount);
            expenses.Add(new DomainMessageCollection(), applicationExpense);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void CancellationBankDetailsSwitchFail()
        {
            CancellationBankDetailsSwitch rule = new CancellationBankDetailsSwitch();

            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationType applicationType = _mockery.StrictMock<IApplicationType>();

            SetupResult.For(applicationMortgageLoan.Key).Return(1);
            SetupResult.For(applicationMortgageLoan.ApplicationType).Return(applicationType);
            SetupResult.For(applicationType.Key).Return((int)SAHL.Common.Globals.OfferTypes.SwitchLoan);
            SetupResult.For(applicationMortgageLoan.ApplicationExpenses).Return(new EventList<IApplicationExpense>());

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        #endregion CancellationBankDetailsSwitch

        #region SellersDetailsMandatoryNewPurchase

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void SellersDetailsMandatoryNewPurchaseSuccess()
        {
            SellersDetailsMandatoryNewPurchase rule = new SellersDetailsMandatoryNewPurchase();

            // Setup the correct object to pass along
            IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = _mockery.StrictMock<IApplicationMortgageLoanNewPurchase>();
            SetupResult.For(applicationMortgageLoanNewPurchase.Key).Return(1);

            // Setup IApplication.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> applicationRoleStack = new Stack<IApplicationRole>();
            applicationRoleStack.Push(applicationRole);
            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationRoleStack);
            SetupResult.For(applicationMortgageLoanNewPurchase.ApplicationRoles).Return(applicationRoles);

            // Setup IApplicationRole.ApplicationRoleType
            IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(applicationRoleType.Key).Return((int)OfferRoleTypes.Seller);
            SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);

            // Setup IApplicationRole.GeneralStatus
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(applicationRole.GeneralStatus).Return(generalStatus);

            ExecuteRule(rule, 0, applicationMortgageLoanNewPurchase);
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void SellersDetailsMandatoryNewPurchaseFail()
        {
            SellersDetailsMandatoryNewPurchase rule = new SellersDetailsMandatoryNewPurchase();

            // Setup the correct object to pass along
            IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = _mockery.StrictMock<IApplicationMortgageLoanNewPurchase>();
            SetupResult.For(applicationMortgageLoanNewPurchase.Key).Return(1);

            // Setup IApplication.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> applicationRoleStack = new Stack<IApplicationRole>();
            applicationRoleStack.Push(applicationRole);
            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationRoleStack);
            SetupResult.For(applicationMortgageLoanNewPurchase.ApplicationRoles).Return(applicationRoles);

            // Setup IApplicationRole.ApplicationRoleType
            IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(applicationRoleType.Key).Return((int)OfferRoleTypes.PreviousInsurer);   // This should cause an error ...
            SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);

            // Setup IApplicationRole.GeneralStatus
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(applicationRole.GeneralStatus).Return(generalStatus);

            ExecuteRule(rule, 1, applicationMortgageLoanNewPurchase);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void SellersDetailsMandatoryNewPurchaseSuccessNonNewPurchase()
        {
            SellersDetailsMandatoryNewPurchase rule = new SellersDetailsMandatoryNewPurchase();

            // Setup a non new pucrhase application object to pass along
            IApplicationMortgageLoanSwitch applicationMortgageLoanSwitch = _mockery.StrictMock<IApplicationMortgageLoanSwitch>();
            SetupResult.For(applicationMortgageLoanSwitch.Key).Return(1);

            // Setup IApplication.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> applicationRoleStack = new Stack<IApplicationRole>();
            applicationRoleStack.Push(applicationRole);
            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationRoleStack);
            SetupResult.For(applicationMortgageLoanSwitch.ApplicationRoles).Return(applicationRoles);

            // Setup IApplicationRole.ApplicationRoleType
            IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(applicationRoleType.Key).Return((int)OfferRoleTypes.Seller);
            SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);

            // Setup IApplicationRole.GeneralStatus
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(applicationRole.GeneralStatus).Return(generalStatus);

            ExecuteRule(rule, 0, applicationMortgageLoanSwitch);
        }

        #endregion SellersDetailsMandatoryNewPurchase

        #region ApplicationCheckEmploymentTypeIsNotUnknown

        [NUnit.Framework.Test]
        public void ApplicationCheckEmploymentTypeIsNotUnknownTestPass()
        {
            IApplicationFurtherLoan furtherLoan = _mockery.StrictMock<IApplicationFurtherLoan>();
            SetupResult.For(furtherLoan.Key).Return(10);
            IApplicationProductVariableLoan svli = _mockery.StrictMock<IApplicationProductVariableLoan>();
            IApplicationInformationVariableLoan aiv = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            IEmploymentType empType = _mockery.StrictMock<IEmploymentType>();
            SetupResult.For(empType.Key).Return((int)EmploymentTypes.Salaried);
            SetupResult.For(aiv.EmploymentType).Return(empType);
            SetupResult.For(svli.VariableLoanInformation).Return(aiv);
            SetupResult.For(furtherLoan.CurrentProduct).Return(svli as IApplicationProductVariableLoan);

            ApplicationCheckEmploymentTypeIsNotUnknown rule = new ApplicationCheckEmploymentTypeIsNotUnknown();
            ExecuteRule(rule, 0, furtherLoan);
        }

        [NUnit.Framework.Test]
        public void ApplicationCheckEmploymentTypeIsNotUnknownTestFail()
        {
            IApplicationFurtherLoan furtherLoan = _mockery.StrictMock<IApplicationFurtherLoan>();
            SetupResult.For(furtherLoan.Key).Return(10);
            IApplicationProductVariableLoan svli = _mockery.StrictMock<IApplicationProductVariableLoan>();
            IApplicationInformationVariableLoan aiv = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            IEmploymentType empType = _mockery.StrictMock<IEmploymentType>();
            SetupResult.For(empType.Key).Return((int)EmploymentTypes.Unknown);
            SetupResult.For(aiv.EmploymentType).Return(empType);
            SetupResult.For(svli.VariableLoanInformation).Return(aiv);
            SetupResult.For(furtherLoan.CurrentProduct).Return(svli as IApplicationProductVariableLoan);

            ApplicationCheckEmploymentTypeIsNotUnknown rule = new ApplicationCheckEmploymentTypeIsNotUnknown();
            ExecuteRule(rule, 1, furtherLoan);
        }

        #endregion ApplicationCheckEmploymentTypeIsNotUnknown

        #endregion Application MortgageLoan Spec

        #region Account MortgageLoan Spec

        #region MortgageLoanAccountBondMinimum

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountBondMinimumSuccess()
        {
            MortgageLoanAccountBondMinimum rule = new MortgageLoanAccountBondMinimum();

            // Setup the correct object to pass along
            IMortgageLoan mortgageLoan = _mockery.StrictMock<IMortgageLoan>();

            // Setup applicationMortgageLoan.ApplicationDebitOrders
            IEventList<IBond> bonds = new EventList<IBond>();
            SetupResult.For(mortgageLoan.Bonds).Return(bonds);

            // Setup Bonds.Count
            bonds.Add(Messages, _mockery.StrictMock<IBond>());

            ExecuteRule(rule, 0, mortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountBondMinimumFail()
        {
            MortgageLoanAccountBondMinimum rule = new MortgageLoanAccountBondMinimum();

            // Setup the correct object to pass along
            IMortgageLoan mortgageLoan = _mockery.StrictMock<IMortgageLoan>();

            // Setup applicationMortgageLoan.ApplicationDebitOrders
            IEventList<IBond> bonds = new EventList<IBond>();
            SetupResult.For(mortgageLoan.Bonds).Return(bonds);

            ExecuteRule(rule, 1, mortgageLoan);
        }

        #endregion MortgageLoanAccountBondMinimum

        #region AccountMailingAddress

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AccountMailingAddressSuccess()
        {
            AccountMailingAddress rule = new AccountMailingAddress();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);

            // mortgageLoanAccount.MailingAddresses
            IEventList<IMailingAddress> mailingAddresses = new EventList<IMailingAddress>();
            IMailingAddress mailingAddress = _mockery.StrictMock<IMailingAddress>();
            mailingAddresses.Add(Messages, mailingAddress);
            SetupResult.For(mortgageLoanAccount.MailingAddresses).Return(mailingAddresses);

            // mailingAddress.Address.Key
            IAddress address = _mockery.StrictMock<IAddress>();
            SetupResult.For(mailingAddress.Address).Return(address);
            SetupResult.For(address.Key).Return(1);

            // Setup roles.Count
            IRole role = _mockery.DynamicMock<IRole>();
            roles.Add(Messages, role);

            // Setup role.RoleType.Key
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.LeadMainApplicant);
            SetupResult.For(role.RoleType).Return(roleType);

            // Setup role.RoleType.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(role.GeneralStatus).Return(generalStatus);

            // Setup role.LegalEntity
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(role.LegalEntity).Return(legalEntity);

            // Setup LegalEntity.LegalEntityAddresses
            IEventList<ILegalEntityAddress> legalEntityAddresses = new EventList<ILegalEntityAddress>();
            ILegalEntityAddress legalEntityAddress = _mockery.StrictMock<ILegalEntityAddress>();
            legalEntityAddresses.Add(Messages, legalEntityAddress);
            SetupResult.For(legalEntityAddress.Address).Return(address);
            SetupResult.For(legalEntity.LegalEntityAddresses).Return(legalEntityAddresses);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AccountMailingAddressFail()
        {
            AccountMailingAddress rule = new AccountMailingAddress();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);

            // mortgageLoanAccount.MailingAddresses
            IEventList<IMailingAddress> mailingAddresses = new EventList<IMailingAddress>();
            IMailingAddress mailingAddress = _mockery.StrictMock<IMailingAddress>();
            mailingAddresses.Add(Messages, mailingAddress);
            SetupResult.For(mortgageLoanAccount.MailingAddresses).Return(mailingAddresses);

            // mailingAddress.Address.Key
            IAddress address = _mockery.StrictMock<IAddress>();
            SetupResult.For(mailingAddress.Address).Return(address);
            SetupResult.For(address.Key).Return(1);

            // Setup roles.Count
            IRole role = _mockery.DynamicMock<IRole>();
            roles.Add(Messages, role);

            // Setup role.RoleType.Key
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.LeadMainApplicant);
            SetupResult.For(role.RoleType).Return(roleType);

            // Setup role.RoleType.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(role.GeneralStatus).Return(generalStatus);

            // Setup role.LegalEntity
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(role.LegalEntity).Return(legalEntity);

            // Setup LegalEntity.LegalEntityAddresses
            IEventList<ILegalEntityAddress> legalEntityAddresses = new EventList<ILegalEntityAddress>();
            ILegalEntityAddress legalEntityAddress = _mockery.StrictMock<ILegalEntityAddress>();
            legalEntityAddresses.Add(Messages, legalEntityAddress);

            IAddress addressLE = _mockery.StrictMock<IAddress>();
            SetupResult.For(addressLE.Key).Return(2);   // A different address key to fail the test.
            SetupResult.For(legalEntityAddress.Address).Return(addressLE);
            SetupResult.For(legalEntity.LegalEntityAddresses).Return(legalEntityAddresses);

            ExecuteRule(rule, 1, mortgageLoanAccount);
        }

        #endregion AccountMailingAddress

        #region MortgageLoanAccountMailingActive

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountMailingActiveSuccess()
        {
            MortgageLoanAccountMailingActive rule = new MortgageLoanAccountMailingActive();

            // Setup the correct object to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(applicationMortgageLoan.Key).Return(1);

            // Setup applicationMortgageLoan.ApplicationMailingAddresses
            IEventList<IApplicationMailingAddress> applicationMailingAddresses = new EventList<IApplicationMailingAddress>();
            SetupResult.For(applicationMortgageLoan.ApplicationMailingAddresses).Return(applicationMailingAddresses);
            IApplicationMailingAddress applicationMailingAddress = _mockery.StrictMock<IApplicationMailingAddress>();
            applicationMailingAddresses.Add(Messages, applicationMailingAddress);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountMailingActiveFail()
        {
            MortgageLoanAccountMailingActive rule = new MortgageLoanAccountMailingActive();

            // Setup the correct object to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(applicationMortgageLoan.Key).Return(1);

            // Setup applicationMortgageLoan.ApplicationMailingAddresses
            IEventList<IApplicationMailingAddress> applicationMailingAddresses = new EventList<IApplicationMailingAddress>();
            SetupResult.For(applicationMortgageLoan.ApplicationMailingAddresses).Return(applicationMailingAddresses);

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        #endregion MortgageLoanAccountMailingActive

        #region MortgageLoanAccountFixedDebitOrderValueNonSubsidy

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountFixedDebitOrderValueNonSubsidySuccess()
        {
            MortgageLoanAccountFixedDebitOrderValueNonSubsidy rule = new MortgageLoanAccountFixedDebitOrderValueNonSubsidy();

            // Setup the correct object to pass along
            IAccount account = _mockery.StrictMock<IAccount>();

            // Setup account.FixedPayment
            SetupResult.For(account.FixedPayment).Return(0.0);

            // Setup account.InstallmentSummary.TotalAmountDue
            IAccountInstallmentSummary accountInstallmentSummary = _mockery.StrictMock<IAccountInstallmentSummary>();
            SetupResult.For(account.InstallmentSummary).Return(accountInstallmentSummary);
            SetupResult.For(accountInstallmentSummary.TotalAmountDue).Return(0.0);

            ExecuteRule(rule, 0, account);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountFixedDebitOrderValueNonSubsidyFail()
        {
            MortgageLoanAccountFixedDebitOrderValueNonSubsidy rule = new MortgageLoanAccountFixedDebitOrderValueNonSubsidy();

            // Setup the correct object to pass along
            IAccount account = _mockery.StrictMock<IAccount>();

            // Setup account.FixedPayment
            SetupResult.For(account.FixedPayment).Return(4300.0);

            // Setup account.InstallmentSummary.TotalAmountDue
            IAccountInstallmentSummary accountInstallmentSummary = _mockery.StrictMock<IAccountInstallmentSummary>();
            SetupResult.For(account.InstallmentSummary).Return(accountInstallmentSummary);
            SetupResult.For(accountInstallmentSummary.TotalAmountDue).Return(4500.0);

            ExecuteRule(rule, 1, account);
        }

        #endregion MortgageLoanAccountFixedDebitOrderValueNonSubsidy

        #region MortgageLoanAccountEmploymentCurrent

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountEmploymentCurrentSuccess()
        {
            MortgageLoanAccountEmploymentCurrent rule = new MortgageLoanAccountEmploymentCurrent();

            // Setup the correct object to pass along
            IAccount account = _mockery.StrictMock<IAccount>();

            // Setup account.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);
            SetupResult.For(account.Roles).Return(roles);

            // Setup role.GeneralStatus.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);

            // role.LegalEntity.Employment
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEventList<IEmployment> employments = new EventList<IEmployment>();
            employments.Add(Messages, employment);
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            SetupResult.For(legalEntity.Employment).Return(employments);

            // Setup employment.EmploymentStatus.Key
            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Current);
            SetupResult.For(employment.EmploymentStatus).Return(employmentStatus);

            // Setup employment.IsConfirmed
            SetupResult.For(employment.IsConfirmed).Return(true);

            ExecuteRule(rule, 0, account);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountEmploymentCurrentFail()
        {
            MortgageLoanAccountEmploymentCurrent rule = new MortgageLoanAccountEmploymentCurrent();

            // Setup the correct object to pass along
            IAccount account = _mockery.StrictMock<IAccount>();

            // Setup account.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);
            SetupResult.For(account.Roles).Return(roles);

            // Setup role.GeneralStatus.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);

            // role.LegalEntity.Employment
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEventList<IEmployment> employments = new EventList<IEmployment>();
            employments.Add(Messages, employment);
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            SetupResult.For(legalEntity.Employment).Return(employments);

            // Setup employment.EmploymentStatus.Key
            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Current);
            SetupResult.For(employment.EmploymentStatus).Return(employmentStatus);

            // Setup employment.IsConfirmed
            SetupResult.For(employment.IsConfirmed).Return(false);

            ExecuteRule(rule, 1, account);
        }

        #endregion MortgageLoanAccountEmploymentCurrent

        #region MortgageLoanPropertyLink

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanPropertyLinkSuccess()
        {
            MortgageLoanPropertyLink rule = new MortgageLoanPropertyLink();

            // Setup the correct object to pass along
            IMortgageLoanAccount account = _mockery.StrictMock<IMortgageLoanAccount>();

            IProperty property = _mockery.StrictMock<IProperty>();

            IMortgageLoan securedMortgageLoan = _mockery.StrictMock<IMortgageLoan>();
            SetupResult.For(account.SecuredMortgageLoan).Return(securedMortgageLoan);
            SetupResult.For(securedMortgageLoan.Property).Return(property);

            ExecuteRule(rule, 0, account);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanPropertyLinkFail()
        {
            MortgageLoanPropertyLink rule = new MortgageLoanPropertyLink();

            // Setup the correct object to pass along
            IMortgageLoanAccount account = _mockery.StrictMock<IMortgageLoanAccount>();

            IMortgageLoan securedMortgageLoan = _mockery.StrictMock<IMortgageLoan>();
            SetupResult.For(account.SecuredMortgageLoan).Return(securedMortgageLoan);
            SetupResult.For(securedMortgageLoan.Property).Return(null);

            ExecuteRule(rule, 1, account);
        }

        #endregion MortgageLoanPropertyLink

        #region MortgageLoanAccountMonthlyServiceFees

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountMonthlyServiceFeesSuccess()
        {
            MortgageLoanAccountMonthlyServiceFees rule = new MortgageLoanAccountMonthlyServiceFees();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.FinancialServices
            IEventList<IFinancialService> financialServices = new EventList<IFinancialService>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            financialServices.Add(Messages, financialService);
            SetupResult.For(mortgageLoanAccount.FinancialServices).Return(financialServices);

            // Setup financialService.FinancialServiceRecurringTransactions
            IEventList<IManualDebitOrder> manualDebitOrders = new EventList<IManualDebitOrder>();
            IManualDebitOrder manualDebitOrder = _mockery.StrictMock<IManualDebitOrder>();
            manualDebitOrders.Add(Messages, manualDebitOrder);
            SetupResult.For(financialService.ManualDebitOrders).Return(manualDebitOrders);

            // Setup financialServiceRecurringTransaction.TransactionType.Key
            ITransactionType transactionType = _mockery.StrictMock<ITransactionType>();
            SetupResult.For(transactionType.Key).Return((System.Int16)TransactionTypes.MonthlyServiceFee);
            SetupResult.For(manualDebitOrder.TransactionType).Return(transactionType);

            IProperty property = _mockery.StrictMock<IProperty>();

            IMortgageLoan securedMortgageLoan = _mockery.StrictMock<IMortgageLoan>();
            SetupResult.For(mortgageLoanAccount.SecuredMortgageLoan).Return(securedMortgageLoan);
            SetupResult.For(securedMortgageLoan.Property).Return(property);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountMonthlyServiceFeesFail()
        {
            MortgageLoanAccountMonthlyServiceFees rule = new MortgageLoanAccountMonthlyServiceFees();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.FinancialServices
            IEventList<IFinancialService> financialServices = new EventList<IFinancialService>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            financialServices.Add(Messages, financialService);
            SetupResult.For(mortgageLoanAccount.FinancialServices).Return(financialServices);

            // Setup financialService.FinancialServiceRecurringTransactions
            IEventList<IManualDebitOrder> manualDebitOrders = new EventList<IManualDebitOrder>();
            IManualDebitOrder manualDebitOrder = _mockery.StrictMock<IManualDebitOrder>();
            manualDebitOrders.Add(Messages, manualDebitOrder);
            SetupResult.For(financialService.ManualDebitOrders).Return(manualDebitOrders);

            // Setup financialServiceRecurringTransaction.TransactionType.Key
            ITransactionType transactionType = _mockery.StrictMock<ITransactionType>();
            SetupResult.For(transactionType.Key).Return((System.Int16)56);
            SetupResult.For(manualDebitOrder.TransactionType).Return(transactionType);

            IProperty property = _mockery.StrictMock<IProperty>();
            IMortgageLoan securedMortgageLoan = _mockery.StrictMock<IMortgageLoan>();
            SetupResult.For(mortgageLoanAccount.SecuredMortgageLoan).Return(securedMortgageLoan);
            SetupResult.For(securedMortgageLoan.Property).Return(property);

            ExecuteRule(rule, 1, mortgageLoanAccount);
        }

        #endregion MortgageLoanAccountMonthlyServiceFees

        #region MortgageLoanLegalEntityBankAccount

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityBankAccountSuccess()
        {
            MortgageLoanLegalEntityBankAccount rule = new MortgageLoanLegalEntityBankAccount();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.FinancialServices
            IEventList<IFinancialService> financialServices = new EventList<IFinancialService>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            financialServices.Add(Messages, financialService);
            SetupResult.For(mortgageLoanAccount.FinancialServices).Return(financialServices);

            // Setup financialService.FinancialServiceBankAccounts
            IEventList<IFinancialServiceBankAccount> financialServiceBankAccounts = new EventList<IFinancialServiceBankAccount>();
            IFinancialServiceBankAccount financialServiceBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
            financialServiceBankAccounts.Add(Messages, financialServiceBankAccount);
            SetupResult.For(financialService.FinancialServiceBankAccounts).Return(financialServiceBankAccounts);

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);

            // Setup role.LegalEntity.LegalEntityBankAccounts
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            ILegalEntityBankAccount legalEntityBankAccount = _mockery.StrictMock<ILegalEntityBankAccount>();
            IEventList<ILegalEntityBankAccount> legalEntityBankAccounts = new EventList<ILegalEntityBankAccount>();
            legalEntityBankAccounts.Add(Messages, legalEntityBankAccount);
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            SetupResult.For(legalEntity.LegalEntityBankAccounts).Return(legalEntityBankAccounts);

            // Setup legalEntityBankAccounts.BankAccount.Key
            IBankAccount bankAccount = _mockery.StrictMock<IBankAccount>();
            SetupResult.For(legalEntityBankAccount.BankAccount).Return(bankAccount);
            SetupResult.For(bankAccount.Key).Return(2);

            // Setup financialServiceBankAccount.BankAccount.Key
            SetupResult.For(financialServiceBankAccount.BankAccount).Return(bankAccount);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityBankAccountFail()
        {
            MortgageLoanLegalEntityBankAccount rule = new MortgageLoanLegalEntityBankAccount();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.FinancialServices
            IEventList<IFinancialService> financialServices = new EventList<IFinancialService>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            financialServices.Add(Messages, financialService);
            SetupResult.For(mortgageLoanAccount.FinancialServices).Return(financialServices);

            // Setup financialService.FinancialServiceBankAccounts
            IEventList<IFinancialServiceBankAccount> financialServiceBankAccounts = new EventList<IFinancialServiceBankAccount>();
            IFinancialServiceBankAccount financialServiceBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
            financialServiceBankAccounts.Add(Messages, financialServiceBankAccount);
            SetupResult.For(financialService.FinancialServiceBankAccounts).Return(financialServiceBankAccounts);

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);

            // Setup role.LegalEntity.LegalEntityBankAccounts
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            ILegalEntityBankAccount legalEntityBankAccount = _mockery.StrictMock<ILegalEntityBankAccount>();
            IEventList<ILegalEntityBankAccount> legalEntityBankAccounts = new EventList<ILegalEntityBankAccount>();
            legalEntityBankAccounts.Add(Messages, legalEntityBankAccount);
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            SetupResult.For(legalEntity.LegalEntityBankAccounts).Return(legalEntityBankAccounts);

            // Setup legalEntityBankAccounts.BankAccount.Key
            IBankAccount bankAccount = _mockery.StrictMock<IBankAccount>();
            SetupResult.For(legalEntityBankAccount.BankAccount).Return(bankAccount);
            SetupResult.For(bankAccount.Key).Return(2);

            // Setup financialServiceBankAccount.BankAccount.Key
            IBankAccount fsBankAccount = _mockery.StrictMock<IBankAccount>();
            SetupResult.For(fsBankAccount.Key).Return(1); // A different key - expect failure.
            SetupResult.For(financialServiceBankAccount.BankAccount).Return(fsBankAccount);

            ExecuteRule(rule, 1, mortgageLoanAccount);
        }

        #endregion MortgageLoanLegalEntityBankAccount

        #region MortgageLoanBankAccountPaymentTypeDebitOrder

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanBankAccountPaymentTypeDebitOrderSuccess()
        {
            MortgageLoanBankAccountPaymentTypeDebitOrder rule = new MortgageLoanBankAccountPaymentTypeDebitOrder();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.FinancialServices
            IEventList<IFinancialService> financialServices = new EventList<IFinancialService>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            financialServices.Add(Messages, financialService);
            SetupResult.For(mortgageLoanAccount.FinancialServices).Return(financialServices);

            // Setup financialService.FinancialServiceBankAccounts
            IEventList<IFinancialServiceBankAccount> financialServiceBankAccounts = new EventList<IFinancialServiceBankAccount>();
            IFinancialServiceBankAccount financialServiceBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
            financialServiceBankAccounts.Add(Messages, financialServiceBankAccount);
            SetupResult.For(financialService.FinancialServiceBankAccounts).Return(financialServiceBankAccounts);

            // Setup financialServiceBankAccount.FinancialServicePaymentType.Key
            IFinancialServicePaymentType financialServicePaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();
            SetupResult.For(financialServicePaymentType.Key).Return((int)FinancialServicePaymentTypes.DebitOrderPayment);
            SetupResult.For(financialServiceBankAccount.FinancialServicePaymentType).Return(financialServicePaymentType);

            // Setup financialServiceBankAccount.BankAccount.Key
            IBankAccount bankAccount = _mockery.StrictMock<IBankAccount>();
            SetupResult.For(bankAccount.Key).Return(2);
            SetupResult.For(financialServiceBankAccount.BankAccount).Return(bankAccount);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanBankAccountPaymentTypeDebitOrderFail()
        {
            MortgageLoanBankAccountPaymentTypeDebitOrder rule = new MortgageLoanBankAccountPaymentTypeDebitOrder();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.FinancialServices
            IEventList<IFinancialService> financialServices = new EventList<IFinancialService>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            financialServices.Add(Messages, financialService);
            SetupResult.For(mortgageLoanAccount.FinancialServices).Return(financialServices);

            // Setup financialService.FinancialServiceBankAccounts
            IEventList<IFinancialServiceBankAccount> financialServiceBankAccounts = new EventList<IFinancialServiceBankAccount>();
            IFinancialServiceBankAccount financialServiceBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
            financialServiceBankAccounts.Add(Messages, financialServiceBankAccount);
            SetupResult.For(financialService.FinancialServiceBankAccounts).Return(financialServiceBankAccounts);

            // Setup financialServiceBankAccount.FinancialServicePaymentType.Key
            IFinancialServicePaymentType financialServicePaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();
            SetupResult.For(financialServicePaymentType.Key).Return((int)FinancialServicePaymentTypes.DebitOrderPayment);
            SetupResult.For(financialServiceBankAccount.FinancialServicePaymentType).Return(financialServicePaymentType);

            // Setup financialServiceBankAccount.BankAccount
            SetupResult.For(financialServiceBankAccount.BankAccount).Return(null);

            ExecuteRule(rule, 1, mortgageLoanAccount);
        }

        #endregion MortgageLoanBankAccountPaymentTypeDebitOrder

        #region MortgageLoanBankAccountPaymentTypeSubsidyAdd

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanBankAccountPaymentTypeSubsidyAddSuccess()
        {
            MortgageLoanBankAccountPaymentTypeSubsidyAdd rule = new MortgageLoanBankAccountPaymentTypeSubsidyAdd();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.FinancialServices
            IEventList<IFinancialService> financialServices = new EventList<IFinancialService>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            financialServices.Add(Messages, financialService);
            SetupResult.For(mortgageLoanAccount.FinancialServices).Return(financialServices);

            // Setup financialService.FinancialServiceBankAccounts
            IEventList<IFinancialServiceBankAccount> financialServiceBankAccounts = new EventList<IFinancialServiceBankAccount>();
            IFinancialServiceBankAccount financialServiceBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
            financialServiceBankAccounts.Add(Messages, financialServiceBankAccount);
            SetupResult.For(financialService.FinancialServiceBankAccounts).Return(financialServiceBankAccounts);

            // Setup financialServiceBankAccount.FinancialServicePaymentType.Key
            IFinancialServicePaymentType financialServicePaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();
            SetupResult.For(financialServicePaymentType.Key).Return((int)FinancialServicePaymentTypes.SubsidyPayment);
            SetupResult.For(financialServiceBankAccount.FinancialServicePaymentType).Return(financialServicePaymentType);

            // Setup account.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);

            // Setup role.GeneralStatus.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);

            // Setup role.LegalEntity
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(role.LegalEntity).Return(legalEntity);

            // role.LegalEntity.Employment
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEventList<IEmployment> employments = new EventList<IEmployment>();
            employments.Add(Messages, employment);
            SetupResult.For(legalEntity.Employment).Return(employments);

            // Setup employment.IsConfirmed
            SetupResult.For(employment.IsConfirmed).Return(true);

            // Setup employment.EmploymentType
            IEmploymentType employmentType = _mockery.StrictMock<IEmploymentType>();
            SetupResult.For(employment.EmploymentType).Return(employmentType);
            SetupResult.For(employmentType.Key).Return((int)(int)EmploymentTypes.SalariedwithDeduction);

            // Setup employment.EmploymentStatus
            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(employment.EmploymentStatus).Return(employmentStatus);
            SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Current);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanBankAccountPaymentTypeSubsidyAddFail()
        {
            MortgageLoanBankAccountPaymentTypeSubsidyAdd rule = new MortgageLoanBankAccountPaymentTypeSubsidyAdd();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.FinancialServices
            IEventList<IFinancialService> financialServices = new EventList<IFinancialService>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            financialServices.Add(Messages, financialService);
            SetupResult.For(mortgageLoanAccount.FinancialServices).Return(financialServices);

            // Setup financialService.FinancialServiceBankAccounts
            IEventList<IFinancialServiceBankAccount> financialServiceBankAccounts = new EventList<IFinancialServiceBankAccount>();
            IFinancialServiceBankAccount financialServiceBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
            financialServiceBankAccounts.Add(Messages, financialServiceBankAccount);
            SetupResult.For(financialService.FinancialServiceBankAccounts).Return(financialServiceBankAccounts);

            // Setup financialServiceBankAccount.FinancialServicePaymentType.Key
            IFinancialServicePaymentType financialServicePaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();
            SetupResult.For(financialServicePaymentType.Key).Return((int)FinancialServicePaymentTypes.SubsidyPayment);
            SetupResult.For(financialServiceBankAccount.FinancialServicePaymentType).Return(financialServicePaymentType);

            // Setup account.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);

            // Setup role.GeneralStatus.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);

            // Setup role.LegalEntity
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(role.LegalEntity).Return(legalEntity);

            // role.LegalEntity.Employment
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEventList<IEmployment> employments = new EventList<IEmployment>();
            employments.Add(Messages, employment);
            SetupResult.For(legalEntity.Employment).Return(employments);

            // Setup employment.IsConfirmed
            SetupResult.For(employment.IsConfirmed).Return(true);

            // Setup employment.EmploymentType
            IEmploymentType employmentType = _mockery.StrictMock<IEmploymentType>();
            SetupResult.For(employment.EmploymentType).Return(employmentType);
            SetupResult.For(employmentType.Key).Return((int)(int)EmploymentTypes.Salaried);

            // Setup employment.EmploymentStatus
            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(employment.EmploymentStatus).Return(employmentStatus);
            SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Current);

            ExecuteRule(rule, 1, mortgageLoanAccount);
        }

        #endregion MortgageLoanBankAccountPaymentTypeSubsidyAdd

        #region MortgageLoanDebitOrderAmount

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanDebitOrderAmountSuccess()
        {
            MortgageLoanDebitOrderAmount rule = new MortgageLoanDebitOrderAmount();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.FixedPayment
            SetupResult.For(mortgageLoanAccount.FixedPayment).Return(0.0);

            // Setup account.InstallmentSummary.TotalAmountDue
            IAccountInstallmentSummary accountInstallmentSummary = _mockery.StrictMock<IAccountInstallmentSummary>();
            SetupResult.For(mortgageLoanAccount.InstallmentSummary).Return(accountInstallmentSummary);
            SetupResult.For(accountInstallmentSummary.TotalAmountDue).Return(0.0);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanDebitOrderAmountFail()
        {
            MortgageLoanDebitOrderAmount rule = new MortgageLoanDebitOrderAmount();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.FixedPayment
            SetupResult.For(mortgageLoanAccount.FixedPayment).Return(0.0);

            // Setup account.InstallmentSummary.TotalAmountDue
            IAccountInstallmentSummary accountInstallmentSummary = _mockery.StrictMock<IAccountInstallmentSummary>();
            SetupResult.For(mortgageLoanAccount.InstallmentSummary).Return(accountInstallmentSummary);
            SetupResult.For(accountInstallmentSummary.TotalAmountDue).Return(5250.0);   // Anything > 0.0 to make it fail.

            ExecuteRule(rule, 1, mortgageLoanAccount);
        }

        #endregion MortgageLoanDebitOrderAmount

        #region MortgageLoanAccountClosedFurtherTransactions

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountClosedFurtherTransactionsSuccess()
        {
            MortgageLoanAccountClosedFurtherTransactions rule = new MortgageLoanAccountClosedFurtherTransactions();

            // SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);

            // Setup the correct object to pass along
            IFinancialTransaction loantransaction = _mockery.StrictMock<IFinancialTransaction>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();

            // SetupResult.For(loantransaction.LoanNumber).Return(-1);
            SetupResult.For(loantransaction.FinancialService).Return(financialService);

            // Setup financialServiceRepository.GetFinancialServiceByKey()
            // IFinancialServiceRepository financialServiceRepository = _mockery.StrictMock<IFinancialServiceRepository>();
            // MockCache.Add((typeof(IFinancialServiceRepository)).ToString(), financialServiceRepository);
            // IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            // SetupResult.For(financialServiceRepository.GetFinancialServiceByKey(1)).IgnoreArguments().Return(financialService);

            // Setup financialService.AccountStatus.Key
            IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
            SetupResult.For(financialService.AccountStatus).Return(accountStatus);
            SetupResult.For(accountStatus.Key).Return((int)AccountStatuses.Open);

            ExecuteRule(rule, 0, loantransaction);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountClosedFurtherTransactionsFail()
        {
            MortgageLoanAccountClosedFurtherTransactions rule = new MortgageLoanAccountClosedFurtherTransactions();

            // SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);

            // Setup the correct object to pass along
            IFinancialTransaction loantransaction = _mockery.StrictMock<IFinancialTransaction>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            SetupResult.For(loantransaction.FinancialService).Return(financialService);

            // Setup financialServiceRepository.GetFinancialServiceByKey()
            //IFinancialServiceRepository financialServiceRepository = _mockery.StrictMock<IFinancialServiceRepository>();
            //MockCache.Add((typeof(IFinancialServiceRepository)).ToString(), financialServiceRepository);

            //SetupResult.For(financialServiceRepository.GetFinancialServiceByKey(1)).IgnoreArguments().Return(financialService);

            // Setup financialService.AccountStatus.Key
            IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
            SetupResult.For(financialService.AccountStatus).Return(accountStatus);
            SetupResult.For(accountStatus.Key).Return((int)AccountStatuses.Closed);

            ExecuteRule(rule, 1, loantransaction);
        }

        #endregion MortgageLoanAccountClosedFurtherTransactions

        #region MortgageLoanAccountBondProperty

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountBondPropertySuccess()
        {
            MortgageLoanAccountBondProperty rule = new MortgageLoanAccountBondProperty();

            // Setup the correct object to pass along
            IBond bond = _mockery.StrictMock<IBond>();

            // Setup bond.MortgageLoan
            IEventList<IMortgageLoan> mortgageLoans = new EventList<IMortgageLoan>();
            IMortgageLoan mortgageLoan = _mockery.StrictMock<IMortgageLoan>();
            mortgageLoans.Add(Messages, mortgageLoan);
            SetupResult.For(bond.MortgageLoans).Return(mortgageLoans);

            // Setup mortgageLoan.AccountStatus.Key
            IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
            SetupResult.For(mortgageLoan.AccountStatus).Return(accountStatus);
            SetupResult.For(accountStatus.Key).Return((int)AccountStatuses.Open);

            // Setup mortgageLoan.FinancialServiceType.Key
            IFinancialServiceType financialServiceType = _mockery.StrictMock<IFinancialServiceType>();
            SetupResult.For(mortgageLoan.FinancialServiceType).Return(financialServiceType);
            SetupResult.For(financialServiceType.Key).Return((int)FinancialServiceTypes.VariableLoan);

            ExecuteRule(rule, 0, bond);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountBondPropertyFail()
        {
            MortgageLoanAccountBondProperty rule = new MortgageLoanAccountBondProperty();

            // Setup the correct object to pass along
            IBond bond = _mockery.StrictMock<IBond>();

            // Setup bond.MortgageLoan
            IEventList<IMortgageLoan> mortgageLoans = new EventList<IMortgageLoan>();
            IMortgageLoan mortgageLoan = _mockery.StrictMock<IMortgageLoan>();
            mortgageLoans.Add(Messages, mortgageLoan);
            mortgageLoans.Add(Messages, mortgageLoan);  // Make two open fin services.
            SetupResult.For(bond.MortgageLoans).Return(mortgageLoans);

            // Setup mortgageLoan.AccountStatus.Key
            IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
            SetupResult.For(mortgageLoan.AccountStatus).Return(accountStatus);
            SetupResult.For(accountStatus.Key).Return((int)AccountStatuses.Open);

            // Setup mortgageLoan.FinancialServiceType.Key
            IFinancialServiceType financialServiceType = _mockery.StrictMock<IFinancialServiceType>();
            SetupResult.For(financialServiceType.Key).Return((int)FinancialServiceTypes.VariableLoan);
            SetupResult.For(mortgageLoan.FinancialServiceType).Return(financialServiceType);

            ExecuteRule(rule, 1, bond);
        }

        #endregion MortgageLoanAccountBondProperty

        #region MortgageLoanActiveDebitOrdersMinimum

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanActiveDebitOrdersMinimumSuccess()
        {
            MortgageLoanActiveDebitOrdersMinimum rule = new MortgageLoanActiveDebitOrdersMinimum();

            // Setup the correct object to pass along
            IMortgageLoan mortgageLoan = _mockery.StrictMock<IMortgageLoan>();

            // Setup financialService.FinancialServiceBankAccounts
            IEventList<IFinancialServiceBankAccount> financialServiceBankAccounts = new EventList<IFinancialServiceBankAccount>();
            IFinancialServiceBankAccount financialServiceBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
            financialServiceBankAccounts.Add(Messages, financialServiceBankAccount);
            SetupResult.For(mortgageLoan.FinancialServiceBankAccounts).Return(financialServiceBankAccounts);

            // Setup financialServiceBankAccount.FinancialServicePaymentType.Key
            IFinancialServicePaymentType financialServicePaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();
            SetupResult.For(financialServicePaymentType.Key).Return((int)FinancialServicePaymentTypes.DebitOrderPayment);
            SetupResult.For(financialServiceBankAccount.FinancialServicePaymentType).Return(financialServicePaymentType);

            // Setup financialServiceBankAccount.GeneralStatus
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(financialServiceBankAccount.GeneralStatus).Return(generalStatus);

            ExecuteRule(rule, 0, mortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanActiveDebitOrdersMinimumFail()
        {
            MortgageLoanActiveDebitOrdersMinimum rule = new MortgageLoanActiveDebitOrdersMinimum();

            // Setup the correct object to pass along
            IMortgageLoan mortgageLoan = _mockery.StrictMock<IMortgageLoan>();

            // Setup financialService.FinancialServiceBankAccounts
            IEventList<IFinancialServiceBankAccount> financialServiceBankAccounts = new EventList<IFinancialServiceBankAccount>();
            IFinancialServiceBankAccount financialServiceBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
            financialServiceBankAccounts.Add(Messages, financialServiceBankAccount);
            financialServiceBankAccounts.Add(Messages, financialServiceBankAccount); // Add it twice to simulate failure
            SetupResult.For(mortgageLoan.FinancialServiceBankAccounts).Return(financialServiceBankAccounts);

            // Setup financialServiceBankAccount.FinancialServicePaymentType.Key
            IFinancialServicePaymentType financialServicePaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();
            SetupResult.For(financialServicePaymentType.Key).Return((int)FinancialServicePaymentTypes.DebitOrderPayment);
            SetupResult.For(financialServiceBankAccount.FinancialServicePaymentType).Return(financialServicePaymentType);

            // Setup financialServiceBankAccount.GeneralStatus
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(financialServiceBankAccount.GeneralStatus).Return(generalStatus);

            ExecuteRule(rule, 1, mortgageLoan);
        }

        #endregion MortgageLoanActiveDebitOrdersMinimum

        #region MortgageLoanAccountFixedDebitOrderValueSubsidy

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountFixedDebitOrderValueSubsidySuccess()
        {
            MortgageLoanAccountFixedDebitOrderValueSubsidy rule = new MortgageLoanAccountFixedDebitOrderValueSubsidy();

            // Setup the correct object to pass along
            IAccount account = _mockery.StrictMock<IAccount>();

            // Setup account.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);
            SetupResult.For(account.Roles).Return(roles);

            // Setup role.GeneralStatus.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);

            // role.LegalEntity.Employment
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            IEmploymentSalaried employment = _mockery.StrictMock<IEmploymentSalaried>();
            IEventList<IEmployment> employments = new EventList<IEmployment>();
            employments.Add(Messages, employment);
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            SetupResult.For(legalEntity.Employment).Return(employments);

            // Setup employment.EmploymentStatus.Key
            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Current);
            SetupResult.For(employment.EmploymentStatus).Return(employmentStatus);

            // Setup employment.EmploymentType.Key
            IEmploymentType employmentType = _mockery.StrictMock<IEmploymentType>();
            SetupResult.For(employmentType.Key).Return((int)EmploymentTypes.SalariedwithDeduction);
            SetupResult.For(employment.EmploymentType).Return(employmentType);

            // Setup employment.RequiresExtended
            SetupResult.For(employment.RequiresExtended).Return(true);

            // Setup employment.IsConfirmed
            SetupResult.For(employment.IsConfirmed).Return(true);

            // Setup supportsExtendedEmployment.ExtendedEmployment.ConfirmedIncome
            SetupResult.For(employment.ConfirmedIncome).Return(4000.00);

            // Setup account.FixedPayment
            SetupResult.For(account.FixedPayment).Return(11000.00);

            // Setup account.InstallmentSummary.TotalAmountDue
            IAccountInstallmentSummary accountInstallmentSummary = _mockery.StrictMock<IAccountInstallmentSummary>();
            SetupResult.For(account.InstallmentSummary).Return(accountInstallmentSummary);
            SetupResult.For(accountInstallmentSummary.TotalAmountDue).Return(15000.0);

            ExecuteRule(rule, 0, account);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountFixedDebitOrderValueSubsidyFail()
        {
            MortgageLoanAccountFixedDebitOrderValueSubsidy rule = new MortgageLoanAccountFixedDebitOrderValueSubsidy();

            // Setup the correct object to pass along
            IAccount account = _mockery.StrictMock<IAccount>();

            // Setup account.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);
            SetupResult.For(account.Roles).Return(roles);

            // Setup role.GeneralStatus.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);

            // role.LegalEntity.Employment
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            IEmploymentSalaried employment = _mockery.StrictMock<IEmploymentSalaried>();
            IEventList<IEmployment> employments = new EventList<IEmployment>();
            employments.Add(Messages, employment);
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            SetupResult.For(legalEntity.Employment).Return(employments);

            // Setup employment.EmploymentStatus.Key
            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Current);
            SetupResult.For(employment.EmploymentStatus).Return(employmentStatus);

            // Setup employment.EmploymentType.Key
            IEmploymentType employmentType = _mockery.StrictMock<IEmploymentType>();
            SetupResult.For(employmentType.Key).Return((int)EmploymentTypes.SalariedwithDeduction);
            SetupResult.For(employment.EmploymentType).Return(employmentType);

            // Setup employment.RequiresExtended
            SetupResult.For(employment.RequiresExtended).Return(true);

            // Setup employment.IsConfirmed
            SetupResult.For(employment.IsConfirmed).Return(true);

            SetupResult.For(employment.ConfirmedIncome).Return(7000.00);

            // Setup account.FixedPayment
            SetupResult.For(account.FixedPayment).Return(9000.00);

            // Setup account.InstallmentSummary.TotalAmountDue
            IAccountInstallmentSummary accountInstallmentSummary = _mockery.StrictMock<IAccountInstallmentSummary>();
            SetupResult.For(account.InstallmentSummary).Return(accountInstallmentSummary);
            SetupResult.For(accountInstallmentSummary.TotalAmountDue).Return(15000.0);

            ExecuteRule(rule, 1, account);
        }

        #endregion MortgageLoanAccountFixedDebitOrderValueSubsidy

        #region MortgageLoanAccountFixedDebitOrderBank

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountFixedDebitOrderBankSuccess()
        {
            MortgageLoanAccountFixedDebitOrderBank rule = new MortgageLoanAccountFixedDebitOrderBank();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup account.FixedPayment
            SetupResult.For(mortgageLoanAccount.FixedPayment).Return(9000.00);

            // Setup mortgageLoanAccount.FinancialServices
            IEventList<IFinancialService> financialServices = new EventList<IFinancialService>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            financialServices.Add(Messages, financialService);
            SetupResult.For(mortgageLoanAccount.FinancialServices).Return(financialServices);

            // Setup financialService.FinancialServiceBankAccounts
            IEventList<IFinancialServiceBankAccount> financialServiceBankAccounts = new EventList<IFinancialServiceBankAccount>();
            IFinancialServiceBankAccount financialServiceBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
            financialServiceBankAccounts.Add(Messages, financialServiceBankAccount);
            SetupResult.For(financialService.FinancialServiceBankAccounts).Return(financialServiceBankAccounts);

            // Setup financialServiceBankAccount.FinancialServicePaymentType.Key
            IFinancialServicePaymentType financialServicePaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();
            SetupResult.For(financialServicePaymentType.Key).Return((int)FinancialServicePaymentTypes.DebitOrderPayment);
            SetupResult.For(financialServiceBankAccount.FinancialServicePaymentType).Return(financialServicePaymentType);

            // Setup financialServiceBankAccount.GeneralStatus
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(financialServiceBankAccount.GeneralStatus).Return(generalStatus);

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);

            // Setup role.GeneralStatus
            SetupResult.For(role.GeneralStatus).Return(generalStatus);

            // Setup role.LegalEntity.LegalEntityBankAccounts
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            ILegalEntityBankAccount legalEntityBankAccount = _mockery.StrictMock<ILegalEntityBankAccount>();
            IEventList<ILegalEntityBankAccount> legalEntityBankAccounts = new EventList<ILegalEntityBankAccount>();
            legalEntityBankAccounts.Add(Messages, legalEntityBankAccount);
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            SetupResult.For(legalEntity.LegalEntityBankAccounts).Return(legalEntityBankAccounts);

            // Setup legalEntityBankAccounts.BankAccount.Key
            IBankAccount bankAccount = _mockery.StrictMock<IBankAccount>();
            SetupResult.For(legalEntityBankAccount.BankAccount).Return(bankAccount);
            SetupResult.For(bankAccount.Key).Return(2);

            // Setup financialServiceBankAccount.BankAccount.Key
            SetupResult.For(financialServiceBankAccount.BankAccount).Return(bankAccount);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountFixedDebitOrderBankFail()
        {
            MortgageLoanAccountFixedDebitOrderBank rule = new MortgageLoanAccountFixedDebitOrderBank();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup account.FixedPayment
            SetupResult.For(mortgageLoanAccount.FixedPayment).Return(9000.00);

            // Setup mortgageLoanAccount.FinancialServices
            IEventList<IFinancialService> financialServices = new EventList<IFinancialService>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            financialServices.Add(Messages, financialService);
            SetupResult.For(mortgageLoanAccount.FinancialServices).Return(financialServices);

            // Setup financialService.FinancialServiceBankAccounts
            IEventList<IFinancialServiceBankAccount> financialServiceBankAccounts = new EventList<IFinancialServiceBankAccount>();
            IFinancialServiceBankAccount financialServiceBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
            financialServiceBankAccounts.Add(Messages, financialServiceBankAccount);
            SetupResult.For(financialService.FinancialServiceBankAccounts).Return(financialServiceBankAccounts);

            // Setup financialServiceBankAccount.FinancialServicePaymentType.Key
            IFinancialServicePaymentType financialServicePaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();
            SetupResult.For(financialServicePaymentType.Key).Return((int)FinancialServicePaymentTypes.DebitOrderPayment);
            SetupResult.For(financialServiceBankAccount.FinancialServicePaymentType).Return(financialServicePaymentType);

            // Setup financialServiceBankAccount.GeneralStatus
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(financialServiceBankAccount.GeneralStatus).Return(generalStatus);

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);

            // Setup role.GeneralStatus
            SetupResult.For(role.GeneralStatus).Return(generalStatus);

            // Setup role.LegalEntity.LegalEntityBankAccounts
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            ILegalEntityBankAccount legalEntityBankAccount = _mockery.StrictMock<ILegalEntityBankAccount>();
            IEventList<ILegalEntityBankAccount> legalEntityBankAccounts = new EventList<ILegalEntityBankAccount>();
            legalEntityBankAccounts.Add(Messages, legalEntityBankAccount);
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            SetupResult.For(legalEntity.LegalEntityBankAccounts).Return(legalEntityBankAccounts);

            // Setup legalEntityBankAccounts.BankAccount.Key
            IBankAccount bankAccount = _mockery.StrictMock<IBankAccount>();
            SetupResult.For(legalEntityBankAccount.BankAccount).Return(bankAccount);
            SetupResult.For(bankAccount.Key).Return(2);

            // Setup financialServiceBankAccount.BankAccount.Key
            IBankAccount bankAccountFS = _mockery.StrictMock<IBankAccount>();
            SetupResult.For(bankAccountFS.Key).Return(1);
            SetupResult.For(financialServiceBankAccount.BankAccount).Return(bankAccountFS);

            ExecuteRule(rule, 1, mortgageLoanAccount);
        }

        [NUnit.Framework.Test]
        public void ApplicationConditionMandatoryNullConditionSetTestFail()
        {
            ApplicationConditionMandatory rule = new ApplicationConditionMandatory(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(appML.Key).Return(1);
            SetupResult.For(appML.ApplicationConditions).Return(null);

            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            SetupResult.For(appType.Key).Return(1);
            SetupResult.For(appML.ApplicationType).Return(appType);

            ExecuteRule(rule, 1, appML);
        }

        [NUnit.Framework.Test]
        public void ApplicationConditionMandatoryNoConditionsTestFail()
        {
            ApplicationConditionMandatory rule = new ApplicationConditionMandatory(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
            IEventList<IApplicationCondition> appCons = new EventList<IApplicationCondition>();

            SetupResult.For(appML.Key).Return(1);
            SetupResult.For(appML.ApplicationConditions).Return(appCons);

            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            SetupResult.For(appType.Key).Return(1);
            SetupResult.For(appML.ApplicationType).Return(appType);

            ExecuteRule(rule, 1, appML);
        }

        [NUnit.Framework.Test]
        public void ApplicationConditionMandatoryConditionsTestPass()
        {
            ApplicationConditionMandatory rule = new ApplicationConditionMandatory(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
            IEventList<IApplicationCondition> appCons = new EventList<IApplicationCondition>();
            IApplicationCondition appcon = _mockery.StrictMock<IApplicationCondition>();
            appCons.Add(Messages, appcon);

            SetupResult.For(appML.Key).Return(1);
            SetupResult.For(appML.ApplicationConditions).Return(appCons);

            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            SetupResult.For(appType.Key).Return(7);
            SetupResult.For(appML.ApplicationType).Return(appType);

            ExecuteRule(rule, 1, appML);
        }

        [NUnit.Framework.Test]
        public void ApplicationConditionMandatoryConditions_TestSingle()
        {
            using (new SessionScope())
            {
                ApplicationConditionMandatory rule = new ApplicationConditionMandatory(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                IApplication app = appRepo.GetApplicationByKey(610370);

                ExecuteRule(rule, 0, app);
            }
        }

        #endregion MortgageLoanAccountFixedDebitOrderBank

        #endregion Account MortgageLoan Spec

        #region Mortgage Loan Term Test

        [NUnit.Framework.Test]
        public void ApplicationProductMortgageLoanTermPass()
        {
            //Test that term rule passes on a normal Mortgage Loan Product
            ApplicationProductMortgageLoanTerm _rule = new ApplicationProductMortgageLoanTerm();
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();

            IApplicationProductMortgageLoan appProduct = _mockery.StrictMock<IApplicationProductMortgageLoan>();
            SetupResult.For(app.CurrentProduct).Return(appProduct);
            SetupResult.For(appProduct.Term).Return(360);
            SetupResult.For(appProduct.ProductType).Return(SAHL.Common.Globals.Products.NewVariableLoan);
            ExecuteRule(_rule, 0, app);

            //Test that term rule passes on a VariFix Loan Product
            IApplicationProductVariFixLoan appProduct2 = _mockery.StrictMock<IApplicationProductVariFixLoan>();
            SetupResult.For(app.CurrentProduct).Return(appProduct2);
            SetupResult.For(appProduct2.Term).Return(240);
            SetupResult.For(appProduct2.ProductType).Return(SAHL.Common.Globals.Products.VariFixLoan);
            ExecuteRule(_rule, 0, app);
        }

        [NUnit.Framework.Test]
        public void ApplicationProductMortgageLoanTermFail()
        {
            //Test the term rule that fails on a normal Mortgage Loan Product
            ApplicationProductMortgageLoanTerm _rule = new ApplicationProductMortgageLoanTerm();
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductMortgageLoan appProduct = _mockery.StrictMock<IApplicationProductMortgageLoan>();

            SetupResult.For(app.CurrentProduct).Return(appProduct);
            SetupResult.For(appProduct.Term).Return(400);
            SetupResult.For(appProduct.ProductType).Return(SAHL.Common.Globals.Products.NewVariableLoan);
            ExecuteRule(_rule, 1, app);

            //Test the term rule that fails on a VariFix Loan Product
            IApplicationProductVariFixLoan appProductVf = _mockery.StrictMock<IApplicationProductVariFixLoan>();
            SetupResult.For(app.CurrentProduct).Return(appProductVf);
            SetupResult.For(appProductVf.Term).Return(300);
            SetupResult.For(appProductVf.ProductType).Return(SAHL.Common.Globals.Products.VariFixLoan);
            ExecuteRule(_rule, 1, app);
        }

        #endregion Mortgage Loan Term Test

        #region MortgageLoanSPVChangeCheck

        //[Test]
        //public void MortgageLoanSPVChangeCheckTestPass()
        //{
        //    using (new SessionScope())
        //    {
        //        MortgageLoanSPVChangeCheck rule = new MortgageLoanSPVChangeCheck();
        //        IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
        //        IAccount ac = _mockery.StrictMock<IAccount>();
        //        ISPV spv = _mockery.StrictMock<ISPV>();
        //        IMortgageLoanRepository mlRepo = _mockery.StrictMock<IMortgageLoanRepository>();
        //        //
        //        int SPVTerm = -1;
        //        int newTerm = -1;
        //        int newSPV = -1;
        //        //
        //        SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
        //        MockCache.Add(typeof(IMortgageLoanRepository).ToString(), mlRepo);
        //        //
        //        SetupResult.For(spv.Key).Return(1);
        //        SetupResult.For(spv.Description).Return("test");
        //        SetupResult.For(ac.SPV).Return(spv);
        //        SetupResult.For(ml.Account).Return(ac);
        //        SetupResult.For(ml.InitialInstallments).Return((short)1);
        //        SetupResult.For(ml.RemainingInstallments).Return((short)1);
        //        Expect.Call(delegate { mlRepo.LookUpPendingTermChangeDetailFromX2(out newSPV, out newTerm, 1); }).OutRef(1, 1);
        //        SetupResult.For(mlRepo.IsSPVTermWithinMax(SPVTerm, -1)).IgnoreArguments().Return(true);
        //        ExecuteRule(rule, 0, ml, 1);
        //    }
        //}

        //[Test]
        //public void MortgageLoanSPVChangeCheckTestFail()
        //{
        //    using (new SessionScope())
        //    {
        //        MortgageLoanSPVChangeCheck rule = new MortgageLoanSPVChangeCheck();
        //        IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
        //        IAccount account = _mockery.StrictMock<IAccount>();
        //        ISPV spv = _mockery.StrictMock<ISPV>();
        //        IMortgageLoanRepository mlRepo = _mockery.StrictMock<IMortgageLoanRepository>();
        //        //
        //        int SPVTerm = -1;
        //        int newTerm = -1;
        //        int newSPV  = -1;
        //        //
        //        SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
        //        MockCache.Add(typeof(IMortgageLoanRepository).ToString(), mlRepo);
        //        //
        //        SetupResult.For(spv.Key).Return(1);
        //        SetupResult.For(spv.Description).Return("test");
        //        SetupResult.For(account.SPV).Return(spv);
        //        SetupResult.For(ml.Account).Return(account);
        //        SetupResult.For(ml.InitialInstallments).Return((short)1);
        //        SetupResult.For(ml.RemainingInstallments).Return((short)1);
        //        //
        //        Expect.Call(delegate { mlRepo.LookUpPendingTermChangeDetailFromX2(out newSPV, out newTerm, 1); }).OutRef(1, 1);
        //        SetupResult.For(mlRepo.IsSPVTermWithinMax(SPVTerm, -1)).IgnoreArguments().Return(false);
        //        SetupResult.For(mlRepo.GetNewSPVKeyTermChange(1)).IgnoreArguments().Return(2);
        //        SetupResult.For(mlRepo.GetNewSPVDescription(1)).IgnoreArguments().Return("test");
        //        ExecuteRule(rule, 1, ml,1);
        //    }
        //}

        #endregion MortgageLoanSPVChangeCheck

        #region Account not in Debt Counselling

        /// <summary>
        /// Account not in Debt Counselling Pass
        /// </summary>
        [Test]
        public void AccountNotInDebtCounsellingPass()
        {
            var rule = new AccountNotInDebtCounselling();

            IAccount account = _mockery.StrictMock<IAccount>();
            SetupResult.For(account.UnderDebtCounselling).Return(true);

            ExecuteRule(rule, 0, account);
        }

        /// <summary>
        /// Account not in Debt Counselling Fail
        /// </summary>
        [Test]
        public void AccountNotInDebtCounsellingFail()
        {
            var rule = new AccountNotInDebtCounselling();

            IAccount account = _mockery.StrictMock<IAccount>();
            SetupResult.For(account.UnderDebtCounselling).Return(false);

            ExecuteRule(rule, 1, account);
        }

        #endregion Account not in Debt Counselling
    }
}