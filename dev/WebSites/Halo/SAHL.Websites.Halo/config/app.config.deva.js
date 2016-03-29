'use strict';
// domain becomes $domainWebService
var globalConfiguration = {
    webServices: {
        userProfile: 'http://devaws02/UserProfileService',
        halo: 'http://devaws02/haloservice',
        logging: 'http://devaws03/loggingService',
        workflowTask: 'http://devaws03/WorkflowTaskService',
        search: 'http://devaws02/searchService',
        jsonDocument: 'http://devaws03/jsonDocumentService',
        query: 'http://devaws02/QueryService',
        x2: 'http://deva14/X2EngineService',
        documentManager: 'http://devaws02/DocumentManagerService',
        domainProcessManagerProxy: 'http://devaws03/DomainProcessManagerProxyService',
        financeDomain: 'http://devaws02/FinanceDomainService',
        workflowAssignmentDomain: 'http://devaws02/workflowassignmentdomainservice',
        cats: 'http://devaws03/CATSService'
    },
    domainServices: {
        AddressDomainService : {
            name : 'Address Domain Service',
            url : 'http://devaws02/AddressDomainService/docs'
        },
        ApplicationDomainService : {
            name: 'Application Domain Service',
            url: 'http://devaws02/ApplicationDomainService/docs'
        },
        BankAccountDomainService : {
            name: 'Bank Account Domain Service',
            url: 'http://devaws02/BankAccountDomainService/docs'
        },
        ClientDomainService : {
            name : 'Client Domain Service',
            url: 'http://devaws02/ClientDomainService/docs'
        },
        FinanceDomainService: {
            name: 'Finance Domain Service',
            url: 'http://devaws02/FinanceDomainService/docs'
        }
    }
};

angular.module('sahl.services.config', []).constant('$configuration', globalConfiguration);
