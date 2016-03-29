using System.Data;
using Castle.ActiveRecord;
using DomainService2;
using DomainService2.Workflow.DebtCounselling;
using NUnit.Framework;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;

namespace SAHL.DomainService.Test.DebtCounselling.CancelDebtCounselling_EXT
{
    [TestFixture]
    public class CancelDebtCounselling_EXTTests : DomainServiceTestBase
    {
        [Test]
        public void CancelDebtCounselling_EXTCommand_All_LE_under_dc_Test()
        {
            CancelDebtCounselling_EXTCommand command;
            IHandlesDomainServiceCommand<CancelDebtCounselling_EXTCommand> handler;

            string query = @"select top 1 i.id, dc.DebtCounsellingKey
                        from [2am].dbo.Account a
                        join [2am].dbo.FinancialService fs on fs.AccountKey = a.AccountKey
	                        and fs.FinancialServiceTypeKey = 1
	                        and fs.AccountStatusKey = 1
                        join [2am].debtcounselling.DebtCounselling dc on dc.AccountKey = a.AccountKey
	                        and dc.DebtCounsellingStatusKey = 1
                        JOIN [2am]..ExternalRole er (NOLOCK) ON er.GenericKey = dc.DebtCounsellingKey
	                        and er.GeneralStatusKey = 1
	                        and er.ExternalRoleTypeKey = 1 --Clients
	                        and er.GenericKeyTypeKey = 27 --DebtCounselling
                        join [2am]..[Role] r (nolock) on r.AccountKey = a.AccountKey
                        JOIN [2am]..LegalEntity le (NOLOCK) ON le.LegalEntityKey = r.LegalEntityKey
	                        and le.LegalEntityKey = er.LegalEntityKey
	                        and le.LegalEntityTypeKey = 2 --Natural Person
                        join x2.X2DATA.Debt_Counselling xdc on xdc.DebtCounsellingKey = dc.DebtCounsellingKey
                        join x2.X2.Instance i on i.id = xdc.InstanceID
	                        and i.ParentInstanceID is null
                        join x2.X2.[State] s on s.id = i.StateID
	                        and s.Type <> 5
                        join (select max(id) id, InstanceID, ADUserKey
		                        from x2.X2.WorkflowRoleAssignment
		                        where GeneralStatusKey = 1
		                        and WorkflowRoleTypeOrganisationStructureMappingKey = 6
		                        group by InstanceID, ADUserKey) wra on wra.InstanceID = i.id
                        join [2am]..aduser ad on ad.ADUserKey = wra.ADUserKey
                        left join [2am].dbo.Detail d on d.AccountKey = a.AccountKey
	                        and DetailTypeKey = 251
                        where a.RRR_ProductKey = 9
                        and a.RRR_OriginationSourceKey = 1
                        and dc.DebtCounsellingKey in (select DebtCounsellingKey 
								                        from [2am].debtcounselling.Proposal
								                        where ProposalStatusKey = 1
								                        and Accepted = 1)
                        and d.AccountKey is null
                        group by dc.DebtCounsellingKey, i.id
                        having count(distinct le.LegalEntityKey) = count(distinct er.LegalEntityKey)
                        order by dc.DebtCounsellingKey desc";

            long instanceID = 0;
            int debtCounsellingKey = 0;

            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                using (IDbCommand cmd = dbHelper.CreateCommand(query))
                {
                    using (IDataReader reader = dbHelper.ExecuteReader(cmd))
                    {
                        if (reader.Read())
                        {
                            instanceID = reader.GetInt64(0);
                            debtCounsellingKey = reader.GetInt32(1);
                        }
                    }
                }
            }

            if (instanceID > 0 && debtCounsellingKey > 0)
            {
                command = new CancelDebtCounselling_EXTCommand(instanceID, "Capture Recoveries Proposal", debtCounsellingKey);
                handler = loader.DomainServiceIOC.GetCommandHandler<CancelDebtCounselling_EXTCommand>();

                using (new TransactionScope(OnDispose.Rollback))
                {
                    handler.Handle(messages, command);
                }

                Assert.True(command.Result);
            }
            else
                Assert.Inconclusive("no data");
        }

