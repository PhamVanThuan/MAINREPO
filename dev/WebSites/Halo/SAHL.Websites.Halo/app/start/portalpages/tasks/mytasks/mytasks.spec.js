'use strict';
describe('[halo.portalpages.tasks.mytasks]', function () {
    beforeEach(module('halo.start.portalpages.tasks.mytasks'));
    beforeEach(module('sahl.js.ui.notifications'));


    var controller;
    var tagService, workflowTaskService, eventAggregatorService, rootScope, filter, timeout, activityManager, entityManager, toastManagerService;
    var returnPromise = function (containing) {
        return {
            then: function (fn) {
                fn(containing);
            }
        };
    };

    var phantomClick = function (el) {
        var ev = document.createEvent("MouseEvent");
        ev.initMouseEvent(
            "click",
            true /* bubble */ , true /* cancelable */ ,
            window, null,
            0, 0, 0, 0, /* coordinates */
            false, false, false, false, /* modifier keys */
            0 /*left*/ , null
        );
        el.dispatchEvent(ev);
    };

    var copyObject = function (obj) {
        return angular.fromJson(angular.toJson(obj));
    };

    var tags = {
        '6F1E32AF-74AC-49C2-B616-F33C1414A998': {
            Caption: 'test',
            Id: '6F1E32AF-74AC-49C2-B616-F33C1414A998',
            Style: {
                'background-color': '#FFFFFF',
                'color': '#000000'
            }
        },
        '8BB03246-4EA8-4FDC-9875-9C8BA48FB04E': {
            Caption: 'tess',
            Id: '8BB03246-4EA8-4FDC-9875-9C8BA48FB04E',
            Style: {
                'background-color': '#FFFFFF',
                'color': '#000000'
            }
        }
    };
    var processes = {};

    beforeEach(inject(function ($controller, $injector, $rootScope, $q, $filter, $timeout, $activityManager, $entityManagerService) {
        controller = $controller;
        filter = $filter;
        timeout = $timeout;
        tagService = $injector.get('$tagService');
        activityManager = $activityManager;
        entityManager = $entityManagerService;
        spyOn(tagService, 'getTags').and.returnValue(returnPromise(tags));

        workflowTaskService = {
            getAssignedTasks: function () {
                var deferred = $q.defer();
                setTimeout(function () {
                    deferred.resolve(processes);
                }, 100);
                return deferred.promise;

            }
        };
        eventAggregatorService = $injector.get('$eventAggregatorService');
        rootScope = $rootScope;
    }));




    describe(' my tasks controller', function () {
        var scope;

        var createController = function () {
            scope = rootScope.$new();
            return controller('MyTasksCtrl', {
                $scope: scope,
                $tagService: tagService,
                $workflowTaskService: workflowTaskService,
                $eventAggregatorService: eventAggregatorService,
                $timeout: timeout,
                $activityManager: activityManager,
                $entityManager: entityManager
            });
        };

        describe('controller', function () {
            var myTaskController;
            beforeEach(function () {
                myTaskController = createController();
            });

            describe('should exists', function () {
                it('should not be null', function () {

                    expect(myTaskController).not.toBe(null);
                });
                it('tags should be set', function () {
                    expect(scope.allTags).toBe(tags);
                });
            });

            describe('hiding and showing elements', function () {
                describe('process has viewable items', function () {
                    describe('when process is null', function () {
                        it('should return false', function () {
                            expect(scope.shouldShowProcess()).toBe(false);
                        });
                    });

                    describe('when there are processes and workflows', function () {
                        var process;
                        describe('when no workflows are showing', function () {
                            beforeEach(function () {
                                process = {
                                    WorkFlows: {
                                        $values: [{
                                            $shouldShow: false
                                        }, {
                                            $shouldShow: false
                                        }]
                                    }
                                };
                            });
                            it('should return false', function () {
                                expect(scope.shouldShowProcess(process)).toBe(false);
                            });
                        });
                        describe('when at least 1 workflow is showing', function () {
                            beforeEach(function () {
                                process = {
                                    WorkFlows: {
                                        $values: [{
                                            $shouldShow: false
                                        }, {
                                            $shouldShow: true
                                        }]
                                    }
                                };
                            });
                            it('should return true', function () {
                                expect(scope.shouldShowProcess(process)).toBe(true);
                            });

                            it('should initialise the first workflow', function() {
                                var workflowTasks = {
                                    $values: [{
                                        id: 1,
                                        WorkFlows: {
                                            $values: [{
                                                id: 12,
                                                States: {
                                                    $values: [{
                                                        id: 13,
                                                        isActive: 'active',
                                                        ColumnHeaders: {
                                                            $values: [
                                                                'Test'                                                            
                                                            ]
                                                        },
                                                        Tasks: {
                                                            $values: [{
                                                                id: 14,
                                                                Row: {
                                                                    $values: []
                                                                },
                                                                TagIds: {
                                                                    $values: []
                                                                }
                                                            }]
                                                        }
                                                    }]
                                                }
                                            }]
                                        }
                                    }]
                                };

                                scope.intialiseFirstWorkflow(workflowTasks);
                                expect(scope.currentWorkflow.id).toEqual(12);
                            });
                            
                            it('should be able to stop spinner on controller scope', function(){
                                spyOn(activityManager,'stopActivityWithKey').and.callThrough();
                                scope.stopSpinner();
                                expect(activityManager.stopActivityWithKey).toHaveBeenCalledWith(scope.spinString);
                            });
                        });
                    });


                });


                describe('workflow has viewable Items', function () {
                    describe('when there are states', function () {
                        var returned;
                        var workFlow;
                        describe('when no child states are showing', function () {
                            beforeEach(function () {
                                workFlow = {
                                    States: {
                                        $values: [{
                                            $shouldShow: false
                                        }, {
                                            $shouldShow: false
                                        }]
                                    }
                                };
                                returned = scope.shouldShowWorkflow(workFlow);
                            });
                            it('should return false', function () {
                                expect(returned).toBe(false);
                            });
                            it('should set $shouldShow of workflow to false', function () {
                                expect(workFlow.$shouldShow).toBe(false);
                            });
                        });
                        describe('when at least 1 child state is showing', function () {
                            beforeEach(function () {
                                workFlow = {
                                    States: {
                                        $values: [{
                                            $shouldShow: false
                                        }, {
                                            $shouldShow: true
                                        }]
                                    }
                                };
                                returned = scope.shouldShowWorkflow(workFlow);
                            });
                            it('should return true', function () {
                                expect(returned).toBe(true);
                            });
                            it('should set $shouldShow of workflow to true', function () {
                                expect(workFlow.$shouldShow).toBe(true);
                            });
                        });
                    });
                });

                describe('state has viewable tasks', function () {
                    describe('when there are tasks', function () {
                        var returned;
                        var state;
                        describe('when no child tasks are showing', function () {
                            beforeEach(function () {
                                state = {
                                    $children: 0
                                };
                                returned = scope.shouldShowState(state);
                            });
                            it('should return false', function () {
                                expect(returned).toBe(false);
                            });
                            it('should set $shouldShow of state to false', function () {
                                expect(state.$shouldShow).toBe(false);
                            });
                        });
                        describe('when at least 1 child task is showing', function () {
                            beforeEach(function () {
                                state = {
                                    $children: 5
                                };
                                returned = scope.shouldShowState(state);
                            });
                            it('should return true', function () {
                                expect(returned).toBe(true);
                            });
                            it('should set $shouldShow of state to true', function () {
                                expect(state.$shouldShow).toBe(true);
                            });

                            it('should make entity active when task is selected', function() {
                                var entity = {
                                    id: 1
                                };
                                var task = {
                                    ApplicationNo: 2,
                                    InstanceId: 1,
                                    GenericKey: 3,
                                    ProcessName: 'test',
                                    WorkflowName: 'testWorkflow'
                                };
                                
                                spyOn(entityManager, 'createEntity').and.returnValue(entity);
                                spyOn(entityManager,'addEntity');
                                spyOn(entityManager,'makeEntityActive');

                                scope.taskSelected(task);

                                expect(entityManager.createEntity).toHaveBeenCalled();
                                expect(entityManager.addEntity).toHaveBeenCalledWith(entity);
                                expect(entityManager.makeEntityActive).toHaveBeenCalledWith(entity);
                            });
                        });
                    });
                });

            });

            describe('when a tag is selected from adding tag directive', function () {

                var $compile;
                var deferredForCreateNewTag, deferredForAddTag2, deferredForUnlinkTag;
                var fake$event = {
                    stopPropagation: function () {}
                };

                beforeEach(inject(function (_$compile_, $templateCache, $q) {
                    $compile = _$compile_;

                    spyOn(fake$event, 'stopPropagation').and.callThrough();
                    spyOn(tagService, 'createNewTag').and.callFake(function () {
                        deferredForCreateNewTag = $q.defer();
                        return deferredForCreateNewTag.promise;
                    });
                    spyOn(tagService, 'addTag2').and.callFake(function () {
                        deferredForAddTag2 = $q.defer();
                        return deferredForAddTag2.promise;
                    });

                    spyOn(tagService, 'unlinkTag').and.callFake(function(){
                        deferredForUnlinkTag = $q.defer();
                        return deferredForUnlinkTag.promise;
                    });
                    
                    $templateCache.put('app/start/portalPages/tasks/mytasks/mytasks.addTagButton.tpl.html', '<div></div>');
                    $templateCache.put('app/start/portalPages/tasks/mytasks/mytasks.tagCellTemplate.tpl.html','<div></div>');
                }));


                var compileAndDigest = function (target, scope, $httpBackend) {
                    var $element = $compile(target)(scope);
                    scope.$digest();
                    return $element.scope();
                };

                describe('when adding a tag', function () {
                    beforeEach(function () {
                        var element = angular.element('<add-tag-button></add-tag-button>');
                        scope = compileAndDigest(element, rootScope);
                    });

                    it('should add methods to the scope.', function () {
                        expect(scope.addNewUserTag).not.toBe(undefined);
                        expect(scope.addNewUserTag).not.toBe(null);
                    });

                    describe('when creating a new tag', function () {
                        var tagText = 'test';
                        beforeEach(function () {
                            scope.newTagEntry = tagText;
                            scope.addNewUserTag(0, fake$event);
                        });

                        it('should tell the tag service to create new tag', function () {
                            expect(tagService.createNewTag).toHaveBeenCalledWith(null, null, 0, tagText);
                        });

                        it('should stop propagation', function () {
                            expect(fake$event.stopPropagation).toHaveBeenCalled();
                        });

                        describe('when createNewTag function is successful', function () {
                            beforeEach(function () {
                                deferredForCreateNewTag.resolve({
                                    newTag: {
                                        Style: {
                                            color: '#000000'
                                        }
                                    }
                                });
                                rootScope.$digest();
                            });

                            it('should add the new tag', function () {
                                expect(tagService.addTag2).toHaveBeenCalled();
                            });
                        });
                    });
                });

                describe('when wating to add a new tag to a task item', function () {
                    var taskController;
                    var workflowItem;
                    var workflowItemWithoutArray;
                    var focused = false;
                    var activeAdding;
                    var event = {
                        target: {
                            nextElementSibling: {
                                children: [{
                                    focus: function () {
                                        focused = true;
                                    }
                                }]
                            }
                        }
                    };

                    beforeEach(function () {
                        workflowItem = {
                            TagIds: {
                                $values: []
                            }
                        };
                        workflowItemWithoutArray = {
                            TagIds: {}
                        };
                        taskController = createController();
                    });
                });

                describe('selecting different process', function () {
                    beforeEach(function () {
                        scope.selectProcess(1);
                    });
                    it('should update the tabIndex of the scope', function () {
                        expect(scope.tabIndex).toBe(1);
                    });

                });

                describe('when removing an item from a task', function () {
                    var previousTags, returningTags, task, removingTag;
                    var $parent ={grid:function(){}};
                    var mockEvent = {                
                        preventDefault: function() {
                        },
                        stopPropagation: function() {

                        },
                        target: {
                            parentNode: {
                                parentElement: {
                                    remove: function () {

                                    }
                                }
                            }
                        }
                    };

                    beforeEach(function () {
                        var element = angular.element('<tag-cell-template></tag-cell-template');
                        scope = compileAndDigest(element, rootScope);
                        
                        returningTags = ['C0EBEF6C-C522-4039-998D-8CCBC8FA8BAC'];
                        removingTag = '9DD4F0C2-8F31-454E-9256-C15E18A4923D';
                        previousTags = ['C0EBEF6C-C522-4039-998D-8CCBC8FA8BAC'];
                        task = {
                            InstanceId: 15,
                            TagIds: {
                                $values: previousTags
                            }
                        };
                        var appScope = { updateGrid: function (task, tagId) { }};
                        scope.removeTagFromTask(appScope, task, removingTag, mockEvent);
                    });

                     it('should call tagService unlinkTag', function () {
                         expect(tagService.unlinkTag).toHaveBeenCalled();
                     });

                     it('should set the task array to the new Array', function () {
                         expect(task.TagIds.$values).toEqual(returningTags);
                     });

                });

                describe('when filter changes', function () {
                    beforeEach(function () {
                        eventAggregatorService.publish(tagService.getEvents.FILTERUPDATED, ['9DD4F0C2-8F31-454E-9256-C15E18A4923D']);
                    });

                    it('should update the filter in the scope', function () {
                        expect(scope.filterTags).toEqual(['9DD4F0C2-8F31-454E-9256-C15E18A4923D']);
                    });
                });

            });

            describe('on scope destroy', function () {
                beforeEach(function () {
                    spyOn(eventAggregatorService, 'unsubscribe');
                    scope.$destroy();
                });
                it('should unsubscribe from tags updated event', function () {
                    expect(eventAggregatorService.unsubscribe).toHaveBeenCalledWith(tagService.getEvents.TAGSUPDATED, jasmine.any(Function));
                });
                it('should unsubscribe from filter updated event', function () {
                    expect(eventAggregatorService.unsubscribe).toHaveBeenCalledWith(tagService.getEvents.FILTERUPDATED, jasmine.any(Function));
                });
            });

            describe('removal of tag filter', function () {
                var $q,
                    $rootScope;
                beforeEach(inject(function (_$q_, _$rootScope_) {
                    $q = _$q_;
                    $rootScope = _$rootScope_;
                }));

                it('should have a tag marked for deletion when a tag ui element has started being dragged', function () {
                    scope.tagFilterStartedDrag({
                        Id: '1'
                    });

                    expect(scope.tagSetForDeletion.Id).toEqual('1');
                });

                it('should call deleteTag on the $tagService dependancy', function () {
                    scope.tagSetForDeletion = {
                        Id: '1'
                    };
                    spyOn(tagService, 'deleteTag').and.callFake(function () {
                        var deferred = $q.defer();
                        deferred.resolve(scope.tagSetForDeletion);
                        return deferred.promise;
                    });

                    scope.dropTag('1');

                    expect(tagService.deleteTag).toHaveBeenCalledWith('1');
                });

                it('should remove any tags currently associated to a workflow task displayed on the grid', function () {
                    scope.currentWorkflow = {
                        States: {
                            $values: [
                                {
                                    Tasks: {
                                        $values: [
                                            {
                                                TagIds: {
                                                    $values: [
                                                '1'
                                            ]
                                                }
                                    }
                                    ]
                                    }
                            }
                            ]
                        }
                    };
                    scope.tagSetForDeletion = {
                        Id: '1'
                    };

                    spyOn(tagService, 'deleteTag').and.callFake(function () {
                        var deferred = $q.defer();
                        deferred.resolve(scope.tagSetForDeletion);
                        return deferred.promise;
                    });

                    spyOn(scope, 'updatetagsAfterDeletion').and.callThrough();
                    spyOn(scope, 'removeExistingTagFromTasks').and.callThrough();
                    spyOn(scope, 'removeExistingTagFromAllStates').and.callThrough();

                    scope.dropTag();

                    $rootScope.$digest();

                    expect(tagService.deleteTag).toHaveBeenCalledWith('1');
                    expect(scope.updatetagsAfterDeletion).toHaveBeenCalled();
                    expect(scope.updatetagsAfterDeletion).toHaveBeenCalledWith(scope.tagSetForDeletion);
                    expect(scope.removeExistingTagFromTasks).toHaveBeenCalled();
                    expect(scope.removeExistingTagFromAllStates).toHaveBeenCalled();
                    expect(scope.currentWorkflow.States.$values[0]).toBeDefined();
                    expect(scope.currentWorkflow.States.$values[0].Tasks.$values[0]).toBeDefined();
                    expect(scope.currentWorkflow.States.$values[0].Tasks.$values[0].TagIds.$values[0]).not.toBeDefined();
                });

                
            });

            describe(' - tag-filter-container - ',function() {

                beforeEach(function() {
                    scope.currentState = {
                        Tasks: {
                            $values: [
                                {
                                    TagIds: {
                                        $values: [
                                            '1'
                                        ]
                                    }
                                }
                            ]
                        }
                    };
                });

                it('should add tag to list of currently selected tags when tag is not active', function() {
                    scope.tagFilterSelected({Id: 3});
                    expect(scope.currentTagsSelected.length).toEqual(1);
                });

                it('should deselect tag in currently selected tag list when tag is set to active', function() {
                    scope.tagFilterSelected({id: 3});
                    expect(scope.currentTagsSelected.length).toEqual(1);
                    scope.tagFilterSelected({id: 3, activeClass: 'active'});
                    expect(scope.currentTagsSelected.length).toEqual(0);
                });

            });

            
        });

        describe(' - Adding Default Tag Item - ', function () {
            it('should select a random color, but select the minimum available position to prioritize better colours', function () {

            });
        });



    });

    describe('FilterOnTags', function () {

        var tasks = [{
            TagIds: {
                $values: ['1', '2', '3']
            }
        }];
        var filterTags = [{
            Id: '1'
        }];
        var _filterName_ = 'filterOnTags';


        it('should exist in MyTaskCtrl', function () {
            expect(filter(_filterName_)).not.toBe(undefined);
        });

        it('should return at least one common tag from within the workflow task', function () {
            var filteredTasks = filter(_filterName_)(tasks, filterTags, {});
            expect(filteredTasks.length).toEqual(1);
        });
    });

    describe('FilterOnState', function () {
        var $rootScope;
        beforeEach(inject(function (_$rootScope_) {
            // The injector unwraps the underscores (_) from around the parameter names when matching
            $rootScope = _$rootScope_;
            $rootScope.currentState = "";
        }));

        it('should exist in MyTaskCtrl', function () {
            expect(filter('filterOnState')).toBeDefined();
        });

        var workflowStates = {};
        workflowStates.States = [];
        workflowStates.States.push({
            Name: "Application Query"
        });
        workflowStates.States.push({
            Name: "Decline"
        });
        workflowStates.States.push({
            Name: "Disputes"
        });
        workflowStates.States.push({
            Name: "Further Info Request"
        });

        var tempState = {
            Name: "Decline"
        };


        it('should have a currentState to filter on', function () {
            expect($rootScope.currentState).toBeDefined();
        });

        it('should have a filter that returns the tasks associated with the selected state', function () {
            $rootScope.currentState = tempState;
            var stateFilteredList = filter('filterOnState')(workflowStates.States, $rootScope);
            expect(stateFilteredList.length).toEqual(1);
            expect(stateFilteredList[0].Name).toEqual($rootScope.currentState.Name);
        });

    });

    describe(' - directives - ',function() {
        var $compile,
            $rootScope,
            $httpBackend,
            directiveScope,
            fakedResponse,
            fakedAddTagButtonResponse,
            element;

        var metroFn = {
                hint: jasmine.createSpy(),
                dropdown: jasmine.createSpy(),
                each: jasmine.createSpy()
            };

        beforeEach(inject(function (_$compile_, _$rootScope_, _$httpBackend_, $q) {
            // The injector unwraps the underscores (_) from around the parameter names when matching
            $compile = _$compile_;
            $rootScope = _$rootScope_;
            $httpBackend = _$httpBackend_;
        }));

        describe(' - tagFilterContainer - ', function () {            
            var fakedResponse = '<div>test</div><div>tess</div>';           

            it('should replace tag-filter-container directive with the appropriate content', function () {
                $httpBackend.whenGET('app/start/portalPages/tasks/mytasks/mytasks.tagFilterContainer.tpl.html').respond(fakedResponse);
                // Compile a piece of HTML containing the directive
                var element = $compile("<tag-filter-container></tag-filter-container>")($rootScope);

                $rootScope.$digest();
                $httpBackend.flush();
                expect(element.html()).toContain('tess');
                expect(element.html()).toContain('test');
            });
        });

        describe(' - addTagButton - ', function () {
            

            beforeEach(inject(function (_$compile_, _$rootScope_, _$httpBackend_, $q) {            
                spyOn(tagService,'createNewTag').and.callThrough();
                spyOn(tagService,'addTag2').and.callThrough();
                spyOn(tagService,'linkTag').and.callThrough();
            }));

            var fakedResponse = '<ul><li><a>Important</a></li><li><div class="tag-entry"><input type="text"></div></li>';
            beforeEach(function() {
                $httpBackend.whenGET('app/start/portalPages/tasks/mytasks/mytasks.addTagButton.tpl.html').respond(fakedResponse);

                element = $compile("<add-tag-button></add-tag-button>")($rootScope);
                $rootScope.$digest();
                $httpBackend.flush();

                directiveScope = element.scope();
                spyOn(directiveScope,'addExistingUserTag').and.callThrough();
                spyOn(directiveScope,'addNewUserTag').and.callThrough();
            });

            var mockEvent = {                
                preventDefault: function() {
                },
                stopPropagation: function() {

                }
            };      
            var mockWorkflowInstance =   {
                InstanceId: 1,
                tags: []
            };

            var mockUserTag = {
                Id: 1,
                Caption: "userTag"
            };

            var mockAutoTag = {
                Id: 2
            };

            
            it('should replace add-tag-button directive with the appropriate content', function () {            
                expect(element.html()).toContain('Important');
                expect(element.html()).toContain('tag-entry');
            });

            it('should be able to add a new user tag', function () {
                directiveScope.newTagEntry = "new tag";
                directiveScope.addNewUserTag({},mockEvent);
                expect(tagService.createNewTag).toHaveBeenCalled();
            });

            it('should be able to add an existing tag', function() {
                directiveScope.addExistingUserTag(mockWorkflowInstance,
                    mockEvent,
                    mockUserTag
                );
                expect(tagService.linkTag).toHaveBeenCalled();
            });

            it('should be able to add tag when it has been selected from the tag dropdown menu and the tag is captioned by the user', function() {            
                directiveScope.newTagDropDownItemSelected(
                    mockEvent,
                    mockWorkflowInstance,
                    mockUserTag
                );
                expect(directiveScope.newTagEntry).toEqual("userTag");
                expect(directiveScope.addExistingUserTag).toHaveBeenCalled();
            });

            it('should be able to add tag when it has been selected from the tag dropdown menu and the tag is captioned by the user', function() {
                
                directiveScope.newTagDropDownItemSelected(
                    mockEvent,
                    mockWorkflowInstance,
                    mockAutoTag
                );
                expect(directiveScope.addNewUserTag).toHaveBeenCalled();
            });

            it('should ensure that events arent propagated up when a new tag input box is selected', function() {
                spyOn(mockEvent,'stopPropagation');
                directiveScope.newTagInputSelected(mockEvent);
                expect(mockEvent.stopPropagation).toHaveBeenCalled();
            });
        });

        describe(' - tagCellTemplate - ', function () {        
            var mockTask = {
                id: 11, 
                TagIds: {
                    $values: [
                        '1'
                    ]
                }
            };
            var scope = { updateGrid: function (task, tagId) { } };
            var mockEvent = {                
                preventDefault: function() {
                },
                stopPropagation: function() {

                },
                target: {
                    parentNode: {
                        parentElement: {
                            remove: function () {

                            }
                        }
                    }
                }
            };   

            beforeEach(inject(function (_$compile_, _$rootScope_, _$httpBackend_, $q) {
                
                spyOn(tagService,'unlinkTag').and.callFake(function() {
                    var deferred = $q.defer();
                    deferred.resolve(mockTask,'1');
                    return deferred.promise;
                });
            }));
             
            beforeEach(function() {
                fakedResponse = '<div class="gridTag icon-tag dropdown-toggle" style="color: rgb(46, 204, 113);"></div><add-tag-button></add-tag-button>';
                fakedAddTagButtonResponse = '<ul><li><a>Important</a></li><li><div class="tag-entry"><input type="text"></div></li>';
                
                $httpBackend.whenGET('app/start/portalPages/tasks/mytasks/mytasks.tagCellTemplate.tpl.html').respond(fakedResponse);
                $httpBackend.whenGET('app/start/portalPages/tasks/mytasks/mytasks.addTagButton.tpl.html').respond(fakedAddTagButtonResponse);

                element = $compile("<tag-cell-template></tag-cell-template>")($rootScope);

                $rootScope.$digest();
                $httpBackend.flush();
                directiveScope = element.scope();
            });

            
            it('should replace add-tag-button directive with the appropriate content', function () {
                expect(element.html()).toContain('gridTag');
                expect(element.html()).toContain('dropdown-toggle');
                expect(element.html()).toContain('add-tag-button');
                expect(element.html()).toContain('Important');
                expect(element.html()).toContain('tag-entry');
            });

            it('should be able to unlink a tag from an existing task', function() {            
                directiveScope.removeTagFromTask(scope, mockTask , '1', mockEvent);
                expect(tagService.unlinkTag).toHaveBeenCalled();
            });
        });
        
        describe(' - initMetroHint - ', function() {
            var mockNotification;
            

            beforeEach(inject(function (_$compile_, _$rootScope_, _$httpBackend_, $q) {
                spyOn($.fn, "parent").and.callFake(function() {
                    return {
                        find: function(attr) {
                            return metroFn;
                        }
                    };
                });
            }));

            it('should initialise the metro ui hint plugin when the directive is applied', function() {
                expect(metroFn.hint).not.toHaveBeenCalled();
                element = $compile('<div data-hint><div init-metro-hint></div></div>')($rootScope);
                
                $rootScope.$digest();
                timeout.flush();

                expect(metroFn.hint).toHaveBeenCalled();
            });
        });

        describe(' - initMetroDropdown - ',function() {
            beforeEach(inject(function (_$compile_, _$rootScope_, _$httpBackend_, $q) {     
                spyOn($.fn, "parent").and.callFake(function() {
                    return {
                        find: function(attr) {
                            return metroFn;
                        }
                    };
                });
            }));

            it('should initialise the metro ui dropdown plugin when the directive is applied', function() {
                expect(metroFn.dropdown).not.toHaveBeenCalled();
                element = $compile('<div data-hint><div init-metro-dropdown></div></div>')($rootScope);

                $rootScope.$digest();
                timeout.flush();

                expect(metroFn.dropdown).toHaveBeenCalled();
            });
        });

        describe(' - droppable - ', function() {
            var isolatedDirectiveScope;
            var e = $.Event('');
            e.dataTransfer = {
                types: ['tag'],
                dropEffect: 'move',
                getData: function(attr){
                    return 'tag';
                }
            };
            e.preventDefault = jasmine.createSpy();                

            var eventWrapper = {
                originalEvent: e
            };

            beforeEach(inject(function (_$compile_, _$rootScope_, _$httpBackend_, $q) {
                $compile = _$compile_;
                element = $compile('<div droppable drop="dropTag()"></div>')($rootScope);
                
                $rootScope.$digest();
                directiveScope = element.scope();
                isolatedDirectiveScope = element.isolateScope();

                spyOn(isolatedDirectiveScope,'dragOverEvent').and.callThrough();;
                spyOn(isolatedDirectiveScope,'dropTag').and.callThrough();
            }));

            it('should initialise listeners for the required events to satisfy the droppable components', function() {
                expect(isolatedDirectiveScope.elementBindings).toContain('dragover');
                expect(isolatedDirectiveScope.elementBindings).toContain('dragleave');
                expect(isolatedDirectiveScope.elementBindings).toContain('drop');
                expect(isolatedDirectiveScope.elementBindings).toContain('dragenter');
            });

            it('should transform dom element of event when dragover is signalled', function() {
                expect(isolatedDirectiveScope.dragOverEvent).toBeDefined();

                var hasPropagatedEvent = isolatedDirectiveScope.dragOverEvent(eventWrapper);
                expect(hasPropagatedEvent).toBeFalsy();

                expect(eventWrapper.originalEvent.preventDefault).toHaveBeenCalled();
                expect(isolatedDirectiveScope.directiveElement.classList).toContain('over');
                expect(isolatedDirectiveScope.directiveElement.classList).toContain('dragOrange');
            });

            it('should transform dom element of an event when dragenter is sinalled',function() {
                expect(isolatedDirectiveScope.dragEnterEvent).toBeDefined();

                var hasPropagatedEvent = isolatedDirectiveScope.dragEnterEvent(eventWrapper);
                expect(hasPropagatedEvent).toBeFalsy();

                expect(isolatedDirectiveScope.directiveElement.classList).toContain('over');
                expect(isolatedDirectiveScope.directiveElement.classList).toContain('dragOrange');
            });

            it('should transform dom element of an event when dragleave is signalled',function() {
                expect(isolatedDirectiveScope.dragLeaveEvent).toBeDefined();

                var hasPropagatedEvent = isolatedDirectiveScope.dragLeaveEvent(eventWrapper);
                expect(hasPropagatedEvent).toBeFalsy();

                expect(isolatedDirectiveScope.directiveElement.classList).not.toContain('over');
                expect(isolatedDirectiveScope.directiveElement.classList).not.toContain('dragOrange');
            });

            it('should transform dom element of an event when drop is signalled', function() {
                expect(isolatedDirectiveScope.dropEvent).toBeDefined();

                var hasPropagatedEvent = isolatedDirectiveScope.dropEvent(eventWrapper);
                expect(hasPropagatedEvent).toBeFalsy();

                expect(isolatedDirectiveScope.dropTag).toHaveBeenCalled();
                expect(isolatedDirectiveScope.directiveElement.classList).not.toContain('over');
                expect(isolatedDirectiveScope.directiveElement.classList).not.toContain('dragOrange');
            });
        });

        describe(' - draggable - ', function() {
            var isolatedDirectiveScope;

            var e = $.Event('');
            e.dataTransfer = {
                types: ['tag'],
                dropEffect: 'move',
                getData: function(attr){
                    return 'tag';
                },
                effectAllowed: true,
                setData: jasmine.createSpy()
            };
            e.preventDefault = jasmine.createSpy();                

            beforeEach(inject(function (_$compile_, _$rootScope_, _$httpBackend_, $q) {
                element = $compile('<button draggable dragItem="dragItem()"></button>')($rootScope);

                $rootScope.$digest();
                directiveScope = element.scope();
                isolatedDirectiveScope = element.isolateScope();

                spyOn(isolatedDirectiveScope,'dragItem').and.callThrough();
            }));

            it('should initialise listeners for the required events to satisfy the draggable components', function() {
                expect(isolatedDirectiveScope.elementBindings).toContain('dragstart');
                expect(isolatedDirectiveScope.elementBindings).toContain('dragend');
            });

            it('should transform dom element of an event when dragstart is signalled', function() {
                expect(isolatedDirectiveScope.dragStartEvent).toBeDefined();

                var hasPropagatedEvent = isolatedDirectiveScope.dragStartEvent(e);
                expect(hasPropagatedEvent).toBeFalsy();

                expect(e.dataTransfer.setData).toHaveBeenCalled();
                expect(isolatedDirectiveScope.directiveElement.classList).toContain('drag');                
                expect(isolatedDirectiveScope.dragItem).toHaveBeenCalled();                
            });

            it('should trasnform dom element of an event when dragend is signalled',function() {
                expect(isolatedDirectiveScope.dragEndEvent).toBeDefined();

                var hasPropagatedEvent = isolatedDirectiveScope.dragEndEvent(e);
                expect(hasPropagatedEvent).toBeFalsy();

                expect(isolatedDirectiveScope.directiveElement.classList).not.toContain('drag');
            });


        });
    });

});