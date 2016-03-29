using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
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
using SAHL.Services.Interfaces.ITC.Queries;
using System.Collections.Generic;
using System;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.PerformITCForApplicantCommandHandlerSpecs
{
    public class when_applicant_does_not_have_existing_itc : WithFakes
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
        private static Guid personID, applicantID, itcID;
        private static DateTime itcDate;
        private static string identityNumber;
        private static bool hasPerformedITC;
        private static ITCRequestDataModel itcModel;


        private Establish context = () =>
        {
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            applicantDataManager = An<IApplicantDataManager>();
            itcManager = An<IITCManager>();
            itcDataManager = An<IITCDataManager>();
            itcServiceClient = An<IItcServiceClient>();
            lookupService = An<ILookupManager>();
            handler = new PerformITCForApplicantCommandHandler(unitOfWorkFactory, applicantDataManager, itcManager, itcDataManager, itcServiceClient, lookupService);

            applicantID = Guid.Parse("{4145AFF3-2002-49C2-93B5-DBBC30B15F2B}");
            itcID = Guid.Parse("{96D21727-1574-4AD8-AAA1-E0E0E61D0E04}");
            personID = Guid.Parse("{D1BDC45D-23A0-4273-A69A-2997F55851BB}");
            itcDate = new DateTime(2015, 01, 08);

            var applicantStubs = new ApplicantStubs();
            applicant = applicantStubs.GetApplicant;
            identityNumber = applicant.Information.IdentityNumber;
            itcRequest = new ApplicantITCRequestDetailsModel("", "", DateTime.Now, identityNumber, "", "", "", "", "", "", "", "", "", "");

            command = new PerformITCForApplicantCommand(applicantID, applicant);
            itcModel = new ITCRequestDataModel(itcID, itcDate, "");
            itcProfile = new ItcProfile(500, null, null, null, null, null, true);

            lookupService.WhenToldTo(x => x.GenerateCombGuid()).Return(itcID);
            itcManager.WhenToldTo(x => x.CreateApplicantITCRequest(applicant)).Return(itcRequest);
            applicantDataManager.WhenToldTo(x => x.DidApplicantAnswerYesToDeclaration(applicantID, Param.IsAny<string>())).Return(true);
            applicantDataManager.WhenToldTo(x => x.GetPersonID(identityNumber)).Return(personID);
            var serviceQueryResult = new ServiceQueryResult<ITCRequestDataModel>(new ITCRequestDataModel[]
                {
                    itcModel
                });

            itcServiceClient.WhenToldTo(x => x.PerformQuery(Param<GetITCQuery>.Matches(m =>
                m.ItcID == itcID))).Return<GetITCQuery>(y => { y.Result = serviceQueryResult; return SystemMessageCollection.Empty(); });
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

        private It should_get_itc_from_the_itc_service = () =>
        {
            itcServiceClient.WasToldTo(x => x.PerformQuery(Param<GetITCQuery>.Matches(m =>
                m.ItcID == itcID)));
        };

        private It should_save_itc = () =>
        {
            itcDataManager.WasToldTo(x => x.SaveITC(itcModel.Id, itcModel.ITCDate, itcModel.ITCData));
        };

        private It should_get_the_person_for_the_applicant = () =>
        {
            applicantDataManager.WasToldTo(x => x.GetPersonID(identityNumber));
        };

        private It should_link_the_itc_to_the_person = () =>
        {
            itcManager.WasToldTo(x => x.LinkItcToPerson(personID, itcID, itcDate));
        };
    }
}