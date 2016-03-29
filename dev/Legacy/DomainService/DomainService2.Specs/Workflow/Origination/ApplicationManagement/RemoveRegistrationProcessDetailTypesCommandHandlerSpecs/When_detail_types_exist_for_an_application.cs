using System.Collections.Generic;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.RemoveRegistrationProcessDetailTypesCommandHandlerSpecs
{
    [Subject(typeof(RemoveRegistrationProcessDetailTypesCommandHandler))]
    public class When_detail_types_exist_for_an_application : WithFakes
    {
        protected static RemoveRegistrationProcessDetailTypesCommand command;
        protected static RemoveRegistrationProcessDetailTypesCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IEventList<IDetail> details;
        protected static IApplicationRepository applicationRepository;

        // Arrange
        Establish context = () =>
            {
                messages = An<IDomainMessageCollection>();
                applicationRepository = An<IApplicationRepository>();

                IApplication application = An<IApplication>();
                IAccount account = An<IAccount>();
                IDetail detail = An<IDetail>();
                IDetailType detailType = An<IDetailType>();

                detailType.WhenToldTo(x => x.Key).Return((int)DetailTypes.Scheduled);
                detail.WhenToldTo(x => x.DetailType).Return(detailType);

                List<IDetail> detailList = new List<IDetail>();
                detailList.Add(detail);
                details = new EventList<IDetail>(detailList);

                account.WhenToldTo(x => x.Details).Return(details);
                application.WhenToldTo(x => x.Account).Return(account);

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);
                applicationRepository.WhenToldTo(x => x.SaveApplication(Param<IApplication>.IsAnything));

                command = new RemoveRegistrationProcessDetailTypesCommand(Param<int>.IsAnything);
                handler = new RemoveRegistrationProcessDetailTypesCommandHandler(applicationRepository);
            };

        // Act
        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        // Assert
        It should_remove_detail_types_from_application = () =>
            {
                details.Count.ShouldEqual<int>(0);
            };

        // Assert
        It should_save_the_application_after_detail_types_removed = () =>
        {
            applicationRepository.WasToldTo(x => x.SaveApplication(Param<IApplication>.IsAnything));
        };
    }
}