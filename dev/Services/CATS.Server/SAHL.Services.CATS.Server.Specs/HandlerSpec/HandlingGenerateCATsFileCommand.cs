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
    public class HandlingGenerateCATsFileCommand : WithFakes
    {
        private GenerateCatsFileCommand command;
        private IDomainRuleManager<PaymentBatch> ruleManager;
        private ICATSFileGenerator catsFileGenerator;
        private IFileWriter fileWriter;
        private ICATSFileRecordGenerator catsFileRecordGenerator;
        private ISystemMessageCollection msgs;
        private string outputFileName;

        [Test]
        public void HandleGenerationofCATsFile()
        {
            this.Given(x => AValidCATsPayment("SHL02", 1167, new DateTime(2015, 04, 21, 14, 25, 10)
                , new DateTime(2015, 04, 21, 14, 25, 10)
                , new List<PaymentBatch>
                {
                    new PaymentBatch(
                    new List<Payment>
                    {
                        new Payment(
                        new BankAccountDataModel("632005","9212020837", (int)ACBType.Current, "P. Miller", null,null)
                        , 5000M,"1492225 Disburse")
                    }
                    , new BankAccountDataModel("42826", "51403153", (int)ACBType.Current, "SA Home Loans (Pty) Limited", null, null)
                    , 5000M, "Reference")
                ,new PaymentBatch(
                    new List<Payment>
                    {
                        new Payment(
                            new BankAccountDataModel("42826","9086504522", (int)ACBType.Current, "Ms MARLENE THERESA HECTORMs MARLENE THERESA HECTOR", null,null)
                        , 1121.0M,"1492759 Disburse")
                    }
                    ,new BankAccountDataModel("42826", "51403153", (int)ACBType.Current, "SA Home Loans (Pty) Limited", null, null)
                    , 200M, "Reference")
                },
                new BankAccountDataModel("42826", "51403153", (int)ACBType.Current, "SA Home Loans (Pty) Limited", null, null), CATsEnvironment.Live))
                 .When(x => HandleGenerateCATsFileCommand())
                 .Then(x => GeneratingCATsFile())
                 .And(x => WriteCATsFileHeader())
                 .And(x => WritePaymentBatchDetailsAndPaymentBatchTotals())
                 .And(x => WriteTrailer())
                 .And(x => ShouldNotHaveErrorMessages())
                .BDDfy("Handling generate CATs File Command");
        }


        private void AValidCATsPayment(string profile, int fileSequenceNo, DateTime createdDateTime, DateTime actionedDateTime, List<PaymentBatch> paymentBatches,
                                        BankAccountDataModel bankAccountDataModel, CATsEnvironment cATsEnvironment)
        {
            var fileName = System.IO.Path.GetTempPath() + "SHL04_Disbursement";
            command = new GenerateCatsFileCommand(cATsEnvironment, fileName, profile, fileSequenceNo, createdDateTime, actionedDateTime, paymentBatches);
            catsFileGenerator = Substitute.For<ICATSFileGenerator>();
            fileWriter = An<IFileWriter>();
            catsFileRecordGenerator = An<ICATSFileRecordGenerator>();
            outputFileName = "TestFileName";
            fileWriter.WhenToldTo(x => x.CreateFile(command.OutputFileName)).Return(outputFileName);
        }

        private void WriteTrailer()
        {
            catsFileGenerator.WasToldTo(x => x.WriteTrailerRecord(
                Param.IsAny<IFileWriter>()
                , Param.IsAny<string>()
                , Param.IsAny<ICATSFileRecordGenerator>()
                , Param.IsAny<IEnumerable<PaymentBatch>>()
                , Param.IsAny<int>()));
        }

        private void HandleGenerateCATsFileCommand()
        {
            ruleManager = An<IDomainRuleManager<PaymentBatch>>();
            GenerateCatsFileCommandHandler commandHandler = new GenerateCatsFileCommandHandler(fileWriter, catsFileGenerator, catsFileRecordGenerator, ruleManager);
            ServiceRequestMetadata metadata = new ServiceRequestMetadata();
            msgs = commandHandler.HandleCommand(command, metadata);
        }

        private void ShouldNotHaveErrorMessages()
        {
            msgs.HasErrors.ShouldBeFalse();
        }

        private void WritePaymentBatchDetailsAndPaymentBatchTotals()
        {
            catsFileGenerator.WasToldTo(x => x.WritePaymentBatchDetailRecords(command.FileSequenceNo, Param.IsAny<int>(), Param.IsAny<PaymentBatch>()
               , catsFileRecordGenerator
               , fileWriter, outputFileName, command.PaymentBatch.First().Reference));
        }

        private void WriteCATsFileHeader()
        {
            catsFileGenerator.WasToldTo(x => x.WriteHeader(Param.IsAny<IFileWriter>()
               , outputFileName
               , catsFileRecordGenerator
               , command.FileSequenceNo
               , command.Profile
               , command.Date
               , command.CATsEnvironment));
        }

        private void GeneratingCATsFile()
        {
            fileWriter.WasToldTo(x => x.CreateFile(command.OutputFileName));
        }
    }
}