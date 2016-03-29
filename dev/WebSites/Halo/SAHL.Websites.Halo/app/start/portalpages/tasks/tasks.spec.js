'use strict';
describe('[halo.portalpages.tasks]', function () {
    beforeEach(module('ui.router'));
    beforeEach(module('halo.start.portalpages.tasks'));
    var controller, rootScope, scope;
    var returnPromise = function (containing) {
        return {
            then: function (fn) {
                fn(containing);
            }
        };
    };
    describe('controller', function () {
        beforeEach(inject(function ($rootScope, $controller) {
            rootScope = $rootScope;
            scope = rootScope.$new;
            controller = $controller;
        }));
        describe(' - (task : page)', function () {
            var taskController, scope;
            beforeEach(function () {
                scope = rootScope.$new();
                taskController = controller('TasksCtrl', {
                    $scope: scope
                });
            });
            describe('on setup', function () {
                it('controller should not be null', function () {
                    expect(taskController).not.toBe(null);
                });
            });
        });
    });
});

