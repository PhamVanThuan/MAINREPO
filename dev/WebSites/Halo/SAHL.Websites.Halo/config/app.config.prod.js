'use strict';
// domain becomes $domainWebService
var globalConfiguration = {
    webServices: {
        userProfile: 'http://sahl-ws02/UserProfileService',
        halo: 'http://sahl-ws02/haloservice',
        logging: 'http://sahl-ws03/loggingService',
        workflowTask: 'http://sahl-ws03/WorkflowTaskService',
        search: 'http://sahl-ws02/searchService',
        jsonDocument: 'http://sahl-ws03/jsonDocumentService',
        query: 'http://sahl-ws02/QueryService',
        x2: 'http://sahl14/X2EngineService',
        documentManager: 'http://sahl-ws02/DocumentManagerService',
        domainProcessManagerProxy: 'http://sahl-ws03/DomainProcessManagerProxyService',
        financeDomain: 'http://sahl-ws02/FinanceDomainService',
        workflowAssignmentDomain: 'http://sahl-ws02/workflowassignmentdomainservice',
        cats: 'http://sahl-ws03/CATSService'
    },
    domainServices: {
        AddressDomainService : {
            name : 'Address Domain Service',
            url : 'http://sahl-ws02/AddressDomainService/docs'
        },
        ApplicationDomainService : {
            name: 'Application Domain Service',
            url: 'http://sahl-ws02/ApplicationDomainService/docs'
        },
        BankAccountDomainService : {
            name: 'Bank Account Domain Service',
            url: 'http://sahl-ws02/BankAccountDomainService/docs'
        },
        ClientDomainService : {
            name : 'Client Domain Service',
            url: 'http://sahl-ws02/ClientDomainService/docs'
        },
        FinanceDomainService: {
            name: 'Finance Domain Service',
            url: 'http://sahl-ws02/FinanceDomainService/docs'
        }
    }
};

angular.module('sahl.services.config', []).constant('$configuration', globalConfiguration);
