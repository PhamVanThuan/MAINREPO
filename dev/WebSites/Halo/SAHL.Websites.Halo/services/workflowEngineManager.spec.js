describe('[sahl.js.core.workflowEngineManagement]', function() {
    var workflowManagerDecoration, x2Commands, x2WebService, authenticatedUser;
    var decoratedService;
    angular.module('test.sahl.js.core.workflowEngineManagement', ['sahl.js.core.workflowEngineManagement'], function() {})
        .config(['$workflowManagerDecorationProvider', function($workflowManagerDecorationProvider) {
            workflowManagerDecoration = $workflowManagerDecorationProvider;
        }]);

    beforeEach(module('sahl.js.core.workflowEngineManagement'));
    beforeEach(module('test.sahl.js.core.workflowEngineManagement'));

    beforeEach(inject(function($x2Commands, $x2WebService, $q, $authenticatedUser) {
        var delegate = {};
        x2WebService = $x2WebService;
        x2Commands = $x2Commands;
        authenticatedUser = $authenticatedUser;
        decoratedService = workflowManagerDecoration.decoration[4](delegate, x2WebService, x2Commands, authenticatedUser);
        spyOn(x2WebService, 'sendX2RequestWithReturnedDataAsync').and.callFake(function() {});

        spyOn(x2Commands, 'startActivityRequest').and.callFake(function() {
            this.name = 'startActivityRequest';
        });
        spyOn(x2Commands, 'cancelActivityRequest').and.callFake(function() {
            this.name = 'cancelActivityRequest';
        });
        spyOn(x2Commands, 'completeActivityRequest').and.callFake(function() {
            this.name = 'completeActivityRequest';
        });
    }));


    describe(' - (Provider: $workflowManagerDecoration)-', function() {
        it('should have a decoration method', function() {
            expect(workflowManagerDecoration).not.toBe(undefined);
            expect(workflowManagerDecoration).not.toBe(null);
            expect(workflowManagerDecoration.decoration).not.toBe(undefined);
            expect(workflowManagerDecoration.decoration).not.toBe(null);
        });

        describe('when starting an activity', function() {
            beforeEach(function () {
                decoratedService.startActivity();
            });

            it('should create new start activity command', function() {
                expect(x2Commands.startActivityRequest).toHaveBeenCalled();
            });

            it('should call webservice with command', function() {
                expect(x2WebService.sendX2RequestWithReturnedDataAsync).toHaveBeenCalled();
            });
        });

        describe('when canceling an activity', function() {
            beforeEach(function () {
                decoratedService.cancelActivity();
            });

            it('should create new cancel activity command', function() {
                expect(x2Commands.cancelActivityRequest).toHaveBeenCalled();
            });

            it('should call webservice with command', function() {
                expect(x2WebService.sendX2RequestWithReturnedDataAsync).toHaveBeenCalled();
            });
        });

        describe('when completing an activity', function() {
            beforeEach(function () {
                decoratedService.completeActivity();
            });

            it('should create new complete activity command', function() {
                expect(x2Commands.completeActivityRequest).toHaveBeenCalled();
            });

            it('should call webservice with command', function() {
                expect(x2WebService.sendX2RequestWithReturnedDataAsync).toHaveBeenCalled();
            });
        });
    });
});
