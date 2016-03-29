'use strict';

/* Services */

angular.module('sahl.tools.app.services.startables', [
    'sahl.tools.app.services.designSurfaceManager',
    'sahl.tools.app.services.undoManager',
    'sahl.tools.app.services.searchManager',
    'sahl.tools.app.services.selectionManager',
    'sahl.tools.app.services.clipboardManager',
    'sahl.tools.app.services.recentDocumentsService',
    'sahl.tools.app.services.documentVersionProviders.version_0_1',
    'sahl.tools.app.services.documentVersionProviders.version_0_2',
    'sahl.tools.app.services.documentVersionProviders.version_0_3',
    'sahl.tools.app.services.documentVersionProviders.version_0_4',
    'sahl.tools.app.services.debugService',
    'sahl.tools.app.services.breakpointService',
    'sahl.tools.app.services.testDataManager',
    'sahl.tools.app.services.highlightingService',
    'sahl.tools.app.services.queryGlobalsVersionService',
    'sahl.tools.app.services.globalDataManager'
])
.service('$startableServices', ['$q', '$designSurfaceManager', '$selectionManager', '$clipboardManager', '$undoManager', '$recentDocumentsService', '$documentVersionProviderVersion_0_1', '$documentVersionProviderVersion_0_2', '$documentVersionProviderVersion_0_3', '$documentVersionProviderVersion_0_4', '$debugService', '$searchManager', '$breakpointService', '$testDataManager', '$highlightingService', '$sessionLockService', '$queryGlobalsVersionService', '$codemirrorVariablesService', '$globalDataManager',
    function ($q) {
        var startableServices = arguments;
        var waitableServices = [];
        return {
            startServices: function () {
                angular.forEach(startableServices, function (value) {
                    if (value["start"] !== undefined) {
                        var result = value.start();
                        if (result !== undefined) {
                            if (result.hasOwnProperty('then')) {
                                waitableServices.push(result);
                            }
                        }
                    }
                })
                return $q.all(waitableServices);
            }
        };
    }]);