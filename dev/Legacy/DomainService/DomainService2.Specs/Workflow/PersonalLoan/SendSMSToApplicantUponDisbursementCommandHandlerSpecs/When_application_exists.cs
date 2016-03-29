using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using DomainService2.Workflow.PersonalLoan;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.PersonalLoan.SendSMSToApplicantUponDisbursementCommandHandlerSpecs
{
    public class When_application_exists : WithFakes
    {
        private static SendSMSToApplicantUponDisbursementCommand command;
        private static SendSMSToApplicantUponDisbursementCommandHandler handler;
        private static IDomainMessageCollection messages;

        private static IMessageService messageService;

        Establish context = () =>
            {
                IApplicationRepository appRepo = An<IApplicationRepository>();
                ICommonRepository commonRepo = An<ICommonRepository>();

                messageService = An<IMessageService>();

                var appInfoType = An<IApplicationInformationType>();
                appInfoType.WhenToldTo(x => x.Key).Return((int)OfferInformationTypes.AcceptedOffer);

                var appInfoPersonalLoan = An<IApplicationInformationPersonalLoan>();
                appInfoPersonalLoan.WhenToldTo(x => x.LoanAmount).Return(40000.00);


                var applicationProduct = An<IApplicationProductPersonalLoan>();
                applicationProduct.WhenToldTo(x => x.ApplicationInformationPersonalLoan).Return(appInfoPersonalLoan);

                var appInfo = An<IApplicationInformation>();
                appInfo.WhenToldTo(x => x.ApplicationInsertDate).Return(DateTime.Now);
                appInfo.WhenToldTo(x => x.ApplicationInformationType).Return(appInfoType);
                appInfo.WhenToldTo(x => x.ApplicationProduct).Return(applicationProduct);


                var appInformations = new EventList<IApplicationInformation>();
                appInformations.Add(null, appInfo);


                var appUnsecuredLending = An<IApplicationUnsecuredLending>();
                appUnsecuredLending.WhenToldTo(x => x.ApplicationInformations).Return(appInformations);


                var naturalPerson = An<ILegalEntityNaturalPerson>();
                naturalPerson.WhenToldTo(x => x.CellPhoneNumber).Return("0824445555");

                var naturalPeople = new EventList<ILegalEntityNaturalPerson>();
                naturalPeople.Add(null, naturalPerson);

                appUnsecuredLending.WhenToldTo(x => x.GetNaturalPersonsByExternalRoleType(Param.IsAny<ExternalRoleTypes>(), Param.IsAny<GeneralStatuses>())).Return(new ReadOnlyEventList<ILegalEntityNaturalPerson>(naturalPeople));
                
                appRepo.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(appUnsecuredLending);


                var template = An<ICorrespondenceTemplate>();
                template.WhenToldTo(x => x.Template).Return("Your money is in your account. All R {0} of it.");
                commonRepo.WhenToldTo(x => x.GetCorrespondenceTemplateByKey(Param.IsAny<CorrespondenceTemplates>())).Return(template);


                command = new SendSMSToApplicantUponDisbursementCommand(1);
                handler = new SendSMSToApplicantUponDisbursementCommandHandler(appRepo, messageService, commonRepo);

                messages = new DomainMessageCollection();
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_send_the_SMS_to_the_applicant = () =>
            {
                messageService.WasToldTo(x => x.SendSMS(Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>()));
            };
    }
}
