'use strict';
angular.module('sahl.js.core.routing')
.provider('$uiStateDecorator', [function () {
    this.decoration = ['$delegate', '$uiStateManagerService', '$rootScope', function ($delegate, $uiStateManagerService, $rootScope) {

        $rootScope.$on('$stateChangeStart', function () {
            $uiStateManagerService.transition(arguments[2]);
        });

        var goFn = $delegate.go;

        $delegate.go = function () {
            if (arguments[1] && arguments[1] instanceof Object) {
                $uiStateManagerService.set(arguments[1]);
            }
            goFn.apply(this, arguments);
        };

        return $delegate;
    }];

    this.$get = [function DecoratorFactory() {
    }];
}]);