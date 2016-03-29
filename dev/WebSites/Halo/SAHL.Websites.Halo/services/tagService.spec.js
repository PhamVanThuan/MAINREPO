'use strict';
describe('[sahl.websites.halo.services.tagManagement]', function () {
    var tagService, q, workflowTaskWebService,
        workflowTaskCommands,
        workflowTaskQueries,
        userManagerService,
        eventAggregatorService,
        toastManagerService,
        returnQuery = {},
        rootScope,
        returnCommand = {},
        timeout,
        fullusername = 'SAHL\\user';

    var promise = function (returnWith) {
        var deffered = q.defer();
        deffered.resolve(returnWith);
        return deffered.promise;
    };
    var validResponse = function (obj) {
        return {
            data: {
                ReturnData: {
                    Results: {
                        $values: [obj]
                    }
                }
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

    beforeEach(module('sahl.websites.halo.services.tagManagement', function ($provide) {
        workflowTaskWebService = {
            sendQueryAsync: function (query, options) {
                return returnQuery;
            },
            sendCommandAsync: function (command) {
                return returnCommand;
            }
        };
        $provide.value('$workflowTaskWebService', workflowTaskWebService);
        $provide.value('$toastManagerService', toastManagerService);
        $provide.value('$userManagerService', {
            getAuthenticatedUser: function () {
                return { fullAdName: fullusername };
            }
        });
    }));
    beforeEach(inject(function ($injector, $eventAggregatorService) {
        q = $injector.get('$q');
        spyOn(workflowTaskWebService, 'sendQueryAsync').and.callThrough();
        spyOn(workflowTaskWebService, 'sendCommandAsync').and.callThrough();
        workflowTaskCommands = $injector.get('$workflowTaskCommands');
        workflowTaskQueries = $injector.get('$workflowTaskQueries');
        timeout = $injector.get('$timeout');
        eventAggregatorService = $eventAggregatorService;
        spyOn(eventAggregatorService, 'publish');
        tagService = $injector.get('$tagService');
        rootScope = $injector.get('$rootScope');
    }));

    describe('on load', function () {
        it('tag service should not be null', function () {
            expect(tagService).not.toBe(null);
        });
    });

    describe('when fetching Tags', function () {
        describe('valid response', function () {
            beforeEach(function () {
                returnQuery = promise(validResponse({Tags: tags}));
                tagService.getTags();
                rootScope.$apply();
            });

            it('should call ', function () {
                expect(workflowTaskWebService.sendQueryAsync).toHaveBeenCalledWith(jasmine.any(workflowTaskQueries.GetAllTagsForUserQuery));
            });

            it('should publish the event with the tags', function () {
                expect(eventAggregatorService.publish).toHaveBeenCalledWith(tagService.getEvents.TAGSUPDATED, tags);
            });

            describe('calling again', function () {
                var tags;
                beforeEach(function () {
                    tagService.getTags().then(function (tag) {
                        tags = tag;
                    });
                });
                it('should return same object', function () {
                    var innerTags;
                    tagService.getTags().then(function (tag) {
                        innerTags = tag;
                    });
                    rootScope.$apply();
                    expect(innerTags).toBe(tags);
                });
            });
        });

        describe('invalid response', function () {
            beforeEach(function () {
                returnQuery = promise(tags);
                tagService.getTags();
                rootScope.$apply();
            });
            it('should call to get the tags', function () {
                expect(workflowTaskWebService.sendQueryAsync).toHaveBeenCalledWith(jasmine.any(workflowTaskQueries.GetAllTagsForUserQuery));
            });

            it('should publish the event', function () {
                expect(eventAggregatorService.publish).not.toHaveBeenCalled();
            });
        });

    });

    describe('creating a new tag object', function () {
        var tag;
        describe('without any colours set', function () {
            beforeEach(function () {
                tag = tagService.createTagAsObject('caption');
            });
            it('Create a tag with a random style', function () {
                expect(tag).not.toBeNull();
                expect(tag.Style).not.toBeNull();
                expect(tag.Id).toBe(undefined);
                expect(tag.Caption).toBe('caption');
            });
        });
        describe('without with a colour', function () {

            describe('with forecolour', function () {
                beforeEach(function () {
                    tag = tagService.createTagAsObject('caption', "#002154");
                });
                it('default background colour', function () {
                    expect(tag).not.toBeNull();
                    expect(tag.Style).not.toBeNull();
                    expect(tag.Style.color).toBe('#002154');
                    expect(tag.Style['background-color']).toBe('#FFFFFF');
                    expect(tag.Id).toBe(undefined);
                    expect(tag.Caption).toBe('caption');
                });
            });
            describe('with background colour', function () {
                beforeEach(function () {
                    tag = tagService.createTagAsObject('caption', null, "#002154");
                });
                it('default background colour', function () {
                    expect(tag).not.toBeNull();
                    expect(tag.Style).not.toBeNull();
                    expect(tag.Style.color).toBe('#000000');
                    expect(tag.Style['background-color']).toBe('#002154');
                    expect(tag.Id).toBe(undefined);
                    expect(tag.Caption).toBe('caption');
                });
            });

        });


        describe('without a caption', function () {
            beforeEach(function () {
                tag = tagService.createTagAsObject(null, '#123456', '#FEDCBA');
            });
            it('should default the caption to text', function () {
                expect(tag.Caption).toBe('Text');
            });
        });

        describe('with id', function () {
            beforeEach(function () {
                tag = tagService.createTagAsObject('new Tag', '#123456', '#FEDCBA', '57971B5F-4CB9-412C-8B05-9AF4AF8F6B45');
            });
            it('should create tag with all fields set', function () {
                expect(tag.Caption).toBe('new Tag');
                expect(tag.Style.color).toBe('#123456');
                expect(tag.Style['background-color']).toBe('#FEDCBA');
                expect(tag.Id).toBe('57971B5F-4CB9-412C-8B05-9AF4AF8F6B45');
            });
        });

    });

    describe('when updating a tag', function () {
        beforeEach(function () {
            returnCommand = promise();
            tagService.updateTag();
            rootScope.$apply();
        });
        it('should call the webservice to update the tag', function () {
            expect(workflowTaskWebService.sendCommandAsync).toHaveBeenCalledWith(jasmine.any(workflowTaskCommands.UpdateUserTagCommand));
        });
        it('should publish event with new tags', function () {
            expect(eventAggregatorService.publish).toHaveBeenCalledWith(tagService.getEvents.TAGSUPDATED, jasmine.any(Object));
        });
    });

    describe('when adding a new tag', function () {
      describe('if comb comes down correctly', function(){
          beforeEach(function () {
              returnCommand = promise();
              returnQuery = promise(validResponse({TagID: '7D0D6FC7-BB01-4D92-AE56-D093EAF97D4D'}));
              tagService.addTag();
              rootScope.$apply();
          });

          it('should call to get a new guid', function () {
              expect(workflowTaskWebService.sendQueryAsync).toHaveBeenCalledWith(jasmine.any(workflowTaskQueries.GetNewTagIdQuery));
          });

          it('should call webservice to add the new tag', function () {
              expect(workflowTaskWebService.sendCommandAsync).toHaveBeenCalledWith(jasmine.any(workflowTaskCommands.CreateTagForUserCommand));
          });

          it('should send out event for changed tags', function () {
              expect(eventAggregatorService.publish).toHaveBeenCalledWith(tagService.getEvents.TAGSUPDATED, jasmine.any(Object));
          });
      });
        describe('if comb does not come down correctly', function(){
            beforeEach(function () {
                returnCommand = promise();
                returnQuery = promise({TagID: '7D0D6FC7-BB01-4D92-AE56-D093EAF97D4D'});
                tagService.addTag();
                rootScope.$apply();
            });

            it('should call to get a new guid', function () {
                expect(workflowTaskWebService.sendQueryAsync).toHaveBeenCalledWith(jasmine.any(workflowTaskQueries.GetNewTagIdQuery));
            });

            it('should call webservice to add the new tag', function () {
                expect(workflowTaskWebService.sendCommandAsync).not.toHaveBeenCalledWith(jasmine.any(workflowTaskCommands.CreateTagForUserCommand));
            });

            it('should send out event for changed tags', function () {
                expect(eventAggregatorService.publish).not.toHaveBeenCalledWith(tagService.getEvents.TAGSUPDATED, jasmine.any(Object));
            });
        });
    });

    describe('when deleting a tag', function () {
        beforeEach(function () {
            tagService.getFilter(['7D0D6FC7-BB01-4D92-AE56-D093EAF97D4D']);
            returnCommand = promise();
            tagService.deleteTag('7D0D6FC7-BB01-4D92-AE56-D093EAF97D4D');
            rootScope.$apply();
        });

        it('should call webservice to add the new tag', function () {
            expect(workflowTaskWebService.sendCommandAsync).toHaveBeenCalledWith(jasmine.any(workflowTaskCommands.DeleteTagForUserCommand));
        });

        it('should send out event for changed tags', function () {
            expect(eventAggregatorService.publish).toHaveBeenCalledWith(tagService.getEvents.TAGSUPDATED, jasmine.any(Object));
        });

        it('should remove tag from filter if applicable', function () {
            expect(tagService.getFilter()).toEqual([]);
        });
    });

    describe('filtering', function () {
        describe('adding an item to the filter', function () {
            beforeEach(function () {
                tagService.getFilter([]);
                tagService.addTagToFiltering('7D0D6FC7-BB01-4D92-AE56-D093EAF97D4D');
            });
            it('should remove the item from the filter', function () {
                expect(tagService.getFilter()).toEqual(['7D0D6FC7-BB01-4D92-AE56-D093EAF97D4D']);
            });

            it('should publish the event that the filter was changed', function () {
                expect(eventAggregatorService.publish).toHaveBeenCalledWith(tagService.getEvents.FILTERUPDATED, ['7D0D6FC7-BB01-4D92-AE56-D093EAF97D4D']);
            });
        });
        describe('removing an item from the filter', function () {
            describe('if item exists in filter', function () {

                beforeEach(function () {
                    tagService.getFilter(['7D0D6FC7-BB01-4D92-AE56-D093EAF97D4D']);
                    tagService.removeTagFromFiltering('7D0D6FC7-BB01-4D92-AE56-D093EAF97D4D');
                });
                it('should remove the item from the filter', function () {
                    expect(tagService.getFilter()).toEqual([]);
                });

                it('should publish the event that the filter was changed', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalledWith(tagService.getEvents.FILTERUPDATED, []);
                });
            });

            describe('if item does not exist in filter', function () {
                beforeEach(function () {
                    tagService.getFilter(['7D0D6FC7-BB01-4D92-AE56-D093EAF97D4D']);
                    tagService.removeTagFromFiltering('0DB6A330-A0BE-4449-B8AF-AEFA22DDC63C');
                });
                it('should not affect the filter', function () {
                    expect(tagService.getFilter()).toEqual(['7D0D6FC7-BB01-4D92-AE56-D093EAF97D4D']);
                });
                it('should not publish the event that the filter was changed', function () {
                    expect(eventAggregatorService.publish).not.toHaveBeenCalledWith(tagService.getEvents.FILTERUPDATED, []);
                });
            });
        });

        describe('clearing the filter', function () {
            beforeEach(function () {
                tagService.getFilter(['7D0D6FC7-BB01-4D92-AE56-D093EAF97D4D']);
            });
            it('should have data before being cleared', function () {
                expect(tagService.getFilter()).toEqual(['7D0D6FC7-BB01-4D92-AE56-D093EAF97D4D']);
            });
            describe('after filter', function () {
                beforeEach(function () {
                    tagService.clearTagFiltering();
                });
                it('should clear the filter', function () {
                    expect(tagService.getFilter()).toEqual([]);
                });
                it('should send out event that filter has been changed', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalledWith(tagService.getEvents.FILTERUPDATED, []);
                });
            });
        });
    });


    describe('linking tags and items', function () {
        describe('when linking', function () {
            var pushArray = [];
            beforeEach(function () {
                returnCommand = promise();
                tagService.linkTag('10FC6DE3-3FFC-44D9-9523-AB3C73130203', 0, pushArray);
                rootScope.$apply();
            });
            it('should call to link the 2 items together', function () {
                expect(workflowTaskWebService.sendCommandAsync).toHaveBeenCalledWith(jasmine.any(workflowTaskCommands.AddTagToWorkflowInstanceCommand));
            });
            it('should add the tag to the array', function () {
                expect(pushArray).toEqual(['10FC6DE3-3FFC-44D9-9523-AB3C73130203']);
            });

            describe('if tag already exists in array', function () {
                beforeEach(function () {
                    pushArray = ['10FC6DE3-3FFC-44D9-9523-AB3C73130203'];
                    returnCommand = promise();
                    tagService.linkTag('10FC6DE3-3FFC-44D9-9523-AB3C73130203', 0, pushArray);
                    rootScope.$apply();
                });
                it('should not call the webservice', function () {
                    expect(workflowTaskWebService.sendCommandAsync).not.toHaveBeenCalledWith(jasmine.any(workflowTaskCommands.AddTagToWorkflowInstanceCommand));
                });

                it('should not change the array', function () {
                    expect(pushArray).toEqual(['10FC6DE3-3FFC-44D9-9523-AB3C73130203']);
                });
            });
        });

        describe('when unlinking', function () {
            var pushArray = ['10FC6DE3-3FFC-44D9-9523-AB3C73130203'];
            var newPushArray;
            beforeEach(function () {
                returnCommand = promise();
                tagService.unlinkTag('10FC6DE3-3FFC-44D9-9523-AB3C73130203', 0, pushArray).then(function (array) {
                    newPushArray = array;
                });
                rootScope.$apply();
                rootScope.$apply();
            });
            it('should call to remove the tag from the item', function () {
                expect(workflowTaskWebService.sendCommandAsync).toHaveBeenCalledWith(jasmine.any(workflowTaskCommands.DeleteTagFromWorkflowInstanceCommand));
            });
            it('should remove the item from the array', function () {
                expect(newPushArray).toEqual([]);
            });
        });
    });
});
