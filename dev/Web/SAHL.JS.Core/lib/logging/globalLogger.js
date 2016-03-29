'use strict';
angular.module('sahl.js.core.logging')
.provider('$globalLogger', [function () {


    this.decoration = ['$delegate', '$injector','$window', function ($delegate, $injector,$window) {
        var $logger;
        return function (exception, cause) {
            $delegate(exception, cause);
            $logger = $logger || $injector.get('$logger');
            $logger.logError(exception.message, exception.stack);
            throw exception;
        };
    }];


    this.$get = [function GlobalLoggerFactory() { }];
}]);