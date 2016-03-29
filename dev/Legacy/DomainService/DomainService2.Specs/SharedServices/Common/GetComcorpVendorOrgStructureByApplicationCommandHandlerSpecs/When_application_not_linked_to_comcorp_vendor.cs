using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.GetComcorpVendorOrgStructureByApplicationCommandHandlerSpecs
{
    [Subject(typeof(GetComcorpVendorOrgStructureByApplicationCommandHandler))]
    public class When_application_not_linked_to_comcorp_vendor : DomainServiceSpec<GetComcorpVendorOrgStructureByApplicationCommand, GetComcorpVendorOrgStructureByApplicationCommandHandler>
    {
        private static int orgStructureKey;

        private Establish context = () =>
        {
            IVendor vendor = null;

            IApplication application = An<IApplication>();

            application.WhenToldTo(x => x.GetComcorpVendor())
                       .Return(vendor);

            IApplicationReadOnlyRepository appRepository = An<IApplicationReadOnlyRepository>();
            appRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything))
                .Return(application);

            command = new GetComcorpVendorOrgStructureByApplicationCommand(Param<int>.IsAnything);
            handler = new GetComcorpVendorOrgStructureByApplicationCommandHandler(appRepository);
        };

        private Because of = () =>
        {
            handler.Handle(messages, command);
        };

        private It should_return_minus_1 = () =>
        {
            command.Result.ShouldEqual(-1);
        };
    }
}
