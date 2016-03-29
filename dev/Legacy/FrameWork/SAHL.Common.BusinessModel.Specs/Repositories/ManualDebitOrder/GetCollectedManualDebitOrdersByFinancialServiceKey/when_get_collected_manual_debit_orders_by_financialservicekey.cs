using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Repositories.ManualDebitOrder.GetCollectedManualDebitOrdersByFinancialServiceKey
{
    internal class when_get_collected_manual_debit_orders_by_financialservicekey : ManualDebitOrderRepositoryWithFakesBase
    {
        Establish context = () =>
        {
            manualDebitOrderRepository.WhenToldTo(x => x.GetCollectedManualDebitOrdersByFinancialServiceKey(Param.IsAny<int>())).Return(manualDebitOrders);
        };

        Because of = () =>
        {
            manualDebitOrderRepository.GetCollectedManualDebitOrdersByFinancialServiceKey(Param.IsAny<int>());
        };

        It should_get_collected_manual_debit_orders_by_financialservicekey = () =>
        {
            manualDebitOrderRepository.WasToldTo(x => x.GetCollectedManualDebitOrdersByFinancialServiceKey(Param.IsAny<int>()));
        };
    }
}