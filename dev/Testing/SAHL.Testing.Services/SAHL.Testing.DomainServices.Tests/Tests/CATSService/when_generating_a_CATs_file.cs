using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Enums;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Testing.Common;
using SAHL.Testing.Common.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SAHL.Testing.Services.Tests.CATSService
{
    [TestFixture]
    public class when_generating_a_CATs_file : ServiceTestBase<ICATSServiceClient>
    {
        private static string filePath;
        private static string fileName;
        private static string fullFileName;
        private static string searchPattern;

        [SetUp]
        new public void OnTestSetup()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            filePath = CatsAppConfigSettings.CATSOutputFileLocation;
            fileName = "AutomationTest_CATSFile" + "_" + timestamp;
            fullFileName = filePath + fileName;
            searchPattern = fileName + "*";
        }

        [TearDown]
        new public void OnTestTearDown()
        {
            var command = new RemoveFileByPathCommand(fullFileName);
            try
            {
                PerformCommand(command);
            }
            catch (Exception x)
            {
                if (!x.Message.Contains("does not exist."))
                {
                    throw;
                }
            }
            base.OnTestTearDown();
        }

        [Test]
        public void when_GroupedPaymentsQA_test()
        {
            var command = new GenerateCatsFileCommand(CATsEnvironment.Live, fullFileName, "SHL02", 1167
                , new DateTime(2015, 04, 21, 14, 25, 10)
                , new DateTime(2015, 04, 21, 14, 25, 10)
                , new List<PaymentBatch>
                {
                    new PaymentBatch(new List<Payment>{
                        new Payment(new BankAccountDataModel("632005","9212020837", (int)ACBType.Current, "P. Miller", null,null)
                            , 5000M,"1492225 Disburse")}
                            , new BankAccountDataModel("42826", "51403153", (int)ACBType.Current, "SA Home Loans (Pty) Limited", null, null)
                    , 5000M, "SAHL      SPV 27")
                    ,new PaymentBatch(new List<Payment>{
                        new Payment(new BankAccountDataModel("632005","9086504522", (int)ACBType.Current, "Ms MARLENE THERESA HECTOR", null,null)
                    , 2000M,"1492759 Disburse")}
                    , new BankAccountDataModel("42826", "51403153", (int)ACBType.Current, "SA Home Loans (Pty) Limited", null, null)
                    , 2000M, "SAHL      SPV 34")
                });
            Execute(command).WithoutErrors();

            var getNamesOfFilesInDirectory = new GetFileNamesOfFilesInDirectoryQuery(filePath, searchPattern, SearchOption.TopDirectoryOnly);
            PerformQuery(getNamesOfFilesInDirectory);
            foreach (var fileInfo in getNamesOfFilesInDirectory.Result.Results)
            {
                fullFileName = fileInfo.FullName;
            }
            var query = new GetFileUsingPathQuery(fullFileName);
            PerformQuery(query);
            var file = query.Result.Results.FirstOrDefault();
            Assert.NotNull(file, filePath + " file was not found");
            var expectedFile = ResourcesHelper.GetResourceText("SHL04_Disbursement");
            var normalizedFile = ResourcesHelper.NormalizeCarriageReturn(file);
            Assert.AreEqual(normalizedFile, expectedFile, filePath + " file did not match template file");
        }

        [Test]
        public void when_source_bank_account_is_invalid_test()
        {
            var accountNumber = "".PadLeft(14, '1');

            var command = new GenerateCatsFileCommand(CATsEnvironment.Live, fullFileName, "SHL02", 1167
                , new DateTime(2015, 04, 21, 14, 25, 10)
                , new DateTime(2015, 04, 21, 14, 25, 10)
                , new List<PaymentBatch>
                {
                    new PaymentBatch(new List<Payment>{
                        new Payment(new BankAccountDataModel("632005",accountNumber, (int)ACBType.Current, "P. Miller", null,null)
                            , 5000M,"1492225 Disburse")}
                            , new BankAccountDataModel("42826", "51403153", (int)ACBType.Current, "SA Home Loans (Pty) Limited", null, null)
                    , 5000M, "SAHL      SPV 27")
                    ,new PaymentBatch(new List<Payment>{
                        new Payment(new BankAccountDataModel("632005","9086504522", (int)ACBType.Current, "Ms MARLENE THERESA HECTOR", null,null)
                    , 2000M,"1492759 Disburse")}
                    , new BankAccountDataModel("42826", "51403153", (int)ACBType.Current, "SA Home Loans (Pty) Limited", null, null)
                    , 2000M, "SAHL      SPV 34")
                });

            var expectedError = "Invalid account number: " + accountNumber +
                 ". The target account's account number should not be longer than 13 characters.";
            Execute(command).AndExpectThatErrorMessagesContain(expectedError);
            Assert.True(messages.ErrorMessages().Any(x => x.Message.Equals(expectedError)), "The expected error message was not found: " + expectedError);
        }
    }
}