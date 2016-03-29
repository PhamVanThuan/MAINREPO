using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Rules;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Models;

namespace SAHL.Services.CATS.CommandHandlers
{
    public class GenerateCatsFileCommandHandler : IServiceCommandHandler<GenerateCatsFileCommand>
    {
        private IFileWriter fileWriter;
        private ICATSFileGenerator catsFileGenerator;
        private ICATSFileRecordGenerator catsFileRecordGenerator;
        private IDomainRuleManager<PaymentBatch> ruleManager;

        public GenerateCatsFileCommandHandler(IFileWriter fileWriter, ICATSFileGenerator catsFileGenerator, ICATSFileRecordGenerator catsFileRecordGenerator
            , IDomainRuleManager<PaymentBatch> ruleManager)
        {
            this.fileWriter = fileWriter;
            this.catsFileGenerator = catsFileGenerator;
            this.catsFileRecordGenerator = catsFileRecordGenerator;
            this.ruleManager = ruleManager;
            ruleManager.RegisterRule(new APaymentBatchShouldHaveAtleastOnePaymentRule());
            ruleManager.RegisterRule(new SourceBankAccountDetailsShouldFollowCatsFormatRule());
            ruleManager.RegisterRule(new TargetBankAccountDetailsShouldFollowCatsFormatRule());
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(GenerateCatsFileCommand command, IServiceRequestMetadata metadata)
        {
            var msgs = SystemMessageCollection.Empty();
            foreach (var paymentBatch in command.PaymentBatch)
            {
                ruleManager.ExecuteRules(msgs, paymentBatch);
            }
            if (msgs.HasErrors)
            {
                return msgs;
            }

            var outputFileName = fileWriter.CreateFile(command.OutputFileName);
            catsFileGenerator.WriteHeader(fileWriter, outputFileName
                , catsFileRecordGenerator, command.FileSequenceNo
                , command.Profile, command.Date, command.CATsEnvironment);
            int subBatchSequenceNo = 1;
            foreach (var paymentBatch in command.PaymentBatch)
            {
                catsFileGenerator.WritePaymentBatchDetailRecords(command.FileSequenceNo
                    , subBatchSequenceNo, paymentBatch, catsFileRecordGenerator
                    , fileWriter, outputFileName, paymentBatch.Reference);
                subBatchSequenceNo++;
            }
            catsFileGenerator.WriteTrailerRecord(fileWriter, outputFileName, catsFileRecordGenerator, command.PaymentBatch, command.FileSequenceNo);
            return msgs;
        }
    }
}