        [Test]
        public void CancelDebtCounselling_EXTCommand_Not_All_LE_under_dc_Test()
        {
            CancelDebtCounselling_EXTCommand command;
            IHandlesDomainServiceCommand<CancelDebtCounselling_EXTCommand> handler;

            string query = @"select top 1 i.id, dc.DebtCounsellingKey
                        from [2am].dbo.Account a
                        join [2am].dbo.FinancialService fs on fs.AccountKey = a.AccountKey
	                        and fs.FinancialServiceTypeKey = 1
	                        and fs.AccountStatusKey = 1
                        join [2am].debtcounselling.DebtCounselling dc on dc.AccountKey = a.AccountKey
	                        and dc.DebtCounsellingStatusKey = 1
                        JOIN [2am]..ExternalRole er (NOLOCK) ON er.GenericKey = dc.DebtCounsellingKey
	                        and er.GeneralStatusKey = 1
	                        and er.ExternalRoleTypeKey = 1 --Clients
	                        and er.GenericKeyTypeKey = 27 --DebtCounselling
                        join [2am]..[Role] r (nolock) on r.AccountKey = a.AccountKey
                        JOIN [2am]..LegalEntity le (NOLOCK) ON le.LegalEntityKey = r.LegalEntityKey
	                        and le.LegalEntityTypeKey = 2 --Natural Person
                        join x2.X2DATA.Debt_Counselling xdc on xdc.DebtCounsellingKey = dc.DebtCounsellingKey
                        join x2.X2.Instance i on i.id = xdc.InstanceID
	                        and i.ParentInstanceID is null
                        join x2.X2.[State] s on s.id = i.StateID
	                        and s.Type <> 5
                        join (select max(id) id, InstanceID, ADUserKey
		                        from x2.X2.WorkflowRoleAssignment
		                        where GeneralStatusKey = 1
		                        and WorkflowRoleTypeOrganisationStructureMappingKey = 6
		                        group by InstanceID, ADUserKey) wra on wra.InstanceID = i.id
                        join [2am]..aduser ad on ad.ADUserKey = wra.ADUserKey
                        left join [2am].dbo.Detail d on d.AccountKey = a.AccountKey
	                        and DetailTypeKey = 251
                        where a.RRR_ProductKey = 9
                        and a.RRR_OriginationSourceKey = 1
                        and dc.DebtCounsellingKey in (select DebtCounsellingKey 
								                        from [2am].debtcounselling.Proposal
								                        where ProposalStatusKey = 1
								                        and Accepted = 1)
                        and d.AccountKey is null
                        group by dc.DebtCounsellingKey, i.id
                        having count(distinct le.LegalEntityKey) > 1
                        and count(distinct er.LegalEntityKey) = 1
                        order by dc.DebtCounsellingKey desc";

            long instanceID = 0;
            int debtCounsellingKey = 0;

            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                using (IDbCommand cmd = dbHelper.CreateCommand(query))
                {
                    using (IDataReader reader = dbHelper.ExecuteReader(cmd))
                    {
                        if (reader.Read())
                        {
                            instanceID = reader.GetInt64(0);
                            debtCounsellingKey = reader.GetInt32(1);
                        }
                    }
                }
            }

