using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.DebtCounselling.GetEworkDataForLossControlCaseCommandHandlerSpecs
{
    public class When_an_eworks_case_exists : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static GetEworkDataForLossControlCaseCommand command;
        protected static GetEworkDataForLossControlCaseCommandHandler handler;
        protected static IDebtCounsellingRepository debtCounsellingRepository;

        static string eFolderID;
        static string eStageName;
        static IADUser adUser;

        // Arrange
        Establish context = () =>
        {
            MockRepository mocks = new MockRepository();
            IDebtCounsellingRepository debtCounsellingRepository = mocks.StrictMock<IDebtCounsellingRepository>();

            int accountKey = 1;
            eFolderID = "";
            eStageName = "";
            adUser = mocks.StrictMock<IADUser>();
            adUser.Expect(x => x.ADUserName).Return("bob");

            debtCounsellingRepository.Expect(x => x.GetEworkDataForLossControlCase(accountKey, out eStageName, out eFolderID, out adUser)).OutRef("aaa", "bbb", adUser);

            command = new GetEworkDataForLossControlCaseCommand(accountKey);
            handler = new GetEworkDataForLossControlCaseCommandHandler(debtCounsellingRepository);

            mocks.ReplayAll();
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_set_command_output_parameters_to_non_empty = () =>
        {
            command.eFolderID.ShouldNotBeEmpty();
            command.eStageName.ShouldNotBeEmpty();
            command.eADUserName.ShouldNotBeEmpty();
        };
    }
}