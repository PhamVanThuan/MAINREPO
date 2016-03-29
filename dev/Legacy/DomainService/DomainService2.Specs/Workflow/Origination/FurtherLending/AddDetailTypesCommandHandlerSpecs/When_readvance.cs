using System;
using DomainService2.Specs.DomainObjects;
using DomainService2.Workflow.Origination.FurtherLending;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.Origination.FurtherLending.AddDetailTypesCommandHandlerSpecs
{
    [Subject(typeof(AddDetailTypesCommandHandler))]
    public class When_readvance : AddDetailTypesCommandHandlerBase
    {
        Establish Context = () =>
        {
            IApplicationMortgageLoan app = An<IApplicationMortgageLoan>();
            IApplicationType appType = An<IApplicationType>();
            IDetail detail = An<IDetail>();
            IAccount account = An<IAccount>();
            IEventList<IDetailType> detailtypes = new StubEventList<IDetailType>();

            IApplicationRepository applicationRepository = An<IApplicationRepository>();
            accountRepository = An<IAccountRepository>();
            ILookupRepository lookupRepository = An<ILookupRepository>();

            messages = new DomainMessageCollection();

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(app);

            app.WhenToldTo(x => x.ApplicationType).Return(appType);
            app.WhenToldTo(x => x.Account).Return(account);
            app.WhenToldTo(x => x.LoanAgreementAmount).Return(1111);

            appType.WhenToldTo(x => x.Key).Return((int)OfferTypes.ReAdvance);

            accountRepository.WhenToldTo(x => x.CreateEmptyDetail()).Return(detail);

            lookupRepository.WhenToldTo(x => x.DetailTypes).Return(detailtypes);
            detailtypes.ObjectDictionary.Add(Convert.ToString((int)DetailTypes.ReadvanceInProgress), null);

            command = new AddDetailTypesCommand(Param.IsAny<int>(), Param.IsAny<string>());
            handler = new AddDetailTypesCommandHandler(applicationRepository, lookupRepository, accountRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };
        It should_save_detail = () =>
        {
            accountRepository.WasToldTo(x => x.SaveDetail(Param.IsAny<IDetail>()));
        };
    }
}