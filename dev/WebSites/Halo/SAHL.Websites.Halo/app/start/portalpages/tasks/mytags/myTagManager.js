'use strict';

angular.module('halo.start.portalpages.tasks.tags.mytagmanager', [
    'sahl.websites.halo.services.tagManagement',
    'sahl.js.core.eventAggregation'
])
    .controller('MyTagManagerCtrl', [
        '$scope', '$tagService', '$eventAggregatorService',
        function ($scope, $tagService, $eventAggregatorService) {
            var eventsToSubscribeTo = $tagService.getEvents;
            var allTagsUpdate = function (tags) {
                $scope.allTags = tags;
            };
            $eventAggregatorService.subscribe(eventsToSubscribeTo.TAGSUPDATED, allTagsUpdate);

            $scope.$on('$destroy', function () {
                $eventAggregatorService.unsubscribe(eventsToSubscribeTo.TAGSUPDATED, allTagsUpdate);
            });

            $tagService.getTags();

            $scope.showEditor = function () {
                $scope.showingEditor = true;
                $scope.showTagMenu = false;
            };
            $scope.resetViewToBase = function () {
                $scope.showingEditor = false;
                $scope.showTagMenu = false;
                $scope.hideScreen = true;
            };

            $scope.showMenu = function () {
                $scope.showingEditor = false;
                $scope.showTagMenu = true;
                $scope.hideScreen = false;
            };

            $scope.editTag = function (tag) {
                $scope.showEditor();
                $scope.editingTag = angular.fromJson(angular.toJson(tag));
            };

            $scope.deleteGivenTag = function (tag) {
                $tagService.deleteTag(tag.Id);
            };

            $scope.GetColourRows = function (length) {
                length = Math.ceil(length);
                var arr = [],
                    i = 0;
                for (; i < length; i++) {
                    arr.push(i);
                }
                return arr;
            };

            $scope.tagColours = $tagService.predefinedColours;
            $scope.changeColourSelection = function (newSelection) {
                $scope.editingTag.Style = newSelection;
            };

            $scope.save = function () {
                var tag = $scope.editingTag;
                $tagService.updateTag(tag.Id, tag.Caption, tag.Style['color'], tag.Style['background-color']).then(function () {
                    $scope.showMenu();
                });
            };
        }
    ]);