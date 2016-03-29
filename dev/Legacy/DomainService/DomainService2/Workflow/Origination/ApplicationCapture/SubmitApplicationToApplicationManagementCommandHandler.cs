using System.Text;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class SubmitApplicationToApplicationManagementCommandHandler : IHandlesDomainServiceCommand<SubmitApplicationToApplicationManagementCommand>
    {
        private IOrganisationStructureRepository organisationStructureRepository;
        private IX2Repository x2Repository;

        public SubmitApplicationToApplicationManagementCommandHandler(IOrganisationStructureRepository organisationStructureRepository, IX2Repository x2Repository)
        {
            this.organisationStructureRepository = organisationStructureRepository;
            this.x2Repository = x2Repository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, SubmitApplicationToApplicationManagementCommand command)
        {
            var applicationRole = organisationStructureRepository.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(command.ApplicationKey, (int)OfferRoleTypes.BranchConsultantD);

            var adUser = organisationStructureRepository.GetAdUserByLegalEntityKey(applicationRole.LegalEntity.Key);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<FieldInputs><FieldInput Name = \"ApplicationKey\">{0}</FieldInput><FieldInput Name = \"CaseOwnerName\">{1}</FieldInput><FieldInput Name = \"AppCapIID\">{2}</FieldInput></FieldInputs>", command.ApplicationKey, adUser.ADUserName, command.InstanceID);

            x2Repository.CreateAndSaveActiveExternalActivity(
                Constants.WorkFlowExternalActivity.CreateInstanceFromAppCapture,
                command.InstanceID,
                Constants.WorkFlowName.ApplicationManagement,
                Constants.WorkFlowProcessName.Origination,
                sb.ToString());
        }
    }
}