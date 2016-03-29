'use strict';

describe('[halo.portalpages.myhalo.tags.mytags]', function () {
    beforeEach(module('halo.start.portalpages.tasks.tags.mytags'));
    beforeEach(module('sahl.js.ui.notifications'));

    var scope, tagService, eventAggrigatorService, rootScope, controller;
    var returnPromise = function (containing) {
        return {
            then: function (fn) {
                fn(containing);
            }
        };
    };
    var tags = {
        '6F1E32AF-74AC-49C2-B616-F33C1414A998': {
            Caption: 'test',
            Id: '6F1E32AF-74AC-49C2-B616-F33C1414A998',
            Style: {'background-color': '#FFFFFF', 'color': '#000000'}
        }, '8BB03246-4EA8-4FDC-9875-9C8BA48FB04E': {
            Caption: 'tess',
            Id: '8BB03246-4EA8-4FDC-9875-9C8BA48FB04E',
            Style: {'background-color': '#FFFFFF', 'color': '#000000'}
        }
    };
    describe('mytags', function () {
        beforeEach(inject(function ($rootScope, $tagService, $eventAggregatorService, $controller) {
            scope = $rootScope.$new();
            tagService = $tagService;
            eventAggrigatorService = $eventAggregatorService;
            controller = $controller;

            tagService.getTags = function () {
                return returnPromise(tags);
            };
            tagService.getFilter = ['6F1E32AF-74AC-49C2-B616-F33C1414A998'];
        }));
        describe('when loading', function () {
            var myController;
            beforeEach(function () {
                myController = controller('MyTagsCtrl', {
                    $scope: scope,
                    $tagService: tagService,
                    $eventAggregatorService: eventAggrigatorService
                });
            });
            describe('all Tags ', function () {
                it('should be set', function () {
                    expect(scope.allTags).not.toBe(null);
                });
            });
        });
        describe('checking if tagId is in active filter', function () {
            var myController;
            beforeEach(function () {
                myController = controller('MyTagsCtrl', {
                    $scope: scope,
                    $tagService: tagService,
                    $eventAggregatorService: eventAggrigatorService
                });
            });
            describe('when tag id is part of active filter', function () {
                it('should return true', function () {
                    expect(scope.$active('6F1E32AF-74AC-49C2-B616-F33C1414A998')).toBe(true);
                });
            });

            describe('when tag id is not part of the filter', function () {
                it('should return false', function () {
                    expect(scope.$active('8BB03246-4EA8-4FDC-9875-9C8BA48FB04E')).toBe(false);
                });
            });
        });

        describe('clearing filter', function () {
            var myController;
            beforeEach(function () {
                spyOn(tagService, 'clearTagFiltering');
                myController = controller('MyTagsCtrl', {
                    $scope: scope,
                    $tagService: tagService,
                    $eventAggregatorService: eventAggrigatorService
                });
                scope.clearSelection();
            });
            it('should call the service to clear the filter', function () {
                expect(tagService.clearTagFiltering).toHaveBeenCalled();
            });
        });

        describe('adding and removing from filter', function () {
            var myController;
            beforeEach(function () {

                myController = controller('MyTagsCtrl', {
                    $scope: scope,
                    $tagService: tagService,
                    $eventAggregatorService: eventAggrigatorService
                });
                spyOn(tagService, 'clearTagFiltering');
                spyOn(scope, '$active').and.callThrough();
                spyOn(tagService, 'removeTagFromFiltering');
                spyOn(tagService, 'addTagToFiltering');
            });
            describe('when id given is not part of filter', function () {
                beforeEach(function () {
                    scope.forFiltering('6F1E32AF-74AC-49C2-B616-F33C1414A998');
                });
                it('should call to check if tag is active', function () {
                    expect(scope.$active).toHaveBeenCalled();
                });
                it('should call to add tag to filter', function () {
                    expect(tagService.removeTagFromFiltering).toHaveBeenCalled();
                });
            });
            describe('when id given is part of filter', function () {
                beforeEach(function () {
                    scope.forFiltering('8BB03246-4EA8-4FDC-9875-9C8BA48FB04E');
                });
                it('should call to check if tag is active', function () {
                    expect(scope.$active).toHaveBeenCalled();
                });
                it('should call to add tag to filter', function () {
                    expect(tagService.addTagToFiltering).toHaveBeenCalled();
                });
            });

            describe('when filter is updated by event', function () {
                beforeEach(function () {
                    eventAggrigatorService.publish('FilterArrayItemsUpdated', ['8BB03246-4EA8-4FDC-9875-9C8BA48FB04E']);
                });
                it('should update filter array in scope', function () {
                    expect(scope.filterTags).toEqual(['8BB03246-4EA8-4FDC-9875-9C8BA48FB04E']);
                });
            });
        });

        describe('upon destroy', function () {
            var myController;
            beforeEach(function () {
                myController = controller('MyTagsCtrl', {
                    $scope: scope,
                    $tagService: tagService,
                    $eventAggregatorService: eventAggrigatorService
                });
                spyOn(eventAggrigatorService, 'unsubscribe');
                scope.$destroy();
            });
            it('should call to unsubscribe from updated tags object', function () {
                expect(eventAggrigatorService.unsubscribe.calls.count()).toEqual(2);
            });

        });

    });
});
