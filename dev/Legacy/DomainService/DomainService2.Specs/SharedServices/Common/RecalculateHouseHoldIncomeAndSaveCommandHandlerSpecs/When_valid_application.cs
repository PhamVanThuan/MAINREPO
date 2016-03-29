using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.RecalculateHouseHoldIncomeAndSaveCommandHandlerSpecs
{
    public class When_valid_application : DomainServiceSpec<RecalculateHouseHoldIncomeAndSaveCommand, RecalculateHouseHoldIncomeAndSaveCommandHandler>
    {
        protected static IApplicationRepository applicationRepository;
        protected static IApplication application;

        Establish context = () =>
            {
                applicationRepository = An<IApplicationRepository>();
                application = An<IApplication>();

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);
                command = new RecalculateHouseHoldIncomeAndSaveCommand(Param.IsAny<int>());
                handler = new RecalculateHouseHoldIncomeAndSaveCommandHandler(applicationRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_calc_household_income = () =>
            {
                application.WasToldTo(x => x.CalculateHouseHoldIncome());
            };

        It should_save_application = () =>
            {
                applicationRepository.WhenToldTo(x => x.SaveApplication(application));
            };
    }
}