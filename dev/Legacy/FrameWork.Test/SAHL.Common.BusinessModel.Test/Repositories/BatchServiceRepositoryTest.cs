using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Test;
using System;
using System.IO;


namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class BatchServiceRepositoryTest : TestBase
    {
        [Ignore()]
        [Test]
        public void SaveBatchServiceTest()
        {
            using (TransactionScope tx = new TransactionScope(OnDispose.Rollback))
            {
                IBatchServiceRepository batchServiceRepo = RepositoryFactory.GetRepository<IBatchServiceRepository>();
                var batchService = batchServiceRepo.GetEmptyBatchService();
                FileStream fs = new FileStream(@"C:\TestFiles\id numbers.csv", FileMode.Open, FileAccess.Read);
                byte[] fileContent = new byte[fs.Length];
                fs.Read(fileContent, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                batchService.RequestedBy = "test";
                batchService.RequestedDate = DateTime.Now;
                batchService.BatchCount = 10;
                batchService.BatchServiceTypeKey = (int)BatchServiceTypes.PersonalLoanLeadImport;
                batchService.FileContent = fileContent;
                batchService.FileName = "test";
                batchServiceRepo.SaveBatchService(batchService);
                tx.VoteCommit();
            }
        }

        [Ignore()]
        [Test]
        public void GetBatchServiceTest()
        {
            using (new SessionScope())
            {
                IBatchServiceRepository batchServiceRepo = RepositoryFactory.GetRepository<IBatchServiceRepository>();
                var batchService = batchServiceRepo.GetBatchService(3);
                FileStream fs = new FileStream(@"C:\TestFiles\output.csv", FileMode.Create, FileAccess.ReadWrite);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(batchService.FileContent);
                bw.Close();
            }
        }

        [Ignore()]
        [Test]
        public void GetBatchServiceResultsTest()
        {
            using (new SessionScope())
            {
                IPersonalLoanRepository plRepo = new PersonalLoanRepository();
                var results = plRepo.GetBatchServiceResults(BatchServiceTypes.PersonalLoanLeadImport);
            }
        }

        [Ignore]
        [Test]
        public void PublishLeadsForImportTest()
        {
            using (var tx = new TransactionScope(OnDispose.Rollback))
            {
                var fileName = new Guid() + "_test.csv";
                MemoryStream stream = new MemoryStream();
                TextWriter writer = new StreamWriter(stream);
                writer.WriteLine("IDNumber");
                writer.WriteLine("5604160048082");
                ILeadImportPublisherService importService = ServiceFactory.GetService<ILeadImportPublisherService>();
                importService.PublishLeadsForImport<PersonalLoanLead>(stream, fileName);
                tx.VoteCommit();
            }
        }

    }
}