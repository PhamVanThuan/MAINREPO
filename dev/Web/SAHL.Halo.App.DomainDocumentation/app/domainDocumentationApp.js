'use strict';
angular.module('sahl.halo.app.domainDocumentation', [
    'sahl.halo.app.domainDocumentation.start',
    'sahl.js.core.applicationManagement'
])
.service('$domainDocumentationAppService', ['$documentApplicationService',
function ($documentApplicationService) {
    var operations = {
        start: function () {
            $documentApplicationService.register('Domain Service Documentation', 'View functions, requirements and rules for a domain services', 'start.portalPages.apps.domainDocumentation', 'sahl.halo.app.domainDocumentation/assets/images/icon.png', 0);
        }
    }
    return {
        start: operations.start
    };
}]);
