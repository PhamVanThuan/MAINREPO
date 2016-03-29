using System;
using DomainService2.Specs.DomainObjects;
using DomainService2.Workflow.Origination.Credit;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Credit.IsReviewRequiredCommandHandlerSpecs
{
    [Subject(typeof(IsReviewRequiredCommandHandler))]
    public class When_underwriter_performed_activity_last : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static IsReviewRequiredCommand command;
        protected static IsReviewRequiredCommandHandler handler;
        protected static IX2Repository X2Repository;
        protected static IOrganisationStructureRepository organisationStructureRepository;
        protected static Int64 instanceID = 1;
        protected static String activityName = "Test Activity";
        protected static String userName = "TestUser";
        protected static IApplicationRoleType applicationRoleType;
        protected static IEventList<IOrganisationStructure> offerRoleTypeOrganisationStructures;
        protected static IEventList<IADUser> adusers;

        // Arrange
        Establish context = () =>
        {
            X2Repository = An<IX2Repository>();
            organisationStructureRepository = An<IOrganisationStructureRepository>();
            applicationRoleType = An<IApplicationRoleType>();
            offerRoleTypeOrganisationStructures = new StubEventList<IOrganisationStructure>();
            //need to add items to the list
            IOrganisationStructure organisationStructure = An<IOrganisationStructure>();
            offerRoleTypeOrganisationStructures.Add(messages, organisationStructure);

            adusers = new StubEventList<IADUser>();
            //need to add items to the list
            IADUser aduser = An<IADUser>();
            adusers.Add(messages, aduser);

            command = new IsReviewRequiredCommand(instanceID, activityName);
            handler = new IsReviewRequiredCommandHandler(X2Repository, organisationStructureRepository);

            X2Repository.WhenToldTo(x => x.GetUserWhoPerformedActivity(Param.IsAny<Int64>(), Param.IsAny<String>())).Return(userName);
            organisationStructureRepository.WhenToldTo(x => x.GetApplicationRoleTypeByKey(Param.IsAny<Int32>())).Return(applicationRoleType);

            applicationRoleType.WhenToldTo(x => x.OfferRoleTypeOrganisationStructures).Return(offerRoleTypeOrganisationStructures);
            organisationStructure.WhenToldTo(x => x.ADUsers).Return(adusers);
            aduser.WhenToldTo(x => x.ADUserName).Return(userName);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_execute_GetUserWhoPerformedActivity = () =>
        {
            X2Repository.WasToldTo(x => x.GetUserWhoPerformedActivity(instanceID, activityName));
        };

        // Assert
        It should_execute_GetApplicationRoleTypeByKey = () =>
        {
            organisationStructureRepository.WasToldTo(x => x.GetApplicationRoleTypeByKey(Param.IsAny<Int32>()));
        };

        //Assert
        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };

        //Assert
        It should_set_command_ActivityName = () =>
        {
            command.ActivityName.Equals(activityName);
        };

        //Assert
        It should_set_command_InstanceID = () =>
        {
            command.InstanceID.Equals(instanceID);
        };
    }
}