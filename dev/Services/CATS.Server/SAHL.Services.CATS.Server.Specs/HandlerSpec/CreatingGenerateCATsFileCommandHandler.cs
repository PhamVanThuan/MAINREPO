using FluentAssertions;
using Machine.Fakes;
using Machine.Specifications;
using NUnit.Framework;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Services.CATS.CommandHandlers;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Rules;
using SAHL.Services.CATS.Utils;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Models;
using System;
using TestStack.BDDfy;

namespace SAHL.Services.CATS.Server.Specs.HandlerSpec
{
    [TestFixture]
    public class CreatingGenerateCATsFileCommandHandler : WithFakes
    {
        private IDomainRuleManager<PaymentBatch> ruleManager;
        private GenerateCatsFileCommandHandler commandHandler;
        private IFileWriter fileWriter;
        private ICATSFileGenerator catsFileGenerator;
        private ICATSFileRecordGenerator catsFileRecordGenerator;
        [Test]
        public void CreatingCommandHandler()
        {
            this.When(x => GenerateCATsFileCommandHandlerIsCreated())
            .Then(x => registerArule(typeof(APaymentBatchShouldHaveAtleastOnePaymentRule)))
            .Then(x => registerArule(typeof(SourceBankAccountDetailsShouldFollowCatsFormatRule)))
            .Then(x => registerArule(typeof(TargetBankAccountDetailsShouldFollowCatsFormatRule)))
            .Then(x => GenerateCATsFileCommandHandlerShouldBeCreated())
            .And(x => CommandHandlerShouldBeAssignableTo(typeof(IServiceCommandHandler<GenerateCatsFileCommand>)))
           .BDDfy("Creating generate CATs file command handler");
        }

        private void registerArule(Type T)
        {
            ruleManager.WasToldTo(x => x.RegisterRule(Param<IDomainRule<PaymentBatch>>.Matches(y => y.GetType().Name.Equals(T.Name))));
        }
        private void CommandHandlerShouldBeAssignableTo(Type T)
        {
            T.IsAssignableFrom(commandHandler.GetType()).Should().BeTrue();
        }

        private void GenerateCATsFileCommandHandlerShouldBeCreated()
        {
            commandHandler.ShouldNotBeNull();
        }

        private void GenerateCATsFileCommandHandlerIsCreated()
        {
            fileWriter = An<IFileWriter>();
            catsFileGenerator = An<ICATSFileGenerator>();
            catsFileRecordGenerator = An<ICATSFileRecordGenerator>();

            ruleManager = An<IDomainRuleManager<PaymentBatch>>();
            commandHandler = new GenerateCatsFileCommandHandler(fileWriter, catsFileGenerator, catsFileRecordGenerator, ruleManager);
        }
    }
}