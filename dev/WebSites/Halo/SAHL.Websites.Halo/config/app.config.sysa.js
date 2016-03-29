'use strict';
// domain becomes $domainWebService
var globalConfiguration = {
    webServices: {
        userProfile: 'http://sysaws02/UserProfileService',
        halo: 'http://sysaws02/haloservice',
        logging: 'http://sysaws03/loggingService',
        workflowTask: 'http://sysaws03/WorkflowTaskService',
        search: 'http://sysaws02/searchService',
        jsonDocument: 'http://sysaws03/jsonDocumentService',
        query: 'http://sysaws02/QueryService',
        x2: 'http://sysa14/X2EngineService',
        documentManager: 'http://sysaws02/DocumentManagerService',
        domainProcessManagerProxy: 'http://sysaws03/DomainProcessManagerProxyService',
        financeDomain: 'http://sysaws02/FinanceDomainService',
        workflowAssignmentDomain: 'http://sysaws02/workflowassignmentdomainservice',
        cats: 'http://sysaws03/CATSService'
    },
    domainServices: {
        AddressDomainService : {
            name : 'Address Domain Service',
            url : 'http://sysaws02/AddressDomainService/docs'
        },
        ApplicationDomainService : {
            name: 'Application Domain Service',
            url: 'http://sysaws02/ApplicationDomainService/docs'
        },
        BankAccountDomainService : {
            name: 'Bank Account Domain Service',
            url: 'http://sysaws02/BankAccountDomainService/docs'
        },
        ClientDomainService : {
            name : 'Client Domain Service',
            url: 'http://sysaws02/ClientDomainService/docs'
        },
        FinanceDomainService: {
            name: 'Finance Domain Service',
            url: 'http://sysaws02/FinanceDomainService/docs'
        }
    }
};

angular.module('sahl.services.config', []).constant('$configuration', globalConfiguration);
