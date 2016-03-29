'use strict';
describe('[sahl.halo.app.variables.app.start]', function () {
    beforeEach(module('sahl.halo.app.variables.app.start'));
    beforeEach(inject(function ($injector, $q) {
    }));

    describe(' - (Controller: startCtrl)-', function () {
        beforeEach(inject(function ($injector, $q) {
            var $controller = $injector.get('$controller');
            createController = function () {
                return $controller('startCtrl', {
                });
            };
        }));
    });
});