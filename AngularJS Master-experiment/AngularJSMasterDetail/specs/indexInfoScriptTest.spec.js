'use strict'

describe('Controller: angularControllers', function () {

    beforeEach(module('angularApp'));

    var MainCtrl,scope;

    beforeEach(inject(function ($controller, $rootScope) {
        scope = $rootScope.$new();
        MainCtrl = $controller('angularControllers', {
            $scope: scope
        });
    }));
});