'use strict';

describe('[sahl.websites.halo.services.workflowManagement]', function () {

    var workflowTaskService, q, workflowTaskWebService,
        workflowTaskCommands,
        workflowTaskQueries,
        returnQuery = {},
        rootScope,
        returnCommand = {},
        timeout,
        fullusername = 'SAHL\\user',
        capabilities = "'Invoice Processor, Invoice Approver'";

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

    beforeEach(module('sahl.websites.halo.services.workflowManagement', function ($provide) {
        workflowTaskWebService = {
            sendQueryAsync: function (query, options) {
                return returnQuery;
            },
            sendCommandAsync: function (command) {
                return returnCommand;
            }
        };
        $provide.value('$workflowTaskWebService', workflowTaskWebService);
        $provide.value('$userManagerService', {
            getAuthenticatedUser: function () {
                return {
                    fullAdName: fullusername,
                    capabilities: capabilities
                };
            }
        });
    }));
    beforeEach(inject(function ($injector) {
        q = $injector.get('$q');
        spyOn(workflowTaskWebService, 'sendQueryAsync').and.callThrough();
        spyOn(workflowTaskWebService, 'sendCommandAsync').and.callThrough();
        workflowTaskCommands = $injector.get('$workflowTaskCommands');
        workflowTaskQueries = $injector.get('$workflowTaskQueries');
        workflowTaskService = $injector.get('$workflowTaskService');
        rootScope = $injector.get('$rootScope');
    }));

    describe('get assigned tasks', function () {
        describe('correct responses', function () {
            beforeEach(function () {
                returnQuery = promise(validResponse({BusinessProcess: {}}));
                workflowTaskService.getAssignedTasks();
                rootScope.$apply();
            });
            it('should get all users assigned tasks', function () {
                expect(workflowTaskWebService.sendQueryAsync).toHaveBeenCalledWith(jasmine.any(workflowTaskQueries.GetAssignedTasksForUserQuery));
            });
            describe('on a second call', function () {
                var workflowItems;
                beforeEach(function () {
                    workflowTaskService.getAssignedTasks().then(function (workflow) {
                        workflowItems = workflow;
                    });
                });
                it('should return the same list', function () {
                    var innerworkflowItems;
                    workflowTaskService.getAssignedTasks().then(function (workflow) {
                        innerworkflowItems = workflow;
                    });
                    returnQuery = promise(validResponse({BusinessProcess: {}}));
                    rootScope.$apply();
                    rootScope.$apply();
                    expect(innerworkflowItems).toBe(workflowItems);
                });
            });
        });

        describe('bad responses', function () {
            beforeEach(function () {
                returnQuery = promise({BusinessProcess: {}});
                workflowTaskService.getAssignedTasks();
                rootScope.$apply();
            });
            it('should get all users assigned tasks', function () {
                expect(workflowTaskWebService.sendQueryAsync).toHaveBeenCalledWith(jasmine.any(workflowTaskQueries.GetAssignedTasksForUserQuery));
            });
            it('should reject', function () {
                var rejected = false;
                workflowTaskService.getAssignedTasks().then(function () { }, function () {
                    rejected = true;
                });
                rootScope.$apply();
                expect(rejected).toBe(true);
            });
        });
    });


})
;
