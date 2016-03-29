'use strict';
angular.module('sahl.js.core.templating')
.service('$templateManagerService', ['$q', '$http', '$templateCache', function ($q, $http, $templateCache) {
    var operations = {
        get: function (templateUrl) {
            var deferred = $q.defer();
            $http.get(templateUrl, { cache: $templateCache }).then(function (response) {
                deferred.resolve(response.data);
            });
            return deferred.promise;
        }
    };
    return {
        get: operations.get
    };
}]);
