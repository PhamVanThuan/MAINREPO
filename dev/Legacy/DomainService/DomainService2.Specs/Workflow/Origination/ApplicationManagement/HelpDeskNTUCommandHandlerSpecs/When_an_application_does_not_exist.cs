using DomainService2.Specs.DomainObjects;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.HelpDeskNTUCommandHandlerSpecs
{
    [Subject(typeof(HelpDeskNTUCommandHandler))]
    internal class When_an_application_does_not_exist : WithFakes
    {
        protected static IApplicationRepository applicationRepository;
        protected static IReasonRepository reasonRepository;
        protected static IDomainMessageCollection messages;
        protected static HelpDeskNTUCommandHandler handler;
        protected static HelpDeskNTUCommand command;
        protected static int applicationKey;
        protected static Exception exception;
        Establish context = () =>
        {
            applicationKey = -1;
            reasonRepository = An<IReasonRepository>();
            applicationRepository = An<IApplicationRepository>();
           
            handler = new HelpDeskNTUCommandHandler(reasonRepository, applicationRepository);
            command = new HelpDeskNTUCommand(applicationKey);
        };
        Because of = () =>
        {
            exception = Catch.Exception(()=>handler.Handle(messages, command));
        };
        It should_return_the_exception = () =>
        {
            exception.ShouldBeOfType(typeof(NullReferenceException));
            exception.ShouldNotBeNull();
        };
        It should_not_save_instance = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}
