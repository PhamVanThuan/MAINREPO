using System.Collections.Generic;
using DomainService2.SharedServices;
using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using X2DomainService.Interface.Common;

namespace DomainService2.Specs.Workflow.DebtCounselling.CancelDebtCounselling_EXTCommandHandlerSpecs
{
    [Subject(typeof(CancelDebtCounselling_EXTCommandHandler))]
    public class When_debtcounsellor_not_exists : DomainServiceSpec<CancelDebtCounselling_EXTCommand, CancelDebtCounselling_EXTCommandHandler>
    {
        protected static IDebtCounsellingRepository debtCounsellingRepository;
        protected static IDebtCounselling debtCounsellingCase;
        protected static ICommonRepository commonRepository;
        protected static IMessageService messageService;
        protected static IX2WorkflowService x2WorkflowService;
        protected static ICommon commonWorkflowService;
        protected static ILookupRepository lookupRepository;
        protected static IDebtCounsellingStatus debtCounsellingStatus;
        protected static IWorkflowSecurityRepository workflowSecurityRepository;
        protected static IOrganisationStructureRepository organisationStructureRepository;

        Establish context = () =>
        {
            debtCounsellingRepository = An<IDebtCounsellingRepository>();
            x2WorkflowService = An<IX2WorkflowService>();
            lookupRepository = An<ILookupRepository>();

            ILegalEntity debtCounsellor = null;

            debtCounsellingCase = An<IDebtCounselling>();
            debtCounsellingStatus = An<IDebtCounsellingStatus>();
            x2WorkflowService.WhenToldTo(x => x.HasInstancePerformedActivity(Param.IsAny<long>(), Param.IsAny<string>())).Return(false);
            
            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param.IsAny<int>())).Return(debtCounsellingCase);
            debtCounsellingCase.WhenToldTo(x => x.DebtCounsellor).Return(debtCounsellor);

            IDictionary<DebtCounsellingStatuses, IDebtCounsellingStatus> debtcounsellingStatuses = new Dictionary<DebtCounsellingStatuses, IDebtCounsellingStatus>();
            debtcounsellingStatuses.Add(DebtCounsellingStatuses.Cancelled, debtCounsellingStatus);
            lookupRepository.WhenToldTo(x => x.DebtCounsellingStatuses).Return(debtcounsellingStatuses);

            command = new CancelDebtCounselling_EXTCommand(1, "test", 1);
            handler = new CancelDebtCounselling_EXTCommandHandler(x2WorkflowService, debtCounsellingRepository, commonRepository,
                                                                  messageService, commonWorkflowService, lookupRepository,
                                                                  workflowSecurityRepository, organisationStructureRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_throw_error = () =>
        {
            messages.Count.Equals(1);
        };

        It should_save_debt_counselling_record = () =>
        {
            debtCounsellingRepository.WasToldTo(x => x.SaveDebtCounselling(debtCounsellingCase));
        };

        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };
    }
}