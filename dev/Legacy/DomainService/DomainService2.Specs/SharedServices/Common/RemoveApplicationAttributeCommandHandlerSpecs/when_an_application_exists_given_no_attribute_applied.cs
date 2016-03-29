using DomainService2.SharedServices.Common;
using DomainService2.Specs.DomainObjects;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.Specs.SharedServices.Common.RemoveApplicationAttributeCommandHandlerSpecs
{
    [Subject(typeof(RemoveApplicationAttributeCommandHandler))]
    public class when_an_application_exists_given_no_attribute_applied : DomainServiceSpec<RemoveApplicationAttributeCommand, RemoveApplicationAttributeCommandHandler>
    {
        protected static IApplicationRepository applicationRepository;
        protected static IApplication application;
        protected static int appKey = 1;
        protected static int attributeKey = 1;
        protected static int attributeKeyToLookFor = 2;
        // Arrange
        Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();
            application = An<IApplication>();
            IApplicationAttribute applicationAttribute = An<IApplicationAttribute>();
            IApplicationAttributeType applicationAttributeType = An<IApplicationAttributeType>();
            applicationAttributeType.WhenToldTo(x => x.Key).Return(attributeKey); ;
            applicationAttribute.WhenToldTo(x => x.ApplicationAttributeType).Return(applicationAttributeType);
            var attributes = new StubEventList<IApplicationAttribute>();
            attributes.Add(applicationAttribute);
            application.WhenToldTo(x => x.ApplicationAttributes).Return(attributes);
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(appKey)).Return(application);
            command = new RemoveApplicationAttributeCommand(appKey, attributeKeyToLookFor);
            handler = new RemoveApplicationAttributeCommandHandler(applicationRepository);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_get_the_application = () =>
        {
            applicationRepository.WasToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything));
        };

        // Assert
        It should_save_application = () =>
        {
            applicationRepository.WasNotToldTo(x => x.SaveApplication(application));
        };
    }
}
