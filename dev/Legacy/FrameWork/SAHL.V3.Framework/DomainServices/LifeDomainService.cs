using SAHL.Common.CacheData;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.LifeDomain;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.X2.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.DomainServices
{
    public class LifeDomainService : DomainServiceClientBase<ILifeDomainServiceClient>, ILifeDomainService
    {
        private IV3ServiceCommon v3ServiceCommon;
        private IServiceCoordinator serviceCoordinator;
        private ICombGuid combGuid;
        private ILinkedKeyManager linkedKeyManager;

        public LifeDomainService(ILifeDomainServiceClient lifeDomainServiceClient, IV3ServiceCommon v3ServiceCommon, IServiceCoordinator serviceCoordinator, ICombGuid combGuid, ILinkedKeyManager linkedKeyManager)
            : base(lifeDomainServiceClient)
        {
            this.v3ServiceCommon = v3ServiceCommon;
            this.serviceCoordinator = serviceCoordinator;
            this.combGuid = combGuid;
            this.linkedKeyManager = linkedKeyManager;
        }

        public new ISystemMessageCollection PerformQuery<T>(T query) where T : IServiceQuery
        {
            return base.PerformQuery(query);
        }

        public new ISystemMessageCollection PerformCommand<T>(T command) where T : IServiceCommand
        {
            return base.PerformCommand(command);
        }

        public bool CreateClaim(int AccountLifePolicyKey, int LegalEntityKey, out long instanceId)
        {
            int disabilityClaimKey = 0;
            instanceId = 0;
            IServiceRequestMetadata metadata = new ServiceRequestMetadata();
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();
            X2ServiceResponseWithInstance response = null;

            var serviceCoordinatorMessageCollection = this.serviceCoordinator.StartSequence(
                () =>
                {
                    systemMessageCollection = SystemMessageCollection.Empty();
                    Guid disabilityClaimGuid = this.combGuid.Generate();

                    LodgeDisabilityClaimCommand lodgeClaimCommand = new LodgeDisabilityClaimCommand(AccountLifePolicyKey, LegalEntityKey, disabilityClaimGuid);
                    systemMessageCollection = this.PerformCommand(lodgeClaimCommand);

                    if (!systemMessageCollection.HasErrors)
                    {
                        disabilityClaimKey = this.linkedKeyManager.RetrieveLinkedKey(disabilityClaimGuid);
                        this.linkedKeyManager.DeleteLinkedKey(disabilityClaimGuid);
                    }

                    this.v3ServiceCommon.HandleSystemMessages(systemMessageCollection);
                    return systemMessageCollection;
                },
                () =>
                {
                    systemMessageCollection = SystemMessageCollection.Empty();

                    if (disabilityClaimKey > 0)
                    {
                        CompensateLodgeDisabilityClaimCommand compensateLodgeClaimCommand = new CompensateLodgeDisabilityClaimCommand(disabilityClaimKey);
                        systemMessageCollection = this.PerformCommand(compensateLodgeClaimCommand);
                    }

                    this.v3ServiceCommon.HandleSystemMessages(systemMessageCollection);
                    return systemMessageCollection;

                }).ThenWithNoCompensationAction(
                    () =>
                    {
                        systemMessageCollection = SystemMessageCollection.Empty();
                        Dictionary<string, string> Inputs = new Dictionary<string, string>();
                        Inputs.Add("DisabilityClaimKey", disabilityClaimKey.ToString());                        
                        response = (X2ServiceResponseWithInstance)this.v3ServiceCommon.CreateWorkflowCase(systemMessageCollection, SAHL.Common.Constants.WorkFlowProcessName.LifeClaims, (-1).ToString(), SAHL.Common.Constants.WorkFlowName.DisabilityClaim, "Create Disability Claim", Inputs, false);
                        if (response.IsError)
                        {
                            systemMessageCollection.AddMessage(new SystemMessage("Error Creating Disability Claim Instance", SystemMessageSeverityEnum.Error));
                        }
                        this.v3ServiceCommon.HandleSystemMessages(systemMessageCollection);
                        return systemMessageCollection;
                    }).EndSequence().Run();

            this.v3ServiceCommon.HandleSystemMessages(serviceCoordinatorMessageCollection);

            if (response != null)
            {
                instanceId = response.InstanceId;
            }

            return ((systemMessageCollection.HasErrors) || (serviceCoordinatorMessageCollection.HasErrors));
        }
    }
}
