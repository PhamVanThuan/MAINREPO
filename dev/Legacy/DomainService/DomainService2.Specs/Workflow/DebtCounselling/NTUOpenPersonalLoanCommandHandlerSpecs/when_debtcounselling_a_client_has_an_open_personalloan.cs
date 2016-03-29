using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using DomainService2.Workflow.DebtCounselling;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.DataAccess;

namespace DomainService2.Specs.Workflow.DebtCounselling.NTUOpenPersonalLoanCommandHandlerSpecs
{
	[Subject(typeof(NTUOpenPersonalLoanCommandHandler))]
	public class when_debtcounselling_a_client_has_an_open_personalloan : WithFakes
	{
		private static IDebtCounsellingRepository debtCounsellingRepository;
		private static IX2Repository x2Repository;
		private static ITransactionManager transactionManager;
		private static IDomainMessageCollection messages;
		private static NTUOpenPersonalLoanCommand command;
		private static NTUOpenPersonalLoanCommandHandler commandHandler;
		private static IInstance instance;
		private static int instanceID = 1;
		Establish context = () =>
		{
			messages = new DomainMessageCollection();
			debtCounsellingRepository = An<IDebtCounsellingRepository>();
			x2Repository = An<IX2Repository>();
			transactionManager = An<ITransactionManager>();

			command = new NTUOpenPersonalLoanCommand(1);
			commandHandler = new NTUOpenPersonalLoanCommandHandler(debtCounsellingRepository, x2Repository, transactionManager);

			var debtCounselling = An<IDebtCounselling>();
			var personalLoanApplication = An<IApplicationUnsecuredLending>();
			var client = An<ILegalEntity>();
			var clients = new List<ILegalEntity>(new[] { client });
			instance = An<IInstance>();

			personalLoanApplication.WhenToldTo(x => x.IsOpen).Return(true);
			client.WhenToldTo(x => x.PersonalLoanApplication).Return(personalLoanApplication);
			debtCounselling.WhenToldTo(x => x.Clients).Return(clients);
			debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param.IsAny<int>())).Return(debtCounselling);

			instance.WhenToldTo(x => x.ID).Return(instanceID);

			x2Repository.WhenToldTo(x=>x.GetInstanceForGenericKey(Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>())).Return(instance);
		};

		Because of = () =>
		{
			commandHandler.Handle(messages, command);
		};

		It should_call_external_ntu = () =>
		{
			x2Repository.WasToldTo(x => x.CreateAndSaveActiveExternalActivity(SAHL.Common.Constants.WorkFlowExternalActivity.PersonalLoanExternalNTU, instance.ID, SAHL.Common.Constants.WorkFlowName.PersonalLoans, SAHL.Common.Constants.WorkFlowProcessName.PersonalLoan, null));
		};
	}
}
