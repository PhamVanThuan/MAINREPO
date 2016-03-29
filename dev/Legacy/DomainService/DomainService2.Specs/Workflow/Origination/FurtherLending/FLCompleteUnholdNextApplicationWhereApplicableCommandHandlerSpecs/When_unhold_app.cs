using DomainService2.Specs.DomainObjects;
using DomainService2.Workflow.Origination.FurtherLending;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.FurtherLending.IsReadvanceAdvanceApplicationCommandHandlerSpecs
{
    [Subject(typeof(FLCompleteUnholdNextApplicationWhereApplicableCommandHandler))]
    public class When_unhold_app : WithFakes
    {
        static IAccount account;
        static IApplicationRepository applicationRepository;
        static IX2Repository x2Repository;
        static FLCompleteUnholdNextApplicationWhereApplicableCommand command;
        static FLCompleteUnholdNextApplicationWhereApplicableCommandHandler handler;
        static IDomainMessageCollection messages;
        static IWorkFlow workflow;

        Establish context = () =>
            {
                IApplication app = An<IApplication>();
                IApplicationType appType = An<IApplicationType>();
                IApplicationStatus appStatus = An<IApplicationStatus>();
                IEventList<IApplication> applications = new StubEventList<IApplication>();
                IInstance iID = An<IInstance>();
                workflow = An<IWorkFlow>();
                applicationRepository = An<IApplicationRepository>();
                x2Repository = An<IX2Repository>();
                //IEventList<IApplication> applications
                account = An<IAccount>();

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(app);
                app.WhenToldTo(x => x.Account).Return(account);
                app.WhenToldTo(x => x.ApplicationType).Return(appType);
                app.WhenToldTo(x => x.ApplicationStatus).Return(appStatus);
                app.WhenToldTo(x => x.Key).Return(1);

                appType.WhenToldTo(x => x.Key).Return((int)OfferTypes.FurtherAdvance);

                appStatus.WhenToldTo(x => x.Key).Return((int)OfferStatuses.Open);

                account.WhenToldTo(x => x.Applications).Return(applications);

                iID.WhenToldTo(x => x.ID).Return(1);
                workflow.WhenToldTo(x => x.Name).Return("");

                x2Repository.WhenToldTo(x => x.GetWorkFlowByName(Param.IsAny<string>(), Param.IsAny<string>())).Return(workflow);

                x2Repository.WhenToldTo(x => x.GetInstanceForGenericKey(Param.IsAny<int>(), Param.Is<string>(SAHL.Common.Constants.WorkFlowName.ApplicationManagement), Param.Is<string>(SAHL.Common.Constants.WorkFlowProcessName.Origination))).Return(iID);

                messages = new DomainMessageCollection();

                applications.Add(messages, app);

                command = new FLCompleteUnholdNextApplicationWhereApplicableCommand(1111);
                handler = new FLCompleteUnholdNextApplicationWhereApplicableCommandHandler(applicationRepository, x2Repository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_create_and_save_activity = () =>
            {
                x2Repository.WasToldTo(x => x.CreateAndSaveActiveExternalActivity(Param.Is<string>(Constants.WorkFlowExternalActivity.ReturnApplicationFromHold), Param.IsAny<long>(), Param.IsAny<string>(), Param.Is<string>(SAHL.Common.Constants.WorkFlowProcessName.Origination), Param.IsAny<string>()));
            };
    }
}