using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.Models;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System.Collections.Generic;
using System.Linq;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Utilities
{
    public class when_mapping_a_natural_person_client : WithFakes
    {
        private static DomainModelMapper domainMapper;
        private static ApplicantModel applicantModel;
        private static NaturalPersonClientModel naturalPersonClient;

        private Establish context = () =>
        {
            domainMapper = new DomainModelMapper();
            domainMapper.CreateMap<ApplicantModel, NaturalPersonClientModel>();
            applicantModel = ApplicationCreationTestHelper.PopulateApplicantModel(new List<AddressModel>());         
        };

        private Because of = () =>
        {
            naturalPersonClient = domainMapper.Map(applicantModel);
        };

        private It should_return_an_object_with_matching_properties = () =>
        {
            naturalPersonClient.ShouldMatch(m =>
                m.CitizenshipType == applicantModel.CitizenshipType &&
                m.DateOfBirth == applicantModel.DateOfBirth &&
                m.Education == applicantModel.Education &&
                m.FirstName == applicantModel.FirstName &&
                m.Surname == applicantModel.Surname &&
                m.IDNumber == applicantModel.IDNumber
            );
        };
    }
}