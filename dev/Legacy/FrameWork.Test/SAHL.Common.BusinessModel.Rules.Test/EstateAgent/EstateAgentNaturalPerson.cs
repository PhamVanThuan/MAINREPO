using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Rules.EstateAgent;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Rules.Test.EstateAgent
{
    [TestFixture]
    public class EstateAgentMultipleAgenciesTest : RuleBase
    {
        [SetUp]
        public new void Setup()
        {
            base.Setup();
            Messages = new DomainMessageCollection();
        }

        [TearDown]
        public new void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void EstateAgentMultipleAgencies()
        {
            int expectedMessageCount = 0;
            EstateAgentMultipleAgencies rule = new EstateAgentMultipleAgencies(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            ILegalEntity leP = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(leP.Key).Return(0);
            SetupResult.For(leP.DisplayName).Return("test");
            ExecuteRule(rule, expectedMessageCount, leP);

            using (new SessionScope())
            {
                string sql = string.Format(@"SELECT     TOP (1) leos.LegalEntityKey
				FROM         LegalEntityOrganisationStructure AS leos INNER JOIN
				OrganisationStructure AS os ON leos.OrganisationStructureKey = os.OrganisationStructureKey
				WHERE     (os.ParentKey =
				(SELECT     ControlNumeric
				FROM          Control
				WHERE      (ControlDescription = 'EstateAgentChannelRoot')))
				ORDER BY leos.LegalEntityOrganisationStructureKey DESC");

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                if (obj != null)
                {
                    int leKey = Convert.ToInt32(obj);
                    ILegalEntity leF = _mockery.StrictMock<ILegalEntity>();
                    SetupResult.For(leF.Key).Return(leKey);
                    SetupResult.For(leF.DisplayName).Return("test");
                    ExecuteRule(rule, 1, leF);
                }
            }
        }

        [Test]
        public void EstateAgentDeleteParentTest()
        {
            int expectedMessageCount = 0;
            EstateAgentDeleteParent rule = new EstateAgentDeleteParent(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            IOrganisationStructure org = _mockery.StrictMock<IOrganisationStructure>();
            SetupResult.For(org.Key).Return(0);
            ExecuteRule(rule, expectedMessageCount, org);
        }
    }
}