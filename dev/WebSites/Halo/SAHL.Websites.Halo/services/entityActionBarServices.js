(function () {
    angular.module('halo.core.start.entityActionBarServices', ['sahl.js.core.eventAggregation']).service('$entityActionBarSlidingWindowService', ['$eventAggregatorService', function ($eventAggregatorService) {

        var events = {
            scrollLeftChanged: 'EntityActionBarScrollLeftChanged',
            scrollRightChanged: 'EntityActionBarScrollRightChanged',
            viewableWindowChanged: 'EntityActionBarViewWindowChanged'
        };

        var internal = {
            canScrollRight: false,
            canScrollLeft: false,
            maxActionsViewable: 0,
            firstIndex: 0,
            masterListOfActions: [],
            groupMetadata: {},
            viewableActions: {},
            findStartPositionInGroup: function (numberOfActionsFound, firstIndex) {
                var counter = 0;
                if (numberOfActionsFound < firstIndex) {
                    counter = firstIndex - numberOfActionsFound;
                }
                return counter;
            },
            changeViewableWindow: function (newViewableWindow) {
                if (!angular.equals(internal.viewableActions, newViewableWindow)) {
                    internal.viewableActions = newViewableWindow;
                    $eventAggregatorService.publish(events.viewableWindowChanged, internal.viewableActions);
                }
            }, setInternalListsWhereApplicable: function (groupMetadata, masterListOfAction) {
                internal.groupMetadata = groupMetadata || internal.groupMetadata;
                internal.masterListOfActions = masterListOfAction || internal.masterListOfActions;
            }, updateLeftScrollable: function (newLeftScrollable) {
                if (newLeftScrollable !== internal.canScrollLeft) {
                    internal.canScrollLeft = newLeftScrollable;
                    $eventAggregatorService.publish(events.scrollLeftChanged, internal.canScrollLeft);

                }
            }, loadViewableTilesForGroup: function (key, numberOfActionsFound, effectiveLastIndex, viewableActions) {
                var createResponse = function (earlyStop) {
                    return {windowStopsBeforeEnd: earlyStop, numberOfActionsFound: numberOfActionsFound};
                };


                var dataForGroup = internal.groupMetadata[key];
                if (!dataForGroup.includedWithFilter) {
                    return createResponse(false);
                }

                var counter = internal.findStartPositionInGroup(numberOfActionsFound, internal.firstIndex);
                if (counter >= dataForGroup.placements.length) {
                    numberOfActionsFound += dataForGroup.placements.length;
                    return createResponse(false);
                }

                numberOfActionsFound += counter;
                var requiredToFill = effectiveLastIndex - numberOfActionsFound;
                var viewableTilesInGroup = dataForGroup.placements.slice(counter, requiredToFill).map(function (place) {
                    return internal.masterListOfActions[place];
                });
                var numberOfActionsShowingForGroup = viewableTilesInGroup.length;
                if (numberOfActionsShowingForGroup > 0) {
                    numberOfActionsFound += numberOfActionsShowingForGroup;
                    viewableActions[key] = viewableTilesInGroup;
                }
                return createResponse(requiredToFill < dataForGroup.placements.length);
            },
            updateRightScrollable: function (newRightScrollable) {
                if (newRightScrollable !== internal.canScrollRight) {
                    internal.canScrollRight = newRightScrollable;
                    $eventAggregatorService.publish(events.scrollRightChanged, internal.canScrollRight);
                }
            }, raiseChangedEvents: function (moreRight, viewableActions) {
                internal.updateLeftScrollable(internal.firstIndex > 0);
                internal.updateRightScrollable(moreRight);
                internal.changeViewableWindow(viewableActions);
            }

        };


        var operations = {

            recalculate: function (groupMetadata, masterListOfAction) {
                var effectiveLastIndex = internal.firstIndex + internal.maxActionsViewable;
                internal.setInternalListsWhereApplicable(groupMetadata, masterListOfAction);
                var viewableActions = {};
                var numberOfActionsFound = 0;
                var moreRight = false;
                for (var groupName in  internal.groupMetadata) {
                    if(internal.groupMetadata.hasOwnProperty(groupName)) {
                        var returnedObject = internal.loadViewableTilesForGroup(groupName, numberOfActionsFound, effectiveLastIndex, viewableActions);
                        if (returnedObject.windowStopsBeforeEnd) {
                            moreRight = returnedObject.windowStopsBeforeEnd;
                            break;
                        }
                        numberOfActionsFound = returnedObject.numberOfActionsFound;
                    }
                }
                internal.raiseChangedEvents(moreRight, viewableActions);
            },
            setMaxActionsViewable: function (max) {
                internal.maxActionsViewable = max;
            }
        };


        var movement = {
            moveFirstIndex: function (byNumber) {
                internal.firstIndex += byNumber;
                operations.recalculate();
            },
            scrollLeft: function (byNumber) {
                if (internal.canScrollLeft) {
                    movement.moveFirstIndex(-byNumber);
                }
            },
            scrollRight: function (byNumber) {
                if (internal.canScrollRight) {
                    movement.moveFirstIndex(byNumber);
                }
            },
            scrollToFront : function(groupMetadata, masterListOfAction){
                internal.firstIndex = 0;
                operations.recalculate(groupMetadata, masterListOfAction);
            }
        };

        return {
            publishingEvents: events,
            recalculate: operations.recalculate,
            setMaxAmountOfActionsOnScreen: operations.setMaxActionsViewable,
            scrollLeft: movement.scrollLeft,
            scrollRight: movement.scrollRight,
            resetWindow : movement.scrollToFront
        };
    }]);
})();
