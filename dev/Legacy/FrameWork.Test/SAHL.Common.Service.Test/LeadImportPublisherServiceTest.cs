using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Logging;
using SAHL.Common.Service.Interfaces;
using SAHL.Communication;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading;


namespace SAHL.Common.Service.Test
{
    public class TestIDNumber
    {
        public string idnumber { get; set; }
    }

    public class GenericMessageResult
    {
        public int BatchServiceKey { get; set; }

        public int Leads { get; set; }
    }

    [TestFixture]
    public class LeadImportPublisherServiceTest : TestBase
    {
        [Test]
        public void PublishLeadsForImport_FOR_LEADS_THAT_DO_NOT_HAVE_PERSONAL_LOANS()
        {
            using (var tx = new TransactionScope(OnDispose.Rollback))
            {
                var leads = Get_Leads_With_No_Personal_Loans();
                var fileName = new Guid() + "_test.csv";
                MemoryStream stream = new MemoryStream();
                TextWriter writer = new StreamWriter(stream);
                writer.WriteLine("IDNumber");
                foreach (var lead in leads)
                {
                    writer.WriteLine(lead);
                }
                writer.WriteLine("");
                writer.Flush();
                stream.Position = 0;

                IMessageBus messageBus = new FakeMessageBus();
                ILeadImportPublisherService importService = new LeadImportPublisherService(new BatchPublisher(messageBus, new MessageBusDefaultConfiguration()), new CsvFileParse(), RepositoryFactory.GetRepository<IBatchServiceRepository>());
                IBatchService batchService = importService.PublishLeadsForImport<PersonalLoanLead>(stream, fileName);
                tx.VoteCommit();
            }
        }

        [Test]
        public void PublishLeadsForImport_FOR_LEADS_THAT_HAVE_PERSONAL_LOANS()
        {
            using (var tx = new TransactionScope(OnDispose.Rollback))
            {
                var leads = Get_Leads_With_Personal_Loans();
                var fileName = new Guid() + "_test.csv";
                MemoryStream stream = new MemoryStream();
                TextWriter writer = new StreamWriter(stream);
                writer.WriteLine("IDNumber");
                foreach (var lead in leads)
                {
                    writer.WriteLine(lead);
                }
                writer.WriteLine("");
                writer.Flush();
                stream.Position = 0;

                IMessageBus messageBus = new FakeMessageBus();
                ILeadImportPublisherService importService = new LeadImportPublisherService(new BatchPublisher(messageBus, new MessageBusDefaultConfiguration()), new CsvFileParse(), RepositoryFactory.GetRepository<IBatchServiceRepository>());
                IBatchService  batchService =  importService.PublishLeadsForImport<PersonalLoanLead>(stream, fileName);
                tx.VoteCommit();
            }
        }

        private List<TestIDNumber> Get_Leads_With_No_Personal_Loans()
        {
            string query = @"select
	                            top 2 le.idnumber
                            from
	                            [2AM]..LegalEntity le
                            left join
	                            [2AM]..ExternalRole ex
                            on
	                            le.legalEntityKey = ex.LegalEntityKey
	                            and ex.externalRoleTypeKey = 1
	                            and ex.GenericKeyTypeKey = 2
	                            and ex.GeneralStatusKey = 1
                            where
	                            le.LegalEntityTypeKey = 2
		                            and
	                            le.idnumber is not null
		                            and
	                            ex.ExternalRoleKey is null";

            List<TestIDNumber> results = Helper.Many<TestIDNumber>(query, new ParameterCollection());
            return results;
        }

        private List<TestIDNumber> Get_Leads_With_Personal_Loans()
        {
            string query = @"select
	                            top 2 le.idnumber
                            from
	                            [2AM]..LegalEntity le
                            join
	                            [2AM]..ExternalRole ex
                            on
	                            le.legalEntityKey = ex.LegalEntityKey
	                            and ex.externalRoleTypeKey = 1
	                            and ex.GenericKeyTypeKey = 2
	                            and ex.GeneralStatusKey = 1
                            where
	                            le.LegalEntityTypeKey = 2
		                            and
	                            le.idnumber is not null";
            List<TestIDNumber> results = Helper.Many<TestIDNumber>(query, new ParameterCollection());
            return results;
        }

        private GenericMessageResult Get_Batch_Service_Result(int batchServiceKey)
        {
            string query = @"SELECT
	                            BS.BatchServiceKey, count(*) as Leads
                            FROM
	                            [2am].[dbo].[BatchService] BS (nolock)
                            JOIN
	                            [2am].[dbo].[BatchServiceType] BST (nolock)
                            ON
	                            BS.BatchServiceTypeKey = BST.BatchServiceTypeKey
                            JOIN
	                            [Cuttlefish].[dbo].[GenericMessage] GM (nolock)
                            ON
	                            BS.BatchServiceKey = GM.BatchID
                            JOIN
	                            [Cuttlefish].[dbo].GenericStatus GS (nolock)
                            ON
	                            GM.GenericStatusID = GS.ID
                            WHERE
	                            BST.BatchServiceTypeKey = 1 AND BS.BatchServiceKey = @BatchServiceKey
                            GROUP BY
	                            BS.BatchServiceKey";

            SAHL.Common.DataAccess.ParameterCollection parameters = new SAHL.Common.DataAccess.ParameterCollection();
            parameters.Add(new SqlParameter("@BatchServiceKey", batchServiceKey));
            List<GenericMessageResult> results = Helper.Many<GenericMessageResult>(query, parameters);
            if (results.Count > 0)
            {
                return results[0];
            }
            return null;
        }
    }

    public class FakeMessageBus : IMessageBus
    {
        public void Publish<T>(T message) where T : class, Shared.Messages.IMessage
        {
            Debug.WriteLine(message);
        }

        public void Subscribe<T>(Action<T> handler) where T : class, Shared.Messages.IMessage
        {
            Debug.WriteLine("Get a message");
        }
    }

}