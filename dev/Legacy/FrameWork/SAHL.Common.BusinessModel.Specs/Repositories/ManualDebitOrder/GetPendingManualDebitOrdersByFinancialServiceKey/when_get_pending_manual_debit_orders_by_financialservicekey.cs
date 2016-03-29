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

namespace SAHL.Common.BusinessModel.Specs.Repositories.ManualDebitOrder.GetPendingManualDebitOrdersByFinancialServiceKey
{
    internal class when_get_pending_manual_debit_orders_by_financialservicekey : ManualDebitOrderRepositoryWithFakesBase
    {
        Establish context = () =>
        {
            manualDebitOrderRepository.WhenToldTo(x => x.GetPendingManualDebitOrdersByFinancialServiceKey(Param.IsAny<int>())).Return(manualDebitOrders);
        };

        Because of = () =>
        {
            manualDebitOrderRepository.GetPendingManualDebitOrdersByFinancialServiceKey(Param.IsAny<int>());
        };

        It should_get_pending_manual_debit_orders_by_financialservicekey = () =>
        {
            manualDebitOrderRepository.WasToldTo(x => x.GetPendingManualDebitOrdersByFinancialServiceKey(Param.IsAny<int>()));
        };
    }
}