'use strict';
describe('[halo.core.start.entityActionBar]', function () {
    var rootScope, scope, controller, eventAggregator, entityActionBarSlidingWindowService, compile, httpBackend;
    beforeEach(module('halo.core.start.entityActionBar'));
    beforeEach(inject(function ($rootScope, $eventAggregatorService, $controller, $entityActionBarSlidingWindowService, $compile, $httpBackend) {
        rootScope = $rootScope;
        eventAggregator = $eventAggregatorService;
        controller = $controller;
        entityActionBarSlidingWindowService = $entityActionBarSlidingWindowService;
        compile = $compile;
        httpBackend = $httpBackend;
    }));
    var createController = function () {
        scope = rootScope.$new();
        return controller('entityActionBarController', {
            $scope: scope,
            $entityActionBarSlidingWindowService: entityActionBarSlidingWindowService,
            $eventAggregatorService: eventAggregator
        });
    };
        var compileAndDigest = function (target, scope) {
            var justDirective = compile(target);
            httpBackend.flush();
            justDirective(scope);
            rootScope.$digest();
            return scope;
        };

    describe('- on', function () {
        var barController;
        describe('start', function () {
            beforeEach(function () {
                spyOn(eventAggregator, 'subscribe');
                barController = createController(scope);
            });
            it('controller should be set', function () {
                expect(barController).not.toBe(null);
            });
            describe('should call to setup the subscriptions for the events from the service', function () {
                it('can Scroll left changed', function () {
                    expect(eventAggregator.subscribe).toHaveBeenCalledWith('EntityActionBarScrollLeftChanged', jasmine.any(Function));
                });
                it('can scroll right changed', function () {
                    expect(eventAggregator.subscribe).toHaveBeenCalledWith('EntityActionBarScrollRightChanged', jasmine.any(Function));
                });
                it('viewable Window Changed', function () {
                    expect(eventAggregator.subscribe).toHaveBeenCalledWith('EntityActionBarViewWindowChanged', jasmine.any(Function));
                });
            });
        });
        describe('$destroy', function () {
            beforeEach(function () {
                spyOn(eventAggregator, 'unsubscribe');
                barController = createController();
                scope.$destroy();
            });
            describe('should unsubscribe from all events', function () {
                it('can Scroll left changed', function () {
                    expect(eventAggregator.unsubscribe).toHaveBeenCalledWith('EntityActionBarScrollLeftChanged', jasmine.any(Function));
                });
                it('can scroll right changed', function () {
                    expect(eventAggregator.unsubscribe).toHaveBeenCalledWith('EntityActionBarScrollRightChanged', jasmine.any(Function));
                });
                it('viewable Window Changed', function () {
                    expect(eventAggregator.unsubscribe).toHaveBeenCalledWith('EntityActionBarViewWindowChanged', jasmine.any(Function));
                });
            });
        });
    });

    describe('events', function () {
        var actionBarController;
        beforeEach(function () {
            actionBarController = createController();
        });
        describe('when viewableWindow changes', function () {
            var updatedActions = {newObject: 'sdf'};
            beforeEach(function () {
                eventAggregator.publish('EntityActionBarViewWindowChanged', updatedActions);
            });
            it('should update scope', function () {
                expect(scope.viewableActions).toEqual(updatedActions);
            });
        });
        describe('when ability to scroll changes', function () {
            beforeEach(function () {
                eventAggregator.publish('EntityActionBarScrollLeftChanged', true);
                eventAggregator.publish('EntityActionBarScrollRightChanged', true);
            });
            describe('- left', function () {
                it('should change ability to scroll on scope', function () {
                    expect(scope.canScrollRight).toBeTruthy();
                });
            });
            describe('- right', function () {
                it('should change ability to scroll on scope', function () {
                    expect(scope.canScrollLeft).toBeTruthy();
                });
            });
        });
    });

    describe('when changing the filtering of a group', function () {
        var barController;
        beforeEach(function () {
            spyOn(entityActionBarSlidingWindowService, 'resetWindow');
            barController = createController();
        });
        describe('- off', function () {
            beforeEach(function () {
                scope.allActionsInRespectiveGroups = {group1: {placements: [], includedWithFilter: true}};
                scope.changeFilteringWith(scope.allActionsInRespectiveGroups.group1);
            });
            describe('then -', function () {
                it('should change the include flag on the group', function () {
                    expect(scope.allActionsInRespectiveGroups.group1.includedWithFilter).toBeFalsy();
                });
                it('should turn on the  [currentlyFiltering] flag', function () {
                    expect(scope.currentlyFiltering).toBeTruthy();
                });
                it('should call to recalulate the window', function () {
                    expect(entityActionBarSlidingWindowService.resetWindow).toHaveBeenCalled();
                });
            });
        });
        describe('- on', function () {
            beforeEach(function () {
                scope.allActionsInRespectiveGroups = {group1: {placements: [], includedWithFilter: false}};
                scope.changeFilteringWith(scope.allActionsInRespectiveGroups.group1);
            });
            describe('then -', function () {
                it('should change the include flag on the group', function () {
                    expect(scope.allActionsInRespectiveGroups.group1.includedWithFilter).toBeTruthy();
                });
                it('should turn on the  [currentlyFiltering] flag', function () {
                    expect(scope.currentlyFiltering).toBeFalsy();
                });
                it('should call to recalulate the window', function () {
                    expect(entityActionBarSlidingWindowService.resetWindow).toHaveBeenCalled();
                });
            });
        });
    });


    describe('when using directive', function () {
        var innerScope;
        var internalAngularElement;


        describe('when startingUp', function () {
            var scope;
            beforeEach(function () {
                spyOn(entityActionBarSlidingWindowService, 'recalculate');
                httpBackend.whenGET('app/start/entityActionBar/entityActionBar.tpl.html').respond('<div id="tile-action-content"></div>');
                internalAngularElement = angular.element('<entity-action-bar ng-model="testingArray"></entity-action-bar>');
                innerScope = rootScope.$new();
                innerScope.testingArray = [{tileAction: {group: 'group1'}}, {tileAction: {group: 'group1'}}, {tileAction: {group: 'group2'}}];
            });
            it('directive should not error on creation', function () {
                expect(function () {
                    compileAndDigest(internalAngularElement, innerScope);
                }).not.toThrow();
            });
            describe('when finding grouped arrays', function () {
                it('should have each group as  a key', function () {
                    scope = compileAndDigest(internalAngularElement, innerScope);
                    expect(innerScope.$$childTail.allActionsInRespectiveGroups.group1).not.toBe(undefined);
                    expect(innerScope.$$childTail.allActionsInRespectiveGroups.group2).not.toBe(undefined);
                });
            });
        });
        describe('when window Resizes', function () {
            var calledRecalculate = false;
            beforeEach(function (done) {
                httpBackend.whenGET('app/start/entityActionBar/entityActionBar.tpl.html').respond('<div id="tile-action-content"></div>');
                internalAngularElement = angular.element('<entity-action-bar ng-model="testingArray"></entity-action-bar>');
                innerScope = rootScope.$new();
                innerScope.testingArray = [{tileAction: {group: 'group1'}}, {tileAction: {group: 'group1'}}, {tileAction: {group: 'group2'}}];
                scope = compileAndDigest(internalAngularElement, innerScope);
                spyOn(entityActionBarSlidingWindowService, 'recalculate').and.callFake(function(){
                    calledRecalculate = true;
                });
                setTimeout(function(){
                    done();
                }, 150);
                $(window).resize();

            });
            it('should call to recalculate the window', function () {
                expect(entityActionBarSlidingWindowService.recalculate).toHaveBeenCalled();
            });
        });
    });

});