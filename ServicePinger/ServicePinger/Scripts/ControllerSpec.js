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

    it('should attach items to the scope', function () {
        expect(scope.awesomeThings.length).toBe(3);
    });
});

describe('Controller: AboutCtrl', function () {
    beforeEach(module('ServicePinger'));
    var AboutCtrl,
      scope;

    beforeEach(inject(function ($controller, $rootScope) {
        scope = $rootScope.$new();
        AboutCtrl = $controller('AboutCtrl', {
            $scope: scope

        });
    }));

    it('should attach items to the scope', function () {
        expect(scope.awesomeThings.length).toBe(3);
    });
});

describe('Controller: LoginCtrl', function () {
    beforeEach(module('ServicePinger'));
    var LoginCtrl,
      scope;

    beforeEach(inject(function ($controller, $rootScope) {
        scope = $rootScope.$new();
        LoginCtrl = $controller('LoginCtrl', {
            $scope: scope
        });
    }));

    it('should attach items to the scope', function () {
        expect(scope.awesomeThings.length).toBe(3);
    });
});

describe('Controller: Error404Ctrl', function () {
    beforeEach(module('ServicePinger'));
    var HomeCtrl,
      scope;

    beforeEach(inject(function ($controller, $rootScope) {
        scope = $rootScope.$new();
        Error404Ctrl = $controller('Error404Ctrl', {
            $scope: scope
        });
    }));

    it('should attach items to the scope', function () {
        expect(scope.awesomeThings.length).toBe(3);
    });
});
