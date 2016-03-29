using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.Workflow.Origination.Credit
{
    public class CheckApplicationIsNewBusinessRuleCommandHandler : RuleDomainServiceCommandHandler<CheckApplicationIsNewBusinessRuleCommand>
    {
        private IApplicationRepository applicationRepository;

        public CheckApplicationIsNewBusinessRuleCommandHandler(IApplicationRepository applicationRepository, ICommandHandler commandHandler)
            : base(commandHandler)
        {
            this.applicationRepository = applicationRepository;
        }

        public override void SetupRule()
        {
            var application = this.applicationRepository.GetApplicationByKey(command.ApplicationKey);
            command.RuleParameters = new Object[] { application };
            base.SetupRule();
        }
    }
}
