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
using SAHL.Common.DataAccess;

namespace DomainService2.Specs.Workflow.DebtCounselling.NTUOpenPersonalLoanCommandHandlerSpecs
{
	[Subject(typeof(NTUOpenPersonalLoanCommandHandler))]
	public class when_debtcounselling_clients_have_no_personalloan : WithFakes
	{
		private static IDebtCounsellingRepository debtCounsellingRepository;
		private static IX2Repository x2Repository;
		private static ITransactionManager transactionManager;
		private static IDomainMessageCollection messages;
		private static NTUOpenPersonalLoanCommand command;
		private static NTUOpenPersonalLoanCommandHandler commandHandler;
		Establish context = () =>
		{
			messages = new DomainMessageCollection();
			debtCounsellingRepository = An<IDebtCounsellingRepository>();
			x2Repository = An<IX2Repository>();
			transactionManager = An<ITransactionManager>();

			command = new NTUOpenPersonalLoanCommand(1);
			commandHandler = new NTUOpenPersonalLoanCommandHandler(debtCounsellingRepository, x2Repository, transactionManager);
			
			var debtCounselling = An<IDebtCounselling>();
			var client = An<ILegalEntity>();
			var clients = new List<ILegalEntity>(new []{client});

			debtCounselling.WhenToldTo(x=>x.Clients).Return(clients);
			debtCounsellingRepository.WhenToldTo(x=>x.GetDebtCounsellingByKey(Param.IsAny<int>())).Return(debtCounselling);
		};

		Because of = () =>
		{
			commandHandler.Handle(messages, command);
		};

		It should_not_call_ntu_external_activity = () =>
		{
			x2Repository.WasNotToldTo(x => x.CreateAndSaveActiveExternalActivity(Param.IsAny<string>(), Param.IsAny<long>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()));
		};
	}
}
