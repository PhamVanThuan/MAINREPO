using DomainService2.Workflow.PersonalLoan;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.PersonalLoan.UpdateOfferInformationType
{
    public class When_handled : DomainServiceSpec<UpdateOfferInformationTypeCommand, UpdateOfferInformationTypeCommandHandler>
    {
        private static IApplicationRepository applicationRepository;
        //protected static string subject = "a test subject";
        Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();
            //    command = new UpdateOfferInformationTypeCommand(Param.IsAny<int>());
            //    handler = new UpdateOfferInformationTypeCommandHandler(x2Repository);

            //    x2Repository.WhenToldTo(x => x.GetPersonalLoansInstanceSubject(Param.IsAny<int>())).Return(subject);
        };

        //Because of = () =>
        //{
        //    handler.Handle(messages, command);
        //};

        //It should_return_the_subject = () =>
        //{
        //    command.Result.ShouldEqual(subject);
        //};
    }
}