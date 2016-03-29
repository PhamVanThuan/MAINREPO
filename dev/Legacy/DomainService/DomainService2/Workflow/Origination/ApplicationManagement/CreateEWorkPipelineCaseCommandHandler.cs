using System;
using System.Collections.Generic;
using EWorkConnector;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class CreateEWorkPipelineCaseCommandHandler : IHandlesDomainServiceCommand<CreateEWorkPipelineCaseCommand>
    {
        private IeWork eWorkEngine;
        private IApplicationRepository applicationRepository;
        private IOrganisationStructureRepository orgStructureRepository;

        public CreateEWorkPipelineCaseCommandHandler(IeWork eWorkEngine, IApplicationRepository applicationRepository, IOrganisationStructureRepository orgStructureRepository)
        {
            this.eWorkEngine = eWorkEngine;
            this.applicationRepository = applicationRepository;
            this.orgStructureRepository = orgStructureRepository;
        }

        public void Handle(IDomainMessageCollection messages, CreateEWorkPipelineCaseCommand command)
        {
            command.Result = false;
            messages.Clear();

            string SessionID = eWorkEngine.LogIn("LighthouseUser");
            // get attorney and creator
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);

            IReadOnlyEventList<IApplicationRole> branchConsultants = application.GetApplicationRolesByType(OfferRoleTypes.BranchConsultantD);

            if (branchConsultants == null || branchConsultants.Count == 0)
            {
                branchConsultants = application.GetApplicationRolesByType(OfferRoleTypes.FLProcessorD);
            }

            if (branchConsultants == null || branchConsultants.Count == 0)
            {
                throw new Exception(string.Format("Unable to get Branch Consultant D(101) or FL Processor D(857) offer role for case:{0}", command.ApplicationKey));
            }

            IReadOnlyEventList<IApplicationRole> attorneys = application.GetApplicationRolesByType(OfferRoleTypes.ConveyanceAttorney);

            IAttorney attorney = null;

            foreach (IApplicationRole tmpRole in attorneys)
            {
                if (tmpRole.GeneralStatus.Key == 1)
                {
                    attorney = orgStructureRepository.GetAttorneyByLegalEntityKey(tmpRole.LegalEntity.Key);
                    continue;
                }
            }

            IADUser consultant = orgStructureRepository.GetAdUserByLegalEntityKey(branchConsultants[0].LegalEntity.Key);

            // *** NB the order of these vars MUST not change. It must match the ework form ***
            Dictionary<string, string> variables = new Dictionary<string, string>();
            string ChangedField = "AttorneyNumber";
            variables.Add("AttorneyNumber", attorney.Key.ToString());
            variables.Add("ProspectNumber", application.Key.ToString());
            variables.Add("UserToDo", consultant.Key.ToString());

            command.EFolderID = eWorkEngine.CreateFolder(SessionID, "Pipeline", variables, ChangedField);

            command.Result = true;
        }
    }
}