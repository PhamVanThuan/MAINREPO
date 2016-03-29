using System;
using DomainService2.Workflow.Origination.Valuations;
using Machine.Specifications;

namespace DomainService2.Specs.Workflow.Origination.Valuations.GetValuationDataHandlerSpecs
{
    public class When_constructed_with_a_null_applicationrepository : DomainServiceSpec<GetValuationDataCommand, GetValuationDataCommandHandler>
    {
        static Exception exception;
        Establish context = () =>
            {
            };

        Because of = () =>
            {
                exception = Catch.Exception(() => handler = new GetValuationDataCommandHandler(null));
            };

        It should_throw_null_argument_exception = () =>
            {
                exception.ShouldBeOfType(typeof(ArgumentNullException));
            };
    }
}