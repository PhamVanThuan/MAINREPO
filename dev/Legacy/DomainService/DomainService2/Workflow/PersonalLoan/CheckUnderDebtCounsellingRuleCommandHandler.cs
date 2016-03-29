using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CheckUnderDebtCounsellingRuleCommandHandler : RuleDomainServiceCommandHandler<CheckUnderDebtCounsellingRuleCommand>
    {
        IApplicationRepository ApplicationRepository;

        public CheckUnderDebtCounsellingRuleCommandHandler(ICommandHandler commandHandler, IApplicationRepository applicationRepository)
            : base(commandHandler)
        {
            this.ApplicationRepository = applicationRepository;
        }

        public override void SetupRule()
        {
            IApplicationUnsecuredLending applicationUnsecuredLending = (IApplicationUnsecuredLending)ApplicationRepository.GetApplicationByKey(command.ApplicationKey);

            ILegalEntity legalEntity = applicationUnsecuredLending.ActiveClientRoles.First<IExternalRole>().LegalEntity;

            command.RuleParameters = new object[] { legalEntity };
        }
    }
}