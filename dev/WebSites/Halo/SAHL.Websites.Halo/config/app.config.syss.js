'use strict';
// domain becomes $domainWebService
var globalConfiguration = {
    webServices: {
        userProfile: 'http://syssws02/UserProfileService',
        halo: 'http://syssws02/haloservice',
        logging: 'http://syssws03/loggingService',
        workflowTask: 'http://syssws03/WorkflowTaskService',
        search: 'http://syssws02/searchService',
        jsonDocument: 'http://syssws03/jsonDocumentService',
        query: 'http://syssws02/QueryService',
        x2: 'http://syss14/X2EngineService',
        documentManager: 'http://syssws02/DocumentManagerService',
        domainProcessManagerProxy: 'http://syssws03/DomainProcessManagerProxyService',
        financeDomain: 'http://syssws02/FinanceDomainService',
        workflowAssignmentDomain: 'http://syssws02/workflowassignmentdomainservice',
        cats: 'http://syssws03/CATSService'
    },
    domainServices: {
        AddressDomainService : {
            name : 'Address Domain Service',
            url : 'http://syssws02/AddressDomainService/docs'
        },
        ApplicationDomainService : {
            name: 'Application Domain Service',
            url: 'http://syssws02/ApplicationDomainService/docs'
        },
        BankAccountDomainService : {
            name: 'Bank Account Domain Service',
            url: 'http://syssws02/BankAccountDomainService/docs'
        },
        ClientDomainService : {
            name : 'Client Domain Service',
            url: 'http://syssws02/ClientDomainService/docs'
        },
        FinanceDomainService: {
            name: 'Finance Domain Service',
            url: 'http://syssws02/FinanceDomainService/docs'
        }
    }
};

angular.module('sahl.services.config', []).constant('$configuration', globalConfiguration);
