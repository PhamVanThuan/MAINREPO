using DomainService2.SharedServices.Common;
using DomainService2.Specs.DomainObjects;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.SharedServices.AddDetailTypeCommandHandlerSpecs
{
    [Subject(typeof(AddDetailTypeCommandHandler))]
    public class When_not_furtheradvance : DomainServiceSpec<AddDetailTypeCommand, AddDetailTypeCommandHandler>
    {
        protected static IApplicationRepository applicationRepository;
        protected static IAccountRepository accountRepository;
        protected static ILookupRepository lookupRepository;

        Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();
            accountRepository = An<IAccountRepository>();
            lookupRepository = An<ILookupRepository>();

            IApplication application = An<IApplication>();
            IApplicationType applicationType = An<IApplicationType>();

            IEventList<IDetailType> detailTypes = new StubEventList<IDetailType>();

            IDetail newDetail = An<IDetail>();
            IAccount account = An<IAccount>();

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);
            application.WhenToldTo(x => x.ApplicationType).Return(applicationType);
            applicationType.WhenToldTo(x => x.Key).Return((int)OfferTypes.FurtherLoan);

            application.WhenToldTo(x => x.Account).Return(account);

            command = new AddDetailTypeCommand(1, "test", "test");
            handler = new AddDetailTypeCommandHandler(applicationRepository, accountRepository, lookupRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_false = () =>
        {
            command.Result.ShouldBeFalse();
        };

        It should_not_save_detail = () =>
        {
            accountRepository.WasNotToldTo(x => x.SaveDetail(Param.IsAny<IDetail>()));
        };
    }
}