using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class LinkOfferMortgageLoanPropertyCommandHandler : IServiceCommandHandler<LinkOfferMortgageLoanPropertyCommand>
    {
        private ITestDataManager testDataManager;

        public LinkOfferMortgageLoanPropertyCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }
        public Core.SystemMessages.ISystemMessageCollection HandleCommand(LinkOfferMortgageLoanPropertyCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessages = SystemMessageCollection.Empty();
            this.testDataManager.LinkOfferMortgageLoanProperty(command.OfferKey, command.PropertyKey);
            return systemMessages;
        }
    }
}
