'use strict';
// domain becomes $domainWebService
var globalConfiguration = {
    webServices: {
        userProfile: 'http://uat2-ws02/UserProfileService',
        halo: 'http://uat2-ws02/haloservice',
        logging: 'http://uat2-ws03/loggingService',
        workflowTask: 'http://uat2-ws03/WorkflowTaskService',
        search: 'http://uat2-ws02/searchService',
        jsonDocument: 'http://uat2-ws03/jsonDocumentService',
        query: 'http://uat2-ws02/QueryService',
        x2: 'http://uat2-14/X2EngineService',
        documentManager: 'http://uat2-ws02/DocumentManagerService',
        domainProcessManagerProxy: 'http://uat2-ws03/DomainProcessManagerProxyService',
        financeDomain: 'http://uat2-ws02/FinanceDomainService',
        workflowAssignmentDomain: 'http://uat2-ws02/workflowassignmentdomainservice',
        cats: 'http://uat2-ws03/CATSService'
    },
    domainServices: {
        AddressDomainService: {
            name: 'Address Domain Service',
            url: 'http://uat2-ws02/AddressDomainService/docs'
        },
        ApplicationDomainService: {
            name: 'Application Domain Service',
            url: 'http://uat2-ws02/ApplicationDomainService/docs'
        },
        BankAccountDomainService: {
            name: 'Bank Account Domain Service',
            url: 'http://uat2-ws02/BankAccountDomainService/docs'
        },
        ClientDomainService: {
            name: 'Client Domain Service',
            url: 'http://uat2-ws02/ClientDomainService/docs'
        },
        FinanceDomainService: {
            name: 'Finance Domain Service',
            url: 'http://uat2-ws02/FinanceDomainService/docs'
        }
    }
};

angular.module('sahl.services.config', []).constant('$configuration', globalConfiguration);