            if (instanceID > 0 && debtCounsellingKey > 0)
            {
                command = new CancelDebtCounselling_EXTCommand(instanceID, "Capture Recoveries Proposal", debtCounsellingKey);
                handler = loader.DomainServiceIOC.GetCommandHandler<CancelDebtCounselling_EXTCommand>();

                using (new TransactionScope(OnDispose.Rollback))
                {
                    handler.Handle(messages, command);
                }

                Assert.True(command.Result);
            }
            else
                Assert.Inconclusive("no data");
        }

        [Test]
        public void CancelDebtCounselling_EXTCommand_CompanyOrTrust_LE_under_dc_Test()
        {
            CancelDebtCounselling_EXTCommand command;
            IHandlesDomainServiceCommand<CancelDebtCounselling_EXTCommand> handler;

            string query = @"select top 1 i.id, dc.DebtCounsellingKey
                        from [2am].dbo.Account a
                        join [2am].dbo.FinancialService fs on fs.AccountKey = a.AccountKey
	                        and fs.FinancialServiceTypeKey = 1
	                        and fs.AccountStatusKey = 1
                        join [2am].debtcounselling.DebtCounselling dc on dc.AccountKey = a.AccountKey
	                        and dc.DebtCounsellingStatusKey = 1
                        JOIN [2am]..ExternalRole er (NOLOCK) ON er.GenericKey = dc.DebtCounsellingKey
	                        and er.GeneralStatusKey = 1
	                        and er.ExternalRoleTypeKey = 1 --Clients
	                        and er.GenericKeyTypeKey = 27 --DebtCounselling
                        join [2am]..[Role] r (nolock) on r.AccountKey = a.AccountKey
                        JOIN [2am]..LegalEntity le (NOLOCK) ON le.LegalEntityKey = r.LegalEntityKey
	                        and le.LegalEntityKey = er.LegalEntityKey
	                        and le.LegalEntityTypeKey in (3, 5) --Company, Trust
                        join x2.X2DATA.Debt_Counselling xdc on xdc.DebtCounsellingKey = dc.DebtCounsellingKey
                        join x2.X2.Instance i on i.id = xdc.InstanceID
	                        and i.ParentInstanceID is null
                        join x2.X2.[State] s on s.id = i.StateID
	                        and s.Type <> 5
                        join (select max(id) id, InstanceID, ADUserKey
		                        from x2.X2.WorkflowRoleAssignment
		                        where GeneralStatusKey = 1
		                        and WorkflowRoleTypeOrganisationStructureMappingKey = 6
		                        group by InstanceID, ADUserKey) wra on wra.InstanceID = i.id
                        join [2am]..aduser ad on ad.ADUserKey = wra.ADUserKey
                        left join [2am].dbo.Detail d on d.AccountKey = a.AccountKey
	                        and DetailTypeKey = 251
                        where a.RRR_ProductKey = 9
                        and a.RRR_OriginationSourceKey = 1
                        and dc.DebtCounsellingKey in (select DebtCounsellingKey 
								                        from [2am].debtcounselling.Proposal
								                        where ProposalStatusKey = 1
								                        and Accepted = 1)
                        and d.AccountKey is null
                        group by dc.DebtCounsellingKey, i.id
                        having count(distinct le.LegalEntityKey) = count(distinct er.LegalEntityKey)
                        order by dc.DebtCounsellingKey desc";

            long instanceID = 0;
            int debtCounsellingKey = 0;

            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                using (IDbCommand cmd = dbHelper.CreateCommand(query))
                {
                    using (IDataReader reader = dbHelper.ExecuteReader(cmd))
                    {
                        if (reader.Read())
                        {
                            instanceID = reader.GetInt64(0);
                            debtCounsellingKey = reader.GetInt32(1);
                        }
                    }
                }
            }

            if (instanceID > 0 && debtCounsellingKey > 0)
            {
                command = new CancelDebtCounselling_EXTCommand(instanceID, "Capture Recoveries Proposal", debtCounsellingKey);
                handler = loader.DomainServiceIOC.GetCommandHandler<CancelDebtCounselling_EXTCommand>();

                using (new TransactionScope(OnDispose.Rollback))
                {
                    handler.Handle(messages, command);
                }

                Assert.True(command.Result);
            }
            else
                Assert.Inconclusive("no data");
        }
    }
}
