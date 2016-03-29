'use strict';

(function () {
    angular.module('halo.core.start.entityActionBar', ['sahl.js.core.eventAggregation', 'halo.core.start.entityActionBarServices'])
        .directive('entityActionBar', ['$entityActionBarSlidingWindowService', function ($entityActionBarSlidingWindowService) {
            return {
                restrict: 'E',
                scope: {
                    ngModel: "=",
                    actionWhenClicking: '&'
                },
                require: 'ngModel',
                templateUrl: 'app/start/entityActionBar/entityActionBar.tpl.html',
                controller: 'entityActionBarController',
                link: function (scope, element, attr, ngModel) {
                    scope.performAction = function (actionToPerform) {
                        scope.actionWhenClicking({action: actionToPerform});
                    };
                    var renderTiles = function (arrayOfActions) {
                        scope.allActionsInRespectiveGroups = {};
                        var maxItemsInArrayOfActions = arrayOfActions ? arrayOfActions.length : 0;
                        for (var counterForArrayOfActions = 0; counterForArrayOfActions < maxItemsInArrayOfActions; counterForArrayOfActions++) {
                            var tile = arrayOfActions[counterForArrayOfActions];
                            var group = tile.tileAction.group;
                            if (!scope.allActionsInRespectiveGroups[group]) {
                                scope.allActionsInRespectiveGroups[group] = {
                                    placements: [],
                                    includedWithFilter: true
                                };
                            }
                            scope.allActionsInRespectiveGroups[group].placements.push(counterForArrayOfActions);
                        }
                        $entityActionBarSlidingWindowService.recalculate(scope.allActionsInRespectiveGroups, scope.ngModel);
                    };

                    var setMaxNumberOnActionsOnScreenAndRerender = function () {
                        var pixelsRequiredPerTile = 100;
                        $entityActionBarSlidingWindowService.setMaxAmountOfActionsOnScreen(Math.floor($('#tile-action-content').width() / pixelsRequiredPerTile));

                    };


                    var onScreenResize = function () {
                        setMaxNumberOnActionsOnScreenAndRerender();
                        $entityActionBarSlidingWindowService.recalculate(scope.allActionsInRespectiveGroups);
                        scope.$apply();
                    };
                    window.onresize = _.debounce(onScreenResize, 100, false);

                    ngModel.$formatters.push(renderTiles);
                    setMaxNumberOnActionsOnScreenAndRerender();
                    renderTiles();
                }
            };

        }])
        .controller('entityActionBarController', ['$scope', '$entityActionBarSlidingWindowService', '$eventAggregatorService', function ($scope, $entityActionBarSlidingWindowService, $eventAggregatorService) {

            var internal = {

                numberOfGroupsNotShowingOnBar: 0,
                eventHandlers: {
                    whenViewableWindowChanges: function (newViewableWindow) {
                        $scope.viewableActions = newViewableWindow;
                    },
                    whenScrollLeftChanges: function (scrollable) {
                        $scope.canScrollLeft = scrollable;
                    },
                    whenScrollRightChanges: function (scrollable) {
                        $scope.canScrollRight = scrollable;
                    }
                },
                eventListeners: {
                    subscribeToEvents: function () {
                        $eventAggregatorService.subscribe($entityActionBarSlidingWindowService.publishingEvents.viewableWindowChanged, internal.eventHandlers.whenViewableWindowChanges);
                        $eventAggregatorService.subscribe($entityActionBarSlidingWindowService.publishingEvents.scrollRightChanged, internal.eventHandlers.whenScrollRightChanges);
                        $eventAggregatorService.subscribe($entityActionBarSlidingWindowService.publishingEvents.scrollLeftChanged, internal.eventHandlers.whenScrollLeftChanges);
                    },
                    unSubscribeEvents: function () {
                        $eventAggregatorService.unsubscribe($entityActionBarSlidingWindowService.publishingEvents.viewableWindowChanged, internal.eventHandlers.whenViewableWindowChanges);
                        $eventAggregatorService.unsubscribe($entityActionBarSlidingWindowService.publishingEvents.scrollRightChanged, internal.eventHandlers.whenScrollRightChanges);
                        $eventAggregatorService.unsubscribe($entityActionBarSlidingWindowService.publishingEvents.scrollLeftChanged, internal.eventHandlers.whenScrollLeftChanges);
                    }
                },
                updateActionBarFilteringFlagIsApplicable: function (isIncludedInFilter) {
                    internal.numberOfGroupsNotShowingOnBar += isIncludedInFilter ? -1 : 1;
                    $scope.currentlyFiltering = internal.numberOfGroupsNotShowingOnBar > 0;
                }

            };


            var operations = {
                changeFilteringWith: function (grouped) {
                    grouped.includedWithFilter = !grouped.includedWithFilter;
                    internal.updateActionBarFilteringFlagIsApplicable(grouped.includedWithFilter);
                    $entityActionBarSlidingWindowService.resetWindow($scope.allActionsInRespectiveGroups);
                },

                initScope: function () {
                    $scope.currentlyFiltering = false;
                    $scope.changeFilteringWith = operations.changeFilteringWith;
                    $scope.scrollLeft = $entityActionBarSlidingWindowService.scrollLeft;
                    $scope.scrollRight = $entityActionBarSlidingWindowService.scrollRight;
                    internal.eventListeners.subscribeToEvents();
                    $scope.$on('$destroy', internal.eventListeners.unSubscribeEvents);
                }
            };
            operations.initScope();
        }]).directive('groupTitle', ['$timeout',function ($timeout) {
            return {
                restrict: 'E',
                scope: {
                    numberOfItems : '=',
                    groupName : '='
                },
                template: '<div id="displayTitle" class="tile-action-group-title"><span ng-if="ellipse" class="group-title-ellipse" tool-tip="groupName.trim()">...</span><div id="innerTitle" class="inner-action-group-title">{{groupName}}</div></div>',
                link: function (scope, element) {
                    var holder = angular.element(element[0].querySelector(".tile-action-group-title"));
                    holder.width(scope.numberOfItems * 98 -2);
                    $timeout(function(){
                        scope.ellipse=  holder.width() < holder.children()[0].scrollWidth;
                    },1);
                }
            };
        }]);
})();