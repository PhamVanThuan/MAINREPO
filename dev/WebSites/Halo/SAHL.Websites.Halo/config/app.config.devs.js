'use strict';
// domain becomes $domainWebService
var globalConfiguration = {
    webServices: {
        userProfile: 'http://devsws02/UserProfileService',
        halo: 'http://devsws02/haloservice',
        logging: 'http://devsws03/loggingService',
        workflowTask: 'http://devsws03/WorkflowTaskService',
        search: 'http://devsws02/searchService',
        jsonDocument: 'http://devsws03/jsonDocumentService',
        query: 'http://devsws02/QueryService',
        x2: 'http://devs14/X2EngineService',
        documentManager: 'http://devsws02/DocumentManagerService',
        domainProcessManagerProxy: 'http://devsws03/DomainProcessManagerProxyService',
        financeDomain: 'http://devsws02/FinanceDomainService',
        workflowAssignmentDomain: 'http://devsws02/workflowassignmentdomainservice',
        cats: 'http://devsws03/CATSService'
    },
    domainServices: {
        AddressDomainService : {
            name : 'Address Domain Service',
            url : 'http://devsws02/AddressDomainService/docs'
        },
        ApplicationDomainService : {
            name: 'Application Domain Service',
            url: 'http://devsws02/ApplicationDomainService/docs'
        },
        BankAccountDomainService : {
            name: 'Bank Account Domain Service',
            url: 'http://devsws02/BankAccountDomainService/docs'
        },
        ClientDomainService : {
            name : 'Client Domain Service',
            url: 'http://devsws02/ClientDomainService/docs'
        },
        FinanceDomainService: {
            name: 'Finance Domain Service',
            url: 'http://devsws02/FinanceDomainService/docs'
        }
    }
};

angular.module('sahl.services.config', []).constant('$configuration', globalConfiguration);
