using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Enums;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Testing.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.CATSService
{
    [TestFixture]
    public class when_creating_a_command_with_invalid_params : ServiceTestBase<ICATSServiceClient>
    {
        private static string fullFileName;

        [SetUp]
        public void OnTestSetup()
        {
            string timestamp = DateTime.Now.ToString("yyyy_MM_dd_HHmmss");
            fullFileName = CatsAppConfigSettings.CATSOutputFileLocation + "AutomationTestCATSFile" + "_" + timestamp;
        }

        [Test]
        public void when_PaymentBatch_is_not_provided()
        {
            var command = new GenerateCatsFileCommand(CATsEnvironment.Live, fullFileName, "SHL02", 1167
                , new DateTime(2015, 04, 21, 14, 25, 10)
                , new DateTime(2015, 04, 21, 14, 25, 10)
                , null);
            Execute(command).AndExpectThatErrorMessagesContain("There was a validation error: [The PaymentBatch field is required. {in: root}], processing has been halted");
        }

        [Test]
        public void when_creating_a_command_with_profile_greater_than_5_chars()
        {
            var accountNumber = "9212020837".PadLeft(31, 'x');

            var command = new GenerateCatsFileCommand(CATsEnvironment.Live, fullFileName, "Longer than 5 characters", 1167
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
            Execute(command).AndExpectThatErrorMessagesContain("There was a validation error: [The field Profile must be a string with a maximum length of 5. {in: root}], processing has been halted");
        }

        [Test]
        public void when_creating_a_command_with_file_sequence_greater_than_5_chars()
        {
            var accountNumber = "9212020837".PadLeft(31, 'x');

            var command = new GenerateCatsFileCommand(CATsEnvironment.Live, fullFileName, "SHL02", 100000
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
            Execute(command).AndExpectThatErrorMessagesContain("There was a validation error: [The field FileSequenceNo must be between 1 and 99999. {in: root}], processing has been halted");
        }

        [Test]
        public void when_creating_a_command_with_null_output_file_name()
        {
            var accountNumber = "9212020837".PadLeft(31, 'x');

            var command = new GenerateCatsFileCommand(CATsEnvironment.Live, null, "SHL02", 1167
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
            Execute(command).AndExpectThatErrorMessagesContain("There was a validation error: [The OutputFileName field is required. {in: root}], processing has been halted");
        }

        [Test]
        public void when_creating_a_command_with_empty_output_file_name_string()
        {
            var accountNumber = "9212020837".PadLeft(31, 'x');

            var command = new GenerateCatsFileCommand(CATsEnvironment.Live, "", "SHL02", 1167
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
            Execute(command).AndExpectThatErrorMessagesContain("There was a validation error: [The OutputFileName field is required. {in: root}], processing has been halted");
            Assert.True(messages.AllMessages.Count() == 1);
        }
    }
}