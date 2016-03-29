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
    public class UpdateAddressCommandHandler : IServiceCommandHandler<UpdateAddressCommand>
    {
        public ITestDataManager testDataManager { get; set; }
        public UpdateAddressCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }
        public Core.SystemMessages.ISystemMessageCollection HandleCommand(UpdateAddressCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            testDataManager.UpdateAddress(command.Address);
            return messages;
        }
    }
}
