using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.Revert30YearTermApplicationToPreviousTermCommandHandlerSpecs
{
    [Subject(typeof(Revert30YearTermApplicationToPreviousTermCommandHandler))]
    public class When_an_application_exists : WithFakes
    {
        protected static Revert30YearTermApplicationToPreviousTermCommand command;
        protected static Revert30YearTermApplicationToPreviousTermCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IApplicationRepository applicationRepository;
        protected static IApplication application;

        Establish context = () =>
        {
            application = An<IApplication>();
            messages = An<IDomainMessageCollection>();
            applicationRepository = An<IApplicationRepository>();
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);
            command = new Revert30YearTermApplicationToPreviousTermCommand(1);
            handler = new Revert30YearTermApplicationToPreviousTermCommandHandler(applicationRepository);
       };

        Because of = () =>
        {
            handler.Handle(messages,command);
        };  

        It should_revert_the_application_to_previous_term = () => 
        {
            applicationRepository.WhenToldTo(x => x.RevertToPreviousTermAsRevisedApplication(application));
        };
    }
}
