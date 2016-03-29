using SAHL.Core.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
        where T : ApplicationCreationModel
    {
        protected void AddClientEmployment(ApplicantModel applicant)
        {
            foreach (var employment in applicant.Employments)
            {
                if (clientDataManager.GetEmployerKey(employment.Employer.EmployerName) == null)
                {
                    var mapper = new DomainModelMapper();
                    mapper.CreateMap<EmployerModel, Services.Interfaces.ClientDomain.Models.EmployerModel>();
                    var employer = mapper.Map(employment.Employer);

                    var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());

                    serviceRequestMetadata.Add("EmployeeIdNumber", applicant.IDNumber);
                    var addEmployerCommand = new AddEmployerCommand(serviceRequestMetadata.CommandCorrelationId, employer);

                    var serviceMessages = this.clientDomainService.PerformCommand(addEmployerCommand, serviceRequestMetadata);
                    applicationStateMachine.SystemMessages.Aggregate(serviceMessages);
                    CheckForCriticalErrors(applicationStateMachine, serviceRequestMetadata.CommandCorrelationId, serviceMessages);
                }
                else
                {
                    var stateMachine = this.ProcessState as IApplicationStateMachine;
                    var clientKey = stateMachine.ClientCollection.First(x => x.Key == applicant.IDNumber).Value;
                    AddEmployment(employment, clientKey);
                }
            }
        }

        protected void AddEmployment(EmploymentModel employment, int clientKey)
        {
            var mapper = new DomainModelMapper();
            dynamic addEmploymentCommand = null;
            int employerKey = clientDataManager.GetEmployerKey(employment.Employer.EmployerName) ?? 0;
            employment.Employer.EmployerKey = employerKey;
            if (employment.EmploymentType == Core.BusinessModel.Enums.EmploymentType.Salaried)
            {
                mapper.CreateMap<SalariedEmploymentModel, Services.Interfaces.ClientDomain.Models.SalariedEmploymentModel>();
                var salariedEmployment = mapper.Map(employment);
                addEmploymentCommand = new AddUnconfirmedSalariedEmploymentToClientCommand(salariedEmployment, clientKey, Core.BusinessModel.Enums.OriginationSource.Comcorp);
            }
            else if (employment.EmploymentType == Core.BusinessModel.Enums.EmploymentType.SalariedwithDeduction)
            {
                mapper.CreateMap<SalaryDeductionEmploymentModel, Services.Interfaces.ClientDomain.Models.SalaryDeductionEmploymentModel>();
                var salariedWithDeductionEmployment = mapper.Map(employment);
                addEmploymentCommand = new AddUnconfirmedSalaryDeductionEmploymentToClientCommand(salariedWithDeductionEmployment, clientKey, Core.BusinessModel.Enums.OriginationSource.Comcorp);
            }
            else if (employment.EmploymentType == Core.BusinessModel.Enums.EmploymentType.Unemployed)
            {
                mapper.CreateMap<UnemployedEmploymentModel, Services.Interfaces.ClientDomain.Models.UnemployedEmploymentModel>();
                var unemployedEmployment = mapper.Map(employment);
                addEmploymentCommand = new AddUnconfirmedUnemployedEmploymentToClientCommand(unemployedEmployment, clientKey, Core.BusinessModel.Enums.OriginationSource.Comcorp);
            }
            var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());

            var serviceMessages = this.clientDomainService.PerformCommand(addEmploymentCommand, serviceRequestMetadata);
            CheckForCriticalErrors(applicationStateMachine, serviceRequestMetadata.CommandCorrelationId, serviceMessages);
        }
    }
}