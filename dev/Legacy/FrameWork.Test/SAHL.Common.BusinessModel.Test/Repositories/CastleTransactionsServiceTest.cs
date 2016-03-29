using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.DataAccess;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    class CastleTransactionsServiceTest : TestBase
    {
        [Test]
        public void SelectManyHQLTest()
        {
            ICastleTransactionsService castleTransactionService = new CastleTransactionsService();

            var hql = "from ApplicationRoleType_DAO o where o.Description=?";

            IList<IApplicationRoleType> appRoleTypes = castleTransactionService.Many<IApplicationRoleType>(QueryLanguages.Hql, hql, Databases.TwoAM, "Consultant");

            Assert.Greater(appRoleTypes.Count, 0);
        }

        [Test]
        public void SelectManySQLWithSqlReturnDefinitionTest()
        {
            string sql = @"select distinct [br].* 
			from [2am].[dbo].[UserOrganisationStructure] uos
			inner join [2am].[dbo].[aduser] ad
			on ad.ADUserKey = uos.ADUserKey
			inner join [2am].[dbo].[broker] br
			on br.ADUserKey = ad.ADUserKey
			inner join [2am].[dbo].[CapCreditBrokerToken] ccbr
			on br.BrokerKey = ccbr.BrokerKey
			where ad.generalstatuskey = 1 and uos.OrganisationStructureKey in (2017)";

            ICastleTransactionsService castleTransactionService = new CastleTransactionsService();

            IList<IBroker> brokerList = castleTransactionService.Many<IBroker>(QueryLanguages.Sql, sql, "br", Databases.TwoAM); 
        }

        [Test]
        public void SelectSingleHQLTest()
        {
            ICastleTransactionsService castleTransactionService = new CastleTransactionsService();

            var hql = "from ApplicationRoleType_DAO";

            IApplicationRoleType appRoleType = castleTransactionService.Single<IApplicationRoleType>(QueryLanguages.Hql, hql, Databases.TwoAM);

            Assert.IsNotNull(appRoleType);
        }


        [Test]
        public void SelectSingleSQLWithSqlRetrunDefinition()
        {
            string sql = @"select distinct [br].* 
			from [2am].[dbo].[UserOrganisationStructure] uos
			inner join [2am].[dbo].[aduser] ad
			on ad.ADUserKey = uos.ADUserKey
			inner join [2am].[dbo].[broker] br
			on br.ADUserKey = ad.ADUserKey
			inner join [2am].[dbo].[CapCreditBrokerToken] ccbr
			on br.BrokerKey = ccbr.BrokerKey
			where ad.generalstatuskey = 1 and uos.OrganisationStructureKey in (2017)";

            ICastleTransactionsService castleTransactionService = new CastleTransactionsService();

            IBroker broker = castleTransactionService.Single<IBroker>(QueryLanguages.Sql, sql, "br", Databases.TwoAM); 
        }


    }
}
