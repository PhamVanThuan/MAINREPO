'use strict';
describe('[%namespace%]', function () {
    beforeEach(module('%namespace%'));
    beforeEach(inject(function ($injector, $q) {
    }));

    describe(' - (Controller: %ccClassName%Ctrl)-', function () {
        beforeEach(inject(function ($injector, $q) {
            var $controller = $injector.get('$controller');
            createController = function () {
                return $controller('%ccClassName%Ctrl', {
                });
            };
        }));
    });
});