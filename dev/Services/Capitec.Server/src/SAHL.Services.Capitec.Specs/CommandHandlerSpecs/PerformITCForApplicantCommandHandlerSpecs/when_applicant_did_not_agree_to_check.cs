using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Capitec.CommandHandlers.Internal;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.ITC;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Capitec.Specs.Stubs;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Interfaces.Capitec.Common;
using SAHL.Services.Interfaces.ITC;
using SAHL.Services.Interfaces.ITC.Commands;
using System;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.PerformITCForApplicantCommandHandlerSpecs
{
    internal class when_applicant_did_not_agree_to_check : WithFakes
    {
        private static Exception exception;

        private static PerformITCForApplicantCommandHandler handler;
        private static PerformITCForApplicantCommand command;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static IApplicantDataManager applicantDataManager;
        private static IITCManager itcManager;
        private static IITCDataManager itcDataManager;
        private static IItcServiceClient itcServiceClient;
        private static ILookupManager lookupService;

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
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => handler.HandleCommand(command, new ServiceRequestMetadata()));
        };

        private It should_check_if_the_applicant_answered_yes_to_credit_check_declaration = () =>
        {
            applicantDataManager.WasToldTo(x => x.DidApplicantAnswerYesToDeclaration(applicantID, DeclarationDefinitions.AllowCreditBureauCheck));
        };

        private It should_throw_an_exception = () =>
        {
            exception.ShouldNotBeNull();
            exception.ShouldBeOfExactType<InvalidOperationException>();
        };

        private It should_not_check_if_a_valid_itc_exists_for_the_applicant = () =>
        {
            itcManager.WasNotToldTo(x => x.GetValidITCModelForPerson(identityNumber));
        };

        private It should_not_perform_the_itc_using_the_itc_service = () =>
        {
            itcServiceClient.WasNotToldTo(x => x.PerformCommand(
                Param.IsAny<PerformCapitecITCCheckCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_link_the_itc_to_the_person = () =>
        {
            itcManager.WasNotToldTo(x => x.LinkItcToPerson(Param.IsAny<Guid>(), Param.IsAny<Guid>(), Param.IsAny<DateTime>()));
        };
    }
}