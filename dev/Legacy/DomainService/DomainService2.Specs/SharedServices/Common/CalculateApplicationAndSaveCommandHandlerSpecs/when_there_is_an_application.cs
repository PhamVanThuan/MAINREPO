using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.CalculateApplicationAndSaveCommandHandlerSpecs
{
	[Subject(typeof(CalculateAndSaveApplicationCommandHandler))]
	public class when_there_is_an_application : DomainServiceSpec<CalculateAndSaveApplicationCommand, CalculateAndSaveApplicationCommandHandler>
	{
		protected static IApplicationRepository applicationRepository;
		protected static IApplication application;
		protected static int applicationKey;
		protected static bool isBondExceptionAction;
		Establish context = () =>
		{
			applicationKey = 0;
			isBondExceptionAction = false;

			application = An<IApplication>();
			applicationRepository = An<IApplicationRepository>();

			command = new CalculateAndSaveApplicationCommand(applicationKey, isBondExceptionAction);
			handler = new CalculateAndSaveApplicationCommandHandler(applicationRepository);

			applicationRepository.WhenToldTo(x => x.GetApplicationByKey(command.ApplicationKey)).Return(application);
		};

		Because of = () =>
		{
			handler.Handle(messages, command);
		};

		It application_should_be_recalculated = () =>
		{
			application.WasToldTo(x => x.CalculateApplicationDetail(command.IsBondExceptionAction, false));
		};

		It application_should_be_saved = () =>
		{
			applicationRepository.WasToldTo(x => x.SaveApplication(application));
		};
	}
}
