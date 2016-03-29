using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Rules.LegalEntity;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Test.LegalEntity
{
    [TestFixture]
    public class LegalEntityNaturalPerson : LegalEntityBase
    {
        ILegalEntityNaturalPerson lenp = null;
        IApplicationRole role = null;
        IApplicationMortgageLoan applicationMortgageLoan;

        [NUnit.Framework.SetUp()]
        public override void Setup()
        {
            base.Setup();
            applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        private void SetupLENP()
        {
            lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            ////SetupResult.For(LERepo.GetLegalEntityByKey(null, 1)).IgnoreArguments().Return(lenp);
        }

        [Test]
        public void LeadApplicants_Exclusions_Test()
        {
            using (new SessionScope())
            {
                List<int> _excludedRuleItems = new List<int>();

                RuleExclusionSet_DAO ruleExclusionSet = RuleExclusionSet_DAO.Find((int)SAHL.Common.Globals.RuleExclusionSets.LegalEntityLeadApplicants);

                foreach (RuleExclusion_DAO ruleExclusion in ruleExclusionSet.RuleExclusions)
                {
                    _excludedRuleItems.Add(ruleExclusion.RuleItemKey);
                }

                Assert.IsTrue(_excludedRuleItems.Count > 0);
            }
        }

        [Test]
        public void GetPreviousValuesTest()
        {
            using (new SessionScope(FlushAction.Never))
            {
                string sql = "select top 1 legalentitykey from legalentity where citizentypekey = 3";
                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                //select * from legalentity where citizentypekey = 3 --141331
                //select * from legalentity where citizentypekey = 1 --55836

                LegalEntityNaturalPersonUpdateToForeigner rule = new LegalEntityNaturalPersonUpdateToForeigner();
                ILegalEntityNaturalPerson lenp = null;

                int leKey = (int)obj;
                if (leKey > 0)
                {
                    lenp = (ILegalEntityNaturalPerson)LERepo.GetLegalEntityByKey((int)obj);
                    ICitizenType ct = lenp.CitizenType;
                    ExecuteRule(rule, 0, lenp);
                }

                sql = "select top 1 legalentitykey from legalentity where citizentypekey = 1";
                obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                leKey = (int)obj;
                if (leKey > 0)
                {
                    lenp = (ILegalEntityNaturalPerson)LERepo.GetLegalEntityByKey(55836);

                    ICitizenType ct = LookupRepository.CitizenTypes.ObjectDictionary[((int)CitizenTypes.Foreigner).ToString()];

                    lenp.CitizenType = ct;

                    ExecuteRule(rule, 1, lenp);
                }

                if (lenp != null)
                {
                    string fname = (string)lenp.GetPreviousValue<string, string>("FirstNames");

                    Assert.AreEqual(fname, lenp.FirstNames);

                    lenp.FirstNames = "some rubbish";
                    fname = (string)lenp.GetPreviousValue<string, string>("FirstNames");

                    Assert.AreNotEqual(fname, lenp.FirstNames);
                }
            }
        }

        [NUnit.Framework.Test]
        public void MandatoryIDNumberBirthDateOneDigitMonthCompareTestFail()
        {
            LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare rule = new LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare();

            ILegalEntityNaturalPerson lenp = CreateIApplicationMortgageLoan();

            DateTime dateOfBirth = Convert.ToDateTime("1955-04-28 00:00:00.000");
            SetupResult.For(lenp.DateOfBirth).Return(dateOfBirth);
            SetupResult.For(lenp.IDNumber).Return("3501210189082");
            SetupResult.For(lenp.DisplayName).Return("Thisisatest");
            ICitizenType ct = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(lenp.CitizenType).Return(ct);
            SetupResult.For(ct.Key).Return((int)CitizenTypes.SACitizen);
            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        [NUnit.Framework.Test]
        public void MandatoryNotNaturalPersonIDNumberBirthDateCompareTestPass()
        {
            LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare rule = new LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare();

            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

            Stack<IApplicationRole> appRoles = new Stack<IApplicationRole>();
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(applicationRole.ApplicationRoleType).Return(appRoleType);
            IApplicationRoleTypeGroup appRoleGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();
            SetupResult.For(appRoleGroup.Key).Return((int)OfferRoleTypeGroups.Client);
            SetupResult.For(appRoleType.ApplicationRoleTypeGroup).Return(appRoleGroup);
            ILegalEntityCompany leCO = _mockery.StrictMock<ILegalEntityCompany>();

            SetupResult.For(applicationRole.LegalEntity).Return(leCO);

            appRoles.Push(applicationRole);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);
            SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(applicationRoles);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        [NUnit.Framework.Test]
        public void MandatoryIDNumberBirthDateOneDigitDayCompareTestFail()
        {
            LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare rule = new LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare();

            ILegalEntityNaturalPerson lenp = CreateIApplicationMortgageLoan();

            DateTime dateOfBirth = Convert.ToDateTime("1955-10-08 00:00:00.000");
            SetupResult.For(lenp.DateOfBirth).Return(dateOfBirth);
            SetupResult.For(lenp.IDNumber).Return("3501210189082");
            SetupResult.For(lenp.DisplayName).Return("Thisisatest");
            ICitizenType ct = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(lenp.CitizenType).Return(ct);
            SetupResult.For(ct.Key).Return((int)CitizenTypes.SACitizen);
            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        private ILegalEntityNaturalPerson CreateIApplicationMortgageLoan()
        {
            Stack<IApplicationRole> appRoles = new Stack<IApplicationRole>();
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(applicationRole.ApplicationRoleType).Return(appRoleType);
            IApplicationRoleTypeGroup appRoleGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();
            SetupResult.For(appRoleGroup.Key).Return((int)OfferRoleTypeGroups.Client);
            SetupResult.For(appRoleType.ApplicationRoleTypeGroup).Return(appRoleGroup);
            ILegalEntityNaturalPerson lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(applicationRole.LegalEntity).Return(lenp);

            appRoles.Push(applicationRole);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);
            SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(applicationRoles);
            return lenp;
        }

        [NUnit.Framework.Test]
        public void MandatoryIDNumberBirthDateCompareTestFail()
        {
            LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare rule = new LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare();

            ILegalEntityNaturalPerson lenp = CreateIApplicationMortgageLoan();

            DateTime dateOfBirth = Convert.ToDateTime("1955-10-28 00:00:00.000");
            SetupResult.For(lenp.DateOfBirth).Return(dateOfBirth);
            SetupResult.For(lenp.IDNumber).Return("3501210189082");
            SetupResult.For(lenp.DisplayName).Return("hello");
            ICitizenType ct = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(lenp.CitizenType).Return(ct);
            SetupResult.For(ct.Key).Return((int)CitizenTypes.SACitizen);
            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        [NUnit.Framework.Test]
        public void MandatoryIDNumberBirthDateCompareTestPass()
        {
            LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare rule = new LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare();

            ILegalEntityNaturalPerson lenp = CreateIApplicationMortgageLoan();

            DateTime dateOfBirth = Convert.ToDateTime("1977-06-08 00:00:00.000");
            SetupResult.For(lenp.DateOfBirth).Return(dateOfBirth);
            SetupResult.For(lenp.IDNumber).Return("7706080189082");
            ICitizenType ct = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(lenp.CitizenType).Return(ct);
            SetupResult.For(ct.Key).Return((int)CitizenTypes.SACitizen);
            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        [NUnit.Framework.Test]
        public void MandatoryNoIDNumberBirthDateCompareTestPass()
        {
            LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare rule = new LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare();

            ILegalEntityNaturalPerson lenp = CreateIApplicationMortgageLoan();

            DateTime dateOfBirth = Convert.ToDateTime("1977-06-08 00:00:00.000");
            SetupResult.For(lenp.DateOfBirth).Return(dateOfBirth);
            SetupResult.For(lenp.IDNumber).Return("");
            ICitizenType ct = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(lenp.CitizenType).Return(ct);
            SetupResult.For(ct.Key).Return((int)CitizenTypes.SACitizen);
            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        [NUnit.Framework.Test]
        public void MandatoryNoBirthDateCompareTestPass()
        {
            LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare rule = new LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare();

            ILegalEntityNaturalPerson lenp = CreateIApplicationMortgageLoan();

            SetupResult.For(lenp.DateOfBirth).Return(null);
            SetupResult.For(lenp.IDNumber).Return("7706080189082");
            ICitizenType ct = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(lenp.CitizenType).Return(ct);
            SetupResult.For(ct.Key).Return((int)CitizenTypes.SACitizen);
            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        [NUnit.Framework.Test]
        public void MandatoryForeignResidentTestPass()
        {
            LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare rule = new LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare();

            ILegalEntityNaturalPerson lenp = CreateIApplicationMortgageLoan();

            DateTime dateOfBirth = Convert.ToDateTime("1977-06-08 00:00:00.000");
            SetupResult.For(lenp.DateOfBirth).Return(dateOfBirth);
            SetupResult.For(lenp.IDNumber).Return(null);
            ICitizenType ct = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(lenp.CitizenType).Return(ct);
            SetupResult.For(ct.Key).Return((int)CitizenTypes.Foreigner);
            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        [NUnit.Framework.Test]
        public void MandatoryNullCitizenTypeTestPass()
        {
            LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare rule = new LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare();

            ILegalEntityNaturalPerson lenp = CreateIApplicationMortgageLoan();

            DateTime dateOfBirth = Convert.ToDateTime("1977-06-08 00:00:00.000");
            SetupResult.For(lenp.DateOfBirth).Return(dateOfBirth);
            SetupResult.For(lenp.IDNumber).Return(null);
            SetupResult.For(lenp.CitizenType).Return(null);
            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        [NUnit.Framework.Test]
        public void EmploymentMandatoryTest()
        {
            using (new SessionScope())
            {
                LegalEntityEmploymentMandatory rule = new LegalEntityEmploymentMandatory(RepositoryFactory.GetRepository<ICastleTransactionsService>());
                IDbConnection con = Helper.GetSQLDBConnection();
                IDataReader reader = null;
                try
                {
                    object obj = null;

                    // Pass

                    #region Pass

                    string sqlQueryPass = "select top 1 ofr.OfferKey " +
                        "from LegalEntity le (nolock) " +
                        "join offerrole ofr on le.LegalEntityKey = ofr.LegalEntityKey " +
                        "where le.LegalEntityKey not in " +
                        "( " +
                        "    select ofr.LegalEntityKey " +
                        "    from offerrole ofr (nolock)  " +
                        "    join offerroletype ort (nolock) on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey  " +
                        "        and ort.OfferRoleTypeGroupKey = 3 " + //clients only
                        "        and ofr.OfferRoleTypeKey != 13 " +  //exclude life clients
                        "    join Employment emp (nolock) on ofr.LegalEntityKey = emp.LegalEntityKey  " +
                        "        and emp.EmploymentStatusKey = 1 " + //current
                        "        and emp.ConfirmedBasicIncome is not null  " + //same as business model IsConfirmed
                        "        and (emp.EmploymentTypeKey = 5 or emp.EmploymentTypeKey is null) " +
                        ")";

                    obj = Helper.ExecuteScalar(con, sqlQueryPass);

                    if (obj != null)
                    {
                        IApplication app = AppRepo.GetApplicationByKey((int)obj);

                        ExecuteRule(rule, 0, app);
                    }

                    #endregion Pass

                    // Fail

                    #region Fail

                    string sqlQueryFail = "select top 3 ofr.OfferKey, count(ofr.OfferKey) " +
                        "from offerrole ofr (nolock)   " +
                        "join offerroletype ort (nolock) on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey   " +
                        "    and ort.OfferRoleTypeGroupKey = 3 " + //clients only
                        "    and ofr.OfferRoleTypeKey != 13  " + //exclude life clients
                        "join Employment emp (nolock) on ofr.LegalEntityKey = emp.LegalEntityKey   " +
                        "    and emp.EmploymentStatusKey = 1 " + //current
                        "    and emp.ConfirmedBasicIncome is not null  " + //same as business model IsConfirmed
                        "    and (emp.EmploymentTypeKey = 5 or emp.EmploymentTypeKey is null)  " +
                        "group by ofr.OfferKey";

                    reader = DBHelper.ExecuteReader(sqlQueryFail);
                    while (reader.Read())
                    {
                        IApplication app = AppRepo.GetApplicationByKey(reader.GetInt32(0));
                        ExecuteRule(rule, reader.GetInt32(1), app);
                    }

                    #endregion Fail
                }
                finally
                {
                    if (reader != null)
                        reader.Dispose();

                    if (con != null)
                        con.Dispose();
                }
            }
        }

        [NUnit.Framework.Test]
        public void MandatorySaluationTestPass()
        {
            LegalEntityNaturalPersonMandatorySaluation rule = new LegalEntityNaturalPersonMandatorySaluation();
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, lenp);
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, role);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void MandatorySaluationTestFail()
        {
            LegalEntityNaturalPersonMandatorySaluation rule = new LegalEntityNaturalPersonMandatorySaluation();
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, lenp);
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, role);
        }

        [NUnit.Framework.Test]
        public void MandatoryInitialsTestPass()
        {
            LegalEntityNaturalPersonMandatoryInitials rule = new LegalEntityNaturalPersonMandatoryInitials();
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, lenp);
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, role);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void MandatoryInitialsTestFail()
        {
            LegalEntityNaturalPersonMandatoryInitials rule = new LegalEntityNaturalPersonMandatoryInitials();
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, lenp);
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, role);
        }

        [NUnit.Framework.Test]
        public void MandatoryGenderTestPass()
        {
            LegalEntityNaturalPersonMandatoryGender rule = new LegalEntityNaturalPersonMandatoryGender();
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, lenp);
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, role);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void MandatoryGenderTestFail()
        {
            LegalEntityNaturalPersonMandatoryGender rule = new LegalEntityNaturalPersonMandatoryGender();
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, lenp);
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, role);
        }

        [NUnit.Framework.Test]
        public void MandatoryMaritalStatusTestPass()
        {
            LegalEntityNaturalPersonMandatoryMaritalStatus rule = new LegalEntityNaturalPersonMandatoryMaritalStatus();
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, lenp);
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, role);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void MandatoryMaritalStatusTestFail()
        {
            LegalEntityNaturalPersonMandatoryMaritalStatus rule = new LegalEntityNaturalPersonMandatoryMaritalStatus();
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, lenp);
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, role);
        }

        [NUnit.Framework.Test]
        public void MandatoryPopulationGroupTestPass()
        {
            LegalEntityNaturalPersonMandatoryPopulationGroup rule = new LegalEntityNaturalPersonMandatoryPopulationGroup();
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, lenp);
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, role);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void MandatoryPopulationGroupTestFail()
        {
            LegalEntityNaturalPersonMandatoryPopulationGroup rule = new LegalEntityNaturalPersonMandatoryPopulationGroup();
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, lenp);
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, role);
        }

        [NUnit.Framework.Test]
        public void MandatoryEducationTestPass()
        {
            LegalEntityNaturalPersonMandatoryEducation rule = new LegalEntityNaturalPersonMandatoryEducation();
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, lenp);
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, role);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void MandatoryEducationTestFail()
        {
            LegalEntityNaturalPersonMandatoryEducation rule = new LegalEntityNaturalPersonMandatoryEducation();
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, lenp);
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, role);
        }

        [NUnit.Framework.Test]
        public void MandatoryCitizenTypeTestPass()
        {
            LegalEntityNaturalPersonMandatoryCitizenType rule = new LegalEntityNaturalPersonMandatoryCitizenType();
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, lenp);
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, role);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void MandatoryCitizenTypeTestFail()
        {
            LegalEntityNaturalPersonMandatoryCitizenType rule = new LegalEntityNaturalPersonMandatoryCitizenType();
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, lenp);
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, role);
        }

        [NUnit.Framework.Test]
        public void MandatoryIDNumberTestPass()
        {
            LegalEntityNaturalPersonMandatoryIDNumber rule = new LegalEntityNaturalPersonMandatoryIDNumber();
            SetupToPassIDNumber();
            ExecuteRule(rule, 0, lenp);
            SetupToPassIDNumber();
            ExecuteRule(rule, 0, role);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void MandatoryIDNumberTestFail()
        {
            LegalEntityNaturalPersonMandatoryIDNumber rule = new LegalEntityNaturalPersonMandatoryIDNumber();
            SetupToFailIDNumber();
            ExecuteRule(rule, 1, lenp);
            SetupToFailIDNumber();
            ExecuteRule(rule, 1, role);
        }

        [NUnit.Framework.Test]
        public void MandatoryDateOfBirthTestPass()
        {
            LegalEntityNaturalPersonMandatoryDateOfBirth rule = new LegalEntityNaturalPersonMandatoryDateOfBirth();
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, lenp);
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, role);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void MandatoryDateOfBirthTestFail()
        {
            LegalEntityNaturalPersonMandatoryDateOfBirth rule = new LegalEntityNaturalPersonMandatoryDateOfBirth();
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, lenp);
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, role);
        }

        [NUnit.Framework.Test]
        public void MandatoryPassportNumberTestPass()
        {
            LegalEntityNaturalPersonMandatoryPassportNumber rule = new LegalEntityNaturalPersonMandatoryPassportNumber();
            SetupToPassPassportNumber();
            ExecuteRule(rule, 0, lenp);

            SetupToPassPassportNumber();
            ExecuteRule(rule, 0, role);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void MandatoryPassportNumberTestFail()
        {
            LegalEntityNaturalPersonMandatoryPassportNumber rule = new LegalEntityNaturalPersonMandatoryPassportNumber();
            SetupToFailPassportNumber();
            ExecuteRule(rule, 1, lenp);
            SetupToFailPassportNumber();
            ExecuteRule(rule, 1, role);
        }

        [NUnit.Framework.Test]
        public void MandatoryHomeLanguageTestPass()
        {
            LegalEntityNaturalPersonMandatoryHomeLanguage rule = new LegalEntityNaturalPersonMandatoryHomeLanguage();
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, lenp);
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, role);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void MandatoryHomeLanguageTestFail()
        {
            LegalEntityNaturalPersonMandatoryHomeLanguage rule = new LegalEntityNaturalPersonMandatoryHomeLanguage();
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, lenp);
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, role);
        }

        [NUnit.Framework.Test]
        public void MandatoryDocumentLanguageTestPass()
        {
            LegalEntityNaturalPersonMandatoryDocumentLanguage rule = new LegalEntityNaturalPersonMandatoryDocumentLanguage();
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, lenp);
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, role);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void MandatoryDocumentLanguageTestFail()
        {
            LegalEntityNaturalPersonMandatoryDocumentLanguage rule = new LegalEntityNaturalPersonMandatoryDocumentLanguage();
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, lenp);
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, role);
        }

        [NUnit.Framework.Test]
        public void MandatoryLegalEntityStatusTestPass()
        {
            LegalEntityNaturalPersonMandatoryLegalEntityStatus rule = new LegalEntityNaturalPersonMandatoryLegalEntityStatus();
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, lenp);
            SetupToPassLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 0, role);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void MandatoryLegalEntityStatusTestFail()
        {
            LegalEntityNaturalPersonMandatoryLegalEntityStatus rule = new LegalEntityNaturalPersonMandatoryLegalEntityStatus();
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, lenp);
            SetupToFailLegalEntityNaturalPersonMandatory();
            ExecuteRule(rule, 1, role);
        }

        [NUnit.Framework.Test]
        public void ValidateIDNumberTestPass()
        {
            LegalEntityNaturalPersonValidateIDNumber rule = new LegalEntityNaturalPersonValidateIDNumber();
            SetupToPassValidateIDNumber();
            ExecuteRule(rule, 0, lenp);
            SetupToPassValidateIDNumber();
            ExecuteRule(rule, 0, role);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void ValidateIDNumberTestFail()
        {
            LegalEntityNaturalPersonValidateIDNumber rule = new LegalEntityNaturalPersonValidateIDNumber();
            SetupToFailValidateIDNumber();
            ExecuteRule(rule, 1, lenp);
            SetupToFailValidateIDNumber();
            ExecuteRule(rule, 1, role);
        }

        [NUnit.Framework.Test]
        public void ValidatePassportNumberTestPass()
        {
            LegalEntityNaturalPersonValidatePassportNumber rule = new LegalEntityNaturalPersonValidatePassportNumber();
            SetupToPassValidatePassportNumber();
            ExecuteRule(rule, 0, lenp);
            SetupToPassValidatePassportNumber();
            ExecuteRule(rule, 0, role);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void ValidatePassportNumberTestFail()
        {
            LegalEntityNaturalPersonValidatePassportNumber rule = new LegalEntityNaturalPersonValidatePassportNumber();
            SetupToFailValidatePassportNumber();
            ExecuteRule(rule, 1, lenp);
            SetupToFailValidatePassportNumber();
            ExecuteRule(rule, 1, role);
        }

        [NUnit.Framework.Test]
        public void IsPassportNumberUniqueTestPass()
        {
            LegalEntityNaturalPersonIsPassportNumberUnique rule = new LegalEntityNaturalPersonIsPassportNumberUnique();
            SetupToPassUniquePassportNumber();
            ExecuteRule(rule, 0, lenp);
            SetupToPassUniquePassportNumber();
            ExecuteRule(rule, 0, role);
        }

        [NUnit.Framework.Test]
        public void IsPassportNumberUniqueTestFail()
        {
            LegalEntityNaturalPersonIsPassportNumberUnique rule = new LegalEntityNaturalPersonIsPassportNumberUnique();
            SetupToFailUniquePassportNumber();
            ExecuteRule(rule, 1, lenp);
            SetupToFailUniquePassportNumber();
            ExecuteRule(rule, 1, role);
        }

        [NUnit.Framework.Test]
        public void IsIDNumberUniqueTestPass()
        {
            LegalEntityNaturalPersonIsIDNumberUnique rule = new LegalEntityNaturalPersonIsIDNumberUnique(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            SetupToPassUniqueIDNumber();
            ExecuteRule(rule, 0, lenp);
            SetupToPassUniqueIDNumber();
            ExecuteRule(rule, 0, role);
        }

        //[Ignore("Ignored as of 2014-06-03, Trying to get a build of 2.43.x green. These tests started failing for no apparent reason")]
        [NUnit.Framework.Test]
        public void IsIDNumberUniqueTestFail()
        {
            LegalEntityNaturalPersonIsIDNumberUnique rule = new LegalEntityNaturalPersonIsIDNumberUnique(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            var legalEntityNaturalPersonMock = SetupToFailUniqueIDNumber();
            ExecuteRule(rule, 1, legalEntityNaturalPersonMock);
            var applicationRoleMock = SetupToFailUniqueIDNumber();
            ExecuteRule(rule, 1, role);
        }

        #region Helper Methods To SetUp Mocks

        private void SetupToPassLegalEntityNaturalPersonMandatory()
        {
            //Legal Entity
            lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            //
            ISalutation salutation = _mockery.StrictMock<ISalutation>();
            SetupResult.For(salutation.Key).Return((int)SalutationTypes.Mr);
            SetupResult.For(lenp.Salutation).Return(salutation);

            //
            SetupResult.For(lenp.Initials).Return("initials");

            //
            SetupResult.For(lenp.FirstNames).Return("firstname");

            //
            SetupResult.For(lenp.Surname).Return("surname");

            //
            SetupResult.For(lenp.PreferredName).Return("Name");

            //
            IGender gender = _mockery.StrictMock<IGender>();
            SetupResult.For(gender.Key).Return(1);
            SetupResult.For(lenp.Gender).Return(gender);

            //
            IMaritalStatus maritalStatus = _mockery.StrictMock<IMaritalStatus>();
            SetupResult.For(maritalStatus.Key).Return(1);
            SetupResult.For(lenp.MaritalStatus).Return(maritalStatus);

            //
            IPopulationGroup populationGroup = _mockery.StrictMock<IPopulationGroup>();
            SetupResult.For(populationGroup.Key).Return(1);
            SetupResult.For(lenp.PopulationGroup).Return(populationGroup);

            //
            IEducation education = _mockery.StrictMock<IEducation>();
            SetupResult.For(education.Key).Return(1);
            SetupResult.For(lenp.Education).Return(education);

            //
            SetupResult.For(lenp.IDNumber).Return("8201015207080");

            //
            SetupResult.For(lenp.PassportNumber).Return("123456789");

            //
            SetupResult.For(lenp.DateOfBirth).Return(DateTime.Now);

            //
            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return(1);
            SetupResult.For(lenp.CitizenType).Return(citizenType);

            //
            ILanguage homeLanguage = _mockery.StrictMock<ILanguage>();
            SetupResult.For(homeLanguage.Key).Return(1);
            SetupResult.For(lenp.HomeLanguage).Return(homeLanguage);

            //
            ILanguage documentLanguage = _mockery.StrictMock<ILanguage>();
            SetupResult.For(documentLanguage.Key).Return(1);
            SetupResult.For(lenp.DocumentLanguage).Return(documentLanguage);

            //
            ILegalEntityStatus legalEntityStatus = _mockery.StrictMock<ILegalEntityStatus>();
            SetupResult.For(legalEntityStatus.Key).Return(1);
            SetupResult.For(lenp.LegalEntityStatus).Return(legalEntityStatus);

            //
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(roleType);
            IEventList<IApplicationRole> roleList = new EventList<IApplicationRole>();
            roleList.Add(null, appRole);
            SetupResult.For(lenp.ApplicationRoles).Return(roleList);

            //
            //Application Role
            role = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(role.LegalEntity).Return(lenp);
            SetupResult.For(role.ApplicationRoleType).Return(roleType);
        }

        private void SetupToFailLegalEntityNaturalPersonMandatory()
        {
            //Legal Entity
            lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            //
            SetupResult.For(lenp.Salutation).Return(null);

            //
            SetupResult.For(lenp.Initials).Return(string.Empty);

            //
            SetupResult.For(lenp.FirstNames).Return(null);

            //
            SetupResult.For(lenp.Surname).Return(null);

            //
            SetupResult.For(lenp.PreferredName).Return(string.Empty);

            //
            SetupResult.For(lenp.Gender).Return(null);

            //
            SetupResult.For(lenp.MaritalStatus).Return(null);

            //
            SetupResult.For(lenp.PopulationGroup).Return(null);

            //
            SetupResult.For(lenp.Education).Return(null);

            //
            SetupResult.For(lenp.IDNumber).Return(string.Empty);

            //
            SetupResult.For(lenp.PassportNumber).Return(string.Empty);

            //
            SetupResult.For(lenp.DateOfBirth).Return(null);

            //
            SetupResult.For(lenp.CitizenType).Return(null);

            //
            SetupResult.For(lenp.HomeLanguage).Return(null);

            //
            SetupResult.For(lenp.DocumentLanguage).Return(null);

            //
            SetupResult.For(lenp.LegalEntityStatus).Return(null);

            //
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(roleType);
            IEventList<IApplicationRole> roleList = new EventList<IApplicationRole>();
            roleList.Add(null, appRole);
            SetupResult.For(lenp.ApplicationRoles).Return(roleList);

            //
            //Application Role
            role = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(role.LegalEntity).Return(lenp);
            SetupResult.For(role.ApplicationRoleType).Return(roleType);
        }

        // Foreigner

        #endregion Helper Methods To SetUp Mocks

        #region Helper Method For Foreign Citizen (Passport Number)

        private void SetupToPassPassportNumber()
        {
            //Legal Entity
            lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            //
            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return((int)CitizenTypes.Foreigner);
            SetupResult.For(lenp.CitizenType).Return(citizenType);

            //
            SetupResult.For(lenp.PassportNumber).Return("123456789");

            //
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(roleType);
            IEventList<IApplicationRole> roleList = new EventList<IApplicationRole>();
            roleList.Add(null, appRole);
            SetupResult.For(lenp.ApplicationRoles).Return(roleList);

            //
            //Application Role
            role = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(role.LegalEntity).Return(lenp);
            SetupResult.For(role.ApplicationRoleType).Return(roleType);
        }

        private void SetupToFailPassportNumber()
        {
            //Legal Entity
            lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            //
            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return((int)CitizenTypes.Foreigner);
            SetupResult.For(lenp.CitizenType).Return(citizenType);

            //
            SetupResult.For(lenp.PassportNumber).Return(string.Empty);

            //
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(roleType);
            IEventList<IApplicationRole> roleList = new EventList<IApplicationRole>();
            roleList.Add(null, appRole);
            SetupResult.For(lenp.ApplicationRoles).Return(roleList);

            //
            //Application Role
            role = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(role.LegalEntity).Return(lenp);
            SetupResult.For(role.ApplicationRoleType).Return(roleType);
        }

        #endregion Helper Method For Foreign Citizen (Passport Number)

        #region Helper Method For ID Numbers

        private void SetupToPassIDNumber()
        {
            //Legal Entity
            lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            //
            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return((int)CitizenTypes.SACitizen);
            SetupResult.For(lenp.CitizenType).Return(citizenType);

            //
            SetupResult.For(lenp.IDNumber).Return("8101015207080");

            //
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(roleType);
            IEventList<IApplicationRole> roleList = new EventList<IApplicationRole>();
            roleList.Add(null, appRole);
            SetupResult.For(lenp.ApplicationRoles).Return(roleList);

            //
            //Application Role
            role = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(role.LegalEntity).Return(lenp);
            SetupResult.For(role.ApplicationRoleType).Return(roleType);
        }

        private void SetupToFailIDNumber()
        {
            //Legal Entity
            lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            //
            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return((int)CitizenTypes.SACitizen);
            SetupResult.For(lenp.CitizenType).Return(citizenType);

            //
            SetupResult.For(lenp.IDNumber).Return(string.Empty);

            //
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(roleType);
            IEventList<IApplicationRole> roleList = new EventList<IApplicationRole>();
            roleList.Add(null, appRole);
            SetupResult.For(lenp.ApplicationRoles).Return(roleList);

            //
            //Application Role
            role = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(role.LegalEntity).Return(lenp);
            SetupResult.For(role.ApplicationRoleType).Return(roleType);
        }

        #endregion Helper Method For ID Numbers

        #region Helper Methods For ID Numbers Validate

        private void SetupToPassValidateIDNumber()
        {
            //Legal Entity
            lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            //
            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return((int)CitizenTypes.SACitizen);
            SetupResult.For(lenp.CitizenType).Return(citizenType);

            //
            SetupResult.For(lenp.IDNumber).Return("8202255207080");

            //
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(roleType);
            IEventList<IApplicationRole> roleList = new EventList<IApplicationRole>();
            roleList.Add(null, appRole);
            SetupResult.For(lenp.ApplicationRoles).Return(roleList);

            //
            //Application Role
            role = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(role.LegalEntity).Return(lenp);
            SetupResult.For(role.ApplicationRoleType).Return(roleType);
        }

        private void SetupToFailValidateIDNumber()
        {
            //Legal Entity
            lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            //
            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return((int)CitizenTypes.SACitizen);
            SetupResult.For(lenp.CitizenType).Return(citizenType);

            //
            SetupResult.For(lenp.IDNumber).Return("1");

            //
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(roleType);
            IEventList<IApplicationRole> roleList = new EventList<IApplicationRole>();
            roleList.Add(null, appRole);
            SetupResult.For(lenp.ApplicationRoles).Return(roleList);

            //
            //Application Role
            role = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(role.LegalEntity).Return(lenp);
            SetupResult.For(role.ApplicationRoleType).Return(roleType);
        }

        #endregion Helper Methods For ID Numbers Validate

        #region Helper Method For Passport Number  Validate

        private void SetupToPassValidatePassportNumber()
        {
            //Legal Entity
            lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            //
            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return((int)CitizenTypes.Foreigner);
            SetupResult.For(lenp.CitizenType).Return(citizenType);

            //
            SetupResult.For(lenp.PassportNumber).Return("123456789");

            //
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(roleType);
            IEventList<IApplicationRole> roleList = new EventList<IApplicationRole>();
            roleList.Add(null, appRole);
            SetupResult.For(lenp.ApplicationRoles).Return(roleList);

            //
            //Application Role
            role = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(role.LegalEntity).Return(lenp);
            SetupResult.For(role.ApplicationRoleType).Return(roleType);
        }

        private void SetupToFailValidatePassportNumber()
        {
            //Legal Entity
            lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            //
            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return((int)CitizenTypes.Foreigner);
            SetupResult.For(lenp.CitizenType).Return(citizenType);

            //
            SetupResult.For(lenp.PassportNumber).Return("123");

            //
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(roleType);
            IEventList<IApplicationRole> roleList = new EventList<IApplicationRole>();
            roleList.Add(null, appRole);
            SetupResult.For(lenp.ApplicationRoles).Return(roleList);

            //
            //Application Role
            role = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(role.LegalEntity).Return(lenp);
            SetupResult.For(role.ApplicationRoleType).Return(roleType);
        }

        #endregion Helper Method For Passport Number  Validate

        #region Helper Method For Unique Passport Number

        private void SetupToPassUniquePassportNumber()
        {
            //
            string HQL = "from LegalEntityNaturalPerson_DAO leNP where leNP.PassportNumber <> ?";
            SimpleQuery<LegalEntityNaturalPerson_DAO> q = new SimpleQuery<LegalEntityNaturalPerson_DAO>(HQL, "");
            q.SetQueryRange(1);
            LegalEntityNaturalPerson_DAO[] res = q.Execute();
            if (res.Length <= 0)
                Assert.Ignore("No LegalEntity NaturalPerson Available with passport available");

            //
            //Legal Entity
            lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            //
            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return((int)CitizenTypes.Foreigner);
            SetupResult.For(lenp.CitizenType).Return(citizenType);

            //
            SetupResult.For(lenp.PassportNumber).Return(res[0].PassportNumber);
            SetupResult.For(lenp.Key).Return(res[0].Key);

            //
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(roleType);
            IEventList<IApplicationRole> roleList = new EventList<IApplicationRole>();
            roleList.Add(null, appRole);
            SetupResult.For(lenp.ApplicationRoles).Return(roleList);

            //
            //Application Role
            role = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(role.LegalEntity).Return(lenp);
            SetupResult.For(role.ApplicationRoleType).Return(roleType);
        }

        private void SetupToFailUniquePassportNumber()
        {
            //
            string HQL = "from LegalEntityNaturalPerson_DAO leNP where leNP.PassportNumber <> ?";
            SimpleQuery<LegalEntityNaturalPerson_DAO> q = new SimpleQuery<LegalEntityNaturalPerson_DAO>(HQL, "");
            q.SetQueryRange(1);
            LegalEntityNaturalPerson_DAO[] res = q.Execute();
            if (res.Length <= 0)
                Assert.Ignore("No LegalEntity NaturalPerson Available with passport available");

            //
            //Legal Entity
            lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            //
            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return((int)CitizenTypes.Foreigner);
            SetupResult.For(lenp.CitizenType).Return(citizenType);

            //
            SetupResult.For(lenp.PassportNumber).Return(res[0].PassportNumber);
            SetupResult.For(lenp.Key).Return(0);

            //
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(roleType);
            IEventList<IApplicationRole> roleList = new EventList<IApplicationRole>();
            roleList.Add(null, appRole);
            SetupResult.For(lenp.ApplicationRoles).Return(roleList);

            //
            //Application Role
            role = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(role.LegalEntity).Return(lenp);
            SetupResult.For(role.ApplicationRoleType).Return(roleType);
        }

        #endregion Helper Method For Unique Passport Number

        #region Helper Method For Unique ID Number

        private void SetupToPassUniqueIDNumber()
        {
            //
            string HQL = "from LegalEntityNaturalPerson_DAO leNP where leNP.IDNumber <> ?";
            SimpleQuery<LegalEntityNaturalPerson_DAO> q = new SimpleQuery<LegalEntityNaturalPerson_DAO>(HQL, "");
            q.SetQueryRange(1);
            LegalEntityNaturalPerson_DAO[] res = q.Execute();
            if (res.Length <= 0)
                Assert.Ignore("No LegalEntity NaturalPerson Available with ID Number available");

            //
            //Legal Entity
            lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            //
            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return((int)CitizenTypes.SACitizen);
            SetupResult.For(lenp.CitizenType).Return(citizenType);

            //
            SetupResult.For(lenp.IDNumber).Return(res[0].IDNumber);
            SetupResult.For(lenp.Key).Return(res[0].Key);

            //
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(roleType);
            IEventList<IApplicationRole> roleList = new EventList<IApplicationRole>();
            roleList.Add(null, appRole);
            SetupResult.For(lenp.ApplicationRoles).Return(roleList);

            //
            //Application Role
            role = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(role.LegalEntity).Return(lenp);
            SetupResult.For(role.ApplicationRoleType).Return(roleType);
        }

        private ILegalEntityNaturalPerson SetupToFailUniqueIDNumber()
        {
            //
            string HQL = "from LegalEntityNaturalPerson_DAO leNP where leNP.IDNumber <> ?";
            SimpleQuery<LegalEntityNaturalPerson_DAO> q = new SimpleQuery<LegalEntityNaturalPerson_DAO>(HQL, "");
            q.SetQueryRange(1);
            LegalEntityNaturalPerson_DAO[] res = q.Execute();
            if (res.Length <= 0)
                Assert.Ignore("No LegalEntity NaturalPerson Available with ID Number available");

            //
            //Legal Entity
            var legalEntityNaturalPersonMock = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            //
            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return((int)CitizenTypes.SACitizen);
            SetupResult.For(legalEntityNaturalPersonMock.CitizenType).Return(citizenType);

            //
            SetupResult.For(legalEntityNaturalPersonMock.IDNumber).Return(res[0].IDNumber);
            SetupResult.For(legalEntityNaturalPersonMock.Key).Return(0);

            //
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(roleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(roleType);
            IEventList<IApplicationRole> roleList = new EventList<IApplicationRole>();
            roleList.Add(null, appRole);
            SetupResult.For(legalEntityNaturalPersonMock.ApplicationRoles).Return(roleList);

            //
            //Application Role
            role = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(role.LegalEntity).Return(legalEntityNaturalPersonMock);
            SetupResult.For(role.ApplicationRoleType).Return(roleType);
            return legalEntityNaturalPersonMock;
        }

        #endregion Helper Method For Unique ID Number

        #region Repositories

        private IApplicationRepository appRepo;

        public IApplicationRepository AppRepo
        {
            get
            {
                if (appRepo == null)
                    appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                return appRepo;
            }
        }

        #endregion Repositories

        #region LegalEntityNaturalPersonUpdateProfileContactDetailsTest

        /// <summary>
        /// The following fields need to be validated: Work Phone Code and Work Phone Number, Cellphone no., Email Address
        /// </summary>
        [NUnit.Framework.Test]
        public void UpdateProfileContactDetailsTest()
        {
            // PASS - All details present
            LegalEntityNaturalPersonUpdateProfileContactDetailsHelper(0, "012", "012345678", "012345678", "test@test.com");

            // FAIL - some details null
            LegalEntityNaturalPersonUpdateProfileContactDetailsHelper(1, null, "012345678", "012345678", "test@test.com");

            // FAIL - some details null
            LegalEntityNaturalPersonUpdateProfileContactDetailsHelper(1, null, null, null, null);
        }

        /// <summary>
        /// Helper method to set up the expectations for the LegalEntityNaturalPersonUpdateProfileContactDetails test.
        /// </summary>
        /// <param name="gs"></param>
        private void LegalEntityNaturalPersonUpdateProfileContactDetailsHelper(int expectedMessageCount, string workPhoneCode, string workPhoneNo, string cellPhoneNo, string emailAddress)
        {
            LegalEntityNaturalPersonUpdateProfileContactDetails rule = new LegalEntityNaturalPersonUpdateProfileContactDetails();

            ILegalEntityNaturalPerson le = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(le.WorkPhoneCode).Return(workPhoneCode);
            SetupResult.For(le.WorkPhoneNumber).Return(workPhoneNo);
            SetupResult.For(le.CellPhoneNumber).Return(cellPhoneNo);
            SetupResult.For(le.EmailAddress).Return(emailAddress);

            ExecuteRule(rule, expectedMessageCount, le);
        }

        #endregion LegalEntityNaturalPersonUpdateProfileContactDetailsTest

        #region LegalEntityNaturalPersonUpdateProfilePreferedNameTest

        /// <summary>
        /// The following fields need to be validated: Work Phone Code and Work Phone Number, Cellphone no., Email Address
        /// </summary>
        [NUnit.Framework.Test]
        public void UpdateProfilePreferedNameTest()
        {
            IADUser aduser = _mockery.StrictMock<IADUser>();

            // PASS - All details present
            LegalEntityNaturalPersonUpdateProfilePreferedNameHelper(0, "Tom", aduser);

            // FAIL - some details null
            LegalEntityNaturalPersonUpdateProfilePreferedNameHelper(1, null, aduser);

            // FAIL - some details null
            LegalEntityNaturalPersonUpdateProfilePreferedNameHelper(1, " ", aduser);

            // PASS - All details present
            LegalEntityNaturalPersonUpdateProfilePreferedNameHelper(0, "Tom", null);
        }

        /// <summary>
        /// Helper method to set up the expectations for the LegalEntityNaturalPersonUpdateProfilePreferedName test.
        /// </summary>
        /// <param name="gs"></param>
        private void LegalEntityNaturalPersonUpdateProfilePreferedNameHelper(int expectedMessageCount, string preferedName, IADUser aduser)
        {
            LegalEntityNaturalPersonUpdateProfilePreferedName rule = new LegalEntityNaturalPersonUpdateProfilePreferedName();

            ILegalEntityNaturalPerson le = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(le.Key).Return(1);

            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            IOrganisationStructureRepository osr = _mockery.StrictMock<IOrganisationStructureRepository>();
            MockCache.Add((typeof(IOrganisationStructureRepository)).ToString(), osr);
            if (aduser != null)
            {
                IGeneralStatus genStat = _mockery.StrictMock<IGeneralStatus>();
                SetupResult.For(genStat.Key).Return((int)GeneralStatuses.Active);
                SetupResult.For(aduser.GeneralStatusKey).Return(genStat);
            }
            SetupResult.For(osr.GetAdUserByLegalEntityKey(1)).IgnoreArguments().Return(aduser);

            SetupResult.For(le.PreferredName).Return(preferedName);

            _mockery.ReplayAll();

            ExecuteRule(rule, expectedMessageCount, le);
        }

        #endregion LegalEntityNaturalPersonUpdateProfilePreferedNameTest

        /// <summary>
        /// Legal Entity Natural Person Household Contributor Confirmed Income Test
        /// </summary>
        [NUnit.Framework.Test]
        public void HouseholdContributorConfirmedIncomeTest()
        {
            //Fail
            LegalEntityNaturalPersonHouseholdContributorConfirmedIncomeHelper(OfferRoleAttributeTypes.IncomeContributor, EmploymentStatuses.Previous, 1.0d, 1);

            //Pass
            LegalEntityNaturalPersonHouseholdContributorConfirmedIncomeHelper(OfferRoleAttributeTypes.IncomeContributor, EmploymentStatuses.Previous, 0.0d, 1);

            //Fail
            LegalEntityNaturalPersonHouseholdContributorConfirmedIncomeHelper(OfferRoleAttributeTypes.IncomeContributor, EmploymentStatuses.Current, 0.0d, 1);

            //Pass
            LegalEntityNaturalPersonHouseholdContributorConfirmedIncomeHelper(OfferRoleAttributeTypes.IncomeContributor, EmploymentStatuses.Current, 1.0d, 0);
        }

        /// <summary>
        /// Legal Entity Natural Person Household Contributor Confirmed Income Helper
        /// </summary>
        /// <param name="offerRoleAttributeType"></param>
        /// <param name="employmentStatus"></param>
        /// <param name="confirmedBasicIncome"></param>
        /// <param name="expectedErrorCount"></param>
        private void LegalEntityNaturalPersonHouseholdContributorConfirmedIncomeHelper(OfferRoleAttributeTypes offerRoleAttributeTypeKey, EmploymentStatuses employmentStatusKey, double? confirmedBasicIncome, int expectedErrorCount)
        {
            LegalEntityNaturalPersonHouseholdContributorConfirmedIncome rule = new LegalEntityNaturalPersonHouseholdContributorConfirmedIncome();

            IApplication application = _mockery.StrictMock<IApplication>();
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleAttribute applicationRoleAttribute = _mockery.StrictMock<IApplicationRoleAttribute>();
            IApplicationRoleAttributeType applicationRoleAttributeType = _mockery.StrictMock<IApplicationRoleAttributeType>();

            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();

            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();

            SetupResult.For(employmentStatus.Key).Return((int)employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(employmentStatus);
            SetupResult.For(employment.ConfirmedBasicIncome).Return(confirmedBasicIncome);
            SetupResult.For(applicationRoleAttributeType.Key).Return((int)offerRoleAttributeTypeKey);
            SetupResult.For(applicationRoleAttribute.OfferRoleAttributeType).Return(applicationRoleAttributeType);

            IEventList<IEmployment> employments = new EventList<IEmployment>();
            employments.Add(Messages, employment);

            SetupResult.For(legalEntity.Employment).Return(employments);
            SetupResult.For(applicationRole.LegalEntity).Return(legalEntity);

            IEventList<IApplicationRoleAttribute> applicationRoleAttributes = new EventList<IApplicationRoleAttribute>();
            applicationRoleAttributes.Add(Messages, applicationRoleAttribute);
            SetupResult.For(applicationRole.ApplicationRoleAttributes).Return(applicationRoleAttributes);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(new List<IApplicationRole>() { applicationRole });

            SetupResult.For(application.GetApplicationRolesByGroup((OfferRoleTypeGroups.Client))).IgnoreArguments().Return(applicationRoles);

            ExecuteRule(rule, expectedErrorCount, application);
        }

        #region LegalEntityNaturalPersonItsAForeigner

        /// <summary>
        /// Legal Entity Natural Person It's a Foreigner Test
        /// </summary>
        [Test]
        public void ItsAForeignerTest()
        {
            LegalEntityNaturalPersonItsAForeignerHelper(0, CitizenTypes.NonResident);
            LegalEntityNaturalPersonItsAForeignerHelper(0, CitizenTypes.NonResidentCMAResident_Citizen);
            LegalEntityNaturalPersonItsAForeignerHelper(0, CitizenTypes.NonResidentConsulate);
            LegalEntityNaturalPersonItsAForeignerHelper(0, CitizenTypes.NonResidentContractWorker);
            LegalEntityNaturalPersonItsAForeignerHelper(0, CitizenTypes.NonResidentDiplomat);
            LegalEntityNaturalPersonItsAForeignerHelper(0, CitizenTypes.NonResidentHighCommissioner);
            LegalEntityNaturalPersonItsAForeignerHelper(0, CitizenTypes.NonResidentRefugee);
            LegalEntityNaturalPersonItsAForeignerHelper(0, CitizenTypes.SACitizen);
            LegalEntityNaturalPersonItsAForeignerHelper(0, CitizenTypes.SACitizenNonResident);

            LegalEntityNaturalPersonItsAForeignerHelper(1, CitizenTypes.Foreigner);
        }

        /// <summary>
        /// Check if the Legal Entity is a Foreigner
        /// </summary>
        /// <param name="expectedErrorCount"></param>
        /// <param name="citizenType"></param>
        private void LegalEntityNaturalPersonItsAForeignerHelper(int expectedErrorCount, CitizenTypes citizenType)
        {
            LegalEntityNaturalPersonItsAForeigner rule = new LegalEntityNaturalPersonItsAForeigner();
            ILegalEntityNaturalPerson legalEntity = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            ICitizenType citizenTypeForEntity = _mockery.StrictMock<ICitizenType>();

            SetupResult.For(citizenTypeForEntity.Key).Return((int)citizenType);
            SetupResult.For(legalEntity.Key).Return(0);
            SetupResult.For(legalEntity.CitizenType).Return(citizenTypeForEntity);

            ExecuteRule(rule, expectedErrorCount, legalEntity);
        }

        #endregion LegalEntityNaturalPersonItsAForeigner

        #region Legal Entity Natural Person Mandatory First Name

        /// <summary>
        /// Legal Entity Natural Person First Name Mandatory Test
        /// </summary>
        [Test]
        public void MandatoryFirstNameTest()
        {
            //Pass
            LegalEntityNaturalPersonMandatoryFirstNameHelper(0, "Samuel Theuns");

            //Fail
            LegalEntityNaturalPersonMandatoryFirstNameHelper(1, null);
        }

        /// <summary>
        /// Legal Entity Natural Person First Name Mandatory Helper
        /// </summary>
        /// <param name="expectedErrorCount"></param>
        /// <param name="firstName"></param>
        private void LegalEntityNaturalPersonMandatoryFirstNameHelper(int expectedErrorCount, string firstName)
        {
            LegalEntityNaturalPersonMandatoryFirstName rule = new LegalEntityNaturalPersonMandatoryFirstName();

            ILegalEntityNaturalPerson legalEntity = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(legalEntity.FirstNames).Return(firstName);

            ExecuteRule(rule, expectedErrorCount, legalEntity);
        }

        #endregion Legal Entity Natural Person Mandatory First Name

        #region Legal Entity Natural Person Mandatory Surname

        /// <summary>
        /// Legal Entity Natural Person Mandatory Surname Test
        /// </summary>
        [Test]
        public void MandatorySurnameTest()
        {
            //Pass
            LegalEntityNaturalPersonMandatorySurnameHelper(0, "Golden");

            //Fail
            LegalEntityNaturalPersonMandatorySurnameHelper(1, null);
        }

        /// <summary>
        /// Legal Entity Natural Person Mandatory Surname Helper
        /// </summary>
        /// <param name="expectedErrorCount"></param>
        /// <param name="surname"></param>
        private void LegalEntityNaturalPersonMandatorySurnameHelper(int expectedErrorCount, string surname)
        {
            LegalEntityNaturalPersonMandatorySurname rule = new LegalEntityNaturalPersonMandatorySurname();

            ILegalEntityNaturalPerson legalEntity = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(legalEntity.Surname).Return(surname);

            ExecuteRule(rule, expectedErrorCount, legalEntity);
        }

        #endregion Legal Entity Natural Person Mandatory Surname

        #region Legal Entity Natural Person Mandatory Email

        /// <summary>
        /// Legal Entity Natural Person Mandatory Email
        /// </summary>
        [Test]
        public void MandatoryEmailTest()
        {
            //Pass
            LegalEntityNaturalPersonMandatoryEmailHelper(0, "blah@ano.com");

            //Fail
            LegalEntityNaturalPersonMandatoryEmailHelper(1, null);
        }

        /// <summary>
        /// Legal Entity Natural Person Mandatory Surname Helper
        /// </summary>
        /// <param name="expectedErrorCount"></param>
        /// <param name="surname"></param>
        private void LegalEntityNaturalPersonMandatoryEmailHelper(int expectedErrorCount, string email)
        {
            LegalEntityNaturalPersonEmailRequired rule = new LegalEntityNaturalPersonEmailRequired();

            ILegalEntityNaturalPerson legalEntity = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(legalEntity.EmailAddress).Return(email);

            ExecuteRule(rule, expectedErrorCount, legalEntity);

            ILegalEntity le = _mockery.StrictMock<ILegalEntity>();
            ExecuteRule(rule, 0, le);

            ILegalEntityCompany lec = _mockery.StrictMock<ILegalEntityCompany>();
            ExecuteRule(rule, 0, lec);

            ILegalEntityCloseCorporation lecc = _mockery.StrictMock<ILegalEntityCloseCorporation>();
            ExecuteRule(rule, 0, lecc);
        }

        #endregion Legal Entity Natural Person Mandatory Email
    }
}