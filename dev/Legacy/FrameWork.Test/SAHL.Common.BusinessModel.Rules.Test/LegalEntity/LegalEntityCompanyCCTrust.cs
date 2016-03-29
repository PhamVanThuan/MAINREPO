using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Rules.LegalEntity;

namespace SAHL.Common.BusinessModel.Rules.Test.LegalEntity
{
    [TestFixture]
    public class LegalEntityCompanyCCTrust : LegalEntityBase
    {
        ILegalEntityCloseCorporation leCC = null;
        ILegalEntityCompany lePTY = null;
        ILegalEntityTrust leTrust = null;
        IApplicationRole appRole = null;
        IApplicationRoleType roleType = null;
        IEventList<IApplicationRole> roleList = null;

        IApplicationRole ccRole = null;
        IApplicationRole ptyRole = null;
        IApplicationRole trustRole = null;

        [NUnit.Framework.SetUp()]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        #region LegalEntityCompanyCCTrustMandatoryTradingName

        [NUnit.Framework.Test]
        public void LegalEntityCompanyCCTrustMandatoryTradingNameTestPass()
        {
            LegalEntityCompanyCCTrustMandatoryTradingName rule = new LegalEntityCompanyCCTrustMandatoryTradingName();
            //CC
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, leCC);
            //Trust
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, leTrust);
            //Company
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, lePTY);

            //CC
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, ccRole);
            //Trust
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, trustRole);
            //Company
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, ptyRole);

        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void LegalEntityCompanyCCTrustMandatoryTradingNameTestFail()
        {
            LegalEntityCompanyCCTrustMandatoryTradingName rule = new LegalEntityCompanyCCTrustMandatoryTradingName();
            // CC
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, leCC);
            // Trust
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, leTrust);
            // Company
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, lePTY);

            // CC
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, ccRole);
            // Trust
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, ptyRole);
            // Company
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, trustRole);

        }

        #endregion

        #region LegalEntityCompanyCCTrustMandatoryContact
        [NUnit.Framework.Test]
        public void ContactNumberRequiredPass()
        {
            LegalEntityCompanyCCTrustMandatoryContact rule = new LegalEntityCompanyCCTrustMandatoryContact();
            leTrust = _mockery.StrictMock<ILegalEntityTrust>();
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, leTrust);
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, lePTY);
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, leCC);
        }

        [NUnit.Framework.Test]
        public void ContactNumberRequiredFail()
        {
            LegalEntityCompanyCCTrustMandatoryContact rule = new LegalEntityCompanyCCTrustMandatoryContact();
            leTrust = _mockery.StrictMock<ILegalEntityTrust>();
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, leTrust);
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, lePTY);
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, leCC);
        }


        #endregion

        #region ValidateUniqueRegistrationNumber
        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ValidateUniqueRegistrationNumber_NoArgumentsPassed()
        {
            ValidateUniqueRegistrationNumber rule = new ValidateUniqueRegistrationNumber();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ValidateUniqueRegistrationNumber_IncorrectArgumentsPassed()
        {
            ValidateUniqueRegistrationNumber rule = new ValidateUniqueRegistrationNumber();

            // Setup an incorrect Argument to pass -- the rule should accept either an ILegalEntityCompany/ILegalEntityCloseCorporation or ILegalEntityTrust
            ILegalEntityUnknown legalEntityUnknown = _mockery.StrictMock<ILegalEntityUnknown>();
            ExecuteRule(rule, 0, legalEntityUnknown);
        }

        [NUnit.Framework.Test]
        public void ValidateUniqueRegistrationNumber_Success_UniqueRegistrationNumber()
        {
            ValidateUniqueRegistrationNumber rule = new ValidateUniqueRegistrationNumber();

            // Setup an correct Argument to pass 
            ILegalEntityCompany legalEntityCompany = _mockery.StrictMock<ILegalEntityCompany>();
            ILegalEntityType legalEntityType = _mockery.StrictMock<ILegalEntityType>();

            // Setup the result
            SetupResult.For(legalEntityType.Key).Return((int)SAHL.Common.Globals.LegalEntityTypes.Company);
            SetupResult.For(legalEntityCompany.LegalEntityType).Return(legalEntityType);
            SetupResult.For(legalEntityCompany.RegistrationNumber).Return("xxYY123");

            // Execute the rule 
            ExecuteRule(rule, 0, legalEntityCompany);
        }

        [NUnit.Framework.Test]
        public void ValidateUniqueRegistrationNumber_Success_SameRegistrationNumberSameLegalEntity()
        {
            ValidateUniqueRegistrationNumber rule = new ValidateUniqueRegistrationNumber();

            // Setup an correct Argument to pass 
            ILegalEntityCompany legalEntityCompany = _mockery.StrictMock<ILegalEntityCompany>();
            ILegalEntityType legalEntityType = _mockery.StrictMock<ILegalEntityType>();

            // Look for the first legal entity Trust with a registration number
            string HQL = "from LegalEntityTrust_DAO trust where trust.RegistrationNumber <> ?";
            SimpleQuery<LegalEntityTrust_DAO> q = new SimpleQuery<LegalEntityTrust_DAO>(HQL, "");
            q.SetQueryRange(1);
            LegalEntityTrust_DAO[] res = q.Execute();
            if (res.Length <= 0)
                Assert.Ignore("No Trusts Available");

            //ILegalEntityTrust leTrust = new LegalEntityTrust(res[0]);

            // Setup the result
            SetupResult.For(legalEntityType.Key).Return((int)SAHL.Common.Globals.LegalEntityTypes.Company);
            SetupResult.For(legalEntityCompany.LegalEntityType).Return(legalEntityType);
            SetupResult.For(legalEntityCompany.RegistrationNumber).Return(res[0].RegistrationNumber);
            SetupResult.For(legalEntityCompany.Key).Return(res[0].Key);

            // Execute the rule 
            ExecuteRule(rule, 0, legalEntityCompany);
        }

        [NUnit.Framework.Test]
        public void ValidateUniqueRegistrationNumber_Failure()
        {
            ValidateUniqueRegistrationNumber rule = new ValidateUniqueRegistrationNumber();

            // Setup an correct Argument to pass 
            ILegalEntityCompany legalEntityCompany = _mockery.StrictMock<ILegalEntityCompany>();
            ILegalEntityType legalEntityType = _mockery.StrictMock<ILegalEntityType>();

            // Look for the first legal entity Trust with a registration number
            string HQL = "from LegalEntityTrust_DAO trust where trust.RegistrationNumber <> ?";
            SimpleQuery<LegalEntityTrust_DAO> q = new SimpleQuery<LegalEntityTrust_DAO>(HQL, "");
            q.SetQueryRange(1);
            LegalEntityTrust_DAO[] res = q.Execute();
            if (res.Length <= 0)
                Assert.Ignore("No Trusts Available");

            //ILegalEntityTrust leTrust = new LegalEntityTrust(res[0]);   

            // Setup the result
            SetupResult.For(legalEntityType.Key).Return((int)SAHL.Common.Globals.LegalEntityTypes.Company);
            SetupResult.For(legalEntityCompany.LegalEntityType).Return(legalEntityType);
            SetupResult.For(legalEntityCompany.RegistrationNumber).Return(res[0].RegistrationNumber);
            SetupResult.For(legalEntityCompany.Key).Return(0);

            // Execute the rule 
            ExecuteRule(rule, 1, legalEntityCompany);
        }
        #endregion

        #region LegalEntityCompanyCCTrustMandatoryRegisteredName

        [NUnit.Framework.Test]
        public void LegalEntityCompanyCCTrustMandatoryRegisteredNameTestPass()
        {
            LegalEntityCompanyCCTrustMandatoryRegisteredName rule = new LegalEntityCompanyCCTrustMandatoryRegisteredName();
            //CC
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, leCC);
            //Trust
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, leTrust);
            //Company
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, lePTY);

            //CC
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, ccRole);
            //Trust
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, trustRole);
            //Company
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, ptyRole);
        }

        [NUnit.Framework.Test]
        public void LegalEntityCompanyCCTrustMandatoryRegisteredNameTestFail()
        {
            LegalEntityCompanyCCTrustMandatoryRegisteredName rule = new LegalEntityCompanyCCTrustMandatoryRegisteredName();
            // CC
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, leCC);
            // Trust
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, leTrust);
            // Company
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, lePTY);

            // CC
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, ccRole);
            // Trust
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, ptyRole);
            // Company
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, trustRole);
        }

        #endregion

        #region LegalEntityCompanyCCTrustMandatoryRegistrationNumber

        [NUnit.Framework.Test]
        public void LegalEntityCompanyCCTrustMandatoryRegistrationNumberTestPass()
        {
            LegalEntityCompanyCCTrustMandatoryRegistrationNumber rule = new LegalEntityCompanyCCTrustMandatoryRegistrationNumber();
            //CC
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, leCC);
            //Trust
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, leTrust);
            //Company
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, lePTY);

            //CC
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, ccRole);
            //Trust
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, trustRole);
            //Company
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, ptyRole);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void LegalEntityCompanyCCTrustMandatoryRegistrationNumberTestFail()
        {

            LegalEntityCompanyCCTrustMandatoryRegistrationNumber rule = new LegalEntityCompanyCCTrustMandatoryRegistrationNumber();
            // CC
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, leCC);
            // Trust
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, leTrust);
            // Company
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, lePTY);

            // CC
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, ccRole);
            // Trust
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, ptyRole);
            // Company
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, trustRole);
        }
        
        #endregion

        #region LegalEntityCompanyCCTrustMandatoryTaxNumber

        [NUnit.Framework.Test]
        public void LegalEntityCompanyCCTrustMandatoryTaxNumberTestPass()
        {
            LegalEntityCompanyCCTrustMandatoryTaxNumber rule = new LegalEntityCompanyCCTrustMandatoryTaxNumber();
            //CC
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, leCC);
            //Trust
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, leTrust);
            //Company
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, lePTY);

            //CC
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, ccRole);
            //Trust
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, trustRole);
            //Company
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, ptyRole);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void LegalEntityCompanyCCTrustMandatoryTaxNumberTestFail()
        {

            LegalEntityCompanyCCTrustMandatoryTaxNumber rule = new LegalEntityCompanyCCTrustMandatoryTaxNumber();
            // CC
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, leCC);
            // Trust
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, leTrust);
            // Company
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, lePTY);

            // CC
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, ccRole);
            // Trust
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, ptyRole);
            // Company
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, trustRole);
        }

        #endregion

        #region LegalEntityCompanyCCTrustMandatoryDocumentLanguage

        [NUnit.Framework.Test]
        public void LegalEntityCompanyCCTrustMandatoryDocumentLanguageTestPass()
        {
            LegalEntityCompanyCCTrustMandatoryDocumentLanguage rule = new LegalEntityCompanyCCTrustMandatoryDocumentLanguage();
            //CC
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, leCC);
            //Trust
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, leTrust);
            //Company
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, lePTY);

            //CC
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, ccRole);
            //Trust
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, trustRole);
            //Company
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, ptyRole);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void LegalEntityCompanyCCTrustMandatoryDocumentLanguageTestFail()
        {

            LegalEntityCompanyCCTrustMandatoryDocumentLanguage rule = new LegalEntityCompanyCCTrustMandatoryDocumentLanguage();
            // CC
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, leCC);
            // Trust
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, leTrust);
            // Company
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, lePTY);

            // CC
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, ccRole);
            // Trust
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, ptyRole);
            // Company
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, trustRole);
        }


        #endregion

        #region LegalEntityCompanyCCTrustMandatoryLegalEntityStatus

        [NUnit.Framework.Test]
        public void LegalEntityCompanyCCTrustMandatoryLegalEntityStatusTestPass()
        {
            LegalEntityCompanyCCTrustMandatoryLegalEntityStatus rule = new LegalEntityCompanyCCTrustMandatoryLegalEntityStatus();
            //CC
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, leCC);
            //Trust
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, leTrust);
            //Company
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, lePTY);

            //CC
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, ccRole);
            //Trust
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, trustRole);
            //Company
            SetupToPassLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 0, ptyRole);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void LegalEntityCompanyCCTrustMandatoryLegalEntityStatusTestFail()
        {
            LegalEntityCompanyCCTrustMandatoryLegalEntityStatus rule = new LegalEntityCompanyCCTrustMandatoryLegalEntityStatus();
            // CC
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, leCC);
            // Trust
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, leTrust);
            // Company
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, lePTY);

            // CC
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, ccRole);
            // Trust
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, ptyRole);
            // Company
            SetupToFailLegalEntityCompanyCCTrustMandatory();
            ExecuteRule(rule, 1, trustRole);
        }

        #endregion
        
        #region SetupTestRequirements

        public void SetupToPassLegalEntityCompanyCCTrustMandatory()
        {
            //
            //LE SetUp
            //
            leCC = _mockery.StrictMock<ILegalEntityCloseCorporation>();
            lePTY = _mockery.StrictMock<ILegalEntityCompany>();
            leTrust = _mockery.StrictMock<ILegalEntityTrust>();
            //
            SetupResult.For(leCC.RegisteredName).Return("RegisteredName");
            SetupResult.For(leTrust.RegisteredName).Return("RegisteredName");
            SetupResult.For(lePTY.RegisteredName).Return("RegisteredName");
            //
            SetupResult.For(leCC.TradingName).Return("TradingName");
            SetupResult.For(leTrust.TradingName).Return("TradingName");
            SetupResult.For(lePTY.TradingName).Return("TradingName");
            //
            SetupResult.For(leCC.RegistrationNumber).Return("0000");
            SetupResult.For(leTrust.RegistrationNumber).Return("0000");
            SetupResult.For(lePTY.RegistrationNumber).Return("0000");
            //
            SetupResult.For(leCC.TaxNumber).Return("000");
            SetupResult.For(leTrust.TaxNumber).Return("000");
            SetupResult.For(lePTY.TaxNumber).Return("000");
            //
            SetupResult.For(lePTY.CellPhoneNumber).Return("123");
            SetupResult.For(lePTY.WorkPhoneCode).Return("");
            SetupResult.For(lePTY.WorkPhoneNumber).Return("");
            SetupResult.For(lePTY.HomePhoneCode).Return("");
            SetupResult.For(lePTY.HomePhoneNumber).Return("");
            //
            SetupResult.For(leCC.CellPhoneNumber).Return("123");
            SetupResult.For(leCC.WorkPhoneCode).Return("");
            SetupResult.For(leCC.WorkPhoneNumber).Return("");
            SetupResult.For(leCC.HomePhoneCode).Return("");
            SetupResult.For(leCC.HomePhoneNumber).Return("");
            //
            SetupResult.For(leTrust.CellPhoneNumber).Return("123");
            SetupResult.For(leTrust.WorkPhoneCode).Return("");
            SetupResult.For(leTrust.WorkPhoneNumber).Return("");
            SetupResult.For(leTrust.HomePhoneCode).Return("");
            SetupResult.For(leTrust.HomePhoneNumber).Return("");
            //
            appRole = _mockery.StrictMock<IApplicationRole>();
            roleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(roleType);
            roleList = new EventList<IApplicationRole>();
            roleList.Add(null, appRole);
            SetupResult.For(leCC.ApplicationRoles).Return(roleList);
            SetupResult.For(lePTY.ApplicationRoles).Return(roleList);
            SetupResult.For(leTrust.ApplicationRoles).Return(roleList);
            //
            ILegalEntityStatus legalEntityStatus = _mockery.StrictMock<ILegalEntityStatus>();
            SetupResult.For(legalEntityStatus.Key).Return(1);
            SetupResult.For(leCC.LegalEntityStatus).Return(legalEntityStatus);
            SetupResult.For(leTrust.LegalEntityStatus).Return(legalEntityStatus);
            SetupResult.For(lePTY.LegalEntityStatus).Return(legalEntityStatus);
            //
            ILanguage documentLanguage = _mockery.StrictMock<ILanguage>();
            SetupResult.For(documentLanguage.Key).Return(1);
            SetupResult.For(leCC.DocumentLanguage).Return(documentLanguage);
            SetupResult.For(leTrust.DocumentLanguage).Return(documentLanguage);
            SetupResult.For(lePTY.DocumentLanguage).Return(documentLanguage);
            //
            //Role SetUp
            //
            ccRole = _mockery.StrictMock<IApplicationRole>();
            ptyRole = _mockery.StrictMock<IApplicationRole>();
            trustRole = _mockery.StrictMock<IApplicationRole>();
            //
            SetupResult.For(ccRole.ApplicationRoleType).Return(roleType);
            SetupResult.For(ptyRole.ApplicationRoleType).Return(roleType);
            SetupResult.For(trustRole.ApplicationRoleType).Return(roleType);
            //
            SetupResult.For(ccRole.LegalEntity).Return(leCC);
            SetupResult.For(ptyRole.LegalEntity).Return(lePTY);
            SetupResult.For(trustRole.LegalEntity).Return(leTrust);

        }

        public void SetupToFailLegalEntityCompanyCCTrustMandatory()
        {
            //
            //LE SetUp
            //
            leCC = _mockery.StrictMock<ILegalEntityCloseCorporation>();
            lePTY = _mockery.StrictMock<ILegalEntityCompany>();
            leTrust = _mockery.StrictMock<ILegalEntityTrust>();
            //
            SetupResult.For(leCC.RegisteredName).Return(string.Empty);
            SetupResult.For(leTrust.RegisteredName).Return(string.Empty);
            SetupResult.For(lePTY.RegisteredName).Return(string.Empty);
            //
            SetupResult.For(leCC.TradingName).Return(string.Empty);
            SetupResult.For(leTrust.TradingName).Return(string.Empty);
            SetupResult.For(lePTY.TradingName).Return(string.Empty);
            //
            SetupResult.For(leCC.RegistrationNumber).Return(string.Empty);
            SetupResult.For(leTrust.RegistrationNumber).Return(string.Empty);
            SetupResult.For(lePTY.RegistrationNumber).Return(string.Empty);
            //
            SetupResult.For(leCC.TaxNumber).Return(string.Empty);
            SetupResult.For(leTrust.TaxNumber).Return(string.Empty);
            SetupResult.For(lePTY.TaxNumber).Return(string.Empty);
            //
            SetupResult.For(lePTY.CellPhoneNumber).Return("");
            SetupResult.For(lePTY.WorkPhoneCode).Return("");
            SetupResult.For(lePTY.WorkPhoneNumber).Return("");
            SetupResult.For(lePTY.HomePhoneCode).Return("");
            SetupResult.For(lePTY.HomePhoneNumber).Return("");
            //
            SetupResult.For(leCC.CellPhoneNumber).Return("");
            SetupResult.For(leCC.WorkPhoneCode).Return("");
            SetupResult.For(leCC.WorkPhoneNumber).Return("");
            SetupResult.For(leCC.HomePhoneCode).Return("");
            SetupResult.For(leCC.HomePhoneNumber).Return("");
            //
            SetupResult.For(leTrust.CellPhoneNumber).Return("");
            SetupResult.For(leTrust.WorkPhoneCode).Return("");
            SetupResult.For(leTrust.WorkPhoneNumber).Return("");
            SetupResult.For(leTrust.HomePhoneCode).Return("");
            SetupResult.For(leTrust.HomePhoneNumber).Return("");
            //
            appRole = _mockery.StrictMock<IApplicationRole>();
            roleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(roleType);
            roleList = new EventList<IApplicationRole>();
            roleList.Add(null,appRole);
            SetupResult.For(leCC.ApplicationRoles).Return(roleList);
            SetupResult.For(lePTY.ApplicationRoles).Return(roleList);
            SetupResult.For(leTrust.ApplicationRoles).Return(roleList);
            //
            SetupResult.For(leCC.LegalEntityStatus).Return(null);
            SetupResult.For(leTrust.LegalEntityStatus).Return(null);
            SetupResult.For(lePTY.LegalEntityStatus).Return(null);
            //
            SetupResult.For(leCC.DocumentLanguage).Return(null);
            SetupResult.For(leTrust.DocumentLanguage).Return(null);
            SetupResult.For(lePTY.DocumentLanguage).Return(null);
            //
            //Role SetUp
            //
            ccRole = _mockery.StrictMock<IApplicationRole>();
            ptyRole = _mockery.StrictMock<IApplicationRole>();
            trustRole = _mockery.StrictMock<IApplicationRole>();
            //
            SetupResult.For(ccRole.ApplicationRoleType).Return(roleType);
            SetupResult.For(ptyRole.ApplicationRoleType).Return(roleType);
            SetupResult.For(trustRole.ApplicationRoleType).Return(roleType);
            //
            SetupResult.For(ccRole.LegalEntity).Return(leCC);
            SetupResult.For(ptyRole.LegalEntity).Return(lePTY);
            SetupResult.For(trustRole.LegalEntity).Return(leTrust);
        }

        #endregion
    }
}