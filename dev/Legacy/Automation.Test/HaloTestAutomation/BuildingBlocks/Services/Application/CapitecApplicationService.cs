using System.Linq;
using BuildingBlocks.Services.Contracts;
using SAHL.Core.Identity;
using SAHL.Services.Capitec.Models.Shared;
using SAHL.Services.Capitec.Models.Shared.Capitec;
using System.Threading;
using Automation.DataAccess.DataHelper.Capitec;
using NUnit.Framework;
using System.Threading.Tasks;
using BuildingBlocks.Timers;
using System.Collections.Generic;
namespace BuildingBlocks.Services
{
    public sealed class CapitecApplicationService: ICapitecApplicationService
    {
        private IMessagePublishService messagePublishService;
        private ICapitecApplicationDataHelper capitecApplicationDataHelper;
        private IApplicationService applicationDataHelper;
        public CapitecApplicationService(IMessagePublishService messagePublishService, ICapitecApplicationDataHelper capitecApplicationDataAccess, IApplicationService applicationDataHelper)
        {
            this.messagePublishService = messagePublishService;
            this.capitecApplicationDataHelper = capitecApplicationDataAccess;
            this.applicationDataHelper = applicationDataHelper;
        }
        public void CreateCapitecApplication(CapitecApplication capitecApplication)
        {
            var offerExists = this.applicationDataHelper.OfferKeyExists(capitecApplication.ReservedApplicationKey);
            Assert.False(offerExists, "reserved applicationkey {0} already in use, this is bad.", capitecApplication.ReservedApplicationKey);
            var messageId = CombGuid.Instance.Generate();
            var createApplicationRequest = new CreateCapitecApplicationRequest(capitecApplication, messageId);
            this.messagePublishService.Publish<CreateCapitecApplicationRequest>(createApplicationRequest);

            //Need to check that the offer was really created before updating the isused
            GeneralTimer.BlockWaitFor(1000, 20, () => {
                return this.applicationDataHelper.OfferKeyExists(capitecApplication.ReservedApplicationKey);
            });
            this.capitecApplicationDataHelper.UpdateReservedApplicationNumber(capitecApplication.ReservedApplicationKey, true);
            
            GeneralTimer.Wait(4000);
        }
        public int GetUnusedOfferKey()
        {
            return this.capitecApplicationDataHelper.GetReservedApplications()
                .Where(x => !x.IsUsed)
                .Select(x => x.ApplicationNumber).First();
        }
    }
}
