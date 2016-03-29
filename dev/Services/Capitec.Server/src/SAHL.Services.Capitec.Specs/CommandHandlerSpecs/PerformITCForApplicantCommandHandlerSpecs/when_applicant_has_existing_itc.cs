using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.Services.Capitec.CommandHandlers.Internal;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.ITC;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Capitec.Specs.Stubs;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Interfaces.Capitec.Common;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using SAHL.Services.Interfaces.ITC;
using SAHL.Services.Interfaces.ITC.Commands;
using SAHL.Services.Interfaces.ITC.Models;
using System;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.PerformITCForApplicantCommandHandlerSpecs
{
    public class when_applicant_has_existing_itc : WithFakes
    {
        private static PerformITCForApplicantCommandHandler handler;
        private static PerformITCForApplicantCommand command;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static IApplicantDataManager applicantDataManager;
        private static IITCManager itcManager;
        private static IITCDataManager itcDataManager;
        private static IItcServiceClient itcServiceClient;
        private static ILookupManager lookupService;

        private static Applicant applicant;
        private static ApplicantITCRequestDetailsModel itcRequest;
        private static ItcProfile itcProfile;
        private static Guid applicantID, itcID;
        private static DateTime itcDate;
        private static string identityNumber;
        private static bool hasPerformedITC;

        private Establish context = () =>
        {
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            applicantDataManager = An<IApplicantDataManager>();
            itcManager = An<IITCManager>();
            itcServiceClient = An<IItcServiceClient>();
            lookupService = An<ILookupManager>();
            handler = new PerformITCForApplicantCommandHandler(unitOfWorkFactory, applicantDataManager, itcManager, itcDataManager, itcServiceClient, lookupService);

            applicantID = Guid.Parse("{4145AFF3-2002-49C2-93B5-DBBC30B15F2B}");
            itcID = Guid.Parse("{96D21727-1574-4AD8-AAA1-E0E0E61D0E04}");
            itcDate = new DateTime(2015, 01, 08);

            var applicantStubs = new ApplicantStubs();
            applicant = applicantStubs.GetApplicant;
            identityNumber = applicant.Information.IdentityNumber;
            itcRequest = new ApplicantITCRequestDetailsModel("", "", DateTime.Now, identityNumber, "", "", "", "", "", "", "", "", "", "");

            command = new PerformITCForApplicantCommand(applicantID, applicant);
            var itcModel = new ITCDataModel(itcID, itcDate, "");
            itcProfile = new ItcProfile(500, null, null, null, null, null, true);

            lookupService.WhenToldTo(x => x.GenerateCombGuid()).Return(itcID);
            itcManager.WhenToldTo(x => x.CreateApplicantITCRequest(applicant)).Return(itcRequest);
            applicantDataManager.WhenToldTo(x => x.DidApplicantAnswerYesToDeclaration(applicantID, Param.IsAny<string>())).Return(true);
            itcManager.WhenToldTo(x => x.GetValidITCModelForPerson(Param.IsAny<string>())).Return(itcModel);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, new ServiceRequestMetadata());
        };

        private It should_check_if_the_applicant_answered_yes_to_credit_check_declaration = () =>
        {
            applicantDataManager.WasToldTo(x => x.DidApplicantAnswerYesToDeclaration(applicantID, DeclarationDefinitions.AllowCreditBureauCheck));
        };

        private It should_check_if_a_valid_itc_exists_for_the_applicant = () =>
        {
            itcManager.WasToldTo(x => x.GetValidITCModelForPerson(identityNumber));
        };

        private It should_not_create_the_applicant_itc_details = () =>
        {
            itcManager.WasNotToldTo(x => x.CreateApplicantITCRequest(Param.IsAny<Applicant>()));
        };

        private It should_not_perform_the_itc_using_the_itc_service = () =>
        {
            itcServiceClient.WasNotToldTo(x => x.PerformCommand(
                Param.IsAny<PerformCapitecITCCheckCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_only_get_the_itc_for_the_applicant_once = () =>
        {
            itcManager.WasToldTo(x => x.GetValidITCModelForPerson(identityNumber)).Times(1);
        };

        private It should_not_link_the_itc_to_the_person = () =>
        {
            itcManager.WasNotToldTo(x => x.LinkItcToPerson(Param.IsAny<Guid>(), Param.IsAny<Guid>(), Param.IsAny<DateTime>()));
        };
    }
}