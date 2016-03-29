namespace DomainService2.Specs.Workflow.DebtCounselling.CancelDebtCounsellingCommandHandlerSpecs
{
    using DomainService2.Workflow.DebtCounselling;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.Globals;
    using X2DomainService.Interface.Common;

    [Subject(typeof(CancelDebtCounsellingCommandHandler))]
    public class When_cancel_debt_counselling_ework_action_success : DomainServiceSpec<CancelDebtCounsellingCommand, CancelDebtCounsellingCommandHandler>
    {
        Establish context = () =>
        {
            MockRepository your_mother_ees_ur_hamster = new MockRepository();
            IDebtCounselling debtCounselling = An<IDebtCounselling>();
            IReasonDefinition latestReasonDefinition = An<IReasonDefinition>();
            IReasonType reasonType = An<IReasonType>();
            IReasonDescription reasonDescription = An<IReasonDescription>();
            IAccount account = An<IAccount>();
            IDebtCounsellingRepository debtCounsellingRepository = An<IDebtCounsellingRepository>();
            IReasonRepository reasonRepository = An<IReasonRepository>();
            ICommon commonWorkflowService = An<ICommon>();
            reasonType.WhenToldTo(x => x.Key).Return((int)ReasonTypes.DebtCounsellingCancelled);
            reasonDescription.WhenToldTo(x => x.Key).Return((int)ReasonDescriptions.DCCancelledClientRehabilitated);
            latestReasonDefinition.WhenToldTo(x => x.ReasonType).Return(reasonType);
            latestReasonDefinition.WhenToldTo(x => x.ReasonDescription).Return(reasonDescription);
            reasonRepository.WhenToldTo(x => x.GetReasonDefinitionByKey(Param<int>.IsAnything)).Return(latestReasonDefinition);
            debtCounselling.WhenToldTo(x => x.Account).Return(account);
            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param<int>.IsAnything)).Return(debtCounselling);
            string eFolderID = "";
            string eStageName = "";
            IADUser adUser = your_mother_ees_ur_hamster.StrictMock<IADUser>();
            adUser.Expect(x => x.ADUserName).Return("some random ADUserName");
            debtCounsellingRepository.Expect(x => x.GetEworkDataForLossControlCase(Param<int>.IsAnything, out eStageName, out eFolderID, out adUser)).OutRef("some random ewok stage", "some random ewok folder", adUser);
            your_mother_ees_ur_hamster.ReplayAll();
            commonWorkflowService.WhenToldTo(x => x.PerformEWorkAction(Param<IDomainMessageCollection>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<int>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything)).Return(true);
            command = new CancelDebtCounsellingCommand(Param<int>.IsAnything, Param<int>.IsAnything);
            handler = new CancelDebtCounsellingCommandHandler(debtCounsellingRepository, reasonRepository, commonWorkflowService);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_no_error_message = () =>
        {
            messages.Count.Equals(0);
        };
    }
}