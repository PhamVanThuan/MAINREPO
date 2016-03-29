using DomainService2.Workflow.Origination.ApplicationCapture;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections.Generic;

namespace DomainService2.Specs.Workflow.Origination.ApplicationCapture.SendNewClientConsultantDetailsSMSCommandHandlerSpecs
{
    public class base_send_new_client_consultant_details_sms_spec_for_passing_tests : DomainServiceSpec<SendNewClientConsultantDetailsSMSCommand, SendNewClientConsultantDetailsSMSCommandHandler>
    {
        protected static IMessageService messageService;
		protected static IApplicationRepository applicationRepository;
		protected static ICommonRepository commonRepository;
		protected static ICorrespondenceTemplate correspondenceTemplate;
		protected static ILookupRepository lookupRepository;
		protected static string template;
		protected static string renderedTemplate;
		protected static int applicationKey = 12345;
		protected static string mainApplicantCellphoneNumber = "0734448888";
		protected static string consultantCellphoneNumber = "0724449999";
		protected static string consultantName = "FirstName";
		protected static ILegalEntity mainApplicant;
		protected static ILegalEntityNaturalPerson consultant;
		protected static IDictionary<string, IControl> objectDictionary;
		protected static string clientCallCentreNumber = "0860 000 0000";
		protected static IAccountSequence reservedAccount;
		protected static IApplication application;
		protected static IControl clientCallCentrePhoneNumberControl;
		protected static ILegalEntity[] mainApplicants;
		protected static ILegalEntity[] consultants;
        public base_send_new_client_consultant_details_sms_spec_for_passing_tests()
		{
            template = "Thank you for choosing SA Home Loans. Your consultant is {0}, contact number {1}. Your reference number is {2}";

			applicationRepository = An<IApplicationRepository>();
			messageService = An<IMessageService>();
			commonRepository = An<ICommonRepository>();
			correspondenceTemplate = An<ICorrespondenceTemplate>();
			lookupRepository = An<ILookupRepository>();

			application = An<IApplication>();
			reservedAccount = An<IAccountSequence>();

			clientCallCentrePhoneNumberControl = An<IControl>();

			objectDictionary = new Dictionary<string, IControl>();
			objectDictionary.Add(SAHL.Common.Constants.ControlTable.HelpDeskContactNumber, clientCallCentrePhoneNumberControl);

			mainApplicant = An<ILegalEntity>();
			consultant = An<ILegalEntityNaturalPerson>();

			mainApplicants = new[] { mainApplicant };
			consultants = new[] { consultant };
		}
		public static void Setup()
		{
			clientCallCentrePhoneNumberControl.WhenToldTo(x => x.ControlText).Return(clientCallCentreNumber);
			application.WhenToldTo(x => x.Key).Return(applicationKey);
			applicationRepository.WhenToldTo(x => x.GetApplicationByKey(applicationKey)).Return(application);
			mainApplicant.WhenToldTo(x => x.CellPhoneNumber).Return(mainApplicantCellphoneNumber);
            consultant.WhenToldTo(x => x.FirstNames).Return(consultantName);
			consultant.WhenToldTo(x => x.CellPhoneNumber).Return(consultantCellphoneNumber);
            renderedTemplate = String.Format(template, consultant.FirstNames, consultant.CellPhoneNumber, application.Key);
			correspondenceTemplate.WhenToldTo(x => x.Template).Return(template);
			application.WhenToldTo(x => x.GetLegalEntitiesByRoleType(new OfferRoleTypes[] { OfferRoleTypes.MainApplicant }, GeneralStatusKey.Active)).Return(new ReadOnlyEventList<ILegalEntity>(mainApplicants));
			application.WhenToldTo(x => x.GetLegalEntitiesByRoleType(new OfferRoleTypes[] { OfferRoleTypes.BranchConsultantD }, GeneralStatusKey.Active)).Return(new ReadOnlyEventList<ILegalEntity>(consultants));
			lookupRepository.WhenToldTo(x => x.Controls.ObjectDictionary).Return(objectDictionary);
			commonRepository.WhenToldTo(x => x.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.NewClientConsultantDetailsSMS)).Return(correspondenceTemplate);

			handler = new SendNewClientConsultantDetailsSMSCommandHandler(messageService, applicationRepository, commonRepository, lookupRepository);
			command = new SendNewClientConsultantDetailsSMSCommand(applicationKey);
		}

    }
}
