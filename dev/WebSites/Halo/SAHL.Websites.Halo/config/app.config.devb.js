'use strict';
// domain becomes $domainWebService
var globalConfiguration = {
    webServices: {
        userProfile: 'http://devbws02/UserProfileService',
        halo: 'http://devbws02/haloservice',
        logging: 'http://devbws03/loggingService',
        workflowTask: 'http://devbws03/WorkflowTaskService',
        search: 'http://devbws02/searchService',
        jsonDocument: 'http://devbws03/jsonDocumentService',
        query: 'http://devbws02/QueryService',
        x2: 'http://devb14/X2EngineService',
        documentManager: 'http://devbws02/DocumentManagerService',
        domainProcessManagerProxy: 'http://devbws03/DomainProcessManagerProxyService',
        financeDomain: 'http://devbws02/FinanceDomainService',
        workflowAssignmentDomain: 'http://devaws02/workflowassignmentdomainservice',
        cats: 'http://devbws03/CATSService'
    },
    domainServices: {
        AddressDomainService : {
            name : 'Address Domain Service',
            url : 'http://devbws02/AddressDomainService/docs'
        },
        ApplicationDomainService : {
            name: 'Application Domain Service',
            url: 'http://devbws02/ApplicationDomainService/docs'
        },
        BankAccountDomainService : {
            name: 'Bank Account Domain Service',
            url: 'http://devbws02/BankAccountDomainService/docs'
        },
        ClientDomainService : {
            name : 'Client Domain Service',
            url: 'http://devbws02/ClientDomainService/docs'
        },
        FinanceDomainService: {
            name: 'Finance Domain Service',
            url: 'http://devbws02/FinanceDomainService/docs'
        }
    }
};

angular.module('sahl.services.config', []).constant('$configuration', globalConfiguration);
