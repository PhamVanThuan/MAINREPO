using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicantDeclarations
{
    public class when_adding_valid_declarations_for_an_applicant : WithCoreFakes
    {
        private static AddApplicantDeclarationsCommand command;
        private static AddApplicantDeclarationsCommandHandler handler;
        private static ISystemMessageCollection expectedMessages;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static IDomainRuleManager<ApplicantDeclarationsModel> domainRuleManager;
        private static ApplicantDeclarationsModel applicantDeclarations;
        private static OfferRoleDataModel applicantRole;
        private static IDomainQueryServiceClient domainQueryService;
        private static int totalDeclarationsToSave;
        private static int ApplicationNumber = 76;
        private static int ClientKey = 1234;
        private static OfferDeclarationDataModel insolvencyRehabDateDeclaration;
        private static OfferDeclarationDataModel declaredInsolventDeclaration;
        private static OfferDeclarationDataModel adminOrderDeclaration;
        private static OfferDeclarationDataModel adminOrderRescindedDateDeclaration;
        private static OfferDeclarationDataModel underDebtCounsellingDeclaration;
        private static OfferDeclarationDataModel hasDebtArrangementDeclaration;
        private static OfferDeclarationDataModel permissionToConductCreditCheckDeclaration;

        private Establish context = () =>
        {
            totalDeclarationsToSave = 7;
            expectedMessages = SystemMessageCollection.Empty();
            applicationDataManager = An<IApplicationDataManager>();
            domainRuleManager = An<IDomainRuleManager<ApplicantDeclarationsModel>>();
            domainQueryService = An<IDomainQueryServiceClient>();
            applicantDataManager = An<IApplicantDataManager>();
            handler = new AddApplicantDeclarationsCommandHandler(serviceCommandRouter, applicationDataManager
                , domainQueryService, domainRuleManager, eventRaiser, applicantDataManager, unitOfWorkFactory);
            applicantDeclarations = new ApplicantDeclarationsModel(ClientKey, ApplicationNumber, DateTime.Now,
                new DeclaredInsolventDeclarationModel(OfferDeclarationAnswer.Yes, DateTime.Now.AddYears(-2)),
                new UnderAdministrationOrderDeclarationModel(OfferDeclarationAnswer.No, DateTime.Now.AddYears(-2)),
                new CurrentlyUnderDebtCounsellingReviewDeclarationModel(OfferDeclarationAnswer.No, null),
                new PermissionToConductCreditCheckDeclarationModel(OfferDeclarationAnswer.Yes));
            command = new AddApplicantDeclarationsCommand(applicantDeclarations);

            applicationDataManager.WhenToldTo(x => x.DoesApplicationExist(Param.IsAny<int>())).Return(true);
            applicantDataManager.WhenToldTo(x => x.CheckClientIsAnApplicantOnTheApplication(Param.IsAny<int>(), Param.IsAny<int>())).Return(true);
            applicantRole = new OfferRoleDataModel(1, 2, 3, 1, DateTime.Now);
            IEnumerable<OfferRoleDataModel> applicantRoles = new OfferRoleDataModel[] { applicantRole };
            applicantDataManager.WhenToldTo(x => x.GetActiveClientRoleOnApplication(Param.IsAny<int>(), Param.IsAny<int>())).Return(applicantRoles);
            int offerRoleKey = applicantRoles.First().OfferRoleKey;
            insolvencyRehabDateDeclaration = new OfferDeclarationDataModel(offerRoleKey, (int)OfferDeclarationQuestionEnum.InsolventRehabilitationDateQuestionKey,
                (int)OfferDeclarationAnswer.Date, command.ApplicantDeclarations.DeclaredInsolventDeclaration.DateRehabilitated);
            declaredInsolventDeclaration = new OfferDeclarationDataModel(offerRoleKey, (int)OfferDeclarationQuestionEnum.DeclaredInsolventQuestionKey,
                (int)command.ApplicantDeclarations.DeclaredInsolventDeclaration.Answer, null);
            adminOrderDeclaration = new OfferDeclarationDataModel(offerRoleKey, (int)OfferDeclarationQuestionEnum.UnderAdministrationOrderQuestionKey,
                (int)command.ApplicantDeclarations.UnderAdministrationOrderDeclaration.Answer, null);
            adminOrderRescindedDateDeclaration = new OfferDeclarationDataModel(offerRoleKey, (int)OfferDeclarationQuestionEnum.AdministrationOrderDateRescindedQuestionKey,
                (int)OfferDeclarationAnswer.Date, command.ApplicantDeclarations.UnderAdministrationOrderDeclaration.DateAdministrationOrderRescinded);
            underDebtCounsellingDeclaration = new OfferDeclarationDataModel(offerRoleKey, (int)OfferDeclarationQuestionEnum.CurrentlyUnderDebtCounsellingQuestionKey,
                (int)command.ApplicantDeclarations.CurrentlyUnderDebtReviewDeclaration.Answer, null);
            hasDebtArrangementDeclaration = new OfferDeclarationDataModel(offerRoleKey, (int)OfferDeclarationQuestionEnum.HasCurrentDebtArrangementQuestionKey,
                (int)OfferDeclarationAnswer.No, null);
            permissionToConductCreditCheckDeclaration = new OfferDeclarationDataModel(offerRoleKey, (int)OfferDeclarationQuestionEnum.PermissionToConductCreditCheckQuestionKey,
                (int)command.ApplicantDeclarations.PermissiontoConductCreditCheckDeclaration.Answer, null);

        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_get_the_applicants_role_for_the_application = () =>
        {
            applicantDataManager.WasToldTo(x => x.GetActiveClientRoleOnApplication(applicantDeclarations.ApplicationNumber, applicantDeclarations.ClientKey));
        };

        private It should_save_all_the_declarations = () =>
        {
            applicantDataManager.WasToldTo(x => x.SaveApplicantDeclarations(
                Arg.Is<IEnumerable<OfferDeclarationDataModel>>(y => y.Count() == totalDeclarationsToSave)));
        };

        private It should_raise_a_declarations_added_to_applicant_Event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<DeclarationsAddedToApplicantEvent>
                (y => y.ApplicationNumber == ApplicationNumber && y.ClientKey == ClientKey),
                    Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_save_the_declared_insolvent_declaration = () =>
        {
            applicantDataManager.Received().SaveApplicantDeclarations(Arg.Is<IEnumerable<OfferDeclarationDataModel>>(y =>
                y.Contains(declaredInsolventDeclaration, new GenericEqualityComparer<OfferDeclarationDataModel>())));
        };

        private It should_save_the_rehabilitation_date_declaration = () =>
        {
            applicantDataManager.Received().SaveApplicantDeclarations(Arg.Is<IEnumerable<OfferDeclarationDataModel>>(y =>
                y.Contains(insolvencyRehabDateDeclaration, new GenericEqualityComparer<OfferDeclarationDataModel>())));
        };

        private It should_save_the_under_admin_order_declaration = () =>
        {
            applicantDataManager.Received().SaveApplicantDeclarations(Arg.Is<IEnumerable<OfferDeclarationDataModel>>(y =>
                y.Contains(adminOrderDeclaration, new GenericEqualityComparer<OfferDeclarationDataModel>())));
        };

        private It should_save_the_admin_order_rescinded_date = () =>
        {
            applicantDataManager.Received().SaveApplicantDeclarations(Arg.Is<IEnumerable<OfferDeclarationDataModel>>(y =>
                y.Contains(adminOrderRescindedDateDeclaration, new GenericEqualityComparer<OfferDeclarationDataModel>())));
        };

        private It should_save_a_under_debt_counselling_declaration = () =>
        {
            applicantDataManager.Received().SaveApplicantDeclarations(Arg.Is<IEnumerable<OfferDeclarationDataModel>>(y =>
                y.Contains(underDebtCounsellingDeclaration, new GenericEqualityComparer<OfferDeclarationDataModel>())));
        };

        private It should_save_the_current_debt_arrangement_declaration = () =>
        {
            applicantDataManager.Received().SaveApplicantDeclarations(Arg.Is<IEnumerable<OfferDeclarationDataModel>>(y =>
                y.Contains(hasDebtArrangementDeclaration, new GenericEqualityComparer<OfferDeclarationDataModel>())));
        };

        private It should_save_the_permission_to_conduct_credit_check_declaration = () =>
        {
            applicantDataManager.Received().SaveApplicantDeclarations(Arg.Is<IEnumerable<OfferDeclarationDataModel>>(y =>
                y.Contains(permissionToConductCreditCheckDeclaration, new GenericEqualityComparer<OfferDeclarationDataModel>())));
        };
    }
}