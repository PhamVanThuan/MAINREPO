using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.Common
{
	public class GetPreviousStateNameCommandHandler : IHandlesDomainServiceCommand<GetPreviousStateNameCommand>
	{
		private IX2Repository x2Repository;
		public GetPreviousStateNameCommandHandler(IX2Repository x2Repository)
		{
			this.x2Repository = x2Repository;
		}
		public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, GetPreviousStateNameCommand command)
		{
			command.Result = x2Repository.GetPreviousStateName(command.InstanceID);
		}
	}
}
