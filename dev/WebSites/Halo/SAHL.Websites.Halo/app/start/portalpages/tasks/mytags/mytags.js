'use strict';

angular.module('halo.start.portalpages.tasks.tags.mytags', [
    'sahl.websites.halo.services.tagManagement',
    'sahl.websites.halo.services.workflowManagement',
    'sahl.js.core.eventAggregation'
])
.controller('MyTagsCtrl', [
    '$scope', '$tagService', '$eventAggregatorService',
    function ($scope, $tagService, $eventAggregatorService) {
        var eventsToSubscribeTo = $tagService.getEvents;
        var allTagsUpdate = function (tags) {
            $scope.allTags = tags;
        };

        $scope.filterTags = $tagService.getFilter;

        var filterUpdate = function (filter) {
            $scope.filterTags = filter;
        };

        $eventAggregatorService.subscribe(eventsToSubscribeTo.TAGSUPDATED, allTagsUpdate);
        $eventAggregatorService.subscribe(eventsToSubscribeTo.FILTERUPDATED, filterUpdate);

        $scope.$on('$destroy', function () {
            $eventAggregatorService.unsubscribe(eventsToSubscribeTo.TAGSUPDATED, allTagsUpdate);
            $eventAggregatorService.unsubscribe(eventsToSubscribeTo.FILTERUPDATED, filterUpdate);
        });

        $tagService.getTags().then(allTagsUpdate);

        $scope.$active = function (tagId) {
            return _.contains($scope.filterTags, tagId);
        };
        $scope.clearSelection = function () {
            $tagService.clearTagFiltering();
        };
        $scope.forFiltering = function (tagId) {
            if ($scope.$active(tagId)) {
                $tagService.removeTagFromFiltering(tagId);
            } else {
                $tagService.addTagToFiltering(tagId);
            }
        };
    }
]);