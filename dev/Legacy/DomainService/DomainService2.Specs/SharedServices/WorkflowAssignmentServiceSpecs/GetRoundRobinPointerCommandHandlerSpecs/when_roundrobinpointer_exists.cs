using DomainService2.SharedServices.WorkflowAssignment;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.Specs.SharedServices.WorkflowAssignmentServiceSpecs.GetRoundRobinPointerCommandHandlerSpecs
{
    [Subject(typeof(GetRoundRobinPointerCommandHandler))]
    public class when_roundrobinpointer_exists : WithFakes
    {
        static GetRoundRobinPointerCommand command;
        static GetRoundRobinPointerCommandHandler handler;
        static IDomainMessageCollection messages;
        static IWorkflowAssignmentRepository repo;

        static OfferRoleTypes offerRoleType;
        static int orgStructure;
        static IRoundRobinPointer roundRobinPointer;
        static int roundRobinPointerKey;

        Establish context = () =>
        {
            offerRoleType = OfferRoleTypes.BranchConsultantD;
            orgStructure = 8081;
            roundRobinPointerKey = 9000;
            roundRobinPointer = An<IRoundRobinPointer>();
            roundRobinPointer.WhenToldTo(x => x.Key)
                             .Return(roundRobinPointerKey);

            repo = An<IWorkflowAssignmentRepository>();
            repo.WhenToldTo(x => x.DetermineRoundRobinPointerByOfferRoleTypeAndOrgStructure(offerRoleType, orgStructure))
                .Return(roundRobinPointer);

            handler = new GetRoundRobinPointerCommandHandler(repo);

            command = new GetRoundRobinPointerCommand(offerRoleType, orgStructure);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_not_return_minus_1 = () =>
        {
            command.Result.ShouldNotEqual(-1);
        };

        It should_return_the_roundrobinpointers_key = () =>
        {
            command.Result.ShouldEqual(roundRobinPointer.Key);
        };
    }
}
