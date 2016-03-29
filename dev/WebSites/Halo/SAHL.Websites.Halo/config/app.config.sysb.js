'use strict';
// domain becomes $domainWebService
var globalConfiguration = {
    webServices: {
        userProfile: 'http://sysbws02/UserProfileService',
        halo: 'http://sysbws02/haloservice',
        logging: 'http://sysbws03/loggingService',
        workflowTask: 'http://sysbws03/WorkflowTaskService',
        search: 'http://sysbws02/searchService',
        jsonDocument: 'http://sysbws03/jsonDocumentService',
        query: 'http://sysbws02/QueryService',
        x2: 'http://sysb14/X2EngineService',
        documentManager: 'http://sysbws02/DocumentManagerService',
        domainProcessManagerProxy: 'http://sysbws03/DomainProcessManagerProxyService',
        financeDomain: 'http://sysbws02/FinanceDomainService',
        workflowAssignmentDomain: 'http://sysbws02/workflowassignmentdomainservice',
        cats: 'http://sysbws03/CATSService'
    },
    domainServices: {
        AddressDomainService : {
            name : 'Address Domain Service',
            url : 'http://sysbws02/AddressDomainService/docs'
        },
        ApplicationDomainService : {
            name: 'Application Domain Service',
            url: 'http://sysbws02/ApplicationDomainService/docs'
        },
        BankAccountDomainService : {
            name: 'Bank Account Domain Service',
            url: 'http://sysbws02/BankAccountDomainService/docs'
        },
        ClientDomainService : {
            name : 'Client Domain Service',
            url: 'http://sysbws02/ClientDomainService/docs'
        },
        FinanceDomainService: {
            name: 'Finance Domain Service',
            url: 'http://sysbws02/FinanceDomainService/docs'
        }
    }
};

angular.module('sahl.services.config', []).constant('$configuration', globalConfiguration);
