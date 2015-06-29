'use strict';

describe('Controller: HomeCtrl', function () {
    beforeEach(module('ServicePinger'));
    var HomeCtrl,
      scope;

    beforeEach(inject(function ($controller, $rootScope) {
        scope = $rootScope.$new();
        HomeCtrl = $controller('HomeCtrl', {
            $scope: scope
        });
    }));

    var AboutCtrl, scope;
    beforeEach(inject(function ($scontroller, $rootscope) {
        scope = $rootscope.$new();
        AboutCtrl = $controller('AboutCtrl', {
            $scope: scope
        });
    }));

    var LoginCtrl, scope;
    beforeEach(inject(function ($scontroller, $rootscope) {
        scope = $rootscope.$new();
        LoginCtrl = $controller('LoginCtrl', {
            $scope: scope
        });
    }));

    var Error404Ctrl, scope;
    beforeEach(inject(function ($scontroller, $rootscope) {
        scope = $rootscope.$new();
        Error404Ctrl = $controller('Error404Ctrl', {
            $scope: scope
        });
    }));

    it('should attach items to the scope', function () {
        expect(scope.awesomeThings.length).isNot(0);
    });
});