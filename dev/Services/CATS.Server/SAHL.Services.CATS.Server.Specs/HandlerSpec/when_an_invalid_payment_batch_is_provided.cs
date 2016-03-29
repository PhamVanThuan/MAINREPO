using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.CommandHandlers;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Utils;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Enums;
using SAHL.Services.Interfaces.CATS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TestStack.BDDfy;

namespace SAHL.Services.CATS.Server.Specs.HandlerSpec
{
    [TestFixture]
    public class when_an_invalid_payment_batch_is_provided : WithFakes
    {
        private GenerateCatsFileCommand command;
        private IDomainRuleManager<PaymentBatch> ruleManager;
        private ICATSFileGenerator catsFileGenerator;
        private IFileWriter fileWriter;
        private ICATSFileRecordGenerator catsFileRecordGenerator;
        private ISystemMessageCollection msgs;
        private PaymentBatch paymentBatch1;
        private PaymentBatch paymentBatch2;

        [Test]
        public void HandleGenerationofCATsFile()
        {
            this.Given(x => AValidCATsPayment("SHL02", 1167, new DateTime(2015, 04, 21, 14, 25, 10)
                , new DateTime(2015, 04, 21, 14, 25, 10)
                , new PaymentBatch(
                    new List<Payment>
                    {
                        new Payment(
                        new BankAccountDataModel("632005","9212020837", (int)ACBType.Current, "P. Miller", null,null)
                        , 5000M,"1492225 Disburse")
                    }
                    , new BankAccountDataModel("42826", "51403153".PadRight(34, '2'), (int)ACBType.Current, "SA Home Loans (Pty) Limited", null, null)
                    , 5000M, "Reference")
                , new PaymentBatch(
                    new List<Payment>
                    {
                        new Payment(
                            new BankAccountDataModel("42826","9086504522", (int)ACBType.Current, "Ms MARLENE THERESA HECTOR", null,null)
                        , 1121.0M,"1492759 Disburse")
                    }
                    , new BankAccountDataModel("42826", "51403153", (int)ACBType.Current, "SA Home Loans (Pty) Limited", null, null)
                    , 200M, "Reference")
                    
                ,
                new BankAccountDataModel("42826", "51403153", (int)ACBType.Current, "SA Home Loans (Pty) Limited", null, null), CATsEnvironment.Live))
                 .When(x => HandleGenerateCATsFileCommand())
                 .Then(x => ExcecuteRules())
                 .And(x => ShouldNotWriteCATsFileHeader())
                 .And(x => ShouldNotWritePaymentBatchDetailsAndPaymentBatchTotals())
                 .And(x => ShouldNotWriteTrailer())
                 .And(x => ShouldReturnWithAnErrorMessage())
                .BDDfy("Handling generate CATs File Command");
        }

        private void AValidCATsPayment(string profile, int fileSequenceNo, DateTime createdDateTime, DateTime actionedDateTime, PaymentBatch paymentBatche1,  PaymentBatch paymentBatche2,
                                        BankAccountDataModel bankAccountDataModel, CATsEnvironment cATsEnvironment)
        {
            this.paymentBatch1 = paymentBatche1;
            this.paymentBatch2 = paymentBatche2;
            var fileName = System.IO.Path.GetTempPath() + "SHL04_Disbursement";
            command = new GenerateCatsFileCommand(cATsEnvironment, fileName, profile, fileSequenceNo
                , createdDateTime, actionedDateTime, new List<PaymentBatch>() { paymentBatche1, paymentBatche2 });
            catsFileGenerator = Substitute.For<ICATSFileGenerator>();
            fileWriter = An<IFileWriter>();
            catsFileRecordGenerator = An<ICATSFileRecordGenerator>();
        }

        private void ShouldNotWriteTrailer()
        {
            catsFileGenerator.WasNotToldTo(x => x.WriteTrailerRecord(
                Param.IsAny<IFileWriter>()
                , Param.IsAny<string>()
                , Param.IsAny<ICATSFileRecordGenerator>()
                , Param.IsAny<IEnumerable<PaymentBatch>>()
                , Param.IsAny<int>()));
        }

        private void HandleGenerateCATsFileCommand()
        {
            ruleManager = An<IDomainRuleManager<PaymentBatch>>();
            
            ruleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), paymentBatch1))
            .Callback<ISystemMessageCollection>(y => y.AddMessage(new SystemMessage("Rule failure message.", SystemMessageSeverityEnum.Error)));

            ruleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), paymentBatch2))
           .Callback<ISystemMessageCollection>(y => y.AddMessage(new SystemMessage("Rule failure message.", SystemMessageSeverityEnum.Error)));
            GenerateCatsFileCommandHandler commandHandler = new GenerateCatsFileCommandHandler(fileWriter, catsFileGenerator, catsFileRecordGenerator, ruleManager);
            ServiceRequestMetadata metadata = new ServiceRequestMetadata();
            msgs = commandHandler.HandleCommand(command, metadata);
        }

        private void ShouldReturnWithAnErrorMessage()
        {
            msgs.HasErrors.ShouldBeTrue();
        }

        private void ShouldNotWritePaymentBatchDetailsAndPaymentBatchTotals()
        {
            catsFileGenerator.WasNotToldTo(x => x.WritePaymentBatchDetailRecords(command.FileSequenceNo, Param.IsAny<int>(), Param.IsAny<PaymentBatch>()
               , catsFileRecordGenerator
               , fileWriter, command.OutputFileName, command.PaymentBatch.First().Reference));
        }

        private void ShouldNotWriteCATsFileHeader()
        {
            catsFileGenerator.WasNotToldTo(x => x.WriteHeader(Param.IsAny<IFileWriter>()
               , command.OutputFileName
               , catsFileRecordGenerator
               , command.FileSequenceNo
               , command.Profile
               , command.Date
               , command.CATsEnvironment));
        }

        private void ExcecuteRules()
        {
            foreach (var paymentBatch in command.PaymentBatch)
            {
                ruleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), paymentBatch));
            }
        }
    }
}