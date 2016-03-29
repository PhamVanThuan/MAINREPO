'use strict';
// domain becomes $domainWebService
var globalConfiguration = {
    webServices: {
        userProfile: 'http://localhost/UserProfileService',
        halo: 'http://localhost/haloservice',
        logging: 'http://localhost/loggingService',
        workflowTask: 'http://localhost/WorkflowTaskService',
        search: 'http://localhost/searchService',
        jsonDocument: 'http://localhost/jsonDocumentService',
        query: 'http://localhost/QueryService',
        x2 : 'http://localhost/X2EngineService'
    },
    domainServices: {
        AddressDomainService: {
            name: 'Address Domain Service',
            url: 'http://localhost/AddressDomainService/docs'
        },
        ApplicationDomainService: {
            name: 'Application Domain Service',
            url: 'http://localhost/ApplicationDomainService/docs'
        },
        BankAccountDomainService: {
            name: 'Bank Account Domain Service',
            url: 'http://localhost/BankAccountDomainService/docs'
        },
        ClientDomainService: {
            name: 'Client Domain Service',
            url: 'http://localhost/ClientDomainService/docs'
        }
    }
};

angular.module('sahl.services.config', []).constant('$configuration', globalConfiguration);
