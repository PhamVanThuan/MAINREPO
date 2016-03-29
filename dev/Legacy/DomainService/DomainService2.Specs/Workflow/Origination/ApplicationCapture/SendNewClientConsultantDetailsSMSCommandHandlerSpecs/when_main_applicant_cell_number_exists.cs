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
	[Subject(typeof(SendNewClientConsultantDetailsSMSCommandHandler))]
    public class when_main_applicant_cell_number_exists : base_send_new_client_consultant_details_sms_spec_for_passing_tests
	{
		Establish context = () =>
		{
			mainApplicantCellphoneNumber = "0734448888";
            base_send_new_client_consultant_details_sms_spec_for_passing_tests.Setup();
		};

		Because of = () =>
		{
			handler.Handle(messages, command);
		};

		It should_send_the_sms = () =>
		{
			messageService.WasToldTo(x => x.SendSMS(applicationKey, renderedTemplate, mainApplicant.CellPhoneNumber));
		};
	}
}