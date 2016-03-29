using DomainService2.SharedServices.Common;
using DomainService2.Specs.DomainObjects;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Specs.SharedServices.Common.SendSMSToMainApplicantsCommandHandlerSpecs
{
    [Subject(typeof(SendSMSToMainApplicantsCommandHandler))]
    public class When_cellnumber_notnull : DomainServiceSpec<SendSMSToMainApplicantsCommand, SendSMSToMainApplicantsCommandHandler>
    {
        protected static IMessageService messageService;
        protected static IApplicationRepository applicationRepository;

        Establish context = () =>
            {
                messageService = An<IMessageService>();
                applicationRepository = An<IApplicationRepository>();

                IApplication application = An<IApplication>();
                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

                ILegalEntity legalEntity = An<ILegalEntity>();
                legalEntity.WhenToldTo(x => x.CellPhoneNumber).Return("1111");
                IReadOnlyEventList<ILegalEntity> legalEntities = new StubReadOnlyEventList<ILegalEntity>(new ILegalEntity[] { legalEntity });

                application.WhenToldTo(x => x.GetLegalEntitiesByRoleType(new OfferRoleTypes[] { OfferRoleTypes.MainApplicant })).Return(legalEntities);

                command = new SendSMSToMainApplicantsCommand(Param.IsAny<string>(), Param.IsAny<int>());
                handler = new SendSMSToMainApplicantsCommandHandler(messageService, applicationRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_send_sms = () =>
            {
                messageService.WasToldTo(x => x.SendSMS(Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>()));
            };
    }
}