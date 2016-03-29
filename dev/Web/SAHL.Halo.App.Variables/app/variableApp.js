'use strict';
angular.module('sahl.halo.app.variables', ['sahl.js.core.applicationManagement', 'sahl.halo.app.variables.start'])
.service('$variablesAppService', ['$rootScope', '$documentApplicationService', 
function ($rootScope, $documentApplicationService) {
    var operations = {
        start: function () {
            $documentApplicationService.register('Variables Editor', 'Application to edit business variables, used within Halo, Decision Trees and X2', 'start.portalPages.apps.variables', 'sahl.halo.app.variables/assets/images/icon.png', 0);
        }
    }
    return {
        start: operations.start
    };
}]);