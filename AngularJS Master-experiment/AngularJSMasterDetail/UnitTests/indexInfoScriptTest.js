﻿describe('Controller: angularControllers', function () {

    // load the controller's module
    beforeEach(module('angularApp'));

    var MainCtrl,scope;

    // Initialize the controller and a 'mock' scope
    beforeEach(inject(function ($controller, $rootScope) {
        scope = $rootScope.$new();
        MainCtrl = $controller('MainCtrl', {
            $scope: scope
        });
    }));
});