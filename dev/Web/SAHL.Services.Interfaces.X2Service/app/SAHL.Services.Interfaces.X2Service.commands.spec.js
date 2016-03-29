'use strict';

describe('[SAHL.Services.Interfaces.X2Service.commands]', function() {

    var x2Commands;
    var correlationID = '9ff3f11a-9864-470b-a8ba-3853064dd22e',
        instanceId = '1234',
        userName = 'SAHL\\HaloUser',
        activityName = 'activityName',
        ignoreWarnings = false,
        mapVariables = {
            test: "variables"
        },
        data = {
            data: "here"
        };

    var metaData = {
        $type: 'SAHL.Core.Services.ServiceRequestMetadata,SAHL.Core',
        username: 'SAHL\\HaloUser',
        currentuserrole: 'RoleName'
    };

    beforeEach(module('SAHL.Services.Interfaces.X2Service.commands'));
    beforeEach(inject(function($x2Commands) {
        x2Commands = $x2Commands;
    }));


    describe('when starting', function() {
        var commandUnderTest;
        beforeEach(function() {
            commandUnderTest = new x2Commands.startActivityRequest(correlationID, instanceId, metaData, activityName, ignoreWarnings, mapVariables, data);
        });
        it('to have correct action set', function() {
            expect(commandUnderTest.action).toEqual(2);
        });

        it('other information should be set correctly', function() {
            expect(commandUnderTest.correlationID).toEqual(correlationID);
            expect(commandUnderTest.instanceId).toEqual(instanceId);
            expect(commandUnderTest.serviceRequestMetadata).toEqual(metaData);
            expect(commandUnderTest.activityName).toEqual(activityName);
            expect(commandUnderTest.ignoreWarnings).toEqual(ignoreWarnings);
            expect(commandUnderTest.mapVariables).toEqual(mapVariables);
            expect(commandUnderTest.data).toEqual(data);
        });

    });
    describe('when cancelling', function() {
        var commandUnderTest;
        beforeEach(function() {
            commandUnderTest = new x2Commands.cancelActivityRequest(correlationID, instanceId, metaData, activityName, ignoreWarnings, mapVariables, data);
        });
        it('to have correct action set', function() {
            expect(commandUnderTest.action).toEqual(8);
        });
        it('other information should be set correctly', function() {
            expect(commandUnderTest.correlationID).toEqual(correlationID);
            expect(commandUnderTest.instanceId).toEqual(instanceId);
            expect(commandUnderTest.serviceRequestMetadata).toEqual(metaData);
            expect(commandUnderTest.activityName).toEqual(activityName);
            expect(commandUnderTest.ignoreWarnings).toEqual(ignoreWarnings);
            expect(commandUnderTest.mapVariables).toEqual(mapVariables);
            expect(commandUnderTest.data).toEqual(data);
        });
    });
    describe('when completing', function() {
        var commandUnderTest;
        beforeEach(function() {
            commandUnderTest = new x2Commands.completeActivityRequest(correlationID, instanceId, metaData, activityName, ignoreWarnings, mapVariables, data);
        });
        it('to have correct action set', function() {
            expect(commandUnderTest.action).toEqual(4);
        });
        it('other information should be set correctly', function() {
            expect(commandUnderTest.correlationID).toEqual(correlationID);
            expect(commandUnderTest.instanceId).toEqual(instanceId);
            expect(commandUnderTest.serviceRequestMetadata).toEqual(metaData);
            expect(commandUnderTest.activityName).toEqual(activityName);
            expect(commandUnderTest.ignoreWarnings).toEqual(ignoreWarnings);
            expect(commandUnderTest.mapVariables).toEqual(mapVariables);
            expect(commandUnderTest.data).toEqual(data);
        });
    });
});
