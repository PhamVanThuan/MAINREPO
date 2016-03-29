'use strict';
describe('[sAHL.halo.app.domainDocumentation.app.start]', function () {
    beforeEach(module('sAHL.halo.app.domainDocumentation.app.start'));
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