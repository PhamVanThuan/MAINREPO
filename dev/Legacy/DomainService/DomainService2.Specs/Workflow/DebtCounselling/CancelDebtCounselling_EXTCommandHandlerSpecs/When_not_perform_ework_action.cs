using System;
using System.Collections.Generic;
using System.Data;
using DomainService2.SharedServices;
using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using X2DomainService.Interface.Common;

namespace DomainService2.Specs.Workflow.DebtCounselling.CancelDebtCounselling_EXTCommandHandlerSpecs
{
    [Subject(typeof(CancelDebtCounselling_EXTCommandHandler))]
    public class When_not_perform_ework_action : DomainServiceSpec<CancelDebtCounselling_EXTCommand, CancelDebtCounselling_EXTCommandHandler>
    {
        protected static IDebtCounsellingRepository debtCounsellingRepository;
        protected static IDebtCounselling debtCounsellingCase;
        protected static ICommonRepository commonRepository;
        protected static IMessageService messageService;
        protected static IX2WorkflowService x2WorkflowService;
        protected static ICommon commonWorkflowService;
        protected static ILookupRepository lookupRepository;
        protected static IDebtCounsellingStatus debtCounsellingStatus;
        protected static Exception exception;
        protected static IWorkflowSecurityRepository workflowSecurityRepository;
        protected static IOrganisationStructureRepository organisationStructureRepository;

        Establish context = () =>
        {
            MockRepository mocks = new MockRepository();

            IADUser adUser = null;

            debtCounsellingRepository = An<IDebtCounsellingRepository>();
            x2WorkflowService = An<IX2WorkflowService>();
            commonWorkflowService = An<ICommon>();

            ILegalEntity debtCounsellor = null;

            debtCounsellingCase = An<IDebtCounselling>();
            debtCounsellingStatus = An<IDebtCounsellingStatus>();
            x2WorkflowService.WhenToldTo(x => x.HasInstancePerformedActivity(Param.IsAny<long>(), Param.IsAny<string>())).Return(false);

            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param.IsAny<int>())).Return(debtCounsellingCase);
            debtCounsellingCase.WhenToldTo(x => x.DebtCounsellor).Return(debtCounsellor);

            string eStageName = "test";
            string eFolderID = "";
            debtCounsellingRepository.Expect(x => x.GetEworkDataForLossControlCase(Param.IsAny<int>(), out eStageName, out eFolderID, out adUser)).OutRef("test", "test", adUser);
            mocks.ReplayAll();

            commonWorkflowService.WhenToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(),
                                                                       Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<string>(),
                                                                       Param.IsAny<string>())).Return(false);

            command = new CancelDebtCounselling_EXTCommand(1, "test", 1);
            handler = new CancelDebtCounselling_EXTCommandHandler(x2WorkflowService, debtCounsellingRepository, commonRepository,
                                                                  messageService, commonWorkflowService, lookupRepository,
                                                                  workflowSecurityRepository, organisationStructureRepository);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        It should_throw_exception = () =>
        {
            exception.ShouldBeOfType(typeof(Exception));
            exception.ShouldNotBeNull();
        };

        It should_return_false = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}