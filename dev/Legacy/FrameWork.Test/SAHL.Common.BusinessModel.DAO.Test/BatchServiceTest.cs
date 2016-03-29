using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class BatchServiceTest : TestBase
    {
        [Ignore()]
        [Test]
        public void BatchServiceSaveTest()
        {
            using (TransactionScope tx = new TransactionScope(OnDispose.Rollback))
            {
                FileStream fs = new FileStream(@"C:\TestFiles\id numbers.csv", FileMode.Open, FileAccess.Read);
                byte[] fileContent = new byte[fs.Length];
                fs.Read(fileContent, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                BatchService_DAO batchService = new BatchService_DAO();
                batchService.RequestedBy = "test";
                batchService.RequestedDate = DateTime.Now;
                batchService.BatchCount = 10;
                batchService.BatchServiceTypeKey = (int)BatchServiceTypes.PersonalLoanLeadImport;
                batchService.FileContent = fileContent;
                batchService.FileName = "test";
                batchService.SaveAndFlush();
                tx.VoteCommit();
            }
        }

        [Ignore()]
        [Test]
        public void BatchServiceReadTest()
        {
            using (new SessionScope())
            {
                FileStream fs = new FileStream(@"C:\TestFiles\output.csv", FileMode.Create, FileAccess.ReadWrite);
                BatchService_DAO batchService = BatchService_DAO.Find(3);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(batchService.FileContent);
                bw.Close();
            }
        }
    }
}

