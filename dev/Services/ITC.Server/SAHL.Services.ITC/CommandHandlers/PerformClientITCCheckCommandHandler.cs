using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Data;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using SAHL.Services.Interfaces.ITC.Commands;
using SAHL.Services.ITC.Common;
using SAHL.Services.ITC.Managers.Itc;
using SAHL.Services.ITC.Rules;
using SAHL.Services.ITC.TransUnion;

namespace SAHL.Services.ITC.CommandHandlers
{
    public class PerformClientITCCheckCommandHandler : IServiceCommandHandler<PerformClientITCCheckCommand>
    {
        private IItcManager itcManager;
        private IDomainRuleManager<PerformClientITCCheckCommand> domainRuleContext;
        private ITransUnionService transUnionService;
        private IUnitOfWorkFactory unitOfWorkFactory;
        private IITCCommon itcCommon;
        private IClientDomainServiceClient clientDomainService;
        private IAddressDomainServiceClient addressDomainService;
        private IApplicationDomainServiceClient applicationDomainClient;

        public PerformClientITCCheckCommandHandler(IItcManager itcManager, ITransUnionService transUnionService, 
            IITCCommon itcCommon, IClientDomainServiceClient clientDomainService, IAddressDomainServiceClient addressDomainService, IDomainRuleManager<PerformClientITCCheckCommand> domainRuleContext,
            IApplicationDomainServiceClient applicationDomainClient)
        {
            this.itcManager = itcManager;
            this.transUnionService = transUnionService;
            this.itcCommon = itcCommon;
            this.clientDomainService = clientDomainService;
            this.addressDomainService = addressDomainService;
            this.domainRuleContext = domainRuleContext;

            this.domainRuleContext.RegisterRule(new ClientShouldBeRelatedToAccountRule(applicationDomainClient));
        }

        public ISystemMessageCollection HandleCommand(PerformClientITCCheckCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();

            domainRuleContext.ExecuteRules(systemMessages, command);

            IEnumerable<GetClientStreetAddressByClientKeyQueryResult> clientAddresses = new List<GetClientStreetAddressByClientKeyQueryResult>();

            ClientDetailsQueryResult clientDetails = PrepareClientDetails(command.IdNumber, systemMessages);
            if (systemMessages.HasErrors)
            {
                return systemMessages;
            }

            clientAddresses = PrepareAddressDetails(clientDetails.LegalEntityKey, systemMessages);
            if (systemMessages.HasErrors)
            {
                return systemMessages;
            }

            var itcRequest = itcManager.CreateITCRequestForApplicant(clientDetails, clientAddresses);
            try
            {
                var itcResponse = itcCommon.PerformITCRequest(itcManager, transUnionService, itcRequest, systemMessages);
                if (itcResponse != null)
                {
                    itcManager.SaveITC(clientDetails.LegalEntityKey, command.AccountKey, itcResponse, command.UserId);
                }
            }
            catch (Exception runtimeException)
            {
                itcManager.LogFailedITCRequestAndResponse(itcRequest, null, this.GetType().Name);
                throw runtimeException;
            }

            return systemMessages;
        }

        private IEnumerable<GetClientStreetAddressByClientKeyQueryResult> PrepareAddressDetails(int clientKey, ISystemMessageCollection systemMessages)
        {
            var clientAddressQuery = new GetClientStreetAddressByClientKeyQuery(clientKey);
            var response = addressDomainService.PerformQuery(clientAddressQuery);
            systemMessages.Aggregate(response);
            if (clientAddressQuery.Result == null || clientAddressQuery.Result.Results == null || clientAddressQuery.Result.Results.Count() == 0)
            {
                systemMessages.AddMessage(new SystemMessage("The client provided does not have a street address.", SystemMessageSeverityEnum.Error));
                return null;
            }
            return clientAddressQuery.Result.Results;
        }

        private ClientDetailsQueryResult PrepareClientDetails(string IdNumber, ISystemMessageCollection systemMessages)
        {
            var query = new FindClientByIdNumberQuery(IdNumber);
            systemMessages.Aggregate(clientDomainService.PerformQuery(query));
            if (query.Result == null || query.Result.Results == null || query.Result.Results.Count() == 0)
            {
                systemMessages.AddMessage(new SystemMessage("The IDNumber provided does not exist.", SystemMessageSeverityEnum.Error));
                return null;
            }
            return query.Result.Results.First();
        }
    }
}