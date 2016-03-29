using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using SAHL.Services.Interfaces.PropertyDomain;
using SAHL.Services.Interfaces.PropertyDomain.Queries;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules
{
    public class OpenMortgageLoanForApplicantAndPropertyCannotExistRule : IDomainRule<ApplicationCreationModel>
    {
        private IPropertyDomainServiceClient propertyDomainService;
        private IApplicationDomainServiceClient applicationDomainService;

        public OpenMortgageLoanForApplicantAndPropertyCannotExistRule(IPropertyDomainServiceClient propertyDomainService, IApplicationDomainServiceClient applicationDomainService)
        {
            this.propertyDomainService = propertyDomainService;
            this.applicationDomainService = applicationDomainService;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, ApplicationCreationModel ruleModel)
        {
            var propertyAddressModel = ruleModel.PropertyAddress;
            var getPropertyByAddressQuery = new GetPropertyByAddressQuery(propertyAddressModel.UnitNumber, propertyAddressModel.BuildingName, propertyAddressModel.BuildingNumber,
                propertyAddressModel.StreetNumber, propertyAddressModel.StreetName, propertyAddressModel.Suburb, propertyAddressModel.City,
                propertyAddressModel.Province, propertyAddressModel.PostalCode, propertyAddressModel.ErfNumber, propertyAddressModel.ErfPortionNumber);
            propertyDomainService.PerformQuery(getPropertyByAddressQuery);
            if (getPropertyByAddressQuery.Result != null)
            {
                var properties = getPropertyByAddressQuery.Result.Results;
                if (properties.Any())
                {
                    foreach (var property in properties)
                    {
                        foreach (var applicant in ruleModel.Applicants)
                        {
                            DoesOpenApplicationExistForPropertyAndClientQuery query = new DoesOpenApplicationExistForPropertyAndClientQuery(property.PropertyKey, applicant.IDNumber);
                            applicationDomainService.PerformQuery(query);
                            if (query.Result != null && query.Result.Results.First())
                            {
                                messages.AddMessage(new SystemMessage("An application for this property already exists against a client on this application.", SystemMessageSeverityEnum.Error));
                            }
                        }
                    }
                }
            }
        }
    }
}