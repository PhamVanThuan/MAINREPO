using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;

namespace DomainService2.Specs.SharedServices.Common.UpdateAssignedUserInIDMCommandHandlerSpecs
{
    [Subject(typeof(UpdateAssignedUserInIDMCommandHandler))]
    public class When_update_assigned_user_in_idm : DomainServiceSpec<UpdateAssignedUserInIDMCommand, UpdateAssignedUserInIDMCommandHandler>
    {
        protected static ICastleTransactionsService castleTransactionsService;
        protected static IApplicationRepository applicationRepository;
        protected static IWorkflowSecurityRepository securityRepository;

        protected static IApplication application;
        protected static IAccountSequence reservedAccount;
        protected static IApplicationStatus applicationStatus;

        protected static SAHL.Common.BusinessModel.Interfaces.Repositories.WorkflowRole.WorkflowAssignment.ADUserRow aduser;

        Establish context = () =>
        {
            castleTransactionsService = An<ICastleTransactionsService>();
            applicationRepository = An<IApplicationRepository>();
            securityRepository = An<IWorkflowSecurityRepository>();

            application = An<IApplication>();
            reservedAccount = An<IAccountSequence>();
            applicationStatus = An<IApplicationStatus>();
            var dataset = new SAHL.Common.BusinessModel.Interfaces.Repositories.WorkflowRole.WorkflowAssignment();
            var adUserRow = dataset.ADUser.NewADUserRow();
            //aduser = new SAHL.Common.BusinessModel.Interfaces.Repositories.WorkflowRole.WorkflowAssignment.ADUserRow();
            //aduser = securityRepository.GetADUser(1);
            //aduser = An<SAHL.Common.BusinessModel.Interfaces.Repositories.WorkflowRole.WorkflowAssignment.ADUserRow>();

            bool isFurtherLoan = true;

            // this gets the aduserkey
            castleTransactionsService.WhenToldTo(x => x.ExecuteScalarOnCastleTran(Param<string>.IsAnything, Param<SAHL.Common.Globals.Databases>.IsAnything, Param<SAHL.Common.DataAccess.ParameterCollection>.IsAnything)).Return(1);
            // get the application
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            application.WhenToldTo(x => x.ReservedAccount).Return(reservedAccount);
            reservedAccount.WhenToldTo(x => x.Key).Return(1);

            application.WhenToldTo(x => x.ApplicationStatus).Return(applicationStatus);
            applicationStatus.WhenToldTo(x => x.Key).Return(1);

            adUserRow.ADUserName = "Blah";
            securityRepository.WhenToldTo(x => x.GetADUser(Param<int>.IsAnything)).Return(adUserRow);

            command = new UpdateAssignedUserInIDMCommand(Param.IsAny<int>(), isFurtherLoan, Param.IsAny<long>(), Param.IsAny<string>());
            handler = new UpdateAssignedUserInIDMCommandHandler(castleTransactionsService, applicationRepository, securityRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_execute_stored_proc_pr_UpdateAssignedUserAndStateFromX2 = () =>
        {
            castleTransactionsService.WasToldTo(x => x.ExecuteNonQueryOnCastleTran(Param.IsAny<string>(), Param.IsAny<SAHL.Common.Globals.Databases>(), Param<SAHL.Common.DataAccess.ParameterCollection>.IsAnything));
        };
    }
}