using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
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
    public class LinkLegalEntityAddressCommandHandler : IServiceCommandHandler<LinkLegalEntityAddressCommand>
    {
        public ITestDataManager testDataManager { get; set; }
        public LinkLegalEntityAddressCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }
        public ISystemMessageCollection HandleCommand(LinkLegalEntityAddressCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            testDataManager.InsertLegalEntityAddress(new LegalEntityAddressDataModel(command.LegalEntityKey, command.AddressKey, (int)command.AddressType, DateTime.Now, (int)GeneralStatus.Active));
            return messages;
        }
    }
}
