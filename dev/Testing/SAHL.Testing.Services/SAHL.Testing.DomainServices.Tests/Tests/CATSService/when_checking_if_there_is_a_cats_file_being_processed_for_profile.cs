using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Enums;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Testing.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.Tests.CATSService
{
    [TestFixture]
    public class when_checking_if_there_is_a_cats_file_being_processed_for_profile : ServiceTestBase<ICATSServiceClient>
    {
        private string timestamp;
        private string filePath;

        [TestFixtureSetUp]
        public void OnTestFixtureSetup()
        {
            var files = new List<GetFileNamesOfFilesInDirectoryQueryResult>();

            var getNamesOfFilesInDirectoryQuery = new GetFileNamesOfFilesInDirectoryQuery(CatsAppConfigSettings.CATSOutputFileLocation, "*", System.IO.SearchOption.TopDirectoryOnly);
            _feTestClient.PerformQuery(getNamesOfFilesInDirectoryQuery).WithoutMessages();
            files.AddRange(getNamesOfFilesInDirectoryQuery.Result.Results);

            getNamesOfFilesInDirectoryQuery = new GetFileNamesOfFilesInDirectoryQuery(CatsAppConfigSettings.CATSFailureFileLocation, "*", System.IO.SearchOption.TopDirectoryOnly);
            _feTestClient.PerformQuery(getNamesOfFilesInDirectoryQuery).WithoutMessages();
            files.AddRange(getNamesOfFilesInDirectoryQuery.Result.Results);

            foreach (var file in files)
            {
                var removeFileByPathCommand = new RemoveFileByPathCommand(file.FullName);
                _feTestClient.PerformCommand(removeFileByPathCommand, new ServiceRequestMetadata()).WithoutMessages();
            }
        }

        [SetUp]
        public void OnTestSetup()
        {
            timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        }

        [Test]
        public void given_there_is_a_file_with_matching_profile_prefix_in_the_out_folder_it_should_return_true()
        {
            filePath = CatsAppConfigSettings.CATSOutputFileLocation + "SHL02_Disbursement_" + timestamp;
            var setup_command = new GenerateCatsFileCommand(CATsEnvironment.Live, filePath, "SHL02", 1167
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
            Execute(setup_command).WithoutErrors();

            var query = new IsThereACatsFileBeingProcessedForProfileQuery((int)CATSPaymentBatchType.ThirdPartyInvoice);
            Execute(query).WithoutErrors();
            Assert.IsTrue(query.Result.Results.FirstOrDefault(),
                        "A valid file was not found in the "+ CatsAppConfigSettings.CATSOutputFileLocation + " folder when there should be one.");
            
        }

        [Test]
        public void given_there_is_a_file_with_matching_profile_prefix_in_the_send_failure_folder_it_should_return_true()
        {
            filePath = CatsAppConfigSettings.CATSFailureFileLocation + "SHL02_Disbursement_" + timestamp;
            var setup_command = new GenerateCatsFileCommand(CATsEnvironment.Live, filePath, "SHL02", 1167
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
            Execute(setup_command).WithoutErrors();

            var query = new IsThereACatsFileBeingProcessedForProfileQuery((int)CATSPaymentBatchType.ThirdPartyInvoice);
            Execute(query).WithoutErrors();
            Assert.IsTrue(query.Result.Results.FirstOrDefault(),
                        "A valid file was not found in the " + CatsAppConfigSettings.CATSFailureFileLocation + " folder when there should be one.");

        }

        [Test]
        public void given_there_is_a_file_without_matching_profile_prefix_in_the_out_folder_it_should_return_false()
        {
            filePath = CatsAppConfigSettings.CATSOutputFileLocation + "InvalidFileName_" + timestamp;
            var setup_command = new GenerateCatsFileCommand(CATsEnvironment.Live, filePath , "SHL02", 1167
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
            Execute(setup_command).WithoutErrors();

            var query = new IsThereACatsFileBeingProcessedForProfileQuery((int)CATSPaymentBatchType.ThirdPartyInvoice);
            Execute(query).WithoutErrors();
            Assert.IsFalse(query.Result.Results.FirstOrDefault(),
                        "An invalid file was found in the " + CatsAppConfigSettings.CATSOutputFileLocation + " folder when there shouldn't be one.");
        }

        [Test]
        public void given_there_is_a_file_without_matching_profile_prefix_in_the_send_failure_folder_it_should_return_false()
        {
            filePath = CatsAppConfigSettings.CATSFailureFileLocation + "InvalidFileName_" + timestamp;
            var setup_command = new GenerateCatsFileCommand(CATsEnvironment.Live, filePath, "SHL02", 1167
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
            Execute(setup_command).WithoutErrors();

            var query = new IsThereACatsFileBeingProcessedForProfileQuery((int)CATSPaymentBatchType.ThirdPartyInvoice);
            Execute(query).WithoutErrors();
            Assert.IsFalse(query.Result.Results.FirstOrDefault(),
                        "An invalid file was found in the " + CatsAppConfigSettings.CATSFailureFileLocation + " folder when there shouldn't be one.");
        }

        [Test]
        public void given_there_is_no_test_in_the_out_or_send_failure_folder_it_should_return_false()
        {
            var files = new List<GetFileNamesOfFilesInDirectoryQueryResult>();

            var getNamesOfFilesInDirectoryQuery = new GetFileNamesOfFilesInDirectoryQuery(CatsAppConfigSettings.CATSOutputFileLocation, "*", System.IO.SearchOption.TopDirectoryOnly);
            _feTestClient.PerformQuery(getNamesOfFilesInDirectoryQuery).WithoutMessages();
            files.AddRange(getNamesOfFilesInDirectoryQuery.Result.Results);

            getNamesOfFilesInDirectoryQuery = new GetFileNamesOfFilesInDirectoryQuery(CatsAppConfigSettings.CATSFailureFileLocation, "*", System.IO.SearchOption.TopDirectoryOnly);
            _feTestClient.PerformQuery(getNamesOfFilesInDirectoryQuery).WithoutMessages();
            files.AddRange(getNamesOfFilesInDirectoryQuery.Result.Results);

            foreach (var file in files)
            {
                var removeFileByPathCommand = new RemoveFileByPathCommand(file.FullName);
                _feTestClient.PerformCommand(removeFileByPathCommand, new ServiceRequestMetadata()).WithoutMessages();
            }
            var query = new IsThereACatsFileBeingProcessedForProfileQuery((int)CATSPaymentBatchType.ThirdPartyInvoice);
            Execute(query).WithoutErrors();
            Assert.IsFalse(query.Result.Results.FirstOrDefault(),
                        "An invalid file was found in the " + CatsAppConfigSettings.CATSFailureFileLocation + " folder when there shouldn't be one.");
        }

        [TearDown]
        public void OnTestTearDown()
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                var cleanup_command = new RemoveFileByPathCommand(filePath);
                PerformCommand(cleanup_command);
                filePath = string.Empty;
            }
        }

    }
}