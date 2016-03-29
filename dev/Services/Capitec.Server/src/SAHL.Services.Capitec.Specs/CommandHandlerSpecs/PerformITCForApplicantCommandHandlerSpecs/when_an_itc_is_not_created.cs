using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
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
using SAHL.Services.Interfaces.ITC.Queries;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.PerformITCForApplicantCommandHandlerSpecs
{
    public class when_an_itc_is_not_created : WithFakes
    {
        private static PerformITCForApplicantCommandHandler handler;
        private static PerformITCForApplicantCommand command;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static IApplicantDataManager applicantDataManager;
        private static IITCManager itcManager;
        private static IITCDataManager itcDataManager;
        private static IItcServiceClient itcServiceClient;
        private static ILookupManager lookupService;
        private static ISystemMessageCollection messages;

        private static Applicant applicant;
        private static ApplicantITCRequestDetailsModel itcRequest;
        private static Guid applicantID, itcID;
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

            var applicantStubs = new ApplicantStubs();
            applicant = applicantStubs.GetApplicant;
            identityNumber = applicant.Information.IdentityNumber;
            itcRequest = new ApplicantITCRequestDetailsModel("", "", DateTime.Now, identityNumber, "", "", "", "", "", "", "", "", "", "");

            command = new PerformITCForApplicantCommand(applicantID, applicant);

            lookupService.WhenToldTo(x => x.GenerateCombGuid()).Return(itcID);
            itcManager.WhenToldTo(x => x.CreateApplicantITCRequest(applicant)).Return(itcRequest);
            applicantDataManager.WhenToldTo(x => x.DidApplicantAnswerYesToDeclaration(applicantID, Param.IsAny<string>())).Return(true);
            var serviceQueryResult = new ServiceQueryResult<ITCRequestDataModel>(new ITCRequestDataModel[]
                {
                });

            itcServiceClient.WhenToldTo(x => x.PerformQuery(Param<GetITCQuery>.Matches(m =>
                m.ItcID == itcID))).Return<GetITCQuery>(y => { y.Result = serviceQueryResult; return SystemMessageCollection.Empty(); });
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, new ServiceRequestMetadata());
        };

        private It should_check_if_the_applicant_answered_yes_to_credit_check_declaration = () =>
        {
            applicantDataManager.WasToldTo(x => x.DidApplicantAnswerYesToDeclaration(applicantID, DeclarationDefinitions.AllowCreditBureauCheck));
        };

        private It should_check_if_a_valid_itc_exists_for_the_applicant = () =>
        {
            itcManager.WasToldTo(x => x.GetValidITCModelForPerson(identityNumber));
        };

        private It should_create_the_applicant_itc_details = () =>
        {
            itcManager.WasToldTo(x => x.CreateApplicantITCRequest(applicant));
        };

        private It should_perform_the_itc_using_the_itc_service = () =>
        {
            itcServiceClient.WasToldTo(x => x.PerformCommand(Param<PerformCapitecITCCheckCommand>.Matches(m =>
                m.ItcID == itcID &&
                m.ApplicantITCRequestDetails == itcRequest),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_try_get_the_newly_created_itc = () =>
        {
            itcServiceClient.WasToldTo(x => x.PerformQuery(Param<GetITCQuery>.Matches(m =>
                m.ItcID == itcID)));
        };

        private It should_not_link_the_itc_to_the_person = () =>
        {
            itcManager.WasNotToldTo(x => x.LinkItcToPerson(Param.IsAny<Guid>(), Param.IsAny<Guid>(), Param.IsAny<DateTime>()));
        };

        private It should_return_an_error_message = () =>
        {
            messages.HasErrors.ShouldBeTrue();
            messages.ErrorMessages().First().Message.ShouldEqual("Failed to perform the itc check");
        };
    }
}