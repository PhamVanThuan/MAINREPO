'use strict';
// domain becomes $domainWebService
var globalConfiguration = {
    webServices: {
        userProfile: 'http://devcws02/UserProfileService',
        halo: 'http://devcws02/haloservice',
        logging: 'http://devcws03/loggingService',
        workflowTask: 'http://devcws03/WorkflowTaskService',
        search: 'http://devcws02/searchService',
        jsonDocument: 'http://devcws03/jsonDocumentService',
        query: 'http://devcws02/QueryService',
        x2: 'http://devc14/X2EngineService',
        documentManager: 'http://devcws02/DocumentManagerService',
        domainProcessManagerProxy: 'http://devcws03/DomainProcessManagerProxyService',
        financeDomain: 'http://devcws02/FinanceDomainService',
        workflowAssignmentDomain: 'http://devcws02/workflowassignmentdomainservice',
        cats: 'http://devcws03/CATSService'
    },
    domainServices: {
        AddressDomainService : {
            name : 'Address Domain Service',
            url : 'http://devcws02/AddressDomainService/docs'
        },
        ApplicationDomainService : {
            name: 'Application Domain Service',
            url: 'http://devcws02/ApplicationDomainService/docs'
        },
        BankAccountDomainService : {
            name: 'Bank Account Domain Service',
            url: 'http://devcws02/BankAccountDomainService/docs'
        },
        ClientDomainService : {
            name : 'Client Domain Service',
            url: 'http://devcws02/ClientDomainService/docs'
        },
        FinanceDomainService: {
            name: 'Finance Domain Service',
            url: 'http://devcws02/FinanceDomainService/docs'
        }
    }
};

angular.module('sahl.services.config', []).constant('$configuration', globalConfiguration);
