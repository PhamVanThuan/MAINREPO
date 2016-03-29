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
	public class when_no_cell_number_exists_for_main_applicant : base_send_new_client_consultant_details_sms_spec
	{
		Establish context = () =>
		{
			mainApplicantCellphoneNumber = String.Empty;
			base_send_new_client_consultant_details_sms_spec.Setup();
		};

		Because of = () =>
		{
			handler.Handle(messages, command);
		};

		It should_not_send_the_sms = () =>
		{
			messageService.WasNotToldTo(x => x.SendSMS(Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>()));
		};
	}
}