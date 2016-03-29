'use strict';

angular.module('halo.start.portalpages.tasks.mytasks', [
    'sahl.websites.halo.services.tagManagement',
    'sahl.websites.halo.services.workflowManagement',
    'sahl.websites.halo.events.entityManagement',
    'sahl.websites.halo.services.entityManagement',
    'sahl.js.core.eventAggregation',
    'sahl.websites.halo.services.paginationHelpers',
    'sahl.websites.halo.services.acronymManager'])
    .controller('MyTasksCtrl', ['$scope', '$tagService', '$workflowTaskService', '$timeout', '$eventAggregatorService', '$paginationHelperService', '$acronymManagerService', '$filter', '$activityManager', '$entityManagerService',
        function ($scope, $tagService, $workflowTaskService, $timeout, $eventAggregatorService, $paginationHelper, $acronymManagerService, $filter, $activityManager, $entityManager) {
            $scope.spinString = "loadingTasks";
            $scope.currentState = { Name: "", Tasks: [] };
            $scope.taskFilterResult = {};
            $scope.allTags = {}
            var taskPageSize = 15;

            var tagCellTemplate = {
                name: "Tags",
                field: "tags",
                enableSorting: false,
                cellTemplate: '<tag-cell-template></tag-cell-template>'
            };

            $scope.gridOptions = {
                enableRowSelection: true,
                enableRowHeaderSelection: false,
                enablePaginationControls: false,
                paginationPageSize: taskPageSize,
                columnDefs: $scope.workflowTaskColumns,
                enableHorizontalScrollbar: 0,
                enableVerticalScrollbar: 1,
                rowTemplate: '<div  ng-repeat="col in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ui-grid-cell></div>'
            };

            $scope.maxTagsPerUser = $tagService.getMaxTagsPerUser();
            $scope.defaultSearchTags = $tagService.getDefaultSearchTags();
            $scope.isMaxTagsPerUserReached = false;
                

            $scope.gridOptions.multiSelect = false;
            $scope.gridOptions.modifierKeysToMultiSelect = false;
            $scope.gridOptions.noUnselect = true;

            $scope.gridOptions.onRegisterApi = function (gridApi) {
                $scope.gridApi = gridApi;
                gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                    $scope.taskSelected(row.entity);
                });
            };

            $scope.getAssignedTasks = function () {
                $activityManager.startActivityWithKey($scope.spinString);
                $workflowTaskService.getAssignedTasks().then(
                    $scope.intialiseFirstWorkflow, 
                    $scope.stopSpinner);
            };

            $scope.stopSpinner = function () {
                $activityManager.stopActivityWithKey($scope.spinString);
            };

            $scope.intialiseFirstWorkflow = function (workflowTasks) {
                $scope.BusinessProcesses = workflowTasks;

                if ($scope.BusinessProcesses.$values[0]) {
                    $scope.currentWorkflow = $scope.BusinessProcesses.$values[0].WorkFlows.$values[0];
                }

                if ($scope.currentWorkflow) {
                    var firstState = $scope.currentWorkflow.States.$values[0];
                    firstState.isActive = "active";
                    $scope.stateFilterSelected(firstState);
                }

                $activityManager.stopActivityWithKey($scope.spinString);
            };

            $scope.getAssignedTasks();

            var eventsToSubscribeTo = $tagService.getEvents;
            var allTagsUpdate = function (tags) {
                $scope.allTags = tags;
                $scope.isMaxTagsPerUserReached = (Object.keys(tags)).length - 1 == $scope.maxTagsPerUser;
                var newList = _.filter($tagService.getDefaultSearchTags(), function (defaultTag) {
                    for (var tag in tags) {
                        if (tags[tag].Caption && defaultTag.tag == tags[tag].Caption) {
                            return false;
                        } 
                    }
                    return true;
                });

                $scope.defaultSearchTags = newList;
            };

            var filterUpdate = function (filter) {
                $scope.filterTags = filter;
            };

            $eventAggregatorService.subscribe(eventsToSubscribeTo.TAGSUPDATED, allTagsUpdate);
            $eventAggregatorService.subscribe(eventsToSubscribeTo.FILTERUPDATED, filterUpdate);

            $scope.$on('$destroy', function () {
                $eventAggregatorService.unsubscribe(eventsToSubscribeTo.TAGSUPDATED, allTagsUpdate);
                $eventAggregatorService.unsubscribe(eventsToSubscribeTo.FILTERUPDATED, filterUpdate);
            });

            $tagService.getTags().then(function (tags) {
                allTagsUpdate(tags);
            });

            $scope.tabIndex = 0;

            $scope.selectProcess = function (index) {
                $scope.tabIndex = index;
            };

            $scope.shouldShowProcess = function (process) {
                if (!process) {
                    return false;
                }
                var workflows = process.WorkFlows.$values;
                for (var workflow in workflows) {
                    if (workflows[workflow].$shouldShow) {
                        return true;
                    }
                }
                return false;
            };

            $scope.shouldShowWorkflow = function (workflow) {
                var states = workflow.States.$values;
                workflow.$shouldShow = false;
                for (var state in states) {
                    if (states[state].$shouldShow) {
                        workflow.$shouldShow = true;
                        return workflow.$shouldShow;
                    }
                }
                return workflow.$shouldShow;
            };

            $scope.shouldShowState = function (state) {
                if (!state.$children) {
                    state.$children = 0;
                }
                state.$shouldShow = state.$children > 0;
                return state.$shouldShow;
            };

            $scope.taskSelected = function (task) {
                var moduleParameters = task.ProcessName +" - "+ task.WorkflowName;
                var entity = $entityManager.createEntity(task.Subject, task.GenericKeyTypeKey, task.GenericKey, task.InstanceId, 'task', null, moduleParameters);
                $entityManager.addEntity(entity);
                $entityManager.makeEntityActive(entity);
            };

            $scope.buildColumnDefs = function (state) {
                var newColumnDefArray = [];
                for (var i = 0; i < state.ColumnHeaders.$values.length; i++) {
                    var header = state.ColumnHeaders.$values[i];

                    var columnDefItem = {
                        name: header,
                        field: "Row.$values[" + i + "]",
                        width: "*"
                    };
                    if ($acronymManagerService.isAcronym(header)) {
                        var acronymDisplayName = $acronymManagerService.getDisplayName(header);
                        columnDefItem.displayName = acronymDisplayName
                    };
                    newColumnDefArray.push(columnDefItem);
                };
                newColumnDefArray.push(tagCellTemplate);
                return newColumnDefArray;
            };
            var initialiseUserTags = function (tags) {
                for (var tag in tags) {
                    if (tag !== "$type" && _.size(_.where($scope.currentTagsSelected, { Id: tag })) == 0) {
                        tags[tag].activeClass = "";
                    };
                };
            };

            var setGridData = function () {
                var columnDefs = $scope.buildColumnDefs($scope.currentState);
                initialiseUserTags($scope.allTags);
                $scope.gridOptions.columnDefs = columnDefs;

                $scope.gridOptions.data = $filter('filterOnTags')(
                    $scope.currentState.Tasks.$values,
                    $scope.currentTagsSelected,
                    $scope.currentState);

                $scope.gridOptions.data.forEach(function (taskItem) {
                    taskItem.newValues = taskItem.Row.$values;
                    taskItem.ApplicationNo = taskItem.Row.$values[0];
                    taskItem.tags = taskItem.TagIds.$values;
                });
            };

            $scope.currentPage = 1;
            $scope.stateFilterSelected = function (state, $event) {
                for (var i = 0; i < $scope.currentWorkflow.States.$values.length; i++) {
                    var workflowState = $scope.currentWorkflow.States.$values[i];
                    workflowState.isActive = "";
                    if (workflowState.Name === state.Name) {
                        workflowState.isActive = "active";
                    };
                };
                if ($scope.currentState) {
                    $scope.currentState.isActive = "";
                }

                $scope.currentState = state;
                setGridData();

                if ($scope.gridApi) {
                    var currentPage = $scope.gridApi.pagination.getPage();
                    $scope.gridApi.selection.clearSelectedRows();
                }
                else {
                    currentPage = 1;
                };

                $scope.seekPage(1);
            };

            $scope.seekPage = function (pageNum) {
                var numberOfPages = Math.ceil($scope.gridOptions.data.length / taskPageSize);

                if (pageNum < 1 || pageNum > numberOfPages) {
                    return;
                }

                $scope.currentPage = pageNum;
                $scope.taskFilterResult.pages = $paginationHelper.buildPagination(numberOfPages, taskPageSize, $scope.currentPage);

                if ($scope.gridApi) {
                    $scope.gridApi.pagination.seek(pageNum);
                };
            };

            $scope.updateGrid = function (task, removedTag) {
                var tasks;
                var selectedTags = $scope.currentTagsSelected;

                var taskSelectedTags = _.filter(task.tags, function (num) {
                    return _.size(_.filter(selectedTags, function (tag) { return tag.Id === num && tag.Id !== removedTag })) > 0;
                });
                if (_.size(taskSelectedTags) == 0 && _.size(selectedTags) > 0) {
                    tasks = _.filter($scope.gridOptions.data, function (gridTask) { if (gridTask.GenericKey !== task.GenericKey) { return true; } return false; });
                    $scope.gridOptions.data = $filter('filterOnTags')(
                    tasks,
                    $scope.currentTagsSelected,
                    $scope.currentState);
                }
            };

            $scope.filterDataOnCurrentTags = function () {
                $scope.gridOptions.data = $filter('filterOnTags')(
                $scope.currentState.Tasks.$values,
                $scope.currentTagsSelected,
                $scope.currentState);
            };

            $scope.currentTagsSelected = [];
            $scope.tagFilterSelected = function (tag) {
                if (tag.activeClass) {
                    var index = $scope.currentTagsSelected.indexOf(tag);
                    $scope.currentTagsSelected.splice(index, 1);
                    tag.activeClass = null;
                }
                else {
                    $scope.currentTagsSelected.push(tag);
                    tag.activeClass = "active";
                };

                $scope.filterDataOnCurrentTags();
            };

            $scope.allowDropTag = function (ev) {
                ev.preventDefault();
            };

            $scope.dragTag = function (ev) {
                ev.dataTransfer.setData("text", ev.target.id);
            };

            $scope.removeExistingTagFromAllStates = function (states) {
                for (var i = 0; i < states.length; i++) {
                    var state = states[i];

                    $scope.removeExistingTagFromTasks(state, $scope.tagSetForDeletion.Id);
                };
            };

            $scope.removeExistingTagFromTasks = function (state, tagForDeletion) {
                for (var j = 0; j < state.Tasks.$values.length; j++) {
                    var task = state.Tasks.$values[j];
                    var tagIndex = task.TagIds.$values.indexOf(tagForDeletion);
                    if (tagIndex >= 0) {
                        task.TagIds.$values.splice(tagIndex, 1);
                        task.tags = task.TagIds.$values;
                    };
                };
            };

            $scope.updatetagsAfterDeletion = function (tags) {
                $scope.removeExistingTagFromAllStates($scope.currentWorkflow.States.$values);

                var index = -1;
                var tagCount = $scope.currentTagsSelected
                    .filter(function (tag) {
                        if (tag.Id === $scope.tagSetForDeletion.Id) {
                            index = $scope.currentTagsSelected.indexOf(tag);
                            return true;
                        };
                        return false;
                    }).length;

                if (index >= 0) {
                    $scope.currentTagsSelected.splice(index, 1);
                    $scope.filterDataOnCurrentTags();
                };
            };

            $scope.dropTag = function () {
                $tagService.deleteTag($scope.tagSetForDeletion.Id).then($scope.updatetagsAfterDeletion);
            };

            $scope.tagFilterStartedDrag = function (tag) {
                $scope.tagSetForDeletion = tag;
            };
        }]).filter('filterOnTags', function () {
            return function (tasks, filterTags, stateToCountOn) {
                if (!filterTags || filterTags.length === 0) {
                    stateToCountOn.$children = tasks.length;
                    return tasks;
                };

                var flattenedFilterTags = [];
                for (var i = 0; i < filterTags.length; i++) {
                    flattenedFilterTags.push(filterTags[i].Id);
                };

                var returningTasks = tasks.filter(function (task) {
                    return _.intersection(flattenedFilterTags, task.TagIds.$values).length > 0;
                });

                stateToCountOn.$children = returningTasks.length;
                return returningTasks;
            };
        }).filter('filterOnState', function () {
            return function (states, scope) {
                return states.filter(function (state) {
                    return state.Name === scope.currentState.Name;
                });
            };
        }).directive('tagFilterContainer', ['$templateCache', function ($templateCache) {
            return {
                restrict: 'E',
                templateUrl: 'app/start/portalPages/tasks/mytasks/mytasks.tagFilterContainer.tpl.html'
            };
        }]).directive('tagCellTemplate', ['$tagService', function ($tagService) {
            return {
                restrict: 'E',
                link: function (scope, element, attrs) {
                    scope.removeTagFromTask = function (appScope, task, tagId, $event) {
                        $event.stopPropagation();
                        $tagService.unlinkTag(tagId, task.InstanceId, task.TagIds.$values).then(function (newArray) {
                            task.TagIds.$values = newArray;
                            task.tags = newArray;
                            $event.target.parentNode.parentElement.remove();
                            appScope.updateGrid(task, tagId);
                        });
                    };
                },
                templateUrl: 'app/start/portalPages/tasks/mytasks/mytasks.tagCellTemplate.tpl.html'
            };
        }]).directive('addTagButton', ['$tagService', function ($tagService) {
            return {
                restrict: 'E',
                link: function (scope, element, attrs) {
                    scope.newTagEntry = "";

                    scope.addNewUserTag = function (workflowInstance, $event) {
                        var newUserTagName = scope.newTagEntry;
                        var newTagId;
                        if ($event && $event.stopPropagation) {
                            $event.stopPropagation();
                        };
                        if (newUserTagName && newUserTagName.trim() && newUserTagName.trim().length > 0) {
                            var tags = $tagService.getAllUserTags();
                            for (var tag in tags) {
                                if (tags[tag].Caption && newUserTagName.toLocaleLowerCase() == tags[tag].Caption.toLocaleLowerCase()) {
                                    newTagId = tags[tag].Id;
                                }
                            }
                            if (newTagId) {
                                $tagService.linkTag(newTagId, workflowInstance.InstanceId, workflowInstance.tags);
                            } else {
                                $tagService.createNewTag(null, null, 0, newUserTagName).then(function (tagArgs) {
                                    var newTag = tagArgs.newTag;
                                    newTag.Caption = newUserTagName;
                                    $tagService.addTag2(newTag.Caption, newTag.Id, newTag.Style['color'], newTag.Style['background-color']).then(function (tagId) {
                                        $tagService.linkTag(newTag.Id, workflowInstance.InstanceId, workflowInstance.tags).then(function (updatedTags) {
                                        });

                                    });
                                });
                            }
                            scope.newTagEntry = "";
                            

                            if ($event && $event.preventDefault) {
                                $event.preventDefault();
                            };
                            $(element).parent().find('[data-role=dropdown]').hide();
                        }
                    };

                    scope.addExistingUserTag = function (workflowInstance, $event, tag) {
                        var newUserTagName = scope.newTagEntry;
                        $tagService.linkTag(tag.Id, workflowInstance.InstanceId, workflowInstance.tags).then(function (updatedTags) {
                        });
                        $(element).parent().find('[data-role=dropdown]').hide();
                    };

                    scope.newTagDropDownItemSelected = function (event, workflowInstance, tag) {
                        event.stopPropagation();
                        event.preventDefault();
                        scope.newTagEntry = tag.tag;
                        if (tag.Caption) {
                            scope.newTagEntry = tag.Caption;
                            scope.addExistingUserTag(workflowInstance, event, tag);
                        }
                        else {
                            scope.addNewUserTag(workflowInstance, event);
                        }

                        $(element).parent().find('[data-role=dropdown]').hide();
                    };

                    scope.newTagInputSelected = function ($event) {
                        $event.stopPropagation();
                    };
                },
                templateUrl: 'app/start/portalPages/tasks/mytasks/mytasks.addTagButton.tpl.html'
            };
        }]).directive('initMetroHint', ['$timeout', function ($timeout) {
            return {
                restrict: 'A',
                link: function (scope, element, attr) {
                    $timeout(function () {
                        $(element).parent().find('[data-hint]').hint();
                    });
                }
            };
        }]).directive('initMetroDropdown', ['$timeout', function ($timeout) {
            return {
                restrict: 'A',
                link: function (scope, element, attr) {
                    $timeout(function () {
                        $(element).parent().find('[data-role=dropdown]').dropdown();
                        $(element).parent().find('.pull-menu, .menu-pull').each(function () {
                            $(this).pullmenu();
                        });
                    });
                }
            };
        }])
     .directive('droppable', function () {
         return {
             scope: {
                 drop: '&' // parent
             },
             link: function (scope, element) {
                 // again we need the native object
                 var el = element[0];

                 scope.elementBindings = [];
                 scope.directiveElement = el;

                 scope.dragOverEvent = function (e) {
                    var originalEvent = e.originalEvent;
                     var dataTransferType = originalEvent.dataTransfer.types[0];
                     if (dataTransferType === 'tag') {
                         originalEvent.dataTransfer.dropEffect = 'move';
                         // allows us to drop
                         if (originalEvent.preventDefault) {
                             originalEvent.preventDefault();
                         };
                         scope.directiveElement.classList.add('over');
                         scope.directiveElement.classList.add('dragOrange');
                     };
                     return false;
                 };

                 scope.dragEnterEvent = function (e) {
                    e = e.originalEvent;
                     if (e.dataTransfer.getData('tag') === 'tag') {
                         scope.directiveElement.classList.add('over');
                         scope.directiveElement.classList.add('dragOrange');
                     };
                     return false;
                 };

                 scope.dragLeaveEvent = function (e) {
                    e = e.originalEvent;
                     scope.directiveElement.classList.remove('over');
                     scope.directiveElement.classList.remove('dragOrange');
                     return false;
                 };

                 scope.dropEvent = function (e) {
                    e = e.originalEvent;
                     if (e.stopPropagation) {
                         e.stopPropagation();
                     };

                     scope.directiveElement.classList.remove('over');
                     scope.directiveElement.classList.remove('dragOrange');
                     scope.dropTag();

                     return false;
                 };

                 scope.dropTag = function() {
                    scope.$apply("drop()");
                 };

                 scope.bindElement = function(element, event, func) {
                    element.bind(event,func);
                    scope.elementBindings.push(event);
                 };

                 scope.bindElement(element, 'dragover', scope.dragOverEvent);
                 scope.bindElement(element, 'dragenter', scope.dragEnterEvent);
                 scope.bindElement(element, 'dragleave', scope.dragLeaveEvent);
                 scope.bindElement(element, 'drop', scope.dropEvent);
             }
         };
     })
    .directive('draggable', ['$timeout', function ($timeout) {
        return {
            scope: {
                dragItem: '&'
            },
            link: function (scope, element) {
                // this gives us the native JS object
                var el = element[0];                

                scope.directiveElement = el;
                scope.directiveElement.draggable = true;
                scope.elementBindings = [];

                scope.dragStartEvent = function (e) {
                    e.dataTransfer.effectAllowed = 'move';
                    e.dataTransfer.setData('tag', 'tag');
                    scope.directiveElement.classList.add('drag');
                    scope.dragItem();
                    return false;
                };

                scope.dragItem = function() {
                    $timeout(function(){
                        scope.$apply("dragItem()");
                    });                    
                };

                scope.dragEndEvent = function (e) {
                    scope.directiveElement.classList.remove('drag');
                    return false;
                };

                scope.bindElement = function(element, event, func) {
                    element.addEventListener(event, func, false);
                    scope.elementBindings.push(event);
                };

                scope.bindElement(scope.directiveElement, 'dragstart', scope.dragStartEvent);
                scope.bindElement(scope.directiveElement, 'dragend', scope.dragEndEvent);
            }
        };
    }])
    .directive('ngEnter', function() {
        return function(scope, element, attrs) {
            element.bind("keydown keypress", function(event) {
                if(event.which === 13) {
                    scope.$apply(function(){
                        scope.$eval(attrs.ngEnter, {'event': event});
                    });

                    event.preventDefault();
                }
            });
        };
    });
