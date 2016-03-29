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
using SAHL.Common.BusinessModel.Interfaces.Repositories.WorkflowRole;

namespace DomainService2.Specs.Workflow.DebtCounselling.CancelDebtCounselling_EXTCommandHandlerSpecs
{
    [Subject(typeof(CancelDebtCounselling_EXTCommandHandler))]
    public class When_stagetransition_exists : DomainServiceSpec<CancelDebtCounselling_EXTCommand, CancelDebtCounselling_EXTCommandHandler>
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
            commonRepository = An<ICommonRepository>();
            messageService = An<IMessageService>();
            x2WorkflowService = An<IX2WorkflowService>();
            commonWorkflowService = An<ICommon>();
            lookupRepository = An<ILookupRepository>();

            workflowSecurityRepository = An<IWorkflowSecurityRepository>();
            organisationStructureRepository = An<IOrganisationStructureRepository>();

            MockRepository mocks = new MockRepository();

            IADUser adUser = An<IADUser>();

            debtCounsellingStatus = An<IDebtCounsellingStatus>();

            x2WorkflowService.WhenToldTo(x => x.HasInstancePerformedActivity(Param.IsAny<long>(), Param.IsAny<string>())).Return(true);

            debtCounsellingCase = An<IDebtCounselling>();
            IAccount account = An<IAccount>();
            account.WhenToldTo(x => x.Key).Return(Param.IsAny<int>());
            debtCounsellingCase.WhenToldTo(x => x.Account).Return(account);

            IList<ILegalEntity> clients = new List<ILegalEntity>();
            debtCounsellingCase.WhenToldTo(x => x.Clients).Return(clients);

            ILegalEntity debtCounsellor = An<ILegalEntity>();
            ICorrespondenceTemplate correspondenceTemplate = An<ICorrespondenceTemplate>();
            correspondenceTemplate.WhenToldTo(x => x.Subject).Return("test");
            correspondenceTemplate.WhenToldTo(x => x.Template).Return("test");

            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param.IsAny<int>())).Return(debtCounsellingCase);
            debtCounsellingCase.WhenToldTo(x => x.DebtCounsellor).Return(debtCounsellor);
            debtCounsellor.WhenToldTo(x => x.EmailAddress).Return(Param.IsAny<string>());

            commonRepository.WhenToldTo(x => x.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.MortgageLoanCancelledContinuePaying)).Return(correspondenceTemplate);

            messageService.WhenToldTo(x => x.SendEmailExternal(Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>(),
                                                               Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(),
                                                               Param.IsAny<string>(), Param.IsAny<string>())).Return(true);

            string eStageName = "test";
            string eFolderID = "";
            debtCounsellingRepository.Expect(x => x.GetEworkDataForLossControlCase(Param.IsAny<int>(), out eStageName, out eFolderID, out adUser)).OutRef("test", "test", adUser);

            WorkflowAssignment assign = new WorkflowAssignment();
            assign.Tables.Add(new DataTable("WFAssignment"));
            assign.WFRAssignment.AddWFRAssignmentRow(Param<int>.IsAnything, Param<long>.IsAnything, Param<int>.IsNotNull, Param<int>.IsAnything, Param<int>.IsAnything, "test", Param<int>.IsAnything, Param<string>.IsAnything, Param<int>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<int>.IsAnything, Param<int>.IsAnything, Param<string>.IsAnything);
            workflowSecurityRepository.WhenToldTo(x => x.GetWorkflowRoleAssignment(Param<List<WorkflowRoleTypes>>.IsAnything, Param<long>.IsAnything)).Return(assign);

            mocks.ReplayAll();

            commonWorkflowService.WhenToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(),
                                                                       Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<string>(),
                                                                       Param.IsAny<string>())).Return(true);

            IDictionary<DebtCounsellingStatuses, IDebtCounsellingStatus> debtcounsellingStatuses = new Dictionary<DebtCounsellingStatuses, IDebtCounsellingStatus>();
            debtcounsellingStatuses.Add(DebtCounsellingStatuses.Cancelled, debtCounsellingStatus);
            lookupRepository.WhenToldTo(x => x.DebtCounsellingStatuses).Return(debtcounsellingStatuses);

            organisationStructureRepository.WhenToldTo(x => x.GetADUserByKey(Param.IsAny<int>())).Return(Param.IsAny<IADUser>());

            command = new CancelDebtCounselling_EXTCommand(1, "test", 1);
            handler = new CancelDebtCounselling_EXTCommandHandler(x2WorkflowService, debtCounsellingRepository, commonRepository,
                                                                  messageService, commonWorkflowService, lookupRepository,
                                                                  workflowSecurityRepository, organisationStructureRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_send_notification = () =>
        {
            debtCounsellingRepository.WasToldTo(x => x.SendNotification(Param.IsAny<IDebtCounselling>()));
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