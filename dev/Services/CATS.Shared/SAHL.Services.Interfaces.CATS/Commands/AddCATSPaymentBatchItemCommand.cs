using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.CATS.Models;

namespace SAHL.Services.Interfaces.CATS.Commands
{
    public class AddCATSPaymentBatchItemCommand : ServiceCommand, ICATSServiceCommand
    {
        public CATSPaymentBatchItemModel CATSPaymentBatchItemModel { get; protected set; }

        public AddCATSPaymentBatchItemCommand(CATSPaymentBatchItemModel catsPaymentBatchItemModel)
        {
            CATSPaymentBatchItemModel = catsPaymentBatchItemModel;
        }
    }
}
