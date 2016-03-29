'use strict';

describe('halo.portalpages.myhalo.tags.mytagmanager', function () {
    beforeEach(module('halo.start.portalpages.tasks.tags.mytagmanager'));
    beforeEach(module('sahl.js.ui.notifications'));
    var scope, tagService, eventAggrigatorService, rootScope, controller;
    var tagmanagerController;

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

    var getController = function () {
        return controller('MyTagManagerCtrl', {
            $scope: scope,
            $tagService: tagService,
            $eventAggregatorService: eventAggrigatorService
        });

    };
    beforeEach(inject(function ($rootScope, $tagService, $eventAggregatorService, $controller) {
        scope = $rootScope.$new();
        tagService = $tagService;
        eventAggrigatorService = $eventAggregatorService;
        controller = $controller;
        tagService.getTags = function () {
            return returnPromise(tags);
        };
        tagmanagerController = getController();
    }));
    describe('when initialising controller', function () {
        it('should not be null', function () {
            expect(tagmanagerController).not.toBe(null);
        });
    });

    describe('when moving between screens', function () {
        describe('when moving to editor', function () {
            beforeEach(
                function () {
                    scope.showEditor();
                });
            it('should set showingEditor to true', function () {
                expect(scope.showingEditor).toBe(true);
            });

            it('should set showtagMenu to false', function () {
                expect(scope.showTagMenu).toBe(false);
            });
        });

        describe('when moving back to main screen', function () {
            beforeEach(function () {
                scope.resetViewToBase();
            });
            it('should set showingEditor to false', function () {
                expect(scope.showingEditor).toBe(false);
            });
            it('should set showTagMenu to false', function () {
                expect(scope.showTagMenu).toBe(false);
            });
            it('should set hideScreen to false', function () {
                expect(scope.hideScreen).toBe(true);
            });
        });

        describe('when moving to main tag menu', function () {
            beforeEach(function () {
                scope.showMenu();
            });
            it('should  set showingEditor to false', function () {
                expect(scope.showingEditor).toBe(false);
            });
            it('should  set hideScreen to false', function () {
                expect(scope.hideScreen).toBe(false);
            });
            it('should  set showTagMenu to true', function () {
                expect(scope.showTagMenu).toBe(true);
            });
        });


    });
    describe('when editing a tag', function () {

        describe('loading the screen', function () {
            describe('function to load array of numbers', function () {
                var returned;
                var expectedReturned = [0, 1, 2, 3, 4];
                beforeEach(function () {
                    returned = scope.GetColourRows(5);
                });

                it('it should return enumeration til number', function () {
                    expect(returned.length).toBe(5);
                    expect(returned).toEqual(expectedReturned);
                });
            });

        });

        describe('on clicking to edit tag', function () {

            var sendingTag = {
                Caption: 'test',
                Id: '6F1E32AF-74AC-49C2-B616-F33C1414A998',
                Style: {'background-color': '#FFFFFF', 'color': '#000000'}
            };
            beforeEach(function () {
                spyOn(scope, 'showEditor');
                scope.editTag(sendingTag);
            });
            it('should show the editor ', function () {
                expect(scope.showEditor).toHaveBeenCalled();
            });
            it('should set the editing tag object equal to the one sent in, but not same object', function () {
                expect(scope.editingTag).toEqual(sendingTag);
                expect(scope.editingTag).not.toBe(sendingTag);
            });
        });

        describe('when deleteing', function () {
            beforeEach(function () {
                spyOn(tagService, 'deleteTag');
                scope.deleteGivenTag('8BB03246-4EA8-4FDC-9875-9C8BA48FB04E');
            });
            it('should call to delete tag', function () {
                expect(tagService.deleteTag).toHaveBeenCalled();
            });
        });

        describe('when selecting colour for tag', function () {
            var editingTag = {
                Caption: 'test',
                Id: '6F1E32AF-74AC-49C2-B616-F33C1414A998',
                Style: {'background-color': '#FFFFFF', 'color': '#000000'}

            };
            var newStyle = {'background-color': '#123456', 'color': '#123456'};

            var expectedTag = {
                Caption: 'test',
                Id: '6F1E32AF-74AC-49C2-B616-F33C1414A998',
                Style: newStyle

            };
            beforeEach(function () {
                scope.editingTag = editingTag;
                scope.changeColourSelection(newStyle);
            });
            it('should change the style of the current tag in edit', function () {
                expect(scope.editingTag).toEqual(expectedTag);
            });
        });

        describe('when saving the new tag', function () {
            beforeEach(function () {
                spyOn(scope, 'showMenu');
                spyOn(tagService, 'updateTag').and.returnValue(returnPromise());
                scope.editingTag = {
                    Caption: 'test',
                    Id: '6F1E32AF-74AC-49C2-B616-F33C1414A998',
                    Style: {'background-color': '#FEDCBA', 'color': '#123456'}
                };
                scope.save();
            });
            it('should call updating tag with values', function () {
                expect(tagService.updateTag).toHaveBeenCalledWith('6F1E32AF-74AC-49C2-B616-F33C1414A998', 'test', '#123456', '#FEDCBA');
            });
            it('should return to main tag view', function () {
                expect(scope.showMenu).toHaveBeenCalled();
            });
        });
    });


    describe('event aggregator', function () {
        describe('on load of controller', function () {
            beforeEach(function () {
                spyOn(eventAggrigatorService, 'subscribe');
                getController();
            });
            it('should subscribe to update event', function () {

                expect(eventAggrigatorService.subscribe).toHaveBeenCalledWith(tagService.getEvents.TAGSUPDATED, jasmine.any(Function));
            });
        });
        describe('on destroy', function () {
            beforeEach(function () {
                spyOn(eventAggrigatorService, 'unsubscribe');
                scope.$destroy();
            });
            it('should unsubscribe from events', function () {
                expect(eventAggrigatorService.unsubscribe).toHaveBeenCalledWith(tagService.getEvents.TAGSUPDATED, jasmine.any(Function));
            });
        });
        describe('on firing of event, should change allTags', function () {
            var newTags = {'6F1E32AF-74AC-49C2-B616-F33C1414A998': {
                Caption: 'test',
                Id: '6F1E32AF-74AC-49C2-B616-F33C1414A998',
                Style: {'background-color': '#FFFFFF', 'color': '#000000'}
            }};
            beforeEach(function () {
eventAggrigatorService.publish(tagService.getEvents.TAGSUPDATED, newTags );
            });
            it('should update allTags', function(){
                expect(scope.allTags).toEqual(newTags);
            });
        });
    });
});

