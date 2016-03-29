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
	public class when_consultant_does_not_have_a_cell_number : base_send_new_client_consultant_details_sms_spec
	{
		Establish context = () =>
		{
			consultantCellphoneNumber = String.Empty;
			clientCallCentreNumber = "012 000 9999";
			base_send_new_client_consultant_details_sms_spec.Setup();

			renderedTemplate = String.Format(template, consultantName, clientCallCentreNumber, command.ApplicationKey);
		};

		Because of = () =>
		{
			handler.Handle(messages, command);
		};

		It should_use_the_call_centre_number_in_the_sms = () =>
		{
			messageService.WasToldTo(x => x.SendSMS(command.ApplicationKey, renderedTemplate, mainApplicant.CellPhoneNumber));
		};
	}
}