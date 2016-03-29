using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers.Internal;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.ITC;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Capitec.Specs.Stubs;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Interfaces.ITC;
using System;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.PerformITCForApplicantCommandHandlerSpecs
{
    internal class when_an_error_message_is_returned : WithFakes
    {
        private static PerformITCForApplicantCommandHandler handler;
        private static PerformITCForApplicantCommand command;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static IApplicantDataManager applicantDataManager;
        private static IITCManager itcManager;
        private static IITCDataManager itcDataManager;
        private static IItcServiceClient itcServiceClient;
        private static ILookupManager lookupService;
        private static ISystemMessage errorMessage;
        private static ISystemMessageCollection messages;

        private static Guid applicantID;
        private static string identityNumber;

        private Establish context = () =>
        {
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            applicantDataManager = An<IApplicantDataManager>();
            itcManager = An<IITCManager>();
            itcServiceClient = An<IItcServiceClient>();
            lookupService = An<ILookupManager>();
            handler = new PerformITCForApplicantCommandHandler(unitOfWorkFactory, applicantDataManager, itcManager, itcDataManager, itcServiceClient, lookupService);

            applicantID = Guid.Parse("{4145AFF3-2002-49C2-93B5-DBBC30B15F2B}");

            var applicantStubs = new ApplicantStubs();
            var applicant = applicantStubs.GetApplicant;
            identityNumber = applicant.Information.IdentityNumber;

            command = new PerformITCForApplicantCommand(applicantID, applicant);
            var errorMessages = SystemMessageCollection.Empty();
            errorMessage = new SystemMessage("Error", SystemMessageSeverityEnum.Error);
            errorMessages.AddMessage(errorMessage);
            applicantDataManager.WhenToldTo(x => x.DidApplicantAnswerYesToDeclaration(applicantID, Param.IsAny<string>())).Return(true);
            itcServiceClient.WhenToldTo(x => x.PerformCommand(Param.IsAny<IITCServiceCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(errorMessages);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, new ServiceRequestMetadata());
        };

        private It should_return_the_error_message = () =>
        {
            messages.ErrorMessages().ShouldContain(errorMessage);
        };
    }
}