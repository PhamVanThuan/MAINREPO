using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Enums;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Testing.Common.Helpers;
using SAHL.Testing.Services.Tests.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.CATSService
{
    [TestFixture]
    public class when_checking_for_previous_file_failure : ServiceTestBase<ICATSServiceClient>
    {
        private int catsPaymentBatchKey;
        private string catsFileFullName;
        [TearDown]
        new public void OnTestTearDown()
        {
            if(catsPaymentBatchKey != 0)
            {
                var removeCATSPaymentBatchCommand = new RemoveCATSPaymentBatchCommand(catsPaymentBatchKey);
                PerformCommand(removeCATSPaymentBatchCommand);
                catsPaymentBatchKey = 0;
            }
            catsFileFullName = null;
            base.OnTestTearDown();
        }

        [Test]
        public void when_a_file_exists_in_the_out_folder_it_should_return_file()
        {
            CreateCATSBatchAndFile(CatsAppConfigSettings.CATSOutputFileLocation);
            var query = new GetPreviousFileFailureQuery(CATSPaymentBatchType.ThirdPartyInvoice);
            base.Execute<GetPreviousFileFailureQuery>(query).WithoutErrors();
            Assert.AreEqual(1, query.Result.Results.Count(), "Expected to find file {0}", catsFileFullName);
        }

        [Test]
        public void when_a_file_exists_in_the_input_folder_it_should_not_return_file()
        {
            CreateCATSBatchAndFile(CatsAppConfigSettings.CATSInputFileLocation);
            var query = new GetPreviousFileFailureQuery(CATSPaymentBatchType.ThirdPartyInvoice);
            base.Execute<GetPreviousFileFailureQuery>(query).WithoutErrors();
            Assert.AreEqual(0, query.Result.Results.Count(), "Expected not to find a file, but found file {0}", catsFileFullName);
        }

        [Test]
        public void when_a_file_exists_in_the_send_failure_folder_it_should_return_file()
        {
            CreateCATSBatchAndFile(CatsAppConfigSettings.CATSFailureFileLocation);
            var query = new GetPreviousFileFailureQuery(CATSPaymentBatchType.ThirdPartyInvoice);
            base.Execute<GetPreviousFileFailureQuery>(query).WithoutErrors();
            Assert.AreEqual(1, query.Result.Results.Count(), "Expected to find file {0}", catsFileFullName);
        }

        [Test]
        public void when_a_file_exists_in_the_send_success_folder_it_should_return_file()
        {
            CreateCATSBatchAndFile(CatsAppConfigSettings.CATSSuccessFileLocation);
            var query = new GetPreviousFileFailureQuery(CATSPaymentBatchType.ThirdPartyInvoice);
            base.Execute<GetPreviousFileFailureQuery>(query).WithoutErrors();
            Assert.AreEqual(0, query.Result.Results.Count(), "Expected not to find a file, but found file {0}", catsFileFullName);
        }


        [Test]
        public void when_a_file_does_not_exists_in_the_out_nor_input_nor_send_failure_folders_it_should_not_return_a_file()
        {
            CreateCATSBatchAndFile(CatsAppConfigSettings.CATSSuccessFileLocation);
            var query = new GetPreviousFileFailureQuery(CATSPaymentBatchType.ThirdPartyInvoice);
            base.Execute<GetPreviousFileFailureQuery>(query).WithoutErrors();
            Assert.AreEqual(0, query.Result.Results.Count(), "Expected not to find a file, but found file {0}", catsFileFullName);
        }

        private void CreateCATSBatchAndFile(string catsFileLocation)
        {
            string catsFileName = string.Concat("CATSFileFailureTest_", TestStartTime.ToString("yyyyMMddHHmmss"));
            catsFileFullName = string.Concat(catsFileLocation, catsFileName);
            var generateCatsFileCommand = new GenerateCatsFileCommand(CATsEnvironment.Test, catsFileFullName, "SHL04", 1, TestStartTime, TestStartTime,
                new List<PaymentBatch>
                {
                    new PaymentBatch(new List<Payment>{
                        new Payment(new BankAccountDataModel("632005","9212020837", (int)ACBType.Current, "P. Miller", null,null)
                            , 5000M,"1492225 Disburse")}
                            , new BankAccountDataModel("42826", "51403153", (int)ACBType.Current, "SA Home Loans (Pty) Limited", null, null)
                    , 5000M, "SAHL      SPV 27")
                });
            Execute(generateCatsFileCommand).WithoutErrors();
            var insertCATSPaymentBatchCommand = new InsertCATSPaymentBatchCommand(new CATSPaymentBatchDataModel((int)CATSPaymentBatchType.ThirdPartyInvoice, TestStartTime, TestStartTime, (int)CATSPaymentBatchStatus.Processed, 1, catsFileName), base.linkedGuid);
            PerformCommand(insertCATSPaymentBatchCommand);
            catsPaymentBatchKey = linkedKeyManager.RetrieveLinkedKey(base.linkedGuid);
        }
    }
}
