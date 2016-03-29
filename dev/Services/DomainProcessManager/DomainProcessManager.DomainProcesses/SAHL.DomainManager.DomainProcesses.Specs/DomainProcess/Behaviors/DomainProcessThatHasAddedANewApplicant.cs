using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Behaviors
{
    [Behaviors]
    public class DomainProcessThatHasAddedANewApplicant
    {
        protected static ApplicantModel applicant;
        protected static int applicationNumber, clientKey, applicationRoleKey;
        protected static IClientDomainServiceClient clientDomainService;
        protected static IApplicationDomainServiceClient applicationDomainService;
        protected static IApplicationStateMachine applicationStateMachine;

        private It should_create_a_new_client = () =>
        {
            clientDomainService.WasToldTo(x => x.PerformCommand(
                Param<AddNaturalPersonClientCommand>.Matches(m =>
                m.NaturalPersonClient.IDNumber == applicant.IDNumber &&
                m.NaturalPersonClient.FirstName == applicant.FirstName &&
                m.NaturalPersonClient.Surname == applicant.Surname &&
                m.NaturalPersonClient.EmailAddress == applicant.EmailAddress),
                Param<DomainProcessServiceRequestMetadata>.Matches(m => m.ContainsKey("IdNumberOfAddedClient"))));
        };

        private It should_link_the_applicant_to_the_application = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(
                Param<AddLeadApplicantToApplicationCommand>.Matches(m =>
                m.ApplicationNumber == applicationNumber &&
                m.ClientKey == clientKey),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_make_the_applicant_an_income_contributor = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(
                Param<MakeApplicantAnIncomeContributorCommand>.Matches(m => m.ApplicationRoleKey == applicationRoleKey), Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_add_the_marketing_options_to_the_client = () =>
        {
            clientDomainService.WasToldTo(x => x.PerformCommand(Param<AddMarketingOptionsToClientCommand>.Matches(m =>
                m.ClientKey == clientKey), Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_add_the_client_to_the_state_machines_client_collection = () =>
        {
            applicationStateMachine.ClientCollection[applicant.IDNumber].ShouldEqual(clientKey);
        };

        private It should_add_the_client_employers = () =>
        {
            var employer = applicant.Employments.First().Employer;
            clientDomainService.WasToldTo(x => x.PerformCommand(
                Param<AddEmployerCommand>.Matches(m =>
                m.Employer.EmployerName == employer.EmployerName &&
                m.Employer.EmployerBusinessType == employer.EmployerBusinessType &&
                m.Employer.ContactEmail == employer.ContactEmail),
                Param<DomainProcessServiceRequestMetadata>.Matches(m => m["EmployeeIdNumber"] == applicant.IDNumber)));
        };

        private It should_add_client_employment = () =>
        {
            var employment = applicant.Employments.First();
            clientDomainService.WasToldTo(x => x.PerformCommand(
                Param<AddUnconfirmedSalariedEmploymentToClientCommand>.Matches(m =>
                m.ClientKey == clientKey &&
                m.SalariedEmploymentModel.BasicIncome == employment.BasicIncome &&
                m.SalariedEmploymentModel.EmploymentType == employment.EmploymentType),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };
    }
}