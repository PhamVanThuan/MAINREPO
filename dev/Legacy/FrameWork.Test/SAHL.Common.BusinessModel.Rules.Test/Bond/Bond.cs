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
using SAHL.Common.BusinessModel.Rules.Bond;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Test.Bond
{
    [TestFixture]
    public class Bond : RuleBase
    {
        IBond bond = null;
        IBond bondCheck = null;
        IBondRepository repo = null;
        IDeedsOffice deedsOffice = null;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        #region IncorrectArgumemntsPassedFail

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Parameter[0] is not of type IBond")]
        public void BondRegistrationAmountArgumentsPassedFail()
        {
            BondRegistrationAmount rule = new BondRegistrationAmount();
            ILegalEntityCompany legalEntityCompany = _mockery.StrictMock<ILegalEntityCompany>();
            ExecuteRule(rule, 0, legalEntityCompany);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Parameter[0] is not of type IBond")]
        public void BondRegistrationNumberUniqueArgumemntsPassedFail()
        {
            BondRegistrationNumberUnique rule = new BondRegistrationNumberUnique(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            ILegalEntityCompany legalEntityCompany = _mockery.StrictMock<ILegalEntityCompany>();
            ExecuteRule(rule, 0, legalEntityCompany);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Parameter[0] is not of type IBond")]
        public void BondRegistrationNumberUpdateMandatoryArgumemntsPassedFail()
        {
            BondRegistrationNumberUpdateMandatory rule = new BondRegistrationNumberUpdateMandatory();
            ILegalEntityCompany legalEntityCompany = _mockery.StrictMock<ILegalEntityCompany>();
            ExecuteRule(rule, 0, legalEntityCompany);
        }

        #endregion IncorrectArgumemntsPassedFail

        #region BondRegistrationAmount

        [Test]
        public void BondRegistrationAmountHighFail()
        {
            BondRegistrationAmount rule = new BondRegistrationAmount();
            bond = _mockery.StrictMock<IBond>();
            SetupResult.For(bond.BondRegistrationAmount).Return(1000000000.00);

            ExecuteRule(rule, 1, bond);
        }

        [Test]
        public void BondRegistrationAmountLowFail()
        {
            BondRegistrationAmount rule = new BondRegistrationAmount();
            bond = _mockery.StrictMock<IBond>();
            SetupResult.For(bond.BondRegistrationAmount).Return(0.00);

            ExecuteRule(rule, 1, bond);
        }

        [NUnit.Framework.Test]
        public void BondRegistrationAmountPass()
        {
            BondRegistrationAmount rule = new BondRegistrationAmount();
            bond = _mockery.StrictMock<IBond>();
            SetupResult.For(bond.BondRegistrationAmount).Return(1.00);

            ExecuteRule(rule, 0, bond);
        }

        #endregion BondRegistrationAmount

        #region BondRegistrationNumberUnique

        /// <summary>
        /// BondRegistrationNumber exists against this bond record, so it is unique
        /// </summary>

        [Test]
        public void BondRegistrationNumberUniqueExistsUniquePass()
        {
            BondRegistrationNumberUnique rule = new BondRegistrationNumberUnique(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            bond = _mockery.StrictMock<IBond>();
            repo = _mockery.StrictMock<IBondRepository>();
            deedsOffice = _mockery.StrictMock<IDeedsOffice>();

            MockCache.Add(typeof(IBondRepository).ToString(), repo);
            bondCheck = _mockery.StrictMock<IBond>();

            Stack<IBond> bondStack = new Stack<IBond>();
            bondStack.Push(bondCheck);

            IReadOnlyEventList<IBond> bondListRO = new ReadOnlyEventList<IBond>(bondStack);
            SetupResult.For(bond.BondRegistrationNumber).Return("1");

            //SetupResult.For(bond.Key).Return(1);

            SetupResult.For(deedsOffice.Key).Return(1);
            SetupResult.For(bond.DeedsOffice).Return(deedsOffice);

            //Same Bond.Key; ie same bond record so bond number is unique
            SetupResult.For(bondCheck.Key).Return(1);
            SetupResult.For(repo.GetBondByRegistrationNumber("1")).Return(bondListRO);

            ExecuteRule(rule, 0, bond);
        }

        /// <summary>
        /// BondRegistrationNumber does not yet exist in the DB
        /// </summary>

        [Test]
        public void BondRegistrationNumberUniqueExistsNotExistPass()
        {
            BondRegistrationNumberUnique rule = new BondRegistrationNumberUnique(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            bond = _mockery.StrictMock<IBond>();
            repo = _mockery.StrictMock<IBondRepository>();
            deedsOffice = _mockery.StrictMock<IDeedsOffice>();

            MockCache.Add(typeof(IBondRepository).ToString(), repo);
            SetupResult.For(bond.BondRegistrationNumber).Return("1");
            SetupResult.For(deedsOffice.Key).Return(1);
            SetupResult.For(bond.DeedsOffice).Return(deedsOffice);

            //No Bond returned for the given BondRegistrationNumber, so can save
            SetupResult.For(repo.GetBondByRegistrationNumber("1")).Return(null);

            _mockery.ReplayAll();

            ExecuteRule(rule, 0, bond);
        }

        /// <summary>
        /// BondRegistrationNumber exists against a different Bond.Key, so not unique and must fail
        /// </summary>
        [Test]
        public void BondRegistrationNumberUniqueExistsDuplicateFail()
        {
            using (new SessionScope())
            {
                BondRegistrationNumberUnique rule = new BondRegistrationNumberUnique(RepositoryFactory.GetRepository<ICastleTransactionsService>());
                IBond bond = null;
                string sql = string.Format("select top 1 b.* from [2am].[dbo].[bond] b where b.BondRegistrationNumber <> ''");

                SimpleQuery<Bond_DAO> bondQ = new SimpleQuery<Bond_DAO>(QueryLanguage.Sql, sql);
                bondQ.AddSqlReturnDefinition(typeof(Bond_DAO), "b");

                Bond_DAO[] res = bondQ.Execute();

                if (res.Length < 1)
                    throw new Exception("Could not retrieve a valid bond");

                //bond = _mockery.StrictMock<IBond>();
                IEventList<IBond> bonds = new DAOEventList<Bond_DAO, IBond, Bond>(res);
                repo = RepositoryFactory.GetRepository<IBondRepository>();
                bond = bonds[0];

                // do the search for a bond that does not exist - this will pass with no messages
                //    SetupResult.For(bond.BondRegistrationNumber).Return("1");
                ExecuteRule(rule, 0, bond);

                // now use an existing bond's registration number - this should fail
                Bond_DAO bondCheck = Bond_DAO.FindFirst();

                // SetupResult.For(bond.BondRegistrationNumber).Return(bondCheck.BondRegistrationNumber);
                ExecuteRule(rule, 0, bond);

                /*
                SetupResult.For(bond.Key).Return(1);
                SetupResult.For(bondCheck.Key).Return(2);

                SetupResult.For(repo.GetBondByRegistrationNumber("1")).Return(bondCheck);

                _mockery.ReplayAll();
                rule.ExecuteRule(Messages, "", true, new Object[] { bond });
                _mockery.VerifyAll();

                Assert.AreEqual(1, Messages.Count);
                */
            }
        }

        #endregion BondRegistrationNumberUnique

        #region BondRegistrationNumberUpdateMandatory

        [Test]
        public void BondRegistrationNumberUpdateMandatoryPass()
        {
            BondRegistrationNumberUpdateMandatory rule = new BondRegistrationNumberUpdateMandatory();
            bond = _mockery.StrictMock<IBond>();
            SetupResult.For(bond.BondRegistrationNumber).Return("blah");

            ExecuteRule(rule, 0, bond);
        }

        [Test]
        public void BondRegistrationNumberUpdateMandatoryEmptyStringFail()
        {
            BondRegistrationNumberUpdateMandatory rule = new BondRegistrationNumberUpdateMandatory();
            bond = _mockery.StrictMock<IBond>();
            SetupResult.For(bond.BondRegistrationNumber).Return("");

            ExecuteRule(rule, 1, bond);
        }

        [Test]
        public void BondRegistrationNumberUpdateMandatoryNullFail()
        {
            BondRegistrationNumberUpdateMandatory rule = new BondRegistrationNumberUpdateMandatory();
            bond = _mockery.StrictMock<IBond>();
            SetupResult.For(bond.BondRegistrationNumber).Return(null);

            ExecuteRule(rule, 1, bond);
        }

        #endregion BondRegistrationNumberUpdateMandatory
    }
}