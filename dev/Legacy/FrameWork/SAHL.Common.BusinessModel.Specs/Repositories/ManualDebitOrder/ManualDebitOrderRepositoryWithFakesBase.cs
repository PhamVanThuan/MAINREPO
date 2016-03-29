using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.DataAccess;

namespace SAHL.Common.BusinessModel.Specs.Repositories.ManualDebitOrder
{
    public class ManualDebitOrderRepositoryWithFakesBase : WithFakes
    {
        protected static IManualDebitOrderRepository manualDebitOrderRepository;
        protected static IEventList<IManualDebitOrder> manualDebitOrders;

        public ManualDebitOrderRepositoryWithFakesBase()
        {
            manualDebitOrderRepository = An<IManualDebitOrderRepository>();
            manualDebitOrders = new EventList<IManualDebitOrder>();
        }
    }
}
