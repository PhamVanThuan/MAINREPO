using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.Models;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Linq;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Utilities
{
    public class when_mapping_a_new_purchase_application_model : WithFakes
    {
        
        private static DomainModelMapper domainMapper;
        private static NewPurchaseApplicationCreationModel newPurchaseApplicationCreationModel;
        private static NewPurchaseApplicationModel resultingModel;     

        private Establish context = () =>
        {
            domainMapper = new DomainModelMapper();
            domainMapper.CreateMap<NewPurchaseApplicationCreationModel, NewPurchaseApplicationModel>();
            newPurchaseApplicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
        };

        private Because of = () =>
        {
            resultingModel = domainMapper.Map(newPurchaseApplicationCreationModel);
        };

        private It should_return_an_object_with_matching_properties = () =>
        {
            resultingModel.ShouldMatch(m =>
                m.ApplicationStatus == newPurchaseApplicationCreationModel.ApplicationStatus &&
                m.ApplicantCount == newPurchaseApplicationCreationModel.ApplicantCount &&
                m.ApplicationSourceKey == newPurchaseApplicationCreationModel.ApplicationSourceKey &&
                m.ApplicationType == newPurchaseApplicationCreationModel.ApplicationType &&
                m.Deposit == newPurchaseApplicationCreationModel.Deposit &&
                m.OriginationSource == newPurchaseApplicationCreationModel.OriginationSource &&
                m.Product == newPurchaseApplicationCreationModel.Product &&
                m.PurchasePrice == newPurchaseApplicationCreationModel.PurchasePrice &&
                m.Term == newPurchaseApplicationCreationModel.Term
            );
        };

        
    }
}