﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Rules.AllCapabilitiesMustBeLinkedToUser
{
    internal class when_both_user_organisation_structure_key_and_capabilities_are_not_provided : WithFakes
    {
        private static AllCapabilitiesMustBeLinkedToUserRule rule;
        private static IThirdPartyInvoiceDataManager dataManager;
        private static ServiceRequestMetadata serviceRequestMetadata;
        private static ISystemMessageCollection messages;
        private static String userCapabilities;
        private static List<String> CapabilitiesLinkedTouser;
        private static string orgStructureKey;

        private Establish context = () =>
        {
            orgStructureKey = null;
            var metaDataDictionary = new Dictionary<string, string>();
            userCapabilities = "";
            CapabilitiesLinkedTouser = new List<string>() { "Invoice Processor","Invoice Approver","Loss Control Fee Consultant"};
            metaDataDictionary.Add(ServiceRequestMetadata.HEADER_USERORGANISATIONSTRUCTUREKEY, orgStructureKey);
            metaDataDictionary.Add(ServiceRequestMetadata.HEADER_CURRENTUSERCAPABILITIES, userCapabilities);
            serviceRequestMetadata = new ServiceRequestMetadata(metaDataDictionary);
            messages = SystemMessageCollection.Empty();
            dataManager = An<IThirdPartyInvoiceDataManager>();
            dataManager.WhenToldTo(x => x.GetUserCapabilitiesByUserOrgStructureKey(1001)).Return(CapabilitiesLinkedTouser);
            rule = new AllCapabilitiesMustBeLinkedToUserRule(dataManager);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, serviceRequestMetadata);
        };

        private It should_get_all_capabilities_must_be_linked_to_user = () =>
        {
            dataManager.WasNotToldTo(x => x.GetUserCapabilitiesByUserOrgStructureKey(Param.IsAny<int>()));
        };

        private It should_not_return_error_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}