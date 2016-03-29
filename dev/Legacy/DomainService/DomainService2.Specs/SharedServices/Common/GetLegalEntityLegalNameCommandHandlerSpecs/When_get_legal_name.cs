using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.GetCaseNameCommandHandlerSpecs
{
    [Subject(typeof(GetLegalEntityLegalNameCommandHandler))]
    public class When_get_legal_name : DomainServiceSpec<GetLegalEntityLegalNameCommand, GetLegalEntityLegalNameCommandHandler>
    {
        Establish context = () =>
        {
            ILegalEntityRepository legalEntityRepository = An<ILegalEntityRepository>();
            ILegalEntity legalEntity = An<ILegalEntity>();

            legalEntityRepository.WhenToldTo(x => x.GetLegalEntityByKey(Param.IsAny<int>()))
                .Return(legalEntity);

            legalEntity.WhenToldTo(x => x.GetLegalName(LegalNameFormat.Full)).Return("Legal Name");

            command = new GetLegalEntityLegalNameCommand(1, LegalNameFormat.Full);
            handler = new GetLegalEntityLegalNameCommandHandler(legalEntityRepository);
        };
        Because of = () =>
        {
            handler.Handle(messages, command);
        };
        It should_return_legal_entity_legal_name = () =>
        {
            command.LegalNameResult.ShouldNotBeEmpty();
        };
    }
}