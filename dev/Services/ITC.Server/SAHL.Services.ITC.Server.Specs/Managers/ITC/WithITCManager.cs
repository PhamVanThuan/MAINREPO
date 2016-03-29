using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Rules;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ITC.Commands;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.Managers.Itc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ITC.Server.Specs.Managers.ITC
{
    public class WithITCManager : WithFakes
    {
        protected static ILogger logger;
        protected static ILoggerSource loggerSource;
        protected static IItcDataManager dataManager;
        protected static ICombGuid combGuid;
        protected static IItcManager itcManager;
        protected static string clientIdNumber;
        protected static int clientId;
        protected static ItcRequest itcRequest;
        protected static IClientDomainServiceClient clientDomainService;
        protected static IAddressDomainServiceClient addressDomainService;
        protected static IEnumerable<ClientDetailsQueryResult> clientDetails;
        protected static IEnumerable<GetClientStreetAddressByClientKeyQueryResult> clientAddresses;
        protected static IDomainRuleManager<PerformClientITCCheckCommand> domainRuleContext;
        protected static IApplicationDomainServiceClient applicationDomainClient;
        protected static int applicationNumber;
        protected static string userId;

        private Establish context = () =>
        {

            logger = An<ILogger>();
            loggerSource = An<ILoggerSource>();
            dataManager = An<IItcDataManager>();
            combGuid = An<ICombGuid>();
            clientDomainService = An<IClientDomainServiceClient>();
            addressDomainService = An<IAddressDomainServiceClient>();
            applicationDomainClient = An<IApplicationDomainServiceClient>();
            domainRuleContext = An<IDomainRuleManager<PerformClientITCCheckCommand>>();
            itcManager = new ItcManager(logger, loggerSource, dataManager, combGuid);
            applicationNumber = 1001;
            userId = "X2";

            clientIdNumber = "8207052599069";
            clientId = 1001;
            clientDetails = new ClientDetailsQueryResult[] {
                new ClientDetailsQueryResult
                {
                    LegalEntityKey = clientId,
                    FirstNames = "Bob",
                    Surname = "Jones",
                    DateOfBirth = new DateTime(2003, 07, 23),
                    IDNumber = "8207052599069",
                    SalutationKey = (int)SalutationType.Mr,
                    HomePhoneCode = "031",
                    HomePhoneNumber = "5605800",
                    WorkPhoneCode = "031",
                    WorkPhoneNumber = "5605801",
                    CellPhoneNumber = "0781234567",
                    EmailAddress = "bob.jones@bjones.com",
                }
            };

            clientAddresses = new List<GetClientStreetAddressByClientKeyQueryResult>
                {
                    new GetClientStreetAddressByClientKeyQueryResult  { 
                        StreetNumber = "263",
                        StreetName = "Mandela Street",
                        Suburb = "Soweto",
                        City = "Johannesburgh",
                        PostalCode = "3105"
                    },
                    new GetClientStreetAddressByClientKeyQueryResult  { 
                        StreetNumber = "263",
                        StreetName = "Mandela Street",
                        Suburb = "Soweto",
                        City = "Johannesburgh",
                        PostalCode = "3105"
                    }
                };

            var Address1 = clientAddresses.First();
            var client = clientDetails.First();

            itcRequest = new ItcRequest
            {
                Forename1 = client.FirstNames,
                Surname = client.Surname,
                BirthDate = client.DateOfBirth.Value.ToString("yyyyMMdd"),
                IdentityNo1 = client.IDNumber,
                Title = ((SalutationType)client.SalutationKey).ToString(),
                HomeTelNo = client.HomePhoneNumber,
                WorkTelNo = client.WorkPhoneNumber,
                CellNo = client.CellPhoneNumber,
                EmailAddress = client.EmailAddress,
                AddressLine1 = Address1.StreetNumber,
                AddressLine2 = Address1.StreetName,
                Suburb = Address1.Suburb,
                City = Address1.City,
                PostalCode = Address1.PostalCode
            };

        };
    }
}