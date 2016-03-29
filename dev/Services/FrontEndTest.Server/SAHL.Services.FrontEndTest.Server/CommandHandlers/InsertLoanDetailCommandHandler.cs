using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Data;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class InsertLoanDetailCommandHandler : IServiceCommandHandler<InsertLoanDetailCommand>
    {
        public ITestDataManager testDataManager { get; set; }
        public ILinkedKeyManager linkedKeyManager { get; set; }
        public IUnitOfWorkFactory uowFactory { get; set; }
        public InsertLoanDetailCommandHandler(ITestDataManager testDataManager, ILinkedKeyManager linkedKeyManager, IUnitOfWorkFactory uowFactory)
        {
            this.testDataManager = testDataManager;
            this.linkedKeyManager = linkedKeyManager;
            this.uowFactory = uowFactory;
        }

        public ISystemMessageCollection HandleCommand(InsertLoanDetailCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            using (var uow = uowFactory.Build())
            {
                testDataManager.InsertLoanDetail(command.LoanDetail);
                linkedKeyManager.LinkKeyToGuid(command.LoanDetail.DetailKey, command.LoanDetailId);
                uow.Complete();
            }
            return messages;
        }
    }
}
