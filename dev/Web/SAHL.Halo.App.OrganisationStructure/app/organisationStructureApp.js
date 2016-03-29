'use strict';
angular.module('sahl.halo.app.organisationStructure',
    ['sahl.js.core.applicationManagement','sahl.halo.app.organisationStructure.start'])
    .service('$organisationStructureAppService',['$rootScope','$documentApplicationService',function ($rootScope, $documentApplicationService) {
    var operations = {
        start: function () {
            $documentApplicationService.register('Organisation Structure',
                'Application to edit organisation structure',
                'start.portalPages.apps.organisationStructure',
                'sahl.halo.app.organisationStructure/assets/images/icon.png',
                0);
        }
    }
    return {
        start: operations.start
    };
    }]);